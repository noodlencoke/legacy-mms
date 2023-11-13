using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.Control
{
    class LocalCall: ISendMessageService
    {
        public CommonResult SendSingleSMS(BizObjects.MessageInfo messageInfo)
        {
            return SendMessageHandler.SendSingleSMS(messageInfo);
        }

        public CommonResult SendSingleMMS(BizObjects.MessageInfo messageInfo)
        {
            return SendMessageHandler.SendSingleMMS(messageInfo);
        }

        public CommonResult SendSingleEDM(BizObjects.MessageInfo messageInfo)
        {
            return SendMessageHandler.SendSingleEDM(messageInfo);
        }

        public CommonResult SendSingleWechat(BizObjects.MessageInfo messageInfo)
        {
            return SendMessageHandler.SendSingleWechat(messageInfo);
        }

        public CommonResult SendBatchSMS(List<BizObjects.MessageInfo> messageList)
        {
            return SendMessageHandler.SendBatchSMS(messageList);
        }

        public CommonResult SendBatchMMS(List<BizObjects.MessageInfo> messageList)
        {
            return SendMessageHandler.SendBatchMMS(messageList);
        }

        public CommonResult SendBatchEDM(List<BizObjects.MessageInfo> messageList)
        {
            return SendMessageHandler.SendBatchEDM(messageList);
        }

        public CommonResult SendBatchWechat(List<BizObjects.MessageInfo> messageList)
        {
            return SendMessageHandler.SendBatchWechat(messageList);
        }

        public List<BizObjects.MessageStateInfo> GetMessageStatusInfo(string dataSource, string[] taskIds)
        {
            return SendMessageHandler.GetMessageStateInfos(dataSource, taskIds);
        }


        public List<BizObjects.MessageReplyInfo> GetMessageReplyInfo(string dataSource, string startDateTime, string endDateTime)
        {
            return SendMessageHandler.GetSMSReplyInfo(dataSource,startDateTime,endDateTime);
        }


        public CommonResult RecordSMSSendLog(BizObjects.RequestLog request)
        {
            return SMSHandler.RecordSMSSendLog(request);
        }

        public CommonResult RecordMMSSendLog(BizObjects.RequestLog request)
        {
            return MMSHandler.RecordMMSSendLog(request);
        }

        public CommonResult RecordEmailSendLog(BizObjects.RequestLog request)
        {
            return EmailHandler.RecordEmailSendLog(request);
        }


        public void RecordExceptionLog(BizObjects.ExceptionLog log)
        {
            CommonHandler.RecordExceptionLog(log);
        }


        public CommonResult ValidateInterfaceAccount(string dataSource, string accountName, string accountPwd)
        {
            return CommonHandler.ValidateInterfaceAccount(dataSource, accountName, accountPwd);
        }

        public List<BizObjects.InterfaceAccount> GetInterfaceAccountListAll()
        {
            return CommonHandler.GetInterfaceAccountListAll();
        }

        public BizObjects.InterfaceAccount GetInterfaceAccountByID(Guid id)
        {
            return CommonHandler.GetInterfaceAccountByID(id);
        }

        public CommonResult AddInterfaceAccount(BizObjects.InterfaceAccount interfaceAccount)
        {
            return CommonHandler.AddInterfaceAccount(interfaceAccount);
        }

        public CommonResult RemoveInterfaceAccount(Guid id)
        {
            return CommonHandler.RemoveInterfaceAccount(id);
        }

        public CommonResult UpdateInterfaceAccount(BizObjects.InterfaceAccount interfaceAccount)
        {
            return CommonHandler.UpdateInterfaceAccount(interfaceAccount);
        }


        public System.Data.DataSet GetMessageLog(string messageType, string messageSource, string beginDate, string endDate, string keyWord, int pageIndex, int pageSize,out int totalCount)
        {
            return CommonHandler.GetMessageLog(messageType, messageSource, beginDate, endDate, keyWord, pageIndex, pageSize, out totalCount);
        }
        public BizObjects.RequestLog GetLogInfoById(string id, string messageType)
        {
            return CommonHandler.GetLogInfoById(id, messageType);
        }



        public List<BizObjects.ExceptionLogInfo> GetExceptionLogByDate(string date, string endDate,int pageSize, int pageIndex)
        {
            return CommonHandler.GetExceptionLogByDate(date,endDate, pageSize, pageIndex);
        }

        public BizObjects.ExceptionLogInfo GetExceptionLogById(string id)
        {
            return CommonHandler.GetExceptionLogById(id);
        }
        public int GetExceptionLogCountByDate(string date,string endDate)
        {
            return CommonHandler.GetExceptionLogCountByDate(date, endDate);
        }


        public List<BizObjects.ShowMessageReplyInfo> GetMessageReplyInfoByCondit(BizObjects.ShowMessageReplyInfo where, int pageSize, int pageIndex)
        {
            return CommonHandler.GetMessageReplyInfoByCondit(where, pageSize, pageIndex);
        }

        public int GetMessageReplyCount(BizObjects.ShowMessageReplyInfo where)
        {
            return CommonHandler.GetMessageReplyCount(where);
        }


        public System.Data.DataTable GetMessageInfoByPage(string messageType, Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            return SendMessageHandler.GetMessageInfoByPage(messageType,dicConditions,pageSize,pageNumber,out count);
        }


        public CommonResult BindAccount(BizObjects.WeChatResponse userInfo)
        {
            return WechatHandler.BindAccount(userInfo);
        }
    }
}
