//获取电厂svg
$.ajax({
    type: "Get",
    url: "/api/areas/svg?Id=2",
    dataType: "html",
    //contentType: "text/html; charset=utf-8",
    async: true,
    success: function (data) {
        //console.log(data)
        $("#showSvg").html(data);
    }
});

var dom = document.getElementById("canvas");
var myChart = echarts.init(dom);
var app = {};
app.timeTicket = setInterval(function () { //定时刷新图表
    initChart();
}, 1000);
option = null;
var symbolSize = 15; //图表点的大小
var data = [];

//添加背景图片
//var img = new Image();
//var canvas = document.createElement('canvas');
//var ctx = canvas.getContext('2d');
//canvas.width = myChart.getWidth() * window.devicePixelRatio;
//canvas.height = myChart.getHeight() * window.devicePixelRatio;
//var backImage = new Image();
//img.onload = function () {
//    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
//    backImage.src = canvas.toDataURL();
//}
//img.src = '../Img/MapPic/PowerPlant.png';

function initChart() {
    $.ajax({
        type: "GET",
        url: "/api/pos",
        dataType: "json",
        success: function (datas) {
            //console.log(datas);
            data.splice(0, datas.length);
            for (var i = 0; i < datas.length; i++) {
                var tag = datas[i].tag;
                var x = datas[i].x;
                var y = datas[i].z;
                data.push([x, y, tag]);
            }

            myChart.setOption({
                title: {
                    text: 'Web实时定位'
                },
                tooltip: {
                    //triggerOn: 'none',
                    formatter: function (params) {
                        return 'tag: ' + params.data[2] + '<br>X: ' + params.data[0].toFixed(2) + '<br>Y: ' + params.data[1].toFixed(2);
                    }
                },
                //调整图表上下左右的边距
                grid: {
                    //top:'20px',
                	left: '0px',
                	//bottom: '0px',
                	//right: '0px'
                },
                xAxis: {
                    min: 2200,
                    max: 2400,
                    type: 'value',
                    //改变X 轴的位置 top\bottom
                    position: 'top',
                    // 控制网格线是否显示
                    splitLine: {
                        show: false
                    },
                    //坐标轴反转
                    inverse: true,
                    //隐藏坐标轴,改变x轴颜色
                    axisLine: {
                        show: false,
                        lineStyle: {
                            color: '#48b'
                        }
                    }
                },
                yAxis: {
                    min: 1573,
                    max: 1817,
                    type: 'value',
                    position: 'right',
                    splitLine: {
                        show: false
                    },
                    inverse: true,
                    axisLine: {
                        show: false,
                        lineStyle: {
                            color: '#48b',
                        }
                    }
                },
                //backgroundColor: {
                //    type: 'pattern',
                //    repeat: 'repeat',
                //    image: backImage,
                //},
                series: [{
                    type: 'scatter',
                    smooth: true,
                    symbolSize: symbolSize,
                    data: data,
                }]
            });
        },
    });
}
window.onresize = myChart.resize;