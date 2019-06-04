function onBegin() {
    //alert('begin');
    $("#myModal").modal({
        //点击背景空白处不被关闭
        backdrop: "static",
        //触发键盘esc事件时不关闭
        keyboard: false
    });   
}

function onSuccess() {
    //alert('success');
}

function onComplete() {
    //alert('complete');   
    //$('#myModal').modal('show');
    $("#myModal").show();
}

function onFailure() {
    //alert('fail');   
}

$("#myModal").each(function () {
    $(this).draggable({
        handle: ".modal-header"   // 只能点击头部拖动
    });
    $(this).css("overflow", "hidden"); // 防止出现滚动条，出现的话，你会把滚动条一起拖着走的    
});