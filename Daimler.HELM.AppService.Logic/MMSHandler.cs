﻿using Daimler.HELM.BizObjects;
using Daimler.HELM.DBInterface;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.MessageFilter;
using Daimler.HELM.MessageInterface;
using Daimler.HELM.MessageInterface.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.HubService.Logic
{
    public static class MMSHandler
    {
        private static readonly object objThead = new object();
        /// <summary>
        /// record the reqeust log of send mms
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CommonResult RecordMMSSendLog(RequestLog request)
        {
            CommonResult result = new CommonResult();
            try
            {
                IMMSDal mmsDal = MMSDalFactory.GetInstance();
                result.IsOK = mmsDal.RecordMMSSendLog(request);
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// record the sending mms message status
        /// </summary>
        /// <param name="msgInfo"></param>
        /// <returns></returns>
        public static CommonResult RecordMMSSendingInfo(MessageInfo msgInfo,bool isError)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            try
            {
                IMMSDal mmsDal = MMSDalFactory.GetInstance();
                Dictionary<string, object> dicMessageInfo = new Dictionary<string, object>();
                dicMessageInfo.Add("id", msgInfo.Id);
                dicMessageInfo.Add("requestId", msgInfo.RequestId);
                dicMessageInfo.Add("productId", msgInfo.ProductId);
                dicMessageInfo.Add("mobile", msgInfo.Number);
                dicMessageInfo.Add("templateId", msgInfo.Content);
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

                result.IsOK = mmsDal.RecordMMSSendingInfo(dicMessageInfo);
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
                    MessageType = "record mms message",
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
        public static CommonResult BatchRecordMMSInfo(List<MessageInfo> msgList, bool isError)
        {
            CommonResult result = new CommonResult();
            if (msgList.Count == 0)
            {
                result.IsOK = false;
                return result;
            }

            try
            {
                DataTable dtMMSInfo = new DataTable();
                dtMMSInfo.TableName = "MMSSendInfo";
                DataColumn[] columns = new DataColumn[] { 
                        new DataColumn("id"),
                        new DataColumn("requestId"),
                        new DataColumn("productId"),
                        new DataColumn("mobile"),
                        new DataColumn("templateId"),
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
                dtMMSInfo.Columns.AddRange(columns);

                foreach (MessageInfo msgInfo in msgList)
                {
                    DataRow row = dtMMSInfo.NewRow();
                    row["id"] = msgInfo.Id;
                    row["requestId"] = msgInfo.RequestId;
                    row["productId"] = msgInfo.ProductId;
                    row["mobile"] = msgInfo.Number;
                    row["templateId"] = msgInfo.Content;
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

                    dtMMSInfo.Rows.Add(row);
                }

                ICommonDal commonDal = CommonDalFactory.GetInstance();
                result.IsOK = commonDal.RecordBatchInfo(dtMMSInfo);
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageId = msgList[0].RequestId,
                    MessageType = "record batch mms message",
                    ExceptionInfo = ex
                });
            }
            return result;
        }





        public static void SendMMSMessageToVendor(MessageInfo msgInfo, bool isRepeatSend=false)
        {
            msgInfo = CommonHandler.GetSender(msgInfo);

            SendMMSToVendor(msgInfo, isRepeatSend);
            
        }

        /// <summary>
        /// send message to vendor
        /// </summary>
        /// <param name="msgObj"></param>
        private static void SendMMSToVendor(MessageInfo msgObj,bool isRepeatSend)
        {
            lock (objThead)
            {
                try
                {
                    if (!string.IsNullOrEmpty(msgObj.VendorName))
                    {
                        //根据sessionId 生成扩展号码
                        IMMSInterface smsInterface = VendorFactory.GetMMSInterface(msgObj.VendorName);
                        RequestResultInfo strResult = smsInterface.SendMMS(msgObj);
                        if (strResult != null)
                        {
                            msgObj.Status = strResult.SMSStatus;
                            msgObj.SubmitDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                            msgObj.SubDesc = strResult.Desc;
                            msgObj.VendorTaskId = strResult.TaskId;
                            msgObj.SenderNumber = strResult.SenderNumber;

                            //修改消息发送状态
                            IMMSDal mmsDal = MMSDalFactory.GetInstance();
                            bool result = mmsDal.UpdateMMSSendingStatus(msgObj, isRepeatSend);
                            if (!result)
                            {
                                throw new Exception("update mms send info error");
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
                        MessageType = "send mms message to vendor",
                        ExceptionInfo = ex
                    });
                }
            }
        }



        public static void SendBatchMMSMessageToVendor(List<MessageInfo> msgList, bool isRepeatSend=false)
        {
            int batchSize = 500;
            
            foreach (MessageInfo msgInfo in msgList)
            {
                CommonHandler.GetSender(msgInfo);
            }
            Log.LogHandler.WriteLog("finished find send account.");

            List<Dictionary<string, List<MessageInfo>>> pageDicList = Common.CommonMethod.SplitBatchMessage(msgList, batchSize);

            Log.LogHandler.WriteLog("start send mms: "+pageDicList.Count);

            SendBatchSMM(pageDicList, isRepeatSend);

        }

        private static void SendBatchSMM(List<Dictionary<string, List<MessageInfo>>> pageDicList,bool isRepeatSend)
        {
            IMMSDal mmsDal = MMSDalFactory.GetInstance();
            foreach (var dic in pageDicList)
            {
                foreach (string key in dic.Keys)
                {
                    try
                    {
                        List<MessageInfo> sendList = dic[key];
                        Log.LogHandler.WriteLog("vendorName:"+sendList[0].VendorName);
                        if (!string.IsNullOrEmpty(sendList[0].VendorName))
                        {
                            IMMSInterface smsInterface = VendorFactory.GetMMSInterface(sendList[0].VendorName);
                            List<RequestResultInfo> resultList = smsInterface.SendMMS(sendList);
                            Log.LogHandler.WriteLog("send result count:"+resultList.Count.ToString());
                            foreach (RequestResultInfo strResult in resultList)
                            {
                                try
                                {
                                    MessageInfo msgInfo = sendList.Find(p => p.Number == strResult.Mobile);

                                    msgInfo.Status = strResult.SMSStatus;
                                    msgInfo.SubmitDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                                    msgInfo.SubDesc = strResult.Desc;
                                    msgInfo.VendorTaskId = strResult.TaskId;
                                    msgInfo.SenderNumber = strResult.SenderNumber;

                                    //修改消息发送状态

                                    bool result = mmsDal.UpdateMMSSendingStatus(msgInfo, isRepeatSend);
                                    if (!result)
                                    {
                                        throw new Exception("MMS批量提交状态更新失败！" + strResult.StrResult);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    CommonHandler.RecordExceptionLog(new ExceptionLog()
                                    {
                                        MessageType = "Update batch MMS sending status",
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
                            MessageType = "Send batch MMS",
                            ExceptionInfo = ex
                        }); 
                    }
                }
            }
        }


        


        public static CommonResult GetMMSSendStatus(MessageClassification classification)
        {
            CommonResult result = new CommonResult();
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                IMMSDal mmsDal = MMSDalFactory.GetInstance();
                List<MessageInfo> messageList = mmsDal.GetMMSSendingMessage(classification);
                foreach (MessageInfo message in messageList)
                {
                    try
                    {
                        IMMSInterface mmsInterface = VendorFactory.GetMMSInterface(message.VendorName);
                        message.Content = message.TemplateId;
                        SendingResult strResult = mmsInterface.GetMMSSendingStatus(message);
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

                            mmsDal.UpdateMMSSendInfo(conditions, dicUpdateList);

                            //smsDal.RecordSMSSendingStatus(strResult, message.Id, message.VendorName);
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

       

        public static bool UpdateMMSRepeatSentTimes(Guid msgId)
        {
            IMMSDal mmsDal = MMSDalFactory.GetInstance();
            return mmsDal.UpdateMMSRepeatSentTimes(msgId);
        }


        public static List<SendingResult> GetMMSSendingResult()
        {
            List<SendingResult> resultList = new List<SendingResult>();
            string strVendorMock = System.Configuration.ConfigurationManager.AppSettings["VendorMock"];
            List<SourceVendorConfig> venderAccountList = CommonHandler.GetVendorAccountList();
            foreach (SourceVendorConfig venderAccount in venderAccountList)
            {
                if (venderAccount.AccountType == "1")
                {
                    string venderName = venderAccount.VendorName;
                    if (strVendorMock == "1")
                    {
                        venderName = "MOCK";
                    }
                    IMMSInterface mmsInterface = VendorFactory.GetMMSInterface(venderAccount.VendorName);
                    List<SendingResult> partOfResult = mmsInterface.GetMMSSendingStatus(venderAccount.AccountName, venderAccount.AccountPwd);
                    resultList.AddRange(partOfResult);
                }
            }
            return resultList;
        }
    }
}
