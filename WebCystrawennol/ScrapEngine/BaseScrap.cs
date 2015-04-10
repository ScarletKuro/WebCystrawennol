using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using WebCystrawennol.Model;

namespace WebCystrawennol.ScrapEngine
{
    /// <summary>
    /// Абстрактный класс, для создания уникальных парсеров под конкретный сайт
    /// </summary>
    public abstract class BaseScrap
    {
        protected BaseScrap()
        {
            Settings = new List<ParseSettings>();
        }

        /// <summary>
        /// Абсткартный метод для парсера
        /// </summary>
        /// <param name="sitecontent">Ссылка на сайт, который нужно спарсить</param>
        protected abstract IEnumerable<SaveToJson.Device> GetItems(string sitecontent);

        /// <summary>
        /// Контейнер для хранения ссылок под каждый сайт
        /// </summary>
        public List<ParseSettings> Settings { get; private set; }

        /// <summary>
        /// Асинхрона получает содержимое страницы
        /// </summary>
        /// <returns>Итемы на странице</returns>
        private IObservable<IEnumerable<SaveToJson.Device>> GetContent(ParseSettings settings)
        {

            if (settings.Method == ParseSettings.Methods.GET)
            {
                return Observable.Create<IEnumerable<SaveToJson.Device>>(o =>
                {
                    var result = new ReplaySubject<IEnumerable<SaveToJson.Device>>();
                    var inner = Observable.Using(() => new SmartWebClient(), wc =>
                    {
                        var obs = Observable
                         .FromEventPattern<
                             DownloadStringCompletedEventHandler,
                             DownloadStringCompletedEventArgs>(
                                 h => wc.DownloadStringCompleted += h,
                                 h => wc.DownloadStringCompleted -= h)
                         .ObserveOn(Scheduler.CurrentThread)
                         .Where(e => !e.EventArgs.Cancelled);

                        var uri = new Uri(settings.UrlSite);
                        PrintStatus("Start parsing", uri.Host, ConsoleColor.Yellow);
                        wc.DownloadStringAsync(uri);
                        return obs;
                    }).Subscribe(ep =>
                    {

                        if (ep.EventArgs.Cancelled)
                        {
                            result.OnCompleted();
                        }
                        else
                        {
                            if (ep.EventArgs.Error != null)
                            {
                                PrintStatus(string.Format("Error {0} ", ep.EventArgs.Error), settings.UrlSite, ConsoleColor.Red);
                                result.OnCompleted();
                            }
                            else
                            {
                                result.OnNext(GetItems(ep.EventArgs.Result));
                                result.OnCompleted();
                            }
                        }
                    }, result.OnError);
                    return new CompositeDisposable(inner, result.Subscribe(o));
                });
            }
            else
            {
                return Observable.Create<IEnumerable<SaveToJson.Device>>(o =>
                {
                    var result = new ReplaySubject<IEnumerable<SaveToJson.Device>>();
                    var inner = Observable.Using(() => new SmartWebClient(), wc =>
                    {
                        var obs = Observable
                         .FromEventPattern<
                             UploadDataCompletedEventHandler,
                             UploadDataCompletedEventArgs>(
                                 h => wc.UploadDataCompleted += h,
                                 h => wc.UploadDataCompleted -= h)
                         .ObserveOn(Scheduler.CurrentThread)
                         .Where(e => !e.EventArgs.Cancelled);

                        var uri = new Uri(settings.UrlSite);
                        PrintStatus("Start parsing", uri.Host, ConsoleColor.Yellow);
                        byte[] postArray = Encoding.UTF8.GetBytes(settings.Data);
                        wc.Headers.Add(settings.Heades);
                        wc.UploadDataAsync(uri, postArray);
                        return obs;
                    }).Subscribe(ep =>
                    {

                        if (ep.EventArgs.Cancelled)
                        {
                            result.OnCompleted();
                        }
                        else
                        {
                            if (ep.EventArgs.Error != null)
                            {
                                PrintStatus(string.Format("Error {0} ", ep.EventArgs.Error), settings.UrlSite, ConsoleColor.Red);
                                result.OnCompleted();
                            }
                            else
                            {
                                result.OnNext(GetItems(Encoding.UTF8.GetString(ep.EventArgs.Result)));
                                result.OnCompleted();
                            }
                        }
                    }, result.OnError);
                    return new CompositeDisposable(inner, result.Subscribe(o));
                });
            }



            
        }
        public IObservable<EngineContainer> StartParse(IEnumerable<ParseSettings> settings)
        {
            var query =
                settings.ToObservable()
                .SelectMany(GetContent,
                    (options, content) => new EngineContainer {Url= options.UrlSite, DeviceContainer = content});
            return query;
        }

        private readonly object _latch = new object();

        public void PrintStatus(string message, string url, ConsoleColor color = ConsoleColor.Gray)
        {
            lock (_latch)
            {
                Console.ForegroundColor = color;
                Console.WriteLine("{0}: {1} on thread {2}", message, url, Thread.CurrentThread.ManagedThreadId);
                Console.ResetColor();
            }
        }
    }

    public struct EngineContainer
    {
        public string Url { get; set; }
        public IEnumerable<SaveToJson.Device> DeviceContainer { get; set; }
    }
}