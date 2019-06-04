using DbModel.Tools;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.Location.AreaAndDev
{ 
    /// <summary>
    /// 节点（物理拓扑/设备)的KKS信息
    /// </summary>
    public class NodeKKS:IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 节点Id
        /// </summary>
        [DataMember]
        [Display(Name = "节点Id")]
        public int NodeId { get; set; }

        /// <summary>
        /// KKS Id
        /// </summary>
        [DataMember]
        [Display(Name = "KKS Id")]
        public int KKSId { get; set; }

        /// <summary>
        /// KKS码
        /// </summary>
        [Display(Name = "KKS码")]
        [DataMember]
        [MaxLength(128)]
        [Required]
        public string KKS { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        [Display(Name = "节点类型")]
        [DataMember]
        public AreaTypes NodeType { get; set; }

        public NodeKKS Clone()
        {
            NodeKKS copy = new NodeKKS();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
