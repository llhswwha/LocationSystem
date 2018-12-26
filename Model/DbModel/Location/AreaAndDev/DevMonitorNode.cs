using IModel;
using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 设备监控点
    /// </summary>
    public class DevMonitorNode: IDevMonitorNode
    {
        [DataMember]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "标签名")]
        [MaxLength(128)]
        public string TagName { get; set; }

        [DataMember]
        [Display(Name = "数据库名")]
        [MaxLength(32)]
        public string DataBaseName { get; set; }

        [DataMember]
        [Display(Name = "数据库标签名")]
        [MaxLength(128)]
        public string DataBaseTagName { get; set; }

        [DataMember]
        [Display(Name = "描述")]
        [MaxLength(128)]
        public string  Describe{ get; set; }

        [DataMember]
        [Display(Name = "值")]
        [MaxLength(32)]
        public string Value { get; set; }

        [DataMember]
        [Display(Name = "单位")]
        [MaxLength(16)]
        public string Unit { get; set; }

        [DataMember]
        [Display(Name = "数据类型")]
        [MaxLength(16)]
        public string DataType { get; set; }

        [DataMember]
        [Display(Name = "标签类型")]
        [MaxLength(16)]
        public string TagType { get; set; }

        public DevMonitorNode()
        {
            TagName = "";
            DataBaseName = "";
            DataBaseTagName = "";
            Describe = "";
            Unit = "";
            DataType = "";
            TagType = "";
        }
    }
}
