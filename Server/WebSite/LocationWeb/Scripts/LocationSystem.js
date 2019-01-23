//获取后台参数
window.onload = function () {
    $.ajax({
        type: "get",
        url: "/Arg/GetLoginInfo",
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
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("失败！");
        }
    });
}

//判断exe程序是否安装
$(function () {
    $("#openExe[href]").click(function (event) {
        //console.log("LocationSystem.js");
        window.protocolCheck($(this).attr("href"),
            function () {
                if (confirm("三维程序未安装,请下载!")) {
                    window.location.href = "/Exe/LocationSystem.exe";
                }
                else {
                    return false;
                }
                //alert("三维程序未安装,请下载!");
            });
        event.preventDefault ? event.preventDefault() : event.returnValue = false;        
    });            
});

//下载三维程序安装包
$(function () {
    $("#downLoading").click(function () {        
        window.location.href = "/Exe/LocationSystem.exe";                    
    });
});

