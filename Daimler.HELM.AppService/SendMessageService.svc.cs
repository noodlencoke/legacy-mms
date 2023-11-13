using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Daimler.HELM.HubService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class SendMessageService : ISendMessageService
    {

        public CommonResult SendSingleSMS(MessageInfo messageInfo)
        {
            return SendMessageHandler.SendSingleSMS(messageInfo);
        }

        public CommonResult SendSingleMMS(MessageInfo messageInfo)
        {
            return SendMessageHandler.SendSingleMMS(messageInfo);
        }

        public CommonResult SendSingleEDM(MessageInfo messageInfo)
        {
            return SendMessageHandler.SendSingleEDM(messageInfo);
        }

        public CommonResult SendSingleWechat(MessageInfo messageInfo)
        {
            return SendMessageHandler.SendSingleWechat(messageInfo);
        }



        public CommonResult SendBatchSMS(List<MessageInfo> messageList)
        {
            return SendMessageHandler.SendBatchSMS(messageList);
        }

        public CommonResult SendBatchMMS(List<MessageInfo> messageList)
        {
            return SendMessageHandler.SendBatchMMS(messageList);
        }

        public CommonResult SendBatchEDM(List<MessageInfo> messageList)
        {
            return SendMessageHandler.SendBatchEDM(messageList);
        }

        public CommonResult SendBatchWechat(List<MessageInfo> messageList)
        {
            return SendMessageHandler.SendBatchWechat(messageList);
        }




        /// <summary>
        /// get message status
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public List<MessageStateInfo> GetMessageStatusInfo(string dataSource, string[] taskIds)
        {
            return SendMessageHandler.GetMessageStateInfos(dataSource, taskIds);
        }


        public List<MessageReplyInfo> GetMessageReplyInfo(string dataSource, string startDateTime, string endDateTime)
        {
            return SendMessageHandler.GetSMSReplyInfo(dataSource, startDateTime, endDateTime);
        }




        public CommonResult RecordSMSSendLog(RequestLog request)
        {
            return SMSHandler.RecordSMSSendLog(request);
        }

        public CommonResult RecordMMSSendLog(RequestLog request)
        {
            return MMSHandler.RecordMMSSendLog(request);
        }

        public CommonResult RecordEmailSendLog(RequestLog request)
        {
            return EmailHandler.RecordEmailSendLog(request);
        }


        public void RecordExceptionLog(ExceptionLog log)
        {
            CommonHandler.RecordExceptionLog(log);
        }

        public CommonResult ValidateInterfaceAccount(string dataSource, string accountName, string accountPwd)
        {
            return CommonHandler.ValidateInterfaceAccount(dataSource,accountName,accountPwd);
        }

        public List<InterfaceAccount> GetInterfaceAccountListAll()
        {
            return CommonHandler.GetInterfaceAccountListAll();
        }

        public InterfaceAccount GetInterfaceAccountByID(Guid id)
        {
            return CommonHandler.GetInterfaceAccountByID(id);
        }

        public CommonResult AddInterfaceAccount(InterfaceAccount interfaceAccount)
        {
            return CommonHandler.AddInterfaceAccount(interfaceAccount);
        }

        public CommonResult RemoveInterfaceAccount(Guid id)
        {
            return CommonHandler.RemoveInterfaceAccount(id);
        }

        public CommonResult UpdateInterfaceAccount(InterfaceAccount interfaceAccount)
        {
            return CommonHandler.UpdateInterfaceAccount(interfaceAccount);
        }

        public System.Data.DataSet GetMessageLog(string messageType, string messageSource, string beginDate, string endDate, string keyWord, int pageIndex, int pageSize,out int totalCount)
        {
            return CommonHandler.GetMessageLog(messageType, messageSource, beginDate, endDate, keyWord, pageIndex, pageSize, out totalCount);
        }
        public RequestLog GetLogInfoById(string id, string messageType)
        {
            return CommonHandler.GetLogInfoById(id, messageType);
        }


        public List<ExceptionLogInfo> GetExceptionLogByDate(string date,string endDate,int pageSize,int pageIndex)
        {
            return CommonHandler.GetExceptionLogByDate(date,endDate,pageSize,pageIndex);
        }
        public ExceptionLogInfo GetExceptionLogById(string id)
        {
            return CommonHandler.GetExceptionLogById(id);
        }

        public int GetExceptionLogCountByDate(string date, string endDate)
        {
            return CommonHandler.GetExceptionLogCountByDate(date,endDate);
        }


        public List<ShowMessageReplyInfo> GetMessageReplyInfoByCondit(ShowMessageReplyInfo where, int pageSize, int pageIndex)
        {
            return CommonHandler.GetMessageReplyInfoByCondit(where, pageSize, pageIndex);
        }
        public int GetMessageReplyCount(ShowMessageReplyInfo where)
        {
            return CommonHandler.GetMessageReplyCount(where);
        }

        public DataTable GetMessageInfoByPage(string messageType, Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            return SendMessageHandler.GetMessageInfoByPage(messageType,dicConditions,pageSize,pageNumber,out count);
        }

        #region Wechat 

        public CommonResult BindAccount(WeChatResponse userInfo) {
            return WechatHandler.BindAccount(userInfo);
        }
        #endregion

    }
}
