using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.DBInterface
{
    public class WechatDal : IWechatDal
    {
        private readonly DBHelper dbHelper = new DBHelper();
        public List<CustomerInfo> GetCustomerInfoByMobileNumber(string mobileNumber)
        {
            List<CustomerInfo> customerList = new List<CustomerInfo>();
            try
            {
                string sql = "SELECT * FROM CustomerInfo WITH(NOLOCK) WHERE mobile=@mobile and [status]=0";
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@mobile",mobileNumber));
                DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), System.Data.CommandType.Text);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            CustomerInfo customerInfo = new CustomerInfo();
                            if (row["wechatId"] != DBNull.Value)
                            {
                                customerInfo.WechatId = row["wechatId"].ToString(); 
                            }

                            customerList.Add(customerInfo);
                        }
                    }

                }
                return customerList;
            }
            catch (Exception ex)
            {
                throw new Exception("get customer info by mobile error:" + ex.Message + ex.StackTrace);
            }
        }

        public bool AddCustomerInfo(Dictionary<string, object> dicCusomterInfo)
        {
            return dbHelper.InsertTable("CustomerInfo", dicCusomterInfo);
        }
    }
}
