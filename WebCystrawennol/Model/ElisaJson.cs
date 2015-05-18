using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebCystrawennol.Model
{
    public class ElisaJson
    {
        public class RootElisa
        {
            [JsonProperty("result")]
            public IList<Result> ElisaDevices { get; set; }

        }
        public class Result
        {
            [JsonProperty("model")]
            public string model { get; set; }
            [JsonProperty("price")]
            public double price { get; set; }
            [JsonProperty("customerPrice")]
            public double customerPrice { get; set; }
            [JsonProperty("clientPrice")]
            public double clientPrice { get; set; }

            [JsonProperty("coverPhotoFilename")]
            public string coverPhotoFilename { get; set; }
        }
    }
}
