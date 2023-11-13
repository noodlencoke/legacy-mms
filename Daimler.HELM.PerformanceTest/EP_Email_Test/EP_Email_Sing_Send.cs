using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Daimler.HELM.UnitTest;

namespace Daimler.HELM.PerformanceTest
{
    [TestClass]
    public class EP_Email_Sing_Send
    {
        static string EPTestCaseFilePath = ConfigurationManager.AppSettings["EpTestCaseFile"].ToString();
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();

        private readonly List<string> CWIBEDM1SendList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CW_IB_EDM_1_Send()");
        
        [TestMethod]
        public void CWIBEDM1Send()
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            string postData = string.Empty;
            
            if (CWIBEDM1SendList.Count > 0)
            {
                string dataFromEp = CWIBEDM1SendList[0].ToString();
                string expect;
                postData = TestObj.EPSetpostData(dataFromEp, out expect);
            }

            LogPerformanceData.LogPerfDataEmailMutiThread(postData, concurrencyNum, "EP_Email_Sing_Send--->>>CWIBEDM1Send");
        }
    }
}
