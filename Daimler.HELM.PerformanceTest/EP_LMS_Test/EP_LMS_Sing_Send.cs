using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Daimler.HELM.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Daimler.HELM.PerformanceTest
{
    [TestClass]
    public class EP_LMS_Sing_Send
    {
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();

        [TestMethod]
        public void CWIBLMS1Send()
        {
            SendLMSService.Cmcc_mas_wbsSoapBindingSoapClient slc = new SendLMSService.Cmcc_mas_wbsSoapBindingSoapClient();

            SendLMSService.sendSmsRequest sendSmsR = new SendLMSService.sendSmsRequest();
            sendSmsR.ApplicationID = "BCZG";
            sendSmsR.DestinationAddresses = new string[] { "tel:1355286xxxx" };
            sendSmsR.ExtendCode = "8";
            sendSmsR.Message = "Matthew Performance Testing - <LMS Test Message>";
            sendSmsR.MessageFormat = SendLMSService.MessageFormat.UCS2;
            sendSmsR.SendMethod = SendLMSService.SendMethodType.Normal;
            sendSmsR.DeliveryResultRequest = false;

            int concurrencyNum = int.Parse(ConcurrencyNumber);

            LogPerformanceData.LogPerfDataLMSMutiThread(sendSmsR, concurrencyNum, "EP_LMS_Sing_Send--->>>CWIBLMS1Send");
        }
    }
}
