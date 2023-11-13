using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class BatchMessage
    {
        public List<MessageInfo> MessageList { get; set; }

        public MessagePriority Priority { get; set; }
    }
}
