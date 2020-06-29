using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.FuncArgs;
using Location.TModel.Location.Alarm;
using TModel.BaseData;
using TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;
using DbModel.Location.Alarm;
using TModel.FuncArgs;
using TModel.Location.Alarm;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/alarm")]
    public  class AlarmController: ApiController, IAlarmService
    {
        protected IAlarmService service;

        public AlarmController()
        {
            service = new AlarmService();
        }

        public DevAlarm Delete(string id)
        {
            return service.Delete(id);
        }

        /// <summary>
        /// 设备告警列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Route("devList")]
        [HttpPost]
        public DeviceAlarmInformation GetDeviceAlarms(AlarmSearchArg arg)
        {
            return service.GetDeviceAlarms(arg);
        }
        [Route("devPage")]
        public Page<DeviceAlarm> GetDeviceAlarmsPage(AlarmSearchArg arg)
        {
            return service.GetDeviceAlarmsPage(arg);
        }



        public DevAlarm GetEntity(string id)
        {
            throw new NotImplementedException();
        }

        public List<DevAlarm> GetList()
        {
            return service.GetList();
        }

        public IList<DevAlarm> GetListByName(string name)
        {
            throw new NotImplementedException();
        }

        [Route("delete/LocationAlarm/{id}")]
        public bool DeleteSpecifiedLocationAlarm(int id)
        {
            return service.DeleteSpecifiedLocationAlarm(id);
        }
        [Route("delete/LocationAlarmList")]
        public bool DeleteLocationAlarm(List<int> idList)
        {
            return service.DeleteLocationAlarm(idList);
        }

        [Route("list")]
        [HttpPost]
        public List<Location.TModel.Location.Alarm.LocationAlarm> GetLocationAlarms(AlarmSearchArg arg)
        {
            return service.GetLocationAlarms(arg);
        }

        [Route("locationList")]
        [HttpPost]
        public LocationAlarmInformation GetLocationAlarmByArgs(AlarmSearchArg arg)
        {
            return service.GetLocationAlarmByArgs(arg);
        }


        public DevAlarm Post(DevAlarm item)
        {
            return service.Post(item);
        }

        public DevAlarm Put(DevAlarm item)
        {
            return service.Put(item);
        }
        [Route("AllAlarms")]
        [HttpPost]
        public AllAlarms GetAllAlarmsByPerson(AlarmSearchArgAll args)
        {
            return service.GetAllAlarmsByPerson(args);
        }
    }
}
