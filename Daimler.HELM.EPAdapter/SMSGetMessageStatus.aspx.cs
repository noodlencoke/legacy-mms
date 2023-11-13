using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.EPAdapter
{
    public partial class SMSGetMessageStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "application/Json";
                Response.Charset = "UTF-8";
                IList<MessageStateInfo> list = SendMessageHandler.GetMessageStateInfos("EP");
                string returnVal = CommonMethod.ConvertObjectToJson(list);
                SMSHandler.UpdateMessageReceiveStates(list);
                Response.Write(returnVal);
                Response.End();
                
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    ExceptionLog log = new ExceptionLog()
                    {
                        Id = Guid.NewGuid(),
                        MessageType = "SMS、MMS getstatus ",
                        ExceptionInfo = ex
                    };
                    CommonHandler.RecordExceptionLog(log);
                }
            }

        }
    }
}