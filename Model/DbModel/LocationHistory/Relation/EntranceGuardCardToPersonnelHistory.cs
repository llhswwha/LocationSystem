using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.LocationHistory.Relation
{
    /// <summary>
    /// 门禁卡与人员之间的历史关系
    /// </summary>
    public class EntranceGuardCardToPersonnelHistory:IId
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
        /// 门禁卡
        /// </summary>
        [DataMember]
        [Display(Name = "门禁卡")]
        public int EntranceGuardCardId { get; set; }
      
        /// <summary>
        /// 人员
        /// </summary>
        [DataMember]
        [Display(Name = "人员")]
        public int PersonnelId { get; set; }

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

        public EntranceGuardCardToPersonnelHistory Clone()
        {
            EntranceGuardCardToPersonnelHistory copy = new EntranceGuardCardToPersonnelHistory();
            copy = this.CloneObjectByBinary();
            
            return copy;
        }
    }
}
