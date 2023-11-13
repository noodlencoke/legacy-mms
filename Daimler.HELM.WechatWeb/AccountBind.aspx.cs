using Daimler.HELM.BizObjects;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.WechatInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.WechatWeb
{
    public partial class AccountBind : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string code = Request.QueryString["code"];
                //code = "1213dskfjoi3lsakjdoqe;sajdaps";
                ViewState["code"] = code;
            }
        }


        public WeChatResponse GetUserInfo(string appId,string secret,string code)
        {
            try
            {
                IWechatService wechatService = WechatServiceFactory.GetInstance(appId, secret);
                WeChatResponse response = wechatService.GetOAuthAccessToken(code);
                string token = response.access_token;
                string openId = response.openid;
                string refreshtoken = response.refresh_token;

                WeChatResponse userInfo = wechatService.GetUserInfo(token, openId);

                return userInfo;

                //WeChatResponse response = new WeChatResponse();
                //response.openid = Guid.NewGuid().ToString();
                //response.nickname = "bobo";
                //response.sex = "1";
                //response.province = "bj";
                //response.city = "chaoyang";
                //response.country = "china";
                //response.unionid = Guid.NewGuid().ToString();
                //return response;

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message+ex.StackTrace);
            }
            return null;
        }

        protected void hdBtnGetPinCode_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(),null,"<script>alert('aa')</script>");
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string pinCode = this.id_PinCode.Value;
            string mobile = this.txtMobile.Value;

            if (string.IsNullOrEmpty(mobile))
            {
                this.labErrorMsg.InnerText = "请输入验证码！";
                return; 
            }

            if (string.IsNullOrEmpty(pinCode.Trim()))
            {
                this.labErrorMsg.InnerText = "请输入验证码！";
                return;
            }

            if (!this.id_check.Checked)
            {
                this.labErrorMsg.InnerText = "请确定是否已阅读并同意《戴姆勒信息安全协议》";
                return;
            }

            if (Cache[mobile] == null || Cache[mobile].ToString() != pinCode.Trim())
            {
                this.labErrorMsg.InnerText = "验证码错误或已过期，请重新获取！";
                return;
            }


            string appId = System.Configuration.ConfigurationManager.AppSettings["AppId"];
            string secret = System.Configuration.ConfigurationManager.AppSettings["Secret"];

            string code = ViewState["code"].ToString();
            WeChatResponse response = GetUserInfo(appId,secret,code);
            response.mobile = mobile;
            CommonResult result = ServiceFactory.GetInstance().BindAccount(response);
            if (result.IsOK)
            {
                this.labErrorMsg.InnerText = "绑定成功！";
                if (Cache[mobile] != null)
                {
                    Cache.Remove(mobile);
                }
            }
        }                                                                         
    }
}