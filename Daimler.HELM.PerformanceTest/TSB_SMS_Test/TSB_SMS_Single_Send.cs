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
    public class TSB_SMS_Single_Send
    {
        static string EPTestCaseFilePath = ConfigurationManager.AppSettings["TSBTestCaseFile"].ToString();
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();

        private readonly List<string> TSBNotifiList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "TSB_Notifi");
        private readonly List<string> CIAMNotifiList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "CIAM_Notifi");
        private readonly List<string> TSBInterPhoneNumberList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "TSB_InterPhoneNumber");

        [TestMethod]
        public void TSBNotifi()
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            List<string> taskIdList = new List<string>();
            for (int i = 1; i <= concurrencyNum; i++)
            {
                if (TSBNotifiList.Count > 0)
                {
                    string taskId;
                    string postData = TestObj.SetTsbData(TSBNotifiList[0], out taskId);
                    taskIdList.Add(taskId);
                    TestObj.TSBSMSSend(postData);
                }
                else
                {
                    Assert.AreEqual("没有测试数据", TSBNotifiList.Count.ToString());
                }
            }

            LogPerformanceData.LogPerfDataPosAndTsb(taskIdList, concurrencyNum, "TSB_SMS_Single_Send--->>>TSBNotifi");
        }

        [TestMethod]
        public void TSBCIAMNotifi()
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            List<string> taskIdList = new List<string>();
            for (int i = 1; i <= concurrencyNum; i++)
            {
                if (CIAMNotifiList.Count > 0)
                {
                    string taskId;
                    string postData = TestObj.SetTsbData(CIAMNotifiList[0], out taskId);
                    taskIdList.Add(taskId);
                    TestObj.TSBSMSSend(postData);
                }
                else
                {
                    Assert.AreEqual("没有测试数据", CIAMNotifiList.Count.ToString());
                }
            }

            LogPerformanceData.LogPerfDataPosAndTsb(taskIdList, concurrencyNum, "TSB_SMS_Single_Send--->>>CIAMNotifi");
        }

        [TestMethod]
        public void TSBInterPhoneNumber()
        {
            List<string> taskIdList = new List<string>();
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            for (int i = 1; i <= concurrencyNum; i++)
            {
                if (TSBInterPhoneNumberList.Count > 0)
                {
                    string taskId;
                    string postData = TestObj.SetTsbData(TSBInterPhoneNumberList[0], out taskId);
                    taskIdList.Add(taskId);
                    TestObj.TSBSMSSend(postData);
                }
                else
                {
                    Assert.AreEqual("没有测试数据", TSBInterPhoneNumberList.Count.ToString());
                }
            }

            LogPerformanceData.LogPerfDataPosAndTsb(taskIdList, concurrencyNum, "TSB_SMS_Single_Send--->>>TSBInterPhoneNumber");
        }
    }
}
