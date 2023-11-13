using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
   public  class MessageStateInfo
    {
       /// <summary>
       /// only Id
       /// </summary>
        [System.Web.Script.Serialization.ScriptIgnore]
       public string id { get; set; }
        /// <summary>
        /// Mercedes-Benz offers downlink message
        /// </summary>
       public string taskId { get; set; }
         /// <summary>
         //SMS receipt states: 
         //0: Successful Receipt 
         //1: Receipt Failed 
         //2: Blacklist Users 
         //3: Content containing keywords 
         //4: The phone number format error 
         //5: Commit failed 
         //6: Business ID / user name / password error 
         //7: No corresponding channel 
         //8: Do not support this number section 
         //9: Repeat number 
        /// </summary>
       public string status { get; set; }
       /// <summary>
       /// Status Description 
       /// </summary>
       public string statusDesc { get; set; }
       /// <summary>
       /// Receipt Return time (Receipt success / failure of receipt) 
       /// </summary>
       public string returnTime { get; set; }
       /// <summary>
       /// messageType: SMS or MMS
       /// </summary>
       [System.Web.Script.Serialization.ScriptIgnore]
       public string messageType { get; set; }

    }
}
