﻿using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Daimler.HELM.MessageInterface.Impl
{
    public class DhstWebServiceVendor : ISMSInterface, IMMSInterface
    {
        private static readonly string proxyAddress = ConfigurationManager.AppSettings["ProxyAddress"];
        private static readonly string proxyPort = ConfigurationManager.AppSettings["ProxyPort"];
        private static readonly string useProxy = ConfigurationManager.AppSettings["UseProxy"];

        public BizObjects.RequestResultInfo SendMMS(BizObjects.MessageInfo msgInfo)
        {

            #region Set Send MMS
            StringBuilder param = new StringBuilder();
            string message = "";

            string _serverURL = ConfigurationManager.AppSettings["MMSServerUrl"].ToString();
            string _smsId = Guid.NewGuid().ToString().Replace("-", "");
            string _phones = msgInfo.Number;
            string _templatedId = msgInfo.Content;
            string _subCode = msgInfo.ExtendNumber;
            string _pwd = Common.CommonMethod.Md5(msgInfo.AccountPwd);
            string path = ConfigurationManager.AppSettings["MMSPath"];
            string _smsContent = "";
            string _title = "";
            GetMmsContent(path, _templatedId, out _smsContent, out _title);
            if (_smsContent == "")
            {
                RequestResultInfo result = new RequestResultInfo();
                result.StrResult = message;
                result.SMSStatus = MessageStatus.EmptyContent;
                result.Desc = "can not find mms template id '" + _templatedId + "'";
                return result;
            }
            try
            {
                param.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                param.Append("<root>");
                param.Append("<head>");
                param.Append("<cmdId>002</cmdId>");
                param.Append("<account>" + msgInfo.AccountName + "</account>");
                param.Append("<password>" + _pwd + "</password>");
                param.Append("</head>");
                param.Append("<body>");
                param.Append("<submitMsg>");
                param.Append("<phone>" + _phones + "</phone>");
                param.Append("<title>" + _title + "</title>");
                param.Append("<content>" + _smsContent + "</content>");
                param.Append("<msgid>" + _smsId + "</msgid>");
                param.Append("<subCode>" + _subCode + "</subCode>");



                param.Append("</submitMsg>");
                param.Append("</body>");
                param.Append("</root>");

                message = PostMethodConnServer(_serverURL, param.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "," + ex.StackTrace);
            }
            #endregion

            #region GetSubmitResult
            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(message);
                //将提交返回的报告封装成 提交报告对象
                RequestResultInfo returnResult = new RequestResultInfo();
                string cmdId = xdoc.SelectNodes("/root/head/cmdId").Item(0).InnerText.ToString();
                string submitResult = xdoc.SelectNodes("/root/head/result").Item(0).InnerText.ToString();
                int resultCount = xdoc.SelectNodes("/root/body/submitResult").Count;
                if ((cmdId == "802" || cmdId == "702") && submitResult == "0" && resultCount > 0)
                {

                    string status = xdoc.SelectNodes("/root/body/submitResult/response/status").Item(0).InnerText.ToString();
                    if (status == "0")
                    {
                        string msgId = xdoc.SelectNodes("/root/body/submitResult/response/msgid").Item(0).InnerText.ToString();

                        returnResult.TaskId = msgId;
                    }
                    returnResult.StrResult = message;
                    returnResult.SMSStatus = GetMMSSubmitStatus(status);
                    returnResult.SenderNumber = "1069XXXX" + _subCode;
                }
                return returnResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace + ",responseMsg:" + message);
            }
            #endregion
        }

        public List<BizObjects.RequestResultInfo> SendMMS(List<BizObjects.MessageInfo> msgInfo)
        {

            #region Set Send MMS
            string _phones = "";
            foreach (var item in msgInfo)
            {
                _phones += item.Number + ",";
            }
            _phones = _phones.TrimEnd(',');

            StringBuilder param = new StringBuilder();
            string message = "";

            string _serverURL = ConfigurationManager.AppSettings["MMSServerUrl"];
            string _smsId = Guid.NewGuid().ToString().Replace("-", "");
            string _templatedId = msgInfo[0].Content;
            string _subCode = msgInfo[0].ExtendNumber;
            string _pwd = Common.CommonMethod.Md5(msgInfo[0].AccountPwd);

            string path = ConfigurationManager.AppSettings["MMSPath"];
            string _smsContent = "";
            string _title = "";
            GetMmsContent(path, _templatedId, out _smsContent, out _title);
            if (_smsContent == "")
            {
                List<RequestResultInfo> list = new List<RequestResultInfo>();
                for (int i = 0; i < msgInfo.Count; i++)
                {
                    RequestResultInfo result = new RequestResultInfo();
                    result.StrResult = message;
                    result.SMSStatus = MessageStatus.EmptyContent;
                    result.Desc = "can not find mms template id '" + _templatedId + "'";
                    list.Add(result);
                }
                return list;
            }
            try
            {
                param.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                param.Append("<root>");
                param.Append("<head>");
                param.Append("<cmdId>002</cmdId>");
                param.Append("<account>" + msgInfo[0].AccountName + "</account>");
                param.Append("<password>" + _pwd + "</password>");
                param.Append("</head>");
                param.Append("<body>");
                param.Append("<submitMsg>");
                param.Append("<phone>" + _phones + "</phone>");
                param.Append("<title>" + _title + "</title>");
                param.Append("<content>" + _smsContent + "</content>");
                param.Append("<msgid>" + _smsId + "</msgid>");
                param.Append("<subCode>" + _subCode + "</subCode>");
                param.Append("</submitMsg>");
                param.Append("</body>");
                param.Append("</root>");

                message = PostMethodConnServer(_serverURL, param.ToString());

            }
            catch (Exception ex)
            {
                // TODO Auto-generated catch block
                throw new Exception(ex.Message + ex.StackTrace + ",responseMsg:" + message);
            }
            #endregion

            #region GetSubmitResult

            XmlDocument xdoc = new XmlDocument();
            List<RequestResultInfo> requestResultList = new List<RequestResultInfo>();
            try
            {
                xdoc.LoadXml(message);
                //将提交返回的报告封装成 提交报告对象

                string cmdId = xdoc.SelectNodes("/root/head/cmdId").Item(0).InnerText.ToString();
                string submitResult = xdoc.SelectNodes("/root/head/result").Item(0).InnerText.ToString();
                int resultCount = xdoc.SelectNodes("/root/body/submitResult").Count;
                if ((cmdId == "802" || cmdId == "702") && submitResult == "0" && resultCount > 0)
                {
                    for (int i = 0; i < resultCount; i++)
                    {
                        string submitResultXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + xdoc.SelectNodes("/root/body/submitResult")[i].OuterXml;

                        XmlDocument submitResultXDOC = new XmlDocument();
                        submitResultXDOC.LoadXml(submitResultXML);

                        int responseCount = submitResultXDOC.SelectNodes("/submitResult/response").Count;

                        if (responseCount > 0)
                        {
                            for (int j = 0; j < responseCount; j++)
                            {
                                string responseXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + submitResultXDOC.SelectNodes("/submitResult/response")[j].OuterXml;

                                XmlDocument responseXDOC = new XmlDocument();
                                responseXDOC.LoadXml(responseXML);
                                RequestResultInfo returnResult = new RequestResultInfo();

                                string status = responseXDOC.SelectNodes("/response/status").Item(0).InnerText.ToString();
                                if (status == "0")
                                {
                                    string msgId = responseXDOC.SelectNodes("/response/msgid").Item(0).InnerText;
                                    string phone = responseXDOC.SelectNodes("/response/phone").Item(0).InnerText;
                                    returnResult.TaskId = msgId;
                                    returnResult.Mobile = phone;
                                    returnResult.StrResult = message;
                                    returnResult.SMSStatus = GetMMSSubmitStatus(status);
                                    returnResult.SenderNumber = "1069XXXX" + _subCode;
                                    requestResultList.Add(returnResult);
                                }
                            }
                        }
                    }

                }
                return requestResultList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace + ",responseMsg:" + message);
            }
            #endregion

        }

        public BizObjects.SendingResult GetMMSSendingStatus(BizObjects.MessageInfo msgInfo)
        {
            throw new NotImplementedException();
        }

        public List<BizObjects.SendingResult> GetMMSSendingStatus(string accountName, string accountPwd)
        {
            StringBuilder param = new StringBuilder();
            string responseXml = "";
            string _pwd = Common.CommonMethod.Md5(accountPwd);
            string _serverURL = ConfigurationManager.AppSettings["MMSServerUrl"];
            param.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            param.Append("<root>");
            param.Append("<head>");
            param.Append("<cmdId>004</cmdId>");
            param.Append("<account>" + accountName + "</account>");
            param.Append("<password>" + _pwd + "</password>");
            param.Append("</head>");
            param.Append("</root>");

            try
            {
                responseXml = PostMethodConnServer(_serverURL, param.ToString());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }
            XmlDocument mmsXdoc = new XmlDocument();
            List<SendingResult> sendResultList = new List<SendingResult>();
            try
            {
                mmsXdoc.LoadXml(responseXml);
                string cmdId = mmsXdoc.ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText;
                string result = mmsXdoc.ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText;

                if ((cmdId == "804" || cmdId == "704") && result == "0")
                {
                    int bodyCount = mmsXdoc.ChildNodes[1].ChildNodes[1].ChildNodes.Count;
                    if (bodyCount != 0)
                    {
                        for (int i = 0; i < bodyCount; i++)
                        {
                            SendingResult sendingResult = new SendingResult();
                            string responseXML = mmsXdoc.ChildNodes[1].ChildNodes[1].ChildNodes[i].InnerXml;
                            Dictionary<string, string> rParams = GetResponseParams(responseXML);
                            sendingResult = GetMMSSendResult(rParams, responseXML);
                            sendResultList.Add(sendingResult);
                        }
                    }
                }

                return sendResultList;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }
        }

        public List<BizObjects.MessageReplyInfo> GetMMSReplyInfo(string startDt, string endDate)
        {
            return GetReply(startDt, endDate); ;
        }





        public BizObjects.RequestResultInfo SendSMS(BizObjects.MessageInfo msgInfo)
        {
            string _serverURL = ConfigurationManager.AppSettings["SMSServerUrl"].ToString();
            string _smsId = Guid.NewGuid().ToString().Replace("-", "");
            string _phones = msgInfo.Number;
            string _smsContent = msgInfo.Content;
            string _sign = msgInfo.SignName;
            string _subCode = msgInfo.ExtendNumber;
            string _pwd = Common.CommonMethod.Md5(msgInfo.AccountPwd);
            string _data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                                    + "<message>"
                                        + "<account>" + msgInfo.AccountName + "</account>"
                                        + "<password>" + _pwd + "</password>"
                                        + "<msgid>" + _smsId + "</msgid>"
                                        + "<phones>" + _phones + "</phones>"
                                        + "<content>" + _smsContent + "</content>"
                                        + "<sign>" + _sign + "</sign>"
                                        + "<subcode>" + _subCode + "</subcode>"
                                        + "<sendtime></sendtime>"
                                    + "</message>";

            SmsService4XMLImplService client = new SmsService4XMLImplService();
            if (Convert.ToBoolean(useProxy))
            {
                WebProxy proxy = new WebProxy(proxyAddress, Convert.ToInt32(proxyPort));
                //proxy.Credentials = new NetworkCredential("","");
                client.UseDefaultCredentials = true;
                client.Proxy = proxy;
            }
            string responseMsg = client.submit(_data);
            return GetSMSSubmitResult(responseMsg, msgInfo.ExtendNumber);
        }

        public List<BizObjects.RequestResultInfo> SendSMS(List<BizObjects.MessageInfo> msgList)
        {
            string[] _phones = new string[msgList.Count];
            string[] _content = new string[msgList.Count];
            string[] _msgid=new string[msgList.Count];
            for (int i = 0; i < msgList.Count; i++)
            {
                _phones[i] = msgList[i].Number;
                _content[i] = msgList[i].Content;
                _msgid[i]= Guid.NewGuid().ToString().Replace("-","");
            }
            string account = msgList[0].AccountName;
            string pwd = Common.CommonMethod.Md5(msgList[0].AccountPwd);
            string mobiles = string.Join(",", _phones);
            string smsId =string.Join(",",_msgid);
            string sign = msgList[0].SignName;
            string subCode = msgList[0].ExtendNumber;
            ISmsAgileServiceService client = new ISmsAgileServiceService();
            if (Convert.ToBoolean(useProxy))
            {
                WebProxy proxy = new WebProxy(proxyAddress, Convert.ToInt32(proxyPort));
                client.UseDefaultCredentials = true;
                client.Proxy = proxy;
            }
            string responseMsg = client.sendSms(account, pwd, mobiles, _content, smsId, subCode, sign);
            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(responseMsg);

                RequestResultInfo returnResult = new RequestResultInfo();
                string faileNum = "";
                string taskId = "";
                int result = -1;
                string desc = "";
                string blackList = "";
                if (xdoc.SelectNodes("/response/msgid").Item(0) != null)
                {
                    taskId = xdoc.SelectNodes("/response/msgid").Item(0).InnerText.ToString();
                }
                if (xdoc.SelectNodes("/response/result").Item(0) != null)
                {
                    result = Convert.ToInt32(xdoc.SelectNodes("/response/result").Item(0).InnerText);
                }
                if (xdoc.SelectNodes("/response/desc").Item(0) != null)
                {
                    desc = xdoc.SelectNodes("/response/desc").Item(0).InnerText.ToString();
                }
                if (xdoc.SelectNodes("/response/blacklist").Item(0) != null)
                {
                    blackList = xdoc.SelectNodes("/response/blacklist").Item(0).InnerText.ToString();
                }
                if (xdoc.SelectNodes("/response/failPhones").Item(0) != null)
                {
                    faileNum = xdoc.SelectNodes("/response/failPhones").Item(0).InnerText.ToString();
                }
                returnResult.TaskId = taskId;
                returnResult.SMSStatus = GetSubmitStatus(result);
                returnResult.Desc = desc;
                returnResult.BlackList = blackList;
                returnResult.StrResult = responseMsg;
                returnResult.SenderNumber = "1069XXXX" + msgList[0].ExtendNumber;

                List<RequestResultInfo> returnResultList = new List<RequestResultInfo>();
                string[] blackListArray = blackList.Split(',');
                string[] faileListArray = faileNum.Split(',');
                string[] taskIds = taskId.Split(',');
                for (int i = 0; i < msgList.Count; i++)
                {
                    RequestResultInfo newReturnResult = new RequestResultInfo();
                    newReturnResult.TaskId = taskIds[i];
                    newReturnResult.SMSStatus = returnResult.SMSStatus;
                    newReturnResult.Desc = returnResult.Desc;
                    newReturnResult.BlackList = returnResult.BlackList;
                    newReturnResult.StrResult = returnResult.StrResult;
                    newReturnResult.SenderNumber = returnResult.SenderNumber;

                    if (blackListArray.Contains(msgList[i].Number))
                    {
                        newReturnResult.SMSStatus = MessageStatus.Blacklist;
                    }
                    if (faileListArray.Contains(msgList[i].Number))
                    { newReturnResult.SMSStatus = MessageStatus.InvalidMobileNumber; }
                    newReturnResult.Mobile = msgList[i].Number;


                    returnResultList.Add(newReturnResult);

                }
                return returnResultList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public BizObjects.SendingResult GetSMSSendingStatus(BizObjects.MessageInfo msgInfo)
        {
            throw new NotImplementedException();
        }

        public List<BizObjects.SendingResult> GetSMSSendingStatus(string accountName, string accountPwd)
        {
            string responseMsg = "";
            string _pwd = Common.CommonMethod.Md5(accountPwd);
            string _data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                           + "<message>"
                               + "<account>" + accountName + "</account>"
                               + "<password>" + _pwd + "</password>"
                           + "</message>";
            SmsService4XMLImplService client = new SmsService4XMLImplService();
            if (Convert.ToBoolean(useProxy))
            {
                WebProxy proxy = new WebProxy(proxyAddress, Convert.ToInt32(proxyPort));
                //proxy.Credentials = new NetworkCredential("","");
                client.UseDefaultCredentials = true;
                client.Proxy = proxy;
            }
            responseMsg = client.report(_data);
            XmlDocument xdoc = new XmlDocument();
            List<SendingResult> sendResultList = new List<SendingResult>();
            try
            {
                xdoc.LoadXml(responseMsg);
                if (xdoc.ChildNodes[1].ChildNodes[0].InnerText == "0" & xdoc.ChildNodes[1].ChildNodes.Count > 2)
                {
                    int count = xdoc.ChildNodes[1].ChildNodes.Count;
                    for (int i = 2; i < count; i++)
                    {
                        SendingResult result = new SendingResult();
                        string responseXML = xdoc.ChildNodes[1].ChildNodes[i].InnerXml;
                        Dictionary<string, string> rParams = GetResponseParams(responseXML);
                        result = GetSMSSendResult(rParams, responseXML);
                        if (result != null)
                        {
                            sendResultList.Add(result);
                        }
                    }
                }

                return sendResultList;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace + ",responseMsg:" + responseMsg);
            }

        }

        public List<BizObjects.MessageReplyInfo> GetReply(string accountName, string accountPwd)
        {
            string responseMsg = "";
            accountPwd = Common.CommonMethod.Md5(accountPwd);
            string _data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                          + "<message>"
                              + "<account>" + accountName + "</account>"
                              + "<password>" + accountPwd + "</password>"
                              + "</message>";
            SmsService4XMLImplService client = new SmsService4XMLImplService();
            if (Convert.ToBoolean(useProxy))
            {
                WebProxy proxy = new WebProxy(proxyAddress, Convert.ToInt32(proxyPort));
                //proxy.Credentials = new NetworkCredential("","");
                client.UseDefaultCredentials = true;
                client.Proxy = proxy;
            }
            List<MessageReplyInfo> messageReplyList = new List<MessageReplyInfo>();
            try
            {
                responseMsg = client.deliver(_data);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(responseMsg);
                int replyCount = xdoc.SelectNodes("/response/sms").Count;
                if (replyCount != 0)
                {
                    for (int i = 0; i < replyCount; i++)
                    {
                        string replyXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><sms>"
                        + xdoc.SelectNodes("/response/sms")[i].InnerXml + "</sms>";

                        MessageReplyInfo messageReplyInfo = GetReplyMessage(replyXml);
                        messageReplyList.Add(messageReplyInfo);
                    }
                }
                return messageReplyList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace + ",responseMsg:" + responseMsg);
            }
        }



        public static string PostMethodConnServer(string iServerURL, string iPostData)
        {
            byte[] _buffer = Encoding.GetEncoding("utf-8").GetBytes(iPostData);
            HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(iServerURL);
            if (Convert.ToBoolean(useProxy))
            {
                WebProxy proxy = new WebProxy(proxyAddress, Convert.ToInt32(proxyPort));
                //proxy.Credentials = new NetworkCredential("","");
                _req.UseDefaultCredentials = true;
                _req.Proxy = proxy;
            }
            _req.Method = "Post";
            _req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            _req.ContentLength = _buffer.Length;
            using (Stream _stream = _req.GetRequestStream())
            {
                _stream.Write(_buffer, 0, _buffer.Length);
                _stream.Flush();
                HttpWebResponse _res = (HttpWebResponse)_req.GetResponse();
                using (Stream _resStream = _res.GetResponseStream())
                {
                    using (StreamReader _resSR = new StreamReader(_resStream, Encoding.GetEncoding("utf-8")))
                    {
                        return _resSR.ReadToEnd();
                    }
                }
            }

        }

        public static void GetMmsContent(string path, string _templatedId, out string content, out string subject)
        {
            ObjectCache cCache = MemoryCache.Default;
            string _titlePath = path + "\\" + _templatedId + "\\title.txt";
            string _filepath = path + "\\" + _templatedId + "\\mms.zip";
            string getContent = "";
            string getSubject = "";
            if (Object.ReferenceEquals(cCache[_templatedId + "content"], null) || Object.ReferenceEquals(cCache[_templatedId + "title"], null))
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now.AddHours(2);
                try
                {
                    using (FileStream fs = new FileStream(_filepath, FileMode.Open))
                    {

                        byte[] buff = new byte[fs.Length];
                        fs.Read(buff, 0, Convert.ToInt32(fs.Length));
                        getContent = Convert.ToBase64String(buff);
                        CacheItem item = new CacheItem(_templatedId + "content", getContent);
                        cCache.Set(item, policy);
                        fs.Close();
                        fs.Dispose();
                    }
                    using (StreamReader sr = new StreamReader(_titlePath, Encoding.Default))
                    {
                        getSubject = sr.ReadToEnd();
                        CacheItem item = new CacheItem(_templatedId + "title", getSubject);
                        cCache.Set(item, policy);
                        sr.Close();
                        sr.Dispose();
                    }

                }
                catch (Exception e)
                {
                    e.Message.ToString();
                }

                content = getContent;
                subject = getSubject;

            }
            else
            {
                content = cCache[_templatedId + "content"].ToString();
                subject = cCache[_templatedId + "title"].ToString();
            }
        }



        private Dictionary<string, string> GetResponseParams(string responseXML)
        {
            XmlDocument rXmlDoc = new XmlDocument();
            Dictionary<string, string> responseParamsList = new Dictionary<string, string>();
            try
            {
                responseXML = "<xml>" + responseXML + "</xml>";
                rXmlDoc.LoadXml(responseXML);
                int count = rXmlDoc.ChildNodes[0].ChildNodes.Count;

                for (int i = 0; i < count; i++)
                {
                    responseParamsList.Add(rXmlDoc.ChildNodes[0].ChildNodes[i].Name, rXmlDoc.ChildNodes[0].ChildNodes[i].InnerText);
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }
            return responseParamsList;
        }
        private SendingResult GetSMSSendResult(Dictionary<string, string> rParams, string responseXML)
        {
            if (rParams.Count == 0)
            { return null; }

            SendingResult sendResult = new SendingResult();
            string taskId = "";
            string number = "";
            int status = -1;
            string desc = "";
            string sendTime = "";
            string wgCode = "";
            if (!string.IsNullOrEmpty(rParams["msgid"]))
            {
                taskId = rParams["msgid"];
            }
            if (!string.IsNullOrEmpty(rParams["phone"]))
            {
                number = rParams["phone"];
            }
            if (!string.IsNullOrEmpty(rParams["status"]))
            {
                status = Convert.ToInt32(rParams["status"]);
            }
            if (!string.IsNullOrEmpty(rParams["desc"]))
            {
                desc = rParams["desc"];
            }
            if (!string.IsNullOrEmpty(rParams["wgcode"]))
            {
                wgCode = rParams["wgcode"];
            }
            if (!string.IsNullOrEmpty(rParams["time"]))
            {
                sendTime = rParams["time"];
            }

            sendResult.TaskId = taskId;
            sendResult.Number = number;
            sendResult.Status = GetSendStatus(status, desc);
            sendResult.Desc = desc + " wgCode:" + wgCode;
            sendResult.SendTime = sendTime;
            sendResult.StrSendingResult = responseXML;
            return sendResult;
        }
        private SendingResult GetMMSSendResult(Dictionary<string, string> rParams, string responseXML)
        {
            if (rParams.Count == 0)
            { return null; }

            SendingResult sendResult = new SendingResult();
            string taskId = "";
            string number = "";
            string status = "-1";
            string desc = "";
            if (!string.IsNullOrEmpty(rParams["msgid"]))
            {
                taskId = rParams["msgid"];
            }
            if (!string.IsNullOrEmpty(rParams["phone"]))
            {
                number = rParams["phone"];
            }
            if (!string.IsNullOrEmpty(rParams["status"]))
            {
                status = rParams["status"];
            }
            if (!string.IsNullOrEmpty(rParams["statusDesp"]))
            {
                desc = rParams["statusDesp"];
            }

            sendResult.TaskId = taskId;
            sendResult.Number = number;
            sendResult.Status = GetMMSSendStatus(status);
            sendResult.Desc = desc;
            sendResult.StrSendingResult = responseXML;
            return sendResult;
        }
        public static MessageStatus GetMMSSubmitStatus(string result)
        {
            switch (result)
            {
                case "0":
                    return MessageStatus.SubmitSuccess;
                case "1":
                    return MessageStatus.InValidRequestSourceUrl;
                case "2":
                    return MessageStatus.InvalidAccount;
                case "3":
                    return MessageStatus.ContentFormateError;
                case "4":
                    return MessageStatus.InvalidMobileNumber;
                case "5":
                    return MessageStatus.Blacklist;
                case "6":
                    return MessageStatus.EmptyContent;
                case "7":
                    return MessageStatus.EmptyContent;
                case "8":
                    return MessageStatus.SystemBusy;
                case "9":
                    return MessageStatus.AccountDisabled;
                case "10":
                    return MessageStatus.SystemBusy;
                case "11":
                    return MessageStatus.VendorGatewayFailed;
                case "12":
                    return MessageStatus.ProccessFailed;
                case "13":
                    return MessageStatus.DataPackageSizeNotMatch;
                case "14":
                    return MessageStatus.IncorrectMobileNumber;
                case "15":
                    return MessageStatus.ProductDisabled;
                case "16":
                    return MessageStatus.IPAuthenticationFailed;
                case "17":
                    return MessageStatus.ProccessFailed;
                case "18":
                    return MessageStatus.LackOfBalance;
                default:
                    return MessageStatus.UnknowError;
            }
        }
        public static MessageStatus GetMMSSendStatus(string status)
        {
            switch (status)
            {
                case "0":
                    return MessageStatus.Success;
                case "1":
                    return MessageStatus.InValidRequestSourceUrl;
                case "2":
                    return MessageStatus.InvalidAccount;
                case "3":
                    return MessageStatus.ContentFormateError;
                case "4":
                    return MessageStatus.InvalidMobileNumber;
                case "5":
                    return MessageStatus.Blacklist;
                case "6":
                    return MessageStatus.EmptyContent;
                case "7":
                    return MessageStatus.EmptyContent;
                case "8":
                    return MessageStatus.SystemBusy;
                case "9":
                    return MessageStatus.AccountDisabled;
                case "10":
                    return MessageStatus.SystemBusy;
                case "11":
                    return MessageStatus.VendorGatewayFailed;
                case "12":
                    return MessageStatus.ProccessFailed;
                case "13":
                    return MessageStatus.DataPackageSizeNotMatch;
                case "14":
                    return MessageStatus.IncorrectMobileNumber;
                case "15":
                    return MessageStatus.ProductDisabled;
                case "16":
                    return MessageStatus.IPAuthenticationFailed;
                case "17":
                    return MessageStatus.ProccessFailed;
                case "18":
                    return MessageStatus.LackOfBalance;
                default:
                    return MessageStatus.UnknowError;
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
        public static MessageReplyInfo GetReplyMessage(string replyXml)
        {
            XmlDocument xdoc = new XmlDocument();
            MessageReplyInfo messageRelyInfo = new MessageReplyInfo();
            try
            {
                xdoc.LoadXml(replyXml);
                string number = xdoc.SelectNodes("/sms/phone").Item(0).InnerText;
                string replyContent = xdoc.SelectNodes("/sms/content").Item(0).InnerText;
                string extendNumber = xdoc.SelectNodes("/sms/subcode").Item(0).InnerText;
                string replyTime = xdoc.SelectNodes("/sms/delivertime").Item(0).InnerText;


                messageRelyInfo.mobile = number;
                messageRelyInfo.content = replyContent;
                messageRelyInfo.sessionId = extendNumber;
                messageRelyInfo.senderNumber = "1069XXXX" + extendNumber;
                messageRelyInfo.getTime = replyTime;
                messageRelyInfo.dataSource = xdoc.InnerXml;
                return messageRelyInfo;
            }
            catch (Exception)
            {

                return null;
            }

        }

        private RequestResultInfo GetSMSSubmitResult(string responseMsg, string number)
        {
            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(responseMsg);

                //将提交返回的报告封装成 提交报告对象
                RequestResultInfo returnResult = new RequestResultInfo();
                string faileNum = "";
                string taskId = "";
                int result = -1;
                string desc = "";
                string blackList = "";
                if (xdoc.SelectNodes("/response/msgid").Item(0) != null)
                {
                    taskId = xdoc.SelectNodes("/response/msgid").Item(0).InnerText.ToString();
                }
                if (xdoc.SelectNodes("/response/result").Item(0) != null)
                {
                    result = Convert.ToInt32(xdoc.SelectNodes("/response/result").Item(0).InnerText);
                }
                if (xdoc.SelectNodes("/response/desc").Item(0) != null)
                {
                    desc = xdoc.SelectNodes("/response/desc").Item(0).InnerText.ToString();
                }
                if (xdoc.SelectNodes("/response/blacklist").Item(0) != null)
                {
                    blackList = xdoc.SelectNodes("/response/blacklist").Item(0).InnerText.ToString();
                }
                if (xdoc.SelectNodes("/response/failPhones").Item(0) != null)
                {
                    faileNum = xdoc.SelectNodes("/response/failPhones").Item(0).InnerText.ToString();
                }

                returnResult.TaskId = taskId;
                if (blackList != "")
                {
                    returnResult.SMSStatus = MessageStatus.Blacklist;
                }
                else if (faileNum != "")
                {
                    returnResult.SMSStatus = MessageStatus.InvalidMobileNumber;
                }
                else
                {
                    returnResult.SMSStatus = GetSubmitStatus(result);
                }
                returnResult.Desc = desc;
                returnResult.BlackList = blackList;
                returnResult.StrResult = responseMsg;
                returnResult.SenderNumber = "1069XXXX" + number;
                return returnResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace + ",responseMsg:" + responseMsg);
            }
        }

    }
}
