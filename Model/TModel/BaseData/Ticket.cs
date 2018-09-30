using Location.TModel.ConvertCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.BaseData
{
    /// <summary>
    /// 两票
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 票号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 类型(字典：两票类型，1是操作票，2是工作票)
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 状态(字典：两票状态)
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 设计人员标识列表(数组)
        /// </summary>
        [ByName("worker_ids")]
        public string WorkerIds { get; set; }

        /// <summary>
        /// 设计区域标识列表(数组)
        /// </summary>
        [ByName("zone_ids")]
        public string ZoneIds { get; set; }

        /// <summary>
        /// 原始信息
        /// </summary>
        public string Detail { get; set; }
    }
}
