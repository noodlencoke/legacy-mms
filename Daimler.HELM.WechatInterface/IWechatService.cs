using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.WechatInterface
{
    public interface IWechatService
    {
        WeChatResponse PushTemplateMsg(string accessToken, string postData);

        WeChatResponse GetAccessToken();

        WeChatResponse GetOAuthAccessToken(string code);

        WeChatResponse GetUserInfo(string accessToken, string openId);

        bool CreateMenu(string accessToken, MenuJson menuJson);
    }

    public class MockWechatService:IWechatService
    {
        public WeChatResponse PushTemplateMsg(string accessToken, string postData)
        {
            throw new NotImplementedException();
        }

        public WeChatResponse GetAccessToken()
        {
            throw new NotImplementedException();
        }


        public WeChatResponse GetOAuthAccessToken(string code)
        {
            throw new NotImplementedException();
        }


        public WeChatResponse GetUserInfo(string accessToken, string openId)
        {
            throw new NotImplementedException();
        }


        public bool CreateMenu(string accessToken, MenuJson menuJson)
        {
            throw new NotImplementedException();
        }
    }

    public static class WechatServiceFactory { 
        public static IWechatService GetInstance(string appId,string secret)
        {
            string mock = System.Configuration.ConfigurationManager.AppSettings["isMock"];
            if (mock == "1")
            {
                return new MockWechatService();
            }
            else {
                return new WechatService(appId,secret);
            }

        }
    }
}
