using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.Work
{

    public class MobileInspectionContent
    {
        /// <summary>
        /// 巡检内容Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检内容Id")]
        public int Id { get; set; }

        /// <summary>
        /// 所属设备Id
        /// </summary>
        [DataMember]
        [Display(Name = "所属设备Id")]
        public string ParentDevID { get; set; }

        /// <summary>
        /// 所属巡检内容
        /// </summary>
        [DataMember]
        [Display(Name = "所属巡检内容")]
        public string Content { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [DataMember]
        [Display(Name = "顺序")]
        public int nOrder { get; set; }
    }
}
