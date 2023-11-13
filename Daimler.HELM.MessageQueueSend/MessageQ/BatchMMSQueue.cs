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
    public class BatchMMSQueue : BaseMessageQ<BatchMessage>
    {

        public BatchMMSQueue(string machineName)
            : base(machineName, "batchMmsQue")
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
                    MMSHandler.SendBatchMMSMessageToVendor(batchMessage.MessageList);
                }
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "send batch mms message in service"
                });
            }
        }
    }
}
