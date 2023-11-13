using Daimler.HELM.BizObjects;
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
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.HubService.Logic
{
    public static class EmailHandler
    {
        private static object objThead = new object();

        /// <summary>
        /// record the reqeust log of send sms
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CommonResult RecordEmailSendLog(RequestLog request)
        {
            CommonResult result = new CommonResult();
            try
            {
                IEmailDal smsDal = EmailDalFactory.GetInstance();
                result.IsOK = smsDal.RecordEmailSendLog(request);
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
        public static CommonResult RecordEDMSendingInfo(MessageInfo msgInfo,bool isError)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            try
            {
                IEmailDal emailDal = EmailDalFactory.GetInstance();
                Dictionary<string, object> dicMessageInfo = new Dictionary<string, object>();
                dicMessageInfo.Add("id", msgInfo.Id);
                dicMessageInfo.Add("requestId", msgInfo.RequestId);
                dicMessageInfo.Add("batchId", msgInfo.BatchId);
                dicMessageInfo.Add("templateId", msgInfo.TemplateId);
                dicMessageInfo.Add("address", msgInfo.Number);
                dicMessageInfo.Add("name", msgInfo.EmailName);
                dicMessageInfo.Add("interestedseries", msgInfo.InterestedSeries);
                dicMessageInfo.Add("taskId", msgInfo.TaskId);
                dicMessageInfo.Add("dataSource", msgInfo.DataSource);
                dicMessageInfo.Add("classification", (int)msgInfo.Classification);
                dicMessageInfo.Add("priority", (int)msgInfo.Priority);
                dicMessageInfo.Add("createdDt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                dicMessageInfo.Add("receivedDt", msgInfo.ReceivedDt);
                dicMessageInfo.Add("emailfrom", msgInfo.From);
                dicMessageInfo.Add("reply", msgInfo.Reply);
                dicMessageInfo.Add("subject", msgInfo.Subject);
                if (isError)
                {
                    dicMessageInfo.Add("status", (int)msgInfo.Status);
                    dicMessageInfo.Add("errorInfo", msgInfo.ErrorInfo); 
                }
                result.IsOK = emailDal.RecordEmailSendingInfo(dicMessageInfo);
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;

                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageId = msgInfo.RequestId,
                    MessageType = "record email message",
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
        public static CommonResult BatchRecordEmailInfo(List<MessageInfo> msgList, bool isError)
        {
            CommonResult result = new CommonResult();
            if (msgList.Count == 0)
            {
                result.IsOK = false;
                return result;
            }

            try
            {
                DataTable dtEmailInfo = new DataTable();
                dtEmailInfo.TableName = "EmailSendInfo";
                DataColumn[] columns = new DataColumn[] { 
                        new DataColumn("id"),
                        new DataColumn("requestId"),
                        new DataColumn("batchId"),
                        new DataColumn("templateId"),
                        new DataColumn("address"),
                        new DataColumn("name"),
                        new DataColumn("interestedseries"),
                        new DataColumn("taskId"),
                        new DataColumn("dataSource"), 
                        new DataColumn("classification"), 
                        new DataColumn("priority"),
                        new DataColumn("createdDt"),   
                        new DataColumn("receivedDt"),
                        new DataColumn("emailfrom"),     
                        new DataColumn("reply"),  
                        new DataColumn("subject"),
                        new DataColumn("status"),
                        new DataColumn("errorInfo")
                    };
                dtEmailInfo.Columns.AddRange(columns);

                foreach (MessageInfo msgInfo in msgList)
                {
                    DataRow row = dtEmailInfo.NewRow();
                    row["id"] = msgInfo.Id;
                    row["requestId"] = msgInfo.RequestId;
                    row["batchId"] = msgInfo.BatchId; 
                    row["templateId"] = msgInfo.TemplateId;
                    row["address"] = msgInfo.Number;
                    row["name"] = msgInfo.EmailName;
                    row["interestedseries"] = msgInfo.InterestedSeries;
                    row["taskId"] = msgInfo.TaskId;
                    row["dataSource"] = msgInfo.DataSource;
                    row["classification"] = (int)msgInfo.Classification;
                    row["priority"] = (int)msgInfo.Priority;
                    row["createdDt"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                    row["receivedDt"] = msgInfo.ReceivedDt;
                    row["emailfrom"] = msgInfo.From;
                    row["reply"] = msgInfo.Reply ;
                    row["subject"] = msgInfo.Subject;
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

                    dtEmailInfo.Rows.Add(row);
                }

                ICommonDal commonDal = CommonDalFactory.GetInstance();
                result.IsOK = commonDal.RecordBatchInfo(dtEmailInfo);
            }
            catch (Exception ex)
            {
                CommonHandler.RecordExceptionLog(new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageId = msgList[0].RequestId,
                    MessageType = "record batch email message",
                    ExceptionInfo = ex
                });
            }
            return result;
        }


        /// <summary>
        /// send message to vendor
        /// </summary>
        /// <param name="msgObj"></param>
        public static void SendEmail(MessageInfo msgObj, bool isRepeatSend=false)
        {
            if (string.IsNullOrEmpty(msgObj.VendorName))
            {
                string strVendorMock = System.Configuration.ConfigurationManager.AppSettings["VendorMock"];
                if (strVendorMock == "1")
                {
                    msgObj.VendorName = "MOCK";
                }
                else
                {
                    msgObj.VendorName = "SendCloud";
                }
            }

            SendEmailToVendor(msgObj, isRepeatSend);
        }


        /// <summary>
        /// send message to vendor
        /// </summary>
        /// <param name="msgObj"></param>
        private static void SendEmailToVendor(MessageInfo msgObj,bool isRepeatSend)
        {
            lock (objThead)
            {
                try
                {
                    IEmailInterface emailInterface = VendorFactory.GetEmailInterface(msgObj.VendorName);
                    RequestResultInfo strResult = emailInterface.SendEmail(msgObj);
                    if (strResult != null)
                    {                    
                        msgObj.EmailStatus = strResult.EmailStatus;
                        msgObj.SubmitDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"); 
                        msgObj.SubDesc = strResult.Desc;
                        msgObj.VendorTaskId = strResult.TaskId;

                        IEmailDal emailDal = EmailDalFactory.GetInstance();
                        bool bol = emailDal.UpdateEmailSendingStatus(msgObj, isRepeatSend);
                        if(!bol)
                        {
                            throw new Exception(msgObj.Id + "提交状态更新失败！" + strResult.StrResult);
                        }
                        //记录短信提交结果
                        //emailDal.RecordEmailSubmitResultLog(strResult, msgObj.Id, msgObj.VendorName);
                    }
                }
                catch (Exception ex)
                {
                    CommonHandler.RecordExceptionLog(new ExceptionLog()
                    {
                        Id = Guid.NewGuid(),
                        MessageId = msgObj.Id,
                        MessageType = "send email to vendor",
                        ExceptionInfo = ex
                    });
                }
            }
        }



        public static void SendBatchEmailMessageToVendor(List<MessageInfo> msgList,bool isRepeatSend=false)
        {
            int batchSize = 5;
            string strVendorMock = System.Configuration.ConfigurationManager.AppSettings["VendorMock"];
            string vendorName = string.Empty;
            if (strVendorMock == "1")
            {
                vendorName = "MOCK";
            }
            else
            {
                vendorName = "SendCloud";
            }


            List<Dictionary<string, List<MessageInfo>>> pageDicList = Common.CommonMethod.SplitBatchMessage(msgList, batchSize);


            SendBatchEmail(vendorName, pageDicList, isRepeatSend);

        }

        private static void SendBatchEmail(string vendorName, List<Dictionary<string, List<MessageInfo>>> pageDicList,bool isRepeatSend)
        {
            IEmailInterface emailInterface = VendorFactory.GetEmailInterface(vendorName);
            IEmailDal emailDal = EmailDalFactory.GetInstance();
            foreach (var dic in pageDicList)
            {
                foreach (string key in dic.Keys)
                {
                    try
                    {
                        List<MessageInfo> sendList = dic[key];
                        List<RequestResultInfo> resultList = emailInterface.SendEmail(sendList);
                        foreach (RequestResultInfo strResult in resultList)
                        {
                            try
                            {
                                MessageInfo msgInfo = sendList.Find(p => p.Number == strResult.Mobile);

                                msgInfo.EmailStatus = strResult.EmailStatus;
                                msgInfo.SubmitDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                                msgInfo.SubDesc = strResult.Desc;
                                msgInfo.VendorTaskId = strResult.TaskId;
                                msgInfo.VendorName = vendorName; 

                                //修改消息发送状态
                                bool result = emailDal.UpdateEmailSendingStatus(msgInfo, isRepeatSend);
                                if (!result)
                                {
                                    throw new Exception("Email批量提交状态更新失败！" + strResult.StrResult);
                                }
                            }
                            catch (Exception ex)
                            {
                                CommonHandler.RecordExceptionLog(new ExceptionLog()
                                {
                                    MessageType = "Update batch Email sending status",
                                    ExceptionInfo = ex
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHandler.RecordExceptionLog(new ExceptionLog()
                        {
                            MessageType = "Send batch Email",
                            ExceptionInfo = ex
                        });
                    }
                }
            }
        }


        public static bool UpdateEmailRepeatSentTimes(Guid msgId)
        {
            IEmailDal emailDal = EmailDalFactory.GetInstance();
            return emailDal.UpdateEmailRepeatSentTimes(msgId);
        }
    }                       
}
