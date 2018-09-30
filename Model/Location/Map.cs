using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Location.Model.Base;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Location.Model
{
    /// <summary>
    /// 地图信息
    /// </summary>
    [DataContract]
    public class Map//: Bound
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "文件")]
        public string FileName { get; set; }

        [DataMember]
        [Display(Name = "排序")]
        public int ShowOrder { get; set; }

        [DataMember]
        public int? DepId { get; set; }
        /// <summary>
        /// 所属机构
        /// </summary>
        [Display(Name = "机构")]
        public virtual Department Dep { get; set; }

        [DataMember]
        public int? TopoNodeId { get; set; }
        /// <summary>
        /// 拓扑区域
        /// </summary>
        [Display(Name = "拓扑区域")]
        public virtual PhysicalTopology TopoNode { get; set; }

        [DataMember]
        [Display(Name = "主地图")]
        /// <summary>
        /// 是否主地图
        /// </summary>
        public bool IsMain { get; set; }

        [ForeignKey("MapId")]
        [DataMember]
        public virtual List<Area> Areas { get; set; }

        [DataMember]
        public double MinX { get; set; }

        [DataMember]
        public double MaxX { get; set; }

        [DataMember]
        public double MinY { get; set; }

        [DataMember]
        public double MaxY { get; set; }

        [DataMember]
        public double MinZ { get; set; }

        [DataMember]
        public double MaxZ { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}