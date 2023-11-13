using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.MQ
{
    public class BaseMessageQ<T>
    {
        #region 私有字段
        private string machineName;
        private QueueType queueType;
        private string queueName;

        private bool queueFlag = false;
        private string queuePath = "";
        private MessageQueue messageQueue = null;

        private readonly WaitCallback callBack = null;
        protected bool useThreadPool = false;
        protected int maxWorkerThreads = 3;
        protected int maxCompletionPortThreads = 3;
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="machine"></param>
        /// <param name="queueType"></param>
        /// <param name="queueName"></param>
        public BaseMessageQ(string machine, QueueType queueType, string queueName)
        {
            if (queueName == null || queueName == "")
            {
                throw new Exception("必须设置队列名称!");
            }

            this.MachineName = string.IsNullOrEmpty(machine) ? "." : machine;
            this.QueueType = queueType;
            this.QueueName = queueName;

            callBack = new WaitCallback(ThreadSendMessage);
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="queueName"></param>
        public BaseMessageQ(string machineName, string queueName)
            : this(machineName, QueueType.qtPrivate, queueName)
        {
        }


        #endregion

        #region 私有方法
        private void SetQueuePath()
        {
            switch (queueType)
            {
                case QueueType.qtPublic:
                    queuePath = this.machineName + "\\" + queueName;
                    break;
                case QueueType.qtJournal:
                    queuePath = this.machineName + "\\" + queueName + "\\Journal$";
                    break;
                default:
                    queuePath = string.Format("FormatName:Direct=TCP:{0}\\Private$\\{1}", this.machineName, queueName);
                    break;
            }
            queueFlag = false;
        }

        // 创建队例
        private void CheckCreateQueue()
        {
            if (!queueFlag)
            {
                if (messageQueue != null)
                {
                    messageQueue.Dispose();
                    messageQueue = null;
                }
                //if (!MessageQueue.Exists(queuePath))
                //{
                //    // 创建队列
                //    messageQueue = MessageQueue.Create(queuePath);
                //    messageQueue.Label = Path.GetFileName(queuePath);
                //    // 授权限
                //    messageQueue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
                //}
                //else
                //{
                    messageQueue = new MessageQueue(queuePath);
                //}
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });
                queueFlag = true;
            }
        }

        // 接收到消息
        private void ReceiveCompleted(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            try
            {
                MessageQueue mq = (MessageQueue)source;
                System.Messaging.Message msg = mq.EndReceive(asyncResult.AsyncResult);
                // 处理消息
                SendMessage((T)msg.Body);
                // 继续接收
                mq.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                ErrorDeal(ex.Message);
            }
        }
        #endregion

        #region 事件方法

        public void SendMessage(T msgObj)
        {
            if (useThreadPool)
            {
                ThreadPool.SetMaxThreads(maxWorkerThreads,maxCompletionPortThreads);
                ThreadPool.QueueUserWorkItem(callBack, msgObj);
            }
            else
            {
                ThreadSendMessage(msgObj);
            }
        }

        public virtual void ThreadSendMessage(object msgObj){
            
        }

        /// <summary>
        /// 处理错误方法
        /// </summary>
        /// <param name="errMsg"></param>
        protected void ErrorDeal(string errMsg)
        {
            Log.LogHandler.WriteLog(errMsg);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 开始处于接收状态
        /// </summary>
        public void Listen()
        {
            CheckCreateQueue();
            messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(ReceiveCompleted);
            messageQueue.BeginReceive();
        }

        /// <summary>
        /// 取消接收状态
        /// </summary>
        public void DisListen()
        {
            if (messageQueue != null)
            {
                messageQueue.ReceiveCompleted -= new ReceiveCompletedEventHandler(ReceiveCompleted);
            }
        }

        /// <summary>
        /// 发送消息到队列
        /// </summary>
        /// <param name="content"></param>
        public void ReceiveMessage(string content)
        {
            using (MessageQueue mq = new MessageQueue(queuePath))
            {
                using (System.Messaging.Message msg = new System.Messaging.Message())
                {
                    msg.Label = Guid.NewGuid().ToString();
                    msg.Body = content;
                    mq.Send(msg);
                }
            }
        }

        public void ReceiveMessage(T msgObj, MessagePriority priority)
        {

            using (MessageQueue mq = new MessageQueue(queuePath))
            {
                using (System.Messaging.Message msg = new System.Messaging.Message())
                {
                    msg.Label = Guid.NewGuid().ToString();
                    msg.Body = msgObj;
                    msg.Priority = priority;
                    mq.Send(msg);
                }
            }
        }

        #endregion


        #region 属性
        /// <summary>
        /// 服务器名
        /// </summary>
        public string MachineName
        {
            get
            {
                return machineName;
            }
            set
            {
                machineName = value;
                SetQueuePath();
            }
        }

        /// <summary>
        /// 队列类型
        /// </summary>
        public QueueType QueueType
        {
            get
            {
                return queueType;
            }
            set
            {
                queueType = value;
                SetQueuePath();
            }
        }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName
        {
            get
            {
                return queueName;
            }
            set
            {
                queueName = value;
                SetQueuePath();
            }
        }
        #endregion
    }

    #region 消息队列类型
    /// <summary>
    /// 消息队列类型
    /// </summary>
    public enum QueueType
    {
        /// <summary>
        /// 专用队列
        /// </summary>
        qtPrivate,

        /// <summary>
        /// 公共队列
        /// </summary>

        qtPublic,
        /// <summary>
        /// 日记队列
        /// </summary>
        qtJournal
    };
    #endregion

    #region 事件类型
    /// <summary>
    /// 接收消息事件类
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="content"></param>
    public delegate void EventReceivedHandler(object sender, object msgObj);

    /// <summary>
    /// 错误处理事件类
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="errMsg"></param>
    public delegate void EventErrorHandler(object sender, string errMsg);
    #endregion
}
