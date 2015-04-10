using System;
using System.Collections.Generic;
using System.Net;
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
            //Телефоны
            //Settings.Add("https://www.elisa.ee/ru/Eraklient/Pood");
            //Планштеы
            //Settings.Add("https://www.elisa.ee/ru/Eraklient/Pood/a/tahvelarvutid");
            const string postData = "{\"categoryName\":\"mobile\",\"consumerSegment\":true,\"page\":0,\"pageSize\":10,\"selectedVendors\":[],\"parameterValueSelectionIds\":[],\"parameterValueRangeSearchGroupIds\":[],\"selectedColors\":[],\"selectedPriceRanges\":[],\"sortOrder\":{\"sortType\":\"private_focus\",\"reversed\":false}}";
            var headers = new WebHeaderCollection {{"Content-Type", "application/json;charset=utf-8"}};
            Settings.Add(new ParseSettings("https://pood.elisa.ee/publicrest/manager/search", ParseSettings.Methods.POST, postData, headers));
        }

        protected override IEnumerable<SaveToJson.Device> GetItems(string sitecontent)
        {
            var stuffs = new List<SaveToJson.Device>();
            Console.WriteLine(sitecontent);
            //var webpage = new HtmlDocument();
            //webpage.LoadHtml(sitecontent);
            //var document = webpage;
            //var page = document.DocumentNode;
            //foreach (var item in page.QuerySelectorAll(".content .tabcontent01 .col:select-parent:not(.special)"))
            //{
            //    var mobilename = item.QuerySelector(".name a").InnerText.Trim();
            //    var price = item.QuerySelector(".regularprice:only-child");
            //    var image = item.QuerySelector(".img img").Attributes["src"].Value;
            //    if (price != null)
            //    {
            //        stuffs.Add(new SaveToJson.Device()
            //        {
            //            ShopName = "Elisa",
            //            Name = mobilename,
            //            ImageUrl = image,
            //            ProductPrice = new List<SaveToJson.DevicePrice>()
            //         {
            //             new SaveToJson.DevicePrice()
            //             {
            //                 Price = price.QuerySelector("span").Attributes["rel"].Value,
            //                 Type = SaveToJson.DevicePrice.PriceType.Normal
            //             }
            //         }

            //        });
            //        //Console.WriteLine("Картинка: {0}", image);
            //        //Console.WriteLine("Название: {0}", mobilename);
            //        //Console.WriteLine("Цена: {0}", price.QuerySelector("span").Attributes["rel"].Value);
            //        //Console.WriteLine("-----------------");
            //    }
            //}
            return stuffs;
        }
    }
}
