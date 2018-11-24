using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.CADEntitys
{
    [XmlType("Shape")]
    public class CADShape: CADEntity
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public string Layer { get; set; }

        [XmlAttribute]
        public int Num { get; set; }

        [XmlElement]
        public List<CADPoint> Points { get; set; }

        public CADShape()
        {
            Points = new List<CADPoint>();
        }

        public CADShape(string name,string layer)
        {
            Name = name;
            Layer = layer;
            Points = new List<CADPoint>();
        }

        public override string ToString()
        {
            return string.Format("{0},{1}",Name,Layer);
        }
    }
}
