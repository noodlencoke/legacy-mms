using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class DhstSendResultInfo
    {
        //result：接口调用结果
        public int Result { get; set; }
        public string Desc { get; set; }
        //发送报告
        public List<DhstSendReportInfo> SendResportList { get; set; }
    }
}
