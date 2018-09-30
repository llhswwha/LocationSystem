using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.Work
{
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
    }
}
