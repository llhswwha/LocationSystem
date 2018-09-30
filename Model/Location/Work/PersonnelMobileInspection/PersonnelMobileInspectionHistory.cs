using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.Work
{
    public class PersonnelMobileInspectionHistory
    {
        /// <summary>
        /// Id号
        /// </summary>
        [DataMember]
        [Display(Name = "Id号")]
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 人员Id号
        /// </summary>
        [DataMember]
        [Display(Name = "人员Id号")]
        public int PersonnelId { get; set; }

        /// <summary>
        /// 人员名称
        /// </summary>
        [DataMember]
        [Display(Name = "人员名称")]
        public string PersonnelName { get; set; }

        /// <summary>
        /// 巡检轨迹Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检轨迹Id")]
        public int MobileInspectionId { get; set; }

        /// <summary>
        /// 巡检轨迹名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检轨迹名称")]
        public string MobileInspectionName { get; set; }

        /// <summary>
        /// 计划巡检开始时间
        /// </summary>
        [DataMember]
        [Display(Name = "计划巡检开始时间")]
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 计划巡检结束时间
        /// </summary>
        [DataMember]
        [Display(Name = "计划巡检结束时间")]
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 巡检开始时间
        /// </summary>
        [DataMember]
        [Display(Name = "巡检开始时间")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 巡检结束时间
        /// </summary>
        [DataMember]
        [Display(Name = "巡检结束时间")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 巡检是否超时
        /// </summary>
        [DataMember]
        [Display(Name = "巡检是否超时")]

        public bool bOverTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        [Display(Name = "备注")]

        public string Remark { get; set; }

        /// <summary>
        /// 巡检项
        /// </summary>
        [DataMember]
        [Display(Name = "巡检项")]
        [ForeignKey("PID")]
        public List<PersonnelMobileInspectionItemHistory> list { get; set; }
    }
}
