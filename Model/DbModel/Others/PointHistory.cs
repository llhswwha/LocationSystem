using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Others
{
    //测点历史数据保存
    public class PointHistory : IId
    {
        [DataMember]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [DataMember]
        [Display(Name = "测点")]
        public string point { get; set; }
        [DataMember]
        [Display(Name = "测点值")]
        public string value { get; set; }
        [DataMember]
        [Display(Name = "保存时间")]
        public DateTime createTime { get; set; }
    }
}
