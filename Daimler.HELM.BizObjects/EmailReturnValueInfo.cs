using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class ReturnValueInfo
    {
        public string Status { get; set; }
        public string Comment { get; set; }
    }
    /// <summary>
    /// Email return value
    /// </summary>
   public  class EmailReturnValueInfo:ReturnValueInfo
    {
       public string TaskId { get; set; }
       public string TaskName { get; set; }
    }
}
