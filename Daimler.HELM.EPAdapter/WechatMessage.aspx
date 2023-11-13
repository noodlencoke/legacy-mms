<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WechatMessage.aspx.cs" Inherits="Daimler.HELM.EPAdapter.WechatMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>MessageType:</td>
                <td>
                    <asp:DropDownList ID="ddlMessageType" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlMessageType_SelectedIndexChanged">
                        <asp:ListItem Text="==Select Message Type==" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Notification" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Marketing Campaign" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Content:</td>
                <td>
                    <asp:TextBox TextMode="MultiLine" Rows="20" ID="txtContent" runat="server" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Mobile:</td>
                <td><asp:TextBox ID="txtMobile" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" align="center"><asp:Button runat="server" Text="发送" ID="btnSend" OnClick="btnSend_Click" /></td>
            </tr>
        </table>
        
    </div>
    </form>
</body>
</html>
