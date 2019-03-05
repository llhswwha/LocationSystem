using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// SIS采样历史数据
    /// </summary>
    public class sis_sampling
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 级别，字典：报警事件级别，0未定，1低，2中，3高
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 代码，各系统上报的原始编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 来源，字典：报警事件来源
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public int device_id { get; set; }

        /// <summary>
        /// 设备说明
        /// </summary>
        public string device_desc { get; set; }

        /// <summary>
        /// 时间戳，PEOCH，到秒
        /// </summary>
        public long t { get; set; }
    }
}
