using System.Collections.Generic;

namespace WebCystrawennol.ScrapEngine
{
    public abstract class BaseScrap
    {
        public abstract void GetItems(string url);
        public abstract List<string> Urls { get; set; }
    }
}
