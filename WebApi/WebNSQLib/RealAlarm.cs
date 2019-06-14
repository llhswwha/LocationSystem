using BLL;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.Alarm;
using DbModel.Location.AreaAndDev;
using DbModel.LocationHistory.Alarm;
using DbModel.Tools;
using Location.TModel.Tools;
using Newtonsoft.Json;
using NsqSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WebNSQLib
{
    public class RealAlarm
    {
        public static string NsqLookupdUrl = "172.16.100.22:4161";
        public static string NsqLookupdTopic = "honeywell";
        public static string NsqLookupdChannel = "channel-name";

        public MessageHandler MessageHandler =null;

        public Action<DevAlarm> callback;

        public RealAlarm(Action<DevAlarm> action)
        {
            callback = action;
        }

        public void ReceiveRealAlarmInfo()
        {
            MessageHandler = new MessageHandler();
            if(callback!=null)
                MessageHandler.DevAlarmReceived += callback;
            var consumer = new Consumer(NsqLookupdTopic, NsqLookupdChannel);
            consumer.AddHandler(MessageHandler);
            //consumer.ConnectToNsqLookupd("ipms.datacase.io:4161");
            consumer.ConnectToNsqLookupd(NsqLookupdUrl);

            while (true) {
                Thread.Sleep(6000);
            }

            consumer.Stop();
        }
    }

    public class MessageHandler : IHandler
    {
        private Bll bll = Bll.Instance();

        private List<DevInfo> DevList = new List<DevInfo>();
        private List<DevAlarm> DaList = new List<DevAlarm>();

        public MessageHandler()
        {
            bll = new Bll();
            DevList = bll.DevInfos.ToList();
            DaList = bll.DevAlarms.Where(p => p.Src == Abutment_DevAlarmSrc.视频监控 || p.Src == Abutment_DevAlarmSrc.门禁 || p.Src == Abutment_DevAlarmSrc.消防).ToList();
        }

        /// <summary>Handles a message.</summary>
        public void HandleMessage(IMessage message)
        {
            string msg = Encoding.UTF8.GetString(message.Body);
            events recv = JsonConvert.DeserializeObject<events>(msg);

            if (recv == null)
            {
                return;
            }

            int nsrc = recv.src;

            DevInfo di = null;

            if (nsrc == 1 || nsrc == 2)
            {
                if (recv.raw_id == null || recv.raw_id == "")
                {
                    return;
                }
                di = DevList.Find(p => p.Abutment_DevID == recv.raw_id);
            }
            else if (nsrc == 3)
            {
                if (recv.node == null || recv.node == "")
                {
                    return;
                }

                di = DevList.Find(p => p.Code == recv.node);
            }

            if (di == null)
            {
                return;
            }

            bool bFlag = false;
            int nLevel = (int)recv.level;
            Abutment_DevAlarmLevel adLevel = (Abutment_DevAlarmLevel)nLevel;

            long lTimeStamp = recv.t * 1000;

            if (nLevel == 0)
            {
                adLevel = Abutment_DevAlarmLevel.未定;
            }

            DevAlarm da = DaList.Find(p => p.DevInfoId == di.Id && p.AlarmTimeStamp == lTimeStamp);
            if (da == null)
            {
                if (recv.state == 0)
                {
                    da = new DevAlarm();
                    da.Abutment_Id = recv.id;
                    da.Title = recv.title;
                    da.Msg = recv.msg;
                    da.Level = adLevel;
                    da.Code = recv.code;
                    da.Src = (Abutment_DevAlarmSrc)recv.src;
                    da.DevInfoId = di.Id;
                    da.Device_desc = recv.deviceDesc;
                    da.AlarmTime = TimeConvert.TimeStampToDateTime(lTimeStamp);
                    da.AlarmTimeStamp = lTimeStamp;
                    bll.DevAlarms.Add(da);
                    DaList.Add(da);
                    bFlag = true;
                }
            }
            else
            {
                if (recv.state == 1 || recv.state == 2)
                {
                    DevAlarmHistory da_history = da.RemoveToHistory();
                    DaList.Remove(da);
                    bll.DevAlarms.DeleteById(da.Id);
                    bll.DevAlarmHistorys.Add(da_history);
                    da.Level = Abutment_DevAlarmLevel.无;
                    bFlag = true;
                }
                else if (adLevel != da.Level)
                {
                    da.Level = adLevel;
                    da.Title = recv.title;
                    da.Msg = recv.msg;
                    bll.DevAlarms.Edit(da);
                    bFlag = true;
                }
            }

            if (bFlag)
            {
                OnDevAlarmReceived(da);
            }

            return;
        }

        public event Action<DevAlarm> DevAlarmReceived;

        protected void OnDevAlarmReceived(DevAlarm alarm)
        {
            if (DevAlarmReceived != null)
            {
                DevAlarmReceived(alarm);
            }
        }

        /// <summary>
        /// Called when a message has exceeded the specified <see cref="Config.MaxAttempts"/>.
        /// </summary>
        /// <param name="message">The failed message.</param>
        public void LogFailedMessage(IMessage message)
        {
            // Log failed messages
        }
    }
}
