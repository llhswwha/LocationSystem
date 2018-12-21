using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    public class position
    {
        //移动设备编号
        public string deviceCode { get; set; }

        //时间戳
        public long t { get; set; }

        public float x { get; set; }

        public float y { get; set; }

        public float z { get; set; }

        public string staffCode { get; set; }

        public string zoneKksCode { get; set; }
    }
}
