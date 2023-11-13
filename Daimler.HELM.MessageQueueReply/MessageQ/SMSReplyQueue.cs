using Daimler.HELM.Adapter.Logic;
using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using Daimler.HELM.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageQueueReply
{
    public class SMSReplyQueue:BaseMessageQ<MessageReplyInfo>
    {
        public SMSReplyQueue(string machineName)
            : base(machineName,"smsReplyQue")
        {

        }

        public override void ThreadSendMessage(object replyObj)
        {
            base.ThreadSendMessage(replyObj);
            
            MessageReplyInfo replyInfo = replyObj as MessageReplyInfo;
            try
            {

                BaseReplyHandler replyHandler = MessageReplyFactory.CreateMessageReply(replyInfo.dataSource);
                CommonResult result = replyHandler.DoReply(replyInfo);
                if (result.IsOK)
                {
                    SMSHandler.UpdateReplyInfoStatus(replyInfo, 1);
                }
            }
            catch (Exception ex)
            {
                replyInfo.statusDesc = ex.Message+ex.StackTrace;
                SMSHandler.UpdateReplyInfoStatus(replyInfo,2);
            }
        }
    }
}
