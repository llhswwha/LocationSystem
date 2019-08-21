using LocationServices.Locations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Person;
using TModel.BaseData;
using TModel.Location.AreaAndDev;
using TModel.Location.Work;
using TModel.LocationHistory.AreaAndDev;
using TModel.LocationHistory.Work;
using WebApiLib.Clients;
using LocationServer;
using BLL;
using DbModel;
using LocationServices.Converters;

namespace LocationServices.Locations.Services
{

    public class BaseDataService : IBaseDataService
    {

        BaseDataClient client = null;

        private BaseDataClient GetClient()
        {
            if (client == null)
            {
                var url = AppContext.DatacaseWebApiUrl;
                //return new BaseDataClient("localhost","9347");
                client = new BaseDataClient(url, null, "api");
            }
            return client;
        }
        private Bll _dbEx = null;
        /// <summary>
        /// 有关联实体属性的
        /// </summary>
        protected Bll dbEx
        {
            get
            {
                if (_dbEx == null)
                {
                    _dbEx = new Bll();
                }
                return _dbEx;
            }
        }


        private Bll db;
        public BaseDataService()
        {
            db = Bll.NewBllNoRelation();
        }

        public BaseDataService(Bll bll)
        {
            this.db = bll;
        }



        public List<EntranceGuardCard> GetCardList()
        {
            var client = GetClient();
            var recv = client.GetGuardCardList(true);
            if (recv == null)
            {
                return new List<EntranceGuardCard>();
            }

            return recv.ToTModel();
        }

        public List<PatrolPoint> Getcheckpoints(int InspectionId)
        {
            List<DbModel.Location.Work.PatrolPoint> lst = new List<DbModel.Location.Work.PatrolPoint>();

            try
            {
                lst = dbEx.PatrolPoints.Where(p => p.ParentId == InspectionId).ToList();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            return lst.ToWcfModelList();
        }

        public List<PatrolPointItem> Getcheckresults(int patrolId)
        {
            List<DbModel.Location.Work.PatrolPointItem> lst = new List<DbModel.Location.Work.PatrolPointItem>();

            try
            {
                lst = dbEx.PatrolPointItems.Where(p => p.ParentId == patrolId).ToList();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            return lst.ToWcfModelList();
        }

        public List<DevInfo> GetDeviceList(string types, string code, string name)
        {
            var client = GetClient();
            var recv = client.GetDevInfoList(types, code, name, true);
            if (recv == null)
            {
                return null;
            }
            return recv.ToWcfModelList();
        }

        public List<DeviceAlarm> GeteventsList(int? src, int? level, long? begin_t, long? end_t)
        {
            var client = GetClient();
            var recv = client.GetDevAlarmList(src, level, begin_t, end_t);
            if (recv == null)
            {
                return new List<DeviceAlarm>();
            }

            return recv.ToTModel();
        }

        public List<InspectionTrackHistory> Getinspectionhistorylist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag)
        {
            try
            {
                List<DbModel.LocationHistory.Work.InspectionTrackHistory> lst = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>();
                if (bFlag)
                {
                    lst = dbEx.InspectionTrackHistorys.ToList();
                }
                else
                {
                    long lBeginTime = Location.TModel.Tools.TimeConvert.ToStamp(dtBeginTime);
                    long lEndTime = Location.TModel.Tools.TimeConvert.ToStamp(dtEndTime);

                    lst = dbEx.InspectionTrackHistorys.Where(p => p.StartTime >= lBeginTime && p.EndTime <= lEndTime).ToList();

                }
                if (lst != null)
                {
                    foreach (var item in lst)
                    {
                        if (item == null) continue;
                        else if (item.Route.Count == 0)
                        {
                            item.Route = null;
                        }
                        else
                        {
                            foreach (var check in item.Route)
                            {
                                if (check.Checks == null) continue;
                                else
                                {
                                    if (check.Checks.Count == 0)
                                    {
                                        check.Checks = null;
                                    }
                                }
                            }
                        }
                    }
                }
                List<TModel.LocationHistory.Work.InspectionTrackHistory> tempList = lst.ToWcfModelList();
                return tempList;
            }
            catch (Exception e)
            {
                string error = e.ToString();
                return null;
            }
        }

        public List<InspectionTrack> Getinspectionlist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag)//WCF已注释
        {
            throw new NotImplementedException();
        }

        public InspectionTrack GetInspectionTrackById(InspectionTrack trackId)
        {
            List<DbModel.Location.Work.InspectionTrack> lst = dbEx.InspectionTracks.ToList();
            if (lst == null) return null;
            DbModel.Location.Work.InspectionTrack trackDBModel = lst.Find(i => i.Id == trackId.Id);
            if (trackDBModel != null)
            {
                InspectionTrack inspectTemp = trackDBModel.ToTModel(false);
                return inspectTemp;
            }
            else
            {
                return null;
            }
        }

        public List<Department> GetorgList()
        {
            var client = GetClient();
            var recv = client.GetDepList(true);
            if (recv == null)
            {
                return null;
            }
            return recv.ToWcfModelList();
        }

        public List<EntranceGuardActionInfo> GetSingleCardActionHistory(int id, string begin_date, string end_date)
        {
            var client = GetClient();
            var recv = client.GetSingleCardActionHistory(id, begin_date, end_date);
            if (recv == null)
            {
                return new List<EntranceGuardActionInfo>();
            }

            return recv;
        }

        public void GetSingleDeviceActionHistory(int id, string begin_date, string end_date)
        {
            var client = GetClient();
            var recv = client.GetSingleDeviceActionHistory(id, begin_date, end_date);
            if (recv == null)
            {
                return;
            }

            return;
        }

        public DevInfo GetSingleDeviceInfo(int id)
        {
            var client = GetClient();
            var recv = client.GetDevInfoDetail(id);
            if (recv == null)
            {
                return null;
            }
            return recv.ToTModel();
        }

        public PhysicalTopology GetSingleZonesInfo(int id, int view)
        {
            var client = GetClient();
            var recv = client.GetAreaDetail(id, view);
            if (recv == null)
            {
                return null;
            }

            return recv.ToTModel();
        }

        public void GetSisSamplingHistoryList(string kks)
        {
            var client = GetClient();
            var recv = client.GetSisSamplingHistoryList(kks);
            if (recv == null)
            {
                return;
            }

            return;
        }

        public void GetSomeSisHistoryList(string kks, bool compact)
        {
            var client = GetClient();
            var recv = client.GetSomeSisHistoryList(kks, compact);
            if (recv == null)
            {
                return;
            }

            return;
        }

        public   List<DevMonitorNode> GetSomesisList(List<string> tags)
        {
            var client = GetClient();
            List<DevMonitorNode> result = new List<DevMonitorNode>();
            string tmp = "";
            for (int i = 0; i < tags.Count; i++)
            {
                var tag = tags[i];

                if (tag.Contains("/"))
                {
                    tag = tag.Replace("/", "");//todo:有其他办法吗？
                }

                if (tag.Contains("#"))
                {
                    tag = tag.Replace("#", "");//todo:有其他办法吗？
                }

                if (tmp == "")
                {
                    tmp = tag;//第一个
                    continue;
                }

                string url = client.GetSisUrl(tmp + "," + tag);
                if (url.Length > AppSetting.UrlMaxLength)
                {
                    var recv = client.GetSomesisList(tmp, true);
                    if (recv != null)
                    {
                        result.AddRange(recv.ToTModel());
                    }
                    tmp = tag;
                }
                else
                {
                    tmp += "," + tag;
                }
            }

            {
                if (tmp != "")//把剩下的也获取
                {
                    var recv = client.GetSomesisList(tmp, true);
                    if (recv != null)
                    {
                        result.AddRange(recv.ToTModel());
                    }
                }
            }

            return result;
        }

        public  List<DevMonitorNode> GetSomesisList(string strTags)
        {
            var tagList = strTags.Split(',').ToList();
            return GetSomesisList(tagList);
        }

        public Ticket GetTicketDetial(int id, string begin_date, string end_date)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> GetTicketList(int type, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public void GetTicketsDetail(int id, string begin_date, string end_date)
        {
            throw new NotImplementedException();
        }

        public void GetTicketsList(string type, string begin_date, string end_date)
        {
            throw new NotImplementedException();
        }

        public List<Personnel> GetUserList()
        {
            throw new NotImplementedException();
        }

        public List<DevInfo> GetZoneDevList(int id)
        {
            throw new NotImplementedException();
        }

        public List<PhysicalTopology> GetZonesList()
        {
            throw new NotImplementedException();
        }

        public void Trys(InspectionTrackList aa)
        {
            throw new NotImplementedException();
        }
    }
}
