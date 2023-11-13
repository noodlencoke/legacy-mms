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
    public partial class ExceptionLogDisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {                
                int pageIndex = this.DataPager.PageIndex;
                int pageSize = this.DataPager.PageSize;                
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtBeginDate.Text = date;
                int total = ServiceFactory.GetInstance().GetExceptionLogCountByDate(date);               
                this.DataPager.RecordCount = total;

                BindData(pageSize, pageIndex, date);
               
            }
        }

        private void BindData(int pageSize,int pageIndex ,string date)
        {
            List<ExceptionLogInfo> exceptionLogList = ServiceFactory.GetInstance().GetExceptionLogByDate(date, pageSize, pageIndex);

            List<ExceptionLogInfo> bindList = new List<ExceptionLogInfo>();
            foreach (var item in exceptionLogList)
            {
                string excetpionInfo = item.ExceptionInfo;
                if (excetpionInfo.Length >= 100)
                {
                    item.ExceptionInfo = excetpionInfo.Substring(0, 99) + "...";
                }
                bindList.Add(item);
            }
            this.DataGridView.DataSource = bindList;
            this.DataGridView.DataBind();
        }

        protected void DataPager_PageIndexChanged(object sender, EventArgs e)
        {
            int pageIndex = this.DataPager.PageIndex;
            int pageSize = this.DataPager.PageSize;
            string date = this.txtBeginDate.Text;
            int total = ServiceFactory.GetInstance().GetExceptionLogCountByDate(date);
            this.DataPager.RecordCount = total;
            BindData(pageSize, pageIndex, date);
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            int pageIndex = this.DataPager.PageIndex;
            int pageSize = this.DataPager.PageSize;
            string date = this.txtBeginDate.Text;

            int total = ServiceFactory.GetInstance().GetExceptionLogCountByDate(date);
            this.DataPager.RecordCount = total;
            BindData(pageSize, pageIndex, date);
        }
    }
}