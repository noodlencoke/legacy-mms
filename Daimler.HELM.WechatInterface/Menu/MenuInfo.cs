using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.WechatInterface
{
    /// <summary>
    /// 菜单基本信息
    /// </summary> 
   
    public class MenuInfo
    {
        /// <summary>
        /// 按钮描述，既按钮名字，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 按钮类型（click或view）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        /// <summary>
        /// 按钮KEY值，用于消息接口(event类型)推送，不超过128字节
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string key { get; set; }

        /// <summary>
        /// 网页链接，用户点击按钮可打开链接，不超过256字节
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }

        /// <summary>
        /// 子按钮数组，按钮个数应为2~5个
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sub_button { get; set; }

        /// <summary>
        /// 参数化构造函数
        /// </summary>
        /// <param name="name">按钮名称</param>
        /// <param name="buttonType">菜单按钮类型</param>
        /// <param name="value">按钮的键值（Click)，或者连接URL(View)</param>
        public MenuInfo(string name, string buttonType, string value)
        {
            this.name = name;
            this.type = buttonType.ToString();

            if (buttonType == ButtonType.Click)
            {
                this.key = value;
            }
            else if (buttonType == ButtonType.View)
            {
                this.url = value;
            }
        }

        /// <summary>
        /// 参数化构造函数,用于构造子菜单
        /// </summary>
        /// <param name="name">按钮名称</param>
        /// <param name="sub_button">子菜单集合</param>
        public MenuInfo(string name, IEnumerable<MenuInfo> sub_button)
        {
            this.name = name;
            string sub_button_json = JsonHelper.ToJson(sub_button);
                 
            this.sub_button = sub_button_json;
        }
        public MenuInfo()
        { }

    }
}
