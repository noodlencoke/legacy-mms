using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    /// <summary>
    /// send message exception log
    /// </summary>
    [DataContract]
    public class ExceptionLog
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid MessageId { get; set; }

        [DataMember]
        public string MessageType { get; set; }

        [DataMember]
        public Exception ExceptionInfo { get; set; }

        [DataMember]
        public string CreatedDt { get; set; }
    }
}
