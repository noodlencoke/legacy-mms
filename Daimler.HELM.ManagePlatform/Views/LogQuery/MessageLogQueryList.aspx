<%@ Page Language="C#"  AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="MessageLogQueryList.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.Views.LogQuery.MessageLogQueryList" %>

<%@ Register assembly="DataGridPageControl" namespace="DataGridPageControl" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=5.0000" />
     <title>Message Log Query Page</title>
    <link type="text/css" rel="stylesheet" href="../StyleCss/FormStyle.css" />
    <link type="text/css" rel="stylesheet" href="../StyleCss/GridViewStyle.css" />
    <link type="text/css" href="../../css/page_style.css" rel="stylesheet" />
    <script   language="javascript" type="text/javascript" src="../JS/My97DatePicker/WdatePicker.js"></script>
     <style type="text/css">
         .auto-style5 {
             width: 557px;
         }
         .Wdate {}
         .auto-style6 {
             height: 22px;
         }
     </style>
    <script language="javascript" type="text/javascript">
        function doclick(id, messagetype)
        {
            window.open("MessageLogQueryItem.aspx?ID=" + id + "&MessageType="+messagetype,'_blank');
        }
        function onSelectChanged()
        {
            var dataSource = document.getElementById("txtMessageSource");
            var obj = document.getElementById("txtMessageType");
            var index = obj.selectedIndex;
            var selectvalue = obj.options[index].value;
            if (selectvalue == '3' || selectvalue == '4' || selectvalue == '5') {
                dataSource.options.length = 0;
                var op = document.createElement("OPTION");
                op.text = "---";
                op.value = "5";
                dataSource.options.add(op);
                dataSource.disabled = true;

            }
            else {
                dataSource.options.length = 0;
                dataSource.options.add(new Option("EP","0"));
                dataSource.options.add(new Option("POS","1"));
                dataSource.options.add(new Option("TSB","2"));
                dataSource.options.add(new Option("LMS", "3"));
                dataSource.options.add(new Option("MeStore", "4"));
                dataSource.disabled = false;
                dataSource.selectedIndex = 0;

            }
        }
        function onloadpage()
        {
            var dataSource = document.getElementById("txtMessageSource");
            var obj = document.getElementById("txtMessageType");
            var index = obj.selectedIndex;
            var selectvalue = obj.options[index].value;
            if (selectvalue == '3' || selectvalue == '4' || selectvalue == '5') {
                dataSource.options.length = 0;
                var op = document.createElement("OPTION");
                op.text = "---";
                op.value = "5";
                dataSource.options.add(op);
                dataSource.disabled = true;

            }
        }
    </script>

</head>


<body>
    <form id="form1" runat="server" >
     <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
            <tr>
                <td>
                    <br />
                    <p>
                        <asp:Label ID="Label1" runat="server" CssClass="PopWindowTitle">Message Log Query Page</asp:Label>
                    </p>
                </td>
            </tr>
            <tr>
                <td bgcolor="#3a6fb5">
                    <img height="3" alt="" src="" width="2" name="" /></td>
            </tr>
        </table>
  <table class="Noprint" width="95%" align="center">
      <tr style="border:0px;">
          <td class="auto-style6"> <asp:Label ID="txtTip" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label></td>
      </tr>
            <tr>
                <td align="center" style="height: 87px">
                    <table bordercolor="#8aafe6" cellspacing="0" bordercolordark="#ffffff" cellpadding="0"
                        width="100%" align="center" bgcolor="#ffffff" border="1">
                        <tr>
                            <td align="right" bgcolor="#eff6fe" class="auto-style1">
                                MessageType：</td>
                            <td align="left" class="auto-style5" style="color: #FF0000">
                                <asp:DropDownList ID="txtMessageType" runat="server" CssClass="DropDownList" onchange="onSelectChanged();">
                                    <asp:ListItem Value="0">SMS</asp:ListItem>
                                    <asp:ListItem Value="1">MMS</asp:ListItem>
                                    <asp:ListItem Value="2">Email</asp:ListItem>
                                </asp:DropDownList>
                                *</td>
                            <td align="right" bgcolor="#eff6fe" class="auto-style3">
                                MessageSource：</td>
                            <td align="left" class="auto-style4" style="color: #FF0000">
                                <asp:DropDownList ID="txtMessageSource"  runat="server" CssClass="DropDownList">
                                    <asp:ListItem Value="0">EP</asp:ListItem>
                                    <asp:ListItem Value="1">POS</asp:ListItem>
                                    <asp:ListItem Value="2">TSB</asp:ListItem>
                                    <asp:ListItem Value="3">LMS</asp:ListItem>
                                    <asp:ListItem Value="4">MeStore</asp:ListItem>
                                </asp:DropDownList>
                                *</td>
                        </tr>
                        <tr>
                            <td align="right" bgcolor="#eff6fe" style="width: 130px; height: 20px">
                                Duration：</td>
                            <td align="left" class="auto-style5" style="color: #FF0000">
                                <asp:TextBox ID="txtBeginDate" runat="server" CssClass="Wdate" onClick="WdatePicker()"></asp:TextBox>--<asp:TextBox ID="txtEndDate" runat="server" CssClass="Wdate" onClick="WdatePicker()"></asp:TextBox>
                               
                                *</td>
                             <td align="right" bgcolor="#eff6fe" class="auto-style3">
                                 KeyWord：</td> 
                             <td align="left" class="auto-style4">

                                 <asp:TextBox ID="txtKeyWord" runat="server" CssClass="TextBox" Width="173px"></asp:TextBox>
                            </td>
                               
                        </tr>
                     
                        <tr>
                            <td align="center" colspan="4" style="height: 22px">
                                &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="btnFind" runat="server" CssClass="Button2" OnClick="btnFind_Click" 
                        Text="Find" Width="50px" /><font face="宋体">&nbsp;</font><font face="宋体">&nbsp;&nbsp;</font><font
                                    face="宋体"></font>
                                </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div >
       <table class="Noprint"  width="95%" align="center">
            <tr>
                <td >
                    <cc1:MTCGridView ID="DataGridView" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CellSpacing="1" CssClass="GridViewStyle" PagingMode="None"  Width="100%" DataKeyNames="Id"  ShowCheckAll="False" OnRowDataBound="DataGridView_RowDataBound">
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                       <Columns>
                            <asp:BoundField DataField="DataSource" HeaderText="DataSource" >
                                 <HeaderStyle Width="80px" />
                                 <itemstyle horizontalalign="Center" />
                                </asp:BoundField>
                            <asp:BoundField DataField="CreateDate" HeaderText="CreateDate" >
                                  <HeaderStyle Width="140px" />
                                  <itemstyle horizontalalign="Center" />
                                 </asp:BoundField>
                            <asp:BoundField DataField="SendContent" HeaderText="SendContent" >
                               <HeaderStyle Wrap="True" />
                               <itemstyle horizontalalign="Left" Wrap="True"/>
                            </asp:BoundField>
                           <asp:BoundField DataField="RequestInfo" HeaderText="RequestInfo">
                               <HeaderStyle  Wrap="True" />
                               <itemstyle horizontalalign="Left" Wrap="True"/>
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
                    <cc1:MTCPager ID="DataPager" runat="server" PageNumberColor="#000000" PagerFontColor="#000000" PagerFontFamily="" PagerLeftText=" " Visible="False" OnPageIndexChanged="MTCPager1_PageIndexChanged" IsShowPageTotalCount="False">
<PageJump PageJumpType="TextBox" LeftText="go to page" Visiable="False" ButtonText="go "></PageJump>
                    </cc1:MTCPager>
                </td>
            </tr>
        </table>
       </div>
       </form>
</body>
    
   <script language="javascript" type="text/javascript">

       window.onload = onloadpage;
       </script>
</html>


