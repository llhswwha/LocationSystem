using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.CADEntitys
{
    [XmlType("Point")]
    public class CADPoint
    {
        [XmlAttribute]
        public double X { get; set; }
        [XmlAttribute]
        public double Y { get; set; }

        public CADPoint()
        {

        }

        public CADPoint(double x,double y)
        {
            X = x;
            Y = y;
        }

        public void Decrease(CADPoint pt)
        {
            X -= pt.X;
            Y -= pt.Y;
        }

        public override string ToString()
        {
            return string.Format("({0:F2},{1:F2})", X, Y);
        }

        public static CADPoint operator +(CADPoint a, CADPoint b)
        {
            return new CADPoint(a.X - b.X, a.Y - b.Y);
        }
        public static CADPoint operator -(CADPoint a, CADPoint b)
        {
            return new CADPoint(a.X - b.X, a.Y - b.Y);
        }


        public bool IsSamePoint(CADPoint other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
    }
}
