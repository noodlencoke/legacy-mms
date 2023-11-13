using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Daimler.HELM.LMSAdapter
{
    public static class MessageHandler
    {
        public static sendSmsRequest GetRequestParams(string paramList)
        {
            try
            {
                string[] strParams = paramList.Split('&');

                Dictionary<string, string> dicParam = new Dictionary<string, string>();
                string[] applicationID = strParams[0].Split('=');
                string[] destinationAddresses = strParams[1].Split('=');
                string[] extendCode = strParams[2].Split('=');
                string[] message = strParams[3].Split('=');
                string[] messageFormat = strParams[4].Split('=');
                string[] sendMethod = strParams[5].Split('=');
                string[] deliveryResultRequest = strParams[6].Split('=');

                dicParam.Add(applicationID[0], applicationID[1]);
                dicParam.Add(destinationAddresses[0], destinationAddresses[1]);
                dicParam.Add(extendCode[0], extendCode[1]);
                dicParam.Add(message[0], message[1]);
                dicParam.Add(messageFormat[0], messageFormat[1]);
                dicParam.Add(sendMethod[0], sendMethod[1]);
                dicParam.Add(deliveryResultRequest[0], deliveryResultRequest[1]);

                sendSmsRequest paramObj = new sendSmsRequest()
                {
                    ApplicationID = dicParam["applicationID"],
                    DestinationAddresses = new string[]{dicParam["destinationAddresses"]},
                    ExtendCode = dicParam["extendCode"],
                    Message = dicParam["message"],
                    MessageFormat = (MessageFormat)Enum.Parse(typeof(MessageFormat),dicParam["messageFormat"]),
                    SendMethod = (SendMethodType)Enum.Parse(typeof(SendMethodType),dicParam["sendMethod"]),
                    DeliveryResultRequest = Boolean.Parse(dicParam["deliveryResultRequest"].ToString())
                };
                return paramObj;
            }
            catch (Exception ex)
            {
                throw new Exception("LMS SMS message data format incurrect:"+ex.Message);
            }
        }

        public static string GetLMSReturnValue(LMSReturnValueInfo returnValueInfo)
        { 
             //XDocument xml = new XDocument(
             //   new XDeclaration("1.0", "UTF-8","yes"),
             //    new XElement("result",
             //       new XElement("User",
             //           new XElement("status",CommonMethod.EncodeBase64(returnValueInfo.Status)),
             //           new XElement("comment",CommonMethod.EncodeBase64(returnValueInfo.Comment)),
             //           new XElement("taskid", CommonMethod.EncodeBase64(returnValueInfo.TaskId))
             //               )));
             //return xml.ToString();
            return returnValueInfo.Status;
        }

        public static MessageInfo ConvertLMS(RequestLog request)
        {
            MessageInfo messageInfo = new MessageInfo();
            string lmsInfo = request.SendContent;
            lmsInfo = lmsInfo.Replace(':', '-');
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(lmsInfo);

            if (lmsInfo.Contains("sms-sendSmsRequest"))
            {
                messageInfo.Id = Guid.NewGuid();
                messageInfo.Type = MessageType.SMS;
                messageInfo.RequestId = request.Id;
                messageInfo.ReceivedDt = request.CreatedDt;
                messageInfo.DataSource = request.DataSrouce;
                messageInfo.Priority = System.Messaging.MessagePriority.Lowest;
                messageInfo.TaskId = Guid.NewGuid().ToString();
                XmlNode numberNode = xdoc.SelectSingleNode("/SOAP-ENV-Envelope/SOAP-ENV-Body/sms-sendSmsRequest/DestinationAddresses/text()");
                if (numberNode != null && numberNode.Value != null && numberNode.Value!=String.Empty)
                {
                    string telephone = numberNode.Value.Substring(4, numberNode.Value.Length - 4);
                    if (telephone.StartsWith("86"))
                        telephone = telephone.Substring(2, telephone.Length - 2);
                    messageInfo.Number = telephone;
                }

                XmlNode contentNode = xdoc.SelectSingleNode("/SOAP-ENV-Envelope/SOAP-ENV-Body/sms-sendSmsRequest/Message/text()");
                if (contentNode != null && contentNode.Value != null && contentNode.Value != String.Empty)
                {
                    messageInfo.Content = contentNode.Value;
                }

                messageInfo.Classification = MessageClassification.Single;
            }
            return messageInfo;
        }

        public static List<MessageInfo> ConvertLMS(RequestLog request, sendSmsRequest lsmp)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            if (lsmp.DestinationAddresses != null && lsmp.DestinationAddresses.Length > 0)
            {
                MessageInfo messageInfo = new MessageInfo();
                messageInfo.Id = Guid.NewGuid();
                messageInfo.Type = MessageType.SMS;
                messageInfo.RequestId = request.Id;
                messageInfo.ReceivedDt = request.CreatedDt;
                messageInfo.DataSource = request.DataSrouce;
                messageInfo.Priority = System.Messaging.MessagePriority.Lowest;
                messageInfo.TaskId = Guid.NewGuid().ToString();
                messageInfo.SessionId = "SMS";

                foreach (string address in lsmp.DestinationAddresses)
                {
                    string telephone = address.Substring(4, address.Length - 4);
                    if (telephone.StartsWith("86"))
                        telephone = telephone.Substring(2, telephone.Length - 2);
                    messageInfo.Number = telephone;
                }

                messageInfo.Content = lsmp.Message;
                messageInfo.Classification = MessageClassification.Single;

                messageList.Add(messageInfo);
            }
            return messageList;
        }
    }
}