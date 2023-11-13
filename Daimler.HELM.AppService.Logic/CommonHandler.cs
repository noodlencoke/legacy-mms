using Daimler.HELM.BizObjects;
using Daimler.HELM.Common;
using Daimler.HELM.DBInterface;
using Daimler.HELM.EmailInterface;
using Daimler.HELM.HubService.Contract;
using Daimler.HELM.Log;
using Daimler.HELM.MessageFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Daimler.HELM.HubService.Logic
{
    public static class CommonHandler
    {
        /// <summary>
        /// record exception log
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static void RecordExceptionLog(ExceptionLog log)
        {
            CommonResult result = new CommonResult();
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                result.IsOK = comDal.RecordExceptionLog(log);

                //ExceptionEmailHandler.SendExceptionMessageWithEmail(log);
            }
            catch (Exception ex)
            {
                result.IsOK = false;
                result.ReturnMessage = ex.Message;
                Log.LogHandler.WriteLog("记录异常日志错误：" + ex.Message);
                Log.LogHandler.WriteLog(string.Format("异常信息:id:{0},messageType:{1},exception:{2}", log.MessageId, log.MessageType, log.ExceptionInfo));
            }
        }

        public static string GetSystemConfig(string operationId)
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                return comDal.GetSystemConfig(operationId);
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "get systemconfig",
                    ExceptionInfo = ex
                });
            }
            return null;
        }

        public static bool UpdateSystemConfig(string operationId, string value)
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                return comDal.UpdateSystemConfig(operationId, value);
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "update systemconfig",
                    ExceptionInfo = ex
                });
            }
            return false;
        }


        /// <summary>
        /// validate interface account
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="accountName"></param>
        /// <param name="accountPwd"></param>
        /// <returns></returns>
        public static CommonResult ValidateInterfaceAccount(string dataSource, string accountName, string accountPwd)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            try
            {
                if (string.IsNullOrEmpty(dataSource))
                {
                    result.IsOK = false;
                    result.ReturnMessage = "数据源名称不能为空！";
                    return result;
                }

                if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(accountPwd))
                {
                    result.IsOK = false;
                    result.ReturnMessage = "账号或密码不能为空！";
                    return result;
                }

                string cacheKey = "accountList_" + dataSource;
                List<InterfaceAccount> accountList = null;
                if (HttpRuntime.Cache[cacheKey] != null && (HttpRuntime.Cache[cacheKey] as List<InterfaceAccount>).Count > 0)
                {
                    accountList = HttpRuntime.Cache[cacheKey] as List<InterfaceAccount>;
                }
                else
                {
                    ICommonDal comDal = CommonDalFactory.GetInstance();
                    accountList = comDal.GetInterfaceAccount(dataSource);
                    if (accountList.Count == 0)
                    {
                        result.IsOK = false;
                        result.ReturnMessage = "账号信息错误,请联系管理员!";
                        return result;
                    }
                    HttpRuntime.Cache.Add(cacheKey, accountList, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
                List<InterfaceAccount> newAccountList = accountList.FindAll(p => p.AccountName == accountName && p.AccountPwd == accountPwd);
                if (newAccountList.Count == 0)
                {
                    result.IsOK = false;
                    result.ReturnMessage = "无对应账号信息!";
                    return result;
                }
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "ValidateInterfaceAccount error",
                    ExceptionInfo = ex
                });

                result.IsOK = false;
                result.ReturnMessage = "验证账号密码错误，请联系管理员！";
            }
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public static List<InterfaceAccount> GetInterfaceAccountListAll()
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                return comDal.GetInterfaceAccount();
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "GetInterfaceAccountListAll Exception",
                    ExceptionInfo = ex
                });
            }
            return null;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static InterfaceAccount GetInterfaceAccountByID(Guid id)
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                List<InterfaceAccount> interfaceAccountList = comDal.GetInterfaceAccountByID(id);
                return interfaceAccountList[0];
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "GetInterfaceAccountByID Exception",
                    ExceptionInfo = ex
                });
            }
            return null;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="InterfaceAccount"></param>
        /// <returns></returns>
        public static CommonResult AddInterfaceAccount(InterfaceAccount interfaceAccount)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                Guid id = interfaceAccount.Id;
                string accountName = interfaceAccount.AccountName;
                string accountPwd = interfaceAccount.AccountPwd;
                DateTime effectiveDt = DateTime.Parse(interfaceAccount.EffectiveDt);
                int effectiveDays = interfaceAccount.EffectiveDays;
                int status = interfaceAccount.Status;
                string datasource = interfaceAccount.Datasource;
                bool createResult = comDal.CreateInterfaceAccount(id, accountName, accountPwd, effectiveDt, effectiveDays, status, datasource);
                if (!createResult)
                {
                    result.IsOK = false;
                    result.ReturnMessage = "账号添加失败,请联系管理员!";
                    return result;
                }
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "AddInterfaceAccount Exception",
                    ExceptionInfo = ex
                });

                result.IsOK = false;
                result.ReturnMessage = "账号添加异常，请联系管理员！";
            }
            return result;
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        public static CommonResult RemoveInterfaceAccount(Guid id)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                bool deleteResult = comDal.DeleteInterfaceAccount(id);
                if (!deleteResult)
                {
                    result.IsOK = false;
                    result.ReturnMessage = "账号删除失败,请联系管理员!";
                    return result;
                }
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "RemoveInterfaceAccount Exception",
                    ExceptionInfo = ex
                });

                result.IsOK = false;
                result.ReturnMessage = "账号删除异常，请联系管理员！";
            }
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="InterfaceAccount"></param>
        /// <returns></returns>
        public static CommonResult UpdateInterfaceAccount(InterfaceAccount interfaceAccount)
        {
            CommonResult result = new CommonResult();
            result.IsOK = true;
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                Guid id = interfaceAccount.Id;
                string accountName = interfaceAccount.AccountName;
                string accountPwd = interfaceAccount.AccountPwd;
                DateTime effectiveDt = DateTime.Parse(interfaceAccount.EffectiveDt);
                int effectiveDays = interfaceAccount.EffectiveDays;
                int status = interfaceAccount.Status;
                string datasource = interfaceAccount.Datasource;
                bool updateResult = comDal.UpdateInterfaceAccount(id, accountName, accountPwd, effectiveDt, effectiveDays, status, datasource);
                if (!updateResult)
                {
                    result.IsOK = false;
                    result.ReturnMessage = "账号更新失败,请联系管理员!";
                    return result;
                }
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "UpdateInterfaceAccount Exception",
                    ExceptionInfo = ex
                });

                result.IsOK = false;
                result.ReturnMessage = "账号更新异常，请联系管理员！";
            }
            return result;
        }

        public static System.Data.DataSet GetMessageLog(string messageType, string messageSource, string beginDate, string endDate, string keyWord, int pageIndex, int pageSize, out int totalCount)
        {

            ICommonDal comDal = CommonDalFactory.GetInstance();
            return comDal.GetMessageLog(messageType, messageSource, beginDate, endDate, keyWord, pageIndex, pageSize, out totalCount);
        }
        public static RequestLog GetLogInfoById(string id, string messageType)
        {
            ICommonDal comDal = CommonDalFactory.GetInstance();
            return comDal.GetLogInfoById(id, messageType);
        }


        /// <summary>
        /// 通过日期获取异常日志
        /// </summary>
        /// <param name="date">日期：2015-10-22</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        public static List<ExceptionLogInfo> GetExceptionLogByDate(string date, string endDate, int pageSize, int pageIndex)
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                return comDal.GetExceptionLogByDate(date, endDate, pageSize, pageIndex);
            }
            catch (Exception ex)
            {

                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "get ExceptionLog Exception",
                    ExceptionInfo = ex
                });
            }
            return null;

        }
        public static ExceptionLogInfo GetExceptionLogById(string id)
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                return comDal.GetExceptionLogById(id);
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
               {
                   MessageType = "get ExceptionLog Detial Exception",
                   ExceptionInfo = ex
               });
            }
            return null;
        }
        public static int GetExceptionLogCountByDate(string date, string endDate)
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                return comDal.GetExceptionLogCountByDate(date, endDate);
            }
            catch (Exception ex)
            {

                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "get ExceptionLogCount Exception",
                    ExceptionInfo = ex
                });
            }
            return -1;
        }

        public static List<ShowMessageReplyInfo> GetMessageReplyInfoByCondit(ShowMessageReplyInfo where, int pageSize, int pageIndex)
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                return comDal.GetMessageReplyInfoByCondit(where, pageSize, pageIndex);
            }
            catch (Exception ex)
            {

                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "get MessageReply Exception",
                    ExceptionInfo = ex
                });
            }
            return null;
        }
        public static int GetMessageReplyCount(ShowMessageReplyInfo where)
        {
            try
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                return comDal.GetMessageReplyCount(where);
            }
            catch (Exception ex)
            {

                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "get MessageReply count Exception",
                    ExceptionInfo = ex
                });
            }
            return -1;
        }

        public static SourceVendorConfig GetSenderConfigBySource(string dataSource, string sessionId, MessagePriority priority)
        {
            try
            {
                string accountType = "2";
                if (priority == MessagePriority.AboveNormal || priority == MessagePriority.High || priority == MessagePriority.VeryHigh || priority == MessagePriority.Highest)
                {
                    accountType = "1";
                }

                SourceVendorConfig souceConfig = null;
                string cacheKey = "sourceVendorConfig_" + dataSource + "_" + sessionId + "_" + accountType;
                if (HttpRuntime.Cache[cacheKey] != null)
                {
                    souceConfig = HttpRuntime.Cache[cacheKey] as SourceVendorConfig;
                }
                else
                {
                    ICommonDal comDal = CommonDalFactory.GetInstance();
                    List<SourceVendorConfig> sourceVenderConfigList = comDal.GetSourceSenderConfig(dataSource, sessionId);

                    if (sourceVenderConfigList.Count == 1)
                    {
                        souceConfig = sourceVenderConfigList[0];
                    }
                    else if (sourceVenderConfigList.Count == 2)
                    {

                        souceConfig = sourceVenderConfigList.Find(p => p.AccountType == accountType);
                    }

                    if (souceConfig != null)
                    {
                        HttpRuntime.Cache.Add(cacheKey, souceConfig, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    }
                }
                return souceConfig;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "," + ex.StackTrace);
            }
        }

        public static MessageInfo GetSender(MessageInfo msgObj)
        {
            try
            {
                if (string.IsNullOrEmpty(msgObj.DataSource) || string.IsNullOrEmpty(msgObj.SessionId))
                {
                    return msgObj;
                }

                if (string.IsNullOrEmpty(msgObj.VendorName))
                {
                    string strVendorMock = System.Configuration.ConfigurationManager.AppSettings["VendorMock"];

                    SourceVendorConfig config = CommonHandler.GetSenderConfigBySource(msgObj.DataSource, msgObj.SessionId, msgObj.Priority);
                    if (config == null)
                    {
                        throw new Exception("loss vendoer config for " + msgObj.DataSource + " " + msgObj.SessionId);
                    }

                    if (strVendorMock == "1")
                    {
                        msgObj.VendorName = "MOCK";
                    }
                    else
                    {
                        msgObj.VendorName = config.VendorName;
                    }

                    msgObj.AccountName = config.AccountName;
                    msgObj.AccountPwd = config.AccountPwd;
                    msgObj.ExtendNumber = config.SignNumber;
                    msgObj.SignName = config.SignName;
                }
                return msgObj;
            }
            catch (Exception ex)
            {
                RecordExceptionLog(new ExceptionLog()
                {
                    MessageType = "get SourceVendorConfig Exception",
                    ExceptionInfo = ex
                });
                return msgObj;
            }
        }


        public static List<SourceVendorConfig> GetVendorAccountList()
        {
            List<SourceVendorConfig> configList = null;
            string cacheKey = "venderAccountList";
            if (HttpRuntime.Cache[cacheKey] != null)
            {
                configList = HttpRuntime.Cache[cacheKey] as List<SourceVendorConfig>;
            }
            else
            {
                ICommonDal comDal = CommonDalFactory.GetInstance();
                configList = comDal.GetVendorAccountList();

                HttpRuntime.Cache.Add(cacheKey, configList, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }

            return configList;

        }

        public static MessageInfo ValidateMessage(MessageInfo msgInfo, out bool isValidate)
        {
            isValidate = false;
            IFilter filter = MessageFilter.FilterFactory.CreateFilter(msgInfo);
            if (filter != null)
            {
                if (filter.Validate())
                {
                    isValidate = true;
                    MessageInfo newMessageInfo = filter.BusinessOperate();
                    if (newMessageInfo != null)
                    {
                        return newMessageInfo;
                    }
                }
                else
                {
                    isValidate = false;
                    //修改消息状态
                    MessageInfo newMessageInfo = filter.BusinessOperate();
                    if (newMessageInfo != null)
                    {
                        Dictionary<string, string> dicErrorList = filter.GetErrorList();
                        if (dicErrorList.Keys.Contains("1"))
                        {
                            newMessageInfo.Status = MessageStatus.IncorrectMobileNoFormate;
                            newMessageInfo.ErrorInfo = dicErrorList["1"];
                        }
                        else if (dicErrorList.Keys.Contains("2"))
                        {
                            newMessageInfo.Status = MessageStatus.EmptyContent;
                            newMessageInfo.ErrorInfo = dicErrorList["2"];
                        }

                        return newMessageInfo;
                    }
                }

                return msgInfo;
            }
            return null;
        }


        public static void ValidateBatchMessage(List<MessageInfo> msgInfoList, out BatchMessage batchMessage, out List<MessageInfo> errorMsgList)
        {
            Dictionary<string, string> msgList = new Dictionary<string, string>();

            batchMessage = new BatchMessage();
            batchMessage.MessageList = new List<MessageInfo>();

            errorMsgList = new List<MessageInfo>();

            //validate message
            foreach (MessageInfo msgInfo in msgInfoList)
            {
                if (!msgList.Keys.Contains(msgInfo.Number))
                {
                    bool isValidate = false;
                    msgInfo.Classification = MessageClassification.Batch;
                    MessageInfo newMessageInfo = CommonHandler.ValidateMessage(msgInfo, out isValidate);

                    if (newMessageInfo != null)
                    {
                        if (isValidate)
                        {
                            batchMessage.MessageList.Add(newMessageInfo);
                        }
                        else
                        {
                            errorMsgList.Add(newMessageInfo);
                        }
                    }

                    msgList.Add(msgInfo.Number, msgInfo.Number);
                }
                else
                {
                    msgInfo.Classification = MessageClassification.Batch;
                    msgInfo.Status = MessageStatus.RepeatMobileNumber;
                    msgInfo.ErrorInfo = "send bath message has repeat number.";
                    errorMsgList.Add(msgInfo);
                }
            }
        }
    }


}
