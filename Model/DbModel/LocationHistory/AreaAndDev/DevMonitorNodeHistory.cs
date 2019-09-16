using IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.AreaAndDev
{
    public class DevMonitorNodeHistory : IDevMonitorNode
    {
        [DataMember]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "标签名")]
        [MaxLength(128)]
        public string TagName { get; set; }


        [DataMember]
        [Display(Name = "数据库标签名")]
        [MaxLength(128)]
        public string DbTagName { get; set; }

        [DataMember]
        [Display(Name = "描述")]
        [MaxLength(128)]
        public string Describe { get; set; }

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
        [Display(Name = "KKS")]
        [MaxLength(128)]
        public string KKS { get; set; }

        [DataMember]
        [Display(Name = "ParentKKS")]
        [MaxLength(256)]
        public string ParentKKS { get; set; }

        [DataMember]
        [Display(Name = "ParseResult")]
        [MaxLength(10)]
        public string ParseResult { get; set; }

        [DataMember]
        [Display(Name = "时间戳")]
        public long Time { get; set; }

        public DevMonitorNodeHistory()
        {
            TagName = "";
            DbTagName = "";
            Describe = "";
            Value = "";
            Unit = "";
            DataType = "";
            KKS = "";
            ParentKKS = "";
            Time = 0;
        }
    }
}
