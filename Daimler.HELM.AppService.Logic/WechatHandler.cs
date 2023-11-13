using Daimler.HELM.BizObjects;
using Daimler.HELM.DBInterface;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.WechatInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Daimler.HELM.HubService.Logic
{
    public static class WechatHandler
    {
        public static bool PushWechatTemplateMessage(string mobile, string templateId, Dictionary<string, object> dicPostData)
        {

            IWechatDal wechatDal = WechatDalFactory.GetInstance();
            List<CustomerInfo> customerInfoList = wechatDal.GetCustomerInfoByMobileNumber(mobile);

            string appId = System.Configuration.ConfigurationManager.AppSettings["AppId"];
            string secret = System.Configuration.ConfigurationManager.AppSettings["Secret"];
            IWechatService wechatService = WechatServiceFactory.GetInstance(appId,secret);

            string accessToken = GetAccessToken(wechatService);
            bool bol = true;

            foreach (CustomerInfo customerInfo in customerInfoList)
            {
                string strData = string.Empty;
                string wechatUserId = customerInfo.WechatId;
                foreach (string key in dicPostData.Keys)
                {
                    strData += "\"" + key + "\":{"
                                +"\"value\":\"" + dicPostData[key].ToString() + "\","
                                +"\"color\":\"#173177\""
                               +"},";
                }
                strData = strData.Substring(0, strData.Length-1).Replace("\r","");
                string postData = "{"
                               + "\"touser\":\"" + wechatUserId + "\","
                               + "\"template_id\":\"" + templateId + "\","
                               + "\"url\":\"http://www.baidu.com/\","
                               + "\"data\":{"
                                      + strData
                                   + "}"
                               + "}";
                WeChatResponse result = wechatService.PushTemplateMsg(accessToken, postData);
                if (result.errmsg != "ok")
                {
                    bol = false;
                }
            }
            return bol;
        }

        private static string GetAccessToken(IWechatService wechatService)
        {
            string cacheKey = "accessToken";
            string accessToken = string.Empty;
            if (HttpRuntime.Cache[cacheKey] != null)
            {
                accessToken = HttpRuntime.Cache[cacheKey].ToString();
            }
            else
            {
                WeChatResponse response = wechatService.GetAccessToken();
                accessToken = response.access_token;
                HttpRuntime.Cache.Add(cacheKey, accessToken, null, DateTime.Now.AddHours(1.5), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            return accessToken;
        }


        public static CommonResult BindAccount(WeChatResponse userInfo)
        {
            CommonResult result = new CommonResult();
            result.IsOK = false;
            try
            {
                Dictionary<string, object> dicCustomerInfo = new Dictionary<string, object>();
                dicCustomerInfo.Add("openId", userInfo.openid);
                dicCustomerInfo.Add("mobile",userInfo.mobile);
                dicCustomerInfo.Add("name", userInfo.nickname);
                dicCustomerInfo.Add("sex", userInfo.sex);
                dicCustomerInfo.Add("province", userInfo.province);
                dicCustomerInfo.Add("city", userInfo.city);
                dicCustomerInfo.Add("country", userInfo.country);
                dicCustomerInfo.Add("headimgurl", userInfo.headimgurl);
                dicCustomerInfo.Add("unionid", userInfo.unionid);
                dicCustomerInfo.Add("bindDt",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

                IWechatDal wechatDal = WechatDalFactory.GetInstance();
                result.IsOK = wechatDal.AddCustomerInfo(dicCustomerInfo);
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "BindAccount error",
                    ExceptionInfo = ex
                });
            }
            return result;
        }
    }
}
