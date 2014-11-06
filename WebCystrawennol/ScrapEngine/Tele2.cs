using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using WebCystrawennol.Utility;

namespace WebCystrawennol.ScrapEngine
{
    public sealed class Tele2 : BaseScrap
    {
        public override List<string> Urls { get; set; }

        public Tele2() 
        {
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
                var price = Helper.ScrubHtml(item.QuerySelector(".price").InnerHtml.Trim());
                Console.WriteLine(price);
                var product = item.QuerySelector(".productTitle a");
                var image = item.QuerySelector("a img").Attributes["src"].Value;
                var title = product.Attributes["title"].Value;
                Console.WriteLine(image);
                Console.WriteLine(title);
                Console.WriteLine("--------------------");
            }
        }
    }
}
