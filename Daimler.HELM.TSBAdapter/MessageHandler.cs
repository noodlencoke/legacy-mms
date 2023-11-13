using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Daimler.HELM.TSBAdapter
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
               var sendSmsRequest = (from c in doc.Descendants(xname + "mnfRequest").Descendants("sendSmsRequest")
                                     select c).FirstOrDefault();
               MessageInfo msgInfo = new MessageInfo();
               string senderName = CommonMethod.GetXmlElementValue(sendSmsRequest, "senderName");
               msgInfo.Number = CommonMethod.GetXmlElementValue(sendSmsRequest, "recipientMobileNr");
               if (senderName == string.Empty)
                   senderName="Mercedes me"; 
               msgInfo.SessionId =senderName;
               if (msgInfo.SessionId != "Daimler" && msgInfo.SessionId != "Access" && msgInfo.SessionId != "Mercedes me")
                   msgInfo.SessionId = "TSBOther";

               msgInfo.Content +="<"+senderName+">\r\n";
               msgInfo.Content += CommonMethod.GetXmlElementValue(sendSmsRequest, "smsText");
               return msgInfo;
           }
           catch (Exception ex)
           {
               throw new Exception("TSB message data formate incurrect:" + ex.Message); 
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
       /// <summary>
       /// get tsb return value
       /// </summary>
       /// <param name="returnValueInfo"></param>
       /// <returns></returns>
       public static string GetReturnValue(ReturnValueInfo returnValueInfo)
       {
           XNamespace tns="http://mbcnotif.daimler.com/messaging/response/v1";
           XNamespace tns1="http://mbcnotif.daimler.com/requestResponse/v1";
           XNamespace tns2="http://mbcnotif.daimler.com/datatypes/v1";
           XNamespace tns3="http://mbcnotif.daimler.com/responseInformation/v1";
           XNamespace xsi="http://www.w3.org/2001/XMLSchema-instance";
           XNamespace schemaLocation="http://mbcnotif.daimler.com/messaging/response/v1 mnfMessagingResponse.xsd";
           XDocument xml = new XDocument(
              new XDeclaration("1.0", "UTF-8", "yes"),
               new XElement(tns + "mnfResponse",
                   new XAttribute(XNamespace.Xmlns + "tns", tns.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "tns1", tns1.NamespaceName),
                     new XAttribute(XNamespace.Xmlns + "tns2", tns2.NamespaceName),
                      new XAttribute(XNamespace.Xmlns + "tns3", tns3.NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                        new XAttribute(xsi + "schemaLocation", schemaLocation.NamespaceName),
                   new XElement("sendSmsResponse"),
                   new XElement("responseInformation",
                      new XElement("code", returnValueInfo.Status),
                      new XElement("messages",
                          new XElement("message", returnValueInfo.Comment)))
                          ));
           return xml.ToString();
       }

    }
}
