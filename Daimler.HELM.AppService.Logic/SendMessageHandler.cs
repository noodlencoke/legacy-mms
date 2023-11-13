using Daimler.HELM.BizObjects;
using Daimler.HELM.DBInterface;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.MessageFilter;
using Daimler.HELM.MQ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.HubService.Logic
{
    public static class SendMessageHandler
    {
        #region Single

        private static string machineName = System.Configuration.ConfigurationManager.AppSettings["HubServer"];
        public static CommonResult SendSingleSMS(MessageInfo msgInfo)
        {
            CommonResult result = new CommonResult();
            result.IsOK = false;
            try
            {
                bool isValidate;
                msgInfo.Classification = MessageClassification.Single;
                msgInfo = CommonHandler.ValidateMessage(msgInfo, out isValidate);
                if (msgInfo != null)
                {
                    if (isValidate)
                    {
                        CommonResult insertResult = SMSHandler.RecordSMSSendingInfo(msgInfo,false);
                        if (insertResult.IsOK)
                        {
                            BaseMessageQ<MessageInfo> smsQue = new BaseMessageQ<MessageInfo>(machineName, "smsQue");
                            smsQue.ReceiveMessage(msgInfo, msgInfo.Priority);
                            result.IsOK = true;
                        }
                    }
                    else {
                       CommonResult insertResult = SMSHandler.RecordSMSSendingInfo(msgInfo, true);
                       result.IsOK = insertResult.IsOK;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;

                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageId = msgInfo.Id,
                    MessageType = "send single sms",
                    ExceptionInfo = ex
                });
            }
            return result;
        }

        public static CommonResult SendSingleMMS(MessageInfo msgInfo)
        {
            CommonResult result = new CommonResult();
            result.IsOK = false;
            try
            {
                bool isValidate;
                msgInfo.Classification = MessageClassification.Single;
                msgInfo.TemplateId = msgInfo.Content;
                MessageInfo newMessageInfo = CommonHandler.ValidateMessage(msgInfo, out isValidate);
                if (newMessageInfo != null)
                {
                    if (isValidate)
                    {
                        CommonResult rt = MMSHandler.RecordMMSSendingInfo(newMessageInfo,false);
                        if (rt.IsOK)
                        {
                            BaseMessageQ<MessageInfo> mmsQue = new BaseMessageQ<MessageInfo>(machineName, "mmsQue");
                            mmsQue.ReceiveMessage(msgInfo, msgInfo.Priority);
                            result.IsOK = true;
                        }
                    }
                    else {
                        CommonResult rt = MMSHandler.RecordMMSSendingInfo(newMessageInfo, true);
                        result.IsOK = rt.IsOK;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;

                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageId = msgInfo.Id,
                    MessageType = "send single mms",
                    ExceptionInfo = ex
                });
            }
            return result;
        }

        public static CommonResult SendSingleEDM(MessageInfo msgInfo)
        {
            CommonResult result = new CommonResult();
            try
            {
                bool isValidate;
                msgInfo.Classification = MessageClassification.Single;
                MessageInfo newMessageInfo = CommonHandler.ValidateMessage(msgInfo, out isValidate);
                if (newMessageInfo != null)
                {
                    if (isValidate)
                    {
                        CommonResult rt = EmailHandler.RecordEDMSendingInfo(newMessageInfo,false);
                        if (rt.IsOK)
                        {
                            BaseMessageQ<MessageInfo> edmQue = new BaseMessageQ<MessageInfo>(machineName, "edmQue");
                            edmQue.ReceiveMessage(msgInfo, msgInfo.Priority);
                            result.IsOK = true;
                        }
                    }
                    else {
                        CommonResult rt = EmailHandler.RecordEDMSendingInfo(newMessageInfo, true);
                        result.IsOK = rt.IsOK;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;

                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageId = msgInfo.Id,
                    MessageType = "send single email",
                    ExceptionInfo = ex
                });
            }
            return result;
        }


        public static CommonResult SendSingleWechat(MessageInfo msgInfo)
        {
            return null;
        }

        #endregion

        #region Batch
        /// <summary>
        /// send batch message
        /// </summary>
        /// <param name="msgInfoList"></param>
        /// <returns></returns>
        public static CommonResult SendBatchSMS(List<MessageInfo> msgInfoList)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;

            BatchMessage batchMessage;
            List<MessageInfo> errorMsgList;
            CommonHandler.ValidateBatchMessage(msgInfoList, out batchMessage, out errorMsgList);

            if (errorMsgList.Count > 0)
            {
               CommonResult rt = SMSHandler.BatchRecordSMSInfo(errorMsgList,true);
               result.IsOK = rt.IsOK;
            }

            if (batchMessage.MessageList.Count > 0)
            {
                CommonResult batchResult = SMSHandler.BatchRecordSMSInfo(batchMessage.MessageList,false);
                if (batchResult.IsOK)
                {
                    batchMessage.Priority = batchMessage.MessageList[0].Priority;
                    BaseMessageQ<BatchMessage> batchSmsQue = new BaseMessageQ<BatchMessage>(machineName, "batchSmsQue");
                    batchSmsQue.ReceiveMessage(batchMessage, batchMessage.Priority);

                    result.IsOK = true;
                }
            }

            return result;
        }

        

        /// <summary>
        /// send mms batch message
        /// </summary>
        /// <param name="msgInfoList"></param>
        /// <returns></returns>
        public static CommonResult SendBatchMMS(List<MessageInfo> msgInfoList)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;

            BatchMessage batchMessage;
            List<MessageInfo> errorMsgList;
            CommonHandler.ValidateBatchMessage(msgInfoList, out batchMessage, out errorMsgList);


            if (errorMsgList.Count > 0)
            {
                CommonResult rt = MMSHandler.BatchRecordMMSInfo(errorMsgList, true);
                result.IsOK = rt.IsOK;
            }

            if (batchMessage.MessageList.Count > 0)
            {
                CommonResult rt = MMSHandler.BatchRecordMMSInfo(batchMessage.MessageList, false);
                if (rt.IsOK)
                {
                    batchMessage.Priority = batchMessage.MessageList[0].Priority;
                    BaseMessageQ<BatchMessage> batchSmsQue = new BaseMessageQ<BatchMessage>(machineName, "batchMmsQue");
                    batchSmsQue.ReceiveMessage(batchMessage, batchMessage.Priority);

                    result.IsOK = true;
                }
            }
            return result;
        }

        public static CommonResult SendBatchEDM(List<MessageInfo> msgInfoList)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;

            BatchMessage batchMessage;
            List<MessageInfo> errorMsgList;
            CommonHandler.ValidateBatchMessage(msgInfoList, out batchMessage, out errorMsgList);

            if (errorMsgList.Count > 0)
            {
                CommonResult rt = EmailHandler.BatchRecordEmailInfo(errorMsgList,true);
                result.IsOK = rt.IsOK;
            }

            if (batchMessage.MessageList.Count > 0)
            {
                CommonResult rt = EmailHandler.BatchRecordEmailInfo(batchMessage.MessageList,false);
                if (rt.IsOK)
                {
                    batchMessage.Priority = batchMessage.MessageList[0].Priority;
                    BaseMessageQ<BatchMessage> batchSmsQue = new BaseMessageQ<BatchMessage>(machineName, "batchEdmQue");
                    batchSmsQue.ReceiveMessage(batchMessage, batchMessage.Priority);

                    result.IsOK = true;
                }
            }

            return result;
        }

        public static CommonResult SendBatchWechat(List<MessageInfo> msgInfoList)
        {
            return null;
        }

        #endregion

        #region Get Message Status


        /// <summary>
        /// get sms send status
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public static List<MessageStateInfo> GetMessageStateInfos(string dataSource, string[] taskIds)
        {
            return SMSHandler.GetMessageStateInfos(dataSource, taskIds);
        }

        /// <summary>
        /// Get SMS、MMS Status  Each times 1000
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static IList<MessageStateInfo> GetMessageStateInfos(string dataSource)
        {
            ISMSDal smsDal = SMSDalFactory.GetInstance();
            IList<MessageStateInfo> messageStateList = smsDal.GetMessageStateInfos(dataSource);
            return messageStateList;
        }

        #endregion


        #region Batch Jobs


        /// <summary>
        /// batch send message
        /// </summary>
        public static void BatchSendSMSMessage()
        {
            try
            {
                List<MessageInfo> messagList = SMSHandler.GetSMSBatchMessage();
                //foreach (MessageInfo message in messagList)
                //{
                //    SendMessageToVendor(message);
                //}

                if (messagList.Count > 0)
                {
                    SMSHandler.SendBatchSMSMessageToVendor(messagList,false);
                }
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "batch send sms message",
                    ExceptionInfo = ex
                });
            }
        }


        public static void BatchSendMMS()
        {
            try
            {
                IMMSDal mmsDal = MMSDalFactory.GetInstance();
                List<MessageInfo> messagList = mmsDal.GetBatchMMSList();
                if (messagList.Count > 0)
                {
                    MMSHandler.SendBatchMMSMessageToVendor(messagList);
                }
                //foreach (MessageInfo message in messagList)
                //{
                //    SendMMSMessageToVendor(message);
                //}
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "batch send MMS message",
                    ExceptionInfo = ex
                });
            }
        }

        public static void BatchSendEmail()
        {
            //get batch email message
            IEmailDal emailDal = EmailDalFactory.GetInstance();
            List<MessageInfo> emailList = emailDal.GetBatchEmailList();
            if (emailList.Count > 0)
            {
                EmailHandler.SendBatchEmailMessageToVendor(emailList);
            }
            //foreach (MessageInfo message in emailList)
            //{
            //    SendEmail(message);
            //}
        }


        #endregion


        #region Get message sending status Jobs

        public static void GetSMSSendStatus()
        {
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                List<SendingResult> sendingResultList = SMSHandler.GetSMSSendingResult();
                ISMSDal smsDal = SMSDalFactory.GetInstance();
                foreach (SendingResult sendResult in sendingResultList)
                {
                    Dictionary<string, object> dicUpdateList = new Dictionary<string, object>();
                    dicUpdateList.Add("status", (int)sendResult.Status);
                    dicUpdateList.Add("sendDesc", sendResult.Desc);
                    dicUpdateList.Add("getStatusDt", time);
                    dicUpdateList.Add("sentDt", sendResult.SendTime);

                    Dictionary<string, object> dicConditions = new Dictionary<string, object>();
                    dicConditions.Add("vendorTaskId", sendResult.TaskId);
                    dicConditions.Add("mobile", sendResult.Number);
                    smsDal.UpdateSMSSendingStatus(dicConditions, dicUpdateList);
                }
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "get sms sending status",
                    ExceptionInfo = ex
                });
            }
        }


        public static void GetMMSSendStatus()
        {
            try {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                List<SendingResult> sendingResultList = MMSHandler.GetMMSSendingResult();
                IMMSDal mmsDal = MMSDalFactory.GetInstance();
                foreach (SendingResult sendResult in sendingResultList)
                {
                    Dictionary<string, object> dicUpdateList = new Dictionary<string, object>();
                    dicUpdateList.Add("status", (int)sendResult.Status);
                    dicUpdateList.Add("sendDesc", sendResult.Desc);
                    dicUpdateList.Add("getStatusDt", time);
                    dicUpdateList.Add("sentDt", sendResult.SendTime);

                    Dictionary<string, object> dicConditions = new Dictionary<string, object>();
                    dicConditions.Add("vendorTaskId", sendResult.TaskId);
                    dicConditions.Add("mobile", sendResult.Number);
                    mmsDal.UpdateMMSSendInfo(dicConditions, dicUpdateList);
                }
                
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "get mms sending status",
                    ExceptionInfo = ex
                }); 
            }
        }

        #endregion


        #region Repeat Jobs

        /// <summary>
        /// repead send failed message
        /// </summary>
        /// <returns></returns>
        public static CommonResult RepeatSendSMS()
        {
            CommonResult result = new CommonResult();
            try
            {
                ISMSDal smsDal = SMSDalFactory.GetInstance();
                MessageStatus[] status = new MessageStatus[]{
                  MessageStatus.SystemBusy
                };

                List<MessageInfo> messageList = smsDal.GetNeedRepeatSendSMS(status);
                foreach (MessageInfo msgObj in messageList)
                {
                    SMSHandler.SendMessageToVendor(msgObj);
                    SMSHandler.UpdateSMSRepeatSentTimes(msgObj.Id);
                }
                //SMSHandler.SendBatchSMSMessageToVendor(messageList,true);

            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "repeat sent sms message",
                    ExceptionInfo = ex
                });
            }
            return result;
        }

        public static void RepeatSendMMS()
        {
            try
            {
                IMMSDal mmsDal = MMSDalFactory.GetInstance();
                MessageStatus[] statusArry = new MessageStatus[] { 
                MessageStatus.SystemBusy
            };
                List<MessageInfo> msgList = mmsDal.GetNeedRepeatSendMMS(statusArry);
                foreach (MessageInfo msgInfo in msgList)
                {
                    MMSHandler.SendMMSMessageToVendor(msgInfo);
                    MMSHandler.UpdateMMSRepeatSentTimes(msgInfo.Id);
                }
                //MMSHandler.SendBatchMMSMessageToVendor(msgList,true);
            }
            catch (Exception ex)
            {
                throw new Exception("repeat send mms error." + ex.Message + "," + ex.StackTrace);
            }
        }


        public static void RepeatSendEmail()
        {
            IEmailDal emailDal = EmailDalFactory.GetInstance();
            List<MessageInfo> emailList = emailDal.GetNeedRepeatSendEmail(new EmailStatus[]{
                EmailStatus.SendError
            });
            //EmailHandler.SendBatchEmailMessageToVendor(emailList,true);
            foreach (MessageInfo msgInfo in emailList)
            {
                EmailHandler.SendEmail(msgInfo);
                EmailHandler.UpdateEmailRepeatSentTimes(msgInfo.Id);
            }
        }

        #endregion


        #region Reply Jobs


        public static CommonResult GetReplyInfo(string startDate, string endDate)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            try
            {
                List<MessageReplyInfo> replyMessageList = SMSHandler.GetSMSReplyMessageFromVendor(startDate, endDate);

                if (replyMessageList.Count > 0)
                {
                    DataTable dtReplyInfo = new DataTable();
                    dtReplyInfo.TableName = "SMSReplyInfo";
                    DataColumn[] columns = new DataColumn[] { 
                        new DataColumn("id"),
                        new DataColumn("messageId"),
                        new DataColumn("mobile"),
                        new DataColumn("content"),
                        new DataColumn("dataSource"),
                        new DataColumn("taskId"),
                        new DataColumn("extendNumber"), 
                        new DataColumn("status"), 
                        new DataColumn("replyDt"),
                        new DataColumn("senderNumber"),   
                        new DataColumn("messageType"),
                        new DataColumn("createdDt")
                    };
                    dtReplyInfo.Columns.AddRange(columns);


                    ISMSDal smsDal = SMSDalFactory.GetInstance();

                    foreach (MessageReplyInfo replyInfo in replyMessageList)
                    {
                        //get reply message from message
                        MessageInfo msgInfo = smsDal.GetReplyInfoFrom(replyInfo);
                        replyInfo.taskId = msgInfo.TaskId;
                        replyInfo.dataSource = msgInfo.DataSource;

                        DataRow row = dtReplyInfo.NewRow();
                        replyInfo.Id = Guid.NewGuid();
                        row["id"] = replyInfo.Id;
                        row["messageId"] = msgInfo.Id;
                        row["mobile"] = replyInfo.mobile;
                        row["content"] = replyInfo.content;
                        row["dataSource"] = replyInfo.dataSource;
                        row["taskId"] = replyInfo.taskId;
                        row["extendNumber"] = replyInfo.extendNumber;
                        row["status"] = 0;
                        row["replyDt"] = replyInfo.getTime;
                        row["senderNumber"] = replyInfo.senderNumber;
                        row["messageType"] = msgInfo.Type.ToString();
                        row["createdDt"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                        dtReplyInfo.Rows.Add(row);
                    }

                    //record sms reply message
                    bool bol = smsDal.RecordSMSReplyInfo(dtReplyInfo);

                    if (bol)
                    {
                        // send reply message to MSMQ
                        foreach (MessageReplyInfo replyInfo in replyMessageList)
                        {
                            if (!string.IsNullOrEmpty(replyInfo.taskId))
                            {
                                BaseMessageQ<MessageReplyInfo> smsQue = new BaseMessageQ<MessageReplyInfo>(machineName, "smsReplyQue");
                                smsQue.ReceiveMessage(replyInfo, System.Messaging.MessagePriority.Highest);
                            }
                        }
                    }
                    else
                    {
                        result.IsOK = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "get sms reply message",
                    ExceptionInfo = ex
                });
            }

            return result;



        }



        #endregion


        /// <summary>
        /// get sms reply info
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public static List<MessageReplyInfo> GetSMSReplyInfo(string dataSource, string startDateTime, string endDateTime)
        {
            return SMSHandler.GetSMSReplyMessageFromDB(dataSource, startDateTime, endDateTime);
        }


        /// <summary>
        /// get message info by page
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="dicConditions"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static DataTable GetMessageInfoByPage(string messageType, Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            if (messageType == "SMS")
            {
                Dictionary<string, object> newConditions = new Dictionary<string, object>();
                string[] arryParam = new string[] { "mobile", "content", "status", "taskId", "dataSource", "classification", "start_receivedDt", "end_receivedDt" };
                foreach (string param in arryParam)
                {
                    if (dicConditions.Keys.Contains(param))
                    {
                        newConditions.Add(param, dicConditions[param]);
                    }
                    else
                    {
                        newConditions.Add(param, DBNull.Value);
                    }
                }

                ISMSDal smsDal = SMSDalFactory.GetInstance();
                return smsDal.GetSMSInfoByPage(newConditions, pageSize, pageNumber, out count);
            }
            else if (messageType == "MMS")
            {
                Dictionary<string, object> newConditions = new Dictionary<string, object>();
                string[] arryParam = new string[] { "mobile", "templateId", "status", "taskId", "dataSource", "classification", "start_receivedDt", "end_receivedDt" };
                foreach (string param in arryParam)
                {
                    if (dicConditions.Keys.Contains(param))
                    {
                        newConditions.Add(param, dicConditions[param]);
                    }
                    else
                    {
                        newConditions.Add(param, DBNull.Value);
                    }
                }
                IMMSDal mmsDal = MMSDalFactory.GetInstance();
                return mmsDal.GetMMSInfoByPage(newConditions, pageSize, pageNumber, out count);
            }
            else
            {

                Dictionary<string, object> newConditions = new Dictionary<string, object>();
                string[] arryParam = new string[] { "address", "name", "templateId", "status", "taskId", "dataSource", "classification", "start_receivedDt", "end_receivedDt" };
                foreach (string param in arryParam)
                {
                    if (dicConditions.Keys.Contains(param))
                    {
                        newConditions.Add(param, dicConditions[param]);
                    }
                    else
                    {
                        newConditions.Add(param, DBNull.Value);
                    }
                }
                IEmailDal emailDal = EmailDalFactory.GetInstance();
                return emailDal.GetEmailInfoByPage(newConditions, pageSize, pageNumber, out count);
            }
        }
    }
}
