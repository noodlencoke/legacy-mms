using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter.MessageRule
{
    public class MMSValidateRule:IFilterRule
    {
        MessageInfo msgInfo;
        string errMsg;
        readonly Dictionary<string, string> dicError = new Dictionary<string, string>();

        public bool Validation(object objInfo)
        {
           msgInfo = objInfo as MessageInfo;

           CheckMobileNo();
           
           return string.IsNullOrEmpty(errMsg);
        }

        public string GetError()
        {
            return errMsg;
        }


        public MessageInfo BusinessOperate(object objInfo)
        {
            msgInfo = objInfo as MessageInfo;
            if (!string.IsNullOrEmpty(msgInfo.Number))
            {
                if (msgInfo.Number.IndexOf("0086") == 0)
                {
                    msgInfo.Number = msgInfo.Number.Substring(4, msgInfo.Number.Length - 4);
                }
                msgInfo.Number = msgInfo.Number.Replace("+86", "");
            }
            return msgInfo;
        }

        public void CheckMobileNo()
        {
            if (string.IsNullOrEmpty(msgInfo.Number))
            {
                errMsg += "mobile number is empty!!";
            }
            if (errMsg != "")
            {
                dicError.Add("1", errMsg);
            }
        }


        public Dictionary<string, string> GetErrorList()
        {
            return dicError;
        }
    }
}
