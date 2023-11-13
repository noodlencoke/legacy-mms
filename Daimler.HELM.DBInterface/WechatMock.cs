using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.DBInterface
{
    public class WechatMock:IWechatDal
    {
        public List<CustomerInfo> GetCustomerInfoByMobileNumber(string mobileNumber)
        {
            throw new NotImplementedException();
        }

        public bool AddCustomerInfo(Dictionary<string, object> dicCusomterInfo)
        {
            throw new NotImplementedException();
        }
    }

    public interface IWechatDal {
        List<CustomerInfo> GetCustomerInfoByMobileNumber(string mobileNumber);

        bool AddCustomerInfo(Dictionary<string, object> dicCusomterInfo);
    }

    public static class WechatDalFactory
    {
        public static IWechatDal GetInstance()
        {
            string mock = System.Configuration.ConfigurationManager.AppSettings["isMock"];
            if (mock == "1")
            {
                return new WechatMock();
            }
            else
            {
                return new WechatDal();
            }
        }
    }
}
