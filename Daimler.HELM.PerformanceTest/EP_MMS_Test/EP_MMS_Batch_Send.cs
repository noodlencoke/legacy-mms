using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Daimler.HELM.UnitTest;

namespace Daimler.HELM.PerformanceTest
{
    [TestClass]
    public class EP_MMS_Batch_Send
    {
        static string EPTestCaseFilePath = ConfigurationManager.AppSettings["EpTestCaseFile"].ToString();
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();

        private readonly List<string> CWNewCarMMSNSendList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_NewCar_MMS_N_Send()");
        private readonly List<string> CWCampaignMMSNSendList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_Campaign_MMS_N_Send()");
        private readonly List<string> CWInvitationMMSNSendList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_Invitation_MMS_N_Send()");

        [TestMethod]
        public void CWNewCarMMSNSend()//8k
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (CWNewCarMMSNSendList.Count > 0)
            {
                postData = CWNewCarMMSNSendList[0].ToString();
            }
            else
            {
                Assert.AreEqual("没有测试数据", CWNewCarMMSNSendList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataMMSBatchMutiThread(postData, concurrencyNum, "EP_MMS_Batch_Send--->>>CWNewCarMMSNSend");
        }

        [TestMethod]
        public void CWCampaignMMSNSend()//8k
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (CWCampaignMMSNSendList.Count > 0)
            {
                postData = CWCampaignMMSNSendList[0].ToString();
            }
            else
            {
                Assert.AreEqual("没有测试数据", CWCampaignMMSNSendList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataMMSBatchMutiThread(postData, concurrencyNum, "EP_MMS_Batch_Send--->>>CWCampaignMMSNSend");
        }

        [TestMethod]
        public void CWInvitationMMSNSend()//8k
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (CWInvitationMMSNSendList.Count > 0)
            {
                postData = CWInvitationMMSNSendList[0].ToString();
            }
            else
            {
                Assert.AreEqual("没有测试数据", CWInvitationMMSNSendList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataMMSBatchMutiThread(postData, concurrencyNum, "EP_MMS_Batch_Send--->>>CWInvitationMMSNSend");
        }

    }
}
