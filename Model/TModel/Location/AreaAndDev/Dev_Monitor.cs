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

        private List<DevMonitorNode> _monitorNodeList;

        [DataMember]
        public List<DevMonitorNode> MonitorNodeList
        {
            get
            {
                return _monitorNodeList;
            }
            set
            {
                _monitorNodeList = value;
                Tags = GetMonitorTags();
            }
        }

        public bool HaveMonitorNode()
        {
            return MonitorNodeList != null && MonitorNodeList.Count > 0;
        }

        //[DataMember]
        public string Tags { get; set; }

        public Dev_Monitor()
        {
            Name = "";
            KKSCode = "";
            ChildrenList = null;
            MonitorNodeList = null;
        }

        public override string ToString()
        {
            bool haveChildren = ChildrenList != null && ChildrenList.Count > 0;
            bool haveMonitor = MonitorNodeList != null && MonitorNodeList.Count > 0;
            return string.Format("{0},{1}[{2}][{3}]", Name, KKSCode, haveChildren, haveMonitor);
        }

        public string GetAllTags()
        {
            List<DevMonitorNode> nodes = GetAllNodes();
            string tags = "";
            foreach (var node in nodes)
            {
                if (tags == "")
                {
                    tags = node.TagName;
                }
                else
                {
                    tags += "," + node.TagName;
                }
            }
            return tags;
        }

        public List<string> GetAllTagList()
        {
            List<DevMonitorNode> nodes = GetAllNodes();
            List<string> tags = new List<string>();
            foreach (var node in nodes)
            {
                tags.Add(node.TagName);
            }
            return tags;
        }

        public string GetMonitorTags()
        {
            string tags = "";
            if (MonitorNodeList != null)
                foreach (var item in MonitorNodeList)
                {
                    if (tags == "")
                    {
                        tags = item.TagName;
                    }
                    else
                    {
                        tags += "," + item.TagName;
                    }
                }
            return tags;
        }

        public List<DevMonitorNode> GetAllNodes()
        {
            List<DevMonitorNode> nodes = new List<DevMonitorNode>();
            if (MonitorNodeList != null)
            {
                nodes.AddRange(MonitorNodeList);
            }
            if (ChildrenList != null)
            {
                foreach (var item in ChildrenList)
                {
                    List<DevMonitorNode> subNodes = item.GetAllNodes();
                    nodes.AddRange(subNodes);
                }
            }
            return nodes;
        }

        public List<Dev_Monitor> GetChildNodes()
        {
            List<Dev_Monitor> nodes = new List<Dev_Monitor>();
            if (ChildrenList != null)
            {
                foreach (var item in ChildrenList)
                {
                    nodes.Add(item);
                    List<Dev_Monitor> subNodes = item.GetChildNodes();
                    nodes.AddRange(subNodes);
                }
            }
            return nodes;
        }


        public void RemoveEmpty()
        {
            if (ChildrenList != null)
            {
                foreach (Dev_Monitor devMonitor in ChildrenList)
                {
                    devMonitor.RemoveEmpty();
                }
                for (int i = 0; i < ChildrenList.Count; i++)
                {
                    var child = ChildrenList[i];
                    if (child.HaveMonitorNode()==false)
                    {
                        ChildrenList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void AddChildrenMonitorNodes()
        {
            var nodes = GetAllNodes();
            if (MonitorNodeList == null)
            {
                MonitorNodeList = nodes;
            }
            else
            {
                MonitorNodeList.AddRange(nodes);
            }
        }
    }
}
