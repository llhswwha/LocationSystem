using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 告警
    /// </summary>
    public class Alarm
    {
        public int Id { get; set; }

        /// <summary>
        /// 告警类型：区域告警，消失告警，低电告警，传感器告警，重启告警，非法采血
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 告警终端
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 告警目标(?)
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 告警内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 告警时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandleTime { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string Handler { get; set; }

        /// <summary>
        /// 处理类型：误报，忽略，确认
        /// </summary>
        public int HandleType { get; set; }
        
    }
}
