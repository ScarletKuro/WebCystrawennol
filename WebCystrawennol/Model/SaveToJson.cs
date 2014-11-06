using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            };

        }
        public class Device
        {
            [JsonProperty("product_name")]
            public string Name { get; set; }
            [JsonProperty("img_url")]
            public string ImageUrl { get; set; }
            [JsonProperty("product_price")]
            public IList<DevicePrice> ProductPrice { get; set; }
            
        }
        public class RootObject
        {
            [JsonProperty("shop")]
            public string ShopName { get; set; }
            [JsonProperty("items")]
            public IList<Device> Items { get; set; }
        }
    }
}
