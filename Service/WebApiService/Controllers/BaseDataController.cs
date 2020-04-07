using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Person;
using TModel.BaseData;
using TModel.Location.AreaAndDev;
using TModel.Location.Work;
using TModel.LocationHistory.AreaAndDev;
using TModel.LocationHistory.Work;
using LocationServices.Locations.Services;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/BaseDate")]
    public class BaseDataController : ApiController, IBaseDataService
    {
        protected IBaseDataService service;

        public BaseDataController()
        {
            service = new BaseDataService();
        }
        [Route("")]
        public List<EntranceGuardCard> GetCardList()
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<PatrolPoint> Getcheckpoints(int InspectionId)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<PatrolPointItem> Getcheckresults(int patrolId)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<DevInfo> GetDeviceList(string types, string code, string name)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<DeviceAlarm> GeteventsList(int? src, int? level, long? begin_t, long? end_t)
        {
            throw new NotImplementedException();
        }

        public InspectionTrackHistory GetInspectionHistoryById(InspectionTrackHistory history)
        {
            throw new NotImplementedException();
        }

        [Route("list/inspectionHis/begin/{dtBeginTime}/end/{dtEndTime}/bFlag/{bFlag}")]
        public List<InspectionTrackHistory> Getinspectionhistorylist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag)
        {
            return service.Getinspectionhistorylist(dtBeginTime,dtEndTime,bFlag);
        }
        [Route("list/inspection/begin/{dtBeginTime}/end/{dtEndTime}/bFlag/{bFlag}")]
        public List<InspectionTrack> Getinspectionlist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag)
        {
            return service.Getinspectionlist(dtBeginTime,dtBeginTime,bFlag);
        }
        [Route("track/trackId")]
        [HttpPost]
        public InspectionTrack GetInspectionTrackById(InspectionTrack trackId)
        {
            //throw new NotImplementedException();
            return service.GetInspectionTrackById(trackId);
        }
        [Route("")]
        public List<Department> GetorgList()
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<EntranceGuardActionInfo> GetSingleCardActionHistory(int id, string begin_date, string end_date)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public void GetSingleDeviceActionHistory(int id, string begin_date, string end_date)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public DevInfo GetSingleDeviceInfo(int id)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public PhysicalTopology GetSingleZonesInfo(int id, int view)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public void GetSisSamplingHistoryList(string kks)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public void GetSomeSisHistoryList(string kks, bool compact)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<DevMonitorNode> GetSomesisList(List<string> strTags)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public Ticket GetTicketDetial(int id, string begin_date, string end_date)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<Ticket> GetTicketList(int type, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public void GetTicketsDetail(int id, string begin_date, string end_date)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public void GetTicketsList(string type, string begin_date, string end_date)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<DevInfo> GetZoneDevList(int id)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<PhysicalTopology> GetZonesList()
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public void Trys(InspectionTrackList aa)
        {
            throw new NotImplementedException();
        }
    }
}
