using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class SourceVendorConfig
    {
        public string DataSource { get; set; }

        public string VendorName { get; set; }

        public string AccountName { get; set; }

        public string AccountPwd { get; set; }
        
        public string SignNumber { get; set; }

        public string SignName { get; set; }

        public string AccountType { get; set; }

        public string SessionId { get; set; }

    }
}
