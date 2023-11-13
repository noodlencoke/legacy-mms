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
    public class MMSDal:IMMSDal
    {
        private readonly DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// record the send mms request log
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool RecordMMSSendLog(RequestLog request)
        {
            string sql = @"INSERT INTO HELMMessageDB..MMSSendLog(id,reqeustInfo,sendContent,dataSource,createdDt)
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

        /// <summary>
        /// get mms message by task id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public List<MessageInfo> GetMMSMessageByTaskId(string taskId)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string sql = "SELECT id,mobile,taskId,vendorName FROM MMSSendInfo WITH(NOLOCK) where taskId=@taskId order by [priority] desc";

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
                    if (row["mobile"] != DBNull.Value)
                    {
                        msgInfo.Number = row["mobile"].ToString();
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

        /// <summary>
        /// record the sending message status
        /// </summary>
        /// <param name="msgInfo"></param>
        /// <returns></returns>
        public bool RecordMMSSendingInfo(Dictionary<string, object> dicMsgInfo)
        {
            return dbHelper.InsertTable("MMSSendInfo", dicMsgInfo);
        }

        /// <summary>
        ///  update the sending 
        /// </summary>
        /// <param name="dicConditions"></param>
        /// <param name="dicUpdateFields"></param>
        /// <returns></returns>
        public bool UpdateMMSSendInfo(Dictionary<string, object> dicConditions, Dictionary<string, object> dicUpdateFields)
        {
            return dbHelper.UpdateTable("MMSSendInfo",dicConditions,dicUpdateFields);
        }

        public bool UpdateMMSSendingStatus(MessageInfo msgInfo, bool isRepeatSend)
        {
            try
            {
                string repeatSetValue = string.Empty;
                if (isRepeatSend)
                {
                    repeatSetValue = ",repeatSentTimes=isnull(repeatSentTimes,0)+1 ";
                }
                string sql = @"UPDATE MMSSendInfo 
                            SET [status]=@status
                               ,vendorName=@vendorName
                               ,submitDt=@submitDt
                               ,subDesc=@subDesc
                               ,vendorTaskId=@vendorTaskId
                               ,extendNumber=@extendNumber
                               ,senderNumber=@senderNumber
                               " + repeatSetValue + " WHERE id=@id";

                object venderName = msgInfo.VendorName;
                object submitDt = msgInfo.SubmitDt;
                object subDesc = msgInfo.SubDesc;
                object vendorTaskId = msgInfo.VendorTaskId;
                object extendNumber = msgInfo.ExtendNumber;
                object senderNumber = msgInfo.SenderNumber;

                venderName = venderName == null ? DBNull.Value : venderName;
                submitDt = submitDt == null ? DBNull.Value : submitDt;
                subDesc = subDesc == null ? DBNull.Value : subDesc;
                vendorTaskId = vendorTaskId == null ? DBNull.Value : vendorTaskId;
                extendNumber = extendNumber == null ? DBNull.Value : extendNumber;
                senderNumber = senderNumber == null ? DBNull.Value : senderNumber;

                SqlParameter[] paramArray = new SqlParameter[] { 
                    new SqlParameter("@status",(int)msgInfo.Status),
                    new SqlParameter("@vendorName",venderName),
                    new SqlParameter("@submitDt",submitDt),
                    new SqlParameter("@subDesc",subDesc),
                    new SqlParameter("@vendorTaskId",vendorTaskId),
                    new SqlParameter("@extendNumber",extendNumber),
                    new SqlParameter("@senderNumber",senderNumber),
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



        public List<MessageInfo> GetBatchMMSList()
        {

            List<MessageInfo> messageList = new List<MessageInfo>();

            string sql = "SELECT top 5000 id,mobile,[priority],templateId,taskId,datasource,vendorName,sessionId,requestId FROM MMSSendInfo WITH(NOLOCK) where [status] is null and classification = 2 order by [priority] desc,createdDt asc";

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
                    if (row["mobile"] != DBNull.Value)
                    {
                        msgInfo.Number = row["mobile"].ToString();
                    }
                    if (row["taskId"] != DBNull.Value)
                    {
                        msgInfo.TaskId = row["taskId"].ToString();
                    }
                    if (row["templateId"] != DBNull.Value)
                    {
                        msgInfo.TemplateId = row["templateId"].ToString();
                        msgInfo.Content = row["templateId"].ToString();
                    }
                    if (row["vendorName"] != DBNull.Value)
                    {
                        msgInfo.VendorName = row["vendorName"].ToString();
                    }
                    if (row["sessionId"] != DBNull.Value)
                    {
                        msgInfo.SessionId = row["sessionId"].ToString();
                    }
                    if (row["requestId"] != DBNull.Value)
                    {
                        msgInfo.RequestId = new Guid(row["requestId"].ToString());
                    }

                    if (row["dataSource"] != DBNull.Value)
                    {
                        msgInfo.DataSource = row["dataSource"].ToString();
                    }

                    if (row["priority"] != DBNull.Value)
                    {
                        System.Messaging.MessagePriority priority;
                        if (Enum.TryParse(row["priority"].ToString(), out priority))
                        {
                            msgInfo.Priority = priority;
                        }
                    }

                    messageList.Add(msgInfo);
                }
            }
            return messageList;

        }



        public List<MessageInfo> GetMMSSendingMessage(MessageClassification classicfiation)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string sql = @"SELECT top 1000 a.id,c.accountName,c.accountPwd,a.dataSource,a.mobile,VendortaskId,a.vendorName,templateId,a.[priority]  
			FROM MMSSendInfo a
			INNER JOIN VendorSignConfig b on a.extendNumber = b.signNumber
            INNER JOIN VendorAccount c ON c.id = b.venderId
			where a.[status]=1 and a.classification=@classification and a.vendorName is not null";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@classification", (int)classicfiation));

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
                    if (row["mobile"] != DBNull.Value)
                    {
                        msgInfo.Number = row["mobile"].ToString();
                    }
                    if (row["VendortaskId"] != DBNull.Value)
                    {
                        msgInfo.VendorTaskId = row["VendortaskId"].ToString();
                    }
                    if (row["vendorName"] != DBNull.Value)
                    {
                        msgInfo.VendorName = row["vendorName"].ToString();
                    }
                    if (row["templateId"] != DBNull.Value)
                    {
                        msgInfo.TemplateId = row["templateId"].ToString();
                    }

                    if (row["accountName"] != DBNull.Value)
                    {
                        msgInfo.AccountName = row["accountName"].ToString();
                    }

                    if (row["accountPwd"] != DBNull.Value)
                    {
                        msgInfo.AccountPwd = row["accountPwd"].ToString();
                    }

                    messageList.Add(msgInfo);
                }
            }

            return messageList;
        }


        /// <summary>
        /// get message by message status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<MessageInfo> GetNeedRepeatSendMMS(MessageStatus[] status)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string strStatus = string.Empty;
            foreach (MessageStatus enumStatus in status)
            {
                strStatus += string.Format(" OR [status]={0} ", (int)enumStatus);
            }
            string sql = @"SELECT top 1000 id,dataSource,[priority],mobile,taskId,[status],[templateId],vendorName,sessionId,requestId 
                            FROM MMSSendInfo 
                            WHERE isnull(repeatSentTimes,0)<=10 and ([status] is null " + strStatus + @") and createdDt <= dateadd(minute,-5,getdate())";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@status", strStatus));

            DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), CommandType.Text);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MessageInfo msgInfo = new MessageInfo();
                    if (row["id"] != DBNull.Value)
                    {
                        msgInfo.Id = new Guid(row["id"].ToString());
                    }
                    if (row["mobile"] != DBNull.Value)
                    {
                        msgInfo.Number = row["mobile"].ToString();
                    }
                    if (row["taskId"] != DBNull.Value)
                    {
                        msgInfo.TaskId = row["taskId"].ToString();
                    }
                    if (row["vendorName"] != DBNull.Value)
                    {
                        msgInfo.VendorName = row["vendorName"].ToString();
                    }
                    if (row["templateId"] != DBNull.Value)
                    {
                        msgInfo.TemplateId = row["templateId"].ToString();
                        msgInfo.Content = row["templateId"].ToString();
                    }
                    if (row["sessionId"] != DBNull.Value)
                    {
                        msgInfo.SessionId = row["sessionId"].ToString();
                    }

                    if (row["dataSource"] != DBNull.Value)
                    {
                        msgInfo.DataSource = row["dataSource"].ToString();
                    }

                    if (row["priority"] != DBNull.Value)
                    {
                        System.Messaging.MessagePriority priority;
                        if (Enum.TryParse(row["priority"].ToString(), out priority))
                        {
                            msgInfo.Priority = priority;
                        }
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
        /// update mms message repeat sent times
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public bool UpdateMMSRepeatSentTimes(Guid msgId)
        {
            try
            {
                string sql = "UPDATE MMSSendInfo set repeatSentTimes=ISNull(repeatSentTimes,0)+1 where Id=@id";
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@id", msgId));
                int cou = dbHelper.Execute(sql, paramList.ToArray(), CommandType.Text);
                if (cou > 0)
                {
                    return true;
                }
                return false;
            }catch(Exception ex)
            {
                throw new Exception("update mms repeat sent times error:"+ex.Message+","+ex.StackTrace);
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
        public DataTable GetMMSInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber,out int count)
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
            SqlParameter outPutParam = new SqlParameter("@count",SqlDbType.Int);
            outPutParam.Direction = ParameterDirection.Output;
            paramList.Add(outPutParam);

            DataSet ds = dbHelper.ExcuteQuery("sp_page_GetMMSInfo", paramList.ToArray(), CommandType.StoredProcedure);
            count = Convert.ToInt32(outPutParam.Value);

            return ds.Tables[0];
        }              
    }

    public interface IMMSDal
    {
        bool RecordMMSSendLog(RequestLog request);
        List<MessageInfo> GetMMSMessageByTaskId(string taskId);
        bool RecordMMSSendingInfo(Dictionary<string, object> dicMsgInfo);
        bool UpdateMMSSendInfo(Dictionary<string, object> dicConditions, Dictionary<string, object> dicUpdateFields);
        bool UpdateMMSSendingStatus(MessageInfo msgInfo, bool isRepeatSend);
        List<MessageInfo> GetBatchMMSList();
        List<MessageInfo> GetMMSSendingMessage(MessageClassification classicfiation);
        List<MessageInfo> GetNeedRepeatSendMMS(MessageStatus[] status);
        bool UpdateMMSRepeatSentTimes(Guid msgId);
        DataTable GetMMSInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count);
    }

    public static class MMSDalFactory
    {
        public static IMMSDal GetInstance()
        {
            string mock = System.Configuration.ConfigurationManager.AppSettings["isMock"];
            if (mock == "1")
            {
                return new MMSDalMock();
            }
            else
            {
                return new MMSDal();
            }
        }
    }
}
