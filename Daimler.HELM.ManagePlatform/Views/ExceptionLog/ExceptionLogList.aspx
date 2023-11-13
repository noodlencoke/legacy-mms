<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ExceptionLogList.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.Views.ExceptionLog.ExceptionLogList" %>

<%@ Register Assembly="DataGridPageControl" Namespace="DataGridPageControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exception Log</title>
    <link type="text/css" rel="stylesheet" href="../StyleCss/FormStyle.css" />
    <link type="text/css" rel="stylesheet" href="../StyleCss/GridViewStyle.css" />
    <link type="text/css" href="../../css/page_style.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/My97DatePicker/WdatePicker.js"></script>
      <script type="text/javascript">
          function showDetail(id) {
              window.open("ExcetptionLogItem.aspx?ID=" + id);
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
                        <asp:Label ID="Label1" runat="server" CssClass="PopWindowTitle">Exception Log Query Page</asp:Label>
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
                <td align="center" style="height: 27px">
                    <table bordercolor="#8aafe6" cellspacing="0" bordercolordark="#ffffff" cellpadding="0"
                        width="100%" align="center" bgcolor="#ffffff" border="1">
                        <tr>
                            <td align="right" bgcolor="#eff6fe" style="width: 130px; height: 20px">Duration：</td>
                            <td bgcolor="#eff6fe" align="left" class="auto-style4">
                                <asp:TextBox ID="txtBeginDate" runat="server" CssClass="Wdate" onClick="WdatePicker()"></asp:TextBox>--<asp:TextBox ID="txtEndDate" runat="server" CssClass="Wdate" onClick="WdatePicker()"></asp:TextBox>
                            </td>
                            <td align="left" bgcolor="#eff6fe" colspan="4" style="height: 22px"><asp:Button ID="btnFind" runat="server" CssClass="Button2"
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
                        CellSpacing="1" CssClass="GridViewStyle" PagingMode="None" Width="100%" DataKeyNames="Id" ShowCheckAll="False" CheckColumnPosition="Left" OnRowDataBound="DataGridView_RowDataBound">
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <Columns>
                            <asp:BoundField DataField="Id" Visible="False" />
                             <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdid" runat="server" Value='<%# Eval("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MessageType" HeaderText="MessageType">
                                <ItemStyle HorizontalAlign="Center" Width="220px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MessageId" HeaderText="MessageId"><ItemStyle HorizontalAlign="Center" Width="280px" />
                                <ItemStyle HorizontalAlign="Center" Width="280px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExceptionInfo" HeaderText="ExceptionInfo">
                                
                            </asp:BoundField>
                            <asp:BoundField DataField="CreatedDt" HeaderText="CreatedDt">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <%--<asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Views/ExceptionLog/ExcetptionLogItem.aspx?ID={0}" DataTextFormatString="Detail" HeaderText="Detail" Text="Detail" Target="_blank">
                                <ItemStyle HorizontalAlign="Center" Width="80px" />
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
                    <cc1:MTCPager ID="DataPager" runat="server" PageNumberColor="#000000" PagerFontColor="#000000" PagerFontFamily="" PagerLeftText=" " OnPageIndexChanged="DataPager_PageIndexChanged">
<PageJump PageJumpType="DropDownList" LeftText="Jump to page" Visiable="False"></PageJump>
                    </cc1:MTCPager>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
