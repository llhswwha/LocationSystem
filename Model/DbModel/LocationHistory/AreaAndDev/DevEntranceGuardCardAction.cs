using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.LocationHistory.AreaAndDev
{
    /// <summary>
    /// 设备、门禁卡操作历史
    /// </summary>
    public class DevEntranceGuardCardAction : IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        [DataMember]
        [Display(Name = "设备ID")]
        public int DevInfoId { get; set; }

        /// <summary>
        /// 门禁卡
        /// </summary>
        [DataMember]
        [Display(Name = "门禁卡")]
        public int EntranceGuardCardId { get; set; }

        /// <summary>
        /// 操作时间，单位 秒
        /// </summary>
        [DataMember]
        [Display(Name = "操作时间")]
        public DateTime? OperateTime { get; set; }

        /// <summary>
        /// 操作时间戳，单位毫秒
        /// </summary>
        [DataMember]
        [Display(Name = "操作时间戳")]
        public long? OperateTimeStamp { get; set; }

        /// <summary>
        /// 结果码(字典：操作状态)，0成功，其他为失败
        /// </summary>
        [DataMember]
        [Display(Name = "结果码")]
        public int code { get; set; }

        /// <summary>
        /// 出入状态，0 表示进入，1 表示出去
        /// </summary>
        [DataMember]
        [Display(Name = "出入状态")]
        public int nInOutState { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [DataMember]
        [Display(Name = "说明")]
        [MaxLength(128)]
        public string description { get; set; }

        public DevEntranceGuardCardAction Clone()
        {
            return this.CloneObjectByBinary();
        }

    }
}
