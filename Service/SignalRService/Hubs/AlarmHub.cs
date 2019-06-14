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
            if (alarms.Length > 0)
            {
                SendDevAlarm(alarms);
            }
        }
       
        public static void SendLocationAlarms(params LocationAlarm[] alarms)
        {
            //foreach (var alarm in alarms)
            //{
            //    LocationAlarms[alarm.Id] = alarm;
            //}
            //IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            //chatHubContext.Clients.All.GetLocationAlarms(alarms);
            if (alarms.Length > 0)
            {
                //if (alarms[0].AreaId == 2) { return; }
                SendLocationAlarm(alarms);
            }
            
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
        private static async void SendLocationAlarm(params LocationAlarm[] alarms)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            await chatHubContext.Clients.All.GetLocationAlarms(alarms);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}
