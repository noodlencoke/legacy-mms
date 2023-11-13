using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageFilter
{
    public abstract class FilterBase<T>:IFilter
    {
        protected T Obj;
        protected List<IFilterRule> ruleList;
        protected string errorMsg;
        protected Dictionary<string, string> dicErrorList = new Dictionary<string, string>();
        public FilterBase(T nObj)
        {
            Obj = nObj;
            ruleList = new List<IFilterRule>();
            BindValidationRule();
        }

        protected abstract void BindValidationRule();

        public bool Validate()
        {
            var errRule = ruleList.FindAll(r => !r.Validation(Obj));
            errRule.ToList().ForEach(r =>{
                errorMsg += r.GetError().Trim() + "\r\n";
                Dictionary<string,string> dicErrors =  r.GetErrorList();
                foreach (string key in dicErrors.Keys)
                {
                    dicErrorList.Add(key,dicErrors[key]);
                }
            });

            

            return string.IsNullOrEmpty(errorMsg);
        }

        public string GetError()
        {
            return errorMsg;
        }


        public MessageInfo BusinessOperate()
        {
            MessageInfo msgInfo = new MessageInfo();
            foreach (IFilterRule rule in ruleList)
            {
               msgInfo = rule.BusinessOperate(Obj);
            }
            return msgInfo;
        }


        public Dictionary<string, string> GetErrorList()
        {
            return dicErrorList;
        }
    }
}
