using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Daimler.HELM.EPAdapter
{
    public static class MessageHandler
    {

        private static readonly string sendExtNumber = System.Configuration.ConfigurationManager.AppSettings["sendExtNumber"];

        public static List<MessageInfo> ConvertMessageList(MessageType type, EPSendMessageParam paramObj, RequestLog request)
        {
            try
            {


                string strMessageList = paramObj.phone_taskId_content;
                List<MessageInfo> messageList = new List<MessageInfo>();
                string[] itemList = strMessageList.Split(new string[] { "/r/n/" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in itemList)
                {
                    string[] strMessageInfo = item.Split(new string[] { "/@/#/" }, StringSplitOptions.None);
                    
                    string sessionId = string.Empty;
                    string content = strMessageInfo[2].Trim();
                    if (type == MessageType.MMS)
                    {
                        sessionId = "MMS"; 
                    }else if(paramObj.productId=="3161"){
                        sessionId = "NeedReplySMS";
                    }
                    else if (content.Length>=8 && content.Substring(0,8).ToLower().Equals("[retail]"))
                    {
                        sessionId = "Retail";
                        content = content.Remove(0,8);
                    }
                    else {
                        sessionId = "Wholesales";
                    }
                    MessageInfo msgInfo = new MessageInfo()
                    {
                        Id = Guid.NewGuid(),
                        RequestId = request.Id,
                        ProductId = paramObj.productId,
                        Type = type,
                        Number = strMessageInfo[0],
                        TaskId = strMessageInfo[1],
                        Content = content,
                        TemplateId = content,
                        ReceivedDt = request.CreatedDt,
                        DataSource = request.DataSrouce,
                        Priority = System.Messaging.MessagePriority.Lowest,
                        SessionId = sessionId,
                        IsNeedReply = true
                    };

                    messageList.Add(msgInfo);
                }

                return messageList;
            }
            catch (Exception ex)
            {
                throw new Exception("Ep SMS message data formate incurrect:" + ex.Message + ex.StackTrace);
            }
        }

        public static EPSendMessageParam GetRequestParams(string paramList)
        {
            try
            {
                string[] strParams = paramList.Split('&');

                Dictionary<string, string> dicParam = new Dictionary<string, string>();
                string[] userName = strParams[0].Split('=');
                string[] productId = strParams[1].Split('=');
                string[] phone_taskId_content = strParams[2].Split('=');
                string[] password = strParams[3].Split('=');

                dicParam.Add(userName[0], userName[1]);
                dicParam.Add(productId[0], productId[1]);
                dicParam.Add(phone_taskId_content[0], phone_taskId_content[1]);
                dicParam.Add(password[0], password[1]);

                EPSendMessageParam paramObj = new EPSendMessageParam()
                {
                    userName = dicParam["username"],
                    productId = dicParam["productId"],
                    phone_taskId_content = System.Web.HttpUtility.UrlDecode(dicParam["phone_taskId_content"]),
                    password = dicParam["password"]
                };
                return paramObj;
            }
            catch (Exception ex)
            {
                throw new Exception("Ep SMS message data formate incurrect:" + ex.Message + ex.StackTrace);
            }
        }

        public static List<MessageInfo> ConvertEmailList(RequestLog request)
        {
            TextReader textReader = null;
            try
            {
                List<MessageInfo> messageList = new List<MessageInfo>();
                string emailInfo = request.SendContent;
                textReader = new StringReader(emailInfo);
                XDocument doc = XDocument.Load(textReader);
                if (emailInfo.Contains("<userspec>"))
                {
                    var user = (from c in doc.Descendants("user")
                                select c).FirstOrDefault();
                    MessageInfo messageInfo = new MessageInfo();
                    messageInfo.Id = Guid.NewGuid();
                    messageInfo.Type = MessageType.Email;
                    messageInfo.RequestId = request.Id;
                    messageInfo.ReceivedDt = request.CreatedDt;
                    messageInfo.DataSource = request.DataSrouce;
                    messageInfo.Priority = System.Messaging.MessagePriority.Lowest;
                    messageInfo.TaskId = Guid.NewGuid().ToString();
                    messageInfo.BatchId = messageInfo.TaskId;
                    messageInfo.Number = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(user, "email"));
                    messageInfo.EmailName = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(user, "name"));
                    messageInfo.TemplateId = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(user, "emailtemplateid"));
                    messageInfo.InterestedSeries = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(user, "interestedseries"));
                    messageInfo.Classification = MessageClassification.Single;
                    messageList.Add(messageInfo);
                }
                else
                    if (emailInfo.Contains("<sendspec>"))
                    {
                        var emailspec = (from c in doc.Descendants("emailspec")
                                         select c).FirstOrDefault();
                        var sendspec = (from c in doc.Descendants("sendspec")
                                        select c).FirstOrDefault();
                        MessageInfo specInfo = new MessageInfo();
                        specInfo.ReceivedDt = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(sendspec, "sendtime"));
                        specInfo.From = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(emailspec, "from"));
                        specInfo.Reply = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(emailspec, "reply"));
                        specInfo.Subject = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(emailspec, "subject"));
                        specInfo.TemplateId = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(emailspec, "zip"));
                        specInfo.BatchId = Guid.NewGuid().ToString();
                        var users = from c in doc.Descendants("users").Descendants("user")
                                    select c;
                        foreach (var user in users)
                        {
                            MessageInfo messageInfo = new MessageInfo();
                            messageInfo.Id = Guid.NewGuid();
                            messageInfo.Type = MessageType.Email;
                            messageInfo.RequestId = request.Id;
                            messageInfo.ReceivedDt = request.CreatedDt;
                            messageInfo.DataSource = request.DataSrouce;
                            messageInfo.Priority = System.Messaging.MessagePriority.Lowest;
                            messageInfo.TaskId = Guid.NewGuid().ToString();
                            messageInfo.Number = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(user, "email"));
                            messageInfo.EmailName = CommonMethod.DecodeBase64(CommonMethod.GetXmlElementValue(user, "name"));
                            messageInfo.BatchId = specInfo.BatchId;
                            messageInfo.From = specInfo.From;
                            messageInfo.Reply = specInfo.Reply;
                            messageInfo.Subject = specInfo.Subject;
                            messageInfo.TemplateId = specInfo.TemplateId;
                            messageInfo.Classification = MessageClassification.Batch;
                            messageList.Add(messageInfo);
                        }
                    }
                    else
                        if (emailInfo.Contains("<reportspec>"))
                        {
                            throw new Exception("EP email report interface does not implement");
                        }
                        else
                        {
                            throw new Exception("Ep Email xml data format incurrect");
                        }
                return messageList;
            }
            catch (Exception ex)
            {
                throw new Exception("Ep Email xml data formate incurrect:" + ex.Message);
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

        public static string GetEmailReturnValue(EmailReturnValueInfo returnValueInfo)
        {
            XDocument xml = new XDocument(
               new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("result",
                   new XElement("User",
                       new XElement("status", CommonMethod.EncodeBase64(returnValueInfo.Status)),
                       new XElement("comment", CommonMethod.EncodeBase64(returnValueInfo.Comment)),
                       new XElement("taskid", CommonMethod.EncodeBase64(returnValueInfo.TaskId)),
                       new XElement("taskname", CommonMethod.EncodeBase64(returnValueInfo.TaskName)))
                           ));
            return xml.ToString();
        }

    }
}