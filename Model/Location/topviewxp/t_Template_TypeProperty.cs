using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Location.Model.topviewxp
{
    [DataContract]
    public class t_Template_TypeProperty
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name ="模型名称")]
        public string TypeName { get; set; }
        //[Key]
        [DataMember]
        public long TypeCode { get; set; }
        [DataMember]
        public Nullable<int> orientation { get; set; }
        [DataMember]
        public Nullable<long> InstantTime { get; set; }
        [DataMember]
        public Nullable<long> ReviseTime { get; set; }
        [DataMember]
        public string height { get; set; }
        [DataMember]
        public string energy { get; set; }
        [DataMember]
        public string weight { get; set; }
        [DataMember]
        public string model { get; set; }
        [DataMember]
        public string style { get; set; }
        [Display(Name ="厂家")]
        [DataMember]
        public string manufacturer { get; set; }
        [DataMember]
        public string sizen { get; set; }
        [DataMember]
        public string colour { get; set; }
        [DataMember]
        public string refrigeration { get; set; }
        [DataMember]
        public string FrontElevation { get; set; }
        [DataMember]
        public string RearView { get; set; }
        [Display(Name ="类型说明书")]
        [DataMember]
        public string BackInstruction { get; set; }
        [DataMember]
        public string Obligate3 { get; set; }
        [DataMember]
        public string Obligate4 { get; set; }
    }
}
