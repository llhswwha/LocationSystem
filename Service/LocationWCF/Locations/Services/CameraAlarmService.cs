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
            List<CameraAlarmJson> list2 = db.CameraAlarmJsons.ToList();
            List<CameraAlarmInfo> list3 = new List<CameraAlarmInfo>();
            //todo:list2=>list3
            if (list2 != null)
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

        public CameraAlarmInfo GetCameraAlarm(int id)
        {
            CameraAlarmJson camera = db.CameraAlarmJsons.Find(id);
            byte[] byte1 = camera.Json;
            string json = Encoding.UTF8.GetString(byte1);
            CameraAlarmInfo cameraAlarmInfo = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
            cameraAlarmInfo.id = camera.Id;//增加了id,这样能够获取到详情
            cameraAlarmInfo.ParseData();

            //Picture pic = GetCameraAlarmPicture(cameraAlarmInfo.pic_name);
            //if (pic != null)
            //{
            //    cameraAlarmInfo.pic_data = Encoding.UTF8.GetString(pic.Info);
            //}
            GetCameraAlarmPicture(cameraAlarmInfo);
            return cameraAlarmInfo;
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
            if (cameraAlarmInfo == null) return;
            string picName = cameraAlarmInfo.pic_name;
            if (AppSetting.CameraAlarmPicSaveMode == 0)
            {
                Picture pic = db.Pictures.Find(i => i.Name == picName);
                if (pic != null)
                {
                    cameraAlarmInfo.pic_data = Encoding.UTF8.GetString(pic.Info);
                }
            }
            else if (AppSetting.CameraAlarmPicSaveMode == 1)
            {
                string dir = AppSetting.CameraAlarmPicSaveDir;
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                if (dirInfo.Exists)
                {
                    FileInfo[] files = dirInfo.GetFiles(picName);
                    if (files.Length > 0)
                    {
                        FileInfo file = files[0];
                        byte[] imageBytes = File.ReadAllBytes(file.FullName);
                        string base64 = Convert.ToBase64String(imageBytes);
                        cameraAlarmInfo.pic_data = base64;
                        //byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);
                        //pic.Info = base64Bytes;
                    }
                }
            }
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

        public string ParseJson(string json,int mode)
        {
            DateTime now = DateTime.Now;

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\CameraAlarms\\" + now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".json";
            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
                fi.Directory.Create();

            File.WriteAllText(path, json);//yyyy_mm_dd_HH_MM_ss_fff=>yyyy_MM_dd_HH_mm_ss_fff

            var info = CameraAlarmInfo.Parse(json);
            CameraAlarmHub.SendInfo(info);//发送告警给客户端

            Bll bll = Bll.NewBllNoRelation();

            string base64 = info.pic_data;
            info.pic_data = "";//图片分开存

            string jsonNoPic = JsonConvert.SerializeObject(info);//新的没有图片的json
            Log.Info(LogTags.ExtremeVision, jsonNoPic);


            byte[] byte1 = Encoding.UTF8.GetBytes(jsonNoPic);
            CameraAlarmJson camera = new CameraAlarmJson();
            camera.Json = byte1;
            bool result = bll.CameraAlarmJsons.Add(camera);//存到数据库中    

            var picName = info.pic_name;

            if (mode == 0)
            {
                bll.Pictures.Update(picName, Encoding.UTF8.GetBytes(base64));
            }
            else if (mode == 0)
            {
                //保存到文件中
                SaveToFile(picName, base64);
            }
            else
            {
                bll.Pictures.Update(picName, Encoding.UTF8.GetBytes(base64));
            }


            return info.ToString();
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
        /// 分离图片，从告警表分离到图片表
        /// </summary>
        /// <param name="callBack"></param>
        public void SeparateImages_ToPictures(Action callBack)
        {
            Worker.Run(() =>
            {
                bool r = true;
                while (r)
                {
                    try
                    {
                        Log.Info("开始");

                        LocationService s = new LocationService();
                        //var list=s.GetAllCameraAlarms(true);
                        var bll = Bll.NewBllNoRelation();
                        int count = bll.CameraAlarmJsons.DbSet.Count();
                        Log.Info("count:" + count);
                        List<CameraAlarmJson> list2 = bll.CameraAlarmJsons.ToList();
                        Log.Info("获取到列表");
                        if (list2 != null)
                        {
                            Log.Info("成功");
                            for (int i1 = 0; i1 < list2.Count; i1++)
                            {
                                Log.Info(string.Format("进度:{0}/{1}", i1, list2.Count));
                                CameraAlarmJson camera = list2[i1];
                                SavePicture(camera, bll);
                            }
                        }
                        else
                        {
                            Log.Info("失败");
                            Log.Info("太多了取不出来，一个一个取");
                            for (int i = 0; i < count; i++)
                            {
                                Log.Info(string.Format("进度:{0}/{1}", i, count));
                                CameraAlarmJson camera = bll.CameraAlarmJsons.Find(i + 1);
                                if (camera == null)
                                {
                                    Log.Info("找不到id:" + (i + 1));
                                    continue;
                                }

                                SavePicture(camera, bll);
                            }
                        }
                        Log.Info("完成");
                        r = false;//真的完成
                    }
                    catch (Exception exception)
                    {
                        //出异常
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
        public void SeparateImages_ToFile(Action callBack)
        {
            Worker.Run(() =>
            {
                try
                {
                    Log.Info("开始");

                    //var bll = Bll.NewBllNoRelation();
                    int count = db.Pictures.DbSet.Count();
                    Log.Info("pic count:" + count);

                    LocationService s = new LocationService();
                    var list = s.GetAllCameraAlarms(false);

                    Log.Info("alarm count:" + list.Count);

                    List<string> picNameList = new List<string>();
                    foreach (var item in list)
                    {
                        if (!picNameList.Contains(item.pic_name))
                        {
                            picNameList.Add(item.pic_name);
                        }
                    }

                    Log.Info("pic count 2:" + picNameList.Count);

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
                            Log.Info(string.Format("进度:{0}/{1}", i, count));
                            Picture pic = picList[i];
                            if (pic == null) continue;
                            if (pic.Name != "顶视图")
                            {
                                SaveFile(pic);
                                var r = db.Pictures.DeleteById(pic.Id);
                            }
                            else
                            {
                                upViewList.Add(pic);//定视图
                            }
                        }
                        catch (Exception ex1)
                        {
                            Log.Error(ex1.ToString());
                        }
                    }

                    if (upViewList.Count > 1)
                    {
                        for (int i = 1; i < upViewList.Count; i++)
                        {
                            var r = db.Pictures.DeleteById(upViewList[i].Id);//重复的顶视图图片
                        }
                    }

                    Log.Info("完成");
                }
                catch (Exception ex2)
                {
                    Log.Error(ex2.ToString());
                }

            }, () =>
            {
                if (callBack != null)
                {
                    callBack();
                }
            });
        }

        private static void SaveToFile(string picName,string base64)
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

        private static void SaveFile(Picture pic)
        {
            ////Log.Info(string.Format("进度:{0}/{1},[{2}]", i1, list.Count, r!=null));
            //string filePath = dirPath + pic.Name;
            string base64 = Encoding.UTF8.GetString(pic.Info);
            //base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            //byte[] bytes = Convert.FromBase64String(base64);
            //System.IO.File.WriteAllBytes(filePath, bytes);

            SaveToFile(pic.Name, base64);
        }

        private void SavePicture(CameraAlarmJson camera, Bll bll)
        {
            byte[] byte1 = camera.Json;
            string json = Encoding.UTF8.GetString(byte1);
            CameraAlarmInfo info = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
            info.id = camera.Id; //增加了id,这样能够获取到详情

            string pic = info.pic_data;
            if (!string.IsNullOrEmpty(pic))
            {
                info.pic_data = ""; //图片分开存
                string json2 = JsonConvert.SerializeObject(info); //新的没有图片的json
                camera.Json = Encoding.UTF8.GetBytes(json2);
                bll.CameraAlarmJsons.Edit(camera);
                var picName = info.pic_name;
                var picture = bll.Pictures.Find(i => i.Name == picName);
                if (picture == null)
                {
                    picture = new Picture();
                    picture.Name = info.pic_name;
                    picture.Info = Encoding.UTF8.GetBytes(pic);
                    bll.Pictures.Add(picture); //保存图片
                }
                else
                {
                    picture.Name = info.pic_name;
                    picture.Info = Encoding.UTF8.GetBytes(pic);
                    bll.Pictures.Edit(picture); //保存图片
                }
            }
            else
            {
                Log.Info("没有图片");

                DateTime now = GetDataTime(info.time_stamp);

                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\CameraAlarms\\" + now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".json";
                FileInfo fi = new FileInfo(path);
                if (!fi.Directory.Exists)
                    fi.Directory.Create();

                File.WriteAllText(path, json);//yyyy_mm_dd_HH_MM_ss_fff=>yyyy_MM_dd_HH_mm_ss_fff
            }
        }
    }
}
