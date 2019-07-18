using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Data
{
    public class PositionJson
    {
        /// <summary>
        /// 定位数据帧此字段为“1”
        /// </summary>
        public string data_type;
        /// <summary>
        /// 定位标签十六进制编号，一般为四位字符串类型
        /// </summary>
        public string tag_id;
        /// <summary>
        /// 定位标签十进制编号
        /// </summary>
        public string tag_id_dec;

        public string x;
        public string y;
        public string z;
        /// <summary>
        /// 实时定位时间戳，毫秒单位
        /// </summary>
        public string timestamp;
        /// <summary>
        /// 实时定位序列号，一般0~255为一个周期
        /// </summary>
        public string sn;
        /// <summary>
        /// 定位标签剩余电量，单位伏特，满电一般为4.2
        /// </summary>
        public string bettery;
        /// <summary>
        /// 当前数据帧关联基站编号，由一个或多个组成
        /// </summary>
        public AnchorJson[] anchors;
        /// <summary>
        /// 标签事件（LOCK、MOVE、SOS、SENSOR、RESTART、DISMANTLE、MOTIONLESS），由一个或多个组成
        /// </summary>
        public EventJson[] events;
    }

    public class AnchorJson
    {
        public string anchor_id;
    }

    public class EventJson
    {
        public string event_type;
    }
}
