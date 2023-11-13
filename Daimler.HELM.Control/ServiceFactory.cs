using Daimler.HELM.HubService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.Control
{
    public static class ServiceFactory
    {
        public static ISendMessageService GetInstance()
        {
            string isLocal = System.Configuration.ConfigurationManager.AppSettings["isLocal"];
            if (isLocal == "1")
            {
                return new LocalCall();
            }
            else
            {
                return new WebSerivce();
            }
        }
    }
}
