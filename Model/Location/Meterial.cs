using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    public class Meterial
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "数量")]
        public int qty { get; set; }

        [DataMember]
        [Display(Name = "单位")]
        public string unit { get; set; }

        [DataMember]
        [Display(Name = "逻辑拓扑节点")]
        public int? phtId { get; set; }

        public virtual PhysicalTopology pht { get; set; }

    }
}
