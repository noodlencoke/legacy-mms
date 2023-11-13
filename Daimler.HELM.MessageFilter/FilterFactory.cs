using Daimler.HELM.BizObjects;
using Daimler.HELM.MessageFilter.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter
{
    public static class FilterFactory
    {
        public static IFilter CreateFilter(MessageInfo msgInfo)
        {
            if (msgInfo.Type == MessageType.SMS)
            {
                return new SMSFilter(msgInfo);
            }
            else if (msgInfo.Type == MessageType.MMS)
            {
                return new MMSFilter(msgInfo); 
            }
            else if (msgInfo.Type == MessageType.Email)
            {
                return new EmailFilter(msgInfo);
            }
            return null;
        }
    }
}
