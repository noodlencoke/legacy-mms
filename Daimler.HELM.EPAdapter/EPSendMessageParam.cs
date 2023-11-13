using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Daimler.HELM.EPAdapter
{
    public class EPSendMessageParam
    {
        public string productId { get; set; }

        public string userName { get; set; }

        public string password { get; set; }

        public string phone_taskId_content { get; set; }
    }
}