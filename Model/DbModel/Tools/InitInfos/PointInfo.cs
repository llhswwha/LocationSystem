using System.Xml.Serialization;
using DbModel.Location.AreaAndDev;

namespace Location.Model.InitInfos
{
    public class PointInfo
    {
        [XmlAttribute]
        public float X { get; set; }

        [XmlAttribute]
        public float Y { get; set; }

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
