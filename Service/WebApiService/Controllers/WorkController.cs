using LocationServices.Locations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TModel.Location.Work;
using TModel.LocationHistory.Work;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/work")]
    public class WorkController : ApiController, IWorkService
    {
        protected IWorkService service;
        public InspectionTrack GetInspectionTrack()
        {
            return service.GetInspectionTrack();
        }

        public List<MobileInspectionDev> GetMobileInspectionDevList()
        {
            return service.GetMobileInspectionDevList();
        }

        public List<MobileInspection> GetMobileInspectionList()
        {
            return service.GetMobileInspectionList();
        }

        public List<OperationTicketHistory> GetOperationTicketHistoryList()
        {
            throw new NotImplementedException();
        }

        public List<OperationTicket> GetOperationTicketList()
        {
            throw new NotImplementedException();
        }

        public List<PersonnelMobileInspectionHistory> GetPersonnelMobileInspectionHistoryList()
        {
            throw new NotImplementedException();
        }

        public List<PersonnelMobileInspection> GetPersonnelMobileInspectionList()
        {
            throw new NotImplementedException();
        }

        public List<WorkTicketHistory> GetWorkTicketHistoryList()
        {
            throw new NotImplementedException();
        }

        public List<WorkTicket> GetWorkTicketList()
        {
            throw new NotImplementedException();
        }
    }
}
