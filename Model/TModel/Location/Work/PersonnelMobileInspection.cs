using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace TModel.Location.Work
{
    /// <summary>
    /// 人员巡检轨迹表
    /// </summary>
    public class PersonnelMobileInspection
    {
        /// <summary>
        /// Id号
        /// </summary>
        [DataMember]
        //[Display(Name = "Id号")]
        public int Id { get; set; }

        /// <summary>
        /// 人员Id号
        /// </summary>
        [DataMember]
        //[Display(Name = "人员Id号")]
        public int PersonnelId { get; set; }

        /// <summary>
        /// 人员名称
        /// </summary>
        [DataMember]
        //[Display(Name = "人员名称")]
        public string PersonnelName { get; set; }

        /// <summary>
        /// 巡检轨迹Id
        /// </summary>
        [DataMember]
        //[Display(Name = "巡检轨迹Id")]
        public int MobileInspectionId { get; set; }

        /// <summary>
        /// 巡检轨迹名称
        /// </summary>
        [DataMember]
        //[Display(Name = "巡检轨迹名称")]
        public string MobileInspectionName { get; set; }

        /// <summary>
        /// 计划巡检开始时间
        /// </summary>
        [DataMember]
        //[Display(Name = "计划巡检开始时间")]
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 计划巡检结束时间
        /// </summary>
        [DataMember]
        //[Display(Name = "计划巡检结束时间")]
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 巡检开始时间
        /// </summary>
        [DataMember]
        //[Display(Name = "巡检开始时间")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 巡检是否超时
        /// </summary>
        [DataMember]
        //[Display(Name = "巡检是否超时")]

        public bool bOverTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        //[Display(Name = "备注")]

        public string Remark { get; set; }

        /// <summary>
        /// 巡检项
        /// </summary>
        [DataMember]
        //[Display(Name = "巡检项")]
        public List<PersonnelMobileInspectionItem> list { get; set; }

        public PersonnelMobileInspection Clone()
        {
            PersonnelMobileInspection copy = new PersonnelMobileInspection();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
