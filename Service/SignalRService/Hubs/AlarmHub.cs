using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalRService.Models;
using Location.TModel.Location.Alarm;

namespace SignalRService.Hubs
{
    public class AlarmHub:Hub
    {
        public static ConcurrentDictionary<int, DeviceAlarm> DeviceAlarms =
            new ConcurrentDictionary<int, DeviceAlarm>();
        public static ConcurrentDictionary<int, LocationAlarm> LocationAlarms =
            new ConcurrentDictionary<int, LocationAlarm>();

        public static void SendDeviceAlarms(params DeviceAlarm[] alarms)
        {
            //foreach (var alarm in alarms)
            //{
            //    DeviceAlarms[alarm.Id]= alarm;
            //}
            //IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            //chatHubContext.Clients.All.GetDeviceAlarms(alarms);
            SendDevAlarm(alarms);
        }
       
        public static void SendLocationAlarms(params LocationAlarm[] alarms)
        {
            //foreach (var alarm in alarms)
            //{
            //    LocationAlarms[alarm.Id] = alarm;
            //}
            //IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            //chatHubContext.Clients.All.GetLocationAlarms(alarms);
            SendDevAlarm(alarms);
        }

        /// <summary>
        /// 异步发送设备告警
        /// </summary>
        /// <param name="alarms"></param>
        private static async void SendDevAlarm(params DeviceAlarm[] alarms)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            await chatHubContext.Clients.All.GetDeviceAlarms(alarms);
        }
        /// <summary>
        /// 异步发送设备告警
        /// </summary>
        /// <param name="alarms"></param>
        private static async void SendDevAlarm(params LocationAlarm[] alarms)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            await chatHubContext.Clients.All.GetDeviceAlarms(alarms);
        }
    }
}
