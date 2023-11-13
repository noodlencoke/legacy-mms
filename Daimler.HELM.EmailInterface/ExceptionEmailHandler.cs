using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.EmailInterface
{
    public static class ExceptionEmailHandler
    {
        private static readonly string userEmail = System.Configuration.ConfigurationManager.AppSettings["UserEmail"];
        private static readonly string from = System.Configuration.ConfigurationManager.AppSettings["From"];
        private static readonly string fromEmailPwd = System.Configuration.ConfigurationManager.AppSettings["FromEmailPwd"];
        private static readonly string smtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];

        public static void SendExceptionMessageWithEmail(ExceptionLog exceptionLog)
        {
            try
            {
                string content = string.Format("<html><head>{0}</head><body>{1}<br/><br/>{2}</body></html>", exceptionLog.MessageType, exceptionLog.ExceptionInfo.Message, exceptionLog.ExceptionInfo.StackTrace);
                string mailuser = from;
                string mailpwd = fromEmailPwd;
                // Create a message and set up the recipients.
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                //添加发件人邮箱
                message.From = new MailAddress(from);

                //添加收件人邮箱
                string[] toUsers = userEmail.Split(',');
                foreach (string addr in toUsers)
                {
                    message.To.Add(new MailAddress(addr));
                }
                //添加邮件主题
                message.Subject = "Daimler HELM Exception Log --" + exceptionLog.MessageType;
                //添加邮件内容
                message.IsBodyHtml = true;
                message.Body = content;
                message.Priority = System.Net.Mail.MailPriority.High;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                //Send the message.
                SmtpClient client = new SmtpClient(smtpServer);
                // Add credentials if the SMTP server requires them.
                client.Credentials = new NetworkCredential(mailuser, mailpwd);
                client.Port = 25;
                client.Send(message);
            }
            catch (Exception ex)
            {
                Log.LogHandler.WriteLog("send exception log with email error:" + ex.Message + "," + ex.StackTrace);
            }

        }
    }
}
