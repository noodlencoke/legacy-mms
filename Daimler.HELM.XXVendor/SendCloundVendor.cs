using CodeScales.Http;
using CodeScales.Http.Entity;
using CodeScales.Http.Entity.Mime;
using CodeScales.Http.Methods;
using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Daimler.HELM.MessageInterface.Impl
{
    public class SendCloundVendor:IEmailInterface
    {
        private static readonly string proxyAddress = ConfigurationManager.AppSettings["ProxyAddress"];
        private static readonly string proxyPort = ConfigurationManager.AppSettings["ProxyPort"];
        private static readonly string useProxy = ConfigurationManager.AppSettings["UseProxy"];
        private readonly string api_key = ConfigurationManager.AppSettings["EdmKey"];

        private readonly string single_api_user = ConfigurationManager.AppSettings["SingleEdmAccount"];
        private readonly string single_senderEmail = ConfigurationManager.AppSettings["SingleSenderEmail"];
        private readonly string single_senderName = ConfigurationManager.AppSettings["SingleSenderName"];

        private readonly string batch_api_user = ConfigurationManager.AppSettings["BatchEdmAccount"];
        private readonly string batch_senderEmail = ConfigurationManager.AppSettings["BatchSenderEmail"];
        private readonly string batch_senderName = ConfigurationManager.AppSettings["BatchSenderName"];

        private readonly string replyTo = ConfigurationManager.AppSettings["ReplyTo"];

        private readonly string sendEmailUrl = ConfigurationManager.AppSettings["EmailServerUrl"];
        private readonly string mehtodName = ConfigurationManager.AppSettings["templateName"];
        public BizObjects.RequestResultInfo SendEmail(BizObjects.MessageInfo msgInfo)
        {

            string templateName = msgInfo.TemplateId;
            string receiverEmail = msgInfo.Number;
            string subject = msgInfo.Subject;
           
                                                      

            #region Call3rdSendEmail

            HttpClient client = new HttpClient();
            if (Convert.ToBoolean(useProxy))
            {
                client.Proxy = new Uri("http://"+proxyAddress + ":" + proxyPort);
            }

            HttpPost postMethod = new HttpPost(new Uri(sendEmailUrl));
            MultipartEntity multipartEntity = new MultipartEntity();
            postMethod.Entity = multipartEntity;
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "template_invoke_name", templateName));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "substitution_vars", "{\"to\": [\"" + receiverEmail + "\"]}"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user",single_api_user));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key",api_key));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "from",single_senderEmail));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname",single_senderName));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", subject));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "replyto", replyTo));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "resp_email_id", "true"));
            #endregion
            try
            {
                HttpResponse response = client.Execute(postMethod);
                JavaScriptSerializer javascript = new JavaScriptSerializer();
                string resultJson = EntityUtils.ToString(response.Entity);

                ResponsInfo emailResponse = javascript.Deserialize<ResponsInfo>(resultJson);
                Daimler.HELM.BizObjects.RequestResultInfo requestResultInfo = new BizObjects.RequestResultInfo();

                if (emailResponse.message == "success")
                {
                    requestResultInfo.EmailStatus = EmailStatus.SendSuccess;
                    requestResultInfo.Mobile = msgInfo.Number;
                    requestResultInfo.TaskId = emailResponse.email_id_list[0];
                    requestResultInfo.Desc = emailResponse.message;
                }
                else
                {
                    requestResultInfo.Mobile = msgInfo.Number;
                    requestResultInfo.EmailStatus = EmailStatus.SendError;
                    requestResultInfo.Desc = emailResponse.message + ":" + resultJson;
                }

                return requestResultInfo;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }


   

        }



        public List<RequestResultInfo> SendEmail(List<MessageInfo> msgInfo)
        {
            
            string templateName = msgInfo[0].TemplateId;
            string receiverEmail = "";
            string subject = msgInfo[0].Subject;
            foreach (var item in msgInfo)
            {
                receiverEmail += "\"" + item.Number + "\",";
            }
            receiverEmail = receiverEmail.TrimEnd(',');

            #region call3rd send email

            HttpClient client = new HttpClient();

            if (Convert.ToBoolean(useProxy))
            {
                client.Proxy = new Uri("http://"+proxyAddress + ":" + proxyPort);
            }

            HttpPost postMethod = new HttpPost(new Uri(sendEmailUrl));
            MultipartEntity multipartEntity = new MultipartEntity();
            postMethod.Entity = multipartEntity;
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "template_invoke_name", templateName));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "substitution_vars", "{\"to\": [" + receiverEmail + "]}"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", batch_api_user));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key",api_key));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", batch_senderEmail));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", batch_senderName));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", subject));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "replyto", replyTo));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "resp_email_id", "true"));
            #endregion
            try
            {
                HttpResponse response = client.Execute(postMethod);
           

                JavaScriptSerializer javascript = new JavaScriptSerializer();
                string resultJson = EntityUtils.ToString(response.Entity);

                ResponsInfo emailResponse = javascript.Deserialize<ResponsInfo>(resultJson);

                List<RequestResultInfo> emailRequestList = new List<RequestResultInfo>();
                if (emailResponse.message == "success")
                {
                    foreach (var item in emailResponse.email_id_list)
                    {
                        Daimler.HELM.BizObjects.RequestResultInfo requestResultInfo = new BizObjects.RequestResultInfo();

                        requestResultInfo.EmailStatus = EmailStatus.SendSuccess;
                        int index = item.IndexOf('$') + 1;
                        requestResultInfo.Mobile = item.Substring(index, item.Length - index);
                        requestResultInfo.TaskId = item;
                        requestResultInfo.Desc = emailResponse.message;

                        emailRequestList.Add(requestResultInfo);
                    }

                }
                else
                {
                    foreach (var item in msgInfo)
                    {
                        Daimler.HELM.BizObjects.RequestResultInfo requestResultInfo = new BizObjects.RequestResultInfo();
                        requestResultInfo.Mobile = item.Number;
                        requestResultInfo.EmailStatus = EmailStatus.SendError;

                        requestResultInfo.Desc = emailResponse.message + ":" + resultJson;
                        emailRequestList.Add(requestResultInfo);
                    }

                }

                return emailRequestList;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }          
        }
    }
    public class ResponsInfo
    {
        public string message { set; get; }
        public string[] email_id_list { set; get; }
        public string[] errors { set; get; }
    }
}
