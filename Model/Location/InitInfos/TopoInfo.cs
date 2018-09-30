using System.Collections.Generic;
using System.Xml.Serialization;

namespace Location.Model.InitInfos
{
    public class TopoInfo
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string KKS { get; set; }

        [XmlAttribute]
        public Types Type { get; set; }

        public BoundInfo BoundInfo { get; set; }

        [XmlElement("TopoInfo")]
        public List<TopoInfo> Children { get; set; }


        public void AddChild(TopoInfo child)
        {
            if (Children == null)
            {
                Children = new List<TopoInfo>();
            }
            Children.Add(child);
        }

        public TopoInfo(PhysicalTopology topo)
        {
            Name = topo.Name;
            KKS = topo.Nodekks == null ? "" : topo.Nodekks.KKS;
            Type = topo.Type;
            if (Type == Types.分组 || Type == Types.区域)
            {
                BoundInfo = null;
            }
            else
            {

                BoundInfo = new BoundInfo(topo.InitBound);
            }

            if(topo.Children!=null)
                foreach (PhysicalTopology child in topo.Children)
                {
                    AddChild(new TopoInfo(child));
                }
        }

        public TopoInfo()
        {

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
