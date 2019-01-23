using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 巡检轨迹列表
    /// </summary>
    public class patrols
    {
        /// <summary>
        /// 巡检单Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 巡检单编号，移动巡检系统中的唯一编号或名称
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 巡检路线名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 巡检点列表
        /// </summary>
        public List<checkpoints> route { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long createTime { get; set; }

        /// <summary>
        /// 巡检状态，新建；已下达 ；已完成；已取消；执行中；已过期
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long startTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long endTime { get; set; }
    }
}
