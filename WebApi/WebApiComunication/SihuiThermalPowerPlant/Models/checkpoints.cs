using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 巡检点列表
    /// </summary>
    public class checkpoints
    {
        /// <summary>
        /// 巡检员工号
        /// </summary>
        public string staffCode { get; set; }

        /// <summary>
        /// 巡检员名称
        /// </summary>
        public string staffName { get; set; }

        /// <summary>
        /// 设备KKD编码
        /// </summary>
        public string kksCode { get; set; }

        /// <summary>
        /// 移动巡检系统中定义的设备编码
        /// </summary>
        public string deviceCode { get; set; }

        public string deviceName { get; set;}

        /// <summary>
        /// 移动巡检系统中定义的设备ID
        /// </summary>
        public string deviceId { get; set; }

        ///// <summary>
        ///// 检查是否通过
        ///// </summary>
        //public bool success { get; set; }

        ///// <summary>
        ///// 检查项列表
        ///// </summary>
        //public List<results> checks { get; set; }

    }
}
