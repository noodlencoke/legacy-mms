using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daimler.HELM.UnitTest;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Daimler.HELM.PerformanceTest
{
    public class MutipleThreadResetEvent : IDisposable
    {
        private readonly ManualResetEvent done;
        private readonly int total;
        private long current;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="total">需要等待执行的线程总数</param>
        public MutipleThreadResetEvent(int total)
        {
            this.total = total;
            current = total;
            done = new ManualResetEvent(false);
        }

        /// <summary>
        /// 唤醒一个等待的线程
        /// </summary>
        public void SetOne()
        {
            // Interlocked 原子操作类 ,此处将计数器减1
            if (Interlocked.Decrement(ref current) == 0)
            {
                //当所以等待线程执行完毕时，唤醒等待的线程
                done.Set();
            }
        }

        /// <summary>
        /// 等待所以线程执行完毕
        /// </summary>
        public void WaitAll()
        {
            done.WaitOne();
        }

        /// <summary>
        /// 释放对象占用的空间
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)done).Dispose();
        }
    }

    public class MemoryInfo
    {
        public struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        [DllImport("kernel32")]
        private static extern void GetWindowsDirectory(StringBuilder WinDir, int count);
        [DllImport("kernel32")]
        private static extern void GetSystemDirectory(StringBuilder SysDir, int count);
        [DllImport("kernel32")]
        private static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        public static MEMORY_INFO GetCurrentMemInfo()
        {
            MEMORY_INFO MemInfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref MemInfo);
            return MemInfo;
        }
    }

    public class Utility
    {
        public static decimal ConvertBytes(string b, int iteration)
        {
            long iter = 1;
            for (int i = 0; i < iteration; i++)
                iter *= 1024;
            return Math.Round((Convert.ToDecimal(b)) / Convert.ToDecimal(iter), 2, MidpointRounding.AwayFromZero);
        }
    }

    public class CallBackParam
    {
        private MutipleThreadResetEvent mre;

        public MutipleThreadResetEvent Mre
        {
            get { return mre; }
            set { mre = value; }
        }
        private object param;

        public object Param
        {
            get { return param; }
            set { param = value; }
        }
    }

    public class LogPerformanceData
    {
        private static void GetPostResultSMS(object postData)
        {
            CallBackParam cbp = (CallBackParam)postData;
            string taskId;
            string postSendData = TestObj.CreateTaskId(cbp.Param.ToString(), out taskId);
            string result = TestObj.SMSMessageDoPost(postSendData);
            cbp.Mre.SetOne();
        }

        private static void GetPostResultBatchSMS(object postData)
        {
            CallBackParam cbp = (CallBackParam)postData;
            List<string> taskIdList;
            List<string> expectList;
            string postSendData = TestObj.CreateNTaskId(cbp.Param.ToString(), out taskIdList, out expectList);
            string result = TestObj.SMSMessageDoPost(postSendData);
            cbp.Mre.SetOne();
        }

        private static void GetPostResultMMS(object postData)
        {
            CallBackParam cbp = (CallBackParam)postData;
            string taskId;
            string postSendData = TestObj.CreateTaskId(cbp.Param.ToString(), out taskId);
            string result = TestObj.MMSmessageDoPost(postSendData);
            cbp.Mre.SetOne();
        }

        private static void GetPostResultBatchMMS(object postData)
        {
            CallBackParam cbp = (CallBackParam)postData;
            List<string> taskIdList;
            List<string> expectList;
            string postSendData = TestObj.CreateNTaskId(cbp.Param.ToString(), out taskIdList, out expectList);
            string result = TestObj.MMSmessageDoPost(postSendData);
            cbp.Mre.SetOne();
        }

        private static void GetPostResultEmail(object postData)
        {
            CallBackParam cbp = (CallBackParam)postData;
            SendEmailService.SendEmailSoapClient client = new SendEmailService.SendEmailSoapClient();
            string result = client.postxml(cbp.Param.ToString());
            cbp.Mre.SetOne();
        }

        private static void GetPostResultLMS(object sendSmsR)
        {
            CallBackParam cbp = (CallBackParam)sendSmsR;
            SendLMSService.sendSmsRequest sendsms = (SendLMSService.sendSmsRequest)(cbp.Param);
            SendLMSService.Cmcc_mas_wbsSoapBindingSoapClient slc = new SendLMSService.Cmcc_mas_wbsSoapBindingSoapClient();
            SendLMSService.sendSmsResponse response = slc.sendSms(sendsms);
            cbp.Mre.SetOne();
        }

        public static void LogPerfDataPosAndTsb(List<string> taskIdList, int concurrencyNum, string methodName)
        {
            MemoryInfo.MEMORY_INFO mInfoBegin = MemoryInfo.GetCurrentMemInfo();
            DateTime beginDt = DateTime.Now;
            TestObj.ExcuteTSBandPOSTestRecursive(taskIdList);
            MemoryInfo.MEMORY_INFO mInfoEnd = MemoryInfo.GetCurrentMemInfo();
            DateTime endDt = DateTime.Now;
            
            TimeSpan span = endDt - beginDt;
            int memoryCost = (int)(mInfoBegin.dwAvailPhys - mInfoEnd.dwAvailPhys);

            int seconds = span.Seconds;
            int minutes = span.Minutes;
            int hours = span.Hours;

            double totalSeconds = hours * 3600 + minutes * 60 + seconds;
            double averageCost = totalSeconds / concurrencyNum;

            OupputResult(methodName, totalSeconds, averageCost, concurrencyNum, memoryCost, mInfoEnd);

        }

        public static void LogPerfDataSMSSingleMutiThread(string postData, int concurrencyNum, string methodName)
        {
            MemoryInfo.MEMORY_INFO mInfoBegin = MemoryInfo.GetCurrentMemInfo();
            DateTime beginDt = DateTime.Now;

            using(MutipleThreadResetEvent mtre = new MutipleThreadResetEvent(concurrencyNum))
            {
                for (int i = 1; i <= concurrencyNum; i++)
                {
                    CallBackParam cbp = new CallBackParam();
                    cbp.Mre = mtre;
                    cbp.Param = postData;
                    ThreadPool.QueueUserWorkItem(GetPostResultSMS, cbp);
                }
                mtre.WaitAll();
            }

            MemoryInfo.MEMORY_INFO mInfoEnd = MemoryInfo.GetCurrentMemInfo();
            DateTime endDt = DateTime.Now;

            TimeSpan span = endDt - beginDt;
            int memoryCost = (int)(mInfoBegin.dwAvailPhys - mInfoEnd.dwAvailPhys);

            int seconds = span.Seconds;
            int minutes = span.Minutes;
            int hours = span.Hours;

            double totalSeconds = hours * 3600 + minutes * 60 + seconds;
            double averageCost = totalSeconds / concurrencyNum;

            OupputResult(methodName, totalSeconds, averageCost, concurrencyNum, memoryCost, mInfoEnd);
        }

        public static void LogPerfDataLMSMutiThread(SendLMSService.sendSmsRequest sendSmsR, int concurrencyNum, string methodName)
        {
            MemoryInfo.MEMORY_INFO mInfoBegin = MemoryInfo.GetCurrentMemInfo();
            DateTime beginDt = DateTime.Now;

            using (MutipleThreadResetEvent mtre = new MutipleThreadResetEvent(concurrencyNum))
            {
                for (int i = 1; i <= concurrencyNum; i++)
                {
                    CallBackParam cbp = new CallBackParam();
                    cbp.Mre = mtre;
                    cbp.Param = sendSmsR;
                    ThreadPool.QueueUserWorkItem(GetPostResultLMS, cbp);
                }
                mtre.WaitAll();
            }

            MemoryInfo.MEMORY_INFO mInfoEnd = MemoryInfo.GetCurrentMemInfo();
            DateTime endDt = DateTime.Now;

            TimeSpan span = endDt - beginDt;
            int memoryCost = (int)(mInfoBegin.dwAvailPhys - mInfoEnd.dwAvailPhys);

            int seconds = span.Seconds;
            int minutes = span.Minutes;
            int hours = span.Hours;

            double totalSeconds = hours * 3600 + minutes * 60 + seconds;
            double averageCost = totalSeconds / concurrencyNum;

            OupputResult(methodName, totalSeconds, averageCost, concurrencyNum, memoryCost, mInfoEnd);
        }

        public static void LogPerfDataSMSBatchMutiThread(string postData, int concurrencyNum, string methodName)
        {
            MemoryInfo.MEMORY_INFO mInfoBegin = MemoryInfo.GetCurrentMemInfo();
            DateTime beginDt = DateTime.Now;
          
            using (MutipleThreadResetEvent mtre = new MutipleThreadResetEvent(concurrencyNum))
            {
                for (int i = 1; i <= concurrencyNum; i++)
                {
                    CallBackParam cbp = new CallBackParam();
                    cbp.Mre = mtre;
                    cbp.Param = postData;
                    ThreadPool.QueueUserWorkItem(GetPostResultBatchSMS, cbp);
                }
                mtre.WaitAll();
            }

            MemoryInfo.MEMORY_INFO mInfoEnd = MemoryInfo.GetCurrentMemInfo();
            DateTime endDt = DateTime.Now;

            TimeSpan span = endDt - beginDt;
            int memoryCost = (int)(mInfoBegin.dwAvailPhys - mInfoEnd.dwAvailPhys);

            int seconds = span.Seconds;
            int minutes = span.Minutes;
            int hours = span.Hours;

            double totalSeconds = hours * 3600 + minutes * 60 + seconds;
            double averageCost = totalSeconds / concurrencyNum;

            OupputResult(methodName, totalSeconds, averageCost, concurrencyNum, memoryCost, mInfoEnd);
        }

        public static void LogPerfDataMMSSingleMutiThread(string postData, int concurrencyNum, string methodName)
        {
            MemoryInfo.MEMORY_INFO mInfoBegin = MemoryInfo.GetCurrentMemInfo();
            DateTime beginDt = DateTime.Now;

            using (MutipleThreadResetEvent mtre = new MutipleThreadResetEvent(concurrencyNum))
            {
                for (int i = 1; i <= concurrencyNum; i++)
                {
                    CallBackParam cbp = new CallBackParam();
                    cbp.Mre = mtre;
                    cbp.Param = postData;
                    ThreadPool.QueueUserWorkItem(GetPostResultMMS, cbp);
                }
                mtre.WaitAll();
            }

            MemoryInfo.MEMORY_INFO mInfoEnd = MemoryInfo.GetCurrentMemInfo();
            DateTime endDt = DateTime.Now;

            TimeSpan span = endDt - beginDt;
            int memoryCost = (int)(mInfoBegin.dwAvailPhys - mInfoEnd.dwAvailPhys);

            int seconds = span.Seconds;
            int minutes = span.Minutes;
            int hours = span.Hours;

            double totalSeconds = hours * 3600 + minutes * 60 + seconds;
            double averageCost = totalSeconds / concurrencyNum;

            OupputResult(methodName, totalSeconds, averageCost, concurrencyNum, memoryCost, mInfoEnd);
        }

        public static void LogPerfDataMMSBatchMutiThread(string postData, int concurrencyNum, string methodName)
        {
            MemoryInfo.MEMORY_INFO mInfoBegin = MemoryInfo.GetCurrentMemInfo();
            DateTime beginDt = DateTime.Now;
           
            using (MutipleThreadResetEvent mtre = new MutipleThreadResetEvent(concurrencyNum))
            {
                for (int i = 1; i <= concurrencyNum; i++)
                {
                    CallBackParam cbp = new CallBackParam();
                    cbp.Mre = mtre;
                    cbp.Param = postData;
                    ThreadPool.QueueUserWorkItem(GetPostResultBatchMMS, cbp);
                }
                mtre.WaitAll();
            }

            MemoryInfo.MEMORY_INFO mInfoEnd = MemoryInfo.GetCurrentMemInfo();
            DateTime endDt = DateTime.Now;

            TimeSpan span = endDt - beginDt;
            int memoryCost = (int)(mInfoBegin.dwAvailPhys - mInfoEnd.dwAvailPhys);

            int seconds = span.Seconds;
            int minutes = span.Minutes;
            int hours = span.Hours;

            double totalSeconds = hours * 3600 + minutes * 60 + seconds;
            double averageCost = totalSeconds / concurrencyNum;

            OupputResult(methodName, totalSeconds, averageCost, concurrencyNum, memoryCost, mInfoEnd);
        }

        public static void LogPerfDataEmailMutiThread(string postData, int concurrencyNum, string methodName)
        {
            MemoryInfo.MEMORY_INFO mInfoBegin = MemoryInfo.GetCurrentMemInfo();
            DateTime beginDt = DateTime.Now;

            using (MutipleThreadResetEvent mtre = new MutipleThreadResetEvent(concurrencyNum))
            {
                for (int i = 1; i <= concurrencyNum; i++)
                {
                    CallBackParam cbp = new CallBackParam();
                    cbp.Mre = mtre;
                    cbp.Param = postData;
                    ThreadPool.QueueUserWorkItem(GetPostResultEmail, cbp);
                }
                mtre.WaitAll();
            }

            MemoryInfo.MEMORY_INFO mInfoEnd = MemoryInfo.GetCurrentMemInfo();
            DateTime endDt = DateTime.Now;

            TimeSpan span = endDt - beginDt;
            int memoryCost = (int)(mInfoBegin.dwAvailPhys - mInfoEnd.dwAvailPhys);

            int seconds = span.Seconds;
            int minutes = span.Minutes;
            int hours = span.Hours;

            double totalSeconds = hours * 3600 + minutes * 60 + seconds;
            double averageCost = totalSeconds / concurrencyNum;

            OupputResult(methodName, totalSeconds, averageCost, concurrencyNum, memoryCost, mInfoEnd);
        }

        public static void OupputResult(string methodName, double totalTime, double averageCost,int concurrencyNum, int memoryCost, MemoryInfo.MEMORY_INFO memoryInfo)
        {
            FileStream fs = new FileStream("./HelmPerformanceTestResult.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            try
            {
                if (fs.CanWrite)
                {
                    sw.WriteLine("Excution Time:" + DateTime.Now.ToString());
                    sw.WriteLine("<MethodName: " + methodName + "> <Concurency Request Num : " + concurrencyNum.ToString() + "> <TotalTime : " + totalTime.ToString() + "> <Each Request Avg Time : " + averageCost.ToString() + ">");
                    sw.WriteLine("<"+ memoryInfo.dwMemoryLoad.ToString() + "% of the memory is being used>");
                    sw.WriteLine("<Physical memory total> :" + Utility.ConvertBytes(memoryInfo.dwTotalPhys.ToString(), 3) + "GB");
                    sw.WriteLine("<Availble physical memory> :" + Utility.ConvertBytes(memoryInfo.dwAvailPhys.ToString(), 3) + "GB");
                    sw.WriteLine("<Total cost physical memory> :" + Utility.ConvertBytes(memoryCost.ToString(), 2) + "MB");
                    sw.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Exception exLogPerformanceData = new Exception("LogPerformanceData encounters problem");
                exLogPerformanceData.Source = ex.Source;
                throw exLogPerformanceData;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }
    }
}
