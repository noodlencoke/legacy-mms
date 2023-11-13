using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.HubServiceProxy
{
    public class BaseServiceProxy<TChannel> : ClientBase<TChannel> where TChannel : class
    {

    }
}
