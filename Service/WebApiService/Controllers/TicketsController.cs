using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using TModel.Location.Work;
using TModel.Tools;
using TModel.LocationHistory.Work;
using TModel.FuncArgs;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/ticket")]
    public class TicketsController : ApiController, ITicketsService
    {
        protected ITicketsService service;
        public TicketsController()
        {
            service = new TicketsService();
        }

        public TwoTickets Delete(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取某一个票
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("twoTickets")]
        [HttpGet]
        public TwoTickets GetEntity(string id)
        {
            return service.GetEntity(id);
        }
        [Route("twoTicketsHis")]
        [HttpGet]
        public TwoTickets GetHisEntity(string id)
        {
            return service.GetHisEntity(id);
        }
        [Route("list/TwoTickets/sortfield/{field}/count/{count}")]
        public List<TwoTickets> GetHistoryListByTime(int count, string field, string sort = "desc")
        {
           return service.GetHistoryListByTime(count,field);
        }

        [Route("pageHis")]
        [HttpPost]
        public PageInfo<TwoTickets> GetHistoryPage(TModel.FuncArgs.TicketSearchArgs args)
        {
            return service.GetHistoryPage(args);
        }

        [Route("list")]
        [HttpGet]
        public List<TwoTickets> GetList()
        {
            return service.GetList();
        }
        [Route("list/value/{value}/startTime/{startTime}/endTime/{endTime}")]
        public List<TwoTickets> GetListByCondition(string value, DateTime startTime, DateTime endTime)
        {
            return service.GetListByCondition(value, startTime, endTime);
        }

        public IList<TwoTickets> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route("WorkTicketHistorySH")]
        public WorkTicketHistorySH GetWorkTicketHisSHById(string id)
       {
            return service.GetWorkTicketHisSHById(id);
        }
        [Route("page/WorkTicketHisSH")]
        [HttpPost]
        public PageInfo<WorkTicketHistorySH> GetWorkTicketHisSHPage(TicketSearchArgs args)
        {
            return service.GetWorkTicketHisSHPage(args);
        }
        [Route("list/WorkTicketHistorySH/sortfield/{field}/count/{count}")]
        public List<WorkTicketHistorySH> GetWorkTickHisListByTime(int count, string field, string sort = "desc")
        {
            return service.GetWorkTickHisListByTime(count,field,sort);
        }

        public TwoTickets Post(TwoTickets item)
        {
            throw new NotImplementedException();
        }

        public TwoTickets Put(TwoTickets item)
        {
            throw new NotImplementedException();
        }
    }
}
