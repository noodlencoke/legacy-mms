using Daimler.HELM.BizObjects;
using Daimler.HELM.Control;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Daimler.HELM.ManagePlatform.Views.LogQuery
{
    public partial class MessageLogQueryList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtBeginDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                this.txtEndDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        protected bool DoValidate()
        {
            if (string.IsNullOrEmpty(this.txtMessageType.SelectedItem.Text))
            {
                this.txtTip.Visible = true;
                this.txtTip.Text = "MessageType field can't be empty";
                return false;
            }
            if (string.IsNullOrEmpty(this.txtMessageSource.SelectedItem.Text))
            {
                this.txtTip.Visible = true;
                this.txtTip.Text = "MessageSource field can't be empty";
                return false;
            }
            if (string.IsNullOrEmpty(this.txtBeginDate.Text))
            {
                this.txtTip.Visible = true;
                this.txtTip.Text = "BeginDate field can't be empty";
                return false;
            }
            if (string.IsNullOrEmpty(this.txtEndDate.Text))
            {
                this.txtTip.Visible = true;
                this.txtTip.Text = "EndDate field can't be empty";
                return false;
            }
            if (this.txtKeyWord.Text!=string.Empty && this.txtKeyWord.Text.Length>30)
            {
                this.txtTip.Visible = true;
                this.txtTip.Text = "KeyWord field is too long ";
                return false;
            }
            if (this.txtKeyWord.Text.Contains("%") || this.txtKeyWord.Text.Contains("_") || this.txtKeyWord.Text.Contains("[") || this.txtKeyWord.Text.Contains("]") || this.txtKeyWord.Text.Contains("^"))
            {
                this.txtTip.Visible = true;
                this.txtTip.Text = " Keyword can not contain special characters";
                return false;  
            }
            this.txtTip.Visible = false;
            return true;
        }
        protected void BindData()
        {
            try
            {
                this.DataPager.Visible = true;
                this.DataGridView.PageIndex = this.DataPager.PageIndex;
                this.DataGridView.PageSize = this.DataPager.PageSize;
                DataSet ds = null;
                if (Session["bizObject"] == null)
                {
                    InitCondition();
                }
                ds = ServiceFactory.GetInstance().GetMessageLog((Session["bizObject"] as ConditionBiz).MessageType, (Session["bizObject"] as ConditionBiz).MessageSource, (Session["bizObject"] as ConditionBiz).BeginDate, (Session["bizObject"] as ConditionBiz).EndDate, (Session["bizObject"] as ConditionBiz).KeyWord, this.DataPager.PageIndex, this.DataPager.PageSize, out totalCount);
                if (IsCount)
                {
                    this.DataPager.RecordCount = this.totalCount;
                    IsCount = false;
                }
                this.DataGridView.DataSource = ds;
                this.DataGridView.DataMember = "table";
                this.DataGridView.DataBind();
                if (ds.Tables[0].Rows.Count==0 &&this.DataPager.PageIndex==1)
                {
                    this.DataPager.Visible = false;
                }
                else
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        this.DataPager.PageIndex = this.DataPager.PageIndex - 1;
                        BindData();
                    }
                    this.DataPager.Visible = true;
  
                }

            }
            catch (Exception ex)
            {
                Daimler.HELM.BizObjects.ExceptionLog log = new Daimler.HELM.BizObjects.ExceptionLog()
                {
                    MessageType = "message log query ",
                    ExceptionInfo = ex
                };
                Daimler.HELM.HubService.Logic.CommonHandler.RecordExceptionLog(log);
            }
            
        }
        public ConditionBiz BizObject { get; set; }
        public void InitCondition()
        {
            ConditionBiz biz = new ConditionBiz();
            biz.MessageType = this.txtMessageType.SelectedItem.Text;
            biz.MessageSource = this.txtMessageSource.SelectedItem.Text;
            biz.BeginDate = this.txtBeginDate.Text;
            biz.EndDate = this.txtEndDate.Text;
            biz.KeyWord = this.txtKeyWord.Text.Trim();
            BizObject = biz;
            Session["bizObject"] = BizObject;
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            if (this.DoValidate())
            {
                this.DataGridView.Visible = true;
                this.DataPager.Visible = true;
                InitCondition();
                this.DataPager.PageIndex = 1;
                IsCount = true;
                BindData();
            }
            else {
                this.DataGridView.Visible = false;
                this.DataPager.Visible = false;
                //this.DataGridView.DataBind();
            }
        }
        private int totalCount;
        private bool IsCount = false;
        protected void MTCPager1_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void DataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            this.DataGridView.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", "return doclick('" + this.DataGridView.DataKeys[e.Row.RowIndex].Value.ToString() + "','" + (Session["bizObject"] as ConditionBiz).MessageType + "');");

                e.Row.Cells[3].ToolTip = e.Row.Cells[3].Text;
                e.Row.Cells[4].ToolTip = e.Row.Cells[4].Text;
                if (e.Row.Cells[3].Text.Length > 60)
                {
                    e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, 60) + "......";
                }
                if (e.Row.Cells[4].Text.Length > 60)
                {
                    e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 60) + "......";
                }
            }
        }

       
    }
    public class ConditionBiz
    {
        public string MessageType { get; set; }
        public string MessageSource { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string KeyWord { get; set; }
    }
}