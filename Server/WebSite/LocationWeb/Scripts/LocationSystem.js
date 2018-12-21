$(function () {
    $("#exe[href]").click(function (event) {
        $.ajax({
            type: "get",
            url: "Arg/GetLoginInfo",
            //data: 'locations', //data 表示发送到服务端的数据，键值对或字符串
            contentType: "text/html; charset=utf-8",
            //dataType: "json",
            async: false,
            success: function (data) {
                //console.log(data);
                $(function () {
                    $("#exe").attr("href", "LocationSystem:");
                    //当前href
                    var location = $("#exe").attr("href");
                    //console.log(location);
                    //连接字段
                    var newStr = location.concat(data);
                    //console.log(newStr);
                    $("#exe").attr("href", newStr);
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //console.log("error");
                //console.log(XMLHttpRequest.status);
                //console.log(XMLHttpRequest.readyState);
                //console.log(textStatus);
                alert("失败！");
            }
        });
       
        //console.log("LocationSystem.js");
        window.protocolCheck($(this).attr("href"),
            function () {
                var downloadEXE = window.confirm("exe程序未安装,是否下载安装包!")
                if (downloadEXE) {
                    //window.alert("下载");
                    window.open("http://localhost:53506/../Exe/LocationSystem.exe");           
                } else {
                    //window.alert("取消");
                    return false;
                }
                    
                //alert("exe程序未安装,请下载!");                
                //window.open("http://localhost:53506/../Exe/LocationSystem.exe");
            });
        event.preventDefault ? event.preventDefault() : event.returnValue = false;
    });            
});


