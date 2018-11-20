using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 人员巡检轨迹巡检项
    /// </summary>
    public class PersonnelMobileInspectionItem
    {
        /// <summary>
        /// 人员巡检项Id
        /// </summary>
        [DataMember]
        [Display(Name = "人员巡检项Id")]
        public int Id { get; set; }

        /// <summary>
        /// 巡检项Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检项Id")]
        public int ItemId { get; set; }

        /// <summary>
        /// 人员巡检轨迹ID
        /// </summary>
        [DataMember]
        [Display(Name = "人员巡检轨迹ID")]
        public int PID { get; set; }

        /// <summary>
        /// 巡检项名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检项名称")]
        [MaxLength(128)]
        public string ItemName { get; set; }

        /// <summary>
        /// 巡检项顺序
        /// </summary>
        [DataMember]
        [Display(Name = "巡检项顺序")]
        public int nOrder { get; set; }

        /// <summary>
        /// 巡检设备ID
        /// </summary>
        [DataMember]
        [Display(Name = "巡检设备ID")]
        public int DevId { get; set; }


        /// <summary>
        /// 巡检设备名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检设备名称")]
        [MaxLength(128)]
        public string DevName { get; set; }

        /// <summary>
        /// 打卡时间
        /// </summary>
        [DataMember]
        [Display(Name = "打卡时间")]
        public DateTime? PunchTime { get; set; }

        public PersonnelMobileInspectionItem Clone()
        {
            PersonnelMobileInspectionItem copy = new PersonnelMobileInspectionItem();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
