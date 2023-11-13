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
    public class BatchEmailQueue : BaseMessageQ<BatchMessage>
    {
        public BatchEmailQueue(string machineName)
            : base(machineName, "batchEdmQue")
        {

        }

        public override void ThreadSendMessage(object msgObj)
        {
            base.ThreadSendMessage(msgObj);

            try
            {
                BatchMessage batchMessage = msgObj as BatchMessage;
                if (batchMessage.MessageList.Count > 0)
                {
                    EmailHandler.SendBatchEmailMessageToVendor(batchMessage.MessageList);
                }
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "send batch email message in service"
                });
            }
        }
    }
}
