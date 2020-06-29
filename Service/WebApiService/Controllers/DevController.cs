using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DbModel.Location.AreaAndDev;
using Location.TModel.Location.AreaAndDev;
using TModel.Location.AreaAndDev;
using TModel.LocationHistory.AreaAndDev;
using TModel.FuncArgs;
using TModel.Location.Alarm;
using LocationServices.Locations.Services;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/dev")]
    public class DevController : ApiController, IDevService
    {
        protected IDevService service;

        public DevController()
        {
            service = new DevService();
        }

        [Route("add/camera")]
        [HttpPost]
        public TModel.Location.AreaAndDev.Dev_CameraInfo AddCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo cameraInfo)
        {
            return service.AddCameraInfo(cameraInfo);
        }
        [Route("add/cameraList")]
        [HttpPost]
        public bool AddCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return service.AddCameraInfoByList(cameraInfoList);
        }
        [Route("add")]
        public Location.TModel.Location.AreaAndDev.DevInfo AddDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return service.AddDevInfo(devInfo);
        }
        [Route("addList")]
        public List<Location.TModel.Location.AreaAndDev.DevInfo> AddDevInfoByList(List<Location.TModel.Location.AreaAndDev.DevInfo> devInfoList)
        {
            return service.AddDevInfoByList(devInfoList);
        }
        [Route("")]
        public bool AddDevPosByList(List<DevPos> posList)
        {
            return service.AddDevPosByList(posList);
        }
        [Route("add/devPos")]
        public bool AddDevPosInfo(DevPos pos)
        {
            return service.AddDevPosInfo(pos);
        }
        [Route("add/doorAccess")]
        [HttpPost]
        public Location.TModel.Location.AreaAndDev.Dev_DoorAccess AddDoorAccess(Location.TModel.Location.AreaAndDev.Dev_DoorAccess doorAccess)
        {
            return service.AddDoorAccess(doorAccess);
        }
        [Route("add/doorAccessByList")]
        [HttpPost]
        public bool AddDoorAccessByList(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return service.AddDoorAccessByList(doorAccessList);
        }

        public Location.TModel.Location.AreaAndDev.DevInfo Delete(string id)
        {
            throw new NotImplementedException();
        }

        [Route("")]
        public bool DeleteArchor(int archorId)
        {
            return service.DeleteArchor(archorId);
        }
        [Route("del/cameraList")]
        public bool DeleteCameraInfo(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return service.DeleteCameraInfo(cameraInfoList);
        }
        [Route("del")]
        [HttpPost]
        public bool DeleteDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return service.DeleteDevInfo(devInfo);
        }
        [Route("del/doorAccessList")]
        [HttpPost]
        public bool DeleteDoorAccess(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return service.DeleteDoorAccess(doorAccessList);
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> DeleteListByPid(string pid)
        {
            throw new NotImplementedException();
        }

        [Route("")]
        public bool DeletePosInfo(DevPos pos)
        {
            return service.DeletePosInfo(pos);
        }
        [Route("")]
        public bool EditArchor(TModel.Location.AreaAndDev.Archor Archor, int ParentId)
        {
            return service.EditArchor(Archor,ParentId);
        }
        [Route("")]
        public IList<Location.TModel.Location.AreaAndDev.DevInfo> FindDevInfos(string key)
        {
            return service.FindDevInfos(key);
        }
        [Route("list")]
        public IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetAllCameraInfo()
        {
            return service.GetAllCameraInfo();
        }
        [Route("")]
        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetAllDevInfos()
        {
            return service.GetAllDevInfos();
        }
        [Route("")]
        public IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> GetAllDoorAccessInfo()
        {
            return service.GetAllDoorAccessInfo();
        }
        [Route("")]
        public TModel.Location.AreaAndDev.Archor GetArchor(string id)
        {
            return service.GetArchor(id);
        }
        [Route("")]
        public TModel.Location.AreaAndDev.Archor GetArchorByDevId(int devId)
        {
            return service.GetArchorByDevId(devId);
        }
        [Route("")]
        public List<TModel.Location.AreaAndDev.Archor> GetArchors()
        {
            return service.GetArchors();
        }

        [Route("camera/detail/ByDev")]
        [HttpPost]
        public TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByDevInfo(Location.TModel.Location.AreaAndDev.DevInfo dev)
        {
            return service.GetCameraInfoByDevInfo(dev);
        }
        [Route("camera/detail/ByIp/{ip}")]
        public TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByIp(string ip)
        {
            return service.GetCameraInfoByIp(ip);
        }
        [Route("list/Bypids")]
        public IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetCameraInfoByParent(int[] pids)
        {
            return service.GetCameraInfoByParent(pids);
        }

        [Route("")]
        public AlarmStatistics GetDevAlarmStatistics(SearchArg arg)
        {
            return service.GetDevAlarmStatistics(arg);
        }

        [Route("")]
        public Location.TModel.Location.AreaAndDev.DevInfo GetDevByGameName(string nameName)
        {
            return service.GetDevByGameName(nameName);
        }
        [Route("")]
        public Location.TModel.Location.AreaAndDev.DevInfo GetDevByGUID(string devId)
        {
            return service.GetDevByGUID(devId);
        }
        [Route("")]
        public Location.TModel.Location.AreaAndDev.DevInfo GetDevById(int id)
        {
            return service.GetDevById(id);
        }
        [Route("")]
        public DevModel GetDevClassByDevModel(string devModelName)
        {
            return service.GetDevClassByDevModel(devModelName);
        }
        [Route("search/pids/list")]
        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetDevInfoByParent(int[] pids)
        {
            return service.GetDevInfoByParent(pids);
        }
        [Route("")]
        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetDevInfos(int[] typeList)
        {
            return service.GetDevInfos(typeList);
        }
        [Route("devMonitor/kks/{KKS}/bFlag/{bFlag}")]
        [HttpGet]
        public Dev_Monitor GetDevMonitorInfoByKKS(string KKS, bool bFlag)
        {
            return service.GetDevMonitorInfoByKKS(KKS,bFlag);
        }
        [Route("list")]
        public IList<DevPos> GetDevPositions()
        {
            return service.GetDevPositions();
        }

        [Route("list/doorAccessByParent")]
        [HttpPost]
        public IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> GetDoorAccessInfoByParent(int[] pids)
        {
            return service.GetDoorAccessInfoByParent(pids);
        }
        [Route("list/doorAccessByDoorName")]
        public IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> GetDoorAccessByDoorName(string doorName)
        {
            return service.GetDoorAccessByDoorName(doorName);
        }

        public Location.TModel.Location.AreaAndDev.DevInfo GetEntity(string id)
        {
            throw new NotImplementedException();
        }

        [Route("list/entrance/id/{id}")]
        public List<EntranceGuardActionInfo> GetEntranceActionInfoByPerson24Hours(int id)
        {
            return service.GetEntranceActionInfoByPerson24Hours(id);
        }

        public List<Location.TModel.Location.AreaAndDev.DevInfo> GetList()
        {
            throw new NotImplementedException();
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetListByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Location.TModel.Location.AreaAndDev.DevInfo> GetListByPid(string pid)
        {
            throw new NotImplementedException();
        }

        public AlarmStatistics GetLocationAlarmStatistics(SearchArg arg)
        {
            return service.GetLocationAlarmStatistics(arg);
        }

        [Route("")]
        public List<NearbyDev> GetNearbyCamera_Alarm(int id, float fDis)
        {
            return service.GetNearbyCamera_Alarm(id,fDis);
        }
        [Route("list/nearDev/search/id/{id}/fDis/{fDis}/nFlag/{nFlag}")]
        public List<NearbyDev> GetNearbyDev_Currency(int id, float fDis, int nFlag)
        {
            return service.GetNearbyDev_Currency(id,fDis,nFlag);
        }

        /// <summary>
        /// 实时数据
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [Route("devMonitor/tags/{tags}")]
        public Dev_Monitor getNowDevMonitorInfoByTags(string tags)
        {
            return service.getNowDevMonitorInfoByTags(tags);
        }

        [Route("")]
        public ObjectAddList GetObjectAddList()
        {
            return service.GetObjectAddList();
        }

        public Location.TModel.Location.AreaAndDev.DevInfo GetParent(string id)
        {
            throw new NotImplementedException();
        }

        [Route("edit/camera")]
        [HttpPut]
        public TModel.Location.AreaAndDev.Dev_CameraInfo ModifyCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo camInfo)
        {
            return service.ModifyCameraInfo(camInfo);
        }
        [Route("")]
        public bool ModifyCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return service.ModifyCameraInfoByList(cameraInfoList);
        }
        [Route("Edit")]
        [HttpPut]
        public bool ModifyDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return service.ModifyDevInfo(devInfo);
        }
        [Route("edit/doorAccessList")]
        [HttpPut]
        public bool ModifyDoorAccess(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return service.ModifyDoorAccess(doorAccessList);
        }
        [Route("Edit/posList")]
        [HttpPut]
        public bool ModifyPosByList(List<DevPos> posList)
        {
            return service.ModifyPosByList(posList);
        }
        [Route("Edit/devPos")]
        [HttpPut]
        public bool ModifyPosInfo(DevPos pos)
        {
            return service.ModifyPosInfo(pos);
        }

        public Location.TModel.Location.AreaAndDev.DevInfo Post(Location.TModel.Location.AreaAndDev.DevInfo item)
        {
            throw new NotImplementedException();
        }

        public Location.TModel.Location.AreaAndDev.DevInfo Post(string pid, Location.TModel.Location.AreaAndDev.DevInfo item)
        {
            throw new NotImplementedException();
        }

        public Location.TModel.Location.AreaAndDev.DevInfo Put(Location.TModel.Location.AreaAndDev.DevInfo item)
        {
            throw new NotImplementedException();
        }
    }
}
