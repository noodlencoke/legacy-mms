using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Daimler.HELM.POSAdapter
{
   public static class MessageHandler
    {
       public static MessageInfo ConvertSMSList(string messageInfo)
       {
           TextReader textReader = null;
           try
           {
               textReader = new StringReader(messageInfo);
               XDocument doc = XDocument.Load(textReader);
               XNamespace xname = doc.Root.Name.NamespaceName;
               var outboundMessage = (from c in doc.Descendants(xname + "OutboundMessageDispatcher").Descendants(xname + "Content").Descendants(xname + "OutboundMessage")
                                      select c).FirstOrDefault();
               var contract = (from c in doc.Descendants(xname + "OutboundMessageDispatcher").Descendants(xname + "Content").Descendants(xname + "OutboundMessage").Descendants(xname + "Recipients").Descendants(xname + "Contact")
                               select c).FirstOrDefault();
               MessageInfo msgInfo = new MessageInfo();
               msgInfo.Number = CommonMethod.GetXmlElementValue(contract, "MobileNumber", xname);
               msgInfo.Content = CommonMethod.GetXmlElementValue(outboundMessage, "Body", xname);
               return msgInfo;
           }
           catch (Exception ex)
           {
               throw new Exception("POS message data formate incurrect:" + ex.Message); 
           }
           finally
           {
               if (textReader != null)
               {
                   textReader.Close();
                   textReader.Dispose();
               }
           }
       }

    }
}
