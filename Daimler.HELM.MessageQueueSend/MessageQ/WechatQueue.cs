using Daimler.HELM.BizObjects;
using Daimler.HELM.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageQueueSend
{
    public class WechatQueue:BaseMessageQ<MessageInfo>
    {
        public WechatQueue(string machineName)
            : base(machineName, "wechatQue")
        {
           
        }

        public override void ThreadSendMessage(object msgObj)
        {
            base.ThreadSendMessage(msgObj);

            MessageInfo msgInfo = msgObj as MessageInfo;
            string msg = string.Format("微信：{0}", msgInfo.Content);
            Log.LogHandler.WriteLog("Wechat",msg);
        }
    }
}
