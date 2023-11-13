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
    public class EP_SMS_BatchSend
    {
        static string EPTestCaseFilePath = ConfigurationManager.AppSettings["EpTestCaseFile"].ToString();
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();

        private readonly List<string> NewcarSMSnSendCaseList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_NewCar_SMS_N_Send()");
        private readonly List<string> CompainSMSnSendCaseList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_Campaign_SMS_N_Send()");
        private readonly List<string> InvitationSMSnSendCaseList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_Invitation_SMS_N_Send()");

        [TestMethod]
        public void CWNewCarSMSNSend()//8k
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (NewcarSMSnSendCaseList.Count > 0)
            {
                postData = NewcarSMSnSendCaseList[0].ToString();
            }
            else
            {
                Assert.AreEqual("没有测试数据", NewcarSMSnSendCaseList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataSMSBatchMutiThread(postData, concurrencyNum, "EP_SMS_BatchSend--->>>CWNewCarSMSNSend");
        }

        [TestMethod]
        public void CWCampaignSMSNSend()//8k
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (CompainSMSnSendCaseList.Count > 0)
            {
                postData = CompainSMSnSendCaseList[0].ToString();
                List<string> taskIdList;
                List<string> expectList;

                postData = TestObj.CreateNTaskId(postData, out taskIdList, out expectList);
            }
            else
            {
                Assert.AreEqual("没有测试数据", CompainSMSnSendCaseList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataSMSBatchMutiThread(postData, concurrencyNum, "EP_SMS_BatchSend--->>>CWCampaignSMSNSend");
        }

        [TestMethod]
        public void CWInvitationSMSNSend()//8k
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (InvitationSMSnSendCaseList.Count > 0)
            {
                postData = InvitationSMSnSendCaseList[0].ToString();
                List<string> taskIdList;
                List<string> expectList;
                postData = TestObj.CreateNTaskId(postData, out taskIdList, out expectList);
            }
            else
            {
                Assert.AreEqual("没有测试数据", InvitationSMSnSendCaseList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataSMSBatchMutiThread(postData, concurrencyNum, "EP_SMS_BatchSend--->>>CWInvitationSMSNSend");
        }
    }
}
