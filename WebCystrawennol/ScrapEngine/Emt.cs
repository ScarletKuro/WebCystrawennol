﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using WebCystrawennol.Model;
using WebCystrawennol.Utility;

namespace WebCystrawennol.ScrapEngine
{
    /// <summary>
    /// Парсит сайт www.emt.ee
    /// </summary>
    public sealed class Emt : BaseScrap
    {
        public Emt()
        {
            //Телефоны
            Settings.Add(new ParseSettings("https://www.emt.ee/sisu?p_p_id=eshop_WAR_eshopportletnew&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_eshop_WAR_eshopportletnew_action=productList.changeFilter&categoryId=410&menuItemId=1005&recommendedProductGroup=false&pagenationInfo.pageSize=80&sortingOption=&pagenationInfo.selectedPage=0&pagenationInfo.startPage=0&tabs1=productList.showProductsList&timestamp=1428703614923&showMainPart=false&showExpandedPart=false&filterSearchPattern="));
            //Settings.Add("http://www.emt.ee/sisu?p_p_id=eshop_WAR_eshopportletnew&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_eshop_WAR_eshopportletnew_action=productList.changeFilter&categoryId=410&menuItemId=1005&pagenationInfo.pageViewOptions=2&recommendedProductGroup=false&pagenationInfo.pageSize=80&sortingOption=&pagenationInfo.selectedPage=0&pagenationInfo.startPage=0&tabs1=productList.showProductsList&timestamp=1415288714119&showMainPart=false&showExpandedPart=false&filterSearchPattern=");
            //Планшеты
            //Settings.Add("https://www.emt.ee/sisu?p_p_id=eshop_WAR_eshopportletnew&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_eshop_WAR_eshopportletnew_action=productList.changeFilter&categoryId=868&menuItemId=42&pagenationInfo.pageViewOptions=2&recommendedProductGroup=false&pagenationInfo.pageSize=80&sortingOption=&pagenationInfo.selectedPage=0&pagenationInfo.startPage=0&tabs1=productList.showProductsList&timestamp=1415314465516&showMainPart=false&showExpandedPart=false&filterSearchPattern=");
            Settings.Add(new ParseSettings("https://www.emt.ee/sisu?p_p_id=eshop_WAR_eshopportletnew&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_eshop_WAR_eshopportletnew_action=productList.changeFilter&categoryId=868&menuItemId=42&pagenationInfo.pageViewOptions=2&recommendedProductGroup=false&pagenationInfo.pageSize=80&sortingOption=&pagenationInfo.selectedPage=0&pagenationInfo.startPage=0&tabs1=productList.showProductsList&timestamp=1415314465516&showMainPart=false&showExpandedPart=false&filterSearchPattern="));
        }

        protected override IEnumerable<SaveToJson.Device> GetItems(string sitecontent)
        {
            var stuffs = new List<SaveToJson.Device>();
            var webpage = new HtmlDocument();
            webpage.LoadHtml(sitecontent);
            var document = webpage;
            var page = document.DocumentNode;


            foreach (var item in page.QuerySelectorAll(".product-view"))
            {
                var image = string.Format("https://www.emt.ee{0}", item.QuerySelector("img").Attributes["src"].Value);
                var title = item.QuerySelector("img").Attributes["alt"].Value;
                var clientprice = string.Empty;
                if (item.QuerySelector(".sum") != null)
                {
                    clientprice = item.QuerySelector(".sum").InnerText.GetNumsFromStr();
                }
                
                var normalprice = item.QuerySelectorAll("p strong").ElementAt(1).InnerText.Split(Convert.ToChar(" "))[1];
                if (string.IsNullOrEmpty(clientprice))
                {
                    stuffs.Add(new SaveToJson.Device()
                    {
                        ShopName = "EMT",
                        Name = title,
                        ImageUrl = image,
                        ProductPrice = new List<SaveToJson.DevicePrice>()
                        
                     {
                         new SaveToJson.DevicePrice()
                         {
                             Price = normalprice,
                             Type = SaveToJson.DevicePrice.PriceType.Normal
                         }

                     }

                    });
                }
                else
                {
                    stuffs.Add(new SaveToJson.Device()
                    {
                        ShopName = "EMT",
                        Name = title,
                        ImageUrl = image,
                        ProductPrice = new List<SaveToJson.DevicePrice>()
                        
                     {
                         new SaveToJson.DevicePrice()
                         {
                             Price = clientprice,
                             Type = SaveToJson.DevicePrice.PriceType.Client
                         }
                         , 
                         
                         new SaveToJson.DevicePrice()
                         {
                             Price = normalprice,
                             Type = SaveToJson.DevicePrice.PriceType.Normal
                         }
                     }

                    });
                }
            }

            //foreach (var item in page.QuerySelectorAll(".products-list tbody tr"))
            //{
            //    /*
            //     * Структура сайта:
            //     * Бренд:(например Samsung)
            //     * Итем(Например Galaxy Note) поэтому нужно делать       
            //     * итем                         Бренд+итем
            //     * ----                         и часть с Mobilecat модно сделать лучше
            //     * Бренд
            //     * Итем
            //     * итем
            //     * ...
            //     */
            //    var mobilecat = item.QuerySelector("th");
            //    if (mobilecat != null)
            //    {
            //        name = mobilecat.InnerHtml.Trim();
            //    }

            //    var product = item.QuerySelector(".product-view");
            //    if (product != null)
            //    {
            //        var image = product.QuerySelector("div img").Attributes["src"].Value;
            //        var title = product.QuerySelector("div img").Attributes["alt"].Value;
            //        //Console.WriteLine("Ссылка на картинку:{0}", image);
            //        //Console.WriteLine("Название:{0} {1}", name, title);
            //        var clientPrice =
            //            item.QuerySelectorAll(".price").ElementAt(0).InnerText.Trim().Split(Convert.ToChar((" ")))[0];
            //        var standartPrice =
            //            item.QuerySelectorAll(".price").ElementAt(1).InnerText.Trim().Split(Convert.ToChar((" ")))[0];


            //        //Console.WriteLine("Обычная цена:{0}", standartPrice);
            //        //Console.WriteLine("Цена для клиента:{0}", clientPrice);
            //        //Console.WriteLine("--------------------");

            //        stuffs.Add(new SaveToJson.Device()
            //        {
            //            ShopName = "EMT",
            //            Name = string.Format("{0} {1}", name,title),
            //            ImageUrl = image,
            //            ProductPrice = new List<SaveToJson.DevicePrice>()
            //         {
            //             new SaveToJson.DevicePrice()
            //             {
            //                 Price = clientPrice,
            //                 Type = SaveToJson.DevicePrice.PriceType.Client
            //             }
            //             , 
            //             new SaveToJson.DevicePrice()
            //             {
            //                 Price = standartPrice,
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
