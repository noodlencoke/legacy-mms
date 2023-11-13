using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.DBInterface
{
    public class CommonDalMock : ICommonDal
    {

        public bool RecordExceptionLog(ExceptionLog log)
        {
            Log.LogHandler.WriteLog("Log");
            return true;
        }


        public string GetSystemConfig(string operationId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSystemConfig(string operationId, string value)
        {
            throw new NotImplementedException();
        }


        public List<InterfaceAccount> GetInterfaceAccount(string dataSource)
        {
            throw new NotImplementedException();
        }

        public List<InterfaceAccount> GetInterfaceAccount()
        {
            throw new NotImplementedException();
        }

        public List<InterfaceAccount> GetInterfaceAccountByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateInterfaceAccount(Guid id, string accountName, string accountPwd, DateTime effectiveDt, int effectiveDays, int status, string dataSource)
        {
            throw new NotImplementedException();
        }

        public bool CreateInterfaceAccount(Guid id, string accountName, string accountPwd, DateTime effectiveDt, int effectiveDays, int status, string dataSource)
        {
            throw new NotImplementedException();
        }

        public bool DeleteInterfaceAccount(Guid id)
        {
            throw new NotImplementedException();
        }


        public System.Data.DataSet GetMessageLog(string messageType, string messageSource, string beginDate, string endDate, string keyWord, int pageIndex, int pageSize, out int totalCount)
        {
            throw new NotImplementedException();
        }

        public int GetMessageLogPageCount(string messageType, string messageSource, string beginDate, string endDate, string keyWord)
        {
            throw new NotImplementedException();
        }


        public RequestLog GetLogInfoById(string id, string messageType)
        {
            throw new NotImplementedException();
        }

        public List<ExceptionLogInfo> GetExceptionLogByDate(string date,string endDate,int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }


        public int GetExceptionLogCountByDate(string date,string endDate)
        {
            throw new NotImplementedException();
        }


        public List<ShowMessageReplyInfo> GetMessageReplyInfoByCondit(ShowMessageReplyInfo where, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public int GetMessageReplyCount(ShowMessageReplyInfo where)
        {
            throw new NotImplementedException();
        }


        public ExceptionLogInfo GetExceptionLogById(string id)
        {
            throw new NotImplementedException();
        }


        public List<SourceVendorConfig> GetSourceSenderConfig()
        {
            throw new NotImplementedException();
        }


        public List<SourceVendorConfig> GetVendorAccountList()
        {
            throw new NotImplementedException();
        }


        public List<SourceVendorConfig> GetSourceSenderConfig(string dataSource, string sessionId)
        {
            throw new NotImplementedException();
        }


        public bool RecordBatchInfo(System.Data.DataTable dtMessage)
        {
            throw new NotImplementedException();
        }
    }
}
