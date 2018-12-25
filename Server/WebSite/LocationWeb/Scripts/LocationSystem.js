window.onload = function () {
    $.ajax({
        type: "get",
        url: "Arg/GetLoginInfo",
        //data: 'locations',
        contentType: "text/html; charset=utf-8",
        //dataType: "json",
        async: false,
        success: function (data) {
            console.log(data);
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
}

$(function () {
    $("#exe[href]").click(function (event) {

        console.log("LocationSystem.js");
        window.protocolCheck($(this).attr("href"),
            function () {
                var downloadEXE = window.confirm("exe程序未安装,是否下载安装包!")
                if (downloadEXE) {
                    window.location.href = "/Exe/LocationSystem.exe";

                    //$.ajax({
                    //    type: "get",
                    //    url: "Arg/GetDownloadLink?relativePath=/Exe/LocationSystem.exe",                   
                    //    contentType: "text/html; charset=utf-8",
                    //    //dataType: "json",
                    //    async: false,
                    //    success: function (data) {
                    //        console.log(data);
                    //        window.location.href = data;                            
                    //    },
                    //    error: function (XMLHttpRequest, textStatus, errorThrown) {                           
                    //        alert("失败！");
                    //    }
                    //});
                } 
                //alert("exe程序未安装,请下载!");                               
            });
        event.preventDefault ? event.preventDefault() : event.returnValue = false;        
    });            
});


