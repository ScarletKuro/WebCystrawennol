using System;
using System.Collections.Generic;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
using WebCystrawennol.Model;
using WebCystrawennol.Utility;

namespace WebCystrawennol.ScrapEngine
{
    /// <summary>
    /// Парсит сайт www.tele2.ee
    /// </summary>
    public sealed class Tele2 : BaseScrap
    {
        public override List<string> Urls { get; set; }
        public override List<SaveToJson.Device> Stuffs { get; set; }

        public Tele2() 
        {
            Stuffs = new List<SaveToJson.Device>();
            Urls = new List<string>();
            //Телефоны
            Urls.Add("https://pood.tele2.ee/et/products/46");
            //Планшеты
            Urls.Add("https://pood.tele2.ee/et/products/809");
        }
        public override void GetItems(string url)
        {
            var webpage = new HtmlDocument();
            webpage.LoadHtml(url);
            var document = webpage;
            var page = document.DocumentNode;
            foreach (var item in page.QuerySelectorAll(".boxItem"))
            {
                var price = Helper.ScrubHtml(item.QuerySelector(".price").InnerHtml.Trim()).GeNumsFromStr();
                var product = item.QuerySelector(".productTitle a");
                var image = item.QuerySelector("a img").Attributes["src"].Value;
                var productname = product.Attributes["title"].Value;
                Stuffs.Add(new SaveToJson.Device()
                {
                     Name = productname,
                     ImageUrl = image,
                     ProductPrice = new List<SaveToJson.DevicePrice>()
                     {
                         new SaveToJson.DevicePrice()
                         {
                             Price = price,
                             Type = SaveToJson.DevicePrice.PriceType.Client
                         }
                     }
                     
                });
                //Console.WriteLine(image);
                //Console.WriteLine(productname);
                //Console.WriteLine(price);
                //Console.WriteLine("--------------------");
            }
            var root = new Model.SaveToJson.RootObject()
            {
                ShopName = "Tele2",
                Items = Stuffs
            };
            var jsonDevice = JsonConvert.SerializeObject(root);
            System.IO.File.WriteAllText(@"C:\Users\Shinigami\Fizzler\tele2.txt", jsonDevice);
        }
    }
}
