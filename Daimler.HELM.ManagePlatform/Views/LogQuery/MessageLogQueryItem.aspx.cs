using Daimler.HELM.BizObjects;
using Daimler.HELM.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.ManagePlatform.Views.LogQuery
{
    public partial class MessageLogQueryItem : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string id = Request.QueryString["Id"].ToString();
                    string messageType = Request.QueryString["MessageType"].ToString();
                    RequestLog requestInfo = ServiceFactory.GetInstance().GetLogInfoById(id, messageType);
                    BinData(requestInfo, messageType);
                }
                catch (Exception ex)
                {
                    Daimler.HELM.BizObjects.ExceptionLog log = new Daimler.HELM.BizObjects.ExceptionLog()
                    {
                        MessageType = "message log query detail ",
                        ExceptionInfo = ex
                    };
                    Daimler.HELM.HubService.Logic.CommonHandler.RecordExceptionLog(log);
                }
            }
        }
        protected void BinData(RequestLog requestInfo,string messageType)
        {
            this.txtId.Text = requestInfo.Id.ToString();
            this.txtCreateDate.Text = requestInfo.CreatedDt;
            this.txtMessageSource.Text = requestInfo.DataSrouce;
            this.txtMessageType.Text = messageType;
            this.txtRequestInfo.Text = requestInfo.RequestInfo;
            this.txtSendContent.Text = requestInfo.SendContent;
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
          ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>window.opener=null;window.open('','_self');window.close();</script>"); 
        }
    }
}