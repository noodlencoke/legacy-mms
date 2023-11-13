using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Daimler.HELM.PublicService
{
    public class XMLMessage
    {
        public string SmsSource { get; set; }

        public string UniqueID { get; set; }

        public string SenderName { get; set; }

        public string RecipientMobile { get; set; }

        public string SmsText { get; set; }

        public int SmsPriority { get; set; }
    }
}