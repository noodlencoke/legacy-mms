using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Daimler.HELM.MessageInterface.Impl
{
    public class MockVendor : ISMSInterface, IMMSInterface, IEmailInterface
    {
        private static readonly string _conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();


        public BizObjects.RequestResultInfo SendSMS(BizObjects.MessageInfo msgInfo)
        {

            string _smsId = Guid.NewGuid().ToString().Replace("-", "");
            string _phones = msgInfo.Number;
            string _smsContent = msgInfo.Content;
            string _sendTime = DateTime.Now.ToString("yyyyMMdd");
            string _sign = "";
            string _subCode = msgInfo.ExtendNumber;

            string responseMsg = MockSubmitReport(msgInfo.Number, Guid.NewGuid().ToString().Replace("-", ""), msgInfo.Content);

            #region Set submit result

            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(responseMsg);
                //将提交返回的报告封装成 提交报告对象
                RequestResultInfo returnResult = new RequestResultInfo();
                int result = Convert.ToInt32(xdoc.SelectNodes("/response/result").Item(0).InnerText);
                string blackList = xdoc.SelectNodes("/response/blacklist").Item(0).InnerText.ToString();


                returnResult.TaskId = xdoc.SelectNodes("/response/msgid").Item(0).InnerText.ToString();
                returnResult.SMSStatus = blackList == "" ? GetSubmitStatus(result) : MessageStatus.InvalidMobileNumber;
                returnResult.Desc = xdoc.SelectNodes("/response/desc").Item(0).InnerText.ToString();
                returnResult.BlackList = blackList;
                returnResult.StrResult = responseMsg;
                returnResult.SenderNumber = "1069XXXX" + _subCode;
                return returnResult;


            }
            catch (Exception ex)
            {

                return null;
            }
            #endregion

        }
        public List<RequestResultInfo> SendSMS(List<MessageInfo> msgList)
        {
            List<RequestResultInfo> requestResultList = new List<RequestResultInfo>();

            foreach (var item in msgList)
            {
                string responseMsg = MockSubmitReport(item.Number, Guid.NewGuid().ToString().Replace("-", ""), item.Content);

                string _subCode = item.ExtendNumber;
                XmlDocument xdoc = new XmlDocument();
                try
                {
                    xdoc.LoadXml(responseMsg);
                    //将提交返回的报告封装成 提交报告对象
                    RequestResultInfo returnResult = new RequestResultInfo();
                    int result = Convert.ToInt32(xdoc.SelectNodes("/response/result").Item(0).InnerText);
                    string blackList = xdoc.SelectNodes("/response/blacklist").Item(0).InnerText.ToString();


                    returnResult.TaskId = xdoc.SelectNodes("/response/msgid").Item(0).InnerText.ToString();
                    returnResult.SMSStatus = blackList == "" ? GetSubmitStatus(result) : MessageStatus.InvalidMobileNumber;
                    returnResult.Desc = xdoc.SelectNodes("/response/desc").Item(0).InnerText.ToString();
                    returnResult.BlackList = blackList;
                    returnResult.StrResult = responseMsg;
                    returnResult.SenderNumber = "1069XXXX" + _subCode;
                    returnResult.Mobile = item.Number;
                    requestResultList.Add(returnResult);
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
            return requestResultList;
        }
        public BizObjects.SendingResult GetSMSSendingStatus(BizObjects.MessageInfo msgInfo)
        {

            string responseMsg = MockSendReport(msgInfo.Number, msgInfo.VendorTaskId, msgInfo.Content);

            #region Set Submit Result

            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(responseMsg);

                if (xdoc.SelectNodes("/response/result").Item(0).InnerText == "0" && xdoc.SelectNodes("/response/report").Count != 0)
                {
                    SendingResult returnResult = new SendingResult();
                    int stats = Convert.ToInt32(xdoc.SelectNodes("/response/report/status").Item(0).InnerText);
                    string desc = xdoc.SelectNodes("/response/report/desc").Item(0).InnerText;
                    string number = xdoc.SelectNodes("/response/report/phone").Item(0).InnerText;
                    string taskId = xdoc.SelectNodes("/response/report/msgid").Item(0).InnerText;
                    string sendTime = xdoc.SelectNodes("/response/report/time").Item(0).InnerText;


                    returnResult.TaskId = taskId;
                    returnResult.Number = number;
                    returnResult.Status = GetSendStatus(stats, desc);
                    returnResult.Desc = desc;
                    returnResult.SendTime = sendTime;
                    returnResult.StrSendingResult = responseMsg;
                    return returnResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                Daimler.HELM.Log.LogHandler.WriteLog(responseMsg);
                return null;
            }

            #endregion

        }
         
        public List<SendingResult> GetSMSSendingStatus(string accountName, string accountPwd)
        {
            throw new NotImplementedException();
        }

        public RequestResultInfo SendMMS(BizObjects.MessageInfo msgInfo)
        {
            StringBuilder param = new StringBuilder();
            string message = "";
            string _smsId = Guid.NewGuid().ToString().Replace("-", "");
            string _phones = msgInfo.Number;
            string _templatedId = msgInfo.TemplateId;
            string _subCode = msgInfo.ExtendNumber;

            RequestResultInfo returnResult = new RequestResultInfo();
            returnResult.TaskId = Guid.NewGuid().ToString().Replace("-", "");
            returnResult.SMSStatus = MessageStatus.SubmitSuccess;
            returnResult.SenderNumber = "1069XXXX" + _subCode;

            return returnResult;

        }
        public List<RequestResultInfo> SendMMS(List<MessageInfo> msgInfo)
        {
            List<RequestResultInfo> requestResultList = new List<RequestResultInfo>();
            string taskId = Guid.NewGuid().ToString().Replace("-", "");
            foreach (var item in msgInfo)
            {
                RequestResultInfo returnResult = new RequestResultInfo();
                returnResult.TaskId = taskId;
                returnResult.SMSStatus = MessageStatus.SubmitSuccess;
                returnResult.Mobile = item.Number;
                returnResult.SenderNumber = "1069XXXX" + item.ExtendNumber;
                requestResultList.Add(returnResult);
            }
            return requestResultList;
            // throw new NotImplementedException();
        }
        public SendingResult GetMMSSendingStatus(MessageInfo msgInfo)
        {
            SendingResult sendResult = new SendingResult();
            sendResult.TaskId = msgInfo.VendorTaskId;
            sendResult.Number = msgInfo.Number;
            sendResult.Status = GetMMSStatus(msgInfo.Content);
            sendResult.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            return sendResult;
        }

        public List<SendingResult> GetMMSSendingStatus(string accountName, string accountPwd)
        {
            throw new NotImplementedException();
        }


        //2015-9-12-liuwei
        public BizObjects.RequestResultInfo SendEmail(BizObjects.MessageInfo msgInfo)
        {

            string receiverEmail = msgInfo.Number;

            string content = msgInfo.TemplateId;
            Daimler.HELM.BizObjects.RequestResultInfo requestResultInfo = new BizObjects.RequestResultInfo();
            if (content.Contains('['))
            {
                string testContent = content.Substring(content.LastIndexOf('[') + 1, 15);

                switch (testContent)
                {
                    case "auto_test:发送成功]":
                        requestResultInfo.Mobile = msgInfo.Number;
                        requestResultInfo.EmailStatus = EmailStatus.SendSuccess;
                        requestResultInfo.TaskId = Guid.NewGuid() + "&" + receiverEmail;
                        requestResultInfo.Desc = "Mock send success";
                        break;
                    default:
                        requestResultInfo.Mobile = msgInfo.Number;
                        requestResultInfo.EmailStatus = EmailStatus.SendError;
                        requestResultInfo.Desc = "Mock send fail";
                        break;
                }
            }
            else
            {
                requestResultInfo.Mobile = msgInfo.Number;
                requestResultInfo.EmailStatus = EmailStatus.SendSuccess;
                requestResultInfo.TaskId = Guid.NewGuid() + "&" + receiverEmail;
                requestResultInfo.Desc = "Mock send success";
            }

            return requestResultInfo;
        }
        public List<RequestResultInfo> SendEmail(List<MessageInfo> msgInfo)
        {
            List<RequestResultInfo> emailRequestList = new List<RequestResultInfo>();
            foreach (var item in msgInfo)
            {
                string receiverEmail = item.Number;

                string content = item.TemplateId;
                Daimler.HELM.BizObjects.RequestResultInfo requestResultInfo = new BizObjects.RequestResultInfo();
                if (content.Contains('['))
                {
                    string testContent = content.Substring(content.LastIndexOf('[') + 1, 15);

                    switch (testContent)
                    {
                        case "auto_test:发送成功]":
                            requestResultInfo.Mobile = item.Number;
                            requestResultInfo.EmailStatus = EmailStatus.SendSuccess;
                            requestResultInfo.TaskId = Guid.NewGuid() + "&" + receiverEmail;
                            requestResultInfo.Desc = "Mock send success";
                            break;
                        default:
                            requestResultInfo.Mobile = item.Number;
                            requestResultInfo.EmailStatus = EmailStatus.SendError;
                            requestResultInfo.Desc = "Mock send fail";
                            break;
                    }
                }
                else
                {
                    requestResultInfo.Mobile = item.Number;
                    requestResultInfo.EmailStatus = EmailStatus.SendSuccess;
                    requestResultInfo.TaskId = Guid.NewGuid() + "&" + receiverEmail;
                    requestResultInfo.Desc = "Mock send success";
                }

                emailRequestList.Add(requestResultInfo);
            }
            return emailRequestList;

        }
        //----2015-9-12-liuwei
        public List<MessageReplyInfo> GetReply(string startDt, string endDate)
        {
            List<MessageReplyInfo> replyList = new List<MessageReplyInfo>();

            string constr = _conStr;
            string sql = "select * from dbo.SMSSendInfo where content like '%test_reply%'";

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandTimeout = 300;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string messageId = dt.Rows[i]["id"].ToString();
                if (!HadReply(messageId))
                {
                    if (dt.Rows[i]["content"].ToString().Contains("<Y>"))
                    {
                        string subCode = dt.Rows[i]["extendNumber"].ToString();
                        MessageReplyInfo replyInfo = new MessageReplyInfo();
                        replyInfo.mobile = dt.Rows[i]["mobile"].ToString();
                        replyInfo.content = "Y";
                        replyInfo.sessionId = subCode;
                        replyInfo.extendNumber = subCode;
                        replyInfo.senderNumber = "1069XXXX" + subCode;
                        replyInfo.getTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                        replyInfo.dataSource = "mock reply";
                        replyList.Add(replyInfo);
                    }
                    else if (dt.Rows[i]["content"].ToString().Contains("<N>"))
                    {
                        string subCode = dt.Rows[i]["extendNumber"].ToString();
                        MessageReplyInfo replyInfo = new MessageReplyInfo();
                        replyInfo.mobile = dt.Rows[i]["mobile"].ToString();
                        replyInfo.content = "N";
                        replyInfo.sessionId = subCode;
                        replyInfo.extendNumber = subCode;
                        replyInfo.senderNumber = "1069XXXX" + subCode;
                        replyInfo.getTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                        replyInfo.dataSource = "mock reply";
                        replyList.Add(replyInfo);
                    }
                }

            }
            return replyList;
        }
        public static bool HadReply(string msgId)
        {

            string constr = _conStr;
            string sql = "select COUNT(id) from dbo.SMSReplyInfo where messageId='" + msgId + "'";
            int count = 0;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            if (count > 0)
            { return true; }
            else
            { return false; }
        }




        public static string MockSubmitReport(string phoneNum, string msgid, string content)
        {
            string responseReport = "<?xml version='1.0' encoding='utf-8'?>"
                                + "<response>"
                                + "<msgid>" + msgid + "</msgid>"
                                + "<result>{0}</result>"
                                + "<desc>{1}</desc>"
                                + "<blacklist>{2}</blacklist>"
                                + "<failPhones></failPhones>"
                                + "</response>";

            if (content.Contains('['))
            {
                string testContent = content.Substring(content.LastIndexOf('[') + 1, 15);
                switch (testContent)
                {

                    case "auto_test:成功提交]":
                        responseReport = string.Format(responseReport, "0", "提交成功", "");
                        break;
                    default:
                        responseReport = string.Format(responseReport, "0", "提交成功了", "");
                        break;
                }
            }
            else
            { responseReport = string.Format(responseReport, "0", "提交成功", ""); }

            return responseReport;
        }
        public static string MockSendReport(string phoneNum, string msgid, string content)
        {
            string responseReport = "<?xml version='1.0' encoding='UTF-8'?>"
                                  + "<response>"
                                        + "<result>0</result>"
                                        + "<desc>提交成功</desc>"
                                        + "<report>"
                                            + "<msgid>" + msgid + "</msgid>"
                                            + "<phone>" + phoneNum + "</phone>"
                                            + "<status>{0}</status>"
                                            + "<desc>{1}</desc>"

                                            + "<wgcode></wgcode>"
                                            + "<time>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "</time>"
                                        + "</report>"
                                  + "</response>";
            if (content.Contains('['))
            {
                string testContent = content.Substring(content.LastIndexOf('[') + 1, 15);

                #region GET STATUS FROM CONTENT
                switch (testContent)
                {
                    case "auto_test:提交成功]":
                        responseReport = string.Format(responseReport, "0", "成功");
                        break;
                    case "auto_test:手机号码无":
                        responseReport = string.Format(responseReport, "1", "4");
                        break;
                    case "auto_test:签名不合法":
                        responseReport = string.Format(responseReport, "1", "5");
                        break;
                    case "auto_test:短信内容超":
                        responseReport = string.Format(responseReport, "1", "6");
                        break;
                    case "auto_test:请求来源地":
                        responseReport = string.Format(responseReport, "1", "9");
                        break;
                    case "auto_test:内容包含敏":
                        responseReport = string.Format(responseReport, "1", "10");
                        break;
                    case "auto_test:余额不足]":
                        responseReport = string.Format(responseReport, "1", "11");
                        break;
                    case "auto_test:购买产品或":
                        responseReport = string.Format(responseReport, "1", "12");
                        break;
                    case "auto_test:账号被禁用":
                        responseReport = string.Format(responseReport, "1", "13");
                        break;
                    case "auto_test:不支持该运":
                        responseReport = string.Format(responseReport, "1", "14");
                        break;
                    case "auto_test:发送号码数":
                        responseReport = string.Format(responseReport, "1", "16");
                        break;
                    case "auto_test:黑名单号码":
                        responseReport = string.Format(responseReport, "1", "19");
                        break;
                    case "auto_test:该模板ID已":
                        responseReport = string.Format(responseReport, "1", "20");
                        break;
                    case "auto_test:非法模板ID":
                        responseReport = string.Format(responseReport, "1", "21");
                        break;
                    case "auto_test:不支持的MS":
                        responseReport = string.Format(responseReport, "1", "22");
                        break;
                    case "auto_test:子号码无效":
                        responseReport = string.Format(responseReport, "1", "23");
                        break;
                    case "auto_test:内容为空]":
                        responseReport = string.Format(responseReport, "1", "24");
                        break;
                    case "auto_test:号码为空]":
                        responseReport = string.Format(responseReport, "1", "25");
                        break;
                    case "auto_test:单个号码相":
                        responseReport = string.Format(responseReport, "1", "26");
                        break;
                    case "auto_test:单个号码次":
                        responseReport = string.Format(responseReport, "1", "27");
                        break;
                    case "auto_test:处理失败]":
                        responseReport = string.Format(responseReport, "1", "96");
                        break;
                    case "auto_test:接入方式错":
                        responseReport = string.Format(responseReport, "1", "97");
                        break;
                    case "auto_test:系统繁忙]":
                        responseReport = string.Format(responseReport, "1", "98");
                        break;
                    case "auto_test:消息格式错":
                        responseReport = string.Format(responseReport, "1", "99");
                        break;

                    case "auto_test:短信内容为":
                        responseReport = string.Format(responseReport, "1", "2021");
                        break;
                    case "auto_test:手机号码个":
                        responseReport = string.Format(responseReport, "1", "205");
                        break;
                    case "auto_test:手机号码为":
                        responseReport = string.Format(responseReport, "1", "2014");
                        break;
                    case "auto_test:系统正忙]":
                        responseReport = string.Format(responseReport, "1", "2098");
                        break;
                    case "auto_test:账号无效]":
                        responseReport = string.Format(responseReport, "1", "201");
                        break;
                    case "auto_test:密码错误]":
                        responseReport = string.Format(responseReport, "1", "202");
                        break;
                    case "auto_test:msgid":
                        responseReport = string.Format(responseReport, "1", "203");
                        break;
                    case "auto_test:错误号码/":
                        responseReport = string.Format(responseReport, "1", "204");
                        break;
                    case "auto_test:扩展子码无":
                        responseReport = string.Format(responseReport, "1", "207");
                        break;
                    case "auto_test:定时时间格":
                        responseReport = string.Format(responseReport, "1", "208");
                        break;
                    case "auto_test:用户被禁发":
                        responseReport = string.Format(responseReport, "1", "2019");
                        break;
                    case "auto_test:ip鉴权失":
                        responseReport = string.Format(responseReport, "1", "2020");
                        break;
                    case "auto_test:数据包大小":
                        responseReport = string.Format(responseReport, "1", "2022");
                        break;

                    default:
                        responseReport = string.Format(responseReport, "0", "成功");
                        break;
                }
                #endregion

            }
            else
            {
                responseReport = string.Format(responseReport, "0", "成功");
            }


            return responseReport;
        }


        public static MessageStatus GetMMSStatus(string content)
        {
            if (content.Contains('['))
            {
                string testContent = content.Substring(content.LastIndexOf('[') + 1, 15);

                #region GET STATUS FROM CONTENT
                switch (testContent)
                {
                    case "auto_test:手机号码无":
                        return MessageStatus.InvalidMobileNumber;
                    case "auto_test:签名不合法":
                        return MessageStatus.IllegalSign;
                    case "auto_test:短信内容超":
                        return MessageStatus.ContentTooLong;
                    case "auto_test:请求来源地":
                        return MessageStatus.InValidRequestSourceUrl;
                    case "auto_test:内容包含敏":
                        return MessageStatus.ContainKeywords;
                    case "auto_test:余额不足]":
                        return MessageStatus.LackOfBalance;
                    case "auto_test:购买产品或":
                        return MessageStatus.ProductDisabled;
                    case "auto_test:账号被禁用":
                        return MessageStatus.AccountDisabled;
                    case "auto_test:不支持该运":
                        return MessageStatus.VendorGatewayFailed;
                    case "auto_test:发送号码数":
                        return MessageStatus.DidNotMeetProductMinLimit;
                    case "auto_test:黑名单号码":
                        return MessageStatus.Blacklist;
                    case "auto_test:该模板ID已":
                        return MessageStatus.DisabledTemplateId;
                    case "auto_test:非法模板ID":
                        return MessageStatus.IlligalTemplateId;
                    case "auto_test:不支持的MS":
                        return MessageStatus.UnsupportMSGFMT;
                    case "auto_test:子号码无效":
                        return MessageStatus.InvalidSubcode;
                    case "auto_test:内容为空]":
                        return MessageStatus.EmptyContent;
                    case "auto_test:号码为空]":
                        return MessageStatus.IncorrectMobileNoFormate;
                    case "auto_test:单个号码相":
                        return MessageStatus.SingleAccountSameContentLimit;
                    case "auto_test:单个号码次":
                        return MessageStatus.SingleAccountSendLimit;
                    case "auto_test:处理失败]":
                        return MessageStatus.ProccessFailed;
                    case "auto_test:接入方式错":
                        return MessageStatus.ConnectModeError;
                    case "auto_test:系统繁忙]":
                        return MessageStatus.SystemBusy;
                    case "auto_test:消息格式错":
                        return MessageStatus.ContentFormateError;

                    case "auto_test:短信内容为":
                        return MessageStatus.EmptyContent;
                    case "auto_test:手机号码个":
                        return MessageStatus.MobileNumberCountLimit;
                    case "auto_test:手机号码为":
                        return MessageStatus.IncorrectMobileNoFormate;
                    case "auto_test:系统正忙]":
                        return MessageStatus.SystemBusy;
                    case "auto_test:账号无效]":
                        return MessageStatus.InvalidAccount;
                    case "auto_test:密码错误]":
                        return MessageStatus.PasswordError;
                    case "auto_test:msgid":
                        return MessageStatus.MsgidTooLong;
                    case "auto_test:错误号码/":
                        return MessageStatus.IncorrectMobileNumber;
                    case "auto_test:扩展子码无":
                        return MessageStatus.InvalidExtSubcode;
                    case "auto_test:定时时间格":
                        return MessageStatus.TimerTimeFormateError;
                    case "auto_test:用户被禁发":
                        return MessageStatus.AccountDisabled;
                    case "auto_test:ip鉴权失":
                        return MessageStatus.IPAuthenticationFailed;
                    case "auto_test:数据包大小":
                        return MessageStatus.DataPackageSizeNotMatch;

                    default:
                        return MessageStatus.Success;
                }
                #endregion

            }
            else
            {
                return MessageStatus.SubmitSuccess;
            }
        }
        public static MessageStatus GetSubmitStatus(int result)
        {
            switch (result)
            {
                case 0:
                    return MessageStatus.SubmitSuccess;
                case 1:
                    return MessageStatus.InvalidAccount;
                case 2:
                    return MessageStatus.PasswordError;
                case 3:
                    return MessageStatus.MsgidTooLong;
                case 4:
                    return MessageStatus.IncorrectMobileNumber;
                case 5:
                    return MessageStatus.MobileNumberCountLimit;
                case 6:
                    return MessageStatus.ContentTooLong;
                case 7:
                    return MessageStatus.InvalidExtSubcode;
                case 8:
                    return MessageStatus.TimerTimeFormateError;
                case 14:
                    return MessageStatus.IncorrectMobileNoFormate;
                case 19:
                    return MessageStatus.AccountDisabled;
                case 20:
                    return MessageStatus.IPAuthenticationFailed;
                case 21:
                    return MessageStatus.EmptyContent;
                case 22:
                    return MessageStatus.DataPackageSizeNotMatch;
                case 98:
                    return MessageStatus.SystemBusy;
                case 99:
                    return MessageStatus.ContentFormateError;
                default:
                    return MessageStatus.UnknowError;


            }
        }
        public static MessageStatus GetSendStatus(int status, string desc)
        {
            if (status == 0)
            { return MessageStatus.Success; }
            else if (status == 1)
            {
                return GetSendStautsByDesc(desc);
            }
            else if (status == 2)
            { return MessageStatus.VendorGatewayFailed; }
            else
            {
                return MessageStatus.UnknowError;
            }



        }
        public static MessageStatus GetSendStautsByDesc(string desc)
        {
            int descResult = Convert.ToInt32(desc);
            switch (descResult)
            {


                case 201:
                    return MessageStatus.InvalidAccount;
                case 202:
                    return MessageStatus.PasswordError;
                case 203:
                    return MessageStatus.MsgidTooLong;
                case 204:
                    return MessageStatus.IncorrectMobileNumber;
                case 205:
                    return MessageStatus.MobileNumberCountLimit;
                case 206:
                    return MessageStatus.ContentTooLong;
                case 207:
                    return MessageStatus.InvalidExtSubcode;
                case 208:
                    return MessageStatus.TimerTimeFormateError;
                case 2014:
                    return MessageStatus.IncorrectMobileNoFormate;
                case 2019:
                    return MessageStatus.AccountDisabled;
                case 2020:
                    return MessageStatus.IPAuthenticationFailed;
                case 2021:
                    return MessageStatus.EmptyContent;
                case 2022:
                    return MessageStatus.DataPackageSizeNotMatch;
                case 2098:
                    return MessageStatus.SystemBusy;
                case 2099:
                    return MessageStatus.ContentFormateError;
                case 4:
                    return MessageStatus.InvalidMobileNumber;
                case 5:
                    return MessageStatus.IllegalSign;
                case 6:
                    return MessageStatus.ContentTooLong;
                case 9:
                    return MessageStatus.InValidRequestSourceUrl;
                case 10:
                    return MessageStatus.ContainKeywords;
                case 11:
                    return MessageStatus.LackOfBalance;
                case 12:
                    return MessageStatus.ProductDisabled;
                case 13:
                    return MessageStatus.AccountDisabled;
                case 14:
                    return MessageStatus.VendorGatewayFailed;
                case 16:
                    return MessageStatus.DidNotMeetProductMinLimit;
                case 19:
                    return MessageStatus.Blacklist;
                case 20:
                    return MessageStatus.DisabledTemplateId;
                case 21:
                    return MessageStatus.IlligalTemplateId;
                case 22:
                    return MessageStatus.UnsupportMSGFMT;
                case 23:
                    return MessageStatus.InvalidSubcode;
                case 24:
                    return MessageStatus.EmptyContent;
                case 25:
                    return MessageStatus.IncorrectMobileNoFormate;
                case 26:
                    return MessageStatus.SingleAccountSameContentLimit;
                case 27:
                    return MessageStatus.SingleAccountSendLimit;
                case 96:
                    return MessageStatus.ProccessFailed;
                case 97:
                    return MessageStatus.ConnectModeError;
                case 98:
                    return MessageStatus.SystemBusy;
                case 99:
                    return MessageStatus.ContentFormateError;
                default:
                    return MessageStatus.UnknowError;
            }
        }



        public List<MessageReplyInfo> GetMMSReplyInfo(string startDt, string endDate)
        {
            throw new NotImplementedException();
        }

    }
}
