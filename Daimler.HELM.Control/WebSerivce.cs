using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubServiceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.Control
{
    public class WebSerivce : ISendMessageService
    {
        private readonly MessageServiceProxy proxy = new MessageServiceProxy();

        public HubService.Contract.CommonResult SendSingleSMS(BizObjects.MessageInfo messageInfo)
        {
            return proxy.ServiceChannel.SendSingleSMS(messageInfo);
        }

        public HubService.Contract.CommonResult SendSingleMMS(BizObjects.MessageInfo messageInfo)
        {
            return proxy.ServiceChannel.SendSingleMMS(messageInfo);
        }

        public HubService.Contract.CommonResult SendSingleEDM(BizObjects.MessageInfo messageInfo)
        {
            return proxy.ServiceChannel.SendSingleMMS(messageInfo);
        }

        public HubService.Contract.CommonResult SendSingleWechat(BizObjects.MessageInfo messageInfo)
        {
            return proxy.ServiceChannel.SendSingleWechat(messageInfo);
        }

        public HubService.Contract.CommonResult SendBatchSMS(List<BizObjects.MessageInfo> messageList)
        {
            return proxy.ServiceChannel.SendBatchSMS(messageList);
        }

        public HubService.Contract.CommonResult SendBatchMMS(List<BizObjects.MessageInfo> messageList)
        {
            return proxy.ServiceChannel.SendBatchMMS(messageList);
        }

        public HubService.Contract.CommonResult SendBatchEDM(List<BizObjects.MessageInfo> messageList)
        {
            return proxy.ServiceChannel.SendBatchEDM(messageList);
        }

        public HubService.Contract.CommonResult SendBatchWechat(List<BizObjects.MessageInfo> messageList)
        {
            return proxy.ServiceChannel.SendBatchWechat(messageList);
        }

        public List<BizObjects.MessageStateInfo> GetMessageStatusInfo(string dataSource, string[] taskIds)
        {
            return proxy.ServiceChannel.GetMessageStatusInfo(dataSource, taskIds);
        }


        public List<BizObjects.MessageReplyInfo> GetMessageReplyInfo(string dataSource, string startDateTime, string endDateTime)
        {
            return proxy.ServiceChannel.GetMessageReplyInfo(dataSource,startDateTime,endDateTime);
        }


        public CommonResult RecordSMSSendLog(BizObjects.RequestLog request)
        {
            return proxy.ServiceChannel.RecordSMSSendLog(request);
        }

        public CommonResult RecordMMSSendLog(BizObjects.RequestLog request)
        {
            return proxy.ServiceChannel.RecordMMSSendLog(request);
        }

        public CommonResult RecordEmailSendLog(BizObjects.RequestLog request)
        {
            return proxy.ServiceChannel.RecordEmailSendLog(request);
        }


        public void RecordExceptionLog(BizObjects.ExceptionLog log)
        {
            proxy.ServiceChannel.RecordExceptionLog(log);
        }


        public CommonResult ValidateInterfaceAccount(string dataSource, string accountName, string accountPwd)
        {
            return proxy.ServiceChannel.ValidateInterfaceAccount(dataSource, accountName, accountPwd);
        }


        public List<BizObjects.InterfaceAccount> GetInterfaceAccountListAll()
        {
            return proxy.ServiceChannel.GetInterfaceAccountListAll();
        }

        public BizObjects.InterfaceAccount GetInterfaceAccountByID(Guid id)
        {
            return proxy.ServiceChannel.GetInterfaceAccountByID(id);
        }

        public CommonResult AddInterfaceAccount(BizObjects.InterfaceAccount interfaceAccount)
        {
            return proxy.ServiceChannel.AddInterfaceAccount(interfaceAccount);
        }

        public CommonResult RemoveInterfaceAccount(Guid id)
        {
            return proxy.ServiceChannel.RemoveInterfaceAccount(id);
        }

        public CommonResult UpdateInterfaceAccount(BizObjects.InterfaceAccount interfaceAccount)
        {
            return proxy.ServiceChannel.UpdateInterfaceAccount(interfaceAccount);
        }

        public System.Data.DataSet GetMessageLog(string messageType, string messageSource, string beginDate, string endDate, string keyWord, int pageIndex, int pageSize, out int totalCount)
        {
            throw new NotImplementedException();
        }


        public BizObjects.RequestLog GetLogInfoById(string id, string messageType)
        {
            throw new NotImplementedException();
        }


        public List<BizObjects.ExceptionLogInfo> GetExceptionLogByDate(string date, string endDate,int pageSize, int pageIndex)
        {
            return proxy.ServiceChannel.GetExceptionLogByDate(date, endDate,pageSize, pageIndex);
        }
        public BizObjects.ExceptionLogInfo GetExceptionLogById(string id)
        {
            return proxy.ServiceChannel.GetExceptionLogById(id);
        }

        public int GetExceptionLogCountByDate(string date,string endDate)
        {
            return proxy.ServiceChannel.GetExceptionLogCountByDate(date,endDate);
        }



        public List<BizObjects.ShowMessageReplyInfo> GetMessageReplyInfoByCondit(BizObjects.ShowMessageReplyInfo where, int pageSize, int pageIndex)
        {
            return proxy.ServiceChannel.GetMessageReplyInfoByCondit(where, pageSize, pageIndex);
        }
        public int GetMessageReplyCount(BizObjects.ShowMessageReplyInfo where)
        {
            return proxy.ServiceChannel.GetMessageReplyCount(where);
        }



        public System.Data.DataTable GetMessageInfoByPage(string messageType, Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            return proxy.ServiceChannel.GetMessageInfoByPage(messageType,dicConditions,pageSize,pageNumber,out count);
        }


        public CommonResult BindAccount(BizObjects.WeChatResponse userInfo)
        {
            return proxy.ServiceChannel.BindAccount(userInfo);
        }
    }
}
