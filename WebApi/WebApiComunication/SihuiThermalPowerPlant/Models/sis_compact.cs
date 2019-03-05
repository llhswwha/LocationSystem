using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 紧凑型sis数据
    /// </summary>
    public class sis_compact
    {
        /// <summary>
        /// KKS编码
        /// </summary>
        public string kks { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 字段名数组
        /// </summary>
        public List<string> fields { get; set; }

        public sis_compact()
        {
            kks = "";
            unit = "";
            fields = new List<string>();
        }
    }


}
