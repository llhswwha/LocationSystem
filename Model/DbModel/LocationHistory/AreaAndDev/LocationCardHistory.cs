using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.LocationHistory.AreaAndDev
{
    /// <summary>
    /// 定位卡历史表
    /// </summary>
    public class LocationCardHistory
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
        /// 终端编号
        /// </summary>
        [DataMember]
        [Display(Name = "终端编号")]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        [DataMember]
        [Display(Name = "终端名称")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        [Display(Name = "描述")]
        public string Describe { get; set; }

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

        public LocationCardHistory Clone()
        {
            LocationCardHistory copy = new LocationCardHistory();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
