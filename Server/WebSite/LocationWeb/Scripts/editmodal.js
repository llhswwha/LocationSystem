function saveSuccess(result) {
    if (result.success) {
        //参数设置，若用默认值可以省略以下面代
        toastr.options = {
            closeButton: false, //是否显示关闭按钮
            debug: false, //是否使用debug模式
            positionClass: "toast-top-full-width",//弹出窗的位置
            showDuration: "300",//显示的动画时间
            hideDuration: "1000",//消失的动画时间
            timeOut: "2000", //展现时间
            extendedTimeOut: "1000",//加长展示时间
            showEasing: "swing",//显示时的动画缓冲方式
            hideEasing: "linear",//消失时的动画缓冲方式
            showMethod: "fadeIn",//显示时的动画方式
            hideMethod: "fadeOut" //消失时的动画方式
        };
        toastr.success("修改成功！");
        //$('#myModal').modal('hide');
        $('#myModal').hide('modal');      

        //延时加载页面
        setTimeout(function () {
            location.reload();
        }, 1000);

    } else {
        $.each(result.errors, function (key, val) {
            var container = $('span[data-valmsg-for="' + key + '"]');
            container.removeClass("field-validation-valid").addClass("field-validation-error");
            container.html(val[val.length - 1].ErrorMessage);
        });
    }
}
$(function () {
    //allow validation framework to parse DOM
    $.validator.unobtrusive.parse('form');
});