using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using Daimler.HELM.MQ;
using IBM.WMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.POSAdapter
{
    public class SMSPOSQueue : BaseMessageIBMMQ
    {
        public SMSPOSQueue(string mqHost, string channel, int mqPort, string qmName, string qName,string userId,string passWord) :
            base(mqHost,channel,mqPort,qmName,qName,userId,passWord)
        { 
        
        }

        public override void SendMessage(MQMessage message)
        {
            ThreadSendMessage(message);
        }
        public override void ThreadSendMessage(object message)
        {
            try
            {
                MQMessage messageQ = message as MQMessage;
                string messText = messageQ.ReadString(messageQ.MessageLength);
                RequestLog request = new RequestLog();
                request.Id = Guid.NewGuid();
                request.CreatedDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                request.DataSrouce = "POS";
                request.RequestInfo = CommonMethod.ByteToHexStr(messageQ.MessageId);
                request.SendContent = messText;
                CommonResult result = SMSHandler.RecordSMSSendLog(request);
                if (!result.IsOK)
                {
                    throw new Exception("请求数据存储异常：" + result.ReturnMessage + request.RequestInfo + "," + request.SendContent);
                }
                if (!string.IsNullOrEmpty(messText))
                {
                    if (messText.Contains("OutboundMessage"))
                    {
                        MessageInfo messageInfo = MessageHandler.ConvertSMSList(messText);
                        messageInfo.Id = Guid.NewGuid();
                        messageInfo.RequestId = request.Id;
                        messageInfo.Type = MessageType.SMS;
                        messageInfo.ReceivedDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                        messageInfo.DataSource = "POS";
                        messageInfo.Priority = System.Messaging.MessagePriority.High;
                        messageInfo.SessionId = "POS";
                        messageInfo.IsNeedReply = true;
                        messageInfo.TaskId = CommonMethod.ByteToHexStr(messageQ.MessageId)+"01";
                        ServiceFactory.GetInstance().SendSingleSMS(messageInfo);
                    }
                    else
                    {
                        throw new Exception("xml data format exception");
                    }
                }
                else
                {
                    throw new Exception("message content is empty ");
                }
            }
            catch (Exception ex)
            {
                ExceptionLog log = new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "pos sms send",
                    ExceptionInfo = ex
                };
                CommonHandler.RecordExceptionLog(log);
            }
        }


    }
}
