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

        //设备ID
        public int? deviceId { get; set; }

        //设备说明
        public string deviceDesc { get; set; }

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

        public events Clone()
        {
            events copy = new events();
            copy.id = this.id;
            copy.title = this.title;
            copy.msg = this.msg;
            copy.level = this.level;
            copy.code = this.code;
            copy.src = this.src;
            copy.deviceId = this.deviceId;
            copy.deviceDesc = this.deviceDesc;
            copy.t = this.t;
            copy.state = this.state;
            copy.raw_id = this.raw_id;

            return copy;
        }
    }
}
