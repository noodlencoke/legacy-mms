using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.DBInterface
{
    public class MMSDalMock:IMMSDal
    {
        public bool RecordMMSSendLog(BizObjects.RequestLog request)
        {
            string requestInfo = string.Format("{0} --\n {1}  --\n {2}  --\n", request.RequestInfo, request.SendContent, request.CreatedDt);
            Log.LogHandler.WriteLog(request.DataSrouce + "-MMS", requestInfo);
            return true;
        }


        public List<BizObjects.MessageInfo> GetMMSMessageByTaskId(string taskId)
        {
            List<MessageInfo> msgList = new List<MessageInfo>();
            msgList.Add(new MessageInfo()
            {
                Id = Guid.NewGuid(),
                Number = "15611100xxxx",
                TaskId = "00TO000000E6vBgMAJ",
                Content = "Test send mms"
            });
            return msgList;
        }

        public bool RecordMMSSendingInfo(Dictionary<string, object> dicMsgInfo)
        {
            Log.LogHandler.WriteLog("RecordMMSSendingInfo");
            return true;
        }


        public bool UpdateMMSSendInfo(Dictionary<string, object> dicConditions, Dictionary<string, object> dicUpdateFields)
        {
            throw new NotImplementedException();
        }


        public List<MessageInfo> GetBatchMMSList()
        {
            throw new NotImplementedException();
        }


        public List<MessageInfo> GetMMSSendingMessage(MessageClassification classicfiation)
        {
            throw new NotImplementedException();
        }


        public List<MessageInfo> GetNeedRepeatSendMMS(MessageStatus[] status)
        {
            throw new NotImplementedException();
        }


        public bool UpdateMMSRepeatSentTimes(Guid msgId)
        {
            throw new NotImplementedException();
        }


        public System.Data.DataTable GetMMSInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            throw new NotImplementedException();
        }


        public bool UpdateMMSSendingStatus(MessageInfo msgInfo, bool isRepeatSend)
        {
            throw new NotImplementedException();
        }
    }
}
