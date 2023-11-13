using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.DBInterface
{
    public class SMSDalMock:ISMSDal
    {
        public bool RecordSMSSendLog(BizObjects.RequestLog request)
        {
            string requestInfo = string.Format("{0} --\n {1}  --\n {2}  --\n", request.RequestInfo, request.SendContent, request.CreatedDt);
            Log.LogHandler.WriteLog(request.DataSrouce+"-SMS", requestInfo);
            return true;
        }


        public bool RecordSMSSendingInfo(Dictionary<string, object> dicMsgInfo)
        {
            Log.LogHandler.WriteLog("RecordSMSSendingInfo");
            return true;
        }


        public bool UpdateSMSSendingStatus(Dictionary<string, object> dicConditions, Dictionary<string, object> dicUpdateList)
        {
            Log.LogHandler.WriteLog("UpdateSMSSendingStatus");
            return true;
        }


        public List<BizObjects.MessageInfo> GetSMSSendingMessage(BizObjects.MessageClassification classicfiation)
        {
            List<MessageInfo> msgList = new List<MessageInfo>();
            msgList.Add(new MessageInfo()
            {
               Id = Guid.NewGuid(),
               Number="15611100253",
               TaskId = "ADSFSDAAAD3232",
               Content ="Test send sms"
            });
            return msgList;
        }


        public bool RecordSMSSendingStatus(SendingResult sendingResult,Guid messageId,string vendorName)
        {
            Log.LogHandler.WriteLog("RecordSMSSendingStatus");
            return true;
        }


        public bool RecordSMSSubmitResult(RequestResultInfo requestResult, Guid messageId, string vendorName)
        {
            Log.LogHandler.WriteLog("RecordSMSSubmitResult");
            return true;
        }



        public IList<MessageStateInfo> GetMessageStateInfos(string dataSource)
        {
            List<MessageStateInfo> msgList = new List<MessageStateInfo>();
            msgList.Add(new MessageStateInfo()
            {
               id = Guid.NewGuid().ToString(),
               messageType = "SMS",
               returnTime = "2015-08-27 10:28:00",
               status = "0",
               statusDesc="Success",
               taskId="ADSFSDF12ADS"
            });
            return msgList;
        }


        public bool UpdateMessageReceiveStates(IList<MessageStateInfo> list)
        {
            Log.LogHandler.WriteLog("UpdateMessageReceiveStates");
            return true;
        }



        public List<MessageInfo> GetSMSMessageByTaskId(string taskId)
        {
            List<MessageInfo> msgList = new List<MessageInfo>();
            msgList.Add(new MessageInfo()
            {
                Id = Guid.NewGuid(),
                Number = "15611100253",
                TaskId = "ADSFSDAAAD3232",
                Content = "Test send sms"
            });
            return msgList;
        }


        public List<MessageInfo> GetNeedRepeatSendSMS(MessageStatus[] status)
        {
            List<MessageInfo> msgList = new List<MessageInfo>();
            msgList.Add(new MessageInfo()
            {
                Id = Guid.NewGuid(),
                Number = "15611100253",
                TaskId = "ADSFSDAAAD3232",
                Content = "Test send sms"
            });
            return msgList;
        }


        public List<MessageInfo> GetSMSBatchMessage()
        {
            List<MessageInfo> msgList = new List<MessageInfo>();
            msgList.Add(new MessageInfo()
            {
                Id = Guid.NewGuid(),
                Number = "15611100253",
                TaskId = "ADSFSDAAAD3232",
                Content = "Test send sms"
            });
            return msgList;
        }


        public List<MessageStateInfo> GetMessageStateInfos(string dataSource, string[] taskIds)
        {
            List<MessageStateInfo> msgStaList = new List<MessageStateInfo>();
            msgStaList.Add(new MessageStateInfo() { 
                taskId = "ADSDFSD21312ASDF",
                status="0",
                statusDesc="Success",
                returnTime="2015-08-26 15:28:00"
            });
            return msgStaList;
        }


        public bool RecordSMSReplyInfo(System.Data.DataTable dtReplyInfo)
        {
            throw new NotImplementedException();
        }

        public MessageInfo GetReplyInfoFrom(MessageReplyInfo replyInfo)
        {
            throw new NotImplementedException();
        }


        public List<MessageReplyInfo> GetRelyInfo(string dataSource, string startDateTime, string endDateTime)
        {
            throw new NotImplementedException();
        }


        public List<MessageReplyInfo> GetNeedRepeatSendReplyInfo(int status)
        {
            throw new NotImplementedException();
        }

        public bool UpdateReplyInfo(Dictionary<string, object> dicConditions, Dictionary<string, object> dicColumns)
        {
            throw new NotImplementedException();
        }


        public bool UpdateSMSRepeatSentTimes(Guid msgId)
        {
            throw new NotImplementedException();
        }


        public System.Data.DataTable GetSMSInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            throw new NotImplementedException();
        }


        public bool UpdateSMSRepeatReplyTimes(Guid msgId)
        {
            throw new NotImplementedException();
        }


        public bool UpdateSMSSendingStatus(MessageInfo msgInfo, bool isRepeatSend)
        {
            throw new NotImplementedException();
        }
    }
}
