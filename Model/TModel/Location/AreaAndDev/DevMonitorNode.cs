using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace TModel.Location.AreaAndDev
{
    public class DevMonitorNode
    {
        [XmlAttribute]
        [DataMember]
        public int Id { get; set; }

        [XmlAttribute]
        [DataMember]
        public string TagName { get; set; }

        [XmlAttribute]
        [DataMember]
        public string DbTagName { get; set; }

        [XmlAttribute]
        [DataMember]
        public string Describe { get; set; }

        [XmlAttribute]
        [DataMember]
        public string Value { get; set; }

        [XmlAttribute]
        [DataMember]
        public string Unit { get; set; }

        [XmlAttribute]
        [DataMember]
        public string DataType { get; set; }

        [XmlAttribute]
        [DataMember]
        public string KKS { get; set; }

        [XmlAttribute]
        [DataMember]
        public string ParentKKS { get; set; }

        [XmlAttribute]
        [DataMember]
        public long Time { get; set; }

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
            Time = 0;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", Describe, TagName, KKS, Value);
        }
    }

    [XmlRoot("DevMonitorNodeList")]
    public class DevMonitorNodeList:List<DevMonitorNode>
    {

    }
}
