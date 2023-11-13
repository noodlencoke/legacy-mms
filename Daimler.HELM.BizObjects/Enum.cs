using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.BizObjects
{
    public enum MessageType
    {
        SMS = 0,
        MMS = 1,
        Email = 2,
        Wechat = 3
    }

    public enum MessageStatus
    {
        /// <summary>
        /// 发送成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 提交成功
        /// </summary>
        SubmitSuccess = 1,

        /// <summary>
        /// 手机号码格式错误
        /// </summary>
        IncorrectMobileNoFormate = 2,

        /// <summary>
        /// 错误号码/限制运行商号码
        /// </summary>
        IncorrectMobileNumber = 3,

        /// <summary>
        /// 手机号码无效
        /// </summary>
        InvalidMobileNumber = 4,

        /// <summary>
        /// 黑名单
        /// </summary>
        Blacklist=5,
 
        /// <summary>
        /// 系统繁忙
        /// </summary>
        SystemBusy = 6,

        /// <summary>
        /// 内容包含敏感词
        /// </summary>
        ContainKeywords = 7,

        /// <summary>
        /// 扩展字码无效
        /// </summary>
        InvalidExtSubcode =8, 

        /// <summary>
        /// 子号码无效
        /// </summary>
        InvalidSubcode = 9,

        /// <summary>
        /// 内容为空
        /// </summary>
        EmptyContent = 10,

        /// <summary>
        /// 消息格式错误
        /// </summary>
        ContentFormateError = 11,

        /// <summary>
        /// 短信内容超过最大限制
        /// </summary>
        ContentTooLong = 12,

        /// <summary>
        /// 不支持的MSGFMT
        /// </summary>
        UnsupportMSGFMT = 13,

        /// <summary>
        /// 该模板ID已被禁用
        /// </summary>
        DisabledTemplateId = 14,

        /// <summary>
        /// msgid太长
        /// </summary>
        MsgidTooLong = 15,

        /// <summary>
        /// 定时时间格式错误
        /// </summary>
        TimerTimeFormateError = 16,

        /// <summary>
        /// 签名不合法
        /// </summary>
        IllegalSign = 17,

        /// <summary>
        /// 非法模板Id
        /// </summary>
        IlligalTemplateId = 18,

        /// <summary>
        /// 账号无效
        /// </summary>
        InvalidAccount = 19,

        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordError = 20,

        /// <summary>
        /// 账号被禁用或禁发
        /// </summary>
        AccountDisabled = 21,

        /// <summary>
        /// 单个号码（账号）次数限制
        /// </summary>
        SingleAccountSendLimit = 22,

        /// <summary>
        /// 余额不足
        /// </summary>
        LackOfBalance = 23,

       /// <summary>
        /// 购买产品或订购还未生效或产品已暂停使用
       /// </summary>
        ProductDisabled = 24,

        /// <summary>
        /// 单个号码相同内容限制
        /// </summary>
        SingleAccountSameContentLimit = 25,

        /// <summary>
        /// 运营商网关失败
        /// </summary>
        VendorGatewayFailed = 26,

        /// <summary>
        /// 手机号码个数超过最大限制
        /// </summary>
        MobileNumberCountLimit = 27,

        /// <summary>
        ///发送号码数没有达到该产品的最小发送数
        /// </summary>
        DidNotMeetProductMinLimit = 28,

        /// <summary>
        /// ip鉴权失败
        /// </summary>
        IPAuthenticationFailed = 29,

        /// <summary>
        ///  数据包大小不匹配
        /// </summary>
        DataPackageSizeNotMatch = 30,

        /// <summary>
        /// 请求来源地址无效
        /// </summary>
        InValidRequestSourceUrl = 31,

        /// <summary>
        /// 处理失败
        /// </summary>
        ProccessFailed = 32,

        /// <summary>
        /// 接入方式错误
        /// </summary>
        ConnectModeError = 33,

        /// <summary>
        /// 重复号码
        /// </summary>
        RepeatMobileNumber = 34,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknowError = -1 
    }
    public enum EmailStatus
    {
        SendSuccess=0,
        SendError=1
    }
    [Serializable]
    public enum MessageClassification
    {

        /// <summary>
        /// 单条
        /// </summary>
        Single = 1,

        /// <summary>
        /// 批量
        /// </summary>
        Batch = 2
    }



}
