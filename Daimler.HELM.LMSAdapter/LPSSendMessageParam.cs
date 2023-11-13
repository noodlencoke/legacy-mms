using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Daimler.HELM.LMSAdapter
{
    public class LMSSendMessageParam
    {
        public string applicationID { get; set; }

        public string destinationAddresses { get; set; }

        public string extendCode { get; set; }

        public string message { get; set; }

        public string messageFormat { get; set; }

        public string sendMethod { get; set; }

        public bool deliveryResultRequest { get; set; }
    }
}