using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    /// <summary>
    /// 监控设备
    /// </summary>
    [DataContract]
    public class Dev_Monitor
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string KKSCode { get; set; }

        [DataMember]
        public List<Dev_Monitor> ChildrenList { get; set; }

        [DataMember]
        public List<DevMonitorNode> MonitorNodeList { get; set; }

        public Dev_Monitor()
        {
            Name = "";
            KKSCode = "";
            ChildrenList = null;
            MonitorNodeList = null;
        }

    }
}
