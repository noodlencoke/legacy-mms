using System;
using System.Web;

namespace Daimler.HELM.WechatServerURL.wechatHandler
{
    public class RequestHandler : IHttpHandler
    {
        string token = System.Configuration.ConfigurationManager.AppSettings["wechatToken"];
        string key = System.Configuration.ConfigurationManager.AppSettings["wechatKey"];
        string appId = System.Configuration.ConfigurationManager.AppSettings["wechatAppId"];
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
           
        }

        #endregion
    }
}
