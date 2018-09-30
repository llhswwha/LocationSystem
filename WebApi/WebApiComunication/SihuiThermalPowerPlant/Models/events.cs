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
        public int? device_id { get; set; }

        //设备说明
        public string device_desc { get; set; }

        //时间戳
        public long t { get; set; }

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

            return copy;
        }
    }
}
