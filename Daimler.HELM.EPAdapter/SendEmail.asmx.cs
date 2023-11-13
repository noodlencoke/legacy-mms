using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Daimler.HELM.EPAdapter
{
    /// <summary>
    /// Summary description for SendEmail
    /// </summary>
    [WebService(Namespace = "http://contact.di.webservice.huiyee.com/")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    public class SendEmail : System.Web.Services.WebService
    {
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://contact.di.webservice.huiyee.com/",
            ResponseNamespace = "http://contact.di.webservice.huiyee.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int push([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0)
        {
            return 1;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://contact.di.webservice.huiyee.com/", ResponseNamespace = "http://contact.di.webservice.huiyee.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hi()
        {
            return "a";
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://contact.di.webservice.huiyee.com/", ResponseNamespace = "http://contact.di.webservice.huiyee.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string postxml([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0)
        {
            string returnXml = string.Empty;
            EmailReturnValueInfo returnValue = new EmailReturnValueInfo();
            try
            {
                RequestLog request = CommonMethod.GetRequestInfo(HttpContext.Current.Request);
                request.SendContent = arg0;
                request.DataSrouce = "EP";
                CommonResult result = EmailHandler.RecordEmailSendLog(request);

                if (!result.IsOK)
                {
                    throw new Exception("请求数据存储异常：" + result.ReturnMessage + request.RequestInfo + "," + request.SendContent);
                }
                List<MessageInfo> messageList = MessageHandler.ConvertEmailList(request);
                if (messageList.Count > 0)
                {
                    if (messageList.Count == 1)
                    {
                        MessageInfo emailMessage = messageList[0];
                        emailMessage.Priority = System.Messaging.MessagePriority.High;
                        ServiceFactory.GetInstance().SendSingleEDM(emailMessage);
                        if(emailMessage.Classification==MessageClassification.Single)
                        {
                            returnValue.TaskId=emailMessage.TaskId;
                            returnValue.TaskName="userspec"+emailMessage.ReceivedDt;
                        }
                        else if (emailMessage.Classification==MessageClassification.Batch)
                        {
                            returnValue.TaskId=emailMessage.BatchId;
                            returnValue.TaskName="sendspec"+emailMessage.ReceivedDt;
                        }
                     
                     }
                     else
                     {
                         ServiceFactory.GetInstance().SendBatchEDM(messageList);
                         MessageInfo emailMessage = messageList[0];
                         returnValue.TaskId=emailMessage.BatchId;
                         returnValue.TaskName="sendspec"+emailMessage.ReceivedDt;
                     }
                    returnValue.Status = "NDP";
                    returnValue.Comment = "success";
                }
               returnXml= MessageHandler.GetEmailReturnValue(returnValue);
               return returnXml;
            }
            catch (Exception ex)
            {
                returnValue.Status = "ERR";
                returnValue.Comment = "send email fail "+ex.Message.ToString();

                ExceptionLog log = new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "email send",
                    ExceptionInfo = ex
                };
                CommonHandler.RecordExceptionLog(log);

                returnXml = MessageHandler.GetEmailReturnValue(returnValue);
                return returnXml;
            }
        }
    }
}
