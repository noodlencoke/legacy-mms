using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.Control;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using IBM.WMQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daimler.HELM.TSBAdapter
{
    partial  class MessageTSBService : ServiceBase
    {
        private  readonly string mqHost = "";
        private  readonly string channel = "";
        private  readonly int mqPort = 0;
        private  readonly string qmName = "";
        private readonly string qName = "";
        private readonly string qReplyName = "";
        private readonly string userId = "";
        private readonly string passWord = "";
        public MessageTSBService()
        {
            InitializeComponent();
            mqHost = ConfigurationManager.AppSettings["MqHost"].ToString();
            channel = ConfigurationManager.AppSettings["Channel"];
            mqPort = Int32.Parse(ConfigurationManager.AppSettings["MqPort"]);
            qmName = ConfigurationManager.AppSettings["QmName"];
            qName = ConfigurationManager.AppSettings["QName"];
            qReplyName = ConfigurationManager.AppSettings["QReplyName"];
            userId = ConfigurationManager.AppSettings["UserId"];
            passWord = ConfigurationManager.AppSettings["PassWord"];
        }
        private SMSTSBQueue smstsbQueue;
        Thread serviceThread = null;
        protected override void OnStart(string[] args)
        {
            serviceThread = new Thread(SMSReceive);
            serviceThread.Start();
        }

        protected override void OnStop()
        {
            if (smstsbQueue != null)
                smstsbQueue.IsStop = true;
        }
    
        private  void SMSReceive()
        {
            try
            {
                smstsbQueue = new SMSTSBQueue(mqHost, channel, mqPort, qmName, qName,qReplyName,userId,passWord);
                smstsbQueue.ListenQ();
            }
            catch (Exception ex)
            {
                Log.LogHandler.WriteLog( ex.Message.ToString());
                ExceptionLog log = new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "tsb sms send",
                    ExceptionInfo = ex
                };
                CommonHandler.RecordExceptionLog(log);
            }
        }

    }
}
