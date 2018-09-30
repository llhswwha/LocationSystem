using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 获取SIS传感数据
    /// </summary>
    public class sis
    {
        /// <summary>
        /// KKS编码
        /// </summary>
        public string kks { get; set; }

        /// <summary>
        /// 值，没找到数据时为null
        /// </summary>
        public string value { get; set; }

        public sis Clone()
        {
            sis copy = new sis();
            copy.kks = this.kks;
            copy.value = this.value;

            return copy;
        }
    }
}
