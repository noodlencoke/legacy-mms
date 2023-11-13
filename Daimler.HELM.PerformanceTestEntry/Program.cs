using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daimler.HELM.PerformanceTest;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Configuration;
using Daimler.HELM.Log;
using System.IO;

namespace Daimler.HELM.PerformanceTestEntry
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                bool needRunSMS = bool.Parse(ConfigurationManager.AppSettings["NeedRunSMS"].ToString());
                bool needRunMMS = bool.Parse(ConfigurationManager.AppSettings["NeedRunMMS"].ToString());
                bool needRunEmail = bool.Parse(ConfigurationManager.AppSettings["NeedRunEmail"].ToString());
                bool needRunLMS = bool.Parse(ConfigurationManager.AppSettings["NeedRunLMS"].ToString());
                bool needRunPOS = bool.Parse(ConfigurationManager.AppSettings["NeedRunPOS"].ToString());
                bool needRunTSB = bool.Parse(ConfigurationManager.AppSettings["NeedRunTSB"].ToString());
                string sendModel = ConfigurationManager.AppSettings["SendModel"].ToString();
                if (needRunSMS)
                {
                    Console.WriteLine("SMS Test is running");
                    if (sendModel == "0")
                    {
                        Console.WriteLine("EP_SMS_SingleSend Test is running");
                        EP_SMS_SingleSend smsS = new EP_SMS_SingleSend();
                        smsS.CWIBSMS1Send();
                    }
                    else
                    {
                        Console.WriteLine("EP_SMS_BatchSend Test is running");
                        EP_SMS_BatchSend smsB = new EP_SMS_BatchSend();
                        smsB.CWNewCarSMSNSend();
                    }
                    
                }

                if (needRunMMS)
                {
                    if (sendModel == "0")
                    {
                        Console.WriteLine("MMS Test is running");
                        Console.WriteLine("EP_MMS_SingleSend Test is running");
                        EP_MMS_SingleSend mmsS = new EP_MMS_SingleSend();
                        mmsS.CWIBMMS1Send();
                    }
                    else
                    {
                        Console.WriteLine("EP_MMS_Batch_Send Test is running");
                        EP_MMS_Batch_Send mmsB = new EP_MMS_Batch_Send();
                        mmsB.CWCampaignMMSNSend();
                    }   
                }

                if (needRunEmail)
                {
                    if (sendModel == "0")
                    {
                        Console.WriteLine("Email Test is running");
                        Console.WriteLine("EP_Email_Sing_Send Test is running");
                        EP_Email_Sing_Send emailS = new EP_Email_Sing_Send();
                        emailS.CWIBEDM1Send();

                    }
                    else
                    {
                        Console.WriteLine("EP_Email_Batch_Send Test is running");
                        EP_Email_Batch_Send emailB = new EP_Email_Batch_Send();
                        emailB.CWInvitationEDMNSend();
                    }   
                }

                if (needRunLMS)
                {
                    Console.WriteLine("LMS Test is running");
                    Console.WriteLine("EP_LMS_Sing_Send Test is running");
                    EP_LMS_Sing_Send lmsS = new EP_LMS_Sing_Send();
                    lmsS.CWIBLMS1Send();
                }

                if (needRunPOS)
                {
                    Console.WriteLine("POS Test is running");
                    Console.WriteLine("Pos_SMS_SingleSend Test is running");
                    Pos_SMS_SingleSend psmsS = new Pos_SMS_SingleSend();
                    psmsS.PoSGetCustomerfollowup();
                }

                if (needRunTSB)
                {
                    Console.WriteLine("TSB Test is running");
                    Console.WriteLine("TSB_SMS_Single_Send Test is running");
                    TSB_SMS_Single_Send tsmsS = new TSB_SMS_Single_Send();
                    tsmsS.TSBInterPhoneNumber();
                }
            }
            catch (Exception ex)
            {   
                StringBuilder sb=new StringBuilder();
                sb.Append("Execution encounters problems.\r\n");
                sb.Append("Message:" + ex.Message+"\r\n");
                sb.Append("InnerException:" + ex.InnerException+"\r\n");
                sb.Append("StackTrace:" + ex.StackTrace);

                using (StreamWriter sw=new StreamWriter(@"C:\PerformanceTesting_log.txt"))
                {
                    sw.Write(sb.ToString());
                }
                    
                Console.WriteLine("Execution encounters problems.");
                Console.WriteLine("Message:" + ex.Message);
                Console.WriteLine("InnerException:" + ex.InnerException);
                Console.WriteLine("StackTrace:" + ex.StackTrace);
                
            }
            finally
            {
                Console.WriteLine("Helm Performance Tesing Finished");
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("You can open HelmPerformanceTestResult.txt to see more detail.");
                Console.ReadLine();
            }
        }
    }
}
