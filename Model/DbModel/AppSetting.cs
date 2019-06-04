using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel
{
    public static class AppSetting
    {
        /// <summary>
        /// 是否将定位引擎获取的数据写入日志
        /// </summary>
        public static bool WritePositionLog { get; set; }

        public static double PositionMoveStateWaitTime { get; set; }

        /// <summary>
        /// 园区节点名称
        /// </summary>
        public static string ParkName { get; set; }
    }
}
