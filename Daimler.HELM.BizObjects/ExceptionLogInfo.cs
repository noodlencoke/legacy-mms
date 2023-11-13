using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    [DataContract]
    public class ExceptionLogInfo
    {
        [DataMember]
        public string Id { set; get; }

        [DataMember]
        public string MessageId { get; set; }

        [DataMember]
        public string MessageType { get; set; }

        [DataMember]
        public string ExceptionInfo { get; set; }

        [DataMember]
        public DateTime CreatedDt { get; set; }
    }
}
