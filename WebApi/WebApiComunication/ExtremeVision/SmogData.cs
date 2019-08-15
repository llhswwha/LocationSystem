using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.ExtremeVision
{
    /*
     * {
       "numOfSmogRects": 1,
       "alertFlag": 1,
       "smogInfo": [{
         "x": 888,
         "y": 1000,
         "width": 240,
         "height": 100
        }
     */
    public class SmogData
    {
        /// <summary>
        /// 检测到有人没带头盔的输出‘1’，否则‘0’
        /// </summary>
        public int alertFlag { get; set; }

        public int numOfSmogRects { get; set; }

        public List<RectInfo> smogInfo { get; set; }
    }
}
