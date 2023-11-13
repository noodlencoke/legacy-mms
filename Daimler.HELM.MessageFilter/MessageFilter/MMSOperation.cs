using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter.MessageRule
{
    class MMSOperation:IFilterRule
    {
        MessageInfo msgInfo;
        string errMsg;

        public bool Validation(object vObj)
        {
            msgInfo = (MessageInfo)vObj;
            //log

            return string.IsNullOrEmpty(errMsg);
        }

        public string GetError()
        {
            return errMsg;
        }
    }
}
