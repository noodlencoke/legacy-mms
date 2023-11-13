using Daimler.HELM.Control;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.ManagePlatform
{
    public partial class MessageSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitPage();
            }
        }

        private void InitPage()
        {
            this.txtStartReceviedDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.txtEndReceviedDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
             this.DataPager.PageIndex = 1;
             int totalCount =0;
             DataTable dt = SearchData(out totalCount);
             if (dt != null)
             {
                 if (totalCount > 100 * this.DataPager.PageSize)
                 {
                     this.DataPager.RecordCount = 1000;
                 }
                 else
                 {
                     this.DataPager.RecordCount = totalCount;
                 }
                 this.DataGridView.DataSource = dt;
                 this.DataGridView.DataMember = "table";
                 this.DataGridView.DataBind();
             }
        }

        private DataTable SearchData(out int totalCount)
        {
            totalCount = 0;
            //validate
            string messageType = this.ddlMessageType.SelectedValue;
            if (messageType != "SMS" && messageType != "MMS" && messageType != "Email")
            {
                return null;
            }


            Dictionary<string, object> conditions = new Dictionary<string, object>();
            if (this.ddlDataSource.SelectedValue != "")
            {
                conditions.Add("dataSource", this.ddlDataSource.SelectedValue);
            }
            if (!string.IsNullOrEmpty(this.txtNumber.Text))
            {
                if (messageType == "SMS" || messageType == "MMS")
                {
                    conditions.Add("mobile", this.txtNumber.Text);
                }
                else
                {
                    conditions.Add("address", this.txtNumber.Text);
                }
            }
            if (this.ddlClassification.SelectedValue != "")
            {
                conditions.Add("classification", this.ddlClassification.SelectedValue);
            }

            if (this.ddlStatus.SelectedValue != "")
            {
                conditions.Add("status", this.ddlStatus.SelectedValue);
            }

            if (!string.IsNullOrEmpty(this.txtTaskId.Text))
            {
                conditions.Add("taskId", this.txtTaskId.Text);
            }

            if (!string.IsNullOrEmpty(this.txtStartReceviedDate.Text))
            {
                conditions.Add("start_receivedDt", this.txtStartReceviedDate.Text);
            }

            if (!string.IsNullOrEmpty(this.txtEndReceviedDate.Text))
            {
                conditions.Add("end_receivedDt", this.txtEndReceviedDate.Text);
            }

            if (!string.IsNullOrEmpty(this.txtContent.Text))
            {
                if (messageType == "SMS")
                {
                    conditions.Add("content", this.txtContent.Text);
                }
                else
                {
                    conditions.Add("templateId", this.txtNumber.Text);
                }
            }


            this.DataPager.Visible = true;
            this.DataGridView.PageIndex = this.DataPager.PageIndex;
            this.DataGridView.PageSize = this.DataPager.PageSize;

            DataTable dt = ServiceFactory.GetInstance().GetMessageInfoByPage(messageType, conditions, this.DataPager.PageSize, this.DataPager.PageIndex, out totalCount);
           
            if(dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                { 
                    string id = row["id"].ToString();
                    if (Cache[id]!=null)
                    {
                        Cache.Remove(id);
                    }
                    Cache.Add(id, row, null, DateTime.Now.AddHours(8), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
            }
            return dt;
        }

        protected void DataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            this.DataGridView.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton btn = e.Row.FindControl("lbDetail") as LinkButton;
                //string id = e.Row.Cells[1].Text;
                //btn.PostBackUrl = "MessageDetail.aspx?id='" + id + "'";

                HiddenField hd = e.Row.FindControl("hdId") as HiddenField;
                string id = hd.Value;
                e.Row.Attributes.Add("onclick","showDetail('"+id+"')");
                e.Row.Style.Add("cursor", "pointer");
            }
        }                                                                  

        protected void DataPager_PageIndexChanged(object sender, EventArgs e)
        {
            int totalCount = 0;
            DataTable dt = SearchData(out totalCount);
            this.DataGridView.DataSource = dt;
            this.DataGridView.DataMember = "table";
            this.DataGridView.DataBind();
        }
    }
}