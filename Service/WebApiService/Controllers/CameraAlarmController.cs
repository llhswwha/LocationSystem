using System.Collections.Generic;
using System.Web.Http;
using WebApiCommunication.ExtremeVision;
using LocationServices.Locations.Services;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/cameraAlarm")]
    public class CameraAlarmController : ApiController, ICameraAlarmService
    {
        protected ICameraAlarmService service;
        public CameraAlarmController()
        {
            service = new CameraAlarmService();
        }

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
        public bool GetBool()
        {
            return service.GetBool();
        }
    }
}
