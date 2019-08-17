using System;
using System.Collections.Generic;
using System.Text;
using DbModel.Tools;
using WebApiCommunication.ExtremeVision;
using DbModel.Location.Alarm;
using DbModel.Location.AreaAndDev;
using Newtonsoft.Json;
using LocationServices.Locations.Services;

namespace LocationServices.Locations
{
    //告警定位相关接口
    public partial class LocationService : ILocationService, IDisposable
    {
        /// <summary>
        /// 获取全部告警
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public List<CameraAlarmInfo> GetAllCameraAlarms(bool merge)
        {
            CameraAlarmService service = new CameraAlarmService(db);
            var list = service.GetAllCameraAlarms(merge);
            return list;
        }

        /// <summary>
        /// 获取某一个摄像机的告警
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public List<CameraAlarmInfo> GetCameraAlarms(string ip, bool merge)
        {
            CameraAlarmService service = new CameraAlarmService(db);
            var list = service.GetCameraAlarms(ip,merge);
            return list;
        }



        public CameraAlarmInfo GetCameraAlarm(int id)
        {
            CameraAlarmService service = new CameraAlarmService(db);
            var info = service.GetCameraAlarm(id);
            return info;
        }

        //public Picture GetCameraAlarmPicture(int id)
        //{
        //    CameraAlarmService service = new CameraAlarmService(db);
        //    var info = service.GetCameraAlarmPicture(id);
        //    return info;
        //}

        //public Picture GetCameraAlarmPicture(string picName)
        //{
        //    CameraAlarmService service = new CameraAlarmService(db);
        //    var info = service.GetCameraAlarmPicture(picName);
        //    return info;
        //}

    }
}
