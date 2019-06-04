using System.Collections.Generic;
using System.Xml.Serialization;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;

namespace Location.Model.InitInfos
{
    public class TopoInfo
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string KKS { get; set; }

        [XmlAttribute]
        public AreaTypes Type { get; set; }

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

        public TopoInfo(Area topo,bool containCAD=true)
        {
            Name = topo.Name;
            KKS = topo.KKS;
            Type = topo.Type;



            if (Type == AreaTypes.分组 || Type == AreaTypes.区域)
            {
                BoundInfo = null;
            }
            else
            {

                BoundInfo = new BoundInfo(topo);
                BoundInfo.IsCreateAreaByData = topo.IsCreateAreaByData;
                BoundInfo.IsOnAlarmArea = topo.IsOnAlarmArea;
                BoundInfo.IsOnLocationArea = topo.IsOnLocationArea;
            }

            if(topo.Children!=null)
                foreach (Area child in topo.Children)
                {
                    if (containCAD == false&& Type == AreaTypes.CAD)
                    {
                        continue;
                    }
                    else
                    {
                        AddChild(new TopoInfo(child));
                    }
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
