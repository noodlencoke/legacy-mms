using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Daimler.HELM.DBInterface;
using Daimler.HELM.BizObjects;

namespace Daimler.HELM.ManagementWeb.DataAccess
{
    public class AccountOperation : IAccountOperation
    {
        public IEnumerable<InterfaceAccount> GetListAll()
        {
            ICommonDal comDal = CommonDalFactory.GetInstance();
            List<InterfaceAccount> accountList = comDal.GetInterfaceAccount();
            return accountList;
        }

        public InterfaceAccount GetByID(int id)
        {
            return null;
        }

        public bool Add(InterfaceAccount interfaceAccount)
        {
            ICommonDal comDal = CommonDalFactory.GetInstance();
            Guid id = interfaceAccount.Id;
            string accountName = interfaceAccount.AccountName;
            string accountPwd = interfaceAccount.AccountPwd;
            DateTime effectiveDt = DateTime.Parse(interfaceAccount.EffectiveDt);
            int effectiveDays = interfaceAccount.EffectiveDays;
            int status = interfaceAccount.Status;
            string datasource = interfaceAccount.Datasource;
            bool result = comDal.CreateInterfaceAccount(id, accountName, accountPwd, effectiveDt, effectiveDays, status, datasource);
            return result;
        }

        public bool Remove(Guid id)
        {
            ICommonDal comDal = CommonDalFactory.GetInstance();
            bool result = comDal.DeleteInterfaceAccount(id);
            return result;
        }

        public bool Update(InterfaceAccount interfaceAccount)
        {
            ICommonDal comDal = CommonDalFactory.GetInstance();
            Guid id = interfaceAccount.Id;
            string accountName = interfaceAccount.AccountName;
            string accountPwd = interfaceAccount.AccountPwd;
            DateTime effectiveDt = DateTime.Parse(interfaceAccount.EffectiveDt);
            int effectiveDays = interfaceAccount.EffectiveDays;
            int status = interfaceAccount.Status;
            string datasource = interfaceAccount.Datasource;
            bool result = comDal.UpdateInterfaceAccount(id, accountName, accountPwd, effectiveDt, effectiveDays, status, datasource);
            return result;
        }
    }
}