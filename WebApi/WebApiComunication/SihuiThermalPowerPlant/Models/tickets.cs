using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.ConvertCodes;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 获取两票列表数据
    /// </summary>
    public class tickets
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 票号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 类型(字典：两票类型，1是操作票，2是工作票)
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 状态(字典：两票状态)
        /// </summary>
        public int? state { get; set; }

        /// <summary>
        /// 设计人员标识列表(数组)
        /// </summary>
        [ByName("WorkerIds")]
        public string worker_ids { get; set; }

        /// <summary>
        /// 设计区域标识列表(数组)
        /// </summary>
        [ByName("ZoneIds")]
        public string zone_ids { get; set; }

        /// <summary>
        /// 原始信息
        /// </summary>
        public string detail { get; set;}

        public tickets Clone()
        {
            tickets copy = new tickets();
            copy.id = this.id;
            copy.code = this.code;
            copy.type = this.type;
            copy.state = this.state;
            copy.worker_ids = this.worker_ids;
            copy.zone_ids = this.zone_ids;
            copy.detail = this.detail;

            return copy;
        }


    }
}
