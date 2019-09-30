using LocationServices.Locations.Services;
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
        public WorkController()
        {
            service = new WorkService();
        }
        [Route("")]
        public InspectionTrack GetInspectionTrack()
        {
            return service.GetInspectionTrack();
        }
        [Route("list/MobileInspectionDev")]
        public List<MobileInspectionDev> GetMobileInspectionDevList()
        {
            return service.GetMobileInspectionDevList();
        }
        [Route("")]
        public List<MobileInspection> GetMobileInspectionList()
        {
            return service.GetMobileInspectionList();
        }
        [Route("list/operation")]
        public List<OperationTicketHistory> GetOperationTicketHistoryList()
        {
            return service.GetOperationTicketHistoryList();
        }
        [Route("list/OperationTicket")]
        public List<OperationTicket> GetOperationTicketList()
        {
            return service.GetOperationTicketList();
        }
        [Route("")]
        public List<PersonnelMobileInspectionHistory> GetPersonnelMobileInspectionHistoryList()
        {
            return GetPersonnelMobileInspectionHistoryList();
        }
        [Route("")]
        public List<PersonnelMobileInspection> GetPersonnelMobileInspectionList()
        {
            return GetPersonnelMobileInspectionList();
        }
        [Route("list/workTicket")]
        public List<WorkTicketHistory> GetWorkTicketHistoryList()
        {
            return service.GetWorkTicketHistoryList();
        }
        [Route("list/WorkTicket")]
        public List<WorkTicket> GetWorkTicketList()
        {
            return service.GetWorkTicketList();
        }
    }
}
