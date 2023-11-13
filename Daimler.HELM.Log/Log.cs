using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.Log
{
    public static class LogHandler
    {
        private static object lockObj = new object();
        private static string logPath = "c:\\inetpub\\HELMLog";
        public static void WriteLog(string msg)
        {
            lock (lockObj)
            {
                string fileName = string.Format("{0}\\WinServiceLog_{1}.txt", logPath, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                string strWord = string.Empty;
                if (File.Exists(fileName))
                {
                    using (StreamWriter swAppend = File.AppendText(fileName))
                    {
                        strWord = string.Format("{0}   {1}\r\n", msg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        swAppend.Write(strWord);
                        swAppend.Close();
                        swAppend.Dispose();
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        strWord = string.Format("{0}   {1}\r\n", msg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        sw.Write(strWord);
                        sw.Close();
                        sw.Dispose();
                    }
                }
            }
        }


        public static void WriteLog(string msgType,string msg)
        {
            lock (lockObj)
            {
                string fileName = string.Format("{0}\\{1}-WinServiceLog_{2}.txt", logPath, msgType, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                string strWord = string.Empty;
                if (File.Exists(fileName))
                {
                    using (StreamWriter swAppend = File.AppendText(fileName))
                    {
                        strWord = string.Format("{0}   {1}\r\n", msg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        swAppend.Write(strWord);
                        swAppend.Close();
                        swAppend.Dispose();
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        strWord = string.Format("{0}   {1}\r\n", msg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        sw.Write(strWord);
                        sw.Close();
                        sw.Dispose();
                    }
                }
            }
        }


        public static string ReadLog(string msgType)
        {
            lock (lockObj)
            {
                string strReturn = string.Empty;
                string fileName = string.Format("{0}\\{1}.txt", logPath, msgType);
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }
                using (StreamReader reader = new StreamReader(fileName, Encoding.Default))
                {
                    String line = reader.ReadLine();
                    while (line != null)
                    {
                        strReturn += line.ToString();
                        line = reader.ReadLine();
                    }
                    reader.Close();
                    reader.Dispose();
                }
                return strReturn;
            }
        }
    }
}
