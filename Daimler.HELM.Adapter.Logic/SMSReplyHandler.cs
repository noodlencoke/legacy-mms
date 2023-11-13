using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.HubService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.Adapter.Logic
{
    public abstract class BaseReplyHandler
    {
        public abstract CommonResult DoReply(MessageReplyInfo info);
    }


    public static class MessageReplyFactory
    {
        public static BaseReplyHandler CreateMessageReply(string dataSource)
        {
            string isMock = System.Configuration.ConfigurationManager.AppSettings["ISMockBS"];
            if (isMock == "1")
            {
                return new MockReply();
            }
            else if (dataSource == "EP")
            {
                return new EPSmsReply();
            }
            else
            {
                throw new Exception("no type to create");
            }

        }

    }

    public class EPSmsReply : BaseReplyHandler
    {
        private readonly string epReplyToCrmUrl = System.Configuration.ConfigurationManager.AppSettings["EpReplyToCrmUrl"];
        private readonly string epReplyToCrmUrl2 = System.Configuration.ConfigurationManager.AppSettings["EpReplyToCrmUrl2"];
        private readonly string epReplyToCrmeName = System.Configuration.ConfigurationManager.AppSettings["EpReplyToCrmName"];
        private readonly string epReplyToCrmPwd = System.Configuration.ConfigurationManager.AppSettings["EpReplyToCrmPwd"];
        public override CommonResult DoReply(MessageReplyInfo info)
        {
            CommonResult result = new CommonResult();
            try
            {
                if (info == null)
                {
                    throw new Exception("reply content is empty");
                }
                List<MessageReplyInfo> list = new List<MessageReplyInfo>();
                list.Add(info);
                string returnVal = CommonMethod.ConvertObjectToJson(list);

                try
                {
                    Log.LogHandler.WriteLog("1:"+epReplyToCrmUrl + "," + epReplyToCrmeName + "," + epReplyToCrmPwd + "," + returnVal);
                    CommonMethod.DoPost(epReplyToCrmUrl, returnVal, epReplyToCrmeName, epReplyToCrmPwd);
                }
                catch (Exception)
                {
                    Log.LogHandler.WriteLog("2:"+epReplyToCrmUrl2 + "," + epReplyToCrmeName + "," + epReplyToCrmPwd + "," + returnVal);
                    CommonMethod.DoPost(epReplyToCrmUrl2, returnVal, epReplyToCrmeName, epReplyToCrmPwd); 
                }
                
                result.IsOK = true;
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                throw ex;
            }
            return result;
        }
    }
    public class MockReply : BaseReplyHandler
    {
        public override CommonResult DoReply(MessageReplyInfo info)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            return result;
        }
    }




}
