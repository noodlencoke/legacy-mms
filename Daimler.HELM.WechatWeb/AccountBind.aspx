<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountBind.aspx.cs" Inherits="Daimler.HELM.WechatWeb.AccountBind" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=yes" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
    <link type="text/css" href="css/accBind.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.6.4.min.js"></script>
    <script type="text/javascript" src="js/Json2.js"></script>

    <script type="text/javascript">

        var sending = null;
        var times = 0;
        function GetPinCode() {

            var mobile = $("#txtMobile").val();

            if (mobile == null || mobile == "")
            {
                $("#labErrorMsg").text("请输入手机号码！");
                return;
            }

            if (isNaN(mobile))
            {
                $("#labErrorMsg").text("请输入正确的手机号码！");
                return;
            }

            var transferData = new Object();
            transferData.Mobile = mobile;
            transferData = JSON.stringify(transferData);
            $.ajax({
                type:"POST",
                url: "/ajax?Type=GetPinCode",
                dataType:'json',
                data: transferData,
                success: function (data) {
                    if (data.IsOK) {
                        times = 60;
                        sending = window.setInterval(ShowPinCodeButton, 1000);
                    } else {
                        $("#labErrorMsg").text(data.ReturnMessage);
                    }
                },
                error: function (data) {
                    $("#labErrorMsg").text("发送验证码失败!");
                }
            });
            
        }

        function ShowPinCodeButton()
        {
            var dt = Date();
            if (times > 0) {
                $("#labErrorMsg").text("验证码已经发送至您的手机，请注意查收!");
                $("#btnGetPinCode").val(times + "s");
                $("#btnGetPinCode").attr("disabled", true);
                $("#btnGetPinCode").addClass("btnSending");

                times = times - 1;
            } else {
                $("#labErrorMsg").text("验证码已过期，请重新获取!");
                $("#btnGetPinCode").val("重新获取");
                $("#btnGetPinCode").attr("disabled", false);
                $("#btnGetPinCode").removeClass("btnSending");

                clearInterval(sending);
            }
        }
    </script>

    <style type="text/css">

        .btnSending {
            color:grey;
            border-color:gray; 
        }

    </style>

</head>
<body>
    <form id="BindAccount" name="editPage" runat="server">
        <div id="TitleBar">
            <div id="logo">身份绑定</div>
        </div>
        <label class="titleDesc">绑定手机号码</label>
        <div>
            <div class="itemDiv">
                <label class="normallabel">手机号码</label>
                <input type="text" class="textBox" id="txtMobile" runat="server" />
            </div>
            <div class="itemDiv">
                <input type="text" class="textBox" id="id_PinCode" style="display: inline; width: 47%" name="PinCode" runat="server" />
                <input type="button" id="btnGetPinCode" class="button" value="获取验证码" style="display: inline; width: 100px;" onclick="GetPinCode()" />
            </div>
            <div style="height: auto">
                <label id="labErrorMsg" runat="server"></label>
            </div>
            <div class="itemDiv">
                <label class="infoLab">
                    <input type="checkbox" id="id_check" runat="server" />我已阅读并同意<a class="link" href="http://www.baidu.com">《戴姆勒信息安全协议》</a>
                </label>
            </div>
            <div class="itemDiv">
                <input type="hidden" runat="server" id="id_IsSubmit" value="0" />
              <%--  <input class="button" type="button" id="id_submit" value="提交" />--%>
                <asp:Button ID="btnSubmit" runat="server"  Text="提交" CssClass="button" OnClick="btnSubmit_Click"/>
            </div>
        </div>
        <div id="bodybottom">
            Cookies&nbsp;|&nbsp;法律提示&nbsp;|&nbsp;数据保护
            <br />
            京ICP备 1234号
            <br />
            © 2014 戴姆勒股份公司（Daimler AG）保留所有权利
        </div>
    </form>
</body>
</html>
