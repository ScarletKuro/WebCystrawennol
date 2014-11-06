using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using WebCystrawennol.Model;

namespace WebCystrawennol.ScrapEngine
{
    /// <summary>
    /// Парсит сайт www.emt.ee
    /// </summary>
    public sealed class Emt : BaseScrap
    {
        public Emt()
        {
            Urls = new List<string>();
            //Телефоны
            Urls.Add("http://www.emt.ee/sisu?p_p_id=eshop_WAR_eshopportletnew&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_eshop_WAR_eshopportletnew_action=productList.changeFilter&categoryId=410&menuItemId=1005&pagenationInfo.pageViewOptions=2&recommendedProductGroup=false&pagenationInfo.pageSize=80&sortingOption=&pagenationInfo.selectedPage=0&pagenationInfo.startPage=0&tabs1=productList.showProductsList&timestamp=1415288714119&showMainPart=false&showExpandedPart=false&filterSearchPattern=");
        }
        public override void GetItems(string url)
        {
            var webpage = new HtmlDocument();
            webpage.LoadHtml(url);
            var document = webpage;
            var page = document.DocumentNode;
            string name = string.Empty;
            foreach (var item in page.QuerySelectorAll(".products-list tbody tr"))
            {
                /*
                 * Структура сайта:
                 * Бренд:(например Samsung)
                 * Итем(Например Galaxy Note) поэтому нужно делать       
                 * итем                         Бренд+итем
                 * ----                         и часть с Mobilecat модно сделать лучше
                 * Бренд
                 * Итем
                 * итем
                 * ...
                 */
                var mobilecat = item.QuerySelector("th");
                if (mobilecat != null)
                {
                    name = mobilecat.InnerHtml.Trim();
                }

                var product = item.QuerySelector(".product-view");
                if (product != null)
                {
                    var image = product.QuerySelector("div img").Attributes["src"].Value;
                    var title = product.QuerySelector("div img").Attributes["alt"].Value;
                    Console.WriteLine("Ссылка на картинку:{0}", image);
                    Console.WriteLine("Название:{0} {1}", name, title);
                    var clientPrice =
                        item.QuerySelectorAll(".price").ElementAt(0).InnerText.Trim().Split(Convert.ToChar((" ")))[0];
                    var standartPrice =
                        item.QuerySelectorAll(".price").ElementAt(1).InnerText.Trim().Split(Convert.ToChar((" ")))[0];


                    Console.WriteLine("Обычная цена:{0}", standartPrice);
                    Console.WriteLine("Цена для клиента:{0}", clientPrice);
                    Console.WriteLine("--------------------");
                }
            }
        }

        public override List<string> Urls { get; set; }
        public override List<SaveToJson.Device> Stuffs { get; set; }
    }
}
