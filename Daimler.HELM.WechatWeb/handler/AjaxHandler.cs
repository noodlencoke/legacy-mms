using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Daimler.HELM.WechatWeb
{
    public class AjaxHandler:IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            ISendMessageService service = ServiceFactory.GetInstance();
            CommonResult result = new CommonResult();
            result.IsOK = false;
            try
            {
                HttpRequest request = context.Request;
                byte[] data = request.BinaryRead(request.TotalBytes);
                string strRequestParams = System.Text.Encoding.UTF8.GetString(data);

                string type = request.QueryString["Type"];

                if (type == "GetPinCode")
                {
                    Dictionary<string, object> dicTransferObj = ConvertJsonToObject(strRequestParams);

                    string mobile = dicTransferObj["Mobile"].ToString();
                    string pinCode = GeneratePinCode(mobile);
                    string content = string.Format("微信身份绑定验证码:{0},如非本人操作请忽略.", pinCode);
                    
                    result =  SendSMS(mobile, content);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog log = new ExceptionLog()
                {
                    MessageType = "wechat  ",
                    ExceptionInfo = ex
                };
                service.RecordExceptionLog(log);
                result.IsOK = false;
            }

           string strResult = CommonMethod.ConvertObjectToJson(result);

           context.Response.Write(strResult);
        }


        /// <summary>
        /// send sms message
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private CommonResult SendSMS(string mobile,string content)
        {
            CommonResult result = new CommonResult();
            result.IsOK = false;
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["SendSMSUrl"];
                string tokenObj = CommonMethod.DoPost(string.Format("{0}/getToken?dataSource={1}&accountId={2}&securityKey={3}", url, "MeStore", "acc1", "1qaz2wsx"), "");
                StringReader sr = new StringReader(tokenObj);
                XmlDocument xd = new XmlDocument();
                xd.Load(sr);
                sr.Close();
                sr.Dispose();

                XmlNode node = xd.SelectSingleNode("xml/accessToken");
                string accessToken = node.InnerText;

                string xmlContent = @"<xml>
                            <smsSource>MeStore</smsSource>
                            <sendSmsRequest>
                            <uniqueID>" + Guid.NewGuid() + @"</uniqueID>
                            <senderName>sender</senderName>
                            <recipientMobile>" + mobile + @"</recipientMobile>
                            <smsText>" + content + @"</smsText>
                            <smsPriority>7</smsPriority>
                            </sendSmsRequest>
                            </xml>";
                url = string.Format("{0}/sendSMS?accessToken={1}", url, System.Web.HttpUtility.UrlEncode(accessToken));
                string resultXml = CommonMethod.DoPost(url, xmlContent);
                StringReader sr2 = new StringReader(resultXml);
                XmlDocument xd2 = new XmlDocument();
                xd2.Load(sr2);
                sr2.Close();
                sr2.Dispose();

                XmlNode resultNode = xd2.SelectSingleNode("xml/isOk");
                XmlNode errorInfoNode = xd2.SelectSingleNode("xml/errorInfo");
                result.IsOK = Convert.ToBoolean(resultNode.InnerText);
                result.ReturnMessage = errorInfoNode.InnerText;
            }
            catch (Exception ex)
            {
                ISendMessageService service = ServiceFactory.GetInstance();
                ExceptionLog log = new ExceptionLog()
                {
                    MessageType = "wechat bing account send sms error",
                    ExceptionInfo = ex
                };
                service.RecordExceptionLog(log);

                result.IsOK = false;
                result.ReturnMessage = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// generate pin code
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private string GeneratePinCode(string mobile)
        {
            Random rm = new Random(DateTime.Now.Millisecond);
            int pinCode = rm.Next(1000,9999);

            if (HttpRuntime.Cache[mobile] != null)
            {
                HttpRuntime.Cache[mobile] = null;
            }

            HttpRuntime.Cache.Add(mobile, pinCode, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);

            return pinCode.ToString();
        }

        public static Dictionary<string,object> ConvertJsonToObject(string json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            object obj = jsonSerializer.DeserializeObject(json);
            Dictionary<string, object> dicObj = obj as Dictionary<string, object>;
            return dicObj;
        }
    }
}