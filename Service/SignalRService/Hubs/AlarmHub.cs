using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalRService.Models;
using Location.TModel.Location.Alarm;
using Location.BLL.Tool;
using BLL;
using DbModel.Tools;
using System.Threading;
using LocationServices.Converters;

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
                for (int i = 0; i < alarms.Length; i++)
                {
                 int? aa =alarms[i].Abutment_Id;
                    if (aa == null)
                    {
                        alarms[i].Abutment_Id = 0;
                    }
                }
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
                Log.Info("LocationAlarm", "SendLocationAlarms:" + alarms.Length);
                for (int i = 0; i < alarms.Length; i++)
                {
                    alarms[i].Tag = null;
                    alarms[i].Personnel = null;
                }
                //if (alarms[0].AreaId == 2) { return; }
                SendLocationAlarm(alarms);
            }
            
        }

        /// <summary>
        /// 异步发送设备告警
        /// </summary>
        /// <param name="alarms"></param>
        public static async void SendDevAlarm(params DeviceAlarm[] alarms)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();

            await chatHubContext.Clients.All.GetDeviceAlarms(alarms);
        }
        /// <summary>
        /// 异步发送设备告警
        /// </summary>
        /// <param name="alarms"></param>
        public static async void SendLocationAlarm(params LocationAlarm[] alarms)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            await chatHubContext.Clients.All.GetLocationAlarms(alarms);
        }

        /// <summary>
        /// 异步发送设备告警
        /// </summary>
        /// <param name="alarms"></param>
        public static async void SendDevAlarm(string connectionId,params DeviceAlarm[] alarms)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            await chatHubContext.Clients.Client(connectionId).GetDeviceAlarms(alarms);
        }
        /// <summary>
        /// 异步发送设备告警
        /// </summary>
        /// <param name="alarms"></param>
        public static async void SendLocationAlarm(string connectionId, params LocationAlarm[] alarms)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
            await chatHubContext.Clients.Client(connectionId).GetLocationAlarms(alarms);
        }

        public override Task OnConnected()
        {
            Task r= base.OnConnected();
            var connectionId = Context.ConnectionId;
            Log.Info("AlarmHub", string.Format("OnConnected connectionId", connectionId));
            System.Threading.ThreadPool.QueueUserWorkItem(a =>
            {
                try
                {
                    Thread.Sleep(2000);
                    
                    using (Bll bll = Bll.NewBllNoRelation())
                    {
                        var devDict = bll.DevInfos.ToDictionary();

                        var devAlarms = bll.DevAlarms.FindAll(i => i.Src == Abutment_DevAlarmSrc.人员定位);//拿到数据库中已经有的告警数据
                        foreach (var item in devAlarms)
                        {
                            item.DictKey = item.Msg;
                            if (devDict.ContainsKey(item.DevInfoId))
                            {
                                var dev = devDict[item.DevInfoId];
                                item.DevInfo = dev;
                            }

                        }

                        var devAlarmsT = devAlarms.ToTModel();
                        SendDevAlarm(connectionId,devAlarmsT.ToArray());    

                        var locationAlarms = bll.LocationAlarms.ToList();
                        var locationAlarmsT = locationAlarms.ToTModel();
                        SendLocationAlarm(connectionId,locationAlarmsT.ToArray());

                       

                        Log.Info("AlarmHub", string.Format("SendAlarms connectionId:{0}, devAlarm:{1},locationAlarm:{2}", connectionId, devAlarms.Count, locationAlarms.Count));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("AlarmHub", "OnConnected:" + ex);
                }
            });
            return r;
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //var connectionId = Context.ConnectionId;
            //Log.Info("AlarmHub", string.Format("OnDisconnected connectionId", connectionId));
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var connectionId = Context.ConnectionId;
            Log.Info("AlarmHub", string.Format("OnReconnected connectionId", connectionId));
            return base.OnReconnected();
        }
    }
}
