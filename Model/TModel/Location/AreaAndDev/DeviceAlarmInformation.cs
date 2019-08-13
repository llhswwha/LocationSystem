using DbModel.Tools;
using Location.TModel.Location.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    [DataContract]
    [Serializable]
    public   class DeviceAlarmInformation
    {
        [DataMember]
        public int Total;

        [DataMember]
        public List<DeviceAlarm> devAlarmList;

        public void SetEmpty()
        {
            if (devAlarmList != null && devAlarmList.Count == 0)
            {
                devAlarmList = null;
            }
        }
    }
}
