using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter
{
    public interface IFilter
    {
        bool Validate();

        MessageInfo BusinessOperate();

        string GetError();

        Dictionary<string, string> GetErrorList();
    }
}
