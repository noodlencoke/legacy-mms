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
    public partial class ExceptionLogList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int pageIndex = this.DataPager.PageIndex;
                int pageSize = this.DataPager.PageSize;
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtBeginDate.Text = date;
                this.txtEndDate.Text = endDate;
                int total = ServiceFactory.GetInstance().GetExceptionLogCountByDate(date, endDate);
                this.DataPager.RecordCount = total;

                BindData(pageSize, pageIndex, date,endDate);

            }
        }
        private void BindData(int pageSize, int pageIndex, string date,string endDate)
        {
            List<ExceptionLogInfo> exceptionLogList = ServiceFactory.GetInstance().GetExceptionLogByDate(date,endDate, pageSize, pageIndex);

            List<ExceptionLogInfo> bindList = new List<ExceptionLogInfo>();
            foreach (var item in exceptionLogList)
            {
                string excetpionInfo = item.ExceptionInfo;
                if (excetpionInfo.Length >= 40)
                {
                    item.ExceptionInfo = excetpionInfo.Substring(0, 39) + "...";
                }
                bindList.Add(item);
            }
            this.DataGridView.PageIndex = this.DataPager.PageIndex;
            this.DataGridView.PageSize = this.DataPager.PageSize;
            this.DataGridView.DataSource = bindList;
            this.DataGridView.DataBind();
        }

        protected void DataPager_PageIndexChanged(object sender, EventArgs e)
        {
            int pageIndex = this.DataPager.PageIndex;
            int pageSize = this.DataPager.PageSize;
            string date = this.txtBeginDate.Text;
            string endDate = this.txtEndDate.Text;
            int total = ServiceFactory.GetInstance().GetExceptionLogCountByDate(date,endDate);
            this.DataPager.RecordCount = total;
            BindData(pageSize, pageIndex, date,endDate);
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            int pageIndex = this.DataPager.PageIndex;
            int pageSize = this.DataPager.PageSize;
            string date = this.txtBeginDate.Text;
            string endDate = this.txtEndDate.Text;
            int total = ServiceFactory.GetInstance().GetExceptionLogCountByDate(date,endDate);
            this.DataPager.RecordCount = total;
            BindData(pageSize, pageIndex, date,endDate);
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
                e.Row.Attributes.Add("onclick", "showDetail('" + id + "')");
                e.Row.Style.Add("cursor", "pointer");
            }
        }
    }
}