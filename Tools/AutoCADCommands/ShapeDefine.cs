using DbModel.CADEntitys;
using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADCommands
{
    public class ShapesDefine
    {
        public List<CADShape> shapelist;

        public ShapesDefine()
        {
            shapelist = new List<CADShape>();
        }

        public void addShape(CADShape P)
        {
            shapelist.Add(P);
        }

        public string toXml()
        {
            return XmlSerializeHelper.GetXmlText(shapelist);
        }
        
    }
}
