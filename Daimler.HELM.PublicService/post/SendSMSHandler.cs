using System;
using System.Web;

namespace Daimler.HELM.PublicService
{
    public class SendSMSHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Log.LogHandler.WriteLog("begin send message");
                HttpRequest request = context.Request;
                byte[] data = request.BinaryRead(request.TotalBytes);
                string strRequestParams = System.Text.Encoding.UTF8.GetString(data);
                string accessToken = request.QueryString["accessToken"];
                Log.LogHandler.WriteLog("token:"+accessToken);
                Log.LogHandler.WriteLog("content:"+strRequestParams);
                string strVal = PublicServiceHandler.SendSingleSMS(accessToken, strRequestParams);
                context.Response.Write(strVal);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        #endregion
    }
}
