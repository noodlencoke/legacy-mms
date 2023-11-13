<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="InterfaceAccountDisplay.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.Views.InterfaceAccount.InterfaceAccountDisplay" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Interface Account</title>
    <link type="text/css" rel="stylesheet" href="../StyleCss/FormStyle.css" />
    <link type="text/css" rel="stylesheet" href="../StyleCss/GridViewStyle.css" />
    <link type="text/css" href="../../css/page_style.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="../JS/My97DatePicker/WdatePicker.js"></script>
    <style type="text/css">
        .auto-style5 {
            width: 89px;
            height: 20px;
        }

        .auto-style6 {
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h3>Interface Account Manage</h3>
        <asp:Label ID="InterfaceAccountLabel" runat="server" Text="Account List" Font-Bold="True"></asp:Label>
        <div>
        </div>
        <asp:GridView ID="InterfaceAccountGridView" runat="server" CellPadding="3" CellSpacing="1" CssClass="GridViewStyle" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowEditing="InterfaceAccountGridView_RowEditing" OnRowUpdating="InterfaceAccountGridView_RowUpdating" OnRowCancelingEdit="InterfaceAccountGridView_RowCancelingEdit" OnRowDeleting="InterfaceAccountGridView_RowDeleting" DataKeyNames="Id" OnRowDataBound="InterfaceAccountGridView_RowDataBound">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <FooterStyle CssClass="GridViewFooterStyle" />
            <HeaderStyle CssClass="GridViewHeaderStyle" />
            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
            <RowStyle CssClass="GridViewRowStyle" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                <asp:BoundField DataField="accountName" HeaderText="Account Name" />
                <asp:BoundField DataField="accountPwd" HeaderText="Account Password" />
                <asp:BoundField DataField="effectiveDt" HeaderText="Effective Time" />
                <asp:BoundField DataField="effectiveDays" HeaderText="Effective Days" />
                <asp:BoundField DataField="dataSource" HeaderText="DataSource" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server"><%# ShowStatusDesc(Eval("status")) %></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem Text="Active"  Value="0"></asp:ListItem>
                            <asp:ListItem Text="Disable" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdStatus" runat="server" Value='<%# Eval("status") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="expireDate" HeaderText="Expire Time" />
                <asp:BoundField DataField="remainingDays" HeaderText="Remaining Days" />
                <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButtonUpdate" runat="server" CommandName="Update" Text="Update"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButtonCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DELETE" ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you confirm delete this account？deleted will can not recover.')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <asp:Label ID="InterfaceAccountAddLabel" runat="server" Text="Add" Font-Bold="True"></asp:Label>
        <table border="1" style="width: 100%;">
            <tr>
                <td class="auto-style5">Account Name：</td>
                <td class="auto-style6">
                    <asp:TextBox ID="TextBoxAccountName" runat="server" CssClass="TextBox" Width="280px"></asp:TextBox>*
                </td>
            </tr>
            <tr>
                <td class="auto-style5">Account Password：</td>
                <td>
                    <asp:TextBox ID="TextBoxAccountPwd" runat="server" TextMode="Password" CssClass="TextBox" Width="280px"></asp:TextBox>*
                </td>
            </tr>
            <tr>
                <td class="auto-style5">Confirm Account Password：</td>
                <td>
                    <asp:TextBox ID="TextBoxAccountPwdConfirm" runat="server" TextMode="Password" CssClass="TextBox" Width="280px"></asp:TextBox>*
                </td>
            </tr>
            <tr>
                <td class="auto-style5">Effective Time：</td>
                <td>
                    <asp:TextBox ID="TextBoxEffectiveDt" runat="server" onClick="WdatePicker()" CssClass="TextBox" Width="280px"></asp:TextBox>
                    (formate:2015-10-01)*</td>
            </tr>
            <tr>
                <td class="auto-style5">Effective Days：</td>
                <td class="auto-style1">
                    <asp:TextBox ID="TextBoxEffectiveDays" runat="server" CssClass="TextBox" Width="280px"></asp:TextBox>*
                </td>
            </tr>
            <tr>
                <td class="auto-style5">Data Source：</td>
                <td class="auto-style1">
                    <asp:TextBox ID="TextBoxDataSource" runat="server" CssClass="TextBox" Width="280px"></asp:TextBox>*
                </td>
            </tr>
            <tr>
                 <td class="auto-style5"></td>
                 <td class="auto-style1">
                     <asp:Label ID="lblErrorMsg" runat="server" BackColor="Red"></asp:Label>
                 </td>
            </tr>
            <tr>
                <td class="auto-style5"></td>
                <td class="auto-style1">
                    <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAdd_Click" Text="Add Account" CssClass="Button2" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
