<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="ResplayMessageDisplay.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.Views.ReplyMessage.ResplayMessageDisplay" %>

<%@ Register Assembly="DataGridPageControl" Namespace="DataGridPageControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reply Message</title>

    <link type="text/css" rel="stylesheet" href="../StyleCss/FormStyle.css" />
    <link type="text/css" rel="stylesheet" href="../StyleCss/GridViewStyle.css" />
    <link type="text/css" href="../../css/page_style.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="from1" runat="server">
        <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
            <tr>
                <td>
                    <br />
                    <p>
                        <asp:Label ID="Label1" runat="server" CssClass="PopWindowTitle">Reply Message Query Page</asp:Label>
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
                <td align="center" style="height: 87px">
                    <table bordercolor="#8aafe6" cellspacing="0" bordercolordark="#ffffff" cellpadding="0"
                        width="100%" align="center" bgcolor="#ffffff" border="1">
                        <tr>
                            <td align="right" bgcolor="#eff6fe" class="auto-style1">MessageType：</td>
                            <td bgcolor="#eff6fe"  align="left" class="auto-style2">
                                <asp:DropDownList ID="txtMessageType" runat="server" CssClass="DropDownList">
                                    <asp:ListItem Value="0">SMS</asp:ListItem>
                                    <asp:ListItem Value="1">MMS</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="right" bgcolor="#eff6fe" class="auto-style3">MessageSource：</td>
                            <td bgcolor="#eff6fe" align="left" class="auto-style4">
                                <asp:DropDownList ID="txtMessageSource" runat="server" CssClass="DropDownList">
                                    <asp:ListItem Value="0">EP</asp:ListItem>
                                    <asp:ListItem Value="1">POS</asp:ListItem>
                                    <asp:ListItem Value="2">TSB</asp:ListItem>
                                    <asp:ListItem Value="3">LMS</asp:ListItem>
                                    <asp:ListItem Value="4">MeStore</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td  align="right" bgcolor="#eff6fe" style="width: 130px; height: 20px">Duration：</td>
                            <td bgcolor="#eff6fe" align="left" class="auto-style4">
                                <asp:TextBox ID="txtBeginDate" runat="server" CssClass="Wdate" onClick="WdatePicker()"></asp:TextBox>--<asp:TextBox ID="txtEndDate" runat="server" CssClass="Wdate" onClick="WdatePicker()"></asp:TextBox>

                            </td>
                            <td align="right" bgcolor="#eff6fe" class="auto-style3"></td>
                            <td bgcolor="#eff6fe" align="left" class="auto-style4"></td>

                        </tr>

                        <tr>
                            <td bgcolor="#eff6fe" align="center" colspan="4" style="height: 22px">&nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="btnFind" runat="server" CssClass="Button2"
                                Text="Find" Width="50px" OnClick="btnFind_Click" /><font face="宋体">&nbsp;</font><font face="宋体">&nbsp;&nbsp;</font><font
                                    face="宋体"></font>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="Noprint" width="95%" align="center">
            <tr>
                <td>
                    <cc1:MTCGridView ID="DataGridView" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CellSpacing="1" CssClass="GridViewStyle" PagingMode="None" Width="100%" DataKeyNames="Id" ShowCheckAll="False" CheckColumnPosition="Left">
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <Columns>
                            <asp:BoundField DataField="id" Visible="False" />
                            <asp:BoundField DataField="taskId" HeaderText="TaskId">
                                <ItemStyle HorizontalAlign="Center" Width="240px" />
                            </asp:BoundField>
                             <asp:BoundField DataField="status" HeaderText="Status">
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="mobile" HeaderText="Mobile">
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="content" HeaderText="Content">
                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="getTime" HeaderText="ReplyTime"></asp:BoundField>
                            <asp:BoundField DataField="senderNumber" HeaderText="SenderNumber">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="dataSource" HeaderText="DataSource">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MessageType" HeaderText="MessageType">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>


                        </Columns>
                        <EmptyDataTemplate>
                            There is no data in the query conditions.
                        </EmptyDataTemplate>
                    </cc1:MTCGridView>
                </td>
            </tr>
            <tr>
                <td style="height: 28px">
                    <cc1:MTCPager ID="DataPager" runat="server" PageNextDescription="NextPage" PageNumberColor="#000000" PagePreviousDescription="PrevPage" PagerFontColor="#000000" PagerFontFamily="" PagerLeftText=" " OnPageIndexChanged="DataPager_PageIndexChanged">
<PageJump PageJumpType="DropDownList" LeftText="Jump to page" Visiable="False"></PageJump>
                    </cc1:MTCPager>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
