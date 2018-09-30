using System.Xml.Serialization;
using Location.Model.Base;

namespace Location.Model.InitInfos
{
    public class PointInfo
    {
        [XmlAttribute]
        public double X { get; set; }

        [XmlAttribute]
        public double Y { get; set; }

        public PointInfo(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public PointInfo()
        {
            
        }
    }
}
