using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BatchSendMessage
{
    public class Program
    {
        static void Main(string[] args)
        {
            //查询批量
            SMSHandler.BatchSendSMSMessage();

        }
    }
}
