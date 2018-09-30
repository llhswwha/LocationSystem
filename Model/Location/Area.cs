using System.ComponentModel.DataAnnotations;
using Location.Model.Base;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Location.Model
{
    /// <summary>
    /// 区域 地图上的一个子部分
    /// </summary>
    [DataContract]
    public class Area//:Bound
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "名称")]        

        public string Name { get; set; }

        [DataMember]
        public int? MapId { get; set; }

        [Display(Name = "地图")]
        public virtual Map Map { get; set; }

        [DataMember]
        public int? TransformId { get; set; }

        [DataMember]
        [ForeignKey("TransformId")]
        public virtual TransformM Transform { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}