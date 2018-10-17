using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    public class device
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// KKS编码
        /// </summary>
        public string kksCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? state { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public int? running_state { get; set; }

        /// <summary>
        /// 是否已指定点位
        /// </summary>
        public bool? placed { get; set; }

        /// <summary>
        /// 原始ID
        /// </summary>
        public string raw_id { get; set; }

        public device Clone()
        {
            device copy = new device();
            copy.id = this.id;
            copy.code = this.code;
            copy.kksCode = this.kksCode;
            copy.name = this.name;
            copy.type = this.type;
            copy.state = this.state;
            copy.running_state = this.running_state;
            copy.placed = this.placed;
            copy.raw_id = this.raw_id;

            return copy;
        }

    }
}
