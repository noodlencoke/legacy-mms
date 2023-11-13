using IBM.WMQ;
using IBM.WMQ.Nmqi;
using IBM.WMQ.PCF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.MQ
{
    public class BaseMessageIBMMQ
    {
        private readonly WaitCallback callBack = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mqHost">Host Ip</param>
        /// <param name="channel">Channel</param>
        /// <param name="mqPort">Port</param>
        /// <param name="qmName">MQ queue manager</param>
        /// <param name="qOutName">MQ queue</param>
        public BaseMessageIBMMQ(string mqHost, string channel, int mqPort, string qmName, string qName,string userId,string passWord)
        {
            if (string.IsNullOrEmpty(mqHost)||string.IsNullOrEmpty(channel)||string.IsNullOrEmpty(qmName)||string.IsNullOrEmpty(qName))
                throw new Exception("IBMMQ config is Error!");
            this.MQHost = mqHost;
            this.Channel = channel;
            this.MQPort = mqPort;
            this.QMName = qmName;
            this.QName = qName;
            this.ReplyQueueDic = new Dictionary<string, MQQueue>();
            this.UserId = userId;
            this.PassWord = passWord;
            callBack = new WaitCallback(ThreadSendMessage);
            this.CreateQManager();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mqHost">Host Ip</param>
        /// <param name="channel">Channel</param>
        /// <param name="mqPort">Port</param>
        /// <param name="qmName">MQ queue manager</param>
        /// <param name="qOutName">MQ queue</param>
        /// <param name="qOutName">reply queue</param>
        public BaseMessageIBMMQ(string mqHost, string channel, int mqPort, string qmName, string qGetName,string qPutName,string userId,string passWord)
        {
            if (string.IsNullOrEmpty(mqHost) || string.IsNullOrEmpty(channel) || string.IsNullOrEmpty(qmName) || string.IsNullOrEmpty(qGetName) || string.IsNullOrEmpty(qPutName))
                throw new Exception("IBMMQ config is Error!");
            this.MQHost = mqHost;
            this.Channel = channel;
            this.MQPort = mqPort;
            this.QMName = qmName;
            this.QName = qGetName;
            this.ReplyQueueDic = new Dictionary<string, MQQueue>();
            this.ReplyQName = qPutName;
            this.UserId = userId;
            this.PassWord = passWord;
            callBack = new WaitCallback(ThreadSendMessage);
            this.CreateQManager();
        }
        private MQQueueManager CreateQManager()
        {
            try
            {
                if (MqQMgr == null && !string.IsNullOrEmpty(this.UserId))
                {
                    Hashtable ht = new Hashtable();
                    ht.Add(MQC.CHANNEL_PROPERTY, this.Channel);
                    ht.Add(MQC.PORT_PROPERTY, this.MQPort);
                    ht.Add(MQC.HOST_NAME_PROPERTY, this.MQHost);
                    ht.Add(MQC.USER_ID_PROPERTY, this.UserId);
                    ht.Add(MQC.PASSWORD_PROPERTY, this.PassWord);
                    MqQMgr = new MQQueueManager(this.QMName, ht);
                }
                else
                    if (MqQMgr == null && string.IsNullOrEmpty(this.UserId))
                    MqQMgr = new MQQueueManager(this.QMName, this.Channel, this.MQHost + "(" + this.MQPort + ")");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MqQMgr;

        }
        /// <summary>
        /// create queue
        /// </summary>
        /// <param name="queueType">0 read 1 write 2 read and write</param>
        /// <returns></returns>
        private MQQueue CreateQueue(int queueType)
        {
            int openOptions =0;
            try
            {
               switch(queueType)
                {
                    case 0:
                        openOptions= MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING;
                        break;
                    case 1:
                          openOptions= MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING;;
                        break;
                    case 2:
                        openOptions=MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING| MQC.MQOO_OUTPUT;
                        break;
                   default:
                        break;
                }
                if(MqQueue==null || !MqQueue.OpenStatus)
                 MqQueue = MqQMgr.AccessQueue(this.QName, openOptions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MqQueue;
        }

        private MQQueue CreateReplyQueue(string replyQName)
        {
            MQQueue replyQueue = null;
            try
            {
                int openOptions = MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING; ;
                if ( this.ReplyQueueDic.ContainsKey(replyQName))
                {
                    replyQueue = ReplyQueueDic[replyQName];
                    if (!replyQueue.OpenStatus)
                    {
                        replyQueue = MqQMgr.AccessQueue(replyQName, openOptions);
                    }
                }
                else
                {
                    if (replyQueue == null || (replyQueue!=null &&!replyQueue.OpenStatus))
                    {
                        replyQueue = MqQMgr.AccessQueue(replyQName, openOptions);
                    }
                    if (!this.ReplyQueueDic.ContainsKey(replyQName))
                    this.ReplyQueueDic.Add(replyQName, replyQueue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return replyQueue;
        }

        private MQQueue CreateReplyQueue()
        {
            try
            {
                int openOptions = MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING; ;
                if (this.MQReplyQueue == null || !this.MQReplyQueue.OpenStatus)
                    MQReplyQueue = MqQMgr.AccessQueue(ReplyQName, openOptions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MQReplyQueue;
        }

        public virtual  void PutMessage(string  messageInfo)
        {
            try
            {
                this.CreateQueue(1);
                MQMessage message = new MQMessage();
                message.WriteString(messageInfo);
                this.MqQueue.Put(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public virtual void ReplyMessage(MQMessage message,string replyQName)
        {
            try
            {
                this.CreateReplyQueue(replyQName);
                this.ReplyQueueDic[replyQName].Put(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual void ReplyMessage(MQMessage message)
        {
            try
            {
                this.CreateReplyQueue();
                this.MQReplyQueue.Put(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private  MQMessage GetMessage()
        {          
            try
            {
                MQMessage message = new MQMessage();
                MQGetMessageOptions gmo = new MQGetMessageOptions();
                gmo.Options = MQC.MQGMO_FAIL_IF_QUIESCING | MQC.MQGMO_WAIT;
                gmo.WaitInterval = MQC.MQWI_UNLIMITED;
                MqQueue.Get(message, gmo);
                return message;
            }
            catch (MQException ex)
            {
                throw ex;
            }

        }
        private  MQMessage GetMessage(MQGetMessageOptions mqGetOptions)
        {
            try
            {
                MQMessage message = new MQMessage();
                MqQueue.Get(message, mqGetOptions);
                return message;
            }
            catch (MQException ex)
            {
                if (ex.Reason == MQC.MQRC_NO_MSG_AVAILABLE)
                {
                    return null;
                }
                throw ex;
            }
        }
        /// <summary>
        /// listen mq port
        /// </summary>
        public void ListenQ()
        {
            this.IsStop = false;
            this.CreateQueue(0);
            MQGetMessageOptions gmo = new MQGetMessageOptions();
            gmo.Options = MQC.MQGMO_FAIL_IF_QUIESCING | MQC.MQGMO_WAIT;
            gmo.WaitInterval = 5000;
            do
            {
                MQMessage message = this.GetMessage(gmo);
                if (message != null)
                {
                    this.SendMessage(message);
                }
            }
            while (!this.IsStop);
            this.Release();
        }
        public virtual void SendMessage(MQMessage message)
        {
            ThreadPool.QueueUserWorkItem(callBack, message);
        }
        public virtual void ThreadSendMessage(object message)
        { 
        
        }
        public virtual void Release()
        {
            if (ReplyQueueDic != null)
            {
                foreach (var item in ReplyQueueDic)
                {
                    if (item.Value != null && item.Value.OpenStatus)
                    {
                        item.Value.Close();
                    }
                }
            }
            if(MQReplyQueue!=null && MQReplyQueue.OpenStatus)
            {
                MQReplyQueue.Close();
            }
            if (MqQueue != null && MqQueue.OpenStatus)
            {
                MqQueue.Close();
            }
            if (MqQMgr != null && MqQMgr.OpenStatus)
            {
                MqQMgr.Disconnect();
            }
        }
        /// <summary>
        /// MQ Host
        /// </summary>
        public string MQHost { get; set; }
        /// <summary>
        /// MQ Port
        /// </summary>
        public int MQPort { get; set; }
        /// <summary>
        /// MQ Manager Name
        /// </summary>
        public string QMName { get; set; }
        /// <summary>
        /// MQ Queue Name
        /// </summary>
        public string QName { get; set; }
        /// <summary>
        /// Reply Queue Name
        /// </summary>
        public string ReplyQName { get; set; }
        /// <summary>
        /// MQ Channel
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// MQ Manager
        /// </summary>
        public  MQQueueManager MqQMgr {get;set;}
        /// <summary>
        /// MQ Queue
        /// </summary>
        public MQQueue MqQueue { get; set; }
        /// <summary>
        /// Reply Queue
        /// </summary>
        public MQQueue MQReplyQueue { get; set; }

        public Dictionary<string, MQQueue> ReplyQueueDic { get; set; }
        /// <summary>
        /// IS Stop
        /// </summary>
        public bool IsStop { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string PassWord { get; set; }


    }
}
