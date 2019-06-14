using BLL;
using Location.BLL.Tool;
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

        private int nEightHourSecond = 28800;

        public event Action<List<DbModel.Location.Work.InspectionTrack>> ListGot;

        public InspectionTrackClient(string url)
        {
            webApiUrl = url;
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


        private void InnerOperate()
        {
            //string strIp = ConfigurationManager.AppSettings["DatacaseWebApiUrl"];
            //string datacaseUrl = string.Format("http://{0}:{1}/", strIp);

            var client = new BaseDataClient(webApiUrl, null,"api");
            bool bFirst = true;
            int nDay = -1;
            Bll bll = new Bll();

            while (true)
            {

                DateTime dtEnd = DateTime.Now;
                DateTime dtBegin = dtEnd.AddDays(nDay);
                if (bFirst)
                {
                    dtBegin = dtEnd.AddDays(-3);
                    bFirst = false;
                }

                if (DealInspectionTrack(client, dtBegin, dtEnd, true) == false)
                {
                    Log.Info(LogTags.Server,"获取巡检轨迹失败！！ break!!");
                    break;
                }

                List<DbModel.Location.Work.InspectionTrack> trackList = bll.InspectionTracks.ToList();
                //List<InspectionTrackHistory> send2 = bll.InspectionTrackHistorys.ToList();
                if (trackList == null || trackList.Count() == 0)
                {
                    Thread.Sleep(5 * 60 * 1000);//等待5分钟
                    continue;
                }

                if (ListGot != null)
                {
                    ListGot(trackList);
                }
                //SignalRService.Hubs.InspectionTrackHub.SendInspectionTracks(trackList.ToWcfModelList().ToArray());
                //DateTime dt2 = DateTime.Now;
                Thread.Sleep(5 * 60 * 1000);//等待5分钟
            }
        }

        private bool DealInspectionTrack(WebApiLib.Clients.BaseDataClient client, DateTime dtBegin, DateTime dtEnd, bool bFlag)
        {
            List<DbModel.Location.Work.InspectionTrack> All = new List<DbModel.Location.Work.InspectionTrack>();
            List<DbModel.Location.Work.InspectionTrack> Add = new List<DbModel.Location.Work.InspectionTrack>();
            List<DbModel.Location.Work.InspectionTrack> Edit = new List<DbModel.Location.Work.InspectionTrack>();
            List<DbModel.Location.Work.InspectionTrack> Delete = new List<DbModel.Location.Work.InspectionTrack>();
            List<DbModel.LocationHistory.Work.InspectionTrackHistory> HAdd = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>();

            long lBegin = Location.TModel.Tools.TimeConvert.DateTimeToTimeStamp(dtBegin) / 1000;
            long lEnd = Location.TModel.Tools.TimeConvert.DateTimeToTimeStamp(dtEnd) / 1000;

            var recv = client.Getinspectionlist(lBegin, lEnd, true);
            if (recv == null)
            {
                return false;
            }

            Bll bll = new Bll(false, false, true, false);//第三参数要设置为true
            List<DbModel.Location.Work.InspectionTrack> itList = bll.InspectionTracks.ToList();
            if (itList == null)
            {
                itList = new List<DbModel.Location.Work.InspectionTrack>();
            }

            List<DbModel.LocationHistory.Work.InspectionTrackHistory> itHList = bll.InspectionTrackHistorys.ToList();
            if (itHList == null)
            {
                itHList = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>(0);
            }

            foreach (CommunicationClass.SihuiThermalPowerPlant.Models.patrols item in recv)
            {
                DbModel.Location.Work.InspectionTrack now = itList.Find(p => p.Abutment_Id == item.id);
                DbModel.LocationHistory.Work.InspectionTrackHistory history = itHList.Find(p => p.Abutment_Id == item.id);

                if (item.state == "新建" || item.state == "已下达" || item.state == "执行中")
                {
                    if (now == null)
                    {
                        now = new DbModel.Location.Work.InspectionTrack();

                        now.Abutment_Id = item.id;
                        now.Code = item.code;
                        now.Name = item.name;
                        now.CreateTime = (item.createTime + nEightHourSecond) * 1000;
                        now.dtCreateTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(now.CreateTime);
                        now.State = item.state;
                        now.StartTime = (item.startTime + nEightHourSecond) * 1000;
                        now.dtStartTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(now.StartTime);
                        now.EndTime = (item.endTime + nEightHourSecond) * 1000;
                        now.dtEndTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(now.EndTime);
                        Add.Add(now);
                    }
                    else
                    {
                        now.State = item.state;
                        Edit.Add(now);
                    }
                }
                else
                {
                    if (now != null)
                    {
                        Delete.Add(now);
                    }

                    if (history == null)
                    {
                        history = new DbModel.LocationHistory.Work.InspectionTrackHistory();

                        history.Abutment_Id = item.id;
                        history.Code = item.code;
                        history.Name = item.name;
                        history.CreateTime = (item.createTime + nEightHourSecond) * 1000;
                        history.dtCreateTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(history.CreateTime);
                        history.State = item.state;
                        history.StartTime = (item.startTime + nEightHourSecond) * 1000;
                        history.dtStartTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(history.StartTime);
                        history.EndTime = (item.endTime + nEightHourSecond) * 1000;
                        history.dtEndTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(history.EndTime);

                        HAdd.Add(history);
                    }
                }
            }

            bll.InspectionTracks.AddRange(Add);
            bll.InspectionTracks.EditRange(Edit);
            bll.InspectionTracks.RemoveList(Delete);
            bll.InspectionTrackHistorys.AddRange(HAdd);

            All.AddRange(Add);
            All.AddRange(Edit);
            DealPatrolPoint(bll, All, Delete, HAdd, client);

            return true;
        }


        private void DealPatrolPoint(Bll bll, List<DbModel.Location.Work.InspectionTrack> All, List<DbModel.Location.Work.InspectionTrack> Delete, List<DbModel.LocationHistory.Work.InspectionTrackHistory> HAdd, WebApiLib.Clients.BaseDataClient client)
        {
            List<DbModel.Location.Work.PatrolPoint> ppList = bll.PatrolPoints.ToList();
            List<DbModel.LocationHistory.Work.PatrolPointHistory> ppHList = bll.PatrolPointHistorys.ToList();
            List<DbModel.Location.AreaAndDev.DevInfo> devList = bll.DevInfos.ToList();
            if (ppList == null)
            {
                ppList = new List<DbModel.Location.Work.PatrolPoint>();
            }

            if (ppHList == null)
            {
                ppHList = new List<DbModel.LocationHistory.Work.PatrolPointHistory>();
            }

            if (devList == null)
            {
                devList = new List<DbModel.Location.AreaAndDev.DevInfo>();
            }

            List<DbModel.Location.Work.PatrolPoint> PAll = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PAdd = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PEdit = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PDelete = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.LocationHistory.Work.PatrolPointHistory> PHAdd = new List<DbModel.LocationHistory.Work.PatrolPointHistory>();

            foreach (DbModel.Location.Work.InspectionTrack item in All)
            {
                int Id = item.Id;
                int patrolId = (int)item.Abutment_Id;
                CommunicationClass.SihuiThermalPowerPlant.Models.patrols recv = client.Getcheckpoints(patrolId);
                if (recv == null || recv.route.Count() <= 0)
                {
                    continue;
                }

                foreach (CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints item2 in recv.route)
                {
                    DbModel.Location.AreaAndDev.DevInfo dev1 = devList.Find(p => p.KKS == item2.kksCode);
                    DbModel.Location.Work.PatrolPoint now = ppList.Find(p => p.DeviceId == item2.deviceId && p.ParentId == Id);
                    if (now == null)
                    {
                        now = new DbModel.Location.Work.PatrolPoint();

                        now.ParentId = Id;
                        now.StaffCode = item2.staffCode;
                        now.StaffName = item2.staffName;
                        now.KksCode = item2.kksCode;
                        now.DevName = item2.deviceName;
                        now.DeviceCode = item2.deviceCode;
                        now.DeviceId = item2.deviceId;
                        if (dev1 != null)
                        {
                            now.DevId = dev1.Id;
                        }

                        PAdd.Add(now);
                    }
                    else
                    {
                        now.ParentId = Id;
                        now.StaffCode = item2.staffCode;
                        now.StaffName = item2.staffName;
                        now.KksCode = item2.kksCode;
                        now.DevName = item2.deviceName;
                        now.DeviceCode = item2.deviceCode;
                        now.DeviceId = item2.deviceId;
                        if (dev1 != null)
                        {
                            now.DevId = dev1.Id;
                        }

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

            foreach (DbModel.LocationHistory.Work.InspectionTrackHistory item in HAdd)
            {
                int Id = item.Id;
                int patrolId = (int)item.Abutment_Id;
                CommunicationClass.SihuiThermalPowerPlant.Models.patrols recv = client.Getcheckpoints(patrolId);
                if (recv == null || recv.route.Count() <= 0)
                {
                    continue;
                }

                foreach (CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints item2 in recv.route)
                {
                    DbModel.Location.AreaAndDev.DevInfo dev1 = devList.Find(p => p.KKS == item2.kksCode);
                    DbModel.LocationHistory.Work.PatrolPointHistory history = ppHList.Find(p => p.DeviceId == item2.deviceId && p.ParentId == Id);
                    if (history == null)
                    {
                        history = new DbModel.LocationHistory.Work.PatrolPointHistory();

                        history.ParentId = Id;
                        history.StaffCode = item2.staffCode;
                        history.StaffName = item2.staffName;
                        history.KksCode = item2.kksCode;
                        history.DevName = item2.deviceName;
                        history.DeviceCode = item2.deviceCode;
                        history.DeviceId = item2.deviceId;
                        if (dev1 != null)
                        {
                            history.DevId = dev1.Id;
                        }

                        PHAdd.Add(history);
                    }
                }
            }

            bll.PatrolPoints.AddRange(PAdd);
            bll.PatrolPoints.EditRange(PEdit);
            bll.PatrolPoints.RemoveList(PDelete);
            bll.PatrolPointHistorys.AddRange(PHAdd);

            PAll.AddRange(PAdd);
            PAll.AddRange(PEdit);
            DealPatrolPointItem(bll, All, HAdd, PAll, PDelete, PHAdd, client);

            return;
        }

        private void DealPatrolPointItem(Bll bll, List<DbModel.Location.Work.InspectionTrack> All, List<DbModel.LocationHistory.Work.InspectionTrackHistory> HAdd, List<DbModel.Location.Work.PatrolPoint> PAll, List<DbModel.Location.Work.PatrolPoint> PDelete, List<DbModel.LocationHistory.Work.PatrolPointHistory> PHAdd, WebApiLib.Clients.BaseDataClient client)
        {
            try
            {

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


                foreach (DbModel.Location.Work.PatrolPoint item in PAll)
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
                    CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints recv = client.Getcheckresults(patrolId, deviceId);
                    if (recv == null || recv.checks.Count() <= 0)
                    {
                        continue;
                    }

                    foreach (CommunicationClass.SihuiThermalPowerPlant.Models.results item2 in recv.checks)
                    {
                        DbModel.Location.Work.PatrolPointItem now = ppiList.Find(p => p.CheckId == item2.checkId && p.ParentId == Id);

                        if (now == null)
                        {
                            now = new DbModel.Location.Work.PatrolPointItem();
                            now.ParentId = Id;
                            now.KksCode = item2.kksCode;
                            now.CheckItem = item2.checkItem;
                            now.StaffCode = item2.staffCode;
                            now.CheckTime = null;
                            now.dtCheckTime = null;
                            if (item2.checkTime != null)
                            {
                                now.CheckTime = (item2.checkTime + nEightHourSecond) * 1000;
                                now.dtCheckTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime((long)now.CheckTime);
                            }
                            now.CheckId = item2.checkId;
                            now.CheckResult = item2.checkResult;
                            PIAdd.Add(now);
                        }
                        else
                        {
                            if (item2.checkTime != null)
                            {
                                now.CheckTime = (item2.checkTime + nEightHourSecond) * 1000;
                                now.dtCheckTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime((long)now.CheckTime);
                            }

                            now.CheckResult = item2.checkResult;
                            PIEdit.Add(now);
                        }
                    }
                }

                foreach (DbModel.Location.Work.PatrolPoint item in PDelete)
                {
                    int Id = item.Id;
                    List<DbModel.Location.Work.PatrolPointItem> lstDelete = ppiList.FindAll(p => p.ParentId == Id).ToList();
                    if (lstDelete != null && lstDelete.Count() > 0)
                    {
                        PIDelete.AddRange(lstDelete);
                    }
                }

                foreach (DbModel.LocationHistory.Work.PatrolPointHistory item in PHAdd)
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
                    CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints recv = client.Getcheckresults(patrolId, deviceId);
                    if (recv == null || recv.checks.Count() <= 0)
                    {
                        continue;
                    }

                    foreach (CommunicationClass.SihuiThermalPowerPlant.Models.results item2 in recv.checks)
                    {
                        DbModel.LocationHistory.Work.PatrolPointItemHistory history = bll.PatrolPointItemHistorys.Find(p => p.CheckId == item2.checkId && p.ParentId == Id);

                        if (history == null)
                        {
                            history = new DbModel.LocationHistory.Work.PatrolPointItemHistory();
                            history.ParentId = Id;
                            history.KksCode = item2.kksCode;
                            history.CheckItem = item2.checkItem;
                            history.StaffCode = item2.staffCode;
                            history.CheckTime = null;
                            history.dtCheckTime = null;
                            if (item2.checkTime != null)
                            {
                                history.CheckTime = (item2.checkTime + nEightHourSecond) * 1000;
                                history.dtCheckTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime((long)history.CheckTime);
                            }
                            history.CheckId = item2.checkId;
                            history.CheckResult = item2.checkResult;
                            PIHAdd.Add(history);
                        }
                    }
                }

                bll.PatrolPointItems.AddRange(PIAdd);
                bll.PatrolPointItems.EditRange(PIEdit);
                bll.PatrolPointItems.RemoveList(PIDelete);
                bll.PatrolPointItemHistorys.AddRange(PIHAdd);
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
            }
            return;

        }
    }
}
