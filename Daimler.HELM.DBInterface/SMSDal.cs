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
    public class SMSDal : ISMSDal
    {
        private readonly DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// record the send sms request log
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool RecordSMSSendLog(RequestLog request)
        {
            string sql = @"INSERT INTO HELMMessageDB..SMSSendLog(id,reqeustInfo,sendContent,dataSource,createdDt)
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
        /// record the sending message status
        /// </summary>
        /// <param name="msgInfo"></param>
        /// <returns></returns>
        public bool RecordSMSSendingInfo(Dictionary<string, object> dicMsgInfo)
        {
            return dbHelper.InsertTable("SMSSendInfo", dicMsgInfo);
        }

       

        /// <summary>
        /// update sending message status
        /// </summary>
        /// <param name="msgInfo"></param>
        /// <param name="messageStatus"></param>
        /// <returns></returns>
        public bool UpdateSMSSendingStatus(Dictionary<string,object> dicConditions, Dictionary<string, object> dicUpdateList)
        {
            return dbHelper.UpdateTable("SMSSendInfo", dicConditions, dicUpdateList);
        }

        public bool UpdateSMSSendingStatus(MessageInfo msgInfo,bool isRepeatSend)
        {
            try
            {
                string repeatSetValue = string.Empty;
                if (isRepeatSend)
                {
                    repeatSetValue = ",repeatSentTimes=isnull(repeatSentTimes,0)+1 ";
                }
                string sql = @"UPDATE SMSSendInfo 
                            SET [status]=@status
                               ,vendorName=@vendorName
                               ,submitDt=@submitDt
                               ,subDesc=@subDesc
                               ,vendorTaskId=@vendorTaskId
                               ,extendNumber=@extendNumber
                               ,senderNumber=@senderNumber
                               " + repeatSetValue + " WHERE id=@id";

                 object venderName =msgInfo.VendorName;  
                 object submitDt =  msgInfo.SubmitDt;
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
                throw new Exception("UpdateSMSSendingStatus error"+","+ex.Message+","+ex.StackTrace);
            }
        }


        /// <summary>
        /// get the sending message
        /// </summary>
        /// <param name="classicfiation"></param>
        /// <returns></returns>
        public List<MessageInfo> GetSMSSendingMessage(MessageClassification classicfiation)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string sql = @"SELECT top 1000 a.id,c.accountName,c.accountPwd,a.dataSource,a.mobile,a.VendortaskId,a.vendorName,a.content,a.[priority] 
            FROM SMSSendInfo a
            INNER JOIN VendorSignConfig b on a.extendNumber = b.signNumber
            INNER JOIN VendorAccount c ON c.id = b.venderId
            WHERE a.[status]=1 and a.classification=@classification and a.vendorName is not null";

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
                    if (row["content"] != DBNull.Value)
                    {
                        msgInfo.Content = row["content"].ToString();
                    }

                    if (row["priority"] != DBNull.Value)
                    {
                        System.Messaging.MessagePriority priority;
                        if (Enum.TryParse(row["priority"].ToString(), out priority))
                        {
                            msgInfo.Priority = priority; 
                        }
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
        /// record the message sending status
        /// </summary>
        /// <param name="statusLog"></param>
        /// <returns></returns>
        public bool RecordSMSSendingStatus(SendingResult sendingResult, Guid messageId, string vendorName)
        {
            string sql = @"INSERT INTO SMSStatusLog(messageId,vendorName,content,createdDt)
                           VALUES(@messageId,@vendorName,@content,@createdDt)";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@messageId", messageId));
            paramList.Add(new SqlParameter("@vendorName", vendorName));
            paramList.Add(new SqlParameter("@content", sendingResult.StrSendingResult));
            paramList.Add(new SqlParameter("@createdDt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")));

            int cou = dbHelper.Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// record the message sending status
        /// </summary>
        /// <param name="statusLog"></param>
        /// <returns></returns>
        public bool RecordSMSSubmitResult(RequestResultInfo requestResult, Guid messageId, string vendorName)
        {
            string sql = @"INSERT INTO SMSSubmitResultLog(messageId,vendorName,content,createdDt)
                           VALUES(@messageId,@vendorName,@content,@createdDt)";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@messageId", messageId));
            paramList.Add(new SqlParameter("@vendorName", vendorName));
            paramList.Add(new SqlParameter("@content", requestResult.StrResult));
            paramList.Add(new SqlParameter("@createdDt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")));

            int cou = dbHelper.Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// get SMS、MMS receive status
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public IList<MessageStateInfo> GetMessageStateInfos(string dataSource)
        {
            IList<MessageStateInfo> messageInfos = new List<MessageStateInfo>();
            string sql = @"select top 1000 a.id, a.taskId,isnull(b.sourceStatus,5) as status ,isnull(a.sentDt,getdate()) as sentDt,a.messageType,isnull(b.StatusDesc,'Commit failed') as StatusDesc from 
                            (
                            select id,ProductId,taskId,status,sentDt,dataSource,'SMS' as messageType from SMSSendInfo a WITH(NOLOCK)  where a.dataSource=@dataSource and a.isRead is  null and a.status is not  null and a.status<>-1 and a.status<>1
                            Union ALL
                            select id,ProductId,taskId,status,sentDt,dataSource,'MMS' as messageType from MMSSendInfo a WITH(NOLOCK) where a.dataSource=@dataSource and a.isRead is  null and a.status is not  null and a.status<>-1 and a.status<>1
                            ) a 
                            Left Join 
                             StatusMapping b WITH(NOLOCK) on a.status=b.helmStatus and a.dataSource=b.dataSource
                            order by a.sentDt asc;";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@dataSource", dataSource));
            DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), CommandType.Text);
                if(ds!=null && ds.Tables.Count>0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        MessageStateInfo info = new MessageStateInfo();
                        if (row["id"] != DBNull.Value)
                        {
                            info.id = row["id"].ToString();
                        }
                        if (row["taskId"] != DBNull.Value)
                        {
                            info.taskId = row["taskId"].ToString();
                        }
                        if (row["sentDt"] != DBNull.Value)
                        {
                            info.returnTime = row["sentDt"].ToString();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            info.status = row["status"].ToString();
                        }
                        if (row["StatusDesc"] != DBNull.Value)
                        {
                            info.statusDesc = row["StatusDesc"].ToString();
                        }
                        if (row["messageType"] != DBNull.Value)
                        {
                            info.messageType = row["messageType"].ToString();
                        }
                        messageInfos.Add(info);
                    }
                }
            return messageInfos;
        }
        /// <summary>
        /// update sms、mms receive status 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool UpdateMessageReceiveStates(IList<MessageStateInfo> list)
        {
            var smslist = from c in list where c.messageType == "SMS" select c.id;
            var mmslist = from c in list where c.messageType == "MMS" select c.id;
            bool result = false;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string smsSql = "update SMSSendInfo set isread=1 where id =@Id ";
            string mmsSql = "update MMSSendInfo set isread=1 where id =@Id";
            foreach (string s in smslist)
            {
                paramList.Clear();
                paramList.Add(new SqlParameter("@Id", s));
                int cou = dbHelper.Execute(smsSql, paramList.ToArray(), System.Data.CommandType.Text);
                if (cou > 0)
                {
                    result = true;
                }
            }
            foreach (string s in mmslist)
            {
                paramList.Clear();
                paramList.Add(new SqlParameter("@Id", s));
                int cou = dbHelper.Execute(mmsSql, paramList.ToArray(), System.Data.CommandType.Text);
                if (cou > 0)
                {
                    result = true;
                }
            }
            return result;
        }


        /// <summary>
        /// get message by task id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public List<MessageInfo> GetSMSMessageByTaskId(string taskId)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string sql = "SELECT id,mobile,taskId,vendorName FROM SMSSendInfo WITH(NOLOCK) where taskId=@taskId order by [priority] desc";

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
        /// get message by message status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<MessageInfo> GetNeedRepeatSendSMS(MessageStatus[] status)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string strStatus = string.Empty;
            foreach (MessageStatus enumStatus in status)
            {
                strStatus += string.Format(" OR [status]={0} ", (int)enumStatus);
            }
            string sql = @"SELECT top 1000 id,dataSource,sessionId,[priority],mobile,taskId,[status],[content],vendorName,requestId 
                            FROM SMSSendInfo 
                            WHERE isnull(repeatSentTimes,0)<=10 AND ([status] is null " + strStatus + @") and createdDt <= dateadd(minute,-5,getdate())";

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
                    if (row["content"] != DBNull.Value)
                    {
                        msgInfo.Content = row["content"].ToString();
                    }

                    if (row["dataSource"] != DBNull.Value)
                    {
                        msgInfo.DataSource = row["dataSource"].ToString();
                    }

                    if (row["sessionId"] != DBNull.Value)
                    {
                        msgInfo.SessionId = row["sessionId"].ToString();
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
        /// get SMS batch message list 
        /// </summary>
        /// <returns></returns>
        public List<MessageInfo> GetSMSBatchMessage()
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            string sql = "SELECT top 5000 id,sessionId,[priority],mobile,content,taskId,datasource,vendorName,requestId FROM SMSSendInfo where classification = 2 and [status] is null";

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
                    if (row["content"] != DBNull.Value)
                    {
                        msgInfo.Content = row["content"].ToString();
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

                    if (row["sessionId"] != DBNull.Value)
                    {
                        msgInfo.SessionId = row["sessionId"].ToString();
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


        /// <summary>
        /// get message status by task Id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public List<MessageStateInfo> GetMessageStateInfos(string dataSource, string[] taskIds)
        {
            List<MessageStateInfo> messageStatusList = new List<MessageStateInfo>();
            string sql = @"SELECT  a.taskId,a.[status],a.sentDt,b.statusDesc FROM SMSSendInfo a WITH(NOLOCK)
                            LEFT JOIN StatusMapping b WITH(NOLOCK) ON a.[status] = b.helmStatus AND b.dataSource =@dataSource
                           Where a.taskId in (@taskId)";
            if (taskIds != null & taskIds.Length > 0)
            {
                string strTaskId = "";
                foreach (string taskId in taskIds)
                {
                    strTaskId += string.Format(" {0},", taskId);
                }
                strTaskId = strTaskId.Substring(0, strTaskId.Length - 1);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@taskId", strTaskId));
                paramList.Add(new SqlParameter("@dataSource", dataSource));

                DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        MessageStateInfo messageStatus = new MessageStateInfo();
                        if (row["taskId"] != DBNull.Value)
                        {
                            messageStatus.taskId = row["taskId"].ToString();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            messageStatus.status = row["status"].ToString();
                        }
                        if (row["sentDt"] != DBNull.Value)
                        {
                            messageStatus.returnTime = row["sentDt"].ToString();
                        }
                        if (row["statusDesc"] != DBNull.Value)
                        {
                            messageStatus.statusDesc = row["statusDesc"].ToString();
                        }

                        messageStatusList.Add(messageStatus);
                    }
                }

            }
            return messageStatusList;
        }


        /// <summary>
        /// save reply info
        /// </summary>
        /// <param name="dtReplyInfo"></param>
        /// <returns></returns>
        public bool RecordSMSReplyInfo(DataTable dtReplyInfo)
        {
            return dbHelper.BatchInsertDt(dtReplyInfo);
        }

        


        /// <summary>
        /// get reply information that from sms
        /// </summary>
        /// <param name="replyInfo"></param>
        /// <returns></returns>
        public MessageInfo GetReplyInfoFrom(MessageReplyInfo replyInfo)
        {
            MessageInfo messageInfo = new MessageInfo();
            string sql = @"SELECT top 1 * FROM(
                            SELECT id,taskid,dataSource,sentDt,'SMS' msgType FROM SMSSendInfo WITH(NOLOCK) WHERE [status]=0 and mobile=@mobile and senderNumber=@senderNumber
                            Union all
                            SELECT id,taskid,dataSource,sentDt,'MMS' msgType FROM MMSSendInfo WITH(NOLOCK) WHERE [status]=0 and mobile=@mobile and senderNumber=@senderNumber
                            )A
                           ORDER BY sentDt desc";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@mobile", replyInfo.mobile));
            if (!string.IsNullOrEmpty(replyInfo.senderNumber))
            {
                paramList.Add(new SqlParameter("@senderNumber",replyInfo.senderNumber));
            }
            else {
                sql = sql.Replace("=@senderNumber", " is null");
            }
            DataSet ds = dbHelper.ExcuteQuery(sql,paramList.ToArray(),CommandType.Text);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                if (row["Id"] != DBNull.Value)
                {
                    messageInfo.Id = new Guid(row["Id"].ToString());
                }
                if (row["taskId"] != DBNull.Value)
                {
                    messageInfo.TaskId = row["taskId"].ToString();
                }
                if (row["dataSource"] != DBNull.Value)
                {
                    messageInfo.DataSource = row["dataSource"].ToString();
                }
                if (row["msgType"] != DBNull.Value)
                {
                    MessageType type;
                    if (Enum.TryParse(row["msgType"].ToString(), out type))
                    {
                        messageInfo.Type = type;
                    }
                }

            }
            return messageInfo;

        }


        /// <summary>
        /// get reply message
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public List<MessageReplyInfo> GetRelyInfo(string dataSource, string startDateTime, string endDateTime)
        {
            List<MessageReplyInfo> messageReplyList = new List<MessageReplyInfo>();
            string sql = "SELECT * FROM SMSReplyInfo WITH(NOLOCK) where dataSource=@dataSource and replyDt between @startDt and @endDt";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@dataSource", dataSource));
            paramList.Add(new SqlParameter("@startDt", startDateTime));
            paramList.Add(new SqlParameter("@endDt", endDateTime));
            DataSet ds = dbHelper.ExcuteQuery(sql,paramList.ToArray(),CommandType.Text);
            if (ds != null && ds.Tables.Count > 0)
            {

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MessageReplyInfo msgReply = new MessageReplyInfo();
                    if (row["mobile"] != DBNull.Value)
                    {
                        msgReply.mobile = row["mobile"].ToString();
                    }
                    if (row["content"] != DBNull.Value)
                    {
                        msgReply.content = row["content"].ToString();
                    }
                    if (row["dataSource"] != DBNull.Value)
                    {
                        msgReply.dataSource = row["dataSource"].ToString();
                    }
                    if (row["taskId"] != DBNull.Value)
                    {
                        msgReply.taskId = row["taskId"].ToString();
                    }
                    if (row["extendNumber"] != DBNull.Value)
                    {
                        msgReply.extendNumber = row["extendNumber"].ToString();
                    }
                    if (row["replyDt"] != DBNull.Value)
                    {
                        msgReply.getTime = row["replyDt"].ToString();
                    }

                    messageReplyList.Add(msgReply);

                }
            }

            return messageReplyList;
        }


        /// <summary>
        /// get reply message by status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<MessageReplyInfo> GetNeedRepeatSendReplyInfo(int status)
        {
            List<MessageReplyInfo> messageReplyList = new List<MessageReplyInfo>();
            string sql = "SELECT * FROM SMSReplyInfo WITH(NOLOCK) where [status]=@status and isnull(repeatSentTimes,0)<=10";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@status", status));
            DataSet ds = dbHelper.ExcuteQuery(sql, paramList.ToArray(), CommandType.Text);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MessageReplyInfo msgReply = new MessageReplyInfo();
                    if (row["id"] != DBNull.Value)
                    {
                        msgReply.Id = new Guid(row["id"].ToString());
                    }
                    if (row["mobile"] != DBNull.Value)
                    {
                        msgReply.mobile = row["mobile"].ToString();
                    }
                    if (row["content"] != DBNull.Value)
                    {
                        msgReply.content = row["content"].ToString();
                    }
                    if (row["dataSource"] != DBNull.Value)
                    {
                        msgReply.dataSource = row["dataSource"].ToString();
                    }
                    if (row["taskId"] != DBNull.Value)
                    {
                        msgReply.taskId = row["taskId"].ToString();
                    }
                    if (row["extendNumber"] != DBNull.Value)
                    {
                        msgReply.extendNumber = row["extendNumber"].ToString();
                    }
                    if (row["replyDt"] != DBNull.Value)
                    {
                        msgReply.getTime = row["replyDt"].ToString();
                    }

                    messageReplyList.Add(msgReply);

                }
            }

            return messageReplyList; 
        }


        /// <summary>
        /// update reply message info
        /// </summary>
        /// <param name="dicConditions"></param>
        /// <param name="dicColumns"></param>
        /// <returns></returns>
        public bool UpdateReplyInfo(Dictionary<string, object> dicConditions, Dictionary<string, object> dicColumns)
        {
            return dbHelper.UpdateTable("SMSReplyInfo", dicConditions, dicColumns);
        }


        /// <summary>
        ///  update sms message repeat sent times
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public bool UpdateSMSRepeatSentTimes(Guid msgId)
        {
            try
            {
                string sql = "UPDATE SMSSendInfo set repeatSentTimes=ISNull(repeatSentTimes,0)+1 where Id=@id";
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@id", msgId));
                int cou = dbHelper.Execute(sql, paramList.ToArray(), CommandType.Text);
                if (cou > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("update repeat sent times error:"+ex.Message+","+ex.StackTrace);
            }
            return false;
        }

        /// <summary>
        ///  update sms message repeat sent times
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public bool UpdateSMSRepeatReplyTimes(Guid msgId)
        {
            try
            {
                string sql = "UPDATE SMSReplyInfo set repeatSentTimes=ISNull(repeatSentTimes,0)+1 where Id=@id";
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@id", msgId));
                int cou = dbHelper.Execute(sql, paramList.ToArray(), CommandType.Text);
                if (cou > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("update repeat sent times error:" + ex.Message + "," + ex.StackTrace);
            }
            return false;
        }

        /// <summary>
        /// get mms info by page
        /// </summary>
        /// <param name="dicConditions"></param>
        /// <param name="pageSize">one page show message count</param>
        /// <param name="pageNumber">the page index,start from 1</param>
        /// <param name="count">return the sum of message count</param>
        /// <returns></returns>
        public DataTable GetSMSInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();

            List<SqlParameter> paramList = new List<SqlParameter>();
            foreach (string column in dicConditions.Keys)
            {
                SqlParameter param = new SqlParameter("@" + column, dicConditions[column]);
                paramList.Add(param);
            }
            paramList.Add(new SqlParameter("@pageSize",pageSize));  
            paramList.Add(new SqlParameter("@pageNumber",pageNumber));
            SqlParameter outPutParam = new SqlParameter("@count", SqlDbType.Int);
            outPutParam.Direction = ParameterDirection.Output;
            paramList.Add(outPutParam);

            DataSet ds = dbHelper.ExcuteQuery("sp_page_GetSMSInfo", paramList.ToArray(), CommandType.StoredProcedure);
            count = Convert.ToInt32(outPutParam.Value);

            return ds.Tables[0];
        }
    }

    public interface ISMSDal
    {
        bool RecordSMSSendLog(RequestLog request);

        bool RecordSMSSendingInfo(Dictionary<string, object> dicMsgInfo);

        bool UpdateSMSSendingStatus(Dictionary<string, object> dicConditions, Dictionary<string, object> dicUpdateList);

        bool UpdateSMSSendingStatus(MessageInfo msgInfo, bool isRepeatSend);

        List<MessageInfo> GetSMSSendingMessage(MessageClassification classicfiation);

        bool RecordSMSSendingStatus(SendingResult sendingResult, Guid messageId, string vendorName);

        bool RecordSMSSubmitResult(RequestResultInfo requestResult, Guid messageId, string vendorName);

        IList<MessageStateInfo> GetMessageStateInfos(string dataSource);

        bool UpdateMessageReceiveStates(IList<MessageStateInfo> list);

        List<MessageInfo> GetSMSMessageByTaskId(string taskId);

        List<MessageInfo> GetNeedRepeatSendSMS(MessageStatus[] status);

        List<MessageInfo> GetSMSBatchMessage();

        List<MessageStateInfo> GetMessageStateInfos(string dataSource, string[] taskIds);

        bool RecordSMSReplyInfo(DataTable dtReplyInfo);

        MessageInfo GetReplyInfoFrom(MessageReplyInfo replyInfo);

        List<MessageReplyInfo> GetRelyInfo(string dataSource,string startDateTime,string endDateTime);

        List<MessageReplyInfo> GetNeedRepeatSendReplyInfo(int status);

        bool UpdateReplyInfo(Dictionary<string,object> dicConditions,Dictionary<string,object> dicColumns);

        bool UpdateSMSRepeatSentTimes(Guid msgId);

        DataTable GetSMSInfoByPage(Dictionary<string, object> dicConditions, int pageSize, int pageNumber, out int count);

        bool UpdateSMSRepeatReplyTimes(Guid msgId);

     
    }

    public static class SMSDalFactory
    {
        public static ISMSDal GetInstance()
        {
            string mock = System.Configuration.ConfigurationManager.AppSettings["isMock"];
            if (mock == "1")
            {
                return new SMSDalMock();
            }
            else
            {
                return new SMSDal();
            }
        }
    }
}
