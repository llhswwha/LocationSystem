using DbModel.Tools;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 每个移动巡检项中具体的内容
    /// </summary>
    public class MobileInspectionContent:IId
    {
        /// <summary>
        /// 巡检内容Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检内容Id")]
        public int Id { get; set; }

        /// <summary>
        /// 所属巡检设备Id
        /// </summary>
        [DataMember]
        [Display(Name = "所属巡检设备Id")]
        public int ParentId { get; set; }

        /// <summary>
        /// 所属巡检内容
        /// </summary>
        [DataMember]
        [Display(Name = "所属巡检内容")]
        [MaxLength(128)]
        public string Content { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [DataMember]
        [Display(Name = "顺序")]
        public int nOrder { get; set; }

        public MobileInspectionContent Clone()
        {
            MobileInspectionContent copy = new MobileInspectionContent();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
