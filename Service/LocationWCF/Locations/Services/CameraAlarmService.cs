using BLL;
using DbModel;
using DbModel.Location.Alarm;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.Tool;
using LocationServer.Tools;
using Newtonsoft.Json;
using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCommunication.ExtremeVision;

namespace LocationServices.Locations.Services
{
    public class CameraAlarmService
    {
        public BLL.Bll db;

        public CameraAlarmService()
        {
            db = BLL.Bll.NewBllNoRelation();
        }

        public CameraAlarmService(BLL.Bll db)
        {
            this.db = db;
        }

        /// <summary>
        /// 获取全部告警
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public List<CameraAlarmInfo> GetAllCameraAlarms(bool merge)
        {
            try
            {
                List<CameraAlarmJson> list2 = db.CameraAlarmJsons.ToList();
                List<CameraAlarmInfo> list3 = new List<CameraAlarmInfo>();

                List<Dev_CameraInfo> cameras = db.Dev_CameraInfos.ToList();
                Dictionary<string, Dev_CameraInfo> cameraDict = new Dictionary<string, Dev_CameraInfo>();

                var devs = db.DevInfos.ToDictionary();

                foreach (Dev_CameraInfo camerainfo in cameras)
                {
                    if (devs.ContainsKey(camerainfo.DevInfoId))
                    {
                        camerainfo.DevInfo = devs[camerainfo.DevInfoId];
                    }

                    if (cameraDict.ContainsKey(camerainfo.Ip))
                    {
                        var cameraOld = cameraDict[camerainfo.Ip];
                        cameraDict[camerainfo.Ip] = camerainfo;
                    }
                    else
                    {
                        cameraDict.Add(camerainfo.Ip, camerainfo);
                    }
                }
                //todo:list2=>list3
                if (list2 != null)
                    foreach (CameraAlarmJson cameraJson in list2)
                    {
                        //byte[] byte1 = camera.Json;
                        //string json = Encoding.UTF8.GetString(byte1);
                        //CameraAlarmInfo cameraAlarmInfo = CameraAlarmInfo.Parse(json);
                        //cameraAlarmInfo.id = camera.Id;//增加了id,这样能够获取到详情
                        //cameraAlarmInfo.pic_data = "";//在详情的地方获取
                        //
                        //cameraAlarmInfo.data = null;

                        CameraAlarmInfo cameraAlarmInfo = GetCamaraAlarmInfo(cameraJson, cameraDict, devs);
                        //string key = cameraAlarmInfo.cid + cameraAlarmInfo.cid_url + "";//用key和

                        list3.Add(cameraAlarmInfo);
                    }

                list3.Sort();//按时间排序
                //if (merge)//默认传true
                //{
                //    list3 = MergeAlarms(list3);//合并相同的告警
                //}
                return list3.ToWCFList();
            }
            catch (Exception e)
            {
                Log.Error(LogTags.ExtremeVision, "GetAllCameraAlarms", e.ToString());
                return null;
            }
        }

        private Dictionary<string, string> urlToIp = new Dictionary<string, string>();

        private string GetCameraIp(string url)
        {
            if (urlToIp.ContainsKey(url))
            {
                string ip = urlToIp[url];
                return ip;
            }
            else
            {
                var rtsp = url; //"rtsp://admin:1111@192.168.108.107/13"
                //Log.Info(LogTags.DbGet, "rtsp:" + rtsp);
                string[] parts = rtsp.Split('@');
                if (parts.Length > 1)
                {
                    string[] parts2 = parts[1].Split('/', ':');
                    string ip = parts2[0];
                    urlToIp.Add(url, ip);
                    return ip;
                }
                else
                {
                    return "";
                }
            }


        }

        public void RemoveAlarmsOutOfDate(int keepDay)
        {
            try
            {
                var list = GetAllCameraAlarms(false);
                if (list == null)
                {
                    Log.Error(LogTags.ExtremeVision, "list == null");
                    return;
                }
                Log.Info(LogTags.ExtremeVision, string.Format("当前告警数量:{0},存储周期:{1}天", list.Count, keepDay));
                DateTime now = DateTime.Now;
                var listRemove = list.Where(i => (now - i.time).TotalDays > keepDay).ToList();
                if (listRemove.Count > 0)
                {
                    Log.Info(LogTags.ExtremeVision, "remove count:" + listRemove.Count);
                    foreach (var item in listRemove)
                    {
                        RemoveAlarm(item);
                    }
                    Log.Info(LogTags.ExtremeVision, "删除过时告警:" + listRemove.Count);
                }
                else
                {
                    Log.Info(LogTags.ExtremeVision, "无过时告警");
                }

            }
            catch (Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "RemoveAlarmsOutOfDate", ex.ToString());
            }

        }

        public bool RemoveAlarm(CameraAlarmInfo item)
        {
            try
            {
                if (item == null) return false;
                Log.Info(LogTags.ExtremeVision, "RemoveAlarm:" + item.pic_name);
                var entity = db.CameraAlarmJsons.DeleteById(item.id);
                var picName = item.pic_name;
                Picture pic = db.Pictures.Find(i => i.Name == picName);
                if (pic != null)
                {
                    db.Pictures.Remove(pic);
                }

                if (AppSetting.DeleteAlarmKeepPictureFile == false)
                {
                    FileInfo file = GetPictureFile(picName);
                    if (file != null)
                    {
                        file.Delete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "RemoveAlarm", ex.ToString());
                return false;
            }

        }

        private CameraAlarmInfo GetCamaraAlarmInfo(CameraAlarmJson camera, Dictionary<string, Dev_CameraInfo> cameraDict, Dictionary<int, DevInfo> devs)
        {
            try
            {
                byte[] byte1 = camera.Json;
                string json = Encoding.UTF8.GetString(byte1);
                CameraAlarmInfo cameraAlarmInfo = CameraAlarmInfo.Parse(json);
                cameraAlarmInfo.id = camera.Id;//增加了id,这样能够获取到详情
                cameraAlarmInfo.pic_data = "";//在详情的地方获取
                cameraAlarmInfo.data = null;
                cameraAlarmInfo.time = GetDataTime(cameraAlarmInfo.time_stamp);


                string ip = GetCameraIp(cameraAlarmInfo.cid_url);
                cameraAlarmInfo.DevIp = ip;

                if (cameraDict.ContainsKey(ip))
                {
                    var camerainfo = cameraDict[ip];
                    if (devs.ContainsKey(camerainfo.DevInfoId))
                    {
                        cameraAlarmInfo.DevName = devs[camerainfo.DevInfoId].Name;
                        cameraAlarmInfo.DevID = camerainfo.DevInfoId;
                    }
                }
                else
                {

                }

                return cameraAlarmInfo;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "GetCamaraAlarmInfo", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 获取某一个摄像机的告警
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public List<CameraAlarmInfo> GetCameraAlarms(string ip, bool merge)
        {
            try
            {
                List<CameraAlarmJson> list2 = db.CameraAlarmJsons.ToList();
                List<CameraAlarmInfo> list3 = new List<CameraAlarmInfo>();

                List<Dev_CameraInfo> cameras = db.Dev_CameraInfos.ToList();
                Dictionary<string, Dev_CameraInfo> cameraDict = new Dictionary<string, Dev_CameraInfo>();

                var devs = db.DevInfos.ToDictionary();

                foreach (Dev_CameraInfo camerainfo in cameras)
                {
                    if (devs.ContainsKey(camerainfo.DevInfoId))
                    {
                        camerainfo.DevInfo = devs[camerainfo.DevInfoId];
                    }

                    if (cameraDict.ContainsKey(camerainfo.Ip))
                    {
                        var cameraOld = cameraDict[camerainfo.Ip];
                        cameraDict[camerainfo.Ip] = camerainfo;
                    }
                    else
                    {
                        cameraDict.Add(camerainfo.Ip, camerainfo);
                    }
                }
                //todo:list2=>list3
                if (list2 != null)
                    foreach (CameraAlarmJson camera in list2)
                    {
                        //byte[] byte1 = camera.Json;
                        //string json = Encoding.UTF8.GetString(byte1);
                        //CameraAlarmInfo cameraAlarmInfo = CameraAlarmInfo.Parse(json);
                        //if (!cameraAlarmInfo.cid_url.Contains(ip)) continue;//不是相同的摄像机
                        //cameraAlarmInfo.id = camera.Id;//增加了id,这样能够获取到详情
                        //cameraAlarmInfo.pic_data = "";//在详情的地方获取
                        //cameraAlarmInfo.time = GetDataTime(cameraAlarmInfo.time_stamp);
                        //cameraAlarmInfo.data = null;

                        CameraAlarmInfo cameraAlarmInfo = GetCamaraAlarmInfo(camera, cameraDict, devs);
                        if (!cameraAlarmInfo.cid_url.Contains(ip)) continue;//不是相同的摄像机
                        list3.Add(cameraAlarmInfo);
                    }

                list3.Sort();//按时间排序
                //if (merge) //默认传true
                //{
                //    list3 = MergeAlarms(list3);//合并相同的告警
                //}
                return list3.ToWCFList();
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "GetCameraAlarms", ex.ToString());
                return null;
            }
        }

        public void LoadAlarmFromJson()
        {
            try
            {
                var list2 = GetAllCameraAlarms(false);
                Dictionary<string, CameraAlarmInfo> dict = new Dictionary<string, CameraAlarmInfo>();
                foreach (var item in list2)
                {
                    string picName = item.pic_name;
                    //picName = picName.Replace(".jpg", "");
                    if (dict.ContainsKey(picName))
                    {
                        var itemOld = dict[picName];
                        dict[picName] = item;
                    }
                    else
                    {
                        dict.Add(picName, item);
                    }
                }

                DirectoryInfo dir = CameraAlarmService.GetJsonDir();

                FileInfo[] files = dir.GetFiles();

                int count = 0;
                foreach (var item in files)
                {
                    string json = File.ReadAllText(item.FullName);
                    CameraAlarmInfo cameraAlarmInfo = CameraAlarmInfo.Parse(json);
                    if (dict.ContainsKey(cameraAlarmInfo.pic_name))//已经存在了
                    {

                    }
                    else
                    {
                        Log.Info(LogTags.ExtremeVision, "添加到数据库:" + cameraAlarmInfo.pic_name);
                        SaveToCameraAlarmJson(json);//保存到数据库中
                        count++;
                    }
                }

                Log.Info(LogTags.ExtremeVision, "LoadAlarmFromJson count:" + count);
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "LoadAlarmFromJson", ex.ToString());
            }
        }

        public void AlarmSaveToJsonAll()
        {
            try
            {
                List<CameraAlarmJson> list2 = db.CameraAlarmJsons.ToList();
                foreach (var item in list2)
                {
                    AlarmSaveToJson(item);
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "AlarmSaveToJsonAll", ex.ToString());
            }
        }

        /// <summary>
        /// 获取一个告警的详情 主要是告警图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CameraAlarmInfo GetCameraAlarmDetail(int id)
        {
            try
            {
                CameraAlarmJson camera = db.CameraAlarmJsons.Find(id);
                if (camera == null) return null;
                byte[] byte1 = camera.Json;
                string json = Encoding.UTF8.GetString(byte1);
                CameraAlarmInfo cameraAlarmInfo = CameraAlarmInfo.Parse(json);
                cameraAlarmInfo.id = camera.Id;//增加了id,这样能够获取到详情
                cameraAlarmInfo.time = GetDataTime(cameraAlarmInfo.time_stamp);
                GetCameraAlarmPicture(cameraAlarmInfo);
                return cameraAlarmInfo;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "GetCameraAlarmDetail", ex.ToString());
                return null;
            }
        }

        //public Picture GetCameraAlarmPicture(int id)
        //{
        //    CameraAlarmJson camera = db.CameraAlarmJsons.Find(id);
        //    byte[] byte1 = camera.Json;
        //    string json = Encoding.UTF8.GetString(byte1);
        //    CameraAlarmInfo cameraAlarmInfo = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
        //    Picture pic = GetCameraAlarmPicture(cameraAlarmInfo.pic_name);
        //    return pic;
        //}

        public void GetCameraAlarmPicture(CameraAlarmInfo cameraAlarmInfo)
        {
            try
            {
                if (cameraAlarmInfo == null) return;

                if (AppSetting.CameraAlarmPicSaveMode == 0)
                {
                    bool r = GetCameraAlarmPictureFromDb(cameraAlarmInfo);
                    if (r == false)
                    {
                        Log.Info(LogTags.ExtremeVision, string.Format("Mode={0};Name={1}. GetCameraAlarmPictureFromDb Fail,Try GetCameraAlarmPictureFromFile...", AppSetting.CameraAlarmPicSaveMode, cameraAlarmInfo.pic_name));
                        GetCameraAlarmPictureFromFile(cameraAlarmInfo);
                    }
                }
                else if (AppSetting.CameraAlarmPicSaveMode == 1)
                {
                    bool r = GetCameraAlarmPictureFromFile(cameraAlarmInfo);
                    if (r == false)
                    {
                        Log.Info(LogTags.ExtremeVision, string.Format("Mode={0};Name={1}. GetCameraAlarmPictureFromFile Fail,Try GetCameraAlarmPictureFromDb...", AppSetting.CameraAlarmPicSaveMode, cameraAlarmInfo.pic_name));
                        GetCameraAlarmPictureFromDb(cameraAlarmInfo);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "GetCameraAlarmPicture", ex.ToString());
            }
        }

        private static bool GetCameraAlarmPictureFromFile(CameraAlarmInfo cameraAlarmInfo)
        {
            try
            {
                FileInfo file = GetPictureFile(cameraAlarmInfo.pic_name);
                if (file != null)
                {
                    byte[] imageBytes = File.ReadAllBytes(file.FullName);
                    string base64 = Convert.ToBase64String(imageBytes);
                    cameraAlarmInfo.pic_data = base64;
                    //byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);
                    //pic.Info = base64Bytes;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "GetCameraAlarmPictureFromFile", ex.ToString());
                return false;
            }
        }


        public static FileInfo GetPictureFile(string fileName)
        {
            try
            {
                string picName = fileName;
                string dir = AppSetting.CameraAlarmPicSaveDir;
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                if (dirInfo.Exists)
                {
                    FileInfo[] files = dirInfo.GetFiles(picName);
                    if (files.Length > 0)
                    {
                        FileInfo file = files[0];
                        return file;
                    }

                }
                return null;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "GetPictureFile", ex.ToString());
                return null;
            }
        }
        public FileInfo[] GetAllPictureFiles()
        {
            string dir = AppSetting.CameraAlarmPicSaveDir;
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            if (dirInfo.Exists)
            {
                return dirInfo.GetFiles();
            }
            return null;
        }

        public List<Picture> GetAllPictures()
        {
            var list = db.Pictures.FindAll(i => i.Name != "顶视图").ToList();
            return list;
        }

        public int GetPictureCount()
        {
            var list = db.Pictures.DbSet.Where(i => i.Name != "顶视图").Count();
            return list;
        }

        private bool GetCameraAlarmPictureFromDb(CameraAlarmInfo cameraAlarmInfo)
        {
            string picName = cameraAlarmInfo.pic_name;
            //int count = db.Pictures.DbSet.Count();
            Picture pic = db.Pictures.Find(i => i.Name == picName);
            if (pic != null)
            {
                cameraAlarmInfo.pic_data = Encoding.UTF8.GetString(pic.Info);
                return true;
            }
            return false;
        }

        //public Picture GetCameraAlarmPicture(string picName)
        //{
        //    Picture pic = null;
        //    if (AppSetting.CameraAlarmPicSaveMode == 0)
        //    {
        //        pic = db.Pictures.Find(i => i.Name == picName);
        //    }
        //    else if (AppSetting.CameraAlarmPicSaveMode == 1)
        //    {
        //        string dir = AppSetting.CameraAlarmPicSaveDir;
        //        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        //        if (dirInfo.Exists)
        //        {
        //            FileInfo[] files=dirInfo.GetFiles(picName);
        //            if (files.Length > 0)
        //            {
        //                FileInfo file = files[0];
        //                byte[] imageBytes=File.ReadAllBytes(file.FullName);
        //                string base64 = Convert.ToBase64String(imageBytes);
        //                byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);
        //                pic.Info = base64Bytes;
        //            }
        //        }
        //    }
        //    return pic;
        //}

        public static DirectoryInfo GetJsonDir()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\CameraAlarms\\";
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                dir.Create();
            }
            return dir;
        }

        public static FileInfo GetNowJsonFile()
        {
            DateTime now = DateTime.Now;

            return GetJsonFile(now, "_raw");
        }

        public static FileInfo GetJsonFile(DateTime now, string raw = "")
        {
            string path = GetJsonDir().FullName + now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + raw + ".json";//yyyy_mm_dd_HH_MM_ss_fff=>yyyy_MM_dd_HH_mm_ss_fff
            FileInfo fi = new FileInfo(path);
            return fi;
        }

        public string ParseJson(string json, int mode)
        {
            try
            {
                FileInfo fi = GetNowJsonFile();
                File.WriteAllText(fi.FullName, json);

                var info = CameraAlarmInfo.Parse(json);
                CameraAlarmHub.SendInfo(info);//发送告警给客户端

                Bll bll = Bll.NewBllNoRelation();

                string base64 = info.pic_data;
                info.pic_data = "";//图片分开存

                string jsonNoPic = JsonConvert.SerializeObject(info);//新的没有图片的json
                Log.Info(LogTags.ExtremeVision, jsonNoPic);
                string alarmType = "";
                if (info.AlarmType == 1)
                {
                    alarmType = "安全帽告警";
                }
                else if (info.AlarmType == 2)
                {
                    alarmType = "火焰告警";
                }
                else if (info.AlarmType == 3)
                {
                    alarmType = "烟雾告警";
                }
                else
                {
                    alarmType = "其他告警:" + info.AlarmType;
                }

                Log.Info(LogTags.ExtremeVision, "告警类型:" + alarmType);

                bool result = SaveToCameraAlarmJson(jsonNoPic);

                var picName = info.pic_name;

                SavePicture(bll, mode, base64, picName);
                return info.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "Error:" + ex);
                return "Error:" + ex.Message;
            }

        }

        private bool SaveToCameraAlarmJson(string json)
        {
            byte[] byte1 = Encoding.UTF8.GetBytes(json);
            CameraAlarmJson camera = new CameraAlarmJson();
            camera.Json = byte1;
            bool result = db.CameraAlarmJsons.Add(camera);//存到数据库中    
            return result;
        }

        public DateTime GetDataTime(long time_stamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            long lTime = ((long)time_stamp * 10000000);
            TimeSpan toNow = new TimeSpan(lTime);
            var toNowNew = toNow.Add(TimeSpan.FromHours(8));
            DateTime AlarmTime = dtStart.Add(toNowNew);
            return AlarmTime;
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<CameraAlarmInfo> MergeAlarms(List<CameraAlarmInfo> input)
        {

            try
            {
                Dictionary<string, List<CameraAlarmInfo>> dict = new Dictionary<string, List<CameraAlarmInfo>>();
                foreach (CameraAlarmInfo info in input)
                {
                    string key = info.cid + info.cid_url;//用aid和ip作为键值
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, new List<CameraAlarmInfo>());
                    }
                    List<CameraAlarmInfo> list = dict[key];
                    list.Add(info);
                }

                List<CameraAlarmInfo> output = new List<CameraAlarmInfo>();
                int mergeInterval = 35;//合并时间间隔
                foreach (string key in dict.Keys)
                {
                    var list = dict[key];
                    List<TimeSpan> timeSpane = new List<TimeSpan>();
                    CameraAlarmInfo startInfo = null;
                    for (int i = 0; i < list.Count - 1; i++)//一个摄像机上的相同告警，火警和安全帽是分开的
                    {
                        if (startInfo == null)
                        {
                            startInfo = list[i];
                        }

                        TimeSpan t = list[i + 1].time - list[i].time;
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

                    output.Add(list[list.Count - 1]);
                }
                return output;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "GetPictureFile", ex.ToString());
                return null;
            }
;
        }


        public void GetImage(string base64)
        {
            base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            byte[] bytes = Convert.FromBase64String(base64);
            //string imagebase64 = base64.Substring(base64.IndexOf(",") + 1);

            MemoryStream memStream = new MemoryStream(bytes);
            System.Drawing.Image mImage = System.Drawing.Image.FromStream(memStream);
            string path = AppDomain.CurrentDomain.BaseDirectory + "1.jpg";
            mImage.Save(path);

            //BitmapImage bi = new BitmapImage();
            //bi.BeginInit();
            //bi.StreamSource = new MemoryStream(bytes);
            //bi.EndInit();
            //Image1.Source = bi;
        }

        /// <summary>
        /// 保存Json到文件中，同时从告警表分离到图片表或者文件中
        /// </summary>
        /// <param name="callBack"></param>
        public void SeparateImages(Action callBack, int mode, int progressBagCount = 5)
        {
            Log.Info(LogTags.ExtremeVision, "开始 保存Json到文件中，同时从告警表分离到图片表或者文件中");
            Worker.Run(() =>
            {
                //int progressBagCount = 5;

                bool r = true;
                while (r)
                {
                    try
                    {
                        LocationService s = new LocationService();
                        //var list=s.GetAllCameraAlarms(true);
                        var bll = Bll.NewBllNoRelation();
                        int count = bll.CameraAlarmJsons.DbSet.Count();
                        Log.Info("count:" + count);
                        List<CameraAlarmJson> list2 = bll.CameraAlarmJsons.ToList();
                        Log.Info(LogTags.ExtremeVision, "获取到列表");
                        if (list2 != null)
                        {
                            Log.Info(LogTags.ExtremeVision, "成功");
                            for (int i1 = 0; i1 < list2.Count; i1++)
                            {
                                if (i1 % progressBagCount == 0)
                                {
                                    Log.Info(LogTags.ExtremeVision, string.Format("进度:{0}/{1}", i1, list2.Count));
                                }

                                CameraAlarmJson camera = list2[i1];
                                SavePicture(camera, bll, mode);
                            }
                        }
                        else
                        {
                            Log.Info(LogTags.ExtremeVision, "失败");
                            Log.Info(LogTags.ExtremeVision, "太多了取不出来，一个一个取");
                            for (int i = 0; i < count; i++)
                            {
                                if (i % progressBagCount == 0)
                                {
                                    Log.Info(LogTags.ExtremeVision, string.Format("进度:{0}/{1}", i, count));
                                }
                                CameraAlarmJson camera = bll.CameraAlarmJsons.Find(i + 1);
                                if (camera == null)
                                {
                                    Log.Info("找不到id:" + (i + 1));
                                    continue;
                                }

                                SavePicture(camera, bll, mode);
                            }
                        }
                        Log.Info(LogTags.ExtremeVision, "完成");
                        r = false;//真的完成
                    }
                    catch (Exception exception)
                    {
                        Log.Error(LogTags.ExtremeVision, exception.Message);
                    }
                }
            }, () =>
            {
                if (callBack != null)
                {
                    callBack();
                }
            });
        }

        /// <summary>
        /// 从图片表分离到文件中
        /// </summary>
        /// <param name="callBack"></param>
        public void SeparateImages_PicToFile(Action callBack)
        {
            Log.Info(LogTags.ExtremeVision, "开始 从图片表分离到文件中");

            Worker.Run(() =>
            {
                try
                {
                    //var bll = Bll.NewBllNoRelation();
                    int picCount = GetPictureCount();
                    if (picCount == 0)
                    {
                        Log.Info(LogTags.ExtremeVision, "Picture表中没有告警图片");
                        return;
                    }

                    int count = db.Pictures.DbSet.Count();
                    Log.Info(LogTags.ExtremeVision, "pic count:" + count);

                    LocationService s = new LocationService();
                    var list = s.GetAllCameraAlarms(false);

                    Log.Info(LogTags.ExtremeVision, "alarm count:" + list.Count);

                    List<string> picNameList = new List<string>();
                    foreach (var item in list)
                    {
                        if (!picNameList.Contains(item.pic_name))
                        {
                            picNameList.Add(item.pic_name);
                        }
                    }

                    Log.Info(LogTags.ExtremeVision, "pic count 2:" + picNameList.Count);

                    //return;
                    //for (int i1 = 0; i1 < picNameList.Count; i1++)
                    //{
                    //    Log.Info(string.Format("进度:{0}/{1}", i1, picNameList.Count));
                    //    //CameraAlarmInfo camera = picNameList[i1];
                    //    //SavePicture(camera, bll);
                    //    //Picture pic = s.GetCameraAlarmPicture(picNameList[i1]);
                    //    string picName = picNameList[i1];
                    //    Picture pic = db.Pictures.Find(i => i.Name == picName);
                    //    if (pic == null) continue;//已经提取出来的
                    //    SaveFile(pic);
                    //    var r = db.Pictures.DeleteById(pic.Id);
                    //}

                    List<Picture> upViewList = new List<Picture>();

                    var picList = db.Pictures.ToList();

                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            Log.Info(LogTags.ExtremeVision, string.Format("进度:{0}/{1}", i, count));
                            Picture pic = picList[i];
                            if (pic == null) continue;
                            if (pic.Name != "顶视图")
                            {
                                PictureSaveFile(pic);
                                var r = db.Pictures.DeleteById(pic.Id);
                            }
                            else
                            {
                                upViewList.Add(pic);//定视图
                            }
                        }
                        catch (Exception ex1)
                        {
                            Log.Error(LogTags.ExtremeVision, ex1.Message);
                        }
                    }

                    if (upViewList.Count > 1)
                    {
                        for (int i = 1; i < upViewList.Count; i++)
                        {
                            var r = db.Pictures.DeleteById(upViewList[i].Id);//重复的顶视图图片
                        }
                    }

                    Log.Info(LogTags.ExtremeVision, "完成");
                }
                catch (Exception ex2)
                {
                    Log.Error(LogTags.ExtremeVision, ex2.Message);
                }

            }, () =>
            {
                if (callBack != null)
                {
                    callBack();
                }
            });
        }

        private static void Base64SaveToFile(string picName, string base64)
        {
            string dir = AppSetting.CameraAlarmPicSaveDir;
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (dirInfo.Exists == false)
            {
                dirInfo.Create();
            }
            string filePath = dir + "\\" + picName;
            base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            byte[] bytes = Convert.FromBase64String(base64);
            System.IO.File.WriteAllBytes(filePath, bytes);
        }

        private static void PictureSaveFile(Picture pic)
        {
            ////Log.Info(string.Format("进度:{0}/{1},[{2}]", i1, list.Count, r!=null));
            //string filePath = dirPath + pic.Name;
            string base64 = Encoding.UTF8.GetString(pic.Info);
            //base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            //byte[] bytes = Convert.FromBase64String(base64);
            //System.IO.File.WriteAllBytes(filePath, bytes);

            Base64SaveToFile(pic.Name, base64);
        }

        private void SavePicture(CameraAlarmJson camera, Bll bll, int mode)
        {
            byte[] byte1 = camera.Json;
            string json = Encoding.UTF8.GetString(byte1);
            CameraAlarmInfo info = CameraAlarmInfo.Parse(json);
            info.time = GetDataTime(info.time_stamp);
            info.id = camera.Id; //增加了id,这样能够获取到详情
            //int mode = AppSetting.CameraAlarmPicSaveMode;
            string base64 = info.pic_data;
            if (!string.IsNullOrEmpty(base64))
            {
                info.pic_data = ""; //图片分开存
                string json2 = JsonConvert.SerializeObject(info); //新的没有图片的json
                camera.Json = Encoding.UTF8.GetBytes(json2);

                bll.CameraAlarmJsons.Edit(camera);

                SavePicture(bll, mode, base64, info.pic_name);

                SaveJsonToFile(info.time_stamp, json2);
            }
            else
            {
                Log.Info("没有图片");
                SaveJsonToFile(info.time_stamp, json);
            }
        }

        private void SaveJsonToFile(long timeStamp, string json)
        {
            DateTime now = GetDataTime(timeStamp);
            FileInfo fi = CameraAlarmService.GetJsonFile(now);
            File.WriteAllText(fi.FullName, json);//yyyy_mm_dd_HH_MM_ss_fff=>yyyy_MM_dd_HH_mm_ss_fff
        }

        private static void SavePicture(Bll bll, int mode, string base64, string picName)
        {
            if (mode == 0)
            {
                bll.Pictures.Update(picName, Encoding.UTF8.GetBytes(base64));
            }
            else if (mode == 1)
            {
                //保存到文件中
                Base64SaveToFile(picName, base64);
            }
            else
            {
                bll.Pictures.Update(picName, Encoding.UTF8.GetBytes(base64));
            }
        }

        public void AlarmSaveToJson(CameraAlarmJson camera)
        {
            byte[] byte1 = camera.Json;
            string json = Encoding.UTF8.GetString(byte1);
            CameraAlarmInfo info = CameraAlarmInfo.Parse(json);
            info.id = camera.Id; //增加了id,这样能够获取到详情

            string pic = info.pic_data;

            DateTime now = GetDataTime(info.time_stamp);
            FileInfo fi = CameraAlarmService.GetJsonFile(now);
        }
    }
}
