using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter
{
    public interface IFilterRule
    {
        bool Validation(object objInfo);
        string GetError();
        Dictionary<string, string> GetErrorList();
        MessageInfo BusinessOperate(object objInfo);
    }
}
