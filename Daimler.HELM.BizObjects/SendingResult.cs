using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public class SendingResult
    {
        /// <summary>
        /// message sending task id
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// phone number / email/ wechat id
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// message sending status
        /// </summary>
        public MessageStatus Status { get; set; }

        /// <summary>
        /// message sending result description 
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// the time of send to user mobile
        /// </summary>
        public string SendTime { get; set; }

        /// <summary>
        /// message sending result string(xml/json)
        /// </summary>
        public string StrSendingResult { get; set; }

    }
}
                                             