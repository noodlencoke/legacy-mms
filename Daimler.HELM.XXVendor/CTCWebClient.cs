using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageInterface.Impl
{
    class CTCWebClient : WebClient
    {
        private static readonly string proxyAddress = ConfigurationManager.AppSettings["ProxyAddress"];
        private static readonly string proxyPort = ConfigurationManager.AppSettings["ProxyPort"];
        private static readonly string useProxy = ConfigurationManager.AppSettings["UseProxy"];
        private int _timeout;
        /// <summary>
        /// 超时时间(毫秒)
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }
        public CTCWebClient()
        {
            this._timeout = 60000;
        }

        public CTCWebClient(int timeout)
        {
            this._timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = this._timeout;
            request.ReadWriteTimeout = this._timeout;

            if ( Convert.ToBoolean(useProxy))
            {                                
                WebProxy proxy = new WebProxy(proxyAddress,Convert.ToInt32(proxyPort));
                //proxy.Credentials = new NetworkCredential("","");
                request.UseDefaultCredentials = true;
                request.Proxy = proxy;
            }
            return request;
        }

    }
}
