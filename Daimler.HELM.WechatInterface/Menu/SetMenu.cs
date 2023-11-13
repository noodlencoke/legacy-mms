using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.WechatInterface
{
    public static class SetMenu
    {
        public static Boolean CreateMenuOfWeChat(string appId,string secret)
        {
            MenuInfo Menu1 = new MenuInfo("用户绑定", ButtonType.View, "http://112.81.47.8:8086/oauth");

            MenuJson menuJson = new MenuJson();
            menuJson.button.AddRange(new MenuInfo[] { Menu1});

            IWechatService wechatservice = WechatServiceFactory.GetInstance(appId,secret);
            string Token = wechatservice.GetAccessToken().access_token;
            bool result = wechatservice.CreateMenu(Token, menuJson);
            return result;
        }
    }
}
