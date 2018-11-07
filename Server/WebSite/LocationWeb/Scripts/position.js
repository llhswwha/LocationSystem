var dom = document.getElementById("canvas");
var myChart = echarts.init(dom);
var app = {};
app.timeTicket = setInterval(function () { //定时刷新图表
    initChart();
}, 100);
option = null;
var symbolSize = 20; //图表点的大小
var data = [];

//添加背景图片
var img = new Image();
var canvas = document.createElement('canvas');
var ctx = canvas.getContext('2d');
canvas.width = myChart.getWidth() * window.devicePixelRatio;
canvas.height = myChart.getHeight() * window.devicePixelRatio;
var backImage = new Image();
img.onload = function () {
    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    backImage.src = canvas.toDataURL();
}
img.src = '../Img/MapPic/PowerPlant.png';

function initChart() {
    $.ajax({
        type: "GET",
        url: "/api/pos",
        dataType: "json",
        success: function (datas) {
            console.log(datas);
            data.splice(0, datas.length);
            for (var i = 0; i < datas.length; i++) {
                //console.log('数据' + i);
                var tag = datas[i].tag;
                //console.log('tag:' + tag);
                var x = datas[i].x;
                //console.log('x:' + x);
                var y = datas[i].z;
                //console.log('z:' + y);
                data.push([x, y, tag]);
            }

            myChart.setOption({
                title: {
                    text: 'Web实时定位模拟'
                },
                tooltip: {
                    //triggerOn: 'none',
                    formatter: function (params) {
                        return 'tag: ' + params.data[2] + '<br>X: ' + params.data[0].toFixed(2) + '<br>Y: ' + params.data[1].toFixed(2);
                    }
                },
                //调整图表上下左右的边距
                //grid: {
                //	left: '20px',
                //	bottom: '20px',
                //	right: '50px'
                //},
                xAxis: {
                    min: 2200,
                    max: 2400,
                    type: 'value',
                    //改变X 轴的位置 top\bottom
                    position: 'top',

                    // 控制网格线是否显示
                    splitLine: {
                        show: true
                    },
                    //坐标轴反转
                    inverse: true,

                    //隐藏坐标轴,改变x轴颜色
                    axisLine: {
                        show: true,
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
                        show: true
                    },
                    inverse: true,
                    axisLine: {
                        show: true,
                        lineStyle: {
                            color: '#48b',
                        }
                    }
                },
                backgroundColor: {
                    type: 'pattern',
                    repeat: 'repeat',
                    image: backImage,
                },
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