using DbModel.LocationHistory.AreaAndDev;
using IModel;
using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 设备监控点
    /// </summary>
    public class DevMonitorNode: IDevMonitorNode
    {
        [DataMember]
        [Display(Name = "Id")]
        [XmlAttribute]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "标签名")]
        [MaxLength(128)]
        [XmlAttribute]
        public string TagName { get; set; }

        [DataMember]
        [Display(Name = "数据库标签名")]
        [MaxLength(128)]
        [XmlAttribute]
        public string DbTagName { get; set; }

        [DataMember]
        [Display(Name = "描述")]
        [MaxLength(128)]
        [XmlAttribute]
        public string Describe { get; set; }

        [DataMember]
        [Display(Name = "值")]
        [MaxLength(32)]
        [XmlAttribute]
        public string Value { get; set; }

        [DataMember]
        [Display(Name = "单位")]
        [MaxLength(16)]
        [XmlAttribute]
        public string Unit { get; set; }

        [DataMember]
        [Display(Name = "数据类型")]
        [MaxLength(16)]
        [XmlAttribute]
        public string DataType { get; set; }

        [DataMember]
        [Display(Name = "KKS")]
        [MaxLength(128)]
        [XmlAttribute]
        public string KKS { get; set; }

        [DataMember]
        [Display(Name = "KKS")]
        [MaxLength(128)]
        [XmlAttribute]
        public string ParentKKS { get; set; }

        [DataMember]
        [Display(Name = "ParseResult")]
        [MaxLength(10)]
        [XmlAttribute]
        public string ParseResult { get; set; }

        [DataMember]
        [Display(Name = "时间戳")]
        [XmlAttribute]
        public long Time { get; set; }

        [NotMapped]
        public KKSCode KKSCode { get; set; }

        [NotMapped]
        public DevMonitorNode PreNode { get; set; }

        [NotMapped]
        public DevMonitorNode NextMode { get; set; }

        public DevMonitorNode()
        {
            TagName = "";
            DbTagName = "";
            Describe = "";
            Value = "";
            Unit = "";
            DataType = "";
            KKS = "";
            ParentKKS = "";
            ParseResult = "";
            Time = 0;
        }

        public void SetNull()
        {
            TagName = null;
            DbTagName = null;
            Describe = null;
            Value = null;
            Unit = null;
            DataType = null;
            KKS = null;
            ParentKKS = null;
            ParseResult = null;
            Time = 0;
        }

        public DevMonitorNodeHistory ToHistory()
        {
            DevMonitorNodeHistory history = new DevMonitorNodeHistory();
            history.TagName = this.TagName;
            history.DbTagName = this.DbTagName;
            history.Describe = this.Describe;
            history.Value = this.Value;
            history.Unit = this.Unit;
            history.DataType = this.DataType;
            history.KKS = this.KKS;
            history.ParentKKS = this.ParentKKS;
            history.Time = this.Time;

            return history;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Describe, KKS, ParentKKS);
        }
    }

    [XmlRoot("DevMonitorNodeList")]
    public class DevMonitorNodeList : List<DevMonitorNode>
    {

    }
}
