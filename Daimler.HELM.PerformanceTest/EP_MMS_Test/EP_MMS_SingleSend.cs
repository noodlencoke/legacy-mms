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
    public class EP_MMS_SingleSend
    {
        static string EPTestCaseFilePath = ConfigurationManager.AppSettings["EpTestCaseFile"].ToString();
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();
        private readonly List<string> CWIBMMS1SendList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_IB_MMS_1_Send()");

        [TestMethod]
        public void CWIBMMS1Send()
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            if (CWIBMMS1SendList.Count > 0)
            {
                postData = CWIBMMS1SendList[0].ToString();
            }
            else
            {
                Assert.AreEqual("没有测试数据", CWIBMMS1SendList.Count.ToString());
            }

            LogPerformanceData.LogPerfDataMMSSingleMutiThread(postData, concurrencyNum, "EP_MMS_SingleSend--->>>CWIBMMS1Send");
        }
    }
}
