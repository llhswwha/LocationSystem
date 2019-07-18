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
using System.IO;
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
            consumer.AfterQueryLookupdException = ex =>
            {
                if (ex != null && ex.Message != null && ex.Message.Contains("请求被中止: 操作超时。")) //Nsq本地测试时连接不上
                {
                    LogEvent.Info("请求被中止: 操作超时。Nsq连接不上。等待60s。");
                    Thread.Sleep(60000); //60s，后再继续
                }
            };
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
        private Bll bll = null;

        private List<DevInfo> DevList = new List<DevInfo>();
        private List<DevAlarm> DaList = new List<DevAlarm>();

        public MessageHandler()
        {
            
        }

        private void Init()
        {
            if (bll == null)
            {
                bll = new Bll();
                DevList = bll.DevInfos.ToList();
                if (DevList == null) return;
                DaList = bll.DevAlarms.Where(p => p.Src == Abutment_DevAlarmSrc.视频监控 || p.Src == Abutment_DevAlarmSrc.门禁 || p.Src == Abutment_DevAlarmSrc.消防).ToList();
            }
        }

        private void SaveMessageToFile(string msg,string subDir)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\json\\alarms\\";
                if (!string.IsNullOrEmpty(subDir))
                {
                    path += subDir + "\\";
                }
                DirectoryInfo dir = new DirectoryInfo(path);
                if (dir.Exists == false)
                {
                    dir.Create();
                }
                string filePath = string.Format("{0}{1}.json",path,DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
                File.WriteAllText(filePath, msg);//保存告警
            }
            catch (Exception ex)
            {
                LogEvent.Info("RealAlarm.SaveMessageToFile:" + ex.Message);
            }
        }

        /// <summary>Handles a message.</summary>
        public void HandleMessage(IMessage message)
        {
            Init();
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

            long lTimeStamp = recv.t * 1000;
            bool bFlag = false;
            int nLevel = (int)recv.level;
            Abutment_DevAlarmLevel adLevel = (Abutment_DevAlarmLevel)nLevel;
            if (nLevel == 0)
            {
                adLevel = Abutment_DevAlarmLevel.未定;
            }

            if (di == null)
            {
                //DevAlarm da2 = new DevAlarm();
                //da2.Abutment_Id = recv.id;
                //da2.Title = recv.title;
                //da2.Msg = recv.msg;
                //da2.Level = adLevel;
                //da2.Code = recv.code;
                //da2.Src = (Abutment_DevAlarmSrc)recv.src;
                //da2.DevInfoId = 0;//未找到设备
                //da2.Device_desc = recv.deviceDesc;
                //da2.AlarmTime = TimeConvert.ToDateTime(lTimeStamp);
                //da2.AlarmTimeStamp = lTimeStamp;
                //bll.DevAlarms.Add(da2);//未找到设备的告警也记录下来，
                //Log. bv
                LogEvent.Info(string.Format("没找到设备信息,json:{0}", msg));
                SaveMessageToFile(msg, "noDev");
                return;//没找到设备信息，则不做任何处理，
            }

            if (recv.title.Contains("防拆") || recv.msg.Contains("防拆"))
            {
                SaveMessageToFile(msg, "filter");
                return;//过滤掉有“防拆”字段的告警，没有意义。
            }

            SaveMessageToFile(msg,"");

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
                    da.DevInfo = di;
                    da.Device_desc = recv.deviceDesc;
                    da.AlarmTime = TimeConvert.ToDateTime(lTimeStamp);
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
                    bll.DevAlarmHistorys.Add(da_history);//告警恢复 放到历史数据中
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
