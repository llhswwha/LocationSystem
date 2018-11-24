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

        public List<CADShape> Shapes { get; set; }

        public CADArea()
        {
            Shapes = new List<CADShape>();
        }

        public void SetZero(CADPoint zero)
        {
            Zero = zero;
            foreach (var sp in Shapes)
            {
                foreach (var pt in sp.Points)
                {
                    pt.Decrease(zero);
                }
            }
        }
    }
}
