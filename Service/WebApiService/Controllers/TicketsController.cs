using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CommunicationClass.SihuiThermalPowerPlant.Models;

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
        public TwoTickets GetEntity(string id)
        {
            return service.GetEntity(id);
        }
        [Route("list")]
        public List<TwoTickets> GetList()
        {
            return service.GetList();
        }

        public IList<TwoTickets> GetListByName(string name)
        {
            throw new NotImplementedException();
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
