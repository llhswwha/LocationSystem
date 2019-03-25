using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    /// <summary>
    /// 园区统计
    /// </summary>
    public class AreaStatistics
    {
        /// <summary>
        /// 人员总数
        /// </summary>
        public int PersonNum { get; set; }

        /// <summary>
        /// 设备总数
        /// </summary>
        public int DevNum { get; set; }

        /// <summary>
        /// 定位告警数
        /// </summary>
        public int LocationAlarmNum { get; set; }

        /// <summary>
        /// 设备告警数
        /// </summary>
        public int DevAlarmNum { get; set; }

        /// <summary>
        /// 告警人员数量
        /// </summary>
        public int AlarmPersonNum { get; set; }

        public AreaStatistics()
        {
            PersonNum = 0;
            DevNum = 0;
            LocationAlarmNum = 0;
            DevAlarmNum = 0;
            AlarmPersonNum = 0;
        }

        public void Add(AreaStatistics ast2)
        {
            PersonNum += ast2.PersonNum;
            DevNum += ast2.DevNum;
            LocationAlarmNum += ast2.LocationAlarmNum;
            DevAlarmNum += ast2.DevAlarmNum;
            AlarmPersonNum += ast2.AlarmPersonNum;
        }
    }
}
