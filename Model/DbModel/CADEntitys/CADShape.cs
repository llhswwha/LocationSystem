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
    public class CADShape: CADEntity,IComparable<CADShape>
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

        [XmlAttribute]
        public string Text { get; set; }

        [XmlIgnore]
        public object Entity { get; set; }

        public CADPoint GetPoint()
        {
            if (Points.Count > 0)
            {
                return Points[0];
            }
            return null;
        }

        public CADPoint GetCenter()
        {
            CADPoint r = new CADPoint();
            if (Points != null && Points.Count>0)
            {
                foreach (CADPoint point in Points)
                {
                    r.X += point.X;
                    r.Y += point.Y;
                }

                r.X /= Points.Count;
                r.Y /= Points.Count;
            }


            return r;
        }


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
            if (Points != null && Points.Count > 0)
            {
                return string.Format("{0},{1},{2}", Name, Text, GetPoint());
            }
            else
            {
                return string.Format("{0},{1},{2}", Name, Type, Text);
            }
        }

        public double GetDistance(CADShape other)
        {
            var p1 = this.GetPoint();
            var p2 = other.GetPoint();
            if (p1 == null || p2 == null)
            {
                return double.MaxValue;
            }

            double distance = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            return distance;
        }

        public void SetZero(CADPoint zero)
        {
            if(Points!=null)
                foreach (CADPoint point in Points)
                {
                    point.X = point.X - zero.X;
                    point.Y = point.Y - zero.Y;
                }
        }

        public int CompareTo(CADShape other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }

    public class CADShapeList : List<CADShape>
    {
        public List<string> GetTypes()
        {
            List<string> types = new List<string>();
            foreach (CADShape item in this)
            {
                string type = item.Type;
                if(!types.Contains(type))
                    types.Add(type);
            }
            return types;
        }

        public Dictionary<string, CADShapeList> GetTypesEx()
        {
            Dictionary<string,CADShapeList> types = new Dictionary<string, CADShapeList>();
            foreach (CADShape item in this)
            {
                string type = item.Type;
                if (!types.ContainsKey(type))
                {
                    types.Add(type,new CADShapeList());
                }

                CADShapeList list = types[item.Type];
                list.Add(item);
            }
            return types;
        }

        public CADShape FindCloset(CADShape text)
        {
            CADShape closet = null;
            double min = double.MaxValue;
            foreach (CADShape shape in this)
            {
                var d = shape.GetDistance(text);
                if (d < min)
                {
                    min = d;
                    closet = shape;
                }
            }
            return closet;
        }

        public void SortByXY()
        {
            this.Sort((a, b) =>
            {
                CADPoint p1 = a.GetPoint();
                CADPoint p2 = b.GetPoint();
                if (p1 != null && p2 != null)
                {
                    int r1= p1.X.CompareTo(p2.X);
                    if (r1 == 0)
                    {
                        return p1.Y.CompareTo(p2.Y);
                    }
                    else
                    {
                        return r1;
                    }
                }
                else
                {
                    return a.Num.CompareTo(b.Num);
                }
            });
        }
    }
}
