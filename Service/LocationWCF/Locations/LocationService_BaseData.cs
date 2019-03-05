using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BLL;
//using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.ServiceHelpers;
using Location.Model.DataObjects.ObjectAddList;
using LocationServices.Converters;
using TModel.BaseData;
using WebApiLib.Clients;
using TModel.Location.Work;
using TModel.LocationHistory.Work;
using Location.TModel.Location.Person;
using Location.TModel.Location.AreaAndDev;

namespace LocationServices.Locations
{
    //基础平台相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        public static string url = "";
        private BaseDataClient GetClient()
        {
            //return new BaseDataClient("localhost","9347");
            return new BaseDataClient(url, "api");
        }

        public Ticket GetTicketDetial(int id, string begin_date, string end_date)
        {
            var client = GetClient();
            var ticket=client.GetTicketsDetail(id, begin_date, end_date);
            if (ticket == null)
            {
                return null;
            }
            return ticket.ToTModel();
        }

        public List<Ticket> GetTicketList(int type, DateTime start, DateTime end)
        {
            var client = GetClient();
            var re=client.GetTicketsList(type+"", start.ToString("yyyyMMdd"), end.ToString("yyyyMMdd"));
            if (re == null)
            {
                return null;
            }
            return re.data.ToWcfModelList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtBeginTime">起始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="bFlag">值为True获取所有历史记录，值为False，按起始时间和结束时间获取历史记录</param>
        /// <returns></returns>
        public List<InspectionTrackHistory> Getinspectionhistorylist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag)
        {
            List<DbModel.LocationHistory.Work.InspectionTrackHistory> lst = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>();
            if (bFlag)
            {
                lst = db2.InspectionTrackHistorys.ToList();
            }
            else
            {
                long lBeginTime = Location.TModel.Tools.TimeConvert.DateTimeToTimeStamp(dtBeginTime);
                long lEndTime = Location.TModel.Tools.TimeConvert.DateTimeToTimeStamp(dtEndTime);

                lst = db2.InspectionTrackHistorys.Where(p=>p.StartTime >= lBeginTime && p.EndTime <= lEndTime).ToList();

            }

            return lst.ToWcfModelList();
        }

        /// <summary>
        /// 获取人员列表
        /// </summary>
        /// <returns></returns>
        public List<Personnel> GetUserList()
        {
            var client = GetClient();
            var recv = client.GetUserList();
            if (recv == null)
            {
                return null;
            }

            return recv.ToWcfModelList();
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<Department> GetorgList()
        {
            var client = GetClient();
            var recv = client.GetorgList();
            if (recv == null)
            {
                return null;
            }

            return recv.ToWcfModelList();
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        public List<PhysicalTopology> GetzonesList()
        {
            var client = GetClient();
            var recv = client.GetzonesList();
            if (recv == null)
            {
                return null;
            }
            return recv.ToWcfModelList();
        }

        /// <summary>
        /// 获取单个区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="view"></param>
        public PhysicalTopology GetSingleZonesInfo(int id, int view)
        {
            var client = GetClient();
            var recv = client.GetSingleZonesInfo(id, view);
            if (recv == null)
            {
                return null;
            }

            return recv.ToTModel();
        }

        /// <summary>
        /// 获取指定区域下设备列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DevInfo> GetZoneDevList(int id)
        {
            var client = GetClient();
            var recv = client.GetZoneDevList(id);
            if (recv == null)
            {
                return null;
            }
            return recv.ToWcfModelList();
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="types"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<DevInfo> GetDeviceList(string types, string code, string name)
        {
            var client = GetClient();
            var recv = client.GetDeviceList(types, code, name);
            if (recv == null)
            {
                return null;
            }
            return recv.ToWcfModelList();
        }

        /// <summary>
        /// 获取单个设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DevInfo GetSingleDeviceInfo(int id)
        {
            var client = GetClient();
            var recv = client.GetSingleDeviceInfo(id);
            if (recv == null)
            {
                return null;
            }
            return recv.ToTModel();
        }

        /// <summary>
        /// 获取单台设备操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
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

        /// <summary>
        /// 获取门禁卡列表
        /// </summary>
        public void GetCardList()
        {
            var client = GetClient();
            var recv = client.GetCardList();
            if (recv == null)
            {
                return;
            }

            return;
        }

        /// <summary>
        /// 获取门禁卡操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        public void GetSingleCardActionHistory(int id, string begin_date, string end_date)
        {
            var client = GetClient();
            var recv = client.GetSingleCardActionHistory(id, begin_date, end_date);
            if (recv == null)
            {
                return;
            }

            return;

        }

        /// <summary>
        /// 获取两票列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        public void GetTicketsList(string type, string begin_date, string end_date)
        {
            var client = GetClient();
            client.GetTicketsList(type, begin_date, end_date);
            
            return;
        }

        /// <summary>
        /// 获取指定的两票详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        public void GetTicketsDetail(int id, string begin_date, string end_date)
        {
            var client = GetClient();
            var recv = client.GetTicketsDetail(id, begin_date, end_date);
            if (recv == null)
            {
                return;
            }

            return;
        }

        /// <summary>
        /// 获取告警事件列表
        /// </summary>
        /// <param name="src"></param>
        /// <param name="level"></param>
        /// <param name="begin_t"></param>
        /// <param name="end_t"></param>
        public void GeteventsList(int? src, int? level, long? begin_t, long? end_t)
        {
            var client = GetClient();
            var recv = client.GeteventsList(src, level, begin_t, end_t);
            if (recv == null)
            {
                return;
            }

            return;

        }

        /// <summary>
        /// 获取SIS传感数据
        /// </summary>
        /// <param name="kks"></param>
        public void GetSomesisList(string kks)
        {
            var client = GetClient();
            var recv = client.GetSomesisList(kks);
            if (recv == null)
            {
                return;
            }

            return;
        }

        /// <summary>
        /// 获取SIS历史数据，当compact为true时，获取紧凑型数据，当compact为false时，获取非紧凑型数据
        /// </summary>
        /// <param name="kks"></param>
        /// <param name="compact"></param>
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

        /// <summary>
        /// 获取SIS采样历史数据
        /// </summary>
        /// <param name="kks"></param>
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

        /// <summary>
        /// 获取巡检轨迹列表
        /// </summary>
        /// <param name="lBegin"></param>
        /// <param name="lEnd"></param>
        /// <param name="bFlag"></param>
        public void Getinspectionlist(long lBegin, long lEnd, bool bFlag)
        {
            var client = GetClient();
            var recv = client.Getinspectionlist(lBegin, lEnd, bFlag);
            if (recv == null)
            {
                return;
            }

            return;
        }

        /// <summary>
        /// 获取巡检节点列表
        /// </summary>
        /// <param name="patrolId"></param>
        public void Getcheckpoints(int patrolId)
        {
            var client = GetClient();
            var recv = client.Getcheckpoints(patrolId);
            if (recv == null)
            {
                return;
            }

            return;
        }

        /// <summary>
        /// 获取巡检结果列表
        /// </summary>
        /// <param name="patrolId"></param>
        /// <param name="deviceId"></param>
        public void Getcheckresults(int patrolId, string deviceId)
        {
            var client = GetClient();
            var recv = client.Getcheckresults(patrolId, deviceId);
            if (recv == null)
            {
                return;
            }

            return;
        }
    }
}
