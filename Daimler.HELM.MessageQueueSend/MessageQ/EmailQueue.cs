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
    public class EmailQueue : BaseMessageQ<MessageInfo>
    {
        public EmailQueue(string machineName)
            : base(machineName, "edmQue")
        {

        }

        public override void ThreadSendMessage(object msgObj)
        {
            base.ThreadSendMessage(msgObj);

            try
            {
                MessageInfo msgInfo = msgObj as MessageInfo;

                EmailHandler.SendEmail(msgInfo);
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "send email message in service"
                });
            }
        }
    }
}
