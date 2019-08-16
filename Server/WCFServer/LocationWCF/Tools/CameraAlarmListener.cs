using BLL;
using DbModel.Location.Alarm;
using Location.BLL.Tool;
using Newtonsoft.Json;
using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCommunication.ExtremeVision;

namespace LocationServer.Tools
{
    public class CameraAlarmListener
    {
        public string Url;

        public int SaveMode;

        public MyHttpListener httpListener;

        public CameraAlarmListener(string url,int saveMode)
        {
            this.Url = url;
            this.SaveMode = saveMode;

            httpListener = new MyHttpListener(url);
            httpListener.OnReceived += (json) =>
            {
                return ParseCameraAlarm(url, json);
            };
        }

        public bool Start()
        {
            try
            {
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
                //Log.Info(LogTags.ExtremeVision, json);
                DateTime now = DateTime.Now;

                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\CameraAlarms\\" + now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".json";
                FileInfo fi = new FileInfo(path);
                if (!fi.Directory.Exists)
                    fi.Directory.Create();

                File.WriteAllText(path, json);//yyyy_mm_dd_HH_MM_ss_fff=>yyyy_MM_dd_HH_mm_ss_fff

                var info = CameraAlarmInfo.Parse(json);
                CameraAlarmHub.SendInfo(info);//发送告警给客户端

                Bll bll = Bll.NewBllNoRelation();

                string pic = info.pic_data;
                info.pic_data = "";//图片分开存

                string json2 = JsonConvert.SerializeObject(info);//新的没有图片的json
                Log.Info(LogTags.ExtremeVision, json2);


                byte[] byte1 = Encoding.UTF8.GetBytes(json2);
                CameraAlarmJson camera = new CameraAlarmJson();
                camera.Json = byte1;
                bool result = bll.CameraAlarmJsons.Add(camera);//存到数据库中    

                var picName = info.pic_name;

                if (SaveMode == 0)
                {
                    bll.Pictures.Update(picName, Encoding.UTF8.GetBytes(pic));
                }
                

                return info.ToString();
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }

        }
    }
}
