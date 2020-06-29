using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using Location.BLL.Tool;
using LocationServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiLib.Clients
{
    /// <summary>
    /// 移动巡检信息获取客户端
    /// </summary>
    public class InspectionTrackClient
    {
        public string webApiUrl = "";

        private static int nEightHourSecond = 28800;//8小时

        public event Action<DbModel.Location.Work.InspectionTrackList> ListGot;

        private string ParkName = "";
        private static  string suffix = "api";

        public InspectionTrackClient(string url,string port)
        {
            webApiUrl = url;
            //  client = new BaseDataClient(webApiUrl, port, "api");
            ParkName = AppContext.ParkName;
            if (ParkName == "中山嘉明电厂")
            {
                suffix = "zhongshan";//中山测试
            }
            client = new BaseDataClient(webApiUrl, port, suffix);   
        }

        private Thread GetInspectionTrackThread;

        public void Stop()
        {
            if (GetInspectionTrackThread != null)
            {
                GetInspectionTrackThread.Abort();
                GetInspectionTrackThread = null;
            }
        }

        public void Start()
        {
            if (GetInspectionTrackThread == null)
            {
                GetInspectionTrackThread = new Thread(InnerOperate);
                GetInspectionTrackThread.IsBackground = true;
                GetInspectionTrackThread.Start();
            }
        }

        public BaseDataClient client = null;

        public int days = 3;

        private void InnerOperate()
        {
            //string strIp = ConfigurationManager.AppSettings["DatacaseWebApiUrl"];
            //string datacaseUrl = string.Format("http://{0}:{1}/", strIp);

            bool bFirst = true;
            int nDay = -1;

            Log.Info(LogTags.Server, "InnerOperate："+client.client.BaseUri);

            while (true)
            {

                DateTime dtEnd = DateTime.Now;
                DateTime dtBegin = dtEnd.Date.AddDays(nDay);
                if (bFirst)
                {
                    dtBegin = dtEnd.Date.AddDays(-days); //获取一天前或者3天前到现在的巡检轨迹
                    bFirst = false;
                }
                var TrackList = DealInspectionTrack(client, dtBegin, dtEnd, true);
               
                if (TrackList == null)//获取巡检轨迹
                {
                    Log.Info(LogTags.Server,"获取巡检轨迹失败！！ break!!");
                    break;
                }

                //Bll bll = Bll.Instance();
                //List<DbModel.Location.Work.InspectionTrack> trackList = bll.InspectionTracks.ToList();//从数据库取
                ////List<InspectionTrackHistory> send2 = bll.InspectionTrackHistorys.ToList();
                //if (trackList == null || trackList.Count() == 0)
                //{
                //    Thread.Sleep(5 * 60 * 1000);//等待5分钟
                //    continue;
                //}

                if (TrackList.AddTrack.Count == 0 && TrackList.ReviseTrack.Count == 0 && TrackList.DeleteTrack.Count == 0)
                {
                    Thread.Sleep(5 * 60 * 1000);//等待5分钟
                    continue;
                }

                if (ListGot != null)
                {
                    ListGot(TrackList);
                }
                //SignalRService.Hubs.InspectionTrackHub.SendInspectionTracks(trackList.ToWcfModelList().ToArray());
                //DateTime dt2 = DateTime.Now;
                Thread.Sleep(5 * 60 * 1000);//等待5分钟
            }
        }

        public List<patrols> GetPatrolList(DateTime dtBegin, DateTime dtEnd)
        {
            return client.GetPatrolList(dtBegin, dtEnd);
        }

        public List<patrols> GetPatrolList()
        {
            return client.GetPatrolList();
        }

        public patrols GetPatrolDetail(int patrolId)
        {
            return client.GetPatrolDetail(patrolId);
        }

        public checkresults Getcheckresults(int patrolId, string deviceId)
        {
            return client.Getcheckresults(patrolId, deviceId);
        }


        private DbModel.Location.Work.InspectionTrackList DealInspectionTrack(WebApiLib.Clients.BaseDataClient client, DateTime dtBegin, DateTime dtEnd, bool bFlag)
        {
            DbModel.Location.Work.InspectionTrackList TrackList = new DbModel.Location.Work.InspectionTrackList();
            try
            {
                Log.Info(LogTags.Inspection, string.Format("DealInspectionTrack Start {0}-{1}", dtBegin, dtEnd));
                //http://172.16.100.22/api/patrols?startDate=1563066082&endDate=1563325282&offset=0&limit=10000
                var All = new List<DbModel.Location.Work.InspectionTrack>();
                var newList = new List<DbModel.Location.Work.InspectionTrack>();
                var changedList = new List<DbModel.Location.Work.InspectionTrack>();
                var deleteList = new List<DbModel.Location.Work.InspectionTrack>();
                var newHisList = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>();

                var changeHisList = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>();
                long lNow = GetNowDateStamp();

                var recv = client.GetPatrolList(dtBegin, dtEnd);//从WebApi获取
                                                                // var recv = Getinspectionlist();
                if (recv == null)
                {
                    return null;
                }
                Log.Info(LogTags.Inspection, string.Format("GetPatrolList:" + recv.Count));

                Bll bll = Bll.NewBllNoRelation();
                var itList = bll.InspectionTracks.ToList();//当前的巡检轨迹
                Log.Info(LogTags.Inspection, string.Format("InspectionTracks:" + itList.Count));

                var itHList = bll.InspectionTrackHistorys.ToList();//历史巡检轨迹
                Log.Info(LogTags.Inspection, string.Format("InspectionTrackHistorys:" + itList.Count));

                foreach (patrols item in recv)
                {
                    var now = itList.Find(p => p.Abutment_Id == item.id);//数据库中已经存在该轨迹
                    var history = itHList.Find(p => p.Abutment_Id == item.id);

                    if (item.endTime >= lNow
                        && !(item.state == "已完成" || item.state == "已过期")
                        )
                    {
                        if (now == null)
                        {
                            now = CreateInspectionTrack(item);
                            newList.Add(now);
                        }
                        else
                        {
                            if (now.State != item.state)
                            {
                                now.State = item.state;
                                changedList.Add(now);
                            }

                        }
                    }
                    else
                    {
                        if (now != null)
                        {
                            now.State = item.state;
                            deleteList.Add(now);
                        }

                        if (history == null)
                        {
                            history = CreateInspectionTrackHistory(item);
                            newHisList.Add(history);
                        }
                        else
                        {
                            if (history.State != item.state)
                            {
                                history.State = item.state;
                                changeHisList.Add(history);
                            }
                        }
                    }
                }

                Log.Info(LogTags.Inspection, string.Format("DealInspectionTrack Edit... newList:{0},changedList:{1},deleteList:{2},changeHisList:{3},newHisList:{4}"
                    , newList.Count, changedList.Count, deleteList.Count, changeHisList.Count, newHisList.Count));

                bll.InspectionTracks.AddRange(newList);//添加
                bll.InspectionTracks.EditRange(changedList);//修改
                bll.InspectionTracks.RemoveList(deleteList);//删除

                bll.InspectionTrackHistorys.EditRange(changeHisList);//历史轨迹
                bll.InspectionTrackHistorys.AddRange(newHisList);//历史轨迹

                All.AddRange(newList);
                All.AddRange(changedList);
                bll.Dispose();



                DealPatrolPoint(All, deleteList, newHisList, client);

                
                TrackList.AddTrack = newList;
                TrackList.ReviseTrack = changedList;
                TrackList.DeleteTrack = deleteList;

                Log.Info(LogTags.Inspection, string.Format("DealInspectionTrack End"));
                return TrackList;
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.Inspection, string.Format("DealInspectionTrack :"+ ex));
                return TrackList;
            }
            
        }

        public static DbModel.LocationHistory.Work.InspectionTrackHistory CreateInspectionTrackHistory(patrols item)
        {
            DbModel.LocationHistory.Work.InspectionTrackHistory history = new DbModel.LocationHistory.Work.InspectionTrackHistory();
            history.Abutment_Id = item.id;
            history.Code = item.code;
            history.Name = item.name;
            history.CreateTime = (item.createTimes + nEightHourSecond) * 1000;
            history.dtCreateTime = Location.TModel.Tools.TimeConvert.ToDateTime(history.CreateTime);
            history.State = item.state;
            history.StartTime = (item.startTime + nEightHourSecond) * 1000;
            history.dtStartTime = Location.TModel.Tools.TimeConvert.ToDateTime(history.StartTime);
            history.EndTime = (item.endTime + nEightHourSecond) * 1000;

            history.dtEndTime = Location.TModel.Tools.TimeConvert.ToDateTime(history.EndTime);
            return history;
        }

        public static DbModel.Location.Work.InspectionTrack CreateInspectionTrack(patrols item)
        {
            DbModel.Location.Work.InspectionTrack now = new DbModel.Location.Work.InspectionTrack();
            now.Abutment_Id = item.id;
            now.Code = item.code;
            now.Name = item.name;
            now.CreateTime = (item.createTimes + nEightHourSecond) * 1000;
            now.dtCreateTime = Location.TModel.Tools.TimeConvert.ToDateTime(now.CreateTime);
            now.State = item.state;
            now.StartTime = (item.startTime + nEightHourSecond) * 1000;
            now.dtStartTime = Location.TModel.Tools.TimeConvert.ToDateTime(now.StartTime);
            now.EndTime = (item.endTime + nEightHourSecond) * 1000;
            now.dtEndTime = Location.TModel.Tools.TimeConvert.ToDateTime(now.EndTime);
            return now;
        }

        public static long GetNowDateStamp()
        {
            DateTime dtNow = DateTime.Now.Date;
            long lNow = Location.TModel.Tools.TimeConvert.ToStamp(dtNow) / 1000;
            lNow = lNow - nEightHourSecond;
            return lNow;
        }

        private void DealPatrolPoint(List<DbModel.Location.Work.InspectionTrack> All, List<DbModel.Location.Work.InspectionTrack> Delete, List<DbModel.LocationHistory.Work.InspectionTrackHistory> HAdd, WebApiLib.Clients.BaseDataClient client)
        {
            Log.Info(LogTags.Inspection, string.Format("DealPatrolPoint Start"));

            Bll bll = Bll.NewBllNoRelation();
            List<DbModel.Location.Work.PatrolPoint> ppList = bll.PatrolPoints.ToList();
            List<DbModel.LocationHistory.Work.PatrolPointHistory> ppHList = bll.PatrolPointHistorys.ToList();
            //List<DbModel.Location.AreaAndDev.DevInfo> devList = bll.DevInfos.ToList();
            if (ppList == null)
            {
                ppList = new List<DbModel.Location.Work.PatrolPoint>();
            }

            if (ppHList == null)
            {
                ppHList = new List<DbModel.LocationHistory.Work.PatrolPointHistory>();
            }

            //if (devList == null)
            //{
            //    devList = new List<DbModel.Location.AreaAndDev.DevInfo>();
            //}

            List<DbModel.Location.Work.PatrolPoint> PAll = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PAdd = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PEdit = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PDelete = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.LocationHistory.Work.PatrolPointHistory> PHAdd = new List<DbModel.LocationHistory.Work.PatrolPointHistory>();

            //foreach (DbModel.Location.Work.InspectionTrack item in All)
            for(int i=0;i<All.Count;i++)
            {
                if (All.Count > 100)
                {
                    if (i % 10 == 0)
                    {
                        Log.Info(LogTags.Inspection, string.Format("DealPatrolPoint Progress1(All) :{0}/{1}", i + 1, All.Count));
                    }
                }
                else
                {
                    Log.Info(LogTags.Inspection, string.Format("DealPatrolPoint Progress1(All) :{0}/{1}", i + 1, All.Count));
                }                   

                var item = All[i];
                int Id = item.Id;
                int patrolId = (int)item.Abutment_Id;
                CommunicationClass.SihuiThermalPowerPlant.Models.patrols recv = client.GetPatrolDetail(patrolId);
               // CommunicationClass.SihuiThermalPowerPlant.Models.patrols recv = Getcheckpoints(patrolId);

                if (recv == null || recv.route.Count() <= 0)
                {
                    continue;
                }

                foreach (checkpoints item2 in recv.route)
                {
                    //DbModel.Location.AreaAndDev.DevInfo dev1 = devList.Find(p => p.KKS == item2.kksCode);
                    DbModel.Location.Work.PatrolPoint now = ppList.Find(p => p.DeviceId == item2.deviceId && p.ParentId == Id);
                    if (now == null)
                    {
                        now = new DbModel.Location.Work.PatrolPoint();
                        SetPatrolPointProperty(Id, item2, now);
                        PAdd.Add(now);
                    }
                    else
                    {
                        SetPatrolPointProperty(Id, item2, now);
                        PEdit.Add(now);
                    }
                }
            }

            foreach (DbModel.Location.Work.InspectionTrack item in Delete)
            {
                int Id = item.Id;
                List<DbModel.Location.Work.PatrolPoint> lstDelete = ppList.FindAll(p => p.ParentId == Id).ToList();
                if (lstDelete != null && lstDelete.Count() > 0)
                {
                    PDelete.AddRange(lstDelete);
                }
            }

            //foreach (DbModel.LocationHistory.Work.InspectionTrackHistory item in HAdd)
            for (int i = 0; i < HAdd.Count; i++)
            {
                if (All.Count > 100)
                {
                    if (i % 20 == 0)
                    {
                        Log.Info(LogTags.Inspection, string.Format("DealPatrolPoint Progress2(HAdd) :{0}/{1}", i + 1, HAdd.Count));
                    }
                }
                else
                {
                    Log.Info(LogTags.Inspection, string.Format("DealPatrolPoint Progress2(HAdd) :{0}/{1}", i + 1, HAdd.Count));
                }

                DbModel.LocationHistory.Work.InspectionTrackHistory item = HAdd[i];
                int Id = item.Id;
                int patrolId = (int)item.Abutment_Id;
                CommunicationClass.SihuiThermalPowerPlant.Models.patrols recv = client.GetPatrolDetail(patrolId);
                //CommunicationClass.SihuiThermalPowerPlant.Models.patrols recv = Getcheckpoints(patrolId);
                if (recv == null || recv.route.Count() <= 0)
                {
                    continue;
                }

                foreach (CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints item2 in recv.route)
                {
                    //DbModel.Location.AreaAndDev.DevInfo dev1 = devList.Find(p => p.KKS == item2.kksCode);
                    DbModel.LocationHistory.Work.PatrolPointHistory history = ppHList.Find(p => p.DeviceId == item2.deviceId && p.ParentId == Id);
                    if (history == null)
                    {
                        history = CreatePatrolPointHistory(Id, item2);
                        PHAdd.Add(history);
                    }
                }
            }


            Log.Info(LogTags.Inspection, string.Format("DealPatrolPoint Edit...."));

            bll.PatrolPoints.AddRange(PAdd);
            bll.PatrolPoints.EditRange(PEdit);
            bll.PatrolPoints.RemoveList(PDelete);
            bll.PatrolPointHistorys.AddRange(PHAdd);

            PAll.AddRange(PAdd);
            PAll.AddRange(PEdit);
            bll.Dispose();

            Log.Info(LogTags.Inspection, string.Format("DealPatrolPoint End...."));
            DealPatrolPointItem( All, HAdd, PAll, PDelete, PHAdd, client);

            return;
        }

        private static DbModel.LocationHistory.Work.PatrolPointHistory CreatePatrolPointHistory(int Id, checkpoints item2)
        {
            DbModel.LocationHistory.Work.PatrolPointHistory history = new DbModel.LocationHistory.Work.PatrolPointHistory();
            history.ParentId = Id;
            history.StaffCode = item2.staffCode;
            history.StaffName = item2.staffName;
            history.KksCode = item2.kksCode;
            history.DevName = item2.deviceName;
            history.DeviceCode = item2.deviceCode;
            history.DeviceId = item2.deviceId;
            //if (dev1 != null)
            //{
            //    history.DevId = dev1.Id;
            //}
            return history;
        }

        private static void SetPatrolPointProperty(int Id, checkpoints item2, DbModel.Location.Work.PatrolPoint now)
        {
            now.ParentId = Id;
            now.StaffCode = item2.staffCode;
            now.StaffName = item2.staffName;
            now.KksCode = item2.kksCode;
            now.DevName = item2.deviceName;
            now.DeviceCode = item2.deviceCode;
            now.DeviceId = item2.deviceId;
            //if (dev1 != null)
            //{
            //    now.DevId = dev1.Id;
            //}
        }

        public bool isAllNew = false;

        private void DealPatrolPointItem( List<DbModel.Location.Work.InspectionTrack> All, List<DbModel.LocationHistory.Work.InspectionTrackHistory> HAdd, List<DbModel.Location.Work.PatrolPoint> PAll, List<DbModel.Location.Work.PatrolPoint> PDelete, List<DbModel.LocationHistory.Work.PatrolPointHistory> PHAdd, WebApiLib.Clients.BaseDataClient client)
        {
            try
            {
                Bll bll = Bll.NewBllNoRelation();
                List<DbModel.Location.Work.PatrolPointItem> ppiList = bll.PatrolPointItems.ToList();
                List<DbModel.LocationHistory.Work.PatrolPointItemHistory> ppiHList = bll.PatrolPointItemHistorys.ToList();
                if (ppiList == null)
                {
                    ppiList = new List<DbModel.Location.Work.PatrolPointItem>();
                }

                if (ppiHList == null)
                {
                    ppiHList = new List<DbModel.LocationHistory.Work.PatrolPointItemHistory>();
                }

                List<DbModel.Location.Work.PatrolPointItem> PIAll = new List<DbModel.Location.Work.PatrolPointItem>();
                List<DbModel.Location.Work.PatrolPointItem> PIAdd = new List<DbModel.Location.Work.PatrolPointItem>();
                List<DbModel.Location.Work.PatrolPointItem> PIEdit = new List<DbModel.Location.Work.PatrolPointItem>();
                List<DbModel.Location.Work.PatrolPointItem> PIDelete = new List<DbModel.Location.Work.PatrolPointItem>();
                List<DbModel.LocationHistory.Work.PatrolPointItemHistory> PIHAdd = new List<DbModel.LocationHistory.Work.PatrolPointItemHistory>();

                for(int i=0;i<PAll.Count;i++)
                //foreach (DbModel.Location.Work.PatrolPoint item in PAll)
                {
                    var item = PAll[i];

                    if (All.Count > 1000)
                    {
                        if (i % 20 == 0)
                        {
                            Log.Info(LogTags.Inspection, string.Format("DealPatrolPointItem Progress1(PAll) :{0}/{1}", i + 1, PAll.Count));
                        }
                    }
                    else
                    {
                        Log.Info(LogTags.Inspection, string.Format("DealPatrolPointItem Progress1(PAll) :{0}/{1}", i + 1, PAll.Count));
                    }


                    try
                    {
                        int Id = item.Id;
                        string deviceId = item.DeviceId;
                        int ParentId = item.ParentId;
                        DbModel.Location.Work.InspectionTrack it = All.Find(p => p.Id == ParentId);
                        if (it == null)
                        {
                            continue;
                        }

                        int patrolId = (int)it.Abutment_Id;
                        var recv = client.Getcheckresults(patrolId, deviceId);
                        //  CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints recv = Getcheckresults(patrolId, deviceId);
                        if (recv == null || recv.checks.Count() <= 0)
                        {
                            continue;
                        }

                        foreach (results item2 in recv.checks)
                        {
                            try
                            {
                                if (isAllNew)
                                {
                                    var now = CreatePatrolPointItem(Id, item2);
                                    PIAdd.Add(now);
                                }
                                else
                                {
                                    var now = ppiList.Find(p => p.CheckId == item2.checkId && p.ParentId == Id);

                                    if (now == null)
                                    {
                                        now = CreatePatrolPointItem(Id, item2);
                                        PIAdd.Add(now);
                                    }
                                    else
                                    {
                                        SetCheckTime(item2, now);

                                        now.CheckResult = item2.checkResult;
                                        PIEdit.Add(now);
                                    }
                                }
                                
                            }
                            catch (Exception ex1)
                            {

                                Log.Error(LogTags.Inspection, ex1);
                            }
                           
                        }

                        if (PIAdd.Count > 1000)
                        {
                            bll.PatrolPointItems.AddRange(PIAdd);
                            PIAdd.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(LogTags.Inspection, ex);
                    }
                    
                }

                foreach (var item in PDelete)
                {
                    int Id = item.Id;
                    List<DbModel.Location.Work.PatrolPointItem> lstDelete = ppiList.FindAll(p => p.ParentId == Id).ToList();
                    if (lstDelete != null && lstDelete.Count() > 0)
                    {
                        PIDelete.AddRange(lstDelete);
                    }
                }
                for(int i=0;i<PHAdd.Count;i++)
                //foreach (var item in PHAdd)
                {
                    if (All.Count > 10000)
                    {
                        if (i % 100 == 0)
                        {
                            Log.Info(LogTags.Inspection, string.Format("DealPatrolPointItem Progress2(PHAdd) :{0}/{1}", i + 1, PHAdd.Count));
                        }
                    }
                    else
                    {
                        Log.Info(LogTags.Inspection, string.Format("DealPatrolPointItem Progress2(PHAdd) :{0}/{1}", i + 1, PHAdd.Count));
                    }

                    var item = PHAdd[i];
                    try
                    {
                        int Id = item.Id;
                        string deviceId = item.DeviceId;
                        int ParentId = item.ParentId;
                        DbModel.LocationHistory.Work.InspectionTrackHistory ith = HAdd.Find(p => p.Id == ParentId);
                        if (ith == null)
                        {
                            continue;
                        }
                        int patrolId = (int)ith.Abutment_Id;
                        var recv = client.Getcheckresults(patrolId, deviceId);
                        //CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints recv = Getcheckresults(patrolId, deviceId);

                        if (recv == null || recv.checks.Count() <= 0)
                        {
                            continue;
                        }

                        foreach (results item2 in recv.checks)
                        {
                            if (isAllNew)
                            {
                                var history = CreatePatrolPointItemHistory(Id, item2);
                                PIHAdd.Add(history);
                            }
                            else
                            {
                                var history = bll.PatrolPointItemHistorys.Find(p => p.CheckId == item2.checkId && p.ParentId == Id);
                                if (history == null)
                                {
                                    history = CreatePatrolPointItemHistory(Id, item2);
                                    PIHAdd.Add(history);
                                }
                            }
                        }
                        if (PIHAdd.Count > 1000)
                        {
                            bll.PatrolPointItemHistorys.AddRange(PIHAdd);
                            PIHAdd.Clear();
                        }
                        
                    }
                    catch (Exception ex)
                    {

                        Log.Error(LogTags.Inspection, ex);
                    }
                    
                }

                bll.PatrolPointItems.AddRange(PIAdd);
                bll.PatrolPointItems.EditRange(PIEdit);
                bll.PatrolPointItems.RemoveList(PIDelete);
                bll.PatrolPointItemHistorys.AddRange(PIHAdd);
                bll.Dispose();

                isAllNew = false;
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
                Log.Error(LogTags.Inspection, ex);
            }
            return;

        }


        private static DbModel.Location.Work.PatrolPointItem CreatePatrolPointItem(int Id, results item2)
        {
            DbModel.Location.Work.PatrolPointItem now = new DbModel.Location.Work.PatrolPointItem();
            now.ParentId = Id;
            now.KksCode = item2.kksCode;
            now.CheckItem = item2.checkItem;
            now.StaffCode = item2.staffCode;
            
            SetCheckTime(item2, now);
            now.CheckId = item2.checkId;
            now.CheckResult = item2.checkResult;
            return now;
        }

        private static void SetCheckTime(results item2, DbModel.Location.Work.PatrolPointItem now)
        {
            if (item2.checkTime != null)
            {
                now.CheckTime = (item2.checkTime/1000 + nEightHourSecond) * 1000;
                now.dtCheckTime = Location.TModel.Tools.TimeConvert.ToDateTime((long)now.CheckTime);
            }
            else
            {
                now.CheckTime = null;
                now.dtCheckTime = null;
            }
        }

        private static void SetCheckTime(results item2, DbModel.LocationHistory.Work.PatrolPointItemHistory history)
        {
            if (item2.checkTime != null)
            {
                history.CheckTime = (item2.checkTime/1000 + nEightHourSecond) * 1000;
                history.dtCheckTime = Location.TModel.Tools.TimeConvert.ToDateTime((long)history.CheckTime);
            }
            else
            {
                history.CheckTime = null;
                history.dtCheckTime = null;
            }
        }

        private static DbModel.LocationHistory.Work.PatrolPointItemHistory CreatePatrolPointItemHistory(int Id, results item2)
        {
            DbModel.LocationHistory.Work.PatrolPointItemHistory history = new DbModel.LocationHistory.Work.PatrolPointItemHistory();
            history.ParentId = Id;
            history.KksCode = item2.kksCode;
            history.CheckItem = item2.checkItem;
            history.StaffCode = item2.staffCode;
            SetCheckTime(item2, history);
            history.CheckId = item2.checkId;
            history.CheckResult = item2.checkResult;
            return history;
        }


        private List<CommunicationClass.SihuiThermalPowerPlant.Models.patrols> Getinspectionlist()
        {
            string strResult = "";

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string strPath = basePath + "Data\\InspectionJson\\InspectionList.txt";
            strResult = System.IO.File.ReadAllText(strPath, Encoding.Default);

            if (strResult.Contains("404 Not Found"))
            {
                throw new Exception("404 Not Found");
            }
        
            List<CommunicationClass.SihuiThermalPowerPlant.Models.patrols> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommunicationClass.SihuiThermalPowerPlant.Models.patrols>>(strResult);
            return obj;
        }

        private CommunicationClass.SihuiThermalPowerPlant.Models.patrols Getcheckpointss_File(int id)
        {
            string strResult = "";

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string strPath = basePath + "Data\\InspectionJson\\PatrolPoint" + Convert.ToString(id) + ".txt";
            strResult = System.IO.File.ReadAllText(strPath, Encoding.Default);

            if (strResult.Contains("404 Not Found"))
            {
                throw new Exception("404 Not Found");
            }

            CommunicationClass.SihuiThermalPowerPlant.Models.patrols obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommunicationClass.SihuiThermalPowerPlant.Models.patrols>(strResult);

           return obj;

        }

        private CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints Getcheckresults_File(int patrolId, string deviceId)
        {
            string strResult = "";

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string strPath = basePath + "Data\\InspectionJson\\GetItem" + Convert.ToString(patrolId) + "_" + deviceId + ".txt";
            strResult = System.IO.File.ReadAllText(strPath, Encoding.Default);

            if (strResult.Contains("404 Not Found"))
            {
                throw new Exception("404 Not Found");
            }

            CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints>(strResult);

            return obj;
        }
    }
}
