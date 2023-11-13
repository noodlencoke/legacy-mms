using Daimler.HELM.BizObjects;
using Daimler.HELM.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.ManagePlatform.Views.ExceptionLog
{
    public partial class ExcetptionLogItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = (Request["ID"] ?? "").ToString();

                ExceptionLogInfo exceptionInfo = ServiceFactory.GetInstance().GetExceptionLogById(id);

                this.txtMessageId.Text = exceptionInfo.MessageId;
                this.txtCreateDate.Text = exceptionInfo.CreatedDt.ToString("yyyy-MM-dd hh:mm:ss:fff");
                this.txtSendContent.Text = exceptionInfo.ExceptionInfo;
                this.txtMessageType.Text = exceptionInfo.MessageType;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>window.opener=null;window.open('','_self');window.close();</script>"); 
        }
    }
}