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
    /// <summary>
    /// 移动巡检列表
    /// </summary>
    public class MobileInspection
    {
        /// <summary>
        /// 巡检轨迹Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检轨迹Id")]
        public int Id { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [DataMember]
        [Display(Name = "顺序")]
        public int nOrder { get; set; }

        /// <summary>
        /// 巡检轨迹名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检轨迹名称")]
        public string Name { get; set; }

        /// <summary>
        /// 操作项
        /// </summary>
        [DataMember]
        [ForeignKey("PID")]
        public virtual List<MobileInspectionItem> Items { get; set; }
    }
}
