using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.EPAdapter
{
    public partial class MMSSendMessage : System.Web.UI.Page
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
                    CommonResult result = MMSHandler.RecordMMSSendLog(request);
                    if (!result.IsOK)
                    {
                        throw new Exception("请求数据存储异常：" + result.ReturnMessage + request.RequestInfo + "," + request.SendContent);
                    }
                    EPSendMessageParam paramObj = MessageHandler.GetRequestParams(request.SendContent);

                    List<MessageInfo> messageList = MessageHandler.ConvertMessageList(MessageType.MMS,paramObj, request);

                    if (messageList.Count > 0)
                    {
                        if (messageList.Count == 1)
                        {
                            MessageInfo mmsMessage = messageList[0];
                            mmsMessage.Priority = System.Messaging.MessagePriority.High;
                            ServiceFactory.GetInstance().SendSingleMMS(mmsMessage);
                        }
                        else
                        {
                            ServiceFactory.GetInstance().SendBatchMMS(messageList);
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
                        MessageType = "mms send",
                        ExceptionInfo =ex
                    };
                    CommonHandler.RecordExceptionLog(log);

                    Response.Write("2");
                }
                Response.End();
            }
        }
    }
}