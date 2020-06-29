using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.Location.Alarm
{
    /// <summary>
    /// 人员进入区域记录(仅在区域权限是时间长度时保存记录)
    /// </summary>
   public  class PersonnelFirstInArea: IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        [XmlIgnore]
        public int Id { get; set; }
        [DataMember]
        [Display(Name = "人员Id")]
        [XmlIgnore]
        public int? personId { get; set; }
        [DataMember]
        [Display(Name = "区域Id")]
        [XmlIgnore]
        public int? areaId { get; set; }
        [DataMember]
        [Display(Name = "进入时间")]
        [XmlIgnore]
        public DateTime dateTime { get; set; }

        [DataMember]
        [Display(Name = "类型")]
        [XmlIgnore]
        public int  type { get; set; }//0:表示判断时间长度的记录；1：表示判断是否签到的记录

    }
}
