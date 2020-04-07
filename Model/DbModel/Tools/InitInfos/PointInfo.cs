using System;
using System.Xml.Serialization;
using DbModel.Location.AreaAndDev;

namespace Location.Model.InitInfos
{
    [Serializable]
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

        public PointInfo(float x,float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }
    }
}
