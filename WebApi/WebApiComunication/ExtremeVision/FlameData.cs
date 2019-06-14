using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCommunication.ExtremeVision
{
    public class FlameData
    {
        /// <summary>
        /// 检测到有人没带头盔的输出‘1’，否则‘0’
        /// </summary>
        public int alertFlag { get; set; }

        public string message { get; set; }

        public int numOfFlameRects { get; set; }

        public List<FlameInfo> flameInfo { get; set; }

    }

    public class FlameInfo
    {
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }
    }

    /*
     "data”:[
	{
	"alertFlag" : 1,
	"flameInfo" : 
	[
		{
			"height" : 56,
			"width" : 100,
			"x" : 950,
			"y" : 666
		}
	],
	"message" : "Waning! Flame is being detected",
	"numOfFlameRects" : 1
}

	]
}
     */
}
