using BLL;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.Alarm;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.TModel.Tools;
using Newtonsoft.Json;
using NsqSharp;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace WebNSQLib
{
    public class RealAlarm
    {
        public MessageHandler MessageHandler = new MessageHandler();
        public void ReceiveRealAlarmInfo()
        {
            var consumer = new Consumer("honeywell", "channel-name");
            consumer.AddHandler(MessageHandler);
            consumer.ConnectToNsqLookupd("ipms.datacase.io:4161");

            while (true) { Thread.Sleep(6000); }

            consumer.Stop();
        }
    }

    public class MessageHandler : IHandler
    {
        private Bll bll = Bll.Instance();

        /// <summary>Handles a message.</summary>
        public void HandleMessage(IMessage message)
        {
            string msg = Encoding.UTF8.GetString(message.Body);
            events recv = JsonConvert.DeserializeObject<events>(msg);
            if (recv == null || recv.deviceId == null)
            {
                return;
            }

            DevInfo di = bll.DevInfos.DbSet.Where(p => p.Abutment_Id == recv.deviceId).FirstOrDefault();
            if (di == null)
            {
                return;
            }

            DevAlarm da = bll.DevAlarms.DbSet.Where(p => p.Abutment_Id == recv.id).FirstOrDefault();
            int nFlag = 0;
            if (da == null)
            {
                da = new DevAlarm();
                nFlag = 1;
            }

            da.Abutment_Id = recv.id;
            da.Title = recv.title;
            da.Msg = recv.msg;
            da.Level = (Abutment_DevAlarmLevel)recv.level;
            da.Code = recv.code;
            da.Src = (Abutment_DevAlarmSrc)recv.src;
            da.DevInfoId = di.Id;
            da.Device_desc = recv.deviceDesc;
            da.AlarmTime = TimeConvert.TimeStampToDateTime(recv.t/1000);
            da.AlarmTimeStamp = recv.t;

            if (nFlag == 1)
            {
                bll.DevAlarms.Add(da);
            }
            else
            {
                bll.DevAlarms.Edit(da);
            }

            OnDevAlarmReceived(da);
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
