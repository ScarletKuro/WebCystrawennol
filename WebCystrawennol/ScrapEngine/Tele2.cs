using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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

            Settings.Add(new ParseSettings("https://tele2.ee/pood"));
            Settings.Add(new ParseSettings("https://tele2.ee/pood?isformbb=196"));
        }

        private Dictionary<string, string> Extract(string url)
        {
            var extractedvalues = new Dictionary<string, string>();
            using (var client = new SmartWebClient())
            {
                var webpage = new HtmlDocument();
                webpage.LoadHtml(client.DownloadString(url));
                var document = webpage;
                var o = double.Parse("199.0000", CultureInfo.InvariantCulture);
                var page = document.DocumentNode;
                var javacointainer = page.QuerySelector("script:contains('sidebarCart')");
                var normalprice = double.Parse(GePrice(javacointainer.InnerText, "fullPrice = (\\S+);"),
                    CultureInfo.InvariantCulture);
                var clientprice = GePrice(javacointainer.InnerText, "\"first_payment\":\"<span>(\\S+)");
                extractedvalues.Add("clientprice", clientprice);
                extractedvalues.Add("normalprice", normalprice.ToString(CultureInfo.InvariantCulture));
            }
            return extractedvalues;
        }

        private string GePrice(string text, string reg)
        {
            var fullprice = Regex.Match(text, reg);
            return fullprice.Success ? fullprice.Groups[1].Value : string.Empty;
        }

        protected override IEnumerable<SaveToJson.Device> GetItems(string sitecontent)
        {
            //Поправить цену теле2
            var stuffs = new List<SaveToJson.Device>();
            var webpage = new HtmlDocument();
            webpage.LoadHtml(sitecontent);
            var document = webpage;
            var page = document.DocumentNode;

            page.QuerySelectorAll(".item")
                .AsParallel()
                .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                .WithDegreeOfParallelism(5)
                .ForAll(item =>
                {
                    var image = item.QuerySelector(".product-image-cnt img").Attributes["src"].Value;
                    var title = item.QuerySelector("a").Attributes["title"].Value;
                    var url = item.QuerySelector("a").Attributes["href"].Value;
                    var vendor = item.QuerySelector(".product-manufactor-cnt .product-manufactor").InnerText.Trim();
                    var extra = Extract(url);
                    stuffs.Add(new SaveToJson.Device()
                    {
                        ShopName = "Tele2",
                        Name = title,
                        Vendor = vendor,
                        ImageUrl = image,
                        Url = url,
                        ProductPrice = new List<SaveToJson.DevicePrice>
                        {
                            new SaveToJson.DevicePrice
                            {
                                Price = extra["clientprice"],
                                Type = SaveToJson.DevicePrice.PriceType.Client
                            },
                            new SaveToJson.DevicePrice
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