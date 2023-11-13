using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.EPAdapter
{
    public partial class MMSGetStauts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/Json";
            Response.Charset = "UTF-8";
            string returnVal = Log.LogHandler.ReadLog("MMS-GetStatus");
            Response.Write(returnVal);
            Response.End();
        }
    }
}