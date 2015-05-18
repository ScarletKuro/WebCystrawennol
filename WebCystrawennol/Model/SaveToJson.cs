using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebCystrawennol.Model
{
    public class SaveToJson
    {
        public class DevicePrice
        {
            
            [JsonProperty("price")]
            public string Price { get; set; }
            [JsonProperty("price_type")]
            public PriceType Type { get; set; }
            public enum PriceType
            {
                Normal=0,
                Client=1,
                CustomerClient=2
            };

        }
        public class Device
        {
            [JsonProperty("shop")]
            public string ShopName { get; set; }
            [JsonProperty("product_name")]
            public string Name { get; set; }
            [JsonProperty("img_url")]
            public string ImageUrl { get; set; }
            [JsonProperty("product_price")]
            public IList<DevicePrice> ProductPrice { get; set; }
            
        }
    }
}
