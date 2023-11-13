using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class RequestResultInfo
    {
        /// <summary>
        /// message task id
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// submit status
        /// </summary>
        public MessageStatus SMSStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EmailStatus EmailStatus { get; set; }

        /// <summary>
        /// submit result description
        /// </summary>
        public string Desc { get; set; }


        public string BlackList { get; set; }

        /// <summary>
        /// string of result
        /// </summary>
        public string StrResult { get; set; }

        public string SenderNumber { get; set; }

        public string ExtendNumber { get; set; }

        public string Mobile { get; set; }
    }
}
