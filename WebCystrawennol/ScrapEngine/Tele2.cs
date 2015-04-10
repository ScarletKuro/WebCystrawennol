using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using WebCystrawennol.Model;
using WebCystrawennol.Utility;

namespace WebCystrawennol.ScrapEngine
{
    /// <summary>
    /// Парсит сайт www.tele2.ee
    /// </summary>
    public sealed class Tele2 : BaseScrap
    {
        public Tele2()
        {
            //Телефоны
            //Settings.Add("https://pood.tele2.ee/et/products/46");
            //Планшеты
            //Settings.Add("https://pood.tele2.ee/et/products/809");
            Settings.Add(new ParseSettings("https://pood.tele2.ee/et/products/46"));
            Settings.Add(new ParseSettings("https://pood.tele2.ee/et/products/809"));
        }

        private Dictionary<string, string> Extract(string url)
        {
            var extractedvalues = new Dictionary<string, string>();
            using (var client = new SmartWebClient())
            {
                var webpage = new HtmlDocument();
                webpage.LoadHtml(client.DownloadString(url));
                var document = webpage;
                var page = document.DocumentNode;
                var image = page.QuerySelector(".preview img").Attributes["src"].Value;
                var normalprice = page.QuerySelector(".price:last p .sum").InnerHtml.GetNumsFromStr();
                extractedvalues.Add("normalprice", normalprice);
                extractedvalues.Add("image", image);

            }
            return extractedvalues;
        }

        protected override IEnumerable<SaveToJson.Device> GetItems(string sitecontent)
        {
            //Поправить цену теле2
            var stuffs = new List<SaveToJson.Device>();
            var webpage = new HtmlDocument();
            webpage.LoadHtml(sitecontent);
            var document = webpage;
            var page = document.DocumentNode;

            //AsParallel запускает парсинг каждого элемента .boxItem в параллельных потоках, установлен лимит на 5 потоков
            page.QuerySelectorAll(".boxItem").AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered).WithDegreeOfParallelism(5).ForAll(item =>
            {
                var priceclient = item.QuerySelector(".price").InnerHtml.Trim().ScrubHtml().GetNumsFromStr();
                var product = item.QuerySelector(".productTitle a");
                var productname = product.Attributes["title"].Value;
                var producturl = item.QuerySelector("a").Attributes["href"].Value.Split(Convert.ToChar(";")).ElementAt(0).Substring(5);
                var extra = Extract(string.Format("https://pood.tele2.ee{0}", producturl));
                stuffs.Add(new SaveToJson.Device()
                {
                    ShopName = "Tele2",
                    Name = productname,
                    ImageUrl = extra["image"],
                    ProductPrice = new List<SaveToJson.DevicePrice>()
                     {
                         new SaveToJson.DevicePrice()
                         {
                             Price = priceclient,
                             Type = SaveToJson.DevicePrice.PriceType.Client
                         },
                         new SaveToJson.DevicePrice()
                         {
                             Price = extra["normalprice"],
                             Type = SaveToJson.DevicePrice.PriceType.Normal
                         }
                     }

                });
            });
            return stuffs;
        }
    }
}
