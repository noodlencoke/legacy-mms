using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.ManagePlatform.Views.MessageQuery
{
    public partial class MessageDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string ShowMessage()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }
            string strDetailInfo = string.Empty;
            if (Cache[id] != null)
            {
                DataRow row = Cache[id] as DataRow;
                foreach (DataColumn column in row.Table.Columns)
                {
                    string columnName = column.ColumnName;
                    if (!columnName.Contains("_"))
                    {
                        string strRow = "<tr>";
                        strRow += string.Format("<td align='right' bgcolor='#eff6fe' class='auto-style9'>{0}:</td>", columnName);
                        strRow += string.Format("<td align='left' colspan='3' class='auto-style9'>{0}</td>", row[columnName]);
                        strRow += "</tr>";
                        strDetailInfo += strRow;
                    }
                }
            }
            return strDetailInfo;
        }
    }
}