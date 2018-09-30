using DbModel.Tools;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace TModel.Location.Work
{
    /// <summary>
    /// 需要移动巡检的设备
    /// </summary>
    public class MobileInspectionDev
    {
        /// <summary>
        /// 设备Id(主键)
        /// </summary>
        [DataMember]
        //[Display(Name = "设备Id(主键)")]
        public int Id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        //[Display(Name = "设备名称")]
        public string Name { get; set; }

        /// <summary>
        /// 所有的巡检内容
        /// </summary>
        [DataMember]
        //[Display(Name = "所有的巡检内容")]
        //[ForeignKey("ParentId")]
        public List<MobileInspectionContent> MobileInspectionContents { get; set; }

        public MobileInspectionDev Clone()
        {
            return this.CloneObjectByBinary();
        }
    }
}
