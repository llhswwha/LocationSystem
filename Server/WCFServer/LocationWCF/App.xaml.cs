using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using DAL;
using log4net.Config;
using Location.BLL.Tool;
using TModel.Tools;
using LocationServer;
using LocationServer.Tools;
using System.Threading;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.Work;
using DbModel.LocationHistory.Work;
using DbModel.Tools;
using Location.TModel.Tools;
using LocationServices.Converters;
using DbModel.Location.AreaAndDev;
using WebApiLib.Clients;
using TModel.Models.Settings;

namespace LocationWCFServer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Thread tGetInspectionTrack = null;
        private int nEightHourSecond = 28800;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            XmlConfigurator.Configure();

            Log.StartWatch();
            Log.AppStart();
            Log.Info("App_OnStartup");

            //LocationDbLite db = new LocationDbLite();
            ////db.Database.Create();
            //var list = db.Books.ToList();
            //if (list.Count == 0)
            //{
            //    db.Books.Add(new Book() { Name = "Book1" });
            //    db.Books.Add(new Book() { Name = "Book2" });
            //    db.Books.Add(new Book() { Name = "Book3" });
            //    db.SaveChanges();
            //}

            //Bll db = new Bll();
            ////bll.InitDevModelAndType();

            //var devs = db.DevInfos.ToList();

            int mode = ConfigurationHelper.GetIntValue("ShowSqlLog");
            if (mode == 1)
            {
                BLL.Bll.ShowLog = true;
            }

            InitDbContext();

            InitData();
            
            AppContext.AutoStartServer= ConfigurationHelper.GetIntValue("AutoStartServer") ==0;
            AppContext.WritePositionLog = ConfigurationHelper.GetBoolValue("WritePositionLog");
            AppContext.PositionMoveStateWaitTime = ConfigurationHelper.GetDoubleValue("PositionMoveStateWaitTime");
            datacaseUrl = ConfigurationHelper.GetValue("DatacaseWebApiUrl");
            LocationContext.LoadOffset(ConfigurationHelper.GetValue("LocationOffset"));

            InitGetInspectionTrack();

            //SystemSetting setting = new SystemSetting();
            //XmlSerializeHelper.Save(setting,AppDomain.CurrentDomain.BaseDirectory + "\\default.xml");
        }

        private string datacaseUrl = "ipms-demo.datacase.io";

        private void InitGetInspectionTrack()
        {
            Log.Info("InitGetInspectionTrack");
            if (tGetInspectionTrack == null)
            {
                tGetInspectionTrack = new Thread(GetInspectionTrackThread);
            }
            tGetInspectionTrack.IsBackground = true;
            tGetInspectionTrack.Start();
        }

        private static void InitDbContext()
        {
            string dbType = ConfigurationHelper.GetValue("DbSourceType");
            AppContext.InitDbContext(dbType);
        }

        

        private void InitData()
        {
            Log.Info("InitData");
            int mode = ConfigurationHelper.GetIntValue("DataInitMode"); //-1:不初始化,0:EF,1:Sql
            Log.Info("DataInitMode:" + mode);
            if (mode >= 0)
            {
                AppContext.InitDb(mode);
            }
        }

        private void GetInspectionTrackThread()
        {
            var client = new BaseDataClient(datacaseUrl, "api");
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
                    Log.Error("获取巡检轨迹失败！！ break!!");
                    break;
                }
               
                List<InspectionTrack> send = bll.InspectionTracks.ToList();
                //List<InspectionTrackHistory> send2 = bll.InspectionTrackHistorys.ToList();
                if (send == null || send.Count() == 0)
                {
                    Thread.Sleep(5 * 60 * 1000);//等待5分钟
                    continue;
                }
                SignalRService.Hubs.InspectionTrackHub.SendInspectionTracks(send.ToWcfModelList().ToArray());
                //DateTime dt2 = DateTime.Now;
                Thread.Sleep(5*60*1000);//等待5分钟
            }
        }

        private bool DealInspectionTrack(BaseDataClient client, DateTime dtBegin, DateTime dtEnd, bool bFlag)
        {
            List<InspectionTrack> All = new List<InspectionTrack>();
            List<InspectionTrack> Add = new List<InspectionTrack>();
            List<InspectionTrack> Edit = new List<InspectionTrack>();
            List<InspectionTrack> Delete = new List<InspectionTrack>();
            List<InspectionTrackHistory> HAdd = new List<InspectionTrackHistory>();

            long lBegin = TimeConvert.DateTimeToTimeStamp(dtBegin) / 1000;
            long lEnd = TimeConvert.DateTimeToTimeStamp(dtEnd) / 1000;
            var recv = client.Getinspectionlist(lBegin, lEnd, true);
            if (recv == null)
            {
                return false;
            }

            Bll bll = new Bll(false, false, true, false);//第三参数要设置为true
            List<InspectionTrack> itList = bll.InspectionTracks.ToList();
            if (itList == null)
            {
                itList = new List<InspectionTrack>();
            }

            List<InspectionTrackHistory> itHList = bll.InspectionTrackHistorys.ToList();
            if (itHList == null)
            {
                itHList = new List<InspectionTrackHistory>(0);
            }

            foreach (patrols item in recv)
            {
                InspectionTrack now = itList.Find(p => p.Abutment_Id == item.id);
                InspectionTrackHistory history = itHList.Find(p => p.Abutment_Id == item.id);

                if (item.state == "新建" || item.state == "已下达" || item.state == "执行中")
                {
                    if (now == null)
                    {
                        now = new InspectionTrack();

                        now.Abutment_Id = item.id;
                        now.Code = item.code;
                        now.Name = item.name;
                        now.CreateTime = (item.createTime + nEightHourSecond) * 1000;
                        now.dtCreateTime = TimeConvert.TimeStampToDateTime(now.CreateTime);
                        now.State = item.state;
                        now.StartTime = (item.startTime + nEightHourSecond) * 1000;
                        now.dtStartTime = TimeConvert.TimeStampToDateTime(now.StartTime);
                        now.EndTime = (item.endTime + nEightHourSecond) * 1000;
                        now.dtEndTime = TimeConvert.TimeStampToDateTime(now.EndTime);
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
                        history = new InspectionTrackHistory();

                        history.Abutment_Id = item.id;
                        history.Code = item.code;
                        history.Name = item.name;
                        history.CreateTime = (item.createTime + nEightHourSecond) * 1000;
                        history.dtCreateTime = TimeConvert.TimeStampToDateTime(history.CreateTime);
                        history.State = item.state;
                        history.StartTime = (item.startTime + nEightHourSecond) * 1000;
                        history.dtStartTime = TimeConvert.TimeStampToDateTime(history.StartTime);
                        history.EndTime = (item.endTime + nEightHourSecond) * 1000;
                        history.dtEndTime = TimeConvert.TimeStampToDateTime(history.EndTime);

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

        private void DealPatrolPoint(Bll bll, List<InspectionTrack> All, List<InspectionTrack> Delete, List<InspectionTrackHistory> HAdd, WebApiLib.Clients.BaseDataClient client)
        {
            List<PatrolPoint> ppList = bll.PatrolPoints.ToList();
            List<PatrolPointHistory> ppHList = bll.PatrolPointHistorys.ToList();
            List<DbModel.Location.AreaAndDev.DevInfo> devList = bll.DevInfos.ToList();
            if (ppList == null)
            {
                ppList = new List<PatrolPoint>();
            }

            if (ppHList == null)
            {
                ppHList = new List<PatrolPointHistory>();
            }

            if (devList == null)
            {
                devList = new List<DbModel.Location.AreaAndDev.DevInfo>();
            }

            List<PatrolPoint> PAll = new List<PatrolPoint>();
            List<PatrolPoint> PAdd = new List<PatrolPoint>();
            List<PatrolPoint> PEdit = new List<PatrolPoint>();
            List<PatrolPoint> PDelete = new List<PatrolPoint>();
            List<PatrolPointHistory> PHAdd = new List<PatrolPointHistory>();

            foreach (InspectionTrack item in All)
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
                    DbModel.Location.AreaAndDev.DevInfo dev1 = devList.Find(p=>p.KKS == item2.kksCode);
                    PatrolPoint now = ppList.Find(p => p.DeviceId == item2.deviceId && p.ParentId == Id);
                    if (now == null)
                    {
                        now = new PatrolPoint();

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

            foreach (InspectionTrack item in Delete)
            {
                int Id = item.Id;
                List<PatrolPoint> lstDelete = ppList.FindAll(p => p.ParentId == Id).ToList();
                if (lstDelete != null && lstDelete.Count() > 0)
                {
                    PDelete.AddRange(lstDelete);
                }
            }

            foreach (InspectionTrackHistory item in HAdd)
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
                    PatrolPointHistory history = ppHList.Find(p => p.DeviceId == item2.deviceId && p.ParentId == Id);
                    if (history == null)
                    {
                        history = new PatrolPointHistory();

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

        private void DealPatrolPointItem(Bll bll, List<InspectionTrack> All, List<InspectionTrackHistory> HAdd, List<PatrolPoint> PAll, List<PatrolPoint> PDelete, List<PatrolPointHistory> PHAdd, WebApiLib.Clients.BaseDataClient client)
        {
            try
            {

                List<PatrolPointItem> ppiList = bll.PatrolPointItems.ToList();
                List<PatrolPointItemHistory> ppiHList = bll.PatrolPointItemHistorys.ToList();
                if (ppiList == null)
                {
                    ppiList = new List<PatrolPointItem>();
                }

                if (ppiHList == null)
                {
                    ppiHList = new List<PatrolPointItemHistory>();
                }

                List<PatrolPointItem> PIAll = new List<PatrolPointItem>();
                List<PatrolPointItem> PIAdd = new List<PatrolPointItem>();
                List<PatrolPointItem> PIEdit = new List<PatrolPointItem>();
                List<PatrolPointItem> PIDelete = new List<PatrolPointItem>();
                List<PatrolPointItemHistory> PIHAdd = new List<PatrolPointItemHistory>();


                foreach (PatrolPoint item in PAll)
                {
                    int Id = item.Id;
                    string deviceId = item.DeviceId;
                    int ParentId = item.ParentId;
                    InspectionTrack it = All.Find(p => p.Id == ParentId);
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
                        PatrolPointItem now = ppiList.Find(p => p.CheckId == item2.checkId && p.ParentId == Id);

                        if (now == null)
                        {
                            now = new PatrolPointItem();
                            now.ParentId = Id;
                            now.KksCode = item2.kksCode;
                            now.CheckItem = item2.checkItem;
                            now.StaffCode = item2.staffCode;
                            now.CheckTime = null;
                            now.dtCheckTime = null;
                            if (item2.checkTime != null)
                            {
                                now.CheckTime = (item2.checkTime + nEightHourSecond) * 1000;
                                now.dtCheckTime = TimeConvert.TimeStampToDateTime((long)now.CheckTime);
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
                                now.dtCheckTime = TimeConvert.TimeStampToDateTime((long)now.CheckTime);
                            }

                            now.CheckResult = item2.checkResult;
                            PIEdit.Add(now);
                        }
                    }
                }

                foreach (PatrolPoint item in PDelete)
                {
                    int Id = item.Id;
                    List<PatrolPointItem> lstDelete = ppiList.FindAll(p => p.ParentId == Id).ToList();
                    if (lstDelete != null && lstDelete.Count() > 0)
                    {
                        PIDelete.AddRange(lstDelete);
                    }
                }

                foreach (PatrolPointHistory item in PHAdd)
                {
                    int Id = item.Id;
                    string deviceId = item.DeviceId;
                    int ParentId = item.ParentId;
                    InspectionTrackHistory ith = HAdd.Find(p => p.Id == ParentId);
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
                        PatrolPointItemHistory history = bll.PatrolPointItemHistorys.Find(p => p.CheckId == item2.checkId && p.ParentId == Id);

                        if (history == null)
                        {
                            history = new PatrolPointItemHistory();
                            history.ParentId = Id;
                            history.KksCode = item2.kksCode;
                            history.CheckItem = item2.checkItem;
                            history.StaffCode = item2.staffCode;
                            history.CheckTime = null;
                            history.dtCheckTime = null;
                            if (item2.checkTime != null)
                            {
                                history.CheckTime = (item2.checkTime + nEightHourSecond) * 1000;
                                history.dtCheckTime = TimeConvert.TimeStampToDateTime((long)history.CheckTime);
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
