using Daimler.HELM.BizObjects;
using Daimler.HELM.DBInterface;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.MessageFilter;
using Daimler.HELM.MessageInterface;
using Daimler.HELM.MessageInterface.Impl;
using Daimler.HELM.MQ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.HubService.Logic
{
    public static class SMSHandler
    {
        private static string machineName = System.Configuration.ConfigurationManager.AppSettings["HubServer"];
        private static object objThead = new object();
        /// <summary>
        /// record the reqeust log of send sms
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CommonResult RecordSMSSendLog(RequestLog request)
        {
            CommonResult result = new CommonResult();
            try
            {
                ISMSDal smsDal = SMSDalFactory.GetInstance();
                result.IsOK = smsDal.RecordSMSSendLog(request);
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// record the sending message status
        /// </summary>
        /// <param name="msgInfo"></param>
        /// <returns></returns>
        public static CommonResult RecordSMSSendingInfo(MessageInfo msgInfo, bool isError)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            try
            {
                ISMSDal smsDal = SMSDalFactory.GetInstance();

                Dictionary<string, object> dicMessageInfo = new Dictionary<string, object>();
                dicMessageInfo.Add("id", msgInfo.Id);
                dicMessageInfo.Add("requestId", msgInfo.RequestId);
                dicMessageInfo.Add("productId", msgInfo.ProductId);
                dicMessageInfo.Add("mobile", msgInfo.Number);
                dicMessageInfo.Add("content", msgInfo.Content);
                dicMessageInfo.Add("taskId", msgInfo.TaskId);
                dicMessageInfo.Add("dataSource", msgInfo.DataSource);
                dicMessageInfo.Add("classification", (int)msgInfo.Classification);
                dicMessageInfo.Add("priority", (int)msgInfo.Priority);
                dicMessageInfo.Add("createdDt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                dicMessageInfo.Add("receivedDt", msgInfo.ReceivedDt);
                dicMessageInfo.Add("sessionId", msgInfo.SessionId);
                dicMessageInfo.Add("isNeedReply", msgInfo.IsNeedReply);
                if (isError)
                {
                    dicMessageInfo.Add("status", (int)msgInfo.Status);
                    dicMessageInfo.Add("errorInfo", msgInfo.ErrorInfo);
                }

                result.IsOK = smsDal.RecordSMSSendingInfo(dicMessageInfo);
                return result;
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;

                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageId = msgInfo.RequestId,
                    MessageType = "record sms message",
                    ExceptionInfo = ex
                });
            }

            return result;
        }


        /// <summary>
        /// batch insert sms message
        /// </summary>
        /// <param name="msgList"></param>
        /// <returns></returns>
        public static CommonResult BatchRecordSMSInfo(List<MessageInfo> msgList, bool isError)
        {
            CommonResult result = new CommonResult();
            if (msgList.Count == 0)
            {
                result.IsOK = false;
                return result;
            }

            try
            {
                DataTable dtSMSInfo = new DataTable();
                dtSMSInfo.TableName = "SMSSendInfo";
                DataColumn[] columns = new DataColumn[] { 
                        new DataColumn("id"),
                        new DataColumn("requestId"),
                        new DataColumn("productId"),
                        new DataColumn("mobile"),
                        new DataColumn("content"),
                        new DataColumn("taskId"),
                        new DataColumn("dataSource"), 
                        new DataColumn("classification"), 
                        new DataColumn("priority"),
                        new DataColumn("createdDt"),   
                        new DataColumn("receivedDt"),
                        new DataColumn("sessionId"),     
                        new DataColumn("isNeedReply"),  
                        new DataColumn("status"),
                        new DataColumn("errorInfo")
                    };
                dtSMSInfo.Columns.AddRange(columns);

                foreach (MessageInfo msgInfo in msgList)
                {
                    DataRow row = dtSMSInfo.NewRow();
                    row["id"] = msgInfo.Id;
                    row["requestId"] = msgInfo.RequestId;
                    row["productId"] = msgInfo.ProductId;
                    row["mobile"] = msgInfo.Number;
                    row["content"] = msgInfo.Content;
                    row["taskId"] = msgInfo.TaskId;
                    row["dataSource"] = msgInfo.DataSource;
                    row["classification"] = (int)msgInfo.Classification;
                    row["priority"] = (int)msgInfo.Priority;
                    row["createdDt"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                    row["receivedDt"] = msgInfo.ReceivedDt;
                    row["sessionId"] = msgInfo.SessionId;
                    row["isNeedReply"] = msgInfo.IsNeedReply;
                    if (isError)
                    {
                        row["status"] = (int)msgInfo.Status;
                        row["errorInfo"] = msgInfo.ErrorInfo;
                    }
                    else
                    {
                        row["status"] = null;
                        row["errorInfo"] = null;
                    }

                    dtSMSInfo.Rows.Add(row);
                }

                ICommonDal commonDal = CommonDalFactory.GetInstance();
                result.IsOK = commonDal.RecordBatchInfo(dtSMSInfo);
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageId = msgList[0].RequestId,
                    MessageType = "record batch sms message",
                    ExceptionInfo = ex
                });
            }
            return result;
        }


        /// <summary>
        /// when send message to vendor record the return message to DB
        /// </summary>
        /// <param name="requestResult"></param>
        /// <param name="messageId"></param>
        /// <param name="vendorName"></param>
        /// <returns></returns>
        public static CommonResult RecordSMSSubmitResult(RequestResultInfo requestResult, Guid messageId, string vendorName)
        {
            CommonResult result = new CommonResult();
            try
            {
                ISMSDal smsDal = SMSDalFactory.GetInstance();
                result.IsOK = smsDal.RecordSMSSubmitResult(requestResult, messageId, vendorName);
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;
            }

            return result;
        }



        /// <summary>
        /// get message sending status
        /// </summary>
        /// <param name="classification"></param>
        /// <returns></returns>
        public static CommonResult GetMessageSendStatus(MessageClassification classification)
        {
            CommonResult result = new CommonResult();
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                ISMSDal smsDal = SMSDalFactory.GetInstance();
                List<MessageInfo> messageList = smsDal.GetSMSSendingMessage(classification);
                foreach (MessageInfo message in messageList)
                {
                    try
                    {
                        ISMSInterface smsInterface = VendorFactory.GetSMSInstance(message.VendorName);
                        SendingResult strResult = smsInterface.GetSMSSendingStatus(message);
                        if (strResult != null)
                        {
                            //update the sending message status
                            Dictionary<string, object> dicUpdateList = new Dictionary<string, object>();
                            dicUpdateList.Add("status", (int)strResult.Status);
                            dicUpdateList.Add("sendDesc", strResult.Desc);
                            dicUpdateList.Add("getStatusDt", time);
                            dicUpdateList.Add("sentDt", strResult.SendTime);

                            Dictionary<string, object> conditions = new Dictionary<string, object>();
                            conditions.Add("id", message.Id);

                            smsDal.UpdateSMSSendingStatus(conditions, dicUpdateList);

                            smsDal.RecordSMSSendingStatus(strResult, message.Id, message.VendorName);
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHandler.RecordExceptionLog(new ExceptionLog()
                        {
                            Id = Guid.NewGuid(),
                            MessageId = message.Id,
                            MessageType = "get sms message status from vendor",
                            ExceptionInfo = ex
                        });
                    }
                }
                result.IsOK = true;
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;
            }
            return result;

        }




        public static CommonResult UpdateMessageReceiveStates(IList<MessageStateInfo> list)
        {
            CommonResult result = new CommonResult();
            ISMSDal smsDal = SMSDalFactory.GetInstance();
            result.IsOK = smsDal.UpdateMessageReceiveStates(list);
            return result;
        }






        /// <summary>
        /// send message to vendor
        /// </summary>
        /// <param name="msgObj"></param>
        public static void SendMessageToVendor(MessageInfo msgObj, bool isRepeat=false)
        {
            Log.LogHandler.WriteLog(msgObj.Id + ",start ");
            msgObj = CommonHandler.GetSender(msgObj);
            SendSMSMessageToVendor(msgObj,isRepeat);
        }

        /// <summary>
        /// send message to vendor
        /// </summary>
        /// <param name="msgObj"></param>
        private static void SendSMSMessageToVendor(MessageInfo msgObj,bool isRepeat)
        {
            lock (objThead)
            {
                try
                {
                    //根据sessionId 生成扩展号码
                    if (!string.IsNullOrEmpty(msgObj.VendorName))
                    {
                        Log.LogHandler.WriteLog(msgObj.Id + ",start send");
                        ISMSInterface smsInterface = VendorFactory.GetSMSInstance(msgObj.VendorName);
                        RequestResultInfo strResult = smsInterface.SendSMS(msgObj);

                        Log.LogHandler.WriteLog(msgObj.Id + ",finish send");
                        if (strResult != null)
                        {
                            Log.LogHandler.WriteLog(msgObj.Id + ",start update");

                            msgObj.Status = strResult.SMSStatus;
                            msgObj.SubDesc = strResult.Desc;
                            msgObj.VendorTaskId = strResult.TaskId;
                            msgObj.SenderNumber = strResult.SenderNumber;
                            msgObj.SubmitDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                            ISMSDal smsDal = SMSDalFactory.GetInstance();

                            bool bol = smsDal.UpdateSMSSendingStatus(msgObj, isRepeat);

                            Log.LogHandler.WriteLog(msgObj.Id + ",finish update");
                            if (!bol)
                            {
                                throw new Exception(msgObj.Id + "提交状态更新失败！" + strResult.StrResult);
                            }

                            //记录短信提交结果
                            //RecordSMSSubmitResult(strResult, msgObj.Id, msgObj.VendorName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommonHandler.RecordExceptionLog(new ExceptionLog()
                    {
                        Id = Guid.NewGuid(),
                        MessageId = msgObj.Id,
                        MessageType = "send sms message to vendor",
                        ExceptionInfo = ex
                    });
                }
            }
        }


        public static void SendBatchSMSMessageToVendor(List<MessageInfo> msgList, bool isRepeatSend=false)
        {
            int batchSize = 500;
            foreach (MessageInfo msgInfo in msgList)
            {
                CommonHandler.GetSender(msgInfo);
            }

            List<Dictionary<string, List<MessageInfo>>> pageDicList = Common.CommonMethod.SplitBatchMessage(msgList, batchSize);

            SendBatchSMS(pageDicList, isRepeatSend);

        }

        private static void SendBatchSMS(List<Dictionary<string, List<MessageInfo>>> pageDicList,bool isRepeatSend)
        {

            ISMSDal smsDal = SMSDalFactory.GetInstance();

            foreach (var dic in pageDicList)
            {
                foreach (string key in dic.Keys)
                {
                    try
                    {
                        List<MessageInfo> sendList = dic[key];
                        if (!string.IsNullOrEmpty(sendList[0].VendorName))
                        {
                            ISMSInterface smsInterface = VendorFactory.GetSMSInstance(sendList[0].VendorName);
                            List<RequestResultInfo> resultList = smsInterface.SendSMS(sendList);
                            foreach (RequestResultInfo strResult in resultList)
                            {
                                try
                                {
                                    MessageInfo msgInfo = sendList.Find(p => p.Number == strResult.Mobile);

                                    msgInfo.Status = strResult.SMSStatus;
                                    msgInfo.SubDesc = strResult.Desc;
                                    msgInfo.VendorTaskId = strResult.TaskId;
                                    msgInfo.SenderNumber = strResult.SenderNumber;
                                    msgInfo.SubmitDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                                    //修改消息发送状态
                                    bool bol = smsDal.UpdateSMSSendingStatus(msgInfo, isRepeatSend);
                                    if (!bol)
                                    {
                                        throw new Exception("SMS批量提交状态更新失败！" + strResult.StrResult);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    CommonHandler.RecordExceptionLog(new ExceptionLog()
                                    {
                                        MessageType = "Update batch SMS sending status",
                                        ExceptionInfo = ex
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHandler.RecordExceptionLog(new ExceptionLog()
                        {
                            MessageType = "Send batch SMS",
                            ExceptionInfo = ex
                        });
                    }
                }
            }
        }


        /// <summary>
        /// get sms batch message
        /// </summary>
        /// <returns></returns>
        public static List<MessageInfo> GetSMSBatchMessage()
        {
            ISMSDal smsDal = SMSDalFactory.GetInstance();
            List<MessageInfo> messageList = smsDal.GetSMSBatchMessage();
            return messageList;
        }



        /// <summary>
        /// get sms send status
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public static List<MessageStateInfo> GetMessageStateInfos(string dataSource, string[] taskIds)
        {
            ISMSDal smsDal = SMSDalFactory.GetInstance();
            List<MessageStateInfo> statusList = smsDal.GetMessageStateInfos(dataSource, taskIds);
            return statusList;
        }




        public static List<MessageReplyInfo> GetSMSReplyMessageFromVendor(string startDate, string endDate)
        {
            List<MessageReplyInfo> replyList = new List<MessageReplyInfo>();
            string strVendorMock = System.Configuration.ConfigurationManager.AppSettings["VendorMock"];

            List<SourceVendorConfig> venderAccountList = CommonHandler.GetVendorAccountList();
            foreach (SourceVendorConfig venderAccount in venderAccountList)
            {
                string venderName = venderAccount.VendorName;
                if (strVendorMock == "1")
                {
                    venderName = "MOCK";
                }
                ISMSInterface smsInterface = VendorFactory.GetSMSInstance(venderAccount.VendorName);
                List<MessageReplyInfo> venderReplyLst = smsInterface.GetReply(venderAccount.AccountName, venderAccount.AccountPwd);
                replyList.AddRange(venderReplyLst);
            }
            return replyList;
        }


        public static List<SendingResult> GetSMSSendingResult()
        {
            List<SendingResult> resultList = new List<SendingResult>();
            string strVendorMock = System.Configuration.ConfigurationManager.AppSettings["VendorMock"];
            List<SourceVendorConfig> venderAccountList = CommonHandler.GetVendorAccountList();
            foreach (SourceVendorConfig venderAccount in venderAccountList)
            {
                string venderName = venderAccount.VendorName;
                if (strVendorMock == "1")
                {
                    venderName = "MOCK";
                }
                ISMSInterface smsInterface = VendorFactory.GetSMSInstance(venderAccount.VendorName);
                List<SendingResult> partOfResult = smsInterface.GetSMSSendingStatus(venderAccount.AccountName, venderAccount.AccountPwd);
                resultList.AddRange(partOfResult);

            }
            return resultList;
        }



        public static List<MessageReplyInfo> GetSMSReplyMessageFromDB(string dataSource, string startDateTime, string endDateTime)
        {
            ISMSDal smsDal = SMSDalFactory.GetInstance();
            return smsDal.GetRelyInfo(dataSource, startDateTime, endDateTime);
        }


        public static List<MessageReplyInfo> GetNeedRepeatSendReplyList()
        {
            ISMSDal smsDal = SMSDalFactory.GetInstance();
            return smsDal.GetNeedRepeatSendReplyInfo(2);
        }

        public static bool UpdateReplyInfoStatus(MessageReplyInfo replyInfo, int status)
        {
            ISMSDal smsDal = SMSDalFactory.GetInstance();

            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("id", replyInfo.Id);

            Dictionary<string, object> updateColumns = new Dictionary<string, object>();
            updateColumns.Add("status", status);
            updateColumns.Add("statusDesc", replyInfo.statusDesc);

            return smsDal.UpdateReplyInfo(conditions, updateColumns);
        }

        public static bool UpdateSMSRepeatSentTimes(Guid msgId)
        {
            ISMSDal smsDal = SMSDalFactory.GetInstance();
            return smsDal.UpdateSMSRepeatSentTimes(msgId);
        }

        public static bool UpdateSMSRepeatReplyTimes(Guid msgId)
        {
            ISMSDal smsDal = SMSDalFactory.GetInstance();
            return smsDal.UpdateSMSRepeatReplyTimes(msgId);
        }

    }
}
