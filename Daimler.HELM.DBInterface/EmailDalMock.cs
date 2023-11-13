using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.DBInterface
{
    public class EmailDalMock : IEmailDal
    {
        public bool RecordEmailSendLog(BizObjects.RequestLog request)
        {
            string requestInfo = string.Format("{0} --\n {1}  --\n {2}  --\n", request.RequestInfo, request.SendContent, request.CreatedDt);
            Log.LogHandler.WriteLog(request.DataSrouce + "-Email", requestInfo);
            return true;
        }


        public List<BizObjects.MessageInfo> GetEmailMessageByTaskId(string taskId)
        {
            List<MessageInfo> msgList = new List<MessageInfo>();
            msgList.Add(new MessageInfo()
            {
                Id = Guid.NewGuid(),
                Number = "test@abc.com",
                TaskId = "6cf751b5-4e3d-42de-b72e-4791a24ce32f",
                TemplateId = "1097"
            });
            return msgList;
        }

        public bool RecordEmailSendingInfo(Dictionary<string, object> dicMsgInfo)
        {
            Log.LogHandler.WriteLog("RecordEmailSendingInfo");
            return true;
        }


        public bool UpdateEmialMessage(Dictionary<string, object> dicConditions, Dictionary<string, object> dicUpdateField)
        {
            throw new NotImplementedException();
        }

        public bool RecordEmailSubmitResultLog(RequestResultInfo requestResult, Guid messageId, string vendorName)
        {
            throw new NotImplementedException();
        }

        public List<MessageInfo> GetBatchEmailList()
        {
            throw new NotImplementedException();
        }

        public List<MessageInfo> GetNeedRepeatSendEmail(EmailStatus[] status)
        {
            throw new NotImplementedException();
        }


        public bool UpdateEmailRepeatSentTimes(Guid msgId)
        {
            throw new NotImplementedException();
        }


        public System.Data.DataTable GetEmailInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            throw new NotImplementedException();
        }


        public bool UpdateEmailSendingStatus(MessageInfo msgInfo, bool isRepeatSend)
        {
            throw new NotImplementedException();
        }
    }
}
