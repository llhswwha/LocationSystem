//获取后台参数
window.onload = function () {
    $.ajax({
        type: "get",
        url: "/Arg/GetLoginInfo",//获取登陆信息 加到链接中
        //data: 'locations',
        contentType: "text/html; charset=utf-8",
        //dataType: "json",
        async: false,
        success: function (data) {
            $(function () {
                $("#openExe").attr("href", "LocationSystem:");
                //当前href
                var location = $("#openExe").attr("href");                
                //连接字段
                var newStr = location.concat(data);
                $("#openExe").attr("href", newStr);
                //最终页面上的内容是 <a id="openExe" href="LocationSystem:192.168.1.16|8733|admin|admin">启动</a>
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("失败！");
        }
    });
    $.ajax({
        type: "get",
        url: "/Arg/GetLoginInfo_Guest",//获取登陆信息 加到链接中
        //data: 'locations',
        contentType: "text/html; charset=utf-8",
        //dataType: "json",
        async: false,
        success: function (data) {
            $(function () {
                $("#openExe_guest").attr("href", "LocationSystem:");
                //当前href
                var location = $("#openExe_guest").attr("href");
                //连接字段
                var newStr = location.concat(data);
                $("#openExe_guest").attr("href", newStr);
                //最终页面上的内容是 <a id="openExe" href="LocationSystem:192.168.1.16|8733|admin|admin">启动</a>
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("失败！");
        }
    });
}

////判断exe程序是否安装
//$(function () {
//    $("#openExe[href]").click(function (event) {
//        //console.log("LocationSystem.js");
//        window.protocolCheck($(this).attr("href"),
//            function () {
//                if (confirm("三维程序未安装,请下载!")) {
//                    window.location.href = "/Exe/LocationSystem.exe";
//                }
//                else {
//                    return false;
//                }
//                //alert("三维程序未安装,请下载!");
//            });
//        event.preventDefault ? event.preventDefault() : event.returnValue = false;        
//    });            
//});

//下载三维程序安装包
$(function () {
    $("#downLoading").click(function () {        
        window.location.href = "/Exe/LocationSystem.exe";  
        //这里用其他平台集成的话要改成http://172.16.100.26/Exe/LocationSystem.exe
    });
});

