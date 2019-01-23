using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Work;
using TModel.LocationHistory.Work;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IWorkService
    {
        [OperationContract]
        List<OperationTicket> GetOperationTicketList();

        [OperationContract]
        List<WorkTicket> GetWorkTicketList();

        [OperationContract]
        List<MobileInspectionDev> GetMobileInspectionDevList();

        [OperationContract]
        List<MobileInspection> GetMobileInspectionList();

        [OperationContract]
        List<PersonnelMobileInspection> GetPersonnelMobileInspectionList();

        [OperationContract]
        List<OperationTicketHistory> GetOperationTicketHistoryList();

        [OperationContract]
        List<WorkTicketHistory> GetWorkTicketHistoryList();

        [OperationContract]
        List<PersonnelMobileInspectionHistory> GetPersonnelMobileInspectionHistoryList();

        [OperationContract]
        InspectionTrack GetInspectionTrack();

    }
}
