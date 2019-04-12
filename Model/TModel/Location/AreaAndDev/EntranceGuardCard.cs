using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    /// <summary>
    /// 门禁卡列表
    /// </summary>
    [DataContract]
    public class EntranceGuardCard
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 门禁卡号
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 卡状态，0 未激活，1激活 使用中，2挂失 暂停
        /// </summary>
        [DataMember]
        public int State { get; set; }
    }
}
