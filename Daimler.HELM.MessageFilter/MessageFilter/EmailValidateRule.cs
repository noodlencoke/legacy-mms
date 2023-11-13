using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter.MessageRule
{
    public class EmailValidateRule:IFilterRule
    {
        MessageInfo msgInfo;
        string errMsg;
        readonly Dictionary<string, string> dicError = new Dictionary<string, string>();

        public bool Validation(object objInfo)
        {
           msgInfo = objInfo as MessageInfo;

           CheckEmailAddress();
           
           return string.IsNullOrEmpty(errMsg);
        }

        public string GetError()
        {
            return errMsg;
        }


        public MessageInfo BusinessOperate(object objInfo)
        {
            msgInfo = objInfo as MessageInfo;
            return msgInfo;
        }

        public void CheckEmailAddress()
        {
            string regexStr = @"^([a-z0-9A-Z]+[-|\.]?)+[a-z0-9A-Z]@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\.)+[a-zA-Z]{2,}$";
            if (string.IsNullOrEmpty(msgInfo.Number))
            {
                errMsg += "email address is empty!";
            }
            if (!Regex.IsMatch(msgInfo.Number, regexStr))
            {
                errMsg += "email address error！";
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
