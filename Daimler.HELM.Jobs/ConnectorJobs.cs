using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daimler.HELM.BizObjects;

namespace Daimler.HELM.Jobs
{
    partial class Jobs : ServiceBase
    {
        private bool isRunRepeatSend = false;
        private bool isRunBathSend = false;
        private bool isRunGetMessageSendingStatus = false;
        private bool isRunGetReplyMessage = false;

        private readonly object repeatObj = new object();
        private readonly object batchSendObj = new object();
        private readonly object getMessageSendingStatusObj = new object();
        private readonly object getReplyMessageObj = new object();

        public Jobs()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            isRunRepeatSend = true;
            isRunBathSend = false;
            isRunGetMessageSendingStatus = true;
            isRunGetReplyMessage = true;

            //repeat
            Thread repeatTh = new Thread(RepeatSend);
            repeatTh.Start();

            //send batch message
            Thread batchTh = new Thread(BatchSend);
            batchTh.Start();

            //get message sending status 
            Thread getMessageSendingStatusTh = new Thread(GetMessageSendingStatus);
            getMessageSendingStatusTh.Start();

            //get reply message
            Thread getReplyMessageTh = new System.Threading.Thread(GetReplyMessage);
            getReplyMessageTh.Start();

        }

        protected override void OnStop()
        {
            isRunRepeatSend = false;
            isRunBathSend = false;
            isRunGetMessageSendingStatus = false;
            isRunGetReplyMessage = false;
            Log.LogHandler.WriteLog("任务停止");
        }


        private void RepeatSend()
        {
            lock (repeatObj)
            {
                string runPeriod = System.Configuration.ConfigurationManager.AppSettings["RepeatPeriod"];
                while (isRunRepeatSend)
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(runPeriod));
                        if (isRunRepeatSend)
                        {
                            SendMessageHandler.RepeatSendSMS(); 
                            SendMessageHandler.RepeatSendMMS();
                            SendMessageHandler.RepeatSendEmail();


                            Log.LogHandler.WriteLog("重发结束");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHandler.RecordExceptionLog(new ExceptionLog() { 
                            ExceptionInfo = ex,
                            MessageType="repeat send message"
                        });
                    }
                }
            }
        }

        private void BatchSend()
        {
            lock (batchSendObj)
            {
                string runPeriod = System.Configuration.ConfigurationManager.AppSettings["BatchSendPeriod"];
                while (isRunBathSend)
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(runPeriod));
                        if (isRunBathSend)
                        {
                            SendMessageHandler.BatchSendSMSMessage();

                            SendMessageHandler.BatchSendEmail();

                            SendMessageHandler.BatchSendMMS();

                            Log.LogHandler.WriteLog("批量发送信息结束");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHandler.RecordExceptionLog(new ExceptionLog()
                        {
                            ExceptionInfo = ex,
                            MessageType = "batch send message"
                        });
                    }
                }
            }
        }


        private void GetMessageSendingStatus()
        {
            lock (getMessageSendingStatusObj)
            {
                string runPeriod = System.Configuration.ConfigurationManager.AppSettings["GetSingleStatusPeriod"];
                while (isRunGetMessageSendingStatus)
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(runPeriod));
                        if (isRunGetMessageSendingStatus)
                        {
                            SendMessageHandler.GetSMSSendStatus();

                            SendMessageHandler.GetMMSSendStatus();

                            Log.LogHandler.WriteLog("获取信息发送状态");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHandler.RecordExceptionLog(new ExceptionLog()
                        {
                            ExceptionInfo = ex,
                            MessageType = "get single message sending status"
                        });
                    }
                }
            }
        }

        private void GetReplyMessage()
        {
            lock (getReplyMessageObj)
            {
                string runPeriod = System.Configuration.ConfigurationManager.AppSettings["GeReplyMessagePeriod"];
                while (isRunGetReplyMessage)
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(runPeriod));
                        if (isRunGetReplyMessage)
                        {
                            //get date
                            SendMessageHandler.GetReplyInfo(null, null);
                            
                            Log.LogHandler.WriteLog("获取短信回复内容结束");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHandler.RecordExceptionLog(new ExceptionLog() { 
                            ExceptionInfo = ex,
                            MessageType="get sms reply info"
                        });
                    }
                }
            }
        }
    }
}
