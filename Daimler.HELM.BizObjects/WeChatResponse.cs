using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    [DataContract]
    public class WeChatResponse
    {
        /// <summary>
        /// 微信返回的错误代码，0为正确
        /// </summary>
        [DataMember]
        public int errcode { get; set; }
        /// <summary>
        /// 微信返回的错误消息，ok为正确
        /// </summary>
        [DataMember]
        public string errmsg { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        [DataMember]
        public string template_id { get; set; }
        /// <summary>
        /// 返回发送的模板消息Id
        /// </summary>
        [DataMember]
        public string msgid { get; set; }

        #region 获取access_token返回参数
        /// <summary>
        /// 返回的access_token
        /// </summary>
        [DataMember]
        public string access_token { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>     
        [DataMember]
        public string expires_in { get; set; }

        [DataMember]
        public string refresh_token { get; set; }
        [DataMember]
        public string openid { get; set; }
        [DataMember]
        public string scope { get; set; }
        [DataMember]
        public string unionid { get; set; }
        #endregion

        #region userInfo
        [DataMember]
        public string nickname { get; set; }
        [DataMember]
        public string sex { get; set; }
        [DataMember]
        public string province { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string headimgurl { get; set; }

        [DataMember]
        public string mobile { get; set; }

        #endregion
    }
}
