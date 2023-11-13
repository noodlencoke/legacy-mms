using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace Daimler.HELM.PublicService
{
    /// <summary>
    /// Summary description for SendSMS1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SendSMS1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string SendSingleSMS(string accessToken, string content)
        {
            return PublicServiceHandler.SendSingleSMS(accessToken,content);
        }

        [WebMethod]
        public string GetAccessToken(string dataSource, string accountName, string securityKey)
        {
            return PublicServiceHandler.GetAccessToken(dataSource,accountName,securityKey);
        }
    }
}
