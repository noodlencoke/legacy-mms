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

namespace Daimler.HELM.POSAdapter
{
    partial  class MessageMQService : ServiceBase
    {
        private  readonly string mqHost = "";
        private  readonly string channel = "";
        private  readonly int mqPort = 0;
        private  readonly string qmName = "";
        private readonly string qName = "";
        private readonly string userId = "";
        private readonly string passWord = "";
        public MessageMQService()
        {
            InitializeComponent();
            mqHost = ConfigurationManager.AppSettings["MqHost"].ToString();
            channel = ConfigurationManager.AppSettings["Channel"];
            mqPort = Int32.Parse(ConfigurationManager.AppSettings["MqPort"]);
            qmName = ConfigurationManager.AppSettings["QmName"];
            qName = ConfigurationManager.AppSettings["QName"];
            userId = ConfigurationManager.AppSettings["UserId"];
            passWord = ConfigurationManager.AppSettings["PassWord"];
        }
        private SMSPOSQueue smsIbmQueue;
        Thread serviceThread = null;
        protected override void OnStart(string[] args)
        {
            serviceThread = new Thread(SMSReceive);
            serviceThread.Start();
        }

        protected override void OnStop()
        {
            if (smsIbmQueue != null)
                smsIbmQueue.IsStop = true;
        }
    
        private  void SMSReceive()
        {
            try
            {
                 smsIbmQueue = new SMSPOSQueue(mqHost, channel, mqPort, qmName, qName,userId,passWord);
                 smsIbmQueue.ListenQ();   
            }
            catch (Exception ex)
            {
                Log.LogHandler.WriteLog(ex.Message.ToString());
                ExceptionLog log = new ExceptionLog()
                {
                    Id = Guid.NewGuid(),
                    MessageType = "pos sms send",
                    ExceptionInfo = ex
                };
                CommonHandler.RecordExceptionLog(log);
            }
        }


    }
}
