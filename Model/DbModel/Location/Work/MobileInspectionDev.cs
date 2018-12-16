using DbModel.Tools;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 需要移动巡检的设备
    /// </summary>
    public class MobileInspectionDev:IEntity
    {
        /// <summary>
        /// 设备Id(主键)
        /// </summary>
        [DataMember]
        [Display(Name = "设备Id(主键)")]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        [Display(Name = "设备名称")]
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// 所有的巡检内容
        /// </summary>
        [DataMember]
        [Display(Name = "所有的巡检内容")]
        [ForeignKey("ParentId")]
        public virtual List<MobileInspectionContent> MobileInspectionContents { get; set; }

        public MobileInspectionDev Clone()
        {
            MobileInspectionDev copy = new MobileInspectionDev();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
