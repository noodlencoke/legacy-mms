using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class ShowMessageReplyInfo : MessageReplyInfo
    {
        public string MessageType { get; set; }
        public string CreatedDt { get; set; }
        public string StartDt { get; set; }
        public string EndDt { get; set; }
    }
}
