using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.CADEntitys
{
    [XmlRoot("Area")]
    public class CADArea
    {
        [XmlAttribute]
        public string Name { get; set; }

        public CADPoint Zero { get; set; }

        [XmlAttribute]
        public string ZeroType { get; set; }

        public List<CADShape> Shapes { get; set; }

        public CADArea()
        {
            Shapes = new List<CADShape>();
        }

        public void SetZero(CADPoint zero,string key)
        {
            Zero = zero;
            ZeroType = key;
            foreach (var sp in Shapes)
            {
                foreach (var pt in sp.Points)
                {
                    double x = pt.X;
                    double y = pt.Y;
                    if(key.Contains("0"))//左下
                    {
                        pt.X -= zero.X;
                        pt.Y -= zero.Y;
                    }
                    else if (key.Contains("1"))//右下
                    {
                        pt.Y = zero.X - x;
                        pt.X = y - zero.Y;
                    }
                    else if (key.Contains("2"))//右上
                    {
                        pt.X = zero.X - x;
                        pt.Y = zero.Y - y;
                    }
                    else if (key.Contains("3"))//左上
                    {
                        pt.X = zero.Y - y;
                        pt.Y = x - zero.X;
                    }
                }
            }
        }
    }
}
