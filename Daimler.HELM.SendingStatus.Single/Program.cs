using Daimler.HELM.BizObjects;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.SendingStatus.Single
{
    public class Program
    {
        private static readonly object getSingleStatusObj = new object();
        private static bool isRunGetSingleStatus = true;
        static void Main(string[] args)
        {
            //CommonResult result = SMSHandler.GetMessageSendStatus(MessageClassification.Single);
            //if (!result.IsOK)
            //{
            //    Log.LogHandler.WriteLog(result.ReturnMessage);
            //}

            Thread getSingleStatusTh = new Thread(GetSingleStatus);
            getSingleStatusTh.Start();
        }

        private static void GetSingleStatus()
        {
            lock (getSingleStatusObj)
            {
                string runPeriod = "10000";
                while (isRunGetSingleStatus)
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(runPeriod));

                        CommonResult result = SMSHandler.GetMessageSendStatus(MessageClassification.Single);
                        if (!result.IsOK)
                        {
                            Log.LogHandler.WriteLog(result.ReturnMessage);
                        }
                        Log.LogHandler.WriteLog("获取单条信息发送状态");
                    }
                    catch (Exception ex)
                    {
                        Log.LogHandler.WriteLog("获取单条信息发送状态失败" + ex.Message + "," + ex.StackTrace);
                    }
                }
            }
        }
    }
}
