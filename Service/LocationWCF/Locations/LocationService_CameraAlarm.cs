using System;
using System.Collections.Generic;
using System.Text;
using DbModel.Tools;
using WebApiCommunication.ExtremeVision;
using DbModel.Location.Alarm;
using DbModel.Location.AreaAndDev;
using Newtonsoft.Json;

namespace LocationServices.Locations
{
    //告警定位相关接口
    public partial class LocationService : ILocationService, IDisposable
    {
        /// <summary>
        /// 获取全部告警
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public List<CameraAlarmInfo> GetAllCameraAlarms(bool merge)
        {
            List<CameraAlarmJson> list2 = db.CameraAlarmJsons.ToList();
            List<CameraAlarmInfo> list3 = new List<CameraAlarmInfo>();
            //todo:list2=>list3
            if(list2!=null)
                foreach (CameraAlarmJson camera in list2)
                {
                    byte[] byte1 = camera.Json;
                    string json = Encoding.UTF8.GetString(byte1);
                    CameraAlarmInfo cameraAlarmInfo = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
                    cameraAlarmInfo.id = camera.Id;//增加了id,这样能够获取到详情
                    cameraAlarmInfo.pic_data = "";//在详情的地方获取
                    cameraAlarmInfo.time = GetDataTime(cameraAlarmInfo.time_stamp);
                    cameraAlarmInfo.ParseData();
                    string key = cameraAlarmInfo.cid + cameraAlarmInfo.cid_url + "";//用key和
                    list3.Add(cameraAlarmInfo);
                }

            list3.Sort();//按时间排序
            if (merge)//默认传true
            {
                list3 = MergeAlarms(list3);//合并相同的告警
            }
            return list3.ToWCFList();
        }

        public DateTime GetDataTime(long time_stamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            long lTime = ((long)time_stamp * 10000000);
            TimeSpan toNow = new TimeSpan(lTime);
            var toNowNew=toNow.Add(TimeSpan.FromHours(8));
            DateTime AlarmTime = dtStart.Add(toNowNew);
            return AlarmTime;
        }

        /// <summary>
        /// 获取某一个摄像机的告警
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public List<CameraAlarmInfo> GetCameraAlarms(string ip, bool merge)
        {
            List<CameraAlarmJson> list2 = db.CameraAlarmJsons.ToList();
            List<CameraAlarmInfo> list3 = new List<CameraAlarmInfo>();
            //todo:list2=>list3
            if (list2 != null)
                foreach (CameraAlarmJson camera in list2)
                {
                    byte[] byte1 = camera.Json;
                    string json = Encoding.UTF8.GetString(byte1);
                    CameraAlarmInfo cameraAlarmInfo = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
                    if (!cameraAlarmInfo.cid_url.Contains(ip)) continue;//不是相同的摄像机
                    cameraAlarmInfo.id = camera.Id;//增加了id,这样能够获取到详情
                    cameraAlarmInfo.pic_data = "";//在详情的地方获取
                    cameraAlarmInfo.ParseData();
                    cameraAlarmInfo.time = GetDataTime(cameraAlarmInfo.time_stamp);
                    list3.Add(cameraAlarmInfo);
                }

            list3.Sort();//按时间排序
            if (merge) //默认传true
            {
                list3 = MergeAlarms(list3);//合并相同的告警
            }
            return list3.ToWCFList();
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<CameraAlarmInfo> MergeAlarms(List<CameraAlarmInfo> input)
        {
           
            Dictionary<string, List<CameraAlarmInfo>> dict = new Dictionary<string, List<CameraAlarmInfo>>();
            foreach (CameraAlarmInfo info in input)
            {
                string key = info.cid + info.cid_url ;//用aid和ip作为键值
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key,new List<CameraAlarmInfo>());
                }
                List<CameraAlarmInfo> list = dict[key];
                list.Add(info);
            }

            List<CameraAlarmInfo> output = new List<CameraAlarmInfo>();
            int mergeInterval = 35;//合并时间间隔
            foreach (string key in dict.Keys)
            {
                var list = dict[key];
                List< TimeSpan > timeSpane=new List<TimeSpan>();
                CameraAlarmInfo startInfo = null;
                for (int i = 0; i < list.Count-1; i++)//一个摄像机上的相同告警，火警和安全帽是分开的
                {
                    if (startInfo == null)
                    {
                        startInfo = list[i];
                    }
                   
                    TimeSpan t = list[i + 1].time- list[i].time;
                    timeSpane.Add(t);
                    if (t.TotalSeconds < mergeInterval)
                    {
                        list[i + 1].startInfo = startInfo;
                        list.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        output.Add(list[i]);
                        startInfo = null;
                    }
                }

                output.Add(list[list.Count-1]);
            }
            return output;
        }

        public CameraAlarmInfo GetCameraAlarm(int id)
        {
            CameraAlarmJson camera=db.CameraAlarmJsons.Find(id);
            byte[] byte1 = camera.Json;
            string json = Encoding.UTF8.GetString(byte1);
            CameraAlarmInfo cameraAlarmInfo = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
            cameraAlarmInfo.id = camera.Id;//增加了id,这样能够获取到详情
            cameraAlarmInfo.ParseData();

            Picture pic = db.Pictures.Find(i => i.Name == cameraAlarmInfo.pic_name);
            if (pic != null)
            {
                cameraAlarmInfo.pic_data = Encoding.UTF8.GetString(pic.Info);
            }
            return cameraAlarmInfo;
        }

        
    }
}
