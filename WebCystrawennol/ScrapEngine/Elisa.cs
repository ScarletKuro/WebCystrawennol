using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using WebCystrawennol.Model;

namespace WebCystrawennol.ScrapEngine
{
    /// <summary>
    /// Парсит сайт www.elisa.ee
    /// </summary>
    public sealed class Elisa : BaseScrap
    {
        public Elisa()
        {
            Urls = new List<string>();
            //Телефоны
            Urls.Add("https://www.elisa.ee/ru/Eraklient/Pood");
            //Планштеы
            Urls.Add("https://www.elisa.ee/ru/Eraklient/Pood/a/tahvelarvutid");
        }
        public override void GetItems(string url)
        {
            var webpage = new HtmlDocument();
            webpage.LoadHtml(url);
            var document = webpage;
            var page = document.DocumentNode;
            foreach (var item in page.QuerySelectorAll(".content .tabcontent01 .col:select-parent:not(.special)"))
            {
                var mobilename = item.QuerySelector(".name a").InnerText.Trim();
                var price = item.QuerySelector(".regularprice:only-child");
                var image = item.QuerySelector(".img img").Attributes["src"].Value;
                if (price != null)
                {
                    Console.WriteLine("Картинка: {0}", image);
                    Console.WriteLine("Название: {0}", mobilename);
                    Console.WriteLine("Цена: {0}", price.QuerySelector("span").Attributes["rel"].Value);   
                }
                Console.WriteLine("-----------------");
            }
        }

        public override List<string> Urls { get; set; }
        public override List<SaveToJson.Device> Stuffs { get; set; }
    }
}
