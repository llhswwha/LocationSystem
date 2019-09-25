using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    //获取告警事件列表
    public class events
    {
        //标识
        public int id { get; set; }

        //标题
        public string title { get; set; }
        
        //内容
        public string msg { get; set; }
        
        //级别(字典：报警事件级别,0 未定，1低，2中，3高)
        public int? level { get; set; }
        
        //代码(各系统上报的原始编码)
        public string code { get; set; } 

        //来源(字典：报警事件来源，0 未知，1 视频监控，2 门禁，3消防，11 SIS，12人员定位)
        public int src { get; set; }

        //设备ID [协议是deviceId实际上是device_id]
        public string device_id { get; set; }

        //设备说明 [协议是deviceDesc实际上是device_desc]
        public string device_desc { get; set; }

        //时间戳
        public long t { get; set; }

        //告警状态，0表示待处理，1表示已处理，2表示忽略
        public int state { get; set; }

        /// <summary>
        /// 原始设备ID
        /// </summary>
        public string raw_id { get; set; }

        /// <summary>
        /// 当接收消防告警时，用node字段对应 设备信息中的code
        /// </summary>
        public string node { get; set; }

        //[协议里面没有]
        public string detail { get; set; }

        //[协议里面没有]
        public string ec_server_name { get; set; }

        //[协议里面没有]
        public string time { get; set; }

        //[协议里面没有]
        public string zone_id { get; set; }

        /*
        {
  "code": 0,
  "detail": "报警控制服务",
  "device_id": 0,
  "ec_server_name": "报警控制服务",
  "id": 0,
  "level": 1,
  "msg": "",
  "raw_id": "53b82015-287f-47bc-8cd6-ca517f043d1a",
  "src": 1,
  "state": 0,
  "t": 1568980773,
  "time": "2019-09-20T19:59:33+08:00",
  "title": "/设备状态/在线",
  "type": "/设备状态/在线",
  "zone_id": 0
}
        */

        /*
{
"cardNum": "",
"code": 10900,
"device_desc": "集控楼0米1号油箱间",
"device_id": 0,
"id": 0,
"level": 1,
"msg": "输入点报警 - 恢复正常",
"node": "",
"raw_id": "0x006FF3A783B6C8DD4766B81BDE01F5BCC45F",
"source": "0x006FF3A783B6C8DD4766B81BDE01F5BCC45F",
"src": 2,
"state": 0,
"t": 1568943698,
"title": "未刷卡强制开门",
"type": "10900",
"zone_id": 0
}
        */

        public events Clone()
        {
            events copy = new events();
            copy.id = this.id;
            copy.title = this.title;
            copy.msg = this.msg;
            copy.level = this.level;
            copy.code = this.code;
            copy.src = this.src;
            copy.device_id = this.device_id;
            copy.device_desc = this.device_desc;
            copy.t = this.t;
            copy.state = this.state;
            copy.raw_id = this.raw_id;

            return copy;
        }
    }
}
