using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class DhstSendReportInfo
    {
        //msgid：短信编号；
        public string Msgid { get; set; }
        //phone：下行手机号码；
        public string Phone { get; set; }
        //status：短信发送结果：0——成功；1——接口处理失败；2——运营商网关失败；
        public int Status { get; set; }
        //desc：当status为1时，以desc的错误码为准
        public string Desc { get; set; }
        //wgcode：当status为2时，表示运营商网关返回的原始值
        public string WgCode { get; set; }
        //状态报告接收时间格式为yyyy-MM-ddHH:mm:ss。
        public string Time { get; set; }

    }
}
