using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 巡检轨迹列表
    /// </summary>
    public class patrols
    {
        /*
        //http://172.16.100.22/api/patrols/2340?offset=0&limit=10000
{
  "createTime": 1568736088,
  "startTime": 1568736000,
  "endTime": 1568764200,
  "id": "2340",
  "code": "巡检单-14270",
  "name": "#1炉机务巡检路线（运行）",
  "state": "执行中",
  "route": [
    {
      "staffCode": "82000007",
      "staffName": "",
      "kksCode": "20190514RHUU",
      "deviceCode": "#1炉辅机间0m区域",
      "deviceName": "",
      "deviceId": 110864
    },
    {
      "staffCode": "82000007",
      "staffName": "",
      "kksCode": "201905142V55",
      "deviceCode": "#1炉定排坑区域",
      "deviceName": "",
      "deviceId": 110781
    },
    {
      "staffCode": "82000007",
      "staffName": "",
      "kksCode": "20190514WLUU",
      "deviceCode": "#1炉辅机间2层平台",
      "deviceName": "",
      "deviceId": 110888
    },
    {
      "staffCode": "82000007",
      "staffName": "",
      "kksCode": "20190514J4YJ",
      "deviceCode": "#1炉辅机间3层平台",
      "deviceName": "",
      "deviceId": 110887
    },
    {
      "staffCode": "82000007",
      "staffName": "",
      "kksCode": "20190514OVHO",
      "deviceCode": "#1炉辅机间4层平台",
      "deviceName": "",
      "deviceId": 110785
    },
    {
      "staffCode": "82000007",
      "staffName": "",
      "kksCode": "2019051447P1",
      "deviceCode": "#1炉29.48m平台区域",
      "deviceName": "",
      "deviceId": 110780
    },
    {
      "staffCode": "82000007",
      "staffName": "",
      "kksCode": "201905141961",
      "deviceCode": "#1炉0m区域",
      "deviceName": "",
      "deviceId": 110890
    }
  ]
}
        */
        /// <summary>
        /// 巡检单Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 巡检单编号，移动巡检系统中的唯一编号或名称
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 巡检路线名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 巡检点列表
        /// </summary>
        public List<checkpoints> route { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long createTimes { get; set; }

        /// <summary>
        /// 巡检状态，新建；已下达 ；已完成；已取消；执行中；已过期
        /// </summary>
        public string state { get; set; }

        private long _startTime = 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        public long startTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
                STime = TimeConvert.ToDateTime((_startTime+ 28800) * 1000);
            }
        }

        public DateTime STime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long endTime { get; set; }

        public override string ToString()
        {
            return name + "," + code;
        }
    }

    [XmlRoot("patrolsList")]
    public class patrolsList:List<patrols>
    {
    }

}
