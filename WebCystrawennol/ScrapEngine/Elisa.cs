using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Policy;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
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
            //Смартфоны
            const string postDataMobile = "{\"categoryName\":\"mobile\",\"consumerSegment\":true,\"page\":0,\"pageSize\":10,\"selectedVendors\":[],\"parameterValueSelectionIds\":[],\"parameterValueRangeSearchGroupIds\":[],\"selectedColors\":[],\"selectedPriceRanges\":[],\"sortOrder\":{\"sortType\":\"private_focus\",\"reversed\":false}}";
            var headers = new WebHeaderCollection {{"Content-Type", "application/json;charset=utf-8"}};
            Settings.Add(new ParseSettings("https://pood.elisa.ee/publicrest/manager/search", ParseSettings.Methods.POST, postDataMobile, headers));

            const string postDataTable = "{\"categoryName\":\"tablet\",\"consumerSegment\":true,\"page\":0,\"pageSize\":10,\"selectedVendors\":[],\"parameterValueSelectionIds\":[],\"parameterValueRangeSearchGroupIds\":[],\"selectedColors\":[],\"selectedPriceRanges\":[],\"sortOrder\":{\"sortType\":\"private_focus\",\"reversed\":false}}";
            Settings.Add(new ParseSettings("https://pood.elisa.ee/publicrest/manager/search", ParseSettings.Methods.POST, postDataTable, headers));
        }

        protected override IEnumerable<SaveToJson.Device> GetItems(string sitecontent)
        {
            var stuffs = new List<SaveToJson.Device>();
            var json = JsonConvert.DeserializeObject<ElisaJson.RootElisa>(sitecontent);
            foreach (var device in json.ElisaDevices)
            {
                stuffs.Add(new SaveToJson.Device()
                {
                    ShopName = "Elisa",
                    Name = device.model,
                    ImageUrl = string.Format("https://pilt.elisa.ee/{0}", device.coverPhotoFilename),
                    ProductPrice = new List<SaveToJson.DevicePrice>()
                    {
                        new SaveToJson.DevicePrice()
                        {
                            Price = device.price.ToString(CultureInfo.InvariantCulture),
                            Type = SaveToJson.DevicePrice.PriceType.Normal
                        },
                        new SaveToJson.DevicePrice()
                        {
                            Price = device.clientPrice.ToString(CultureInfo.InvariantCulture),
                            Type = SaveToJson.DevicePrice.PriceType.Client
                        },
                        new SaveToJson.DevicePrice()
                        {
                            Price = device.customerPrice.ToString(CultureInfo.InvariantCulture),
                            Type = SaveToJson.DevicePrice.PriceType.CustomerClient
                        }
                    }
                });
            }
            return stuffs;
        }
    }
}
