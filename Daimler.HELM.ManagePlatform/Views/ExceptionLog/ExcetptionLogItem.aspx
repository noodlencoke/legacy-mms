<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ExcetptionLogItem.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.Views.ExceptionLog.ExcetptionLogItem" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Exception Log</title>
    <link type="text/css" rel="stylesheet" href="../StyleCss/FormStyle.css" />
    <link type="text/css" rel="stylesheet" href="../StyleCss/GridViewStyle.css" />
    <link type="text/css" href="../../css/page_style.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form runat="server" id="form1">
        <table height="3" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
            <tr>
                <td>
                    <br />
                    <p>
                        <asp:Label ID="lblTitle" runat="server" CssClass="PopWindowTitle">Exception Log Detail</asp:Label>
                    </p>
                </td>
            </tr>
            <tr>
                <td bgcolor="#3a6fb5">
                    <img height="3" alt="" src="" width="2" name="" /></td>
            </tr>
        </table>
        <br/>
        <table class="Noprint" width="95%" align="center">
            <tr>
                <td align="center">
                    <table bordercolor="#8aafe6" cellspacing="0" bordercolordark="#ffffff" cellpadding="0"
                        width="100%" align="center" bgcolor="#ffffff" border="1">
                        <tr>
                            <td align="right" width="130" bgcolor="#eff6fe" style="height: 20px">MessageId：</td>
                            <td align="left" style="height: 20px">
                                <asp:TextBox ID="txtMessageId" runat="server" CssClass="TextBox" Width="280px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 21px" align="right" bgcolor="#eff6fe">MessageType：</td>
                            <td style="height: 21px" align="left">
                                <asp:TextBox ID="txtMessageType" runat="server" CssClass="TextBox" Width="280px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 21px" align="right" bgcolor="#eff6fe">
                                <font face="宋体"><span style="font-family: Verdana">
                                CreateDate：</span></font></td>
                            <td style="height: 21px" align="left">
                                <asp:TextBox ID="txtCreateDate" runat="server" CssClass="TextBox" Width="280px" Enabled="False"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <td align="right" bgcolor="#eff6fe" style="height: 21px">ExceptionInfo：</td>
                            <td align="left" style="height: 21px">
                                <asp:TextBox ID="txtSendContent" runat="server" CssClass="TextBox" Width="95%" Height="400px"
                                    TextMode="MultiLine"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <td align="center" colspan="2" style="height: 22px">
                                <font face="宋体">
                                <asp:Button ID="btnClose" runat="server" CssClass="Button2"
                                    Text="Close" Visible="True" Width="61px" OnClick="btnClose_Click" />&nbsp;</font><font face="宋体">&nbsp;&nbsp;</font><font
                                        face="宋体"></font>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
