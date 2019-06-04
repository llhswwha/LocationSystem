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
using System.Threading.Tasks;

namespace WebNSQLib.RecvMessage
{
    /// <summary>
    /// 接收门禁告警
    /// </summary>
    public class RecvAccessControlAlarm : IHandler
    {
        private Bll bll = null;
        private List<DevInfo> DevList = new List<DevInfo>();
        private List<DevAlarm> DaList = new List<DevAlarm>();

        public RecvAccessControlAlarm()
        {
            bll = new Bll();
            DevList = bll.DevInfos.ToList();
            DaList = bll.DevAlarms.Where(p => p.Src == Abutment_DevAlarmSrc.门禁).ToList();
        }

        /// <summary>Handles a message.</summary>
        public void HandleMessage(IMessage message)
        {
            string msg = Encoding.UTF8.GetString(message.Body);
            events recv = JsonConvert.DeserializeObject<events>(msg);

            if (recv == null || recv.raw_id == null || recv.raw_id == "" || recv.src != 2)
            {
                return;
            }

            DevInfo di = DevList.Find(p => p.Abutment_DevID == recv.raw_id);

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
