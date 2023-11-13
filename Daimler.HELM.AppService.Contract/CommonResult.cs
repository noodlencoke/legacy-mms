using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.HubService.Contract
{
    [DataContract]
    public class CommonResult
    {
        [DataMember]
        public bool IsOK { get; set; }
        [DataMember]
        public string ReturnMessage { get; set; }

        [DataMember]
        public Dictionary<string, object> DicObj { get; set; }
    }
}
