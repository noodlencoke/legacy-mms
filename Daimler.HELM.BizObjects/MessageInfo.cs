using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    [DataContract]
    public class MessageInfo
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid RequestId { get; set; }

        /// <summary>
        /// the message type(SMS/MMS/Email/Wechat)
        /// </summary>
        [DataMember]
        public MessageType Type { get; set; }

        /// <summary>
        /// the message content
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// mobile or email or wechatid 
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        /// message send task id
        /// </summary>
        [DataMember]
        public string TaskId { get; set; }


        /// <summary>
        /// send message product id
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }


        /// <summary>
        /// the message template id
        /// </summary>
        [DataMember]
        public string TemplateId { get; set; }


        /// <summary>
        /// the message come from 
        /// </summary>
        [DataMember]
        public string DataSource { get; set; }


        /// <summary>
        /// message sending priority
        /// </summary>
        [DataMember]
        public MessagePriority Priority { get; set; }

        /// <summary>
        /// message received time from business system
        /// </summary>
         [DataMember]
        public string ReceivedDt { get; set; }


        /// <summary>
        ///  message classification (single/batch)
        /// </summary>
        [DataMember]
        public MessageClassification Classification { get; set; }


        /// <summary>
        /// the vendor name of sending message
        /// </summary>
        [DataMember]
        public string VendorName { get; set; }


        /// <summary>
        /// the message sending status
        /// </summary>
        [DataMember]
        public MessageStatus Status
        {
            get;
            set;
        }

        [DataMember]
        public EmailStatus EmailStatus { get; set; }

        /// <summary>
        /// helm validate error info
        /// </summary>  
        [DataMember]
        public string ErrorInfo { get; set; }

        [DataMember]
        public string VendorTaskId { get; set; }
        /// <summary>
        /// Batch Id
        /// </summary>
        [DataMember]
        public string BatchId { get; set; }
        /// <summary>
        /// Email Name
        /// </summary>
        [DataMember]
        public string EmailName { get; set; }
        /// <summary>
        /// Email interestedSeries
        /// </summary>
        [DataMember]
        public string InterestedSeries { get; set; }
        /// <summary>
        /// Email From Address
        /// </summary>
        [DataMember]
        public string From { get; set; }
        /// <summary>
        /// Email Reply Address
        /// </summary>
        [DataMember]
        public string Reply { get; set; }
        /// <summary>
        /// Email Subject
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string SubDesc { get; set; }

        [DataMember]
        public string ExtendNumber { get; set; }

        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public bool IsNeedReply { get; set; }
        [DataMember]
        public string SenderNumber { get; set; }

        [DataMember]
        public string CreatedDt { get; set; }

        [DataMember]
        public string SubmitDt { get; set; }

        [DataMember]
        public string GetStatusDt { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string AccountPwd { get; set; }

        [DataMember]
        public string SignName { get; set; }

    }
}
