using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter.MessageRule
{
    public class SMSValidateRule:IFilterRule
    {
        MessageInfo msgInfo;
        string errMsg;
        readonly Dictionary<string, string> dicError = new Dictionary<string, string>();

        public bool Validation(object objInfo)
        {
           msgInfo = objInfo as MessageInfo;

           CheckMobileNo();

           CheckSendingContent();

           CheckDataSource();

           

           return string.IsNullOrEmpty(errMsg);
        }

        public string GetError()
        {
            return errMsg;
        }

        public Dictionary<string, string> GetErrorList()
        {
            return dicError;
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
            string errorMsg = "";

            string reg = @"^\s*\+?\s*(\(\s*\d+\s*\)|\d+)(\s*-?\s*(\(\s*\d+\s*\)|\s*\d+\s*))*\s*$";
            if (!Regex.IsMatch(msgInfo.Number, reg))
            {
                errorMsg = "the mobile number formate incorrect！";
                errMsg += errorMsg;
            }


            if (errorMsg != "")
            {
                dicError.Add("1", errorMsg);
            }
        }

        public void CheckSendingContent()
        {
            string errorMsg = "";
            if (string.IsNullOrEmpty(msgInfo.Content))
            {
                errorMsg = "the short message content is empty !";
                errMsg += errorMsg; 
            }

            if (errorMsg != "")
            {
                dicError.Add("2", errorMsg);
            }
        }

        public void CheckDataSource()
        {
            string errorMsg = "";
            if (string.IsNullOrEmpty(msgInfo.DataSource))
            {
                errorMsg = "the message dataSource is empty !";
                errMsg += errorMsg;
                dicError.Add("3", errorMsg);
            }

          
        }


    }
}
