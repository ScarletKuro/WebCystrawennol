using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using WebCystrawennol.ScrapEngine;

namespace WebCystrawennol
{
    internal static class Program
    {
        private static async Task ScrapSitesAsync(IEnumerable<BaseScrap> site, CancellationToken token)
        {
            using (var apartment = new MessageLoopApartment())
            {
                // create WebBrowser inside MessageLoopApartment
                var webBrowser = apartment.Invoke(() => new WebBrowser());
                try
                {
                    foreach (var getEngine in site)
                    {

                        foreach (var url in getEngine.Urls)
                        {
                            Console.WriteLine("URL:" + url);
                            // cancel in 30s or when the main token is signalled
                            var navigationCts = CancellationTokenSource.CreateLinkedTokenSource(token);
                            navigationCts.CancelAfter((int)TimeSpan.FromSeconds(30).TotalMilliseconds);
                            var navigationToken = navigationCts.Token;
                            // run the navigation task inside MessageLoopApartment
                            string gerUrl = url;
                            string html = await apartment.Run(() =>
                                webBrowser.NavigateAsync(gerUrl, navigationToken), navigationToken);
                            getEngine.GetItems(html);
                            //EMT(html);   
                        }
                    }
                }
                finally
                {
                    // dispose of WebBrowser inside MessageLoopApartment
                    apartment.Invoke(webBrowser.Dispose);
                }
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            //EMT();
            try
            {
                //WebBrowserExt.SetFeatureBrowserEmulation(); // enable HTML5
                var cts = new CancellationTokenSource((int)TimeSpan.FromMinutes(3).TotalMilliseconds);
                var createInstance = new List<BaseScrap> { new Tele2(), new Emt() };
                var task = ScrapSitesAsync(createInstance, cts.Token);
                task.Wait(cts.Token);
            }
            catch (Exception ex)
            {
                while (ex is AggregateException && ex.InnerException != null)
                    ex = ex.InnerException;
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// WebBrowserExt - WebBrowser extensions
    /// by Noseratio - http://stackoverflow.com/a/22262976/1768303
    /// </summary>
    public static class WebBrowserExt
    {
        private const int PollDelay = 500;
        // navigate and download
        public static async Task<string> NavigateAsync(this WebBrowser webBrowser, string url, CancellationToken token)
        {
            // navigate and await DocumentCompleted
            var tcs = new TaskCompletionSource<bool>();
            WebBrowserDocumentCompletedEventHandler handler = delegate { tcs.TrySetResult(true); };
            using (token.Register(
                () =>
                {
                    webBrowser.Stop();
                    tcs.TrySetCanceled();
                }, true))
            {
                webBrowser.DocumentCompleted += handler;
                try
                {
                    webBrowser.Navigate(url);
                    await tcs.Task; // wait for DocumentCompleted
                }
                finally
                {
                    webBrowser.DocumentCompleted -= handler;
                }
            }
            // get the root element
            if (webBrowser.Document != null)
            {
                var documentElement = webBrowser.Document.GetElementsByTagName("html")[0];
                webBrowser.ScriptErrorsSuppressed = true;
                var html = documentElement.OuterHtml;
                while (true)
                {
                    // wait asynchronously, this will throw if cancellation requested
                    await Task.Delay(PollDelay, token);
                    // continue polling if the WebBrowser is still busy
                    if (webBrowser.IsBusy)
                        continue;
                    var htmlNow = documentElement.OuterHtml;
                    if (html == htmlNow)
                        break; // no changes detected, end the poll loop
                    html = htmlNow;
                }
                // consider the page fully rendered
                token.ThrowIfCancellationRequested();
                return html;
            }
            return string.Empty;
        }

        // enable HTML5 (assuming we're running IE10+)
        // more info: http://stackoverflow.com/a/18333982/1768303
        public static void SetFeatureBrowserEmulation()
        {
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Runtime)
                return;
            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Registry.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                appName, 10000, RegistryValueKind.DWord);
        }
    }

    /// <summary>
    /// MessageLoopApartment
    /// STA thread with message pump for serial execution of tasks
    /// by Noseratio - http://stackoverflow.com/a/22262976/1768303
    /// </summary>
    public class MessageLoopApartment : IDisposable
    {
        private Thread _thread; // the STA thread
        private TaskScheduler _taskScheduler; // the STA thread's task scheduler

        public TaskScheduler TaskScheduler
        {
            get { return _taskScheduler; }
        }

        /// <summary>MessageLoopApartment constructor</summary>
        public MessageLoopApartment()
        {
            var tcs = new TaskCompletionSource<TaskScheduler>();
            // start an STA thread and gets a task scheduler
            _thread = new Thread(startArg =>
            {
                EventHandler idleHandler = null;
                idleHandler = (s, e) =>
                {
                    // handle Application.Idle just once
                    Application.Idle -= idleHandler;
                    // return the task scheduler
                    tcs.SetResult(TaskScheduler.FromCurrentSynchronizationContext());
                };
                // handle Application.Idle just once
                // to make sure we're inside the message loop
                // and SynchronizationContext has been correctly installed
                Application.Idle += idleHandler;
                Application.Run();
            });
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.IsBackground = true;
            _thread.Start();
            _taskScheduler = tcs.Task.Result;
        }

        /// <summary>shutdown the STA thread</summary>
        public void Dispose()
        {
            if (_taskScheduler != null)
            {
                var taskScheduler = _taskScheduler;
                _taskScheduler = null;
                // execute Application.ExitThread() on the STA thread
                Task.Factory.StartNew(
                    Application.ExitThread,
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    taskScheduler).Wait();
                _thread.Join();
                _thread = null;
            }
        }

        /// <summary>Task.Factory.StartNew wrappers</summary>
        public void Invoke(Action action)
        {
            Task.Factory.StartNew(action,
                CancellationToken.None, TaskCreationOptions.None, _taskScheduler).Wait();
        }

        public TResult Invoke<TResult>(Func<TResult> action)
        {
            return Task.Factory.StartNew(action,
                CancellationToken.None, TaskCreationOptions.None, _taskScheduler).Result;
        }

        public Task Run(Action action, CancellationToken token)
        {
            return Task.Factory.StartNew(action, token, TaskCreationOptions.None, _taskScheduler);
        }

        public Task<TResult> Run<TResult>(Func<TResult> action, CancellationToken token)
        {
            return Task.Factory.StartNew(action, token, TaskCreationOptions.None, _taskScheduler);
        }

        public Task Run(Func<Task> action, CancellationToken token)
        {
            return Task.Factory.StartNew(action, token, TaskCreationOptions.None, _taskScheduler).Unwrap();
        }

        public Task<TResult> Run<TResult>(Func<Task<TResult>> action, CancellationToken token)
        {
            return Task.Factory.StartNew(action, token, TaskCreationOptions.None, _taskScheduler).Unwrap();
        }
    }
}