using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.SendingStatus.Batch
{
    public class Program
    {
        static void Main(string[] args)
        {
            CommonResult result = SMSHandler.GetMessageSendStatus(MessageClassification.Batch);
            if (!result.IsOK)
            {
                Log.LogHandler.WriteLog(result.ReturnMessage);
            }
        }
    }
}
