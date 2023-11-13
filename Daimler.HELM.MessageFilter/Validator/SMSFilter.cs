using Daimler.HELM.BizObjects;
using Daimler.HELM.MessageFilter.MessageRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter.Validator
{
    public class SMSFilter:FilterBase<MessageInfo>
    {
        public SMSFilter(MessageInfo msgInfo)
            : base(msgInfo)
        { }

        protected override void BindValidationRule()
        {
            this.ruleList.Add(new SMSValidateRule());
        }
    }
}
