using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.LocationHistory.AreaAndDev
{
    /// <summary>
    /// 门禁卡历史表
    /// </summary>
    public class EntranceGuardCardHistory:IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 门禁卡名称
        /// </summary>
        [DataMember]
        [Display(Name = "门禁卡名称")]
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// 门禁卡号
        /// </summary>
        [DataMember]
        [Display(Name = "门禁卡号")]
        [MaxLength(64)]
        public string Code { get; set; }

        /// <summary>
        /// 卡状态，0 未激活，1激活 使用中，2挂失 暂停
        /// </summary>
        [DataMember]
        [Display(Name = "卡状态")]
        public int State { get; set; }

        /// <summary>
        /// 历史记录产生时间
        /// </summary>
        [DataMember]
        [Display(Name = "历史记录产生时间")]
        public DateTime HistoryTime { get; set; }

        /// <summary>
        /// 历史记录时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "历史记录时间戳")]
        public long HistoryTimeStamp { get; set; }
        [DataMember]
        [Display(Name = "描述")]
        public string Description { get; set; }
       
        public EntranceGuardCardHistory Clone()
        {
            EntranceGuardCardHistory copy = new EntranceGuardCardHistory();
            copy = this.CloneObjectByBinary();
            return copy;
        }
    }
}
