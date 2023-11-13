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
    public class MMSQueue : BaseMessageQ<MessageInfo>
    {

        public MMSQueue(string machineName)
            : base(machineName, "mmsQue")
        {

        }



        public override void ThreadSendMessage(object msgObj)
        {
            base.ThreadSendMessage(msgObj);

            try
            {
                MessageInfo msgInfo = msgObj as MessageInfo;

                MMSHandler.SendMMSMessageToVendor(msgInfo);
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "send mms message in service"
                });
            }
        }


    }
}
