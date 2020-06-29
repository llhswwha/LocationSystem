using Location.TModel.Location.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Alarm
{

    public  class AllAlarms
    {
        public List<LocationAlarm> alarmList { get; set; }
        public List<DeviceAlarm> devAlarmList { get; set; }
    }
}
