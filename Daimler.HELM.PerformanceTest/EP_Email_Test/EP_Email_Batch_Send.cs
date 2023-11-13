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
    public class EP_Email_Batch_Send
    {
        static string EPTestCaseFilePath = ConfigurationManager.AppSettings["EpTestCaseFile"].ToString();
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();

        private readonly List<string> CWNewCarEDMnSendList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_NewCar_EDM_N_Send()");
        private readonly List<string> CWInvitationEDMNSendList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_Invitation_EDM_N_Send()");
        
        [TestMethod]
        public void CWNewCarEDMNSend()
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;

            if (CWNewCarEDMnSendList.Count > 0)
            {
                string dataFromEp = CWNewCarEDMnSendList[0].ToString();
                string expect;
                postData = TestObj.EPSetNpostData(dataFromEp, out expect);
            }
            else
            {
                Assert.AreEqual("没有测试数据", CWNewCarEDMnSendList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataEmailMutiThread(postData, concurrencyNum, "EP_Email_Batch_Send--->>>CWNewCarEDMNSend");
        }

        [TestMethod]
        public void CWInvitationEDMNSend()
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;

            if (CWInvitationEDMNSendList.Count > 0)
            {
                string dataFromEp = CWInvitationEDMNSendList[0].ToString();
                string expect;
                postData = TestObj.EPSetNpostData(dataFromEp, out expect);
            }
            else
            {
                Assert.AreEqual("没有测试数据", CWInvitationEDMNSendList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataEmailMutiThread(postData, concurrencyNum, "EP_Email_Batch_Send--->>>CWInvitationEDMNSend");
        }
    }
}
