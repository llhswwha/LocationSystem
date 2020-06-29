using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DbModel.LocationHistory.Door;
using LocationServices.Locations.Services;
using LocationServices.ObjToData;
using TModel.FuncArgs;
using TModel.LocationHistory.AreaAndDev;
using TModel.Tools;

namespace WebApiService.Controllers
{
    /// <summary>
    /// 门禁接口（包括嘉明、四会）
    /// </summary>
    [RoutePrefix("api/door")]  
    public class DoorClickController : ApiController, IDoorClickService
    {
        protected IDoorClickService service;

        public DoorClickController()
        {
            service = new DoorClickService();
        }
        [Route("SH/page")]
        [HttpPost]
        public PageInfo<DevEntranceGuardCardsHistroy> GetCardPageByCondition(DoorSearchArgsSH args)
        {
            return service.GetCardPageByCondition(args);
        }

        [Route("doorClickList/begin/{dtBeginTime}/end/{dtEndTime}/doorIndexCode/{doorIndexCode}")]
        public List<DoorClick> GetListByCondition(DateTime dtBeginTime, DateTime dtEndTime, string doorIndexCode)
        {
            ////string[] doorIndexCodes = doorIndexCodesSTR.Split('$');
            //return service.GetListByCondition(dtBeginTime, dtEndTime, "", "", "", null, doorIndexCodes);
            return service.GetListByCondition(dtBeginTime, dtEndTime, doorIndexCode);
        }
        [Route("Page")]
        [HttpPost]
        public DataDoor GetPageByCondition(DoorSearchArgs args)
        {
            return service.GetPageByCondition(args);
        }
    }
}
