
using LocationServices.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Authorizations;
using Location.TModel.FuncArgs;
using Location.TModel.Location;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using Location.TModel.Location.Person;
using Location.TModel.LocationHistory.Data;
using LocationServices.Locations.Plugins;
using TModel.BaseData;
using TModel.Location.AreaAndDev;
using TModel.Location.Manage;
using TModel.Location.Nodes;
using TModel.Location.Person;
using TModel.Location.Work;
using TModel.LocationHistory.AreaAndDev;
using TModel.LocationHistory.Work;
using TModel.Models.Settings;
using WebApiCommunication.ExtremeVision;

namespace WebApiService.Controllers
{
    [RoutePrefix("api")]
    public class LocationController : ApiController
    {
        protected ILocationService service=new LocationService();

        [Route("unitySetting")]
        public UnitySetting GetUnitySetting()
        {
            return service.GetUnitySetting();
        }

        [Route("ObjectAddList")]
        public ObjectAddList GetObjectAddList()
        {
            return service.GetObjectAddList();
        }
    }
}
