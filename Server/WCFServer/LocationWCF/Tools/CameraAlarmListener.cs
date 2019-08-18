using BLL;
using DbModel;
using DbModel.Location.Alarm;
using Location.BLL.Tool;
using LocationServices.Locations.Services;
using Newtonsoft.Json;
using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApiCommunication.ExtremeVision;

namespace LocationServer.Tools
{
    public class CameraAlarmListener
    {
        public string Url;

        public int SaveMode;

        public string SaveDir;

        public MyHttpListener httpListener;

        public CameraAlarmListener(string url,int saveMode,string saveDir)
        {
            this.Url = url;
            this.SaveMode = saveMode;
            this.SaveDir = saveDir;

            httpListener = new MyHttpListener(url);
            httpListener.OnReceived += (json) =>
            {
                return ParseCameraAlarm(url, json);
            };

           
        }

        Thread reomveThread;

        private void RemoveAlarmsOutOfDate()
        {
            
            int keepDay = AppSetting.CameraAlarmKeepDay;
            Log.Info(LogTags.ExtremeVision, "CameraAlarmKeepDay:"+ keepDay);
            if (keepDay > 0)
            {
                Log.Info(LogTags.ExtremeVision, "RemoveAlarmsOutOfDate Start");
                while (true)
                {
                    CameraAlarmService service = new CameraAlarmService();
                    service.RemoveAlarmsOutOfDate(keepDay);
                    //Thread.Sleep(1000 * 60);//测试
                    Thread.Sleep(1000 * 60 * 60);//1小时监测一次
                }
            }
            
        }

        public bool Start()
        {
            try
            {
                reomveThread = new Thread(RemoveAlarmsOutOfDate);
                reomveThread.IsBackground = true;
                reomveThread.Start();

                if (httpListener != null)
                {
                    return httpListener.Start();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.Error(LogTags.ExtremeVision, "CameraAlarmListener.Start:" + e.Message);
                return false;
            }
        }

        public void Stop()
        {
            if (reomveThread != null)
            {
                try
                {
                    reomveThread.Abort();
                reomveThread = null;
                }
                catch (Exception e)
                {
                    Log.Error(LogTags.ExtremeVision, "CameraAlarmListener.Stop:" + e.Message);
                }
            }

            if (httpListener != null)
            {
                try
                {
                    httpListener.Stop();
                    httpListener = null;
                }
                catch (Exception e)
                {
                    Log.Error(LogTags.ExtremeVision, "CameraAlarmListener.Stop:"+e.Message);
                }
            }
        }

        private string ParseCameraAlarm(string url, string json)
        {
            try
            {
                Log.Info(LogTags.ExtremeVision, string.Format("收到消息({0})", url));

                CameraAlarmService service = new CameraAlarmService();
                string result=service.ParseJson(json, SaveMode);

                return result;
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }

        }
    }
}
