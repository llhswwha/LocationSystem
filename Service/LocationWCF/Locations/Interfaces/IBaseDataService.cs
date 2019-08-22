using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Person;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using TModel.BaseData;
using TModel.Location.AreaAndDev;
using TModel.Location.Work;
using TModel.LocationHistory.AreaAndDev;
using TModel.LocationHistory.Work;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IBaseDataService
    {
        [OperationContract]
        List<Ticket> GetTicketList(int type,DateTime start,DateTime end);

        [OperationContract]
        Ticket GetTicketDetial(int id, string begin_date, string end_date);

        [OperationContract]
        List<InspectionTrackHistory> Getinspectionhistorylist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag);

        //[OperationContract]
        //List<Personnel> GetUserList();

        [OperationContract]
        List<Department> GetorgList();

        [OperationContract]
        List<PhysicalTopology> GetZonesList();

        [OperationContract]
        PhysicalTopology GetSingleZonesInfo(int id, int view);

        [OperationContract]
        List<DevInfo> GetZoneDevList(int id);

        [OperationContract]
        List<DevInfo> GetDeviceList(string types, string code, string name);

        [OperationContract]
        DevInfo GetSingleDeviceInfo(int id);

        [OperationContract]
        void GetSingleDeviceActionHistory(int id, string begin_date, string end_date);

        [OperationContract]
        List<EntranceGuardCard> GetCardList();

        [OperationContract]
        List<EntranceGuardActionInfo> GetSingleCardActionHistory(int id, string begin_date, string end_date);

        [OperationContract]
        void GetTicketsList(string type, string begin_date, string end_date);

        [OperationContract]
        void GetTicketsDetail(int id, string begin_date, string end_date);
        
        [OperationContract]
        List<DeviceAlarm> GeteventsList(int? src, int? level, long? begin_t, long? end_t);

        //[OperationContract]
        //List<DevMonitorNode> GetSomesisList(string strTags);

        [OperationContract]
        List<DevMonitorNode> GetSomesisList(List<string> strTags);

        [OperationContract]
        void GetSomeSisHistoryList(string kks, bool compact);

        [OperationContract]
        void GetSisSamplingHistoryList(string kks);

        [OperationContract]
        List<InspectionTrack> Getinspectionlist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag);

        [OperationContract]
        InspectionTrack GetInspectionTrackById(InspectionTrack trackId);


        [OperationContract]
        List<PatrolPoint> Getcheckpoints(int InspectionId);

        [OperationContract]
        List<PatrolPointItem> Getcheckresults(int patrolId);

        [OperationContract]
        void Trys(InspectionTrackList aa);
    }
}
