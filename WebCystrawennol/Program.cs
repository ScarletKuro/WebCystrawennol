using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Newtonsoft.Json;
using WebCystrawennol.Model;
using WebCystrawennol.ScrapEngine;

namespace WebCystrawennol
{
    internal static class Program
    {
        public static void Save(List<SaveToJson.Device> device)
        {
            var jsonDevice = JsonConvert.SerializeObject(device);
            System.IO.File.WriteAllText(@"C:\Users\Shinigami\Fizzler\tele2.txt", jsonDevice);
        }
        private static void StartScraping()
        {
            var u = new List<SaveToJson.Device>();
            var swatch = new System.Diagnostics.Stopwatch();
            swatch.Start();
            //var root = new Model.SaveToJson.RootObject()
            //{
            //    ShopName = "Tele2",
            //    Items = Stuffs
            //};
            //var jsonDevice = JsonConvert.SerializeObject(root);
            //System.IO.File.WriteAllText(@"C:\Users\Shinigami\Fizzler\tele2.txt", jsonDevice);

            var siteList = new List<BaseScrap>
            {
                new Emt(),
                new Tele2(),
                new Elisa()
            };
            var query =
                siteList.ToObservable().SelectMany((site => site.StartParse(site.Settings)),
                    (baseScrap, content) => new { baseScrap, content });

            query.Subscribe(result =>
            {
                u.AddRange(result.content.DeviceContainer);
                result.baseScrap.PrintStatus("Completed", new Uri(result.content.Url).Host, ConsoleColor.Green);
                Console.WriteLine("Total items:{0} on thread {1}", result.content.DeviceContainer.Count(),
                Thread.CurrentThread.ManagedThreadId);
            }, () =>
            {
                Console.WriteLine("Finally");
                swatch.Stop();
                //Save(u);
                Console.WriteLine(swatch.Elapsed); 
            });
        }

        [STAThread]
        private static void Main(string[] args)
        {
            StartScraping();
            Console.ReadLine();
        }


    }
}