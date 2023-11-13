using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Daimler.HELM.EPAdapter
{
    public partial class WechatMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnSend_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dicDataList = new Dictionary<string, object>();        
            string [] keyValueArray = this.txtContent.Text.Split('\n');
            if (this.ddlMessageType.SelectedValue == "1")
            {
                string strFirst = keyValueArray[0].Split(':').Length == 2 ? keyValueArray[0].Split(':')[1] : keyValueArray[0];
                string keyWord1 = keyValueArray[1].Split(':').Length == 2 ? keyValueArray[1].Split(':')[1] : keyValueArray[1];
                string keyWord2 = keyValueArray[2].Split(':').Length == 2 ? keyValueArray[2].Split(':')[1] : keyValueArray[2];
                string keyWord3 = keyValueArray[3].Split(':').Length == 2 ? keyValueArray[3].Split(':')[1] : keyValueArray[3];
                string keyWord4 = keyValueArray[4].Split(':').Length == 2 ? keyValueArray[4].Split(':')[1] : keyValueArray[4];
                string remark = keyValueArray[5].Split(':').Length == 2 ? keyValueArray[5].Split(':')[1] : keyValueArray[5];
                dicDataList.Add("first", strFirst);
                dicDataList.Add("keyword1", keyWord1);
                dicDataList.Add("keyword2", keyWord2);
                dicDataList.Add("keyword3", keyWord3);
                dicDataList.Add("keyword4", keyWord4);
                dicDataList.Add("remark", remark);

                WechatHandler.PushWechatTemplateMessage(this.txtMobile.Text, "JDvQ5YEnUTO92YMrayhOLyOkCUz4Qf31pYGv7kPyWC4", dicDataList);
            }
            else if(this.ddlMessageType.SelectedValue=="2") {
                string strFirst = keyValueArray[0].Split(':').Length == 2 ? keyValueArray[0].Split(':')[1] : keyValueArray[0];
                string keyWord1 = keyValueArray[1].Split(':').Length == 2 ? keyValueArray[1].Split(':')[1] : keyValueArray[1];
                string keyWord2 = keyValueArray[2].Split(':').Length == 2 ? keyValueArray[2].Split(':')[1] : keyValueArray[2];
                string keyWord3 = keyValueArray[3].Split(':').Length == 2 ? keyValueArray[3].Split(':')[1] : keyValueArray[3];
                string keyWord4 = keyValueArray[4].Split(':').Length == 2 ? keyValueArray[4].Split(':')[1] : keyValueArray[4];
                string remark = keyValueArray[5].Split(':').Length == 2 ? keyValueArray[5].Split(':')[1] : keyValueArray[5];
                dicDataList.Add("first", strFirst);
                dicDataList.Add("keyword1", keyWord1);
                dicDataList.Add("keyword2", keyWord2);
                dicDataList.Add("keyword3", keyWord3);
                dicDataList.Add("keyword4", keyWord4);
                dicDataList.Add("remark", remark);
                WechatHandler.PushWechatTemplateMessage(this.txtMobile.Text, "9dN55tTRVGiHGi5OlXtCX8C7ch6CCjUnAcVFvaeVUtc", dicDataList);
            }
        }

        protected void ddlMessageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlMessageType.SelectedValue == "1")
            {
                this.txtContent.Text = "尊敬的阁下,您所购买的此款梅赛德斯-奔驰车辆可以享受原厂保修\n期限:3年\n里程:10,000公里\n其他服务:3年免费24小时道路救援\n客服热线:4008181188\n尊敬的阁下,您所购买的此款梅赛德斯-奔驰车辆享受三年原厂保修，原厂设定的保养周期为10,000公里或一年，同时您还可以免 费享受三年24小时道路救援服务。同时，您可以将客服中心联系方式保存在您的通讯录中，以便您能在遇到任何与梅赛德斯-奔驰相关的问题时随时致电咨询。7*24小时联系方式：4008181188。客服中心将竭诚为您提供最优质的服务体验.";
            }
            else if (this.ddlMessageType.SelectedValue == "2")
            {
                this.txtContent.Text = "尊敬的客户您好，欢迎参加本次活动！！\n参与方式:现场报名\n活动日期:2015年10月1日\n活动地址:朝阳区望京街8号戴姆勒大厦\n活动内容:新老客户大酬宾现场购车5折优惠\n回复[Y]报名参加 点击此信息了解活动更多详情。";
            }
        }
    }
}