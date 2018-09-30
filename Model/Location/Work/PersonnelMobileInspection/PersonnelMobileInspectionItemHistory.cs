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
    public class PersonnelMobileInspectionItemHistory
    {
        /// <summary>
        /// 人员巡检项Id
        /// </summary>
        [DataMember]
        [Display(Name = "人员巡检项Id")]
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
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
        public string DevID { get; set; }


        /// <summary>
        /// 巡检设备名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检设备名称")]
        public string DevName { get; set; }

        /// <summary>
        /// 打卡时间
        /// </summary>
        [DataMember]
        [Display(Name = "打卡时间")]
        public DateTime? PunchTime { get; set; }
    }
}
