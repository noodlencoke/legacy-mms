using Daimler.HELM.WechatInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Daimler.HELM.WechatWeb
{
    public class OAuthHandler:IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try {

                string appId =System.Configuration.ConfigurationManager.AppSettings["AppId"];
                string redirectUrl = System.Configuration.ConfigurationManager.AppSettings["RedirectUrl"];
                redirectUrl = HttpUtility.UrlEncode(redirectUrl);
                string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect", appId, redirectUrl);
                context.Response.Redirect(url,false);
            }
            catch (Exception ex)
            {
                Log.LogHandler.WriteLog(ex.Message+ex.StackTrace);
            }
        }
    }
}