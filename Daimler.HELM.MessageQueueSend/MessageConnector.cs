using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Logic;
using Daimler.HELM.MQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageQueueSend
{
    public partial class MessageConnector : ServiceBase
    {

        SMSQueue smsQue = null;
        EmailQueue emailQue = null;
        MMSQueue mmsQue = null;

        BatchSMSQueue batchSmsQue = null;
        BatchMMSQueue batchMmsQue = null;
        BatchEmailQueue batchEmailQue = null;

        string hubServer = null;



        public MessageConnector()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                hubServer = ConfigurationManager.AppSettings["HubServer"];

                Thread smsTh = new Thread(SMSReceive);
                smsTh.Start();

                Thread emailTh = new Thread(EmailReceive);
                emailTh.Start();

                Thread mmsTh = new Thread(MMSReceive);
                mmsTh.Start();

                Thread batchSmsTh = new Thread(BatchSMSReceive);
                batchSmsTh.Start();

                Thread batchMmsTh = new Thread(BatchMMSReceive);
                batchMmsTh.Start();

                Thread batchEmailTh = new Thread(BatchEmailReceive);
                batchEmailTh.Start();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "start send message service"
                });
            }
        }

        protected override void OnStop()
        {
            try
            {
                smsQue.DisListen();

                emailQue.DisListen();

                mmsQue.DisListen();

                batchSmsQue.DisListen();

                batchMmsQue.DisListen();

                batchEmailQue.DisListen();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "stop send message service"
                });
            }
        }


        private void SMSReceive()
        {
            try
            {
                smsQue = new SMSQueue(hubServer);
                smsQue.Listen();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "start listen sms message queue in service"
                });
            }
        }

        private void EmailReceive()
        {
            try
            {
                emailQue = new EmailQueue(hubServer);
                emailQue.Listen();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "start listen email message queue in service"
                });
            }
        }


        private void MMSReceive()
        {
            try
            {
                mmsQue = new MMSQueue(hubServer);
                mmsQue.Listen();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "start listen MMS message queue in service"
                });
            }
        }

        private void BatchSMSReceive()
        {
            try
            {
                batchSmsQue = new BatchSMSQueue(hubServer);
                batchSmsQue.Listen();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "start listen batch sms message queue in service"
                });
            }
        }

        private void BatchMMSReceive()
        {
            try
            {
                batchMmsQue = new BatchMMSQueue(hubServer);
                batchMmsQue.Listen();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "start listen batch mms message queue in service"
                });
            }
        }

        private void BatchEmailReceive()
        {
            try
            {
                batchEmailQue = new BatchEmailQueue(hubServer);
                batchEmailQue.Listen();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "start listen batch email message queue in service"
                });
            }
        }
    }
}
