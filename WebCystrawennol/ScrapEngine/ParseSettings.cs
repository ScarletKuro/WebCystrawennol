using System.Collections.Generic;
using System.Net;

namespace WebCystrawennol.ScrapEngine
{
    public class ParseSettings
    {
        public string UrlSite { get; private set; }
        public WebHeaderCollection Heades { get; private set; }
        public Methods Method { get; private set; }
        public string Data { get; private set; }
        public enum Methods
        {
            GET = 0,
            POST = 1,
        };

        public ParseSettings()
        {
            UrlSite = string.Empty;
            Data = string.Empty;
            Heades = new WebHeaderCollection();
            Method = Methods.GET;
        }
        public ParseSettings(string url)
        {
            UrlSite = url;
            Data = string.Empty;
            Heades = new WebHeaderCollection();
            Method = Methods.GET;
        }
        public ParseSettings(string url, Methods method, string data, WebHeaderCollection headers)
        {
            UrlSite = url;
            Data = data;
            Method = method;
            Heades = headers;
        }
    }
}
