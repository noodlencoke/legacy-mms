using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    /// <summary>
    /// LMS return value
    /// </summary>
    public class LMSReturnValueInfo
    {
       public string Status { get; set; }
       public string Comment { get; set; }
       public string TaskId { get; set; }
    }
}
