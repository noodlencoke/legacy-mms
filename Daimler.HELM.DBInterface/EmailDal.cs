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
    public class EmailDal : IEmailDal
    {
        private readonly DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// record the send email request log
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool RecordEmailSendLog(RequestLog request)
        {
            string sql = @"INSERT INTO HELMMessageDB..EmailSendLog(id,reqeustInfo,sendContent,dataSource,createdDt)
                            VALUES(@id,@reqeustInfo,@sendContent,@dataSource,@createdDt)";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", request.Id));
            paramList.Add(new SqlParameter("@reqeustInfo", request.RequestInfo));
            paramList.Add(new SqlParameter("@sendContent", request.SendContent));
            paramList.Add(new SqlParameter("@dataSource", request.DataSrouce));
            paramList.Add(new SqlParameter("@createdDt", request.CreatedDt));

            int cou = dbHelper.Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }

        public List<MessageInfo> GetEmailMessageByTaskId(string taskId)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string sql = "SELECT id,address,taskId,vendorName FROM EmailSendInfo WITH(NOLOCK) where taskId=@taskId order by [priority] desc";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@taskId", taskId));

            DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), System.Data.CommandType.Text);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MessageInfo msgInfo = new MessageInfo();
                    if (row["id"] != DBNull.Value)
                    {
                        msgInfo.Id = new Guid(row["id"].ToString());
                    }
                    if (row["address"] != DBNull.Value)
                    {
                        msgInfo.Number = row["address"].ToString();
                    }
                    if (row["taskId"] != DBNull.Value)
                    {
                        msgInfo.TaskId = row["taskId"].ToString();
                    }
                    if (row["vendorName"] != DBNull.Value)
                    {
                        msgInfo.VendorName = row["vendorName"].ToString();
                    }

                    messageList.Add(msgInfo);
                }
            }

            return messageList;
        }

        public bool RecordEmailSendingInfo(Dictionary<string, object> dicMsgInfo)
        {
            return dbHelper.InsertTable("EmailSendInfo", dicMsgInfo);
        }


        public bool UpdateEmialMessage(Dictionary<string, object> dicConditions, Dictionary<string, object> dicUpdateField)
        {
            return dbHelper.UpdateTable("EmailSendInfo", dicConditions, dicUpdateField);
        }

        public bool UpdateEmailSendingStatus(MessageInfo msgInfo, bool isRepeatSend)
        {
            try
            {
                string repeatSetValue = string.Empty;
                if (isRepeatSend)
                {
                    repeatSetValue = ",repeatSentTimes=isnull(repeatSentTimes,0)+1 ";
                }
                string sql = @"UPDATE EmailSendInfo 
                            SET [status]=@status
                               ,vendorName=@vendorName
                               ,submitDt=@submitDt
                               ,subDesc=@subDesc
                               ,vendorTaskId=@vendorTaskId
                               " + repeatSetValue + " WHERE id=@id";

                object venderName = msgInfo.VendorName;
                object submitDt = msgInfo.SubmitDt;
                object subDesc = msgInfo.SubDesc;
                object vendorTaskId = msgInfo.VendorTaskId;

                venderName = venderName == null ? DBNull.Value : venderName;
                submitDt = submitDt == null ? DBNull.Value : submitDt;
                subDesc = subDesc == null ? DBNull.Value : subDesc;
                vendorTaskId = vendorTaskId == null ? DBNull.Value : vendorTaskId;

                SqlParameter[] paramArray = new SqlParameter[] { 
                    new SqlParameter("@status",(int)msgInfo.EmailStatus),
                    new SqlParameter("@vendorName",venderName),
                    new SqlParameter("@submitDt",submitDt),
                    new SqlParameter("@subDesc",subDesc),
                    new SqlParameter("@vendorTaskId",vendorTaskId),
                    new SqlParameter("@id",msgInfo.Id)
            };

                int cou = dbHelper.Execute(sql, paramArray, CommandType.Text);
                if (cou > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateMMSSendingStatus error" + "," + ex.Message + "," + ex.StackTrace);
            }
        }


        public bool RecordEmailSubmitResultLog(RequestResultInfo requestResult, Guid messageId, string vendorName)
        {
            Dictionary<string,object> dicInsertColumns = new Dictionary<string,object>();
            dicInsertColumns.Add("messageId",messageId);                                 
            dicInsertColumns.Add("vendorName",vendorName);                               
            dicInsertColumns.Add("content",requestResult.StrResult);
            dicInsertColumns.Add("createdDt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
           
            return dbHelper.InsertTable("EmailSubmitResultLog", dicInsertColumns);
        }


        public List<MessageInfo> GetBatchEmailList()
        {
            List<MessageInfo> messageList = new List<MessageInfo>();
            string sql = @"SELECT id,[address],name,interestedseries,templateid,taskId,batchid,
                                    dataSource,emailfrom,reply,[subject],vendorName,[priority]
                           FROM EmailSendInfo WITH(NOLOCK) where [status] is null and classification = 2";

            DataSet ds = dbHelper.ExcuteQuery(sql,null,CommandType.Text);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MessageInfo msgInfo = new MessageInfo();
                    if (row["id"] != DBNull.Value)
                    {
                        msgInfo.Id = new Guid(row["id"].ToString());
                    }
                    if (row["address"] != DBNull.Value)
                    {
                        msgInfo.Number = row["address"].ToString();
                    }
                    if (row["name"] != DBNull.Value)
                    {
                        msgInfo.EmailName = row["name"].ToString();
                    }
                    if (row["interestedseries"] != DBNull.Value)
                    {
                        msgInfo.InterestedSeries = row["interestedseries"].ToString();
                    }
                    if (row["templateid"] != DBNull.Value)
                    {
                        msgInfo.TemplateId = row["templateid"].ToString();
                    }
                    if (row["taskId"] != DBNull.Value)
                    {
                        msgInfo.TaskId = row["taskId"].ToString();
                    }
                    if (row["batchid"] != DBNull.Value)
                    {
                        msgInfo.BatchId = row["batchid"].ToString();
                    }
                    if (row["dataSource"] != DBNull.Value)
                    {
                        msgInfo.DataSource = row["dataSource"].ToString();
                    }
                    if (row["emailfrom"] != DBNull.Value)
                    {
                        msgInfo.From = row["emailfrom"].ToString();
                    }
                    if (row["reply"] != DBNull.Value)
                    {
                        msgInfo.Reply = row["reply"].ToString();
                    }
                    if (row["subject"] != DBNull.Value)
                    {
                        msgInfo.Subject = row["subject"].ToString();
                    }
                    if (row["vendorName"] != DBNull.Value)
                    {
                        msgInfo.VendorName = row["vendorName"].ToString();
                    }
                    if (row["priority"] != DBNull.Value)
                    {
                        msgInfo.Priority = (System.Messaging.MessagePriority)row["priority"];
                    }
                    messageList.Add(msgInfo);
                }
            }

            return messageList;
        }


        public List<MessageInfo> GetNeedRepeatSendEmail(EmailStatus [] status)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string strStatus = string.Empty;
            foreach (EmailStatus enumStatus in status)
            {
                strStatus += string.Format(" OR [status]={0} ", (int)enumStatus);
            }
            string sql = @"SELECT id,[address],name,interestedseries,templateid,taskId,batchid,
                                dataSource,emailfrom,reply,[subject],vendorName,[priority],requestId
                        FROM EmailSendInfo WHERE isnull(repeatSentTimes,0)<=10 and ([status] is null " + strStatus + ") and createdDt <= dateadd(minute,-5,getdate())";

            DataSet ds = dbHelper.ExcuteQuery(sql, null, CommandType.Text);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MessageInfo msgInfo = new MessageInfo();
                    if (row["id"] != DBNull.Value)
                    {
                        msgInfo.Id = new Guid(row["id"].ToString());
                    }
                    if (row["address"] != DBNull.Value)
                    {
                        msgInfo.Number = row["address"].ToString();
                    }
                    if (row["name"] != DBNull.Value)
                    {
                        msgInfo.EmailName = row["name"].ToString();
                    }
                    if (row["interestedseries"] != DBNull.Value)
                    {
                        msgInfo.InterestedSeries = row["interestedseries"].ToString();
                    }
                    if (row["templateid"] != DBNull.Value)
                    {
                        msgInfo.TemplateId = row["templateid"].ToString();
                    }
                    if (row["taskId"] != DBNull.Value)
                    {
                        msgInfo.TaskId = row["taskId"].ToString();
                    }
                    if (row["batchid"] != DBNull.Value)
                    {
                        msgInfo.BatchId = row["batchid"].ToString();
                    }
                    if (row["dataSource"] != DBNull.Value)
                    {
                        msgInfo.DataSource = row["dataSource"].ToString();
                    }
                    if (row["emailfrom"] != DBNull.Value)
                    {
                        msgInfo.From = row["emailfrom"].ToString();
                    }
                    if (row["reply"] != DBNull.Value)
                    {
                        msgInfo.Reply = row["reply"].ToString();
                    }
                    if (row["subject"] != DBNull.Value)
                    {
                        msgInfo.Subject = row["subject"].ToString();
                    }
                    if (row["vendorName"] != DBNull.Value)
                    {
                        msgInfo.VendorName = row["vendorName"].ToString();
                    }
                    if (row["priority"] != DBNull.Value)
                    {
                        msgInfo.Priority = (System.Messaging.MessagePriority)row["priority"];
                    }
                    if (row["requestId"] != DBNull.Value)
                    {
                        msgInfo.RequestId = new Guid(row["requestId"].ToString());
                    }

                    messageList.Add(msgInfo);
                }
            }

            return messageList;
        }

        /// <summary>
        /// update email message repeat sent times
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public bool UpdateEmailRepeatSentTimes(Guid msgId)
        {
            try
            {
                string sql = "UPDATE EmailSendInfo set repeatSentTimes=ISNull(repeatSentTimes,0)+1 where Id=@id";
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@id", msgId));
                int cou = dbHelper.Execute(sql, paramList.ToArray(), CommandType.Text);
                if (cou > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("update email repeat sent times error:" + ex.Message + "," + ex.StackTrace);
            }
        }


        /// <summary>
        /// get mms info by page
        /// </summary>
        /// <param name="dicConditions"></param>
        /// <param name="pageSize">one page show message count</param>
        /// <param name="pageNumber">the page index,start from 1</param>
        /// <param name="count">return the sum of message count</param>
        /// <returns></returns>
        public DataTable GetEmailInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            List<SqlParameter> paramList = new List<SqlParameter>();
            foreach (string column in dicConditions.Keys)
            {
                SqlParameter param = new SqlParameter("@" + column, dicConditions[column]);
                paramList.Add(param);
            }
            paramList.Add(new SqlParameter("@pageSize", pageSize));
            paramList.Add(new SqlParameter("@pageNumber", pageNumber));
            SqlParameter outPutParam = new SqlParameter("@count", SqlDbType.Int);
            outPutParam.Direction = ParameterDirection.Output;
            paramList.Add(outPutParam);

            DataSet ds = dbHelper.ExcuteQuery("sp_page_GetEmailInfo", paramList.ToArray(), CommandType.StoredProcedure);
            count = Convert.ToInt32(outPutParam.Value);

            return ds.Tables[0];
        }
    }

    public interface IEmailDal
    {
        bool RecordEmailSendLog(RequestLog request);
        List<MessageInfo> GetEmailMessageByTaskId(string taskId);
        bool RecordEmailSendingInfo(Dictionary<string, object> dicMsgInfo);
        bool UpdateEmialMessage(Dictionary<string,object> dicConditions,Dictionary<string,object> dicUpdateField);
        bool UpdateEmailSendingStatus(MessageInfo msgInfo, bool isRepeatSend);
        bool RecordEmailSubmitResultLog(RequestResultInfo requestResult, Guid messageId, string vendorName);
        List<MessageInfo> GetBatchEmailList();
        List<MessageInfo> GetNeedRepeatSendEmail(EmailStatus[] status);
        bool UpdateEmailRepeatSentTimes(Guid msgId);
        DataTable GetEmailInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count);
    }


    public static class EmailDalFactory
    {
        public static IEmailDal GetInstance()
        {
            string mock = System.Configuration.ConfigurationManager.AppSettings["isMock"];
            if (mock == "1")
            {
                return new EmailDalMock();
            }
            else {
                return new EmailDal();
            }
        }
    }
}
