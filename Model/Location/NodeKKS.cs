using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{ 
    /// <summary>
    /// 节点（物理拓扑/设备)的KKS信息
    /// </summary>
    public class NodeKKS
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int NodeId { get; set; }

        [DataMember]
        public int KKSId { get; set; }

        [Display(Name = "KKS码")]
        [DataMember]
        [Required]
        public string KKS { get; set; }

        [DataMember]
        public Types NodeType { get; set; }
    }
}
