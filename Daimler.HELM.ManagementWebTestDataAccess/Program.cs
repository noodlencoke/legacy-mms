using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daimler.HELM.ManagementWeb.DataAccess;
using Daimler.HELM.DBInterface;
using Daimler.HELM.BizObjects;

namespace TestDataConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountOperation ao = new AccountOperation();

            //InterfaceAccount ia = new InterfaceAccount();
            //ia.Id = Guid.NewGuid();
            //ia.AccountName="MatthewYang";
            //ia.AccountPwd="jolin";
            //ia.EffectiveDt = DateTime.Now.ToString();
            //ia.EffectiveDays = 100;
            //ia.Status = 0;
            //ia.Datasource = "EP";
            //ao.Add(ia);

            //Guid id = Guid.Parse("FE706844-C4F3-4C31-A9CF-BB9273DAE6DF");
            //ao.Remove(id);

            //InterfaceAccount ia = new InterfaceAccount();
            //ia.Id = Guid.Parse("66B5502D-EF25-4A0C-9DD5-EBEAF926952E");
            //ia.AccountName = "Matthew-Yang";
            //ia.AccountPwd = "Matthew-Yang";
            //ia.EffectiveDt = DateTime.Now.ToString();
            //ia.EffectiveDays = 10;
            //ia.Status = 0;
            //ia.Datasource = "LMS";
            //ao.Update(ia);

            IEnumerable<InterfaceAccount> accountLst = ao.GetListAll();
            foreach(InterfaceAccount ita in accountLst)
            {
                Console.WriteLine(ita.AccountName);
                Console.WriteLine(ita.AccountPwd);
            }


        }
    }
}
