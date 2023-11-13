using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageInterface
{
    public interface IEmailInterface
    {
        RequestResultInfo SendEmail(MessageInfo msgInfo);

        List<RequestResultInfo> SendEmail(List<MessageInfo> msgInfo);
    }
}
