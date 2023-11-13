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
    public class EP_SMS_SingleSend
    {
        static string EPTestCaseFilePath = ConfigurationManager.AppSettings["EpTestCaseFile"].ToString();
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();

        private readonly List<string> IBSMS1SendCaseList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_IB_SMS_1_Send()");
        private readonly List<string> IBSuvSMS1SendCaseList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_IBSurvey_SMS_1_Send()");
       
        [TestMethod]
        public void CWIBSMS1Send()//6c
        {
            List<string> taskIdList = new List<string>();
            List<string> expectList = new List<string>();
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (IBSMS1SendCaseList.Count > 0)
            {
                postData = IBSMS1SendCaseList[0].ToString();
            }
            else
            {
                Assert.AreEqual("没有测试数据", IBSMS1SendCaseList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataSMSSingleMutiThread(postData, concurrencyNum, "EP_SMS_SingleSend--->>>CWIBSMS1Send");
        }

        [TestMethod]
        public void CWIBSurveySMS1Send()//7c
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (IBSuvSMS1SendCaseList.Count > 0)
            {
                postData = IBSuvSMS1SendCaseList[0].ToString();
            }
            else
            {
                Assert.AreEqual("没有测试数据", IBSuvSMS1SendCaseList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataSMSSingleMutiThread(postData, concurrencyNum, "EP_SMS_SingleSend--->>>CWIBSurveySMS1Send");
        }
    }
}
