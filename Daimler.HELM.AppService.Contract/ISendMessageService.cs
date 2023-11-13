using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.HubService.Contract
{
    [ServiceContract]
    public interface ISendMessageService
    {
        [OperationContract]
        CommonResult SendSingleSMS(MessageInfo messageInfo);

        [OperationContract]
        CommonResult SendSingleMMS(MessageInfo messageInfo);

        [OperationContract]
        CommonResult SendSingleEDM(MessageInfo messageInfo);

        [OperationContract]
        CommonResult SendSingleWechat(MessageInfo messageInfo);

        [OperationContract]
        CommonResult SendBatchSMS(List<MessageInfo> messageList);

        [OperationContract]
        CommonResult SendBatchMMS(List<MessageInfo> messageList);

        [OperationContract]
        CommonResult SendBatchEDM(List<MessageInfo> messageList);

        [OperationContract]
        CommonResult SendBatchWechat(List<MessageInfo> messageList);



        [OperationContract]
        List<MessageStateInfo> GetMessageStatusInfo(string dataSource, string[] taskIds);

        [OperationContract]
        List<MessageReplyInfo> GetMessageReplyInfo(string dataSource,string startDateTime,string endDateTime);

        [OperationContract]
        CommonResult RecordSMSSendLog(RequestLog request);

        [OperationContract]
        CommonResult RecordMMSSendLog(RequestLog request);

        [OperationContract]
        CommonResult RecordEmailSendLog(RequestLog request);

        [OperationContract]
        void RecordExceptionLog(ExceptionLog log);

        [OperationContract]
        CommonResult ValidateInterfaceAccount(string dataSource, string accountName, string accountPwd);

        [OperationContract]
        List<InterfaceAccount> GetInterfaceAccountListAll();

        [OperationContract]
        InterfaceAccount GetInterfaceAccountByID(Guid id);

        [OperationContract]
        CommonResult AddInterfaceAccount(InterfaceAccount interfaceAccount);

        [OperationContract]
        CommonResult RemoveInterfaceAccount(Guid id);

        [OperationContract]
        CommonResult UpdateInterfaceAccount(InterfaceAccount interfaceAccount);
        [OperationContract]
        System.Data.DataSet GetMessageLog(string messageType, string messageSource, string beginDate, string endDate, string keyWord, int pageIndex, int pageSize,out int totalCount);
        [OperationContract]
        RequestLog GetLogInfoById(string id, string messageType);

        [OperationContract]
        List<ExceptionLogInfo> GetExceptionLogByDate(string date,string endDate,int pageSize,int pageIndex);
        [OperationContract]
        ExceptionLogInfo GetExceptionLogById(string id);
        [OperationContract]
        int GetExceptionLogCountByDate(string date,string endDate);
        [OperationContract]
        List<ShowMessageReplyInfo> GetMessageReplyInfoByCondit(ShowMessageReplyInfo where, int pageSize, int pageIndex);
        [OperationContract]
        int GetMessageReplyCount(ShowMessageReplyInfo where);

        [OperationContract]
        DataTable GetMessageInfoByPage(string messageType, Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count);


        [OperationContract]
        CommonResult BindAccount(WeChatResponse userInfo);
    }
}
