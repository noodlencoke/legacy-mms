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

namespace Daimler.HELM.TSBAdapter
{
    public class SMSTSBQueue : BaseMessageIBMMQ
    {

        public SMSTSBQueue(string mqHost, string channel, int mqPort, string qmName, string qName,string qReplyName,string userId,string passWord) :
            base(mqHost, channel, mqPort, qmName, qName,qReplyName,userId,passWord)
        {

        }
        public override void SendMessage(MQMessage message)
        {
            ThreadSendMessage(message);
        }
        public override void ThreadSendMessage(object message)
        {
            ReturnValueInfo returnValue=null;
            MQMessage messageQ = message as MQMessage;
            try
            {
              
                string messText = messageQ.ReadString(messageQ.MessageLength);
                RequestLog request = new RequestLog();
                request.Id = Guid.NewGuid();
                request.CreatedDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                request.DataSrouce = "TSB";
                request.RequestInfo = CommonMethod.ByteToHexStr(messageQ.MessageId);
                request.SendContent = messText;
                CommonResult result = SMSHandler.RecordSMSSendLog(request);
                if (!result.IsOK)
                {
                    throw new Exception("请求数据存储异常：" + result.ReturnMessage + request.RequestInfo + "," + request.SendContent);
                }
                if (!string.IsNullOrEmpty(messText))
                {
                    if (messText.Contains("mnfRequest"))
                    {
                        MessageInfo messageInfo = MessageHandler.ConvertSMSList(messText);
                        messageInfo.Id = Guid.NewGuid();
                        messageInfo.RequestId = request.Id;
                        messageInfo.Type = MessageType.SMS;
                        messageInfo.ReceivedDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                        messageInfo.DataSource = "TSB";
                        messageInfo.Priority = System.Messaging.MessagePriority.High;
                        messageInfo.IsNeedReply = true;
                        messageInfo.TaskId = CommonMethod.ByteToHexStr(messageQ.MessageId) + "02";
                        ServiceFactory.GetInstance().SendSingleSMS(messageInfo);
                        returnValue = new ReturnValueInfo()
                        {
                            Status = "0",
                            Comment = "success"
                        };

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
                    MessageType = "tsb sms send",
                    ExceptionInfo = ex
                };
                CommonHandler.RecordExceptionLog(log);
                returnValue = new ReturnValueInfo()
                {
                    Status = "99",
                    Comment = ex.Message.ToString()
                };
            }
            finally
            {
                if (messageQ != null)
                    ReplyQ(messageQ, returnValue);
            }
        }

        public virtual  void ReplyQ(MQMessage message, ReturnValueInfo returnValue)
        {
            try
            {
                MQMessage replyMessage = message;
                replyMessage.CorrelationId = message.MessageId;
                replyMessage.ClearMessage();
                replyMessage.WriteString(MessageHandler.GetReturnValue(returnValue));
                if (string.IsNullOrEmpty(message.ReplyToQueueManagerName))
                    this.ReplyMessage(replyMessage);
                else
                this.ReplyMessage(replyMessage,replyMessage.ReplyToQueueManagerName);
            }
            catch (Exception ex)
            { 
                ExceptionLog log = new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "tsb sms return value ",
                    ExceptionInfo = ex
                };
                CommonHandler.RecordExceptionLog(log);
            }
        }

    }
}
