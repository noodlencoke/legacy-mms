using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Daimler.HELM.PublicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SendSMS" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SendSMS.svc or SendSMS.svc.cs at the Solution Explorer and start debugging.
    public class SendSMS : ISendSMS
    {
        public string GetData(string xml)
        {
            try {
                return "xml:"+xml;
            }
            catch (Exception ex)
            {
                return "error:"+ex.Message;
            }
        }

        public string SendSingleSMS(string accessToken, string content)
        {
            return PublicServiceHandler.SendSingleSMS(accessToken,content);
        }

       
        public string GetAccessToken(string dataSource, string accountName, string securityKey)
        {
            return PublicServiceHandler.GetAccessToken(dataSource,accountName,securityKey);
        }

    }
}
