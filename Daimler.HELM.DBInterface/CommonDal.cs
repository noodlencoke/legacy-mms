using Daimler.HELM.BizObjects;
using Daimler.HELM.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.DBInterface
{
    public class CommonDal : ICommonDal
    {
        private readonly DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// log exception to db
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool RecordExceptionLog(ExceptionLog log)
        {
            string sql = @"INSERT INTO [dbo].[MessageExceptionLog] ([messageType],[messageId],[exceptionInfo],[createdDt])
                           Values(@messageType,@messageId,@exceptionInfo,@createdDt)";

            string exceptionError = log.ExceptionInfo.Message + "," + log.ExceptionInfo.StackTrace;
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@messageType", log.MessageType));
            paramList.Add(new SqlParameter("@messageId", log.MessageId));
            paramList.Add(new SqlParameter("@exceptionInfo", exceptionError));
            paramList.Add(new SqlParameter("@createdDt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")));

            int cou = dbHelper.Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }

        public bool RecordBatchInfo(DataTable dtMessage)
        {
            return dbHelper.BatchInsertDt(dtMessage);
        }


        public string GetSystemConfig(string operationId)
        {
            string sql = "SELECT configValue from SystemConfig WITH(NOLOCK) where operationId=@operationId";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@operationId", operationId));
            DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), CommandType.Text);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                if (row["configValue"] != DBNull.Value)
                {
                    return row["configValue"].ToString();
                }
            }
            return null;
        }


        public bool UpdateSystemConfig(string operationId, string value)
        {
            string sql = "Update SystemConfig set configValue=@configValue where operationId=@operationId";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@configValue", value));
            paramList.Add(new SqlParameter("@operationId", operationId));
            int cou = dbHelper.Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }

        public List<InterfaceAccount> GetInterfaceAccount(string dataSource)
        {
            List<InterfaceAccount> accountList = new List<InterfaceAccount>();
            string sql = "SELECT top 1 * FROM InterfaceAccount  WITH(NOLOCK) where dataSource=@dataSource AND [status]=0";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@dataSource", dataSource));
            DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), CommandType.Text);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        InterfaceAccount account = new InterfaceAccount();
                        if (row["accountName"] != DBNull.Value)
                        {
                            account.AccountName = row["accountName"].ToString();
                        }
                        if (row["accountPwd"] != DBNull.Value)
                        {
                            account.AccountPwd = row["accountPwd"].ToString();
                        }
                        if (row["effectiveDt"] != DBNull.Value)
                        {
                            account.EffectiveDt = row["effectiveDt"].ToString();
                        }
                        if (row["effectiveDays"] != DBNull.Value)
                        {
                            account.EffectiveDays = Convert.ToInt32(row["effectiveDays"].ToString());
                        }

                        account.ExpireDate = Convert.ToDateTime(account.EffectiveDt).AddDays(account.EffectiveDays).ToString();
                        TimeSpan tspan = Convert.ToDateTime(account.ExpireDate) - DateTime.Now;
                        account.RemainingDays = tspan.Days;
                        accountList.Add(account);
                    }
                }
            }
            return accountList;
        }


        public List<InterfaceAccount> GetInterfaceAccount()
        {
            List<InterfaceAccount> accountList = new List<InterfaceAccount>();
            string sql = @"SELECT [id]
                                  ,[accountName]
                                  ,[accountPwd]
                                  ,[effectiveDt]
                                  ,[effectiveDays]
                                  ,[status]
                                  ,[dataSource]
                              FROM [dbo].[InterfaceAccount] WITH(NOLOCK)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), CommandType.Text);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        InterfaceAccount account = new InterfaceAccount();
                        if (row["id"] != DBNull.Value)
                        {
                            account.Id = Guid.Parse(row["id"].ToString());
                        }
                        if (row["accountName"] != DBNull.Value)
                        {
                            account.AccountName = row["accountName"].ToString();
                        }
                        if (row["accountPwd"] != DBNull.Value)
                        {
                            account.AccountPwd = row["accountPwd"].ToString();
                        }
                        if (row["effectiveDt"] != DBNull.Value)
                        {
                            account.EffectiveDt = row["effectiveDt"].ToString();
                        }
                        if (row["effectiveDays"] != DBNull.Value)
                        {
                            account.EffectiveDays = Convert.ToInt32(row["effectiveDays"].ToString());
                        }
                        if (row["datasource"] != DBNull.Value)
                        {
                            account.Datasource = row["dataSource"].ToString();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            account.Status = int.Parse(row["status"].ToString());
                        }

                        account.ExpireDate = Convert.ToDateTime(account.EffectiveDt).AddDays(account.EffectiveDays).ToString();
                        TimeSpan tspan = Convert.ToDateTime(account.ExpireDate) - DateTime.Now;
                        account.RemainingDays = tspan.Days;
                        accountList.Add(account);
                    }
                }
            }
            return accountList;
        }

        public List<InterfaceAccount> GetInterfaceAccountByID(Guid id)
        {
            List<InterfaceAccount> accountList = new List<InterfaceAccount>();
            string sql = @"SELECT [id]
                                  ,[accountName]
                                  ,[accountPwd]
                                  ,[effectiveDt]
                                  ,[effectiveDays]
                                  ,[status]
                                  ,[dataSource]
                              FROM [dbo].[InterfaceAccount] WITH(NOLOCK) where id = @idIA";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@idIA", id));
            DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), CommandType.Text);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        InterfaceAccount account = new InterfaceAccount();
                        if (row["accountName"] != DBNull.Value)
                        {
                            account.AccountName = row["accountName"].ToString();
                        }
                        if (row["accountPwd"] != DBNull.Value)
                        {
                            account.AccountPwd = row["accountPwd"].ToString();
                        }
                        if (row["effectiveDt"] != DBNull.Value)
                        {
                            account.EffectiveDt = row["effectiveDt"].ToString();
                        }
                        if (row["effectiveDays"] != DBNull.Value)
                        {
                            account.EffectiveDays = Convert.ToInt32(row["effectiveDays"].ToString());
                        }

                        account.ExpireDate = Convert.ToDateTime(account.EffectiveDt).AddDays(account.EffectiveDays).ToString();
                        TimeSpan tspan = Convert.ToDateTime(account.ExpireDate) - DateTime.Now;
                        account.RemainingDays = tspan.Days;
                        accountList.Add(account);
                    }
                }
            }
            return accountList;
        }

        public bool UpdateInterfaceAccount(Guid id, string accountName, string accountPwd, DateTime effectiveDt, int effectiveDays, int status, string dataSource)
        {
            string sql = @"UPDATE [dbo].[InterfaceAccount]
                               SET [accountName] = @accountNameIA
                                  ,[accountPwd] = @accountPwdIA
                                  ,[effectiveDt] = @effectiveDtIA
                                  ,[effectiveDays] = @effectiveDaysIA
                                  ,[status] = @statusIA
                                  ,[dataSource] = @dataSourceIA
                             WHERE [id] = @idIA";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@accountNameIA", accountName));
            paramList.Add(new SqlParameter("@accountPwdIA", accountPwd));
            paramList.Add(new SqlParameter("@effectiveDtIA", effectiveDt));
            paramList.Add(new SqlParameter("@effectiveDaysIA", effectiveDays));
            paramList.Add(new SqlParameter("@statusIA", status));
            paramList.Add(new SqlParameter("@dataSourceIA", dataSource));
            paramList.Add(new SqlParameter("@idIA", id));
            int cou = dbHelper.Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }

        public bool CreateInterfaceAccount(Guid id, string accountName, string accountPwd, DateTime effectiveDt, int effectiveDays, int status, string dataSource)
        {
            string sql = @"INSERT INTO [dbo].[InterfaceAccount]
                                       ([id]
                                       ,[accountName]
                                       ,[accountPwd]
                                       ,[effectiveDt]
                                       ,[effectiveDays]
                                       ,[status]
                                       ,[dataSource])
                                 VALUES
                                       (@idIA
                                       ,@accountNameIA
                                       ,@accountPwdIA
                                       ,@effectiveDtIA
                                       ,@effectiveDaysIA
                                       ,@statusIA
                                       ,@dataSourceIA)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@accountNameIA", accountName));
            paramList.Add(new SqlParameter("@accountPwdIA", accountPwd));
            paramList.Add(new SqlParameter("@effectiveDtIA", effectiveDt));
            paramList.Add(new SqlParameter("@effectiveDaysIA", effectiveDays));
            paramList.Add(new SqlParameter("@statusIA", status));
            paramList.Add(new SqlParameter("@dataSourceIA", dataSource));
            paramList.Add(new SqlParameter("@idIA", id));
            int cou = dbHelper.Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteInterfaceAccount(Guid id)
        {
            string sql = @"DELETE FROM [dbo].[InterfaceAccount]
                                        WHERE [id] = @idIA";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@idIA", id));
            int cou = dbHelper.Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }


        public DataSet GetMessageLog(string messageType, string messageSource, string beginDate, string endDate, string keyWord, int pageIndex, int pageSize, out int totalCount)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@messageType", messageType));
            paramList.Add(new SqlParameter("@messageSource", messageSource));
            paramList.Add(new SqlParameter("@beginDate", beginDate));
            paramList.Add(new SqlParameter("@endDate", endDate));
            paramList.Add(new SqlParameter("@keyWord", keyWord));
            paramList.Add(new SqlParameter("@pageSize", pageSize));
            paramList.Add(new SqlParameter("@pageIndex", pageIndex));
            SqlParameter sqlPara = new SqlParameter("@count", 0);
            sqlPara.Direction = ParameterDirection.Output;
            paramList.Add(sqlPara);
            DataSet ds = null;
            switch (messageType)
            { 
                case "SMS":
                    ds = dbHelper.ExcuteQuery("sp_page_GetSMSLogInfo", paramList.ToArray(), CommandType.StoredProcedure);
                    break;
                case "MMS":
                    ds = dbHelper.ExcuteQuery("sp_page_GetMMSLogInfo", paramList.ToArray(), CommandType.StoredProcedure);
                    break;
                case "Email":
                    ds = dbHelper.ExcuteQuery("sp_page_GetEmailLogInfo", paramList.ToArray(), CommandType.StoredProcedure);
                    break;
                case "SMSStatus":
                    ds = dbHelper.ExcuteQuery("sp_page_GetSMSStatusLogInfo", paramList.ToArray(), CommandType.StoredProcedure);
                    break;
                case "SMSSubmitResult":
                    ds = dbHelper.ExcuteQuery("sp_page_GetSMSSubmitResultLogInfo", paramList.ToArray(), CommandType.StoredProcedure);
                    break;
                case "EmailSubmitResult":
                    ds = dbHelper.ExcuteQuery("sp_page_GetEmailSubmitResultLogInfo", paramList.ToArray(), CommandType.StoredProcedure);
                    break;
            }
         
            if (sqlPara.Value != DBNull.Value)
                totalCount = Convert.ToInt32(sqlPara.Value);
            else
                totalCount = 0;
            return ds;
        }




        public RequestLog GetLogInfoById(string id, string messageType)
        {
            RequestLog requstInfo = new RequestLog();
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@messageType", messageType));
            paramList.Add(new SqlParameter("@Id", id));

            DataSet ds = dbHelper.ExcuteQuery("sp_page_GetLogInfoById", paramList.ToArray(), CommandType.StoredProcedure);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        InterfaceAccount account = new InterfaceAccount();
                        if (row["Id"] != DBNull.Value)
                        {
                            requstInfo.Id = Guid.Parse(row["Id"].ToString());
                        }
                        if (row["SendContent"] != DBNull.Value)
                        {
                            requstInfo.SendContent = row["SendContent"].ToString();
                        }
                        if (row["DataSource"] != DBNull.Value)
                        {
                            requstInfo.DataSrouce = row["DataSource"].ToString();
                        }
                        if (row["CreateDate"] != DBNull.Value)
                        {
                            requstInfo.CreatedDt = row["CreateDate"].ToString();
                        }
                        if (row["RequestInfo"] != DBNull.Value)
                        {
                            requstInfo.RequestInfo = row["RequestInfo"].ToString();
                        }

                    }
                }
            }
            return requstInfo;
        }


        public List<ExceptionLogInfo> GetExceptionLogByDate(string date, string endDate, int pageSize, int pageIndex)
        {

            string sql = @"SELECT id,messageType,messageId,exceptionInfo,createdDt 
                           FROM 
                           (SELECT ROW_NUMBER() OVER(ORDER BY createdDt DESC) AS num,* 
                                FROM dbo.MessageExceptionLog WITH(NOLOCK) WHERE CONVERT(varchar(10),createdDt,120) BETWEEN @startDate AND @endDate) AS T 
                           WHERE num>=@start AND num<=@end";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@startDate",date),
                new SqlParameter("@endDate",endDate),
                new SqlParameter("@start",(pageIndex-1)*pageSize+1),
                new SqlParameter("@end",pageSize*pageIndex)
            };
            DataSet ds = dbHelper.ExcuteQuery(sql, pms, CommandType.Text);
            List<ExceptionLogInfo> exceptionLogList = new List<ExceptionLogInfo>();
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ExceptionLogInfo exceptionLogInfo = new ExceptionLogInfo();
                        if (row["id"] != DBNull.Value)
                        {
                            exceptionLogInfo.Id = row["id"].ToString();
                        }
                        if (row["messageType"] != DBNull.Value)
                        {
                            exceptionLogInfo.MessageType = row["messageType"].ToString();
                        }
                        if (row["messageId"] != DBNull.Value)
                        {
                            exceptionLogInfo.MessageId = row["messageId"].ToString();
                        }
                        if (row["exceptionInfo"] != DBNull.Value)
                        {
                            exceptionLogInfo.ExceptionInfo = row["exceptionInfo"].ToString();
                        }
                        if (row["createdDt"] != DBNull.Value)
                        {
                            exceptionLogInfo.CreatedDt = Convert.ToDateTime(row["createdDt"]);
                        }

                        exceptionLogList.Add(exceptionLogInfo);
                    }
                }
            }
            return exceptionLogList;
        }

        public ExceptionLogInfo GetExceptionLogById(string id)
        {
            //throw new NotImplementedException();
            string sql = @"SELECT id,messageType,messageId,exceptionInfo,createdDt
                        FROM dbo.MessageExceptionLog  WITH(NOLOCK) 
                        WHERE id=@id";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            DataSet ds = dbHelper.ExcuteQuery(sql, pms, CommandType.Text);
            ExceptionLogInfo exceptionLogInfo = new ExceptionLogInfo();
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    if (ds.Tables[0].Rows[0]["id"] != DBNull.Value)
                    {
                        exceptionLogInfo.Id = ds.Tables[0].Rows[0]["id"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["messageType"] != DBNull.Value)
                    {
                        exceptionLogInfo.MessageType = ds.Tables[0].Rows[0]["messageType"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["messageId"] != DBNull.Value)
                    {
                        exceptionLogInfo.MessageId = ds.Tables[0].Rows[0]["messageId"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["exceptionInfo"] != DBNull.Value)
                    {
                        exceptionLogInfo.ExceptionInfo = ds.Tables[0].Rows[0]["exceptionInfo"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["createdDt"] != DBNull.Value)
                    {
                        exceptionLogInfo.CreatedDt = Convert.ToDateTime(ds.Tables[0].Rows[0]["createdDt"]);
                    }
                }
            }
            return exceptionLogInfo;


        }
        public int GetExceptionLogCountByDate(string date, string endDate)
        {
            string sql = "SELECT COUNT(id) FROM dbo.MessageExceptionLog WITH(NOLOCK) WHERE CONVERT(varchar(10),createdDt,120) BETWEEN @startDate AND @endDate";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@startDate",date),
                new SqlParameter("@endDate",endDate)
            };

            return Convert.ToInt32(dbHelper.ExcuteMyScalar(sql, pms, CommandType.Text));

        }


        public List<ShowMessageReplyInfo> GetMessageReplyInfoByCondit(ShowMessageReplyInfo where, int pageSize, int pageIndex)
        {
            StringBuilder sb = new StringBuilder(@"SELECT id,mobile,status,taskId,content,dataSource,replyDt,senderNumber, messageType
                           FROM 
                           (SELECT ROW_NUMBER() OVER(ORDER BY createdDt DESC) AS num,* 
                                FROM dbo.SMSReplyInfo WITH(NOLOCK) 
								WHERE");

            if (!string.IsNullOrEmpty(where.MessageType))
            {
                sb.Append(" messageType=@messageType AND");
            }
            if (!string.IsNullOrEmpty(where.dataSource))
            {
                sb.Append(" dataSource=@dataSource AND");
            }
            sb.Append(" CONVERT(varchar(10),createdDt,120) BETWEEN @startDt AND @endDt) AS T WHERE num>=@start AND num<=@end");

            string sql = sb.ToString();
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@messageType",where.MessageType),
                new SqlParameter("@dataSource",where.dataSource),                
                new SqlParameter("@startDt",where.StartDt),
                new SqlParameter("@endDt",where.EndDt),
                new SqlParameter("@start",(pageIndex-1)*pageSize+1),
                new SqlParameter("@end",pageSize*pageIndex)
            };
            DataSet ds = dbHelper.ExcuteQuery(sql, pms, CommandType.Text);

            List<ShowMessageReplyInfo> showMessageReplyList = new List<ShowMessageReplyInfo>();
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ShowMessageReplyInfo showMessageReplyInfo = new ShowMessageReplyInfo();
                        if (row["id"] != DBNull.Value)
                        {
                            showMessageReplyInfo.Id = new Guid(row["id"].ToString());
                        }
                        if (row["mobile"] != DBNull.Value)
                        {
                            showMessageReplyInfo.mobile = row["mobile"].ToString();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            showMessageReplyInfo.status = Convert.ToInt32(row["status"]);
                        }
                        if (row["content"] != DBNull.Value)
                        {
                            showMessageReplyInfo.content = row["content"].ToString();
                        }
                        if (row["dataSource"] != DBNull.Value)
                        {
                            showMessageReplyInfo.dataSource = row["dataSource"].ToString();
                        }
                        if (row["replyDt"] != DBNull.Value)
                        {
                            showMessageReplyInfo.getTime = row["replyDt"].ToString();
                        }
                        if (row["senderNumber"] != DBNull.Value)
                        {
                            showMessageReplyInfo.senderNumber = row["senderNumber"].ToString();
                        }
                        if (row["messageType"] != DBNull.Value)
                        {
                            showMessageReplyInfo.MessageType = row["messageType"].ToString();
                        }
                        if (row["taskId"] != DBNull.Value)
                        {
                            showMessageReplyInfo.taskId = row["taskId"].ToString();
                        }
                        showMessageReplyList.Add(showMessageReplyInfo);
                    }
                }
            }
            return showMessageReplyList;
        }

        public int GetMessageReplyCount(ShowMessageReplyInfo where)
        {

            StringBuilder sb = new StringBuilder(@"SELECT COUNT(id) FROM dbo.SMSReplyInfo  WITH(NOLOCK) WHERE 1=1");

            if (!string.IsNullOrEmpty(where.MessageType))
            {
                sb.Append(" AND messageType=@messageType");
            }
            if (!string.IsNullOrEmpty(where.dataSource))
            {
                sb.Append(" AND dataSource=@dataSource");
            }
            sb.Append(" AND CONVERT(varchar(10),createdDt,120) BETWEEN @startDt AND @endDt");

            string sql = sb.ToString();
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@messageType",where.MessageType),
                new SqlParameter("@dataSource",where.dataSource),
                new SqlParameter("@startDt",where.StartDt),
                new SqlParameter("@endDt",where.EndDt)
              
            };

            return Convert.ToInt32(dbHelper.ExcuteMyScalar(sql, pms, CommandType.Text));
        }


        public List<SourceVendorConfig> GetSourceSenderConfig(string dataSource, string sessionId)
        {
            try
            {
                List<SourceVendorConfig> sourceVendorConfiglst = new List<SourceVendorConfig>();
                string sql = @"SELECT c.dataSource,c.sessionId,b.vendorName,b.accountName,b.accountPwd,b.accountType,a.signNumber,a.signName
                            FROM VendorSignConfig a
                            inner join VendorAccount b on a.venderid = b.id
                            inner join SourceSignConfig c ON c.vendorSignId = a.id
                            WHERE c.[status]=0 and c.dataSource=@dataSource and c.sessionId=@sessionId";

                SqlParameter[] pms = new SqlParameter[]{
                    new SqlParameter("@dataSource",dataSource),
                    new SqlParameter("@sessionId",sessionId)
                };

                DataSet ds = dbHelper.ExcuteQuery(sql, pms, CommandType.Text);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            SourceVendorConfig sourceVendorConfig = new SourceVendorConfig();
                            if (row["dataSource"] != DBNull.Value)
                            {
                                sourceVendorConfig.DataSource = row["dataSource"].ToString();
                            }
                            if (row["vendorName"] != DBNull.Value)
                            {
                                sourceVendorConfig.VendorName = row["vendorName"].ToString();
                            }

                            if (row["accountName"] != DBNull.Value)
                            {
                                sourceVendorConfig.AccountName = row["accountName"].ToString();
                            }
                            if (row["accountPwd"] != DBNull.Value)
                            {
                                sourceVendorConfig.AccountPwd = row["accountPwd"].ToString();
                            }
                            if (row["accountType"] != DBNull.Value)
                            {
                                sourceVendorConfig.AccountType = row["accountType"].ToString();
                            }
                            if (row["signNumber"] != DBNull.Value)
                            {
                                sourceVendorConfig.SignNumber = row["signNumber"].ToString();
                            }

                            if (row["signName"] != DBNull.Value)
                            {
                                sourceVendorConfig.SignName = row["signName"].ToString();
                            }

                            if (row["sessionId"] != DBNull.Value)
                            {
                                sourceVendorConfig.SessionId = row["sessionId"].ToString();
                            }

                            sourceVendorConfiglst.Add(sourceVendorConfig);
                        }
                    }
                }

                return sourceVendorConfiglst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "," + ex.StackTrace);
            }
        }



        public List<SourceVendorConfig> GetVendorAccountList()
        {
            List<SourceVendorConfig> configList = new List<SourceVendorConfig>();

            try
            {
                string sql = "SELECT accountName,accountPwd,accountType,vendorName FROM VendorAccount";
                DataSet ds = dbHelper.ExcuteQuery(sql, null, CommandType.Text);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            SourceVendorConfig config = new SourceVendorConfig();
                            if (row["accountName"] != DBNull.Value)
                            {
                                config.AccountName = row["accountName"].ToString();
                            }
                            if (row["accountPwd"] != DBNull.Value)
                            {
                                config.AccountPwd = row["accountPwd"].ToString();
                            }
                            if (row["vendorName"] != DBNull.Value)
                            {
                                config.VendorName = row["vendorName"].ToString();
                            }

                            if (row["accountType"] != DBNull.Value)
                            {
                                config.AccountType = row["accountType"].ToString();
                            }
                            configList.Add(config);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "," + ex.StackTrace);
            }
            return configList;
        }
    }


    public interface ICommonDal
    {
        bool RecordExceptionLog(ExceptionLog log);

        bool RecordBatchInfo(DataTable dtMessage);

        string GetSystemConfig(string operationId);

        bool UpdateSystemConfig(string operationId, string value);

        List<InterfaceAccount> GetInterfaceAccount(string dataSource);

        List<InterfaceAccount> GetInterfaceAccount();

        List<InterfaceAccount> GetInterfaceAccountByID(Guid id);

        bool UpdateInterfaceAccount(Guid id, string accountName, string accountPwd, DateTime effectiveDt, int effectiveDays, int status, string dataSource);

        bool CreateInterfaceAccount(Guid id, string accountName, string accountPwd, DateTime effectiveDt, int effectiveDays, int status, string dataSource);

        bool DeleteInterfaceAccount(Guid id);


        DataSet GetMessageLog(string messageType, string messageSource, string beginDate, string endDate, string keyWord, int pageIndex, int pageSize, out int totalCount);
        RequestLog GetLogInfoById(string id, string messageType);

        List<ExceptionLogInfo> GetExceptionLogByDate(string date, string endDate, int pageSize, int pageIndex);
        ExceptionLogInfo GetExceptionLogById(string id);
        int GetExceptionLogCountByDate(string date, string endDate);

        List<ShowMessageReplyInfo> GetMessageReplyInfoByCondit(ShowMessageReplyInfo where, int pageSize, int pageIndex);
        int GetMessageReplyCount(ShowMessageReplyInfo where);

        List<SourceVendorConfig> GetSourceSenderConfig(string dataSource, string sessionId);

        List<SourceVendorConfig> GetVendorAccountList();
    }


    public static class CommonDalFactory
    {
        public static ICommonDal GetInstance()
        {
            string mock = System.Configuration.ConfigurationManager.AppSettings["isMock"];
            if (mock == "1")
            {
                return new CommonDalMock();
            }
            else
            {
                return new CommonDal();
            }
        }
    }
}
