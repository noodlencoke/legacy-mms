using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Daimler.HELM.WechatInterface
{
    public class WechatService:IWechatService
    {
        private static readonly string useProxy = System.Configuration.ConfigurationManager.AppSettings["UseProxy"];
        private static readonly string proxyAddr = System.Configuration.ConfigurationManager.AppSettings["ProxyAddress"];
        private static readonly string proxyPort = System.Configuration.ConfigurationManager.AppSettings["ProxyPort"];

        private string _appId;
        private string _secret;


        public WechatService(string appId, string secret)
        {
            _appId = appId;
            _secret = secret;
        }

        public WeChatResponse GetUserInfo(string accessToken,string openId)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN",accessToken,openId);
            WeChatResponse response = GetDataFromWeChat(url);
            return response;
        }

        public WeChatResponse GetOAuthAccessToken(string code)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",_appId,_secret,code);
            WeChatResponse response = GetDataFromWeChat(url);
            return response;
        }

        public WeChatResponse PushTemplateMsg(string accessToken, string postData)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accessToken);
            WeChatResponse response = PostDataToWeCaht(url, postData);
            return response;
        }

        public WeChatResponse GetAccessToken()
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", _appId, _secret);
            WeChatResponse response = GetDataFromWeChat(url);
            return response;
        }

        private WeChatResponse GetDataFromWeChat(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=utf-8";
            if (Convert.ToBoolean(useProxy))
            {
                System.Net.WebProxy proxy = new WebProxy(proxyAddr, Convert.ToInt32(proxyPort));
                request.Proxy = proxy;
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            WeChatResponse Token = JsonDeserializeBySingleData<WeChatResponse>(retString);
            return Token;
        }

        private WeChatResponse PostDataToWeCaht(string url, string postdata)
        {
            WeChatResponse ReturnMsg = new WeChatResponse();
            ReturnMsg.errcode = 0;
            ReturnMsg.errmsg = "ok";
            try                                                                                                             
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                if (Convert.ToBoolean(useProxy))
                {
                    System.Net.WebProxy proxy = new WebProxy(proxyAddr, Convert.ToInt32(proxyPort));
                    webReq.Proxy = proxy;
                }
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string message = sr.ReadToEnd();

                ReturnMsg = JsonDeserializeBySingleData<WeChatResponse>(message);

                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                ReturnMsg.errmsg = ex.Message.ToString();
                ReturnMsg.errcode = -1;
            }

            return ReturnMsg;
        }

        private static T JsonDeserializeBySingleData<T>(string JsonString)
        {
            string jsonString = JsonString;
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式 
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        public bool CreateMenu(string accessToken, MenuJson menuJson)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", accessToken);
            string postData = menuJson.ToJson();
            postData = postData.Replace("\\", "");
            postData = postData.Replace("sub_button\":\"", "sub_button\":");
            postData = postData.Replace("}]\"}", "}]}");
            WeChatResponse result = PostDataToWeCaht(url, postData);
            Console.WriteLine(result.errmsg);
            if (result.errcode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
