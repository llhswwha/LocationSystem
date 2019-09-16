using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LocationServices.Locations.Interfaces;
using WebApiCommunication.ExtremeVision;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/cameraAlarm")]
    public class CameraAlarmController : ApiController, ICameraAlarmService
    {
        protected ICameraAlarmService service;

        [Route("list/merge/{merge}")]
        public List<CameraAlarmInfo> GetAllCameraAlarms(bool merge)
        {
            return service.GetAllCameraAlarms(merge);
        }
        [Route("detail/{id}")]
        public CameraAlarmInfo GetCameraAlarm(int id)
        {
            return service.GetCameraAlarm(id);
        }
        [Route("list/ipAndmerge/{ip}/{merge}")]
        public List<CameraAlarmInfo> GetCameraAlarms(string ip, bool merge)
        {
            return service.GetCameraAlarms(ip,merge);
        }
    }
}
