using Daimler.HELM.Adapter.Logic;
using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageQueueReply
{
    public partial class SourceChannerReply : ServiceBase
    {

        SMSReplyQueue smsReplyQue = null;
        string hubServer = null;
        object repeatSendReplyInfoObj = new object();
        bool isRunRepeatSendReplyInfo = false;



        public SourceChannerReply()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                hubServer = ConfigurationManager.AppSettings["HubServer"];

                Thread smsTh = new Thread(SMSReplyToBS);
                smsTh.Start();

                //repeat send failed reply information
                isRunRepeatSendReplyInfo = true;
                Thread repeatTh = new Thread(RepeatReply);
                repeatTh.Start();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "start Reply Service"
                });
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (smsReplyQue != null)
                {
                    smsReplyQue.DisListen();
                }

                isRunRepeatSendReplyInfo = false;
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "stop Reply Service"
                });
            }
        }

        private void SMSReplyToBS()
        {
            try
            {
                smsReplyQue = new SMSReplyQueue(hubServer);
                smsReplyQue.Listen();
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    ExceptionInfo = ex,
                    MessageType = "listen Reply message"
                });
            }
        }


        private void RepeatReply()
        {
            lock (repeatSendReplyInfoObj)
            {
                string runPeriod = System.Configuration.ConfigurationManager.AppSettings["RepeatSendReplyInfoPeriod"];
                while (isRunRepeatSendReplyInfo)
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(runPeriod));
                        if (isRunRepeatSendReplyInfo)
                        {
                            RepeatReplyToBS();
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHandler.RecordExceptionLog(new ExceptionLog()
                        {
                            ExceptionInfo = ex,
                            MessageType = "repeat send Reply message"
                        });
                    }
                }
            }
        }


        public static void RepeatReplyToBS()
        {
            List<MessageReplyInfo> replyInfoList = SMSHandler.GetNeedRepeatSendReplyList();
            foreach (MessageReplyInfo replyInfo in replyInfoList)
            {
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
                    replyInfo.statusDesc = ex.Message;
                    SMSHandler.UpdateReplyInfoStatus(replyInfo, 2);
                }
                finally
                {
                    SMSHandler.UpdateSMSRepeatReplyTimes(replyInfo.Id);
                }
            }
        }
    }
}
