using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Logic;
using Daimler.HELM.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageQueueSend
{
    public class SMSQueue : BaseMessageQ<MessageInfo>
    {
        public SMSQueue(string machineName)
            : base(machineName,"smsQue")
        {

        }

        public override void ThreadSendMessage(object msgObj)
        {
            base.ThreadSendMessage(msgObj);

            try
            {
                MessageInfo msgInfo = msgObj as MessageInfo;
                SMSHandler.SendMessageToVendor(msgInfo,false);
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog() { 
                    ExceptionInfo = ex,
                    MessageType="send sms message in service"
                });
            }
        }
    }
}
