using Daimler.HELM.HubService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.HubServiceProxy
{
    public class MessageServiceProxy:BaseServiceProxy<ISendMessageService>
    {
        public ISendMessageService ServiceChannel
        {
            get { return Channel; }
        }
    }
}
