using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LocationServices.Locations.Services;
using TEntity = DbModel.Location.Alarm.LocationAlarm;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/locationAlarms")]
    public class LocationAlarmController : ApiController, ILocationAlarmService
    {
        ILocationAlarmService service;
        public LocationAlarmController()
        {
            service = new LocationAlarmService();
        }

        [Route("{id}")]
        public TEntity Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("")]//area/?id=1
        [Route("{id}")]
        public TEntity GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("list")]
        public List<TEntity> GetList()
        {
            return service.GetList();
        }

        [Route]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        [Route]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }

        [Route("clear")]
        public void Clear()
        {
            service.Clear();
        }
    }
}
