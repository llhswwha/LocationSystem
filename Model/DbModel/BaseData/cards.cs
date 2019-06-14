using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.BaseData
{
    /// <summary>
    /// 获取门禁卡列表
    /// </summary>
    public class cards
    {
        [Key]
        public int dbId { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public int cardId { get; set; }

        /// <summary>
        /// 门禁卡号
        /// </summary>
        [MaxLength(256)]
        public string cardCode { get; set; }

        /// <summary>
        /// 当前绑定人员标识
        /// </summary>
        public int? emp_id { get; set; }

        /// <summary>
        /// 卡状态，0 未激活，1激活 使用中，2挂失 暂停
        /// </summary>
        public int state { get; set; }

        public cards Clone()
        {
            cards copy = new cards();
            copy.cardId = this.cardId;
            copy.cardCode = this.cardCode;
            copy.emp_id = this.emp_id;
            copy.state = this.state;

            return copy;
        }
    }
}
