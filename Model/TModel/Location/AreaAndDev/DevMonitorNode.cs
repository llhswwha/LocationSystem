using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    public class DevMonitorNode
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string TagName { get; set; }

        [DataMember]
        public string DbTagName { get; set; }

        [DataMember]
        public string Describe { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public string KKS { get; set; }

        [DataMember]
        public string ParentKKS { get; set; }

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

    public class DevMonitorNodeList:List<DevMonitorNode>
    {

    }
}
