using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Logic;
using Daimler.HELM.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageQueueSend
{
    public class BatchSMSQueue : BaseMessageQ<BatchMessage>
    {
        public BatchSMSQueue(string machineName)
            : base(machineName,"batchSmsQue")
        {
            useThreadPool = true;
        }

        public override void ThreadSendMessage(object msgObj)
        {
            base.ThreadSendMessage(msgObj);

            try
            {
                BatchMessage batchMessage = msgObj as BatchMessage;
                foreach (MessageInfo msgInfo in batchMessage.MessageList)
                {
                    SMSHandler.SendMessageToVendor(msgInfo);
                }
                //if (batchMessage.MessageList.Count > 0)
                //{
                //    SMSHandler.SendBatchSMSMessageToVendor(batchMessage.MessageList);
                //}
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "send batch sms message in service"
                });
            }


        }
    }
}
