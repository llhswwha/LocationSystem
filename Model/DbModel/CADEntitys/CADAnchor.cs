using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.CADEntitys
{
    public class CADAnchorList
    {
        //public string Text

        public List<CADShape> Anchors { get; set; }

        [XmlAttribute]
        public string ParentName { get; set; }

        public CADAnchorList()
        {
            Anchors=new List<CADShape>();
        }
    }
}
