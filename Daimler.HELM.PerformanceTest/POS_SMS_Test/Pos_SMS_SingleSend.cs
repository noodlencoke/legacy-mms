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
    public class Pos_SMS_SingleSend
    {
        static string EPTestCaseFilePath = ConfigurationManager.AppSettings["POSTestCaseFile"].ToString();
        static string ConcurrencyNumber = ConfigurationManager.AppSettings["ConcurrencyNumber"].ToString();

        private readonly List<string> PosLeadNotifiList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "PoS_Lead_Notifi");
        private readonly List<string> PoSInformationConfirmList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "PoS_Information_Confirm");
        private readonly List<string> PoSCustomerfollowupList = TestObj.GetCaseFromXls(EPTestCaseFilePath, "PoS_Customer_followup");

        [TestMethod]
        public void PoSGetLeadNotifi()
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            List<string> taskIdList = new List<string>();
            for (int i = 1; i <= concurrencyNum; i++)
            {
                if (PosLeadNotifiList.Count > 0)
                {
                    string taskId;
                    string postData = TestObj.SetTsbData(PosLeadNotifiList[0], out taskId);
                    taskIdList.Add(taskId);
                    TestObj.PosSMSSend(postData);
                }
                else
                {
                    Assert.AreEqual("没有测试数据", PosLeadNotifiList.Count.ToString());
                }
            }

            LogPerformanceData.LogPerfDataPosAndTsb(taskIdList, concurrencyNum, "Pos_SMS_SingleSend--->>>PoSGetLeadNotifi");
        }

        [TestMethod]
        public void PoSGetInformationConfirm()
        {
            int concurrencyNum = int.Parse(ConcurrencyNumber);
            List<string> taskIdList = new List<string>();
            for (int i = 1; i <= concurrencyNum; i++)
            {
                if (PoSInformationConfirmList.Count > 0)
                {
                    string taskId;
                    string postData = TestObj.SetTsbData(PoSInformationConfirmList[0], out taskId);
                    taskIdList.Add(taskId);
                    TestObj.PosSMSSend(postData);
                }
                else
                {
                    Assert.AreEqual("没有测试数据", PoSInformationConfirmList.Count.ToString());
                }
            }

            LogPerformanceData.LogPerfDataPosAndTsb(taskIdList, concurrencyNum, "Pos_SMS_SingleSend--->>>PoSGetInformationConfirm");
        }

        [TestMethod]
        public void PoSGetCustomerfollowup()
        {
             int concurrencyNum = int.Parse(ConcurrencyNumber);
             List<string> taskIdList = new List<string>();
             for (int i = 1; i <= concurrencyNum; i++)
             {
                 if (PoSCustomerfollowupList.Count > 0)
                 {
                     string taskId;
                     string postData = TestObj.SetTsbData(PoSCustomerfollowupList[0], out taskId);
                     taskIdList.Add(taskId);
                     TestObj.PosSMSSend(postData);
                 }
                 else
                 {
                     Assert.AreEqual("没有测试数据", PoSCustomerfollowupList.Count.ToString());
                 }
             }

             LogPerformanceData.LogPerfDataPosAndTsb(taskIdList, concurrencyNum, "Pos_SMS_SingleSend--->>>PoSGetCustomerfollowup");
        }
    }
}
