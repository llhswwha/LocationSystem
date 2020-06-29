using ArchorUDPTool;
using ArchorUDPTool.Commands;
using ArchorUDPTool.Models;
using Base.Common.Threads;
using BLL;
using DbModel.Location.Alarm;
using DbModel.Location.AreaAndDev;
using DbModel.LocationHistory.Alarm;
using DbModel.Tools;
using EngineClient;
using Location.BLL.Tool;
using Location.TModel.Tools;
using LocationServer.Tools;
using LocationServices.Converters;
using LocationServices.Locations;
using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocationServer.Threads
{
    public class AnchorScanThread : IntervalTimerThread
    {
        public AnchorScanThread(int seconds)
            :base(
                 TimeSpan.FromSeconds(seconds)
                 , TimeSpan.FromSeconds(1))
        {
            this.Name = LogTags.AnchorScan;
        }


        protected override void DoBeforeWhile()
        {
            TickFunction();
        }

        ArchorManager archorManager;

        private List<ArchorInfo> _archors = null;

        public List<ArchorInfo> DbArchorList
        {
            get
            {
                return _archors;
            }
            set
            {
                
                _archors = value;
                if (_archors != null)
                {
                    DbArchorListDict = _archors.ToDictionary(i => i.Ip);
                }
            }
        }


        public Dictionary<string, ArchorInfo> DbArchorListDict { get; internal set; }

        UDPArchorList UDPArchorList;
        //Bll bll = Bll.NewBllNoRelation();

        public override void Abort()
        {
            base.Abort();
            //bll.Dispose();
        }

        //UDPArchorList progressList = new UDPArchorList();
        //Dictionary<string,UDPArchor> progressList = new Dictionary<string, UDPArchor>();
        int count = 0;

        Dictionary<int, DevInfo> devDict;

        private List<string> localips;

        private bool Init()
        {
            var ip = EngineClientSetting.LocalIp;
            if (localips==null)
            {
                localips = IpHelper.GetLocalIpList();
            }

            if (localips.Contains(EngineClientSetting.LocalIp)==false)
            {
                return false;
            }

            Log.Info(Name, "Init:" + scanCount);
            InitArchorManager();

            var anchors=LoadAnchors();
            if (anchors.Count < 495)
            {
                DbConfigureHelper.LoadArchorList(Name);
                LoadAnchors();
            }
            LoadList();
            //Log.Info(Name, "Init End");
            return true;
        }

        //public List<UDPArchor> progressList = new List<UDPArchor>();

            public int maxCount=0;

        private void InitArchorManager()
        {
            if (archorManager == null)
            {
                //DispatcherTimer timer = new DispatcherTimer();
                //timer.Interval = TimeSpan.FromMilliseconds(100);
                //timer.Tick += UpdateGridTimer_Tick;
                //timer.Start();
                //updateGridTimer = timer;

                archorManager = new ArchorManager();
                archorManager.LocalIp = IPAddress.Parse(EngineClientSetting.LocalIp);
                archorManager.ArchorListChanged += (list, item) =>
                {
                    //if(UDPArchorList== null)
                    //{
                    //    UDPArchorList = list;
                    //}
                    //else
                    //{

                    //}

                    //if (UDPArchorList != list)
                    //{
                    //    UDPArchorList = list;
                    //}

                    if (item != null)
                    {
                        var id = list.IndexOf(item);
                        SetArchorList(archorManager, list, item, id);
                    }
                    else
                    {
                        SetArchorList(archorManager, list);
                    }

                    if (item != null)
                    {
                        var maxList = archorManager.GetMaxArchorList();
                        if (maxList.Count > maxCount)
                        {
                            maxCount = maxList.Count;
                            Log.Info(Name, string.Format("maxCount:{0}", maxCount));
                        }
                        count++;

                        //Log.Info(Name, "ArchorListChanged:" + count);
                    }
                    //IsDirty = true;
                };
                //archorManager.LogChanged += ArchorManager_LogChanged;
                archorManager.PercentChanged += (p) =>
                {
                    //ProgressBarEx1.Value = p;
                    //Log.Info(Name, "PercentChanged:" + p);
                    if (p == 100)
                    {
                        int sum = DbArchorList.Count;
                        //Log.Info(Name, string.Format("Completed1:{0}-{1}={2}" ,sum, count,sum-count));

                        IsBusySendCmd = false;


                        if (IsBusySendAlarm) return;
                        IsBusySendAlarm = true;
                        ThreadPool.QueueUserWorkItem(a =>
                        {
                            scanCount++;

                            Thread.Sleep(3000);
                            Save();
                            var listOff = GetOfflineList();
                            var count2 = listOff.Count;
                            Log.Info(Name, string.Format("Completed2:{0}-{1}={2}", sum, sum - count2, count2));

                            SendAlarm();

                            //if (scanCount % 5 == 0)
                            //{
                            //    archorManager.ClearMaxArchorList();//重新开始计算
                            //}

                            IsBusySendAlarm = false;
                        });
                    }
                };
                //DataGrid3.archorManager = archorManager;
                archorManager.arg = GetScanArg();
            }
        }

        int scanCount = 0;

        

        private List<ArchorInfo> LoadAnchors()
        {
            using (Bll bll = Bll.NewBllNoRelation())
            {
                devDict = bll.DevInfos.ToDictionary();

                var anchors = bll.Archors.GetInfoList();
               
                var areas = bll.Areas.ToDictionary();
                if (anchors != null && areas != null)
                {
                    var anchors2 = anchors.FindAll(i => i.Ip != null);
                    foreach (var item in anchors2)
                    {
                        if (item.ParentId != null)
                        {
                            if (areas.ContainsKey((int)item.ParentId))
                            {
                                item.Parent = areas[(int)item.ParentId];
                            }
                            else
                            {

                            }
                        }
                    }
                    DbArchorList = anchors2;

                    return anchors2;
                }
                else
                {
                    return anchors;
                }
            }
        }

        private int Save()
        {
            //SetArchorList(archorManager, UDPArchorList);
            SaveResult();//保存结果
            var list1=archorManager.GetMaxArchorList();
            var list2 = list1.FindAll(i => i.Id == null || i.Id == "");
            return list2.Count;
        }

        public UDPArchorList GetOfflineList()
        {
            UDPArchorList list0 = new UDPArchorList();

            var allList = archorManager.GetResultArchorList();
            //var existList = archorManager.GetMaxArchorList();//扫描到的当前的全部数量
            var existList = allList.FindAll(i => i.Id != null && i.Id != "");
            list0.AddRange(allList);
            //foreach (var item in existList)
            int k = 0;
            for(int j=0;j<existList.Count;j++)
            {
                var item = existList[j];
                var existItem = allList.Find(i => i.Client == item.Client);
                if (existItem != null)
                {
                    //k++;
                    //if (scanCount % 2 ==0 && k < 5)
                    //{

                    //}
                    //else
                    {
                        list0.Remove(existItem);
                    }
                    
                }
            }
            return list0;
        }

        private void SendAlarm()
        {
            try
            {
                //var count1 = bll.Archors.GetCount();

                UDPArchorList list0 = GetOfflineList();

                List<DevAlarm> alarms = GenerateNewAlarm(list0);
                
                using (Bll bll = Bll.NewBllNoRelation())
                {
                    if (currentAlarms == null)
                    {
                        var realAlarms = bll.DevAlarms.FindAll(i => i.Src == Abutment_DevAlarmSrc.人员定位);//拿到数据库中已经有的告警数据
                        currentAlarms = new DbUpdateSet<DevAlarm>(realAlarms);
                    }

                    var set = currentAlarms;

                    set.Update(alarms);//更新告警

                    bll.DevAlarms.AddRange(set.AddList);

                    bll.DevAlarms.EditRange(set.EditList);

                    List<DevAlarmHistory> newHist = new List<DevAlarmHistory>();
                    foreach (var item in set.DeleteList)
                    {
                        var hisItem = item.RemoveToHistory();
                        newHist.Add(hisItem);

                        var clone = item.Clone();
                        clone.Msg = clone.Msg.Replace("基站离线", "基站正常");
                        clone.Level = Abutment_DevAlarmLevel.无;
                        set.SendList.Add(clone);

                        set.CurrentList.Remove(item);
                    }
                    bll.DevAlarmHistorys.AddRange(newHist);
                    bll.DevAlarms.RemoveList(set.DeleteList);

                    if(set.IsChanged)
                        LocationService.RefreshDeviceAlarmBuffer(Name);

                    if (AppContext.AnchorScanSendMode == 1)
                    {
                        set.SendList.AddRange(set.CurrentList);//每次都发送当前的告警和恢复的告警
                    }

                    var sendList = set.SendList;

                    foreach (var item in sendList)
                    {
                        item.DictKey = item.Msg;
                        if (devDict.ContainsKey(item.DevInfoId))
                        {
                            var dev = devDict[item.DevInfoId];
                            item.DevInfo = dev;
                        }
                        else
                        {
                            Log.Error(Name, string.Format("未查到设备 devId:{0},code:{1}", item.DevInfoId, item.Code));
                        }
                    }

                    var alarmsM = sendList.ToTModel().ToArray();

                    //var count2 = bll.Archors.GetCount();
                    //if (count2 != count1)
                    //{
                    //    Log.Error(Name, count1 + "-" + count2);
                    //}

                    Log.Info(Name, string.Format("发送告警 当前离线数量:{0},新离线数量:{1},恢复数量:{2},发送数量:{3}", alarms.Count, set.AddList.Count, set.DeleteList.Count, alarmsM.Length));
                    AlarmHub.SendDeviceAlarms(alarmsM);
                }
            }
            catch (Exception ex)
            {

                Log.Error(Name, "Exception!!:" + ex);
            }
        }

        public DbUpdateSet<DevAlarm> currentAlarms;

        private List<DevAlarm> GenerateNewAlarm(UDPArchorList list1)
        {
            List<DevAlarm> alarms = new List<DevAlarm>();
            foreach (var item in list1)
            {
                DevAlarm alarm = CreateDevAlarm(item);
                alarms.Add(alarm);
            }

            return alarms;
        }

        private DevAlarm CreateDevAlarm(UDPArchor item)
        {
            DevAlarm alarm = new DevAlarm();
            alarm.Level = Abutment_DevAlarmLevel.低;
            alarm.Code = "基站离线";
            alarm.Title = "基站离线";
            alarm.Src = Abutment_DevAlarmSrc.人员定位;//其实应该加上“基站扫描”的
            alarm.AlarmTime = DateTime.Now;
            alarm.AlarmTimeStamp = TimeConvert.ToStamp(alarm.AlarmTime);
            var clientIP = item.GetClientIP();
            alarm.Msg = "基站离线:" + clientIP;

            var anchor = DbArchorList.Find(i => i.Ip == clientIP);
            if (anchor != null)
            {
                var devId = (int)anchor.DevId;
                var dev= devDict[devId];
                alarm.DevInfo = dev;
                alarm.DevInfoId = devId;
                alarm.Device_desc = dev.Name;
                alarm.Title = string.Format("基站离线[{0},{1}]",anchor.Code.Trim(),anchor.Ip.Trim());
                alarm.Msg = string.Format("基站离线[{0},{1}]", anchor.Code, anchor.Ip);
                //alarm.AreaId
            }

            alarm.DictKey = alarm.Msg;

            return alarm;
        }

   

        private void LoadList()
        {
            //3.加载清单代码
            var list = ArchorHelper.LoadArchoDevInfo();

            ArchorDevList list2 = new ArchorDevList();
            list2.ArchorList = new List<ArchorDev>();
            foreach (var item in list.ArchorList)
            {
                var a = DbArchorList.Find(i => i.Code == item.ArchorID);
                if (a != null)
                {
                    list2.ArchorList.Add(item);
                    item.Archor = a;
                }
            }
            archorManager.LoadList(list2);
        }

        private void Mh_DevAlarmReceived(DbModel.Location.Alarm.DevAlarm obj)
        {
            AlarmHub.SendDeviceAlarms(obj.ToTModel());
        }

        private void SaveResult()
        {
            string path1 = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
            FileInfo fi1 = new FileInfo(path1);
            archorManager.SaveArchorList(path1);
        }

        private void SetArchorList(ArchorManager archorManager, UDPArchorList list, UDPArchor item1 = null, int id = -1)
        {
            if (DbArchorListDict != null)
            {
                //list.ClearInfo();
                foreach (var item in list)
                {
                    item.DbInfo = "";
                    item.RealArea = "";
                    var clientIP = item.GetClientIP();
                    //var ar = DbArchorList.Find(i => i.Ip == clientIP);
                    if (DbArchorListDict.ContainsKey(clientIP))
                    {
                        var ar = DbArchorListDict[clientIP];
                        if (ar != null)
                        {
                            item.RealArea = ar.Parent.Name;
                            if (item.GetClientIP() != ar.Ip)
                            {
                                item.DbInfo = "IP:" + ar.Ip;
                            }
                            else
                            {
                                string code = ar.Code.Trim();
                                if (!string.IsNullOrEmpty(code))
                                {
                                    item.DbInfo = "有:" + code;
                                }
                                else
                                {
                                    item.DbInfo = "有:" + ar.Ip;
                                }
                            }
                        }
                    }

                }

                if (id > 0)
                {
                    var item2 = list[id];
                    if (item2 != item1)
                    {

                    }

                    int id2 = list.IndexOf(item1);
                }

                if(UDPArchorList== null)
                {
                    UDPArchorList = list;
                }
                else
                {
                    //if(UDPArchorList!= list)
                    //{

                    //}
                }
                

                if (id > 0)
                {
                    var item2 = list[id];
                    if (item2 != item1)
                    {

                    }
                    int id2 = list.IndexOf(item1);
                }
            }
        }

        public bool IsBusySendAlarm = false;

        public bool IsBusySendCmd = false;

        public override bool TickFunction()
        {
            if (IsBusySendAlarm || IsBusySendCmd) return true;

            if (Init() == false) return true;//初始化失败,一般是设置的ip本地没有
            if (AppContext.AnchorScanResetCount > 0)
            {
                if (scanCount % AppContext.AnchorScanResetCount == 0) //10次后重新扫描，避免突然大面积离线的问题无法发现。
                {
                    archorManager.ClearMaxArchorList();//重新开始计算
                    maxCount = 0;
                    Log.Info(Name, "重新开始计算:" + scanCount);
                }
            }

            IsBusySendCmd = true;

            //progressList = new UDPArchorList();
            count = 0;

            archorManager.ScanArchors(GetScanArg(UDPCommands.GetId), archorManager.archorList);
            return true;
        }

        private ArchorManager.ScanArg GetScanArg(params string[] cmds)
        {
            ArchorManager.ScanArg arg = new ArchorManager.ScanArg();
            arg.ipsText = "";
            arg.port = "4646";
            arg.cmds = cmds;
            arg.OneIPS = false;
            arg.ScanList = true;
            ////arg.Ping = (bool)CbPing.IsChecked;
            return arg;
        }

    }
}
