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

namespace WebApiService.Controllers
{
    [RoutePrefix("api/alarm")]
    public  class AlarmController: ApiController, IAlarmService
    {
        protected IAlarmService service;

        public DevAlarm Delete(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备告警列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Route("devList")]
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
            throw new NotImplementedException();
        }

        public IList<DevAlarm> GetListByName(string name)
        {
            throw new NotImplementedException();
        }

        [Route("list")]
        public List<Location.TModel.Location.Alarm.LocationAlarm> GetLocationAlarms(AlarmSearchArg arg)
        {
            return service.GetLocationAlarms(arg);
        }

        public DevAlarm Post(DevAlarm item)
        {
            throw new NotImplementedException();
        }

        public DevAlarm Put(DevAlarm item)
        {
            throw new NotImplementedException();
        }
    }
}
