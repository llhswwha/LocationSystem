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
    public class MobileInspectionDev
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        [DataMember]
        [Key]
        public string DevID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 所有的巡检内容
        /// </summary>
        [DataMember]
        [ForeignKey("ParentDevID")]
        public virtual List<MobileInspectionContent> MobileInspectionContents { get; set; }

    }
}
