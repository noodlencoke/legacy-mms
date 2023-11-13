using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class InterfaceAccount
    {
        public Guid Id { get; set; }

        public string AccountName { get; set; }

        public string AccountPwd { get; set; }

        public string EffectiveDt { get; set; }

        public int EffectiveDays { get; set; }

        public string ExpireDate { get; set; }

        public int RemainingDays { get; set; }

        public int Status { get; set; }

        public string Datasource { get; set; }
    }
}
