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
    /// 定位卡与人员之间的历史关系
    /// </summary>
    public class LocationCardToPersonnelHistory:IId
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
        /// 定位卡
        /// </summary>
        [DataMember]
        [Display(Name = "定位卡")]
        public int LocationCardId { get; set; }

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

        public LocationCardToPersonnelHistory Clone()
        {
            LocationCardToPersonnelHistory copy = new LocationCardToPersonnelHistory();
            copy = this.CloneObjectByBinary();
            
            return copy;
        }
    }
}
