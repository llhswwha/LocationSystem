using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.LocationHistory.Data;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/posHistory")]
    public class PosHistoryController : ApiController, IPosHistoryService
    {
        protected IPosHistoryService service;

        public PosHistoryController()
        {
            service = new PosHistoryService();
        }

        [Route("list/pos")]
        public IList<Position> GetHistoryList(string start, string end, string tag, string person, string area)
        {
            return service.GetHistoryList(start,end,tag,person,area);
        }
        [Route("list/posList/{nFlag}/{strName}/{strName2}/{strName3}")]
        public IList<PositionList> GetHistoryPositonStatistics(int nFlag, string strName, string strName2, string strName3)
        {
            if (strName == "NULL")
            {
                strName = "";
            }
            if (strName2 == "NULL")
            {
                strName2 = "";
            }
            if (strName3 == "NULL")
            {
                strName3 = "";
            }
            return service.GetHistoryPositonStatistics(nFlag,strName,strName2,strName3);
        }
    }
}
