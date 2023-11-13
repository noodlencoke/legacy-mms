using Daimler.HELM.BizObjects;
using Daimler.HELM.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.ManagePlatform.Views.ReplyMessage
{
    public partial class ResplayMessageDisplay : System.Web.UI.Page
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
                ShowMessageReplyInfo where=new ShowMessageReplyInfo();
                where.StartDt=date;
                where.EndDt = endDate;
                where.MessageType=this.txtMessageType.SelectedItem.Text;
                where.dataSource=this.txtMessageSource.SelectedItem.Text;

                int total = ServiceFactory.GetInstance().GetMessageReplyCount(where);
                this.DataPager.RecordCount = total;


                BindData(where,pageSize, pageIndex );
            }
        }

        private void BindData(ShowMessageReplyInfo where, int pageSize, int pageIndex)
        {
            List<ShowMessageReplyInfo> replyList = new List<ShowMessageReplyInfo>();
            replyList = ServiceFactory.GetInstance().GetMessageReplyInfoByCondit(where, pageSize, pageIndex);
            this.DataGridView.PageIndex = this.DataPager.PageIndex;
            this.DataGridView.PageSize = this.DataPager.PageSize;
            this.DataGridView.DataSource = replyList;
            this.DataGridView.DataBind();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            int pageIndex = this.DataPager.PageIndex;
            int pageSize = this.DataPager.PageSize;

            string date = this.txtBeginDate.Text;
            string endDate = this.txtEndDate.Text;
            ShowMessageReplyInfo where = new ShowMessageReplyInfo();
            where.StartDt = date;
            where.EndDt = endDate;
            where.MessageType = this.txtMessageType.SelectedItem.Text;
            where.dataSource = this.txtMessageSource.SelectedItem.Text;

            int total = ServiceFactory.GetInstance().GetMessageReplyCount(where);
            this.DataPager.RecordCount = total;
            BindData(where, pageSize, pageIndex);
        }

        protected void DataPager_PageIndexChanged(object sender, EventArgs e)
        {
            int pageIndex = this.DataPager.PageIndex;
            int pageSize = this.DataPager.PageSize;

            string date = this.txtBeginDate.Text;
            string endDate = this.txtEndDate.Text;
            ShowMessageReplyInfo where = new ShowMessageReplyInfo();
            where.StartDt = date;
            where.EndDt = endDate;
            where.MessageType = this.txtMessageType.SelectedItem.Text;
            where.dataSource = this.txtMessageSource.SelectedItem.Text;

            int total = ServiceFactory.GetInstance().GetMessageReplyCount(where);
            this.DataPager.RecordCount = total;
            BindData(where, pageSize, pageIndex);
        }
    }
}