using Daimler.HELM.UnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestSendEmail.SendEmailSoapClient sendEmial = new TestSendEmail.SendEmailSoapClient();
            //sendEmial.postxml("my test for send email!");
            //List<string> testList = TestObj.GetCaseFromXls(@"D:\Projects\Daimler\helm\5.Test Management\Test Case\UnitTestCase.xls", "CW_NewCar_SMS_N_Send()");
            //List<string> taskIdList=new List<string>();
            //List<string> postList = new List<string>();
            //foreach (var item in testList)
            //{
            //    postList.Add(TestObj.CreateNTaskId(item, out taskIdList));
            //}

            SMSend2endTest test = new SMSend2endTest();
            test.CwIBSurveySMS1Send();
           Console.ReadLine();
            
        }
    }
}
