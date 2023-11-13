using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Daimler.HELM.WechatInterface
{
    public class MenuJson
    {
        public List<MenuInfo> button { get; set; }

        public MenuJson()
        {
            button = new List<MenuInfo>();
        }


        internal string ToJson()
        {
            return JsonHelper.ListToJson(this.button,"button");
        }
    }
}
