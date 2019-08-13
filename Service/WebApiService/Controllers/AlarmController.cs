using LocationServices.Locations.Interfaces;
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

namespace WebApiService.Controllers
{
    [RoutePrefix("api/alarm")]
    public  class AlarmController: ApiController, IAlarmService
    {
        protected IAlarmService service;

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

        [Route("list")]
        public List<LocationAlarm> GetLocationAlarms(AlarmSearchArg arg)
        {
            return service.GetLocationAlarms(arg);
        }
    }
}
