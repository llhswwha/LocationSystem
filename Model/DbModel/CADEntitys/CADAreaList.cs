using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.CADEntitys
{
    [XmlRoot("AreaList")]
    public class CADAreaList : List<CADArea>
    {
        /// <summary>
        /// 把线合并成四边形
        /// </summary>
        public void LineToBlock()
        {
            foreach (var item in this)
            {
                item.LineToBlock();
            }
        }
    }
}
