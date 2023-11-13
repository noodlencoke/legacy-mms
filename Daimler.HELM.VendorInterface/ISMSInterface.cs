using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageInterface
{
    public interface ISMSInterface
    {
        RequestResultInfo SendSMS(MessageInfo msgInfo);

        List<RequestResultInfo> SendSMS(List<MessageInfo> msgList);

        SendingResult GetSMSSendingStatus(MessageInfo msgInfo);

        List<SendingResult> GetSMSSendingStatus(string accountName, string accountPwd);

        List<MessageReplyInfo> GetReply(string accountName, string accountPwd);

    }
}                                                   
