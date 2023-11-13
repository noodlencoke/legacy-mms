using Daimler.HELM.Adapter.Logic;
using Daimler.HELM.BizObjects;
using Daimler.HELM.EmailInterface;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using Daimler.HELM.Log;
using Daimler.HELM.MessageInterface.Impl;
using Daimler.HELM.MessageQueueReply;
using Daimler.HELM.MQ;
using Daimler.HELM.WechatInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Daimler.HELM.Test
{
    public class Program
    {
        static void Main(string[] args)
        {

            
            Simulator();

        }

        private static void Simulator()
        {
            string isContinue = "Y";
            do
            {

                Console.WriteLine("select options as follow:");
                Console.WriteLine("0: Others test ");
                Console.WriteLine("1: EP - Single SMS");
                Console.WriteLine("2: EP - Batch SMS");
                Console.WriteLine("3: EP - Single MMS");
                Console.WriteLine("4: EP - Batch MMS");
                Console.WriteLine("5: EP - Single Email");
                Console.WriteLine("6: EP - Batch Email");
                Console.WriteLine("7: MeStore - Single SMS");
                Console.WriteLine("8: LMS - Single SMS");
                Console.WriteLine("9: Repeat Reply");

                string option = Console.ReadLine();
                if (option == "0")
                {
                    DoOtherTest();
                }
                else if (option == "1")
                {
                    Console.WriteLine("start test ep send single sms.");
                    string sendSMSUrl = System.Configuration.ConfigurationManager.AppSettings["SendSMSURL"];
                    EP_SendSingleSMS(sendSMSUrl);
                }
                else if (option == "2")
                {
                    Console.WriteLine("start test ep send batch sms.");
                    string sendSMSUrl = System.Configuration.ConfigurationManager.AppSettings["SendSMSURL"];
                    EP_SendBatchSMS(sendSMSUrl);
                }
                else if (option == "3")
                {
                    Console.WriteLine("start test ep send single mms.");
                    string sendMMSUrl = System.Configuration.ConfigurationManager.AppSettings["SendMMSURL"];
                    EP_SendSingleMMS(sendMMSUrl);
                }
                else if (option == "4")
                {
                    Console.WriteLine("start test ep send batch mms.");
                    string sendMMSUrl = System.Configuration.ConfigurationManager.AppSettings["SendMMSURL"];
                    EP_SendBatchMMS(sendMMSUrl);
                }
                else if (option == "5")
                {
                    EP_SendSingleEmail();
                }
                else if (option == "6")
                {
                    EP_SendBatchEmail();
                }
                else if (option == "7")
                {
                    Console.WriteLine("start test mestore send single sms.");
                    string publicServiceUrl = System.Configuration.ConfigurationManager.AppSettings["PublicServiceUrl"];
                    MeStore_SendSingleSMS(publicServiceUrl);
                }
                else if (option == "8")
                {
                    Console.WriteLine("start test lms send single sms.");
                    LMS_SendSingleSMS();
                }
                else if (option == "9")
                {
                    Console.WriteLine("start test repeat reply message ...");
                    RepeatRely();
                }



                Console.WriteLine("continue ? (Y/N)");
                isContinue = Console.ReadLine();

            } while (isContinue.ToLower().Equals("y"));

            string url = "http://localhost:62188/SMSSendMessage.aspx";
            url = "http://localhost:62188/MMSSendMessage.aspx";
            url = "http://112.81.47.8:8080/SMSSendMessage.aspx";
        }


        private static void EP_SendSingleSMS(string url)
        {
            string postData = string.Format("username=benzapi&productId=2884&phone_taskId_content=18610285539/@/#/{0}/@/#/Auto Test IBSMS1_尊敬的阁下,感谢您对smart的支持与厚爱。您可以将客服中心联系方式保存在您的通讯录中，以便您能在遇到任何与smart相关的问题时随时致电咨询。7*24小时联系方式：4008110000。以便我们能够在第一时间邀请您参加smart各类专属体验，来电号码为021-55609988，客服中心将竭诚为您提供最优质的服务体验。 回复TD退订[auto_test:处理失败] <1>&password=111111", Guid.NewGuid().ToString());
            TestObj.DoPost(url, postData);
        }

        private static void EP_SendBatchSMS(string url)
        {
            //string postData = "username=benzapi&productId=3161&phone_taskId_content=1340100/@/#/00TO000000EPAi7MAH/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285539/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285539/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/136611543a2/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/13810933708/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285569/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285542/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285540/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285541/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285543/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285544/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285570/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285545/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285546/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285547/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285548/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285549/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285550/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285551/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285553/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285552/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285554/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285556/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285555/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285557/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285558/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285559/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285560/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285561/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订/r/n/18610285562/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加 回复0000退订&password=111111";
            string postData = "username=benzapi&productId=3161&phone_taskId_content=1340100/@/#/00TO000000EPAi7MAH/@/#/明天的会议你参加么？ 1参加 2不参加[test message] 回复0000退订/r/n/18610285539/@/#/" + Guid.NewGuid() + "/@/#/今天吃饭去吗？ 1参加 2不参加[test message] 回复0000退订/r/n/18612074739/@/#/" + Guid.NewGuid() + "/@/#/去旅游吗？ 1参加 2不参加[test message] 回复0000退订/r/n/13401002145/@/#/" + Guid.NewGuid() + "/@/#/去玩儿吗？ 1参加 2不参加[test message] 回复0000退订/r/n/18701687815/@/#/" + Guid.NewGuid() + "/@/#/明天的会议你参加么？ 1参加 2不参加[test message] 回复0000退订&password=111111";
            TestObj.DoPost(url, postData);
        }

        private static void EP_SendSingleMMS(string url)
        {
            string singleMMSData = "username=benzmms&productId=3157&phone_taskId_content=18610285539/@/#/f6727604060741c7a0d61ee03b184765/@/#/110[auto_test:提交成功] <0>&password=benzmms";
            TestObj.DoPost(url, singleMMSData);
        }


        private static void EP_SendBatchMMS(string url)
        {
            string postMMSData = "username=benzmms&productId=3157&phone_taskId_content=18612074739/@/#/" + Guid.NewGuid() + "/@/#/a1S90000000O4ok/r/n/18813112975/@/#/" + Guid.NewGuid() + "/@/#/a1S90000000O4ok/r/n/13401002145/@/#/" + Guid.NewGuid() + "/@/#/a1S90000000O4ok/r/n/18610285539/@/#/" + Guid.NewGuid() + "/@/#/a1S90000000O4ok/r/n/13661154392/@/#/" + Guid.NewGuid() + "/@/#/a1S90000000O4ok/r/n/13810928561/@/#/" + Guid.NewGuid() + "/@/#/a1S90000000O4ok/r/n/13810933708/@/#/" + Guid.NewGuid() + "/@/#/a1S90000000O4ok/r/n/18701687815/@/#/" + Guid.NewGuid() + "/@/#/a1S90000000O4ok&password=benzmms";
            TestObj.DoPost(url, postMMSData);
        }

        private static void MeStore_SendSingleSMS(string url)
        {

            //string url = "https://helm-uat.mercedes-benz.com.cn";
            //string url = "http://localhost:50455/";
            string tokenObj = TestObj.DoPost(string.Format("{0}/getToken?dataSource={1}&accountId={2}&securityKey={3}", url, "MStore", "acc1", "1qaz2wsx"), "");
            StringReader sr = new StringReader(tokenObj);
            XmlDocument xd = new XmlDocument();
            xd.Load(sr);
            sr.Close();
            sr.Dispose();

            XmlNode node = xd.SelectSingleNode("xml/accessToken");
            string accessToken = node.InnerText;

            string xmlContent = @"<xml>
                            <smsSource>MStore</smsSource>
                            <sendSmsRequest>
                            <uniqueID>" + Guid.NewGuid() + @"</uniqueID>
                            <senderName>sender</senderName>
                            <recipientMobile>18610285539</recipientMobile>
                            <smsText>This is a mestore test message</smsText>
                            <smsPriority>1</smsPriority>
                            </sendSmsRequest>
                            </xml>";
            url = string.Format("{0}/sendSMS?accessToken={1}", url, System.Web.HttpUtility.UrlEncode(accessToken));
            TestObj.DoPost(url, xmlContent);
            Console.WriteLine(1);
        }

        private static void LMS_SendSingleSMS()
        {
            LMSSendService2.sendSmsRequest sendSmsR = new LMSSendService2.sendSmsRequest();
            sendSmsR.ApplicationID = "BCZG";
            sendSmsR.DestinationAddresses = new string[] { "tel:1355286xxxx" };
            sendSmsR.ExtendCode = "8";
            sendSmsR.Message = "Liu Lian Bo - <LMS Test Message>";
            sendSmsR.MessageFormat = LMSSendService2.MessageFormat.UCS2;
            sendSmsR.SendMethod = LMSSendService2.SendMethodType.Normal;
            sendSmsR.DeliveryResultRequest = false;
            LMSSendService2.Cmcc_mas_wbsSoapBindingSoapClient slc = new LMSSendService2.Cmcc_mas_wbsSoapBindingSoapClient();
            LMSSendService2.sendSmsResponse response = slc.sendSms(sendSmsR);
            Console.WriteLine(response.RequestIdentifier);
        }


        private static void DoOtherTest()
        {
            TestReplyMessageToEP();

            //string appId = System.Configuration.ConfigurationManager.AppSettings["AppId"];
            //string secret = System.Configuration.ConfigurationManager.AppSettings["Secret"];
            //bool bol = SetMenu.CreateMenuOfWeChat(appId, secret);
            //Console.WriteLine(bol);


        }

        private static void RepeatRely()
        {
            SourceChannerReply.RepeatReplyToBS();
        }

        private static void TestReplyMessageToEP()
        {
            try
            {
                Console.WriteLine("please input the message taskId :");
                string taskId = Console.ReadLine();
                Console.WriteLine("plase input the reply content:");
                string replyContent = Console.ReadLine();
                MessageReplyInfo replyInfo = new MessageReplyInfo();
                replyInfo.taskId = taskId;
                replyInfo.content = replyContent;
                replyInfo.getTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                BaseReplyHandler replyHandler = MessageReplyFactory.CreateMessageReply("EP");
                CommonResult result = replyHandler.DoReply(replyInfo);
                if (result.IsOK)
                {
                    Console.WriteLine("Reply successfull !");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static void EP_SendSingleEmail()
        {
            string postData = @"<?xml version='1.0' encoding='UTF-8' ?> 
                                 <userspec>
                                <users>
                                 <user>
                                  <email>MTIxMjI1MTM1QHFxLmNvbQ==</email> 
                                  <name>5YiYIOi/nuazog==</name> 
                                <emailtemplateid>MTA5NlthdXRvX3Rlc3Q65Y+R6YCB5aSx6LSlXSgxKQ==</emailtemplateid> 
                                  <interestedseries>QS1DbGFzcw==</interestedseries> 
                                  </user>
                                  </users>
                                  </userspec>";

            EPSendEmail.SendEmailSoapClient sendEmail = new EPSendEmail.SendEmailSoapClient();
            string xml = sendEmail.postxml(postData);
            Console.WriteLine(xml);
        }

        private static void EP_SendBatchEmail()
        {
            string postData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><sendspec><sendtime>MjAxNS0xMS0yN1QxNzo0MToyNw==</sendtime><target><allowed><users><user><email>MTIxMjI1MTM1QHFxLmNvbQ==</email><name>5L2VIOePiuePig==</name></user><user><email>bGlhbmJvLmxpdUBkYWltbGVyLmNvbQ==</email><name>6L+e5rOiIOWImA==</name></user><user><email>ZnJhbmNrLmxpQGJlcnRlbHNtYW5uLmNvbS5jbg==</email><name>5p2OIOaWueiIuA==</name></user><user><email>dG9uZ2xpYW5nLmxpQGNhcGdlbWluaS5jb20=</email><name>5p2OIOWQjOS6rg==</name></user><user><email>bW8ucWlAZGFpbWxlci5jb20=</email><name>56WBIOa8oA==</name></user><user><email>Ym8uaHVhbmdAY2FwZ2VtaW5pLmNvbQ==</email><name>6buEIOWNmg==</name></user><user><email>Y3BjLmFsbGVrb3R0ZUBkYWltbGVyLmNvbQ==</email><name>Tm9yYTE=</name></user><user><email>bi5hbGxla290dGVAY3BjLWFnLmRl</email><name>Tm9yYQ==</name></user><user><email>Ym8tYmouemhhbmdAY2FwZ2VtaW5pLmNvbQ==</email><name>5bygIOazog==</name></user><user><email>c2hhbnNoYW4uaGVAY2FwZ2VtaW5pLmNvbQ==</email><name>5L2VIOWls+WjqyjmtYvor5Xlj7cp</name></user><user><email>bGl4ZHAxODg0QDE2My5jb20=</email><name>5YiYIOS8nw==</name></user></users></allowed></target><emailspec><from>Y3VzdG9tZXJfc2VydmljZUBlbWFpbC5jb250YWN0Lm1lcmNlZGVzLWJlbnouY29tLmNuOuaihei1m+W+t+aWry3lpZTpqbA=</from><reply>Y3VzdG9tZXJfc2VydmljZUBlbWFpbC5jb250YWN0Lm1lcmNlZGVzLWJlbnouY29tLmNuOuaihei1m+W+t+aWry3lpZTpqbA=</reply><subject>Q0FSMlNIQVJF6ZqP5b+D5byA5byA5ZCv5YiG5pe255So6L2m5paw5L2T6aqM</subject><zip>ZURNX05vdl8xMTEx</zip></emailspec></sendspec>";

            EPSendEmail.SendEmailSoapClient sendEmail = new EPSendEmail.SendEmailSoapClient();
            string xml = sendEmail.postxml(postData);
            Console.WriteLine(xml);
        }



        private static bool CheckDic(ref Dictionary<string, List<MessageInfo>> listDic, MessageInfo msgInfo)
        {
            foreach (string key in listDic.Keys)
            {

            }
            return true;
        }

        private static void WriteLog(object msg)
        {
            Thread.Sleep(1000);
            LogHandler.WriteLog(msg.ToString());
        }




        public static int SendSingleSMS(string content)
        {
            try
            {
                using (StringReader txtReader = new StringReader(content))
                {

                    string errorMsg = ValidateXML(txtReader);
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        return 1;
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return 1;
        }

        private static string ValidateXML(StringReader txtReader)
        {
            string errorMsg = string.Empty;
            try
            {
                //validate the xml formate
                string schemaFile = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\SMSMessage.xsd";
                //string xmlFile = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\SMSMessage.xml";
                string namespaceUrl = "http://tempuri.org/OrderSchema.xsd";

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, schemaFile);
                settings.ValidationEventHandler += (obj, e) =>
                {
                    errorMsg += e.Message;
                };

                using (XmlReader xmlReader = XmlReader.Create(txtReader, settings))
                {
                    try
                    {
                        xmlReader.MoveToContent();
                        while (xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.Document && xmlReader.NamespaceURI != namespaceUrl)
                            {
                                errorMsg = "The xml formate incorrect";
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMsg += ex.Message;
                    }
                    finally
                    {
                        xmlReader.Close();
                        xmlReader.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg += ex.Message;
            }

            return errorMsg;
        }


    }
}
