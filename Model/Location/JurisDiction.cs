using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    public enum JurisDictionType { 按时间长度设置权限, 按时间点范围设置权限 }
    public class JurisDiction
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "名称")]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "描述")]
        public string Describe { get; set; }

        /// <summary>
        /// 0 表示按时间长度设置权限，1 表示按时间点范围设置权限
        /// </summary>
        [DataMember]
        [Display(Name = "权限种类")]
        public JurisDictionType nFlag { get; set; }

        [DataMember]
        [Display(Name = "权限起始时间点")]
        public DateTime? StartTime { get; set; }

        [DataMember]
        [Display(Name = "权限结束时间点")]
        public DateTime? EndTime { get; set; }

        [DataMember]
        [Display(Name = "权限时长")]
        public int? nTimeLength { get; set; }

        [DataMember]
        [Display(Name = "延迟时间")]
        public int DelayTime { get; set; }

        [DataMember]
        [Display(Name = "误差距离")]
        public int ErrorDistance { get; set; }

        [DataMember]
        [Display(Name = "重复天数")]
        [Required]
        public string RepeatType { get; set; }

        [DataMember]
        [Display(Name = "区域")]
        public int PtId { get; set; }
        public virtual PhysicalTopology Pt { get; set; }

        [DataMember]
        [Display(Name = "标签卡")]
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }

        [DataMember]
        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        [DataMember]
        [Display(Name = "修改时间")]
        public DateTime? ModifyTime { get; set; }


        [DataMember]
        [Display(Name = "删除时间")]
        public DateTime? DeleteTime { get; set; }

    }
}
