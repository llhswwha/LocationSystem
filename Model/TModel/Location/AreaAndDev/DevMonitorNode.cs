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
        public string KKS { get; set; }

        [DataMember]
        public string TagName { get; set; }

        [DataMember]
        public string DataBaseName { get; set; }

        [DataMember]
        public string DataBaseTagName { get; set; }

        [DataMember]
        public string Describe { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public string TagType { get; set; }

        public DevMonitorNode()
        {
            KKS = "";
            TagName = "";
            DataBaseName = "";
            DataBaseTagName = "";
            Describe = "";
            Value = "";
            Unit = "";
            DataType = "";
            TagType = "";
        }
    }
}
