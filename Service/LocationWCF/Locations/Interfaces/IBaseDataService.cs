using System;
using System.Collections.Generic;
using System.ServiceModel;
using TModel.BaseData;
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
        Ticket GetTicketDetial(int id);

        [OperationContract]
        List<InspectionTrackHistory> Getinspectionhistorylist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag);
    }
}
