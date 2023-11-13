<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Daimler.HELM.ManagePlatform.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="IE=5.0000" http-equiv="X-UA-Compatible"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="GENERATOR" content="MSHTML 10.00.9200.16921"/>
    <title>Daimler HELM Manage platform </title>
    <link href="css/index.css" rel="stylesheet" type="text/css"/>
    <link href="css/MasterPage.css" rel="stylesheet" type="text/css"/>
    <link href="css/Guide.css" rel="stylesheet" type="text/css"/>

    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/AdminIndex.js" type="text/javascript"></script>

</head>
<body id="Indexbody" onload="onload();">
    <script type="text/JavaScript">
        function show(id) {
            var obj;
            obj = document.getElementById('PopMenu_' + id);
            obj.style.visibility = "visible";
        }
        function hide(id) {
            var obj;
            obj = document.getElementById('PopMenu_' + id);
            obj.style.visibility = "hidden";
        }
        function hideOthers(id) {
            var divs;
            if (document.all) {
                divs = document.all.tags('DIV');
            }
            else {
                divs = document.getElementsByTagName("DIV");
            }
            for (var i = 0 ; i < divs.length; i++) {
                if (divs[i].id != 'PopMenu_' + id && divs[i].id.indexOf('PopMenu_') >= 0) {
                    divs[i].style.visibility = "hidden";
                }
            }
        }
        function onload() {
            var width = document.body.clientWidth - 207;
            var lHeight = document.body.clientHeight - 78;
  
            var rHeight = lHeight - (jQuery("#FrameTabs").height() || 0);
            var currentWidth = width > 0 ? width : 0;
            var currentHeight = lHeight > 0 ? lHeight : 0;
            $("#main_right").css("width", currentWidth);
            $("#main_right").css("height", (currentHeight-25));
            $("#left").css("height", currentHeight);
            jQuery("#FrameTabs").width(width);
           
        }
        window.onresize = onload;
        function InitSideBarState() {
            var existentSideBarCookie = getCookie("SideBarCookie");
            var SideBarKey = document.getElementById("left").src.substring(document.getElementById("left").src.lastIndexOf('/') + 1, document.getElementById("left").src.lastIndexOf('.'));
            if (existentSideBarCookie.length != 0 && SideBarKey.length != 0 && existentSideBarCookie.indexOf(SideBarKey) != -1) {
                var arrKV = existentSideBarCookie.split("&");
                for (var v in arrKV) {
                    if (arrKV[v].indexOf(SideBarKey) != -1) {
                        var currentValue = arrKV[v].split("=");
                        ChangeSideBarState(currentValue[1]);
                    }
                }
            }
            else {
                var obj = document.getElementById("switchPoint");
                obj.alt = "关闭左栏";
                obj.src = "images/butClose.gif";
                document.getElementById("frmTitle").style.display = "block";
                onload();
            }
        }
        function ChangeSideBarState(temp) {
            var obj = document.getElementById("switchPoint");
            if (temp == "none") {
                obj.alt = "打开左栏";
                obj.src = "images/butOpen.jpg";
                document.getElementById("frmTitle").style.display = "none";
                var width, height;
                width = document.body.clientWidth - 12;
                height = document.body.clientHeight - 95;
                $("#main_right").css("height",height);
                $("#main_right").css("width", width);
                $("#FrameTabs").css("width", width);
            }
            else {
                obj.alt = "关闭左栏";
                obj.src = "images/butClose.gif";
                document.getElementById("frmTitle").style.display = "block";
                onload();
            }
        }
        function killErrors() {
            return true;
        }
        window.onerror = killErrors;

        function AdminOut()
        {
            alert("Quit Success!");
        }
    </script>

    <table border="0" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td colspan="3">
                    <div id="content">
                        <ul id="ChannelMenuItems" style="text-align:center;">
                           <div style="font-size:20px; font-weight:bold;">Daimler HELM Manage Platform</div>
                        </ul>
                        <div id="SubMenu">
                            <div id="ChannelMenu_" style="width: 100%; display: none;">
                                <ul></ul>
                            </div>
                            <div id="ChannelMenu_MenuMyDeskTop" style="width: 100%;">
                                <ul>
                                    <li>Current User：<span id="spanUser" runat="server"></span></li>
                                    <li>
                                        
                                    </li>
                                </ul>
                            </div>
                            <div id="ChannelMenu_News" style="width: 100%; display: none;">
                                <ul><!--<li></li>-->               </ul>
                            </div>
                            <div id="ChannelMenu_Product" style="width: 100%; display: none;">
                                <ul><!--<li></li>-->               </ul>
                            </div>
                            <div id="ChannelMenu_DownLoad" style="width: 100%; display: none;">
                                <ul><!--<li></li>-->               </ul>
                            </div>
                            <div id="ChannelMenu_Talent" style="width: 100%; display: none;">
                                <ul><!--<li></li>-->               </ul>
                            </div>
                            <div id="ChannelMenu_Other" style="width: 100%; display: none;">
                                <ul><!--<li></li>-->               </ul>
                            </div>
                            <div id="ChannelMenu_Feedback" style="width: 100%; display: none;">
                                <ul><!--<li></li>-->               </ul>
                            </div>
                            <div id="ChannelMenu_User" style="width: 100%; display: none;">
                                <ul><!--<li></li>-->               </ul>
                            </div>
                            <div id="ChannelMenu_Html" style="width: 100%; display: none;">
                                <ul><!--<li></li>-->               </ul>
                            </div>
                        </div>
                        <div id="Announce">
                            <a href="index.html" target="main_right">
                                <img width="37" height="14" alt="HOME PAGE" src="images/Home.gif" border="0"/>
                            </a>
                            <a href="javascript:AdminOut()">
                                <img width="37" height="14" alt="Quit" src="images/Exit.gif" border="0"/>
                            </a>
                        </div>
                    </div>
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td id="frmTitle">
                    <iframe name="left" id="left" src="Admin_Index_Left.htm"
                            frameborder="0" scrolling="auto" style="width: 195px; height: 800px; visibility: inherit; z-index: 2;"
                            tabid="1"></iframe>
                </td>
                <td class="but" onclick="switchSysBar();">
                    <img id="switchPoint" style="border: 0px currentColor; width: 12px;"
                         alt="关闭左栏" src="images/butClose.gif">
                </td>
                <td>
                    <div id="FrameTabs" style="overflow: hidden;"></div>
                    <div id="main_right_frame">
                        <iframe name="main_right" id="main_right" src="Index.html"
                                frameborder="0" scrolling="yes" style="width: 1380px; height: 800px; visibility: inherit; z-index: 2; -ms-overflow-x: hidden;"
                               tabid="1"></iframe>
                        <div class="clearbox2"></div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</body>
</html>
