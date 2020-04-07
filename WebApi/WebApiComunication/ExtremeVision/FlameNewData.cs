using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.ExtremeVision
{
  public  class FlameNewData
    {
        public int alert_flag { get; set; }

        public int alert_num { get; set; }

        public List<RectOneInfo> alert_info { get; set; }


    }


    public class RectOneInfo : RectInfo
    {
        public int confidence { get; set; }
    }


/*
[
  {
    "alert_flag": 1,
    "alert_num": 1,
    "alert_info": [
      {
        "confidence": 0.6843218,
        "height": 56,
        "width": 100,
        "x": 950,
        "y": 666
      }
    ]
  }
]
*/

}
