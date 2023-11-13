using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    [DataContract]
    public class RequestLog
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string RequestInfo { get; set; }

        [DataMember]
        public string SendContent { get; set; }

        [DataMember]
        public string SendContentOfBase64 { get; set; }

        [DataMember]
        public string DataSrouce { get; set; }

        [DataMember]
        public string CreatedDt { get; set; }
    }
}
