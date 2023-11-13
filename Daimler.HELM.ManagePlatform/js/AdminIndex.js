$(document).ready(function() {
    var firstLi = jQuery("#ChannelMenuItems li:eq(0)");
    if (firstLi.length > 0 && jQuery("#MenuMyDeskTopMore_Triangle").length <= 0) {
         firstLi.find("a").eq(0).append('<span id="MenuMyDeskTopMore_Triangle" style="background-image: url(../images/seg_side.gif);"><img border="0" src="../images/TopQuick01.gif"/></span>');
     }
    if (tID == 'ChannelMenu_MenuMyDeskTop') {
        tID = "";
        ShowHideLayer('ChannelMenu_MenuMyDeskTop');
    }
}
);

function ShowMain(FileName_Left, FileName_Right) {
    var temp;
    if (FileName_Left != "") {
        var checkLeftUrl = CheckCurrentLeftUrl(FileName_Left);
        if (checkLeftUrl == "false") {
            temp = document.getElementById("left");
            temp.src = FileName_Left + GetUrlParm(FileName_Left);
            temp.contentWindow.window.name = "left";
            frames["left"] = temp.contentWindow.window;
        }
    }
    if (FileName_Right != "") {
        temp = document.getElementById("main_right");
        temp.src = FileName_Right + GetUrlParm(FileName_Right);
        temp.contentWindow.window.name = "main_right";
        frames["main_right"] = temp.contentWindow.window;
    }
    InitSideBarState();
}

function CheckCurrentLeftUrl(leftUrl) {
    var src = document.getElementById("left").src;
    var regex = /\s*[\?&]{1,1}t=[0-9]{1,}$/;
    var currentLeftUrl = src.replace(regex, '');
    if (currentLeftUrl.lastIndexOf(leftUrl) >= 0) {
        if (currentLeftUrl.lastIndexOf(leftUrl) == (currentLeftUrl.length - leftUrl.length)) {
            return "true";
        }
    }
    return "false";
}

function GetUrlParm(url) {
    var urlparm = "?";
    if (url.indexOf('?') >= 0) {
        urlparm = "&";
    }
    urlparm = urlparm + "t=" + GetRandomNum();
    return urlparm;
}

function GetRandomNum() {
    var Range = 1000;
    var Rand = Math.random();
    return (Math.round(Rand * Range));
}

function switchSysBar() {
    var obj = document.getElementById("switchPoint");
    if (obj.alt == "关闭左栏") {
        obj.alt = "打开左栏";
        obj.src = "../images/butOpen.jpg";
        document.getElementById("frmTitle").style.display = "none";
        var width, height;
        width = document.body.clientWidth - 12;
        height = document.body.clientHeight - 95;
        $("#main_right").css("height", height);
        $("#main_right").css("width", width);
        $("#FrameTabs").css("width", width);
    }
    else {
        obj.alt = "关闭左栏";
        obj.src = "../images/butClose.gif";
        document.getElementById("frmTitle").style.display = "";
        onload();
    }
    CreateSideBarCookie();
}

var tID = "ChannelMenu_MenuMyDeskTop";

function ShowHideLayer(ID) {
    if (ID != tID) {
        var triangle = document.getElementById("MenuMyDeskTopMore_Triangle");
        if (tID != "") {
            document.getElementById(tID).style.display = "none";
            document.getElementById("A" + tID).style.backgroundImage = "url(../images/digital_left.gif)";
            if (document.getElementById("A" + tID).childNodes.length < 2) {
                document.getElementById("Span" + tID).style.backgroundImage = "url(../images/digital_side.gif)";
            }
             else {
                document.getElementById("Span" + tID).style.backgroundImage = "none";
                if (triangle) {
                     triangle.style.backgroundImage = "none";
                 }
            }
            document.getElementById("Span" + tID).className = "digitaltext";
        }
        document.getElementById(ID).style.display = "";
        document.getElementById("A" + ID).style.backgroundImage = "url(../images/seg_left.gif)";
        if (document.getElementById("A" + ID).childNodes.length < 2) {
            document.getElementById("Span" + ID).style.backgroundImage = "url(../images/seg_side.gif)";
            if (tID != "" && document.getElementById("A" + tID).childNodes.length >= 2) {
                if (triangle) {
                     triangle.style.backgroundImage = "url(../images/digital_side.gif)";
                 }
            }
        }
         else {
            document.getElementById("Span" + ID).style.backgroundImage = "none";
            if (triangle) {
                 triangle.style.backgroundImage = "url(../images/seg_side.gif)";
             }
        }
        document.getElementById("Span" + ID).className = "segtext";
        tID = ID;
    }
}