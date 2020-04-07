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
    public class LocationAlarmInformation
    {
        [DataMember]
        public int Total;

        [DataMember]
        public List<LocationAlarm> locationAlarmList;

        public void SetEmpty()
        {
            if (locationAlarmList != null && locationAlarmList.Count == 0)
            {
                locationAlarmList = null;
            }
        }
    }
}
