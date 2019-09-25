using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    public class checkresults
    {
        /// <summary>
        /// 检查是否通过
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 检查项列表
        /// </summary>
        public List<results> checks { get; set; }
    }
}
