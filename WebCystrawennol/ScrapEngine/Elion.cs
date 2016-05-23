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
    /// Парсит сайт www.elion.ee
    /// </summary>
    public sealed class Elion : BaseScrap
    {
        public Elion()
        {
            //Телефоны
            Settings.Add(new ParseSettings("https://pood.elion.ee/nutitelefonid"));
           
            //Планшеты
           
            //Settings.Add(new ParseSettings("https://www.emt.ee/sisu?p_p_id=eshop_WAR_eshopportletnew&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_eshop_WAR_eshopportletnew_action=productList.changeFilter&categoryId=868&menuItemId=42&pagenationInfo.pageViewOptions=2&recommendedProductGroup=false&pagenationInfo.pageSize=80&sortingOption=&pagenationInfo.selectedPage=0&pagenationInfo.startPage=0&tabs1=productList.showProductsList&timestamp=1415314465516&showMainPart=false&showExpandedPart=false&filterSearchPattern="));
        }

        protected override IEnumerable<SaveToJson.Device> GetItems(string sitecontent)
        {
            var stuffs = new List<SaveToJson.Device>();
            var webpage = new HtmlDocument();
            webpage.LoadHtml(sitecontent);
            var document = webpage;
            var page = document.DocumentNode;


            //foreach (var item in page.QuerySelectorAll(".product-view"))
            //{
            //    var image = string.Format("https://www.emt.ee{0}", item.QuerySelector("img").Attributes["src"].Value);
            //    var title = item.QuerySelector("img").Attributes["alt"].Value;
            //    var clientprice = string.Empty;
            //    if (item.QuerySelector(".sum") != null)
            //    {
            //        clientprice = item.QuerySelector(".sum").InnerText.GetNumsFromStr();
            //    }
                
            //    var normalprice = item.QuerySelectorAll("p strong").ElementAt(1).InnerText.Split(Convert.ToChar(" "))[1];
            //    if (string.IsNullOrEmpty(clientprice))
            //    {
            //        stuffs.Add(new SaveToJson.Device()
            //        {
            //            ShopName = "EMT",
            //            Name = title,
            //            ImageUrl = image,
            //            ProductPrice = new List<SaveToJson.DevicePrice>()
                        
            //         {
            //             new SaveToJson.DevicePrice()
            //             {
            //                 Price = normalprice,
            //                 Type = SaveToJson.DevicePrice.PriceType.Normal
            //             }

            //         }

            //        });
            //    }
            //    else
            //    {
            //        stuffs.Add(new SaveToJson.Device()
            //        {
            //            ShopName = "EMT",
            //            Name = title,
            //            ImageUrl = image,
            //            ProductPrice = new List<SaveToJson.DevicePrice>()
                        
            //         {
            //             new SaveToJson.DevicePrice()
            //             {
            //                 Price = clientprice,
            //                 Type = SaveToJson.DevicePrice.PriceType.Client
            //             }
            //             , 
                         
            //             new SaveToJson.DevicePrice()
            //             {
            //                 Price = normalprice,
            //                 Type = SaveToJson.DevicePrice.PriceType.Normal
            //             }
            //         }

            //        });
            //    }
            //}
            return stuffs;
        }
    }
}
