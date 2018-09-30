using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TransClass.Models
{
    public class MeterialOther
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
        [Display(Name = "位置")]
        public string loc { get; set; }
    }

    public class MeterialTrans
    {
        public int total { get; set; }

        public string msg { get; set; }

        public List<MeterialOther> data { get; set; }
    }
}
