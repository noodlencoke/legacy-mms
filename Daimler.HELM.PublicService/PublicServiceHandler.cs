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
using System.Xml;

namespace Daimler.HELM.PublicService
{
    public static class PublicServiceHandler
    {

        public static string GetAccessToken(string dataSource, string accountName, string securityKey)
        {
            try
            {
                ISendMessageService service = ServiceFactory.GetInstance();
                bool isOk = false;
                string accessToken = string.Empty;
                string errorInfo = string.Empty;
                try
                {
                    CommonResult result = service.ValidateInterfaceAccount(dataSource, accountName, securityKey);

                    if (result.IsOK)
                    {
                        isOk = true;
                        //加密
                        string token = string.Format("{0}_{1}_{2}_{3}", dataSource, accountName, securityKey, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        accessToken = CommonMethod.EncryptString("!QAZ@WSX4rfv", token);
                    }
                    else
                    {
                        isOk = false;
                        errorInfo = result.ReturnMessage;
                    }
                }
                catch (Exception ex)
                {
                    isOk = false;
                    errorInfo = "获取AccessToken异常，请联系管理员！";

                    ExceptionLog log = new ExceptionLog()
                    {
                        MessageType = "GetAccessToken Error",
                        ExceptionInfo = ex
                    };
                    service.RecordExceptionLog(log);

                    throw ex;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sb.Append("<xml>");
                sb.Append("<isOk>" + isOk.ToString() + "</isOk>");
                sb.Append("<accessToken>" + accessToken + "</accessToken>");
                sb.Append("<errorInfo>" + errorInfo + "</errorInfo>");
                sb.Append("</xml>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message + "," + ex.StackTrace;
            }
        }


        public static string SendSingleSMS(string accessToken, string content)
        {
            try
            {
                bool isOk = true;
                string errorInfo = string.Empty;
                ISendMessageService service = ServiceFactory.GetInstance();
                try
                {
                    RequestLog request = CommonMethod.GetRequestInfo(HttpContext.Current.Request);
                    request.SendContent = content;
                    int xmlSize = Encoding.Default.GetBytes(content).Length;
                    if (xmlSize / 1024 / 1024 >= 1)
                    {
                        request.SendContent = content.Substring(0, 500);
                        Log.LogHandler.WriteLog("unified interface received too large xml message of sms:" + content);
                        //service.RecordSMSSendLog(request);

                        isOk = false;
                        errorInfo = "参数数据量超过 1M !";
                        return SendReturnValue(isOk, errorInfo);
                    }


                    if (string.IsNullOrEmpty(accessToken))
                    {
                        isOk = false;
                        errorInfo = "accessToken 为空！";
                        return SendReturnValue(isOk, errorInfo);
                    }

                    //validate xml formate
                    string errorMsg = ValidateXML(content);
                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        isOk = false;
                        return SendReturnValue(isOk, errorMsg);
                    }

                    MessageInfo msgInfo = GetMessageInfo(content);
                    if (msgInfo == null)
                    {
                        isOk = false;
                        errorInfo = "xml 不正确！";
                        return SendReturnValue(isOk,errorInfo);
                    }

                    string accountInfo = CommonMethod.DecryptString("!QAZ@WSX4rfv", accessToken);
                    string[] infoList = accountInfo.Split('_');
                    if (infoList.Length != 4 || msgInfo.DataSource != infoList[0])
                    {
                        isOk = false;
                        errorInfo = "accessToken 错误！";
                        return SendReturnValue(isOk, errorInfo);
                    }
                    string tokenTime = infoList[3];
                    TimeSpan ts = DateTime.Now - Convert.ToDateTime(tokenTime);
                    if (ts.Hours >= 2)
                    {
                        isOk = false;
                        errorInfo = "accessToken 过期！";
                        return SendReturnValue(isOk, errorInfo);
                    }
                    request.SendContent = content;
                    request.DataSrouce = msgInfo.DataSource;
                    service.RecordSMSSendLog(request);

                    msgInfo.ReceivedDt = request.CreatedDt;
                    msgInfo.RequestId = request.Id;
                    msgInfo.SessionId = "SMS";
                    CommonResult sendResult = service.SendSingleSMS(msgInfo);
                    if (!sendResult.IsOK)
                    {
                        isOk = false;
                        errorInfo = sendResult.ReturnMessage;
                        return SendReturnValue(isOk, errorInfo);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLog log = new ExceptionLog()
                    {
                        MessageType = "unified interface sms send ",
                        ExceptionInfo = ex
                    };
                    service.RecordExceptionLog(log);

                    isOk = false;
                    errorInfo = "发送失败！";
                }

                return SendReturnValue(isOk, errorInfo);
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message + "," + ex.StackTrace;
            }
        }

        private static MessageInfo GetMessageInfo(string content)
        {                           
            try
            {        
                MessageInfo msgInfo = new MessageInfo();
                XmlDocument xmlMessage = new XmlDocument();
                xmlMessage.LoadXml(content);
                msgInfo.Id = Guid.NewGuid();
                msgInfo.Classification = MessageClassification.Single;
                msgInfo.DataSource = xmlMessage.SelectSingleNode("xml/smsSource").InnerText;
                msgInfo.TaskId = xmlMessage.SelectSingleNode("xml/sendSmsRequest/uniqueID").InnerText;
                msgInfo.Number = xmlMessage.SelectSingleNode("xml/sendSmsRequest/recipientMobile").InnerText;
                msgInfo.Content = xmlMessage.SelectSingleNode("xml/sendSmsRequest/smsText").InnerText;
                msgInfo.Priority = (MessagePriority)Convert.ToInt32(xmlMessage.SelectSingleNode("xml/sendSmsRequest/smsPriority").InnerText);    
                return msgInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
          
        }


        private static string SendReturnValue(bool isOk, string errorInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<xml>");
            sb.Append("<isOk>" + isOk.ToString() + "</isOk>");
            sb.Append("<errorInfo>" + errorInfo + "</errorInfo>");
            sb.Append("</xml>");
            return sb.ToString();
        }


        private static string ValidateXML(string content)
        {
            string errorMsg = string.Empty;
            using (StringReader txtReader = new StringReader(content))
            {
                try
                {
                    //validate the xml formate
                    string schemaFile = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\SMSMessage.xsd";
                    //string xmlFile = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\SMSMessage.xml";
                    string namespaceUrl = "http://tempuri.org/OrderSchema.xsd";

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ValidationType = ValidationType.Schema;
                    settings.Schemas.Add(null, schemaFile);
                    settings.ValidationEventHandler += (obj, e) =>
                    {
                        errorMsg += e.Message;
                    };

                    using (XmlReader xmlReader = XmlReader.Create(txtReader, settings))
                    {
                        try
                        {
                            xmlReader.MoveToContent();
                            while (xmlReader.Read())
                            {
                                if (xmlReader.NodeType == XmlNodeType.Document && xmlReader.NamespaceURI != namespaceUrl)
                                {
                                    errorMsg = "The xml formate incorrect";
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorMsg += ex.Message;
                        }
                        finally
                        {
                            xmlReader.Close();
                            xmlReader.Dispose();
                        }

                    }

                }
                catch (Exception ex)
                {
                    errorMsg += ex.Message;
                }
                finally
                {
                    if (txtReader != null)
                    {
                        txtReader.Close();
                        txtReader.Dispose();
                    }
                }
            }

            return errorMsg;
        }
    }
}