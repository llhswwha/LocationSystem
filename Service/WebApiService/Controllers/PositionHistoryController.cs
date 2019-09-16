using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LocationServices.Locations.Services;
using Location.TModel.LocationHistory.Data;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/his/pos")]
    public class PositionHistoryController:ApiController,IPosHistoryService
    {
        IPosHistoryService service;
        public PositionHistoryController()
        {
            service = new PosHistoryService();
        }


        [Route("")]
        public IList<Position> GetHistoryList(string start, string end, string tag, string person, string area)
        {
            return service.GetHistoryList(start, end, tag, person, area);
        }

        [Route("")]
        public IList<Position> GetHistoryList()
        {
            return service.GetHistoryList(null, null, null, null,null);
        }

        [Route("")]
        public IList<Position> GetHistoryList(string start, string end)
        {
            return service.GetHistoryList(start, end, null, null, null);
        }

        [Route("")]
        public IList<Position> GetHistoryListStart(string start)
        {
            return service.GetHistoryList(start, null, null, null, null);
        }

        [Route("")]
        public IList<Position> GetHistoryListEnd(string end)
        {
            return service.GetHistoryList(null, end, null, null, null);
        }

        [Route("")]
        public IList<Position> GetHistoryListByTag(string tag)
        {
            return service.GetHistoryList(null, null, tag, null, null);
        }

        [Route("")]
        public IList<Position> GetHistoryListByTag(string tag,string start, string end)
        {
            return service.GetHistoryList(start, end, tag, null, null);
        }

        [Route("")]
        public IList<Position> GetHistoryListByTagStart(string tag, string start)
        {
            return service.GetHistoryList(start, "", tag, null, null);
        }


        [Route("")]
        public IList<Position> GetHistoryListByTagEnd(string tag, string end)
        {
            return service.GetHistoryList(null, end, tag, null, null);
        }



        [Route("")]
        public IList<Position> GetHistoryListByPerson(string person)
        {
            return service.GetHistoryList(null, null, null, person, null);
        }

        [Route("")]
        public IList<Position> GetHistoryListByPerson(string person,string start, string end)
        {
            return service.GetHistoryList(start, end, null, person, null);
        }

        [Route("")]
        public IList<Position> GetHistoryListByPersonStart(string person, string start)
        {
            return service.GetHistoryList(start, null, null, person, null);
        }

        [Route("")]
        public IList<Position> GetHistoryListByPersonEnd(string person, string end)
        {
            return service.GetHistoryList(null, end, null, person, null);
        }

        [Route("statistics")]
        public IList<PositionList> GetHistoryPositonStatistics(int nFlag, string strName, string strName2, string strName3)
        {
            return service.GetHistoryPositonStatistics(nFlag, strName, strName2, strName3);
        }
    }
}
