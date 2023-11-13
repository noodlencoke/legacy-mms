<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageLogQueryItem.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.Views.LogQuery.MessageLogQueryItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Log Query Detail</title>
    <link type="text/css" rel="stylesheet" href="../StyleCss/FormStyle.css" />
    <link type="text/css" href="../../css/page_style.css" rel="stylesheet" />

    <style type="text/css">
        .auto-style5 {
            height: 26px;
        }
        .auto-style6 {
            height: 27px;
        }
        .auto-style7 {
            height: 35px;
        }
        .auto-style8 {
            height: 22px;
        }
        .auto-style9 {
            height: 32px;
        }
        .auto-style10 {
            height: 282px;
        }
    </style>

</head>
<body>
   <form id="form1" runat="server">

         <table height="3" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
            <tr>
                <td>
                <br />
                    <p>
                        <asp:Label ID="lblTitle" runat="server" CssClass="PopWindowTitle">Log Query Detail</asp:Label></p>
                </td>
            </tr>
            <tr>
                <td bgcolor="#3a6fb5">
                    <img height="3" alt="" src="" width="2" name=""/></td>
            </tr>
        </table>
        <br>
        <table class="Noprint" width="95%" align="center">
            <tr>
                <td align="center" class="auto-style10">
                    <table bordercolor="#8aafe6" cellspacing="0" bordercolordark="#ffffff" cellpadding="0"
                        width="100%" align="center" bgcolor="#ffffff" border="1">
                        <tr>
                            <td align="right" width="130" bgcolor="#eff6fe" style="height: 20px">
                                Id：</td>
                            <td align="left" class="auto-style5">
                             <asp:TextBox ID="txtId" runat="server" CssClass="TextBox" Width="257px" ReadOnly="True"></asp:TextBox>
                                </td>
                                                        <td style="height: 21px" align="right" bgcolor="#eff6fe">
                                <font face="宋体"><span style="font-family: Verdana">
                                CreateDate：</span></font></td>
                            <td align="left" class="auto-style8">
                             <asp:TextBox ID="txtCreateDate" runat="server" CssClass="TextBox" Width="219px" ReadOnly="True"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 21px" align="right" bgcolor="#eff6fe">
                                <font face="宋体"><span style="font-family: Verdana">
                                MessageSource：</span></font></td>
                            <td align="left" class="auto-style6">
                             <asp:TextBox ID="txtMessageSource" runat="server" CssClass="TextBox" Width="257px" ReadOnly="True"></asp:TextBox></td>
                                                        <td style="height: 21px" align="right" bgcolor="#eff6fe">
                                                            <font face="宋体"><span style="font-family: Verdana">
                                                            MessageType：</span></font></td>
                            <td align="left" class="auto-style8">
                             <asp:TextBox ID="txtMessageType" runat="server" CssClass="TextBox" Width="222px" ReadOnly="True"></asp:TextBox></td>
                        </tr>

                        <tr >
                            <td align="right" bgcolor="#eff6fe" style="height: 21px">
                                Content：</td>
                            <td align="left" style="height: 21px" colspan="3">
                                <asp:TextBox ID="txtSendContent" runat="server" CssClass="TextBox" Width="653px" Height="93px"
                                    TextMode="MultiLine"></asp:TextBox>

                            </td>
                        </tr>
                                 <tr>
                            <td align="right" bgcolor="#eff6fe" class="auto-style9">
                                RequestInfo：</td>
                            <td align="left" colspan="3" class="auto-style9">
                                <asp:TextBox ID="txtRequestInfo" runat="server" CssClass="TextBox" Width="652px" Height="91px"
                                    TextMode="MultiLine"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4" class="auto-style7" >
                                <font face="宋体">
                                <input id="txtClose" type="button" value="Close" onclick="window.opener = null; window.open('', '_self'); window.close();" class="Button1" />&nbsp;</font><font face="宋体">&nbsp;&nbsp;</font><font
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
