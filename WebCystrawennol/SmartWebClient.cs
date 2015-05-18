using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebCystrawennol
{
    public class SmartWebClient : WebClient
    {
        private readonly int _maxConcurentConnectionCount;

        public SmartWebClient(int maxConcurentConnectionCount = 50)
        {
            Encoding = Encoding.UTF8;
            this._maxConcurentConnectionCount = maxConcurentConnectionCount;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var httpWebRequest = (HttpWebRequest)base.GetWebRequest(address);
            if (httpWebRequest == null)
            {
                return null;
            }

            if (_maxConcurentConnectionCount != 0)
            {
                httpWebRequest.ServicePoint.ConnectionLimit = _maxConcurentConnectionCount;
            }

            return httpWebRequest;
        }
    }
}
