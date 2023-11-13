using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using Daimler.HELM.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.EPAdapter
{
    public partial class SMSSendMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    RequestLog request = CommonMethod.GetRequestInfo(Request);
                    request.DataSrouce = "EP";
                    //记录调用日志
                    CommonResult result = SMSHandler.RecordSMSSendLog(request);

                    if (!result.IsOK)
                    {
                        throw new Exception("请求数据存储异常：" + result.ReturnMessage + request.RequestInfo + "," + request.SendContent);
                    }
                    EPSendMessageParam paramObj = MessageHandler.GetRequestParams(request.SendContentOfBase64);

                    List<MessageInfo> messageList = MessageHandler.ConvertMessageList(MessageType.SMS,paramObj, request);

                    if (messageList.Count > 0)
                    {
                        if (messageList.Count == 1)
                        {
                            MessageInfo smsMessage = messageList[0];
                            smsMessage.Priority = System.Messaging.MessagePriority.High;
                            ServiceFactory.GetInstance().SendSingleSMS(smsMessage);
                        }
                        else
                        {
                            ServiceFactory.GetInstance().SendBatchSMS(messageList);
                        }
                    }

                    if (result.IsOK)
                    {
                        Response.Write("1");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLog log = new ExceptionLog()
                    {
                        MessageType = "sms send ",
                        ExceptionInfo = ex
                    };
                    CommonHandler.RecordExceptionLog(log);
                    Response.Write("2");
                }
                Response.End();

            }
        }



    }
}