using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 基站分组和概率参数
    /// </summary>
    public class AnchorClassifyProbability
    {
        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 基站
        /// </summary>
        public string Archor { get; set; }

        /// <summary>
        /// 基站状态 (?)
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// ??
        /// </summary>
        public double CondP { get; set; }

        /// <summary>
        /// ??
        /// </summary>
        public double PriorP { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
    }
}
