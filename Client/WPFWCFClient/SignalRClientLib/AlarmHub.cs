using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Location.Alarm;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRClientLib
{
    public class AlarmHub:HubClient
    {
        public AlarmHub(string uri) : base(uri, "AlarmHub")
        {
            HubProxy.On<DeviceAlarm[]>("GetDeviceAlarms", alarms =>
            {
                if (GetDeviceAlarms != null)
                {
                    GetDeviceAlarms(alarms);
                }
            });

            HubProxy.On<LocationAlarm[]>("GetLocationAlarms", alarms =>
            {
                if (GetLocationAlarms != null)
                {
                    GetLocationAlarms(alarms);
                }
            });
        }

        public event Action<DeviceAlarm[]> GetDeviceAlarms;

        public event Action<LocationAlarm[]> GetLocationAlarms;
    }
}
