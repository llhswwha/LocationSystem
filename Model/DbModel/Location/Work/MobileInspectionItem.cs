using DbModel.Tools;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 移动巡检轨迹下的巡检项
    /// </summary>
    public class MobileInspectionItem
    {
        /// <summary>
        /// 巡检项Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检项Id")]
        public int Id { get; set; }


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
        /// 巡检轨迹ID
        /// </summary>
        [DataMember]
        [Display(Name = "巡检轨迹ID")]
        public int PID { get; set; }


        /// <summary>
        /// 巡检设备Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检设备Id")]
        public int DevId { get; set; }


        /// <summary>
        /// 巡检设备名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检设备名称")]
        [MaxLength(128)]
        public string DevName { get; set; }

        public MobileInspectionItem Clone()
        {
            MobileInspectionItem copy = new MobileInspectionItem();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
