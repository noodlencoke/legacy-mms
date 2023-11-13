<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="MessageSearch.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.MessageSearch" %>

<%@ Register Assembly="DataGridPageControl" Namespace="DataGridPageControl" TagPrefix="cc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Log Query Detail</title>
    <link type="text/css" rel="stylesheet" href="../StyleCss/FormStyle.css" />
    <link type="text/css" rel="stylesheet" href="../StyleCss/GridViewStyle.css" />
    <link type="text/css" href="../../css/page_style.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        function showDetail(id)
        {
            window.open("MessageDetail.aspx?ID="+id);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
            <tr>
                <td>
                    <br />
                    <p>
                        <asp:Label ID="Label1" runat="server" CssClass="PopWindowTitle">Message send status Query Page</asp:Label>
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
                <td style="height: 87px; align-content: center;">
                    <table bordercolor="#8aafe6" cellspacing="0" bordercolordark="#ffffff" cellpadding="0"
                        width="100%" align="center" bgcolor="#ffffff" border="1">
                        <tr>
                            <td align="right" bgcolor="#eff6fe" class="auto-style1">MessageType：</td>
                            <td align="left" class="auto-style2">
                                <asp:DropDownList ID="ddlMessageType" runat="server" CssClass="DropDownList">
                                    <asp:ListItem Value="SMS">SMS</asp:ListItem>
                                    <asp:ListItem Value="MMS">MMS</asp:ListItem>
                                    <asp:ListItem Value="Email">Email</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="right" bgcolor="#eff6fe" class="auto-style3">MessageSource：</td>
                            <td align="left" class="auto-style3">
                                <asp:DropDownList ID="ddlDataSource" runat="server" CssClass="DropDownList">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="EP">EP</asp:ListItem>
                                    <asp:ListItem Value="POS">POS</asp:ListItem>
                                    <asp:ListItem Value="TSB">TSB</asp:ListItem>
                                    <asp:ListItem Value="LMS">LMS</asp:ListItem>
                                    <asp:ListItem Value="MeStore">MeStore</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right; background-color: #eff6fe" class="auto-style3">Mobile/Email：</td>
                            <td style="text-align: left;" class="auto-style3">
                                <asp:TextBox runat="server" ID="txtNumber" MaxLength="20" CssClass="TextBox" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" bgcolor="#eff6fe" style="width: 130px; height: 20px">Single/Batch：</td>
                            <td align="left" class="auto-style4">
                                <asp:DropDownList ID="ddlClassification" runat="server" CssClass="DropDownList">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="1">Single</asp:ListItem>
                                    <asp:ListItem Value="2">Batch</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="right" bgcolor="#eff6fe" class="auto-style3">Content：</td>
                            <td align="left" class="auto-style4">

                                <asp:TextBox ID="txtContent" MaxLength="100" runat="server" CssClass="TextBox" Width="173px"></asp:TextBox>
                            </td>
                            <td style="text-align: right; background-color: #eff6fe" class="auto-style3">Status：</td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="DropDownList">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="0">Success</asp:ListItem>
                                    <asp:ListItem Value="1">Sending</asp:ListItem>
                                    <asp:ListItem Value="-1">Failed</asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>
                        <tr>
                            <td align="right" bgcolor="#eff6fe" style="width: 130px; height: 20px">TaskId：</td>
                            <td align="left" class="auto-style4">
                                <asp:TextBox ID="txtTaskId" MaxLength="100" runat="server" CssClass="TextBox" Width="173px"></asp:TextBox>
                            </td>
                            <td align="right" bgcolor="#eff6fe" class="auto-style3">Send Date：</td>
                            <td align="left" colspan="3" class="auto-style4">
                                <asp:TextBox ID="txtStartReceviedDate" runat="server" CssClass="Wdate" onClick="WdatePicker()"></asp:TextBox>--<asp:TextBox ID="txtEndReceviedDate" runat="server" CssClass="Wdate" onClick="WdatePicker()"></asp:TextBox>
                            </td>

                        </tr>

                        <tr>
                            <td align="center" colspan="6" style="height: 22px">&nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:Button ID="btnFind" runat="server" CssClass="Button2" Text="Find" Width="50px" OnClick="btnFind_Click" />
                                <font face="宋体">&nbsp;</font><font face="宋体">&nbsp;&nbsp;</font><font
                                    face="宋体"></font>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="Noprint" width="95%" align="center" style="height:100px;">
            <tr>
                <td>
                    <cc1:MTCGridView ID="DataGridView" AutoGenerateColumns="False" runat="server" CellPadding="3"
                        CellSpacing="1" CssClass="GridViewStyle" PagingMode="None" Width="100%" DataKeyNames="Id" ShowCheckAll="False" OnRowDataBound="DataGridView_RowDataBound">
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <Columns>
                            <asp:BoundField DataField="id" Visible="False" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdid" runat="server" Value='<%# Eval("id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="dataSource" HeaderText="DataSource" />
                            <asp:BoundField DataField="_number" HeaderText="Mobile/Email" />
                            <asp:BoundField DataField="_content" HeaderText="Content" />
                            <asp:BoundField DataField="status" HeaderText="Status" />
                            <asp:BoundField DataField="receivedDt" HeaderText="ReceivedDt" />
                            <asp:BoundField DataField="createdDt" HeaderText="CreatedDt" />
                            <asp:BoundField DataField="submitDt" HeaderText="SubmitDt" />
                            <asp:BoundField DataField="getStatusDt" HeaderText="GetStatusDt" />
                            <asp:BoundField DataField="sentDt" HeaderText="SentDt" />
                           <%-- <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Views/MessageQuery/MessageDetail.aspx?ID={0}" DataTextFormatString="Detail" HeaderText="Detail" Text="Detail" Target="_blank">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>--%>
                        </Columns>
                        <EmptyDataTemplate>
                            There is no data in the query conditions.
                        </EmptyDataTemplate>
                    </cc1:MTCGridView>
                </td>
            </tr>
            <tr>
                <td style="height: 28px">
                    <cc1:MTCPager ID="DataPager" runat="server" PageNumberColor="#000000" PagerFontColor="#000000" PagerFontFamily="" PagerLeftText=" " Visible="False" OnPageIndexChanged="DataPager_PageIndexChanged" IsShowPageTotalCount="False">
<PageJump PageJumpType="DropDownList" LeftText="go to page" Visiable="False"></PageJump>
                    </cc1:MTCPager>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
