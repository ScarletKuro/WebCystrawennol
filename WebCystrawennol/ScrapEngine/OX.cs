using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCystrawennol.Model;

namespace WebCystrawennol.ScrapEngine
{
    public class OX : BaseScrap
    {
        public OX()
        {
            const string postData = "";
        }
        protected override IEnumerable<SaveToJson.Device> GetItems(string sitecontent)
        {
            throw new NotImplementedException();
        }
    }
}
