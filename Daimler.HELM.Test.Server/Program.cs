using Daimler.HELM.BizObjects;
using Daimler.HELM.MessageQueueSend;
using Daimler.HELM.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.Test.Server
{
    class Program
    {
        public static SMSQueue smsQue = null;
        public static EmailQueue emailQue = null;
        public static MMSQueue mmsQue = null;
        public static BatchSMSQueue batchSmsQue = null;
        public static BatchEmailQueue batchEdmQue = null;

        public static Daimler.HELM.MessageQueueSend.BatchMMSQueue batchMmsQue = null;

        public static string hubServer = System.Configuration.ConfigurationManager.AppSettings["HubServer"];

        static void Main(string[] args)
        {
            smsQue = new SMSQueue(hubServer);

            //BaseMessageQ<MessageInfo> smsQueMsg = new BaseMessageQ<MessageInfo>(null, "smsQue");
           

            //for (int i = 0; i < 1000; i++)
            //{
            //    MessageInfo msgInfo = new MessageInfo()
            //    {
            //      Id = Guid.NewGuid(),
            //      Number = "15611100253_"+i,
            //      Content = "Test SMS sending",
            //      Priority = System.Messaging.MessagePriority.Highest
            //    };
            //    smsQueMsg.ReceiveMessage(msgInfo, msgInfo.Priority);
            //}

            smsQue.Listen();

            emailQue = new EmailQueue(hubServer);
            emailQue.Listen();

            mmsQue = new MMSQueue(hubServer);
            mmsQue.Listen();

            batchSmsQue = new BatchSMSQueue(hubServer);
            batchSmsQue.Listen();

            batchMmsQue = new MessageQueueSend.BatchMMSQueue(hubServer);
            batchMmsQue.Listen();

            batchEdmQue = new BatchEmailQueue(hubServer);
            batchEdmQue.Listen();

            Console.ReadLine();
        }
    }
}
