using System;
using System.Web;

namespace Daimler.HELM.PublicService
{
    public class GetTokenHandler : IHttpHandler
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
                Log.LogHandler.WriteLog("begin call service");
                HttpRequest request = context.Request;
                string dataSource = request.QueryString["dataSource"];
                string accountId = request.QueryString["accountId"];
                string securityKey = request.QueryString["securityKey"];

                Log.LogHandler.WriteLog(dataSource+","+accountId+","+securityKey);

                string strVal = PublicServiceHandler.GetAccessToken(dataSource, accountId, securityKey);
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
