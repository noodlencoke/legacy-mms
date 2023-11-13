using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
   public  class MessageReplyInfo
    {

       public string taskId { get; set; }
       public string content { get; set; }
       public string getTime { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
       public Guid Id { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
       public string sessionId { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
       public string mobile { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
       public int status { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
       public string dataSource { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
       public string senderNumber { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
       public string extendNumber { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
       public string statusDesc { get; set; }
    }
}
