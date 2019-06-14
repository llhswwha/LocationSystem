using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCommunication.ExtremeVision
{
    public class HeadData
    {
        /// <summary>
        /// 输出的时间戳置为0，分析对象为视频文件时，记录当前帧的时间；分析对象为流媒体服务时值默认为0
        /// </summary>
        public long timeStamp { get; set; }

        public int numOfHead { get; set; }

        /// <summary>
        /// 检测到有人没带头盔的输出‘1’，否则‘0’
        /// </summary>
        public int alertFlag { get; set; }

        public List<HeadInfo> headInfo { get; set; }

    }

    public class HeadInfo
    {
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        /// <summary>
        /// 当前头部头盔数量,输出数量 '0' or '1'
        /// </summary>
        public float numOfHelmet { get; set; }

        public string color { get; set; }
    }

    /*
    "data":	{
        "1":    [{
                        "timeStamp":    0,		//输出的时间戳置为0，分析对象为视频文件时，记录当前帧的时间；分析对象为流媒体服务时值默认为0
                        "alertFlag":    1,			//检测到有人没带头盔的输出‘1’，否则‘0’
                        "headInfo":     [{
                                        "x":    265,	
                                        "y":    289,
                                        "width":        23,
                                        "height":       22,
                                        "numOfHelmet":  0	//当前头部头盔数量,输出数量 '0' or '1'
                                    	
                                }]
                }]
		}
}
     */
}
