using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageInterface
{
    public interface IMMSInterface
    {
        RequestResultInfo SendMMS(MessageInfo msgInfo);

        List<RequestResultInfo> SendMMS(List<MessageInfo> msgInfo);

        SendingResult GetMMSSendingStatus(MessageInfo msgInfo);

        List<SendingResult> GetMMSSendingStatus(string accountName,string accountPwd);

        List<MessageReplyInfo> GetMMSReplyInfo(string startDt, string endDate);
    }            
}
