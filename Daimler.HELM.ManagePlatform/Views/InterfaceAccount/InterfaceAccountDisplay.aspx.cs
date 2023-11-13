using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;

namespace Daimler.HELM.ManagePlatform.Views.InterfaceAccount
{
    public partial class InterfaceAccountDisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                List<BizObjects.InterfaceAccount> interfaceAccountList = ServiceFactory.GetInstance().GetInterfaceAccountListAll();
                this.InterfaceAccountGridView.DataSource = interfaceAccountList;
                this.InterfaceAccountGridView.DataBind();
            }
        }

        protected void InterfaceAccountGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                this.InterfaceAccountGridView.EditIndex = e.NewEditIndex;
                List<BizObjects.InterfaceAccount> interfaceAccountList = ServiceFactory.GetInstance().GetInterfaceAccountListAll();
                this.InterfaceAccountGridView.DataSource = interfaceAccountList;
                this.InterfaceAccountGridView.DataBind();
            }
            catch(Exception ex)
            {
                Response.Write("GridView Edit Exception");
            }
        }

        protected void InterfaceAccountGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            CommonResult result = null;
            try
            {
                GridViewRow row = (GridViewRow)InterfaceAccountGridView.Rows[e.RowIndex];

                BizObjects.InterfaceAccount interfaceAccount = new BizObjects.InterfaceAccount();

                string idGuid = this.InterfaceAccountGridView.DataKeys[e.RowIndex].Value.ToString();
                Guid id = Guid.Parse(idGuid);

                interfaceAccount.Id = id;
                interfaceAccount.AccountName = e.NewValues["accountName"].ToString();
                interfaceAccount.AccountPwd = e.NewValues["accountPwd"].ToString();
                interfaceAccount.EffectiveDt = e.NewValues["effectiveDt"].ToString();
                interfaceAccount.EffectiveDays = int.Parse(e.NewValues["effectiveDays"].ToString());

                DropDownList ddlStatus = row.FindControl("ddlStatus") as DropDownList;
                //interfaceAccount.Status = int.Parse(e.NewValues["status"].ToString());
                interfaceAccount.Status = int.Parse(ddlStatus.SelectedValue);
                interfaceAccount.Datasource = e.NewValues["dataSource"].ToString();

                result = ServiceFactory.GetInstance().UpdateInterfaceAccount(interfaceAccount);

                this.InterfaceAccountGridView.EditIndex = -1;
                List<BizObjects.InterfaceAccount> interfaceAccountList = ServiceFactory.GetInstance().GetInterfaceAccountListAll();
                this.InterfaceAccountGridView.DataSource = interfaceAccountList;
                this.InterfaceAccountGridView.DataBind();
            }
            catch(Exception ex)
            {
                Response.Write(result.ReturnMessage);
            }
        }

        protected void InterfaceAccountGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                this.InterfaceAccountGridView.EditIndex = -1;
                List<BizObjects.InterfaceAccount> interfaceAccountList = ServiceFactory.GetInstance().GetInterfaceAccountListAll();
                this.InterfaceAccountGridView.DataSource = interfaceAccountList;
                this.InterfaceAccountGridView.DataBind();
            }
            catch(Exception ex)
            {
                Response.Write("GridView Cancel Exception");
            }
        }

        protected void InterfaceAccountGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            CommonResult result = null;
            try
            {
                string idGuid = this.InterfaceAccountGridView.DataKeys[e.RowIndex].Value.ToString();
                Guid id = Guid.Parse(idGuid);
                result = ServiceFactory.GetInstance().RemoveInterfaceAccount(id);
                List<BizObjects.InterfaceAccount> interfaceAccountList = ServiceFactory.GetInstance().GetInterfaceAccountListAll();
                this.InterfaceAccountGridView.DataSource = interfaceAccountList;
                this.InterfaceAccountGridView.DataBind();
            }
            catch(Exception ex)
            {
                Response.Write(result.ReturnMessage);
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            CommonResult result = null;
            try
            {

                if (this.TextBoxAccountName.Text.Trim() == string.Empty)
                {
                    this.lblErrorMsg.Text = "Account Name can not be empty!";
                    return;
                }

                if (this.TextBoxAccountPwd.Text.Trim() == string.Empty)
                {
                    this.lblErrorMsg.Text = "Account Password can not be empty!";
                    return;
                }

                if (this.TextBoxAccountPwdConfirm.Text != this.TextBoxAccountPwd.Text)
                {
                    this.lblErrorMsg.Text = "Account password dose not equal Account Password confirm!";
                    return;
                }

                if (this.TextBoxEffectiveDt.Text == string.Empty)
                {
                    this.lblErrorMsg.Text = "Effective Time can not be empty!";
                    return;
                }

                if (this.TextBoxEffectiveDays.Text == string.Empty)
                {
                    this.lblErrorMsg.Text = "Effective Days can not be empty !";
                    return;
                }
                int days;
                if (!int.TryParse(this.TextBoxEffectiveDays.Text, out days))
                {
                    this.lblErrorMsg.Text = "Effective Days must be a number !";
                    return;
                }

                if (this.TextBoxDataSource.Text == string.Empty)
                {
                    this.lblErrorMsg.Text = "Data Source can not be emtpy !";
                    return;
                }

                BizObjects.InterfaceAccount interfaceAccount = new BizObjects.InterfaceAccount();
                interfaceAccount.Id = Guid.NewGuid();
                interfaceAccount.AccountName = this.TextBoxAccountName.Text.Trim();
                interfaceAccount.AccountPwd = this.TextBoxAccountPwd.Text.Trim();
                interfaceAccount.EffectiveDt = this.TextBoxEffectiveDt.Text.Trim();
                interfaceAccount.EffectiveDays = int.Parse(this.TextBoxEffectiveDays.Text.Trim());
                interfaceAccount.Status = 0;
                interfaceAccount.Datasource = this.TextBoxDataSource.Text.Trim();

                string passWord = this.TextBoxAccountPwdConfirm.Text.Trim();
                if (passWord.Equals(interfaceAccount.AccountPwd))
                {
                    result = ServiceFactory.GetInstance().AddInterfaceAccount(interfaceAccount);
                }
                else
                {
                    Response.Write("用户名密码不匹配！添加失败!");
                }
                List<BizObjects.InterfaceAccount> interfaceAccountList = ServiceFactory.GetInstance().GetInterfaceAccountListAll();
                this.InterfaceAccountGridView.DataSource = interfaceAccountList;
                this.InterfaceAccountGridView.DataBind();
            }
            catch(Exception ex)
            {
                Response.Write(result.ReturnMessage);
            }
        }

        protected void InterfaceAccountGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdStatus = e.Row.FindControl("hdStatus") as HiddenField;
                if (hdStatus != null)
                {
                    DropDownList ddlStatus = e.Row.FindControl("ddlStatus") as DropDownList;
                    ddlStatus.SelectedValue = hdStatus.Value;
                }
            }
        }

        protected string ShowStatusDesc(object status)
        { 
            if(status==null)
            {
               return string.Empty;
            }
            string statusDesc = status.ToString() == "0" ? "Active" : "Disabled";
            return statusDesc;
        }
    }
}