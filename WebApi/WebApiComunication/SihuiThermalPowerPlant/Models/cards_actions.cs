using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 获取门禁卡操作历史
    /// </summary>
    public class cards_actions
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 门禁设备标识
        /// </summary>
        public int device_id { get; set; }

        /// <summary>
        /// 门禁卡号
        /// </summary>
        public string card_code { get; set; }

        /// <summary>
        /// 操作时间，单位 秒
        /// </summary>
        public long? t { get; set; }

        /// <summary>
        /// 结果码(字典：操作状态)，0成功，其他失败
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string description { get; set; }

        public cards_actions Clone()
        {
            cards_actions copy = new cards_actions();
            copy.id = this.id;
            copy.device_id = this.device_id;
            copy.card_code = this.card_code;
            copy.t = this.t;
            copy.code = this.code;
            copy.description = this.description;

            return copy;
        }
    }
}
