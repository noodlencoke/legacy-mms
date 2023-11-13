<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageDetail.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.Views.MessageQuery.MessageDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Message Send Status Detail</title>
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
                        <asp:Label ID="lblTitle" runat="server" CssClass="PopWindowTitle">Message Send Status Detail</asp:Label>
                    </p>
                </td>
            </tr>
            <tr>
                <td bgcolor="#3a6fb5">
                    <img height="3" alt="" src="" width="2" name="" /></td>
            </tr>
        </table>
        <br>
        <table class="Noprint" width="95%" align="center">
            <tr>
                <td align="center" class="auto-style10">
                    <table bordercolor="#8aafe6" cellspacing="0" bordercolordark="#ffffff" cellpadding="0"
                        width="100%" align="center" bgcolor="#ffffff" border="1">

                        <%=ShowMessage() %>
                        <tr>
                            <td align="center" colspan="4" class="auto-style7">
                               
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
