using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class DhstRequestResultInfo
    {
        //msgid：该批短信编号；
        public string Msgid { get; set; }
        //result：该批短信提交结果
        public int Result { get; set; }
        //desc：result状态描述；
        public string Desc { get; set; }
        //blacklist：无效号码，如果提交的号码中含有黑名单或错误（格式）号码将在此显示。
        public string BlackList { get; set; }
    }
}
