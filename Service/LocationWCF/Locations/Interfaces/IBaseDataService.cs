using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Person;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using TModel.BaseData;
using TModel.Location.AreaAndDev;
using TModel.Location.Work;
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

        [OperationContract]
        List<Personnel> GetUserList();

        [OperationContract]
        List<Department> GetorgList();

        [OperationContract]
        List<PhysicalTopology> GetzonesList();

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
        void GetCardList();

        [OperationContract]
        void GetSingleCardActionHistory(int id, string begin_date, string end_date);

        [OperationContract]
        void GetTicketsList(string type, string begin_date, string end_date);

        [OperationContract]
        void GetTicketsDetail(int id, string begin_date, string end_date);
        
        [OperationContract]
        void GeteventsList(int? src, int? level, long? begin_t, long? end_t);

        [OperationContract]
        List<DevMonitorNode> GetSomesisList(string strTags);

        [OperationContract]
        void GetSomeSisHistoryList(string kks, bool compact);

        [OperationContract]
        void GetSisSamplingHistoryList(string kks);

        [OperationContract]
        void Getinspectionlist(long lBegin, long lEnd, bool bFlag);

        [OperationContract]
        void Getcheckpoints(int patrolId);

        [OperationContract]
        void Getcheckresults(int patrolId, string deviceId);
    }
}
