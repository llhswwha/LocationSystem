using LocationServices.Locations.Interfaces;
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

namespace WebApiService.Controllers
{
    [RoutePrefix("api/dev")]
    public class DevController : ApiController, IDevService
    {
        protected IDevService service;

        [Route("")]
        public bool AddArchor(TModel.Location.AreaAndDev.Archor archor)
        {
            return service.AddArchor(archor);
        }
        [Route("")]
        public TModel.Location.AreaAndDev.Dev_CameraInfo AddCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo cameraInfo)
        {
            return service.AddCameraInfo(cameraInfo);
        }
        [Route("")]
        public bool AddCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return service.AddCameraInfoByList(cameraInfoList);
        }
        [Route("")]
        public Location.TModel.Location.AreaAndDev.DevInfo AddDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return service.AddDevInfo(devInfo);
        }
        [Route("")]
        public List<Location.TModel.Location.AreaAndDev.DevInfo> AddDevInfoByList(List<Location.TModel.Location.AreaAndDev.DevInfo> devInfoList)
        {
            return service.AddDevInfoByList(devInfoList);
        }
        [Route("")]
        public bool AddDevPosByList(List<DevPos> posList)
        {
            return service.AddDevPosByList(posList);
        }
        [Route("")]
        public bool AddDevPosInfo(DevPos pos)
        {
            return service.AddDevPosInfo(pos);
        }
        [Route("")]
        public Location.TModel.Location.AreaAndDev.Dev_DoorAccess AddDoorAccess(Location.TModel.Location.AreaAndDev.Dev_DoorAccess doorAccess)
        {
            return service.AddDoorAccess(doorAccess);
        }
        [Route("")]
        public bool AddDoorAccessByList(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return service.AddDoorAccessByList(doorAccessList);
        }
        [Route("")]
        public bool DeleteArchor(int archorId)
        {
            return service.DeleteArchor(archorId);
        }
        [Route("")]
        public bool DeleteCameraInfo(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return service.DeleteCameraInfo(cameraInfoList);
        }
        [Route("")]
        public bool DeleteDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return service.DeleteDevInfo(devInfo);
        }
        [Route("")]
        public bool DeleteDoorAccess(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return service.DeleteDoorAccess(doorAccessList);
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
        [Route("")]
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
        [Route("")]
        public TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByDevInfo(Location.TModel.Location.AreaAndDev.DevInfo dev)
        {
            return service.GetCameraInfoByDevInfo(dev);
        }
        [Route("")]
        public TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByIp(string ip)
        {
            return service.GetCameraInfoByIp(ip);
        }
        [Route("")]
        public IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetCameraInfoByParent(int[] pids)
        {
            return service.GetCameraInfoByParent(pids);
        }

        public AlarmStatistics GetDevAlarmStatistics(SearchArg arg)
        {
            throw new NotImplementedException();
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
        [Route("")]
        public Dev_Monitor GetDevMonitorInfoByKKS(string KKS, bool bFlag)
        {
            return service.GetDevMonitorInfoByKKS(KKS,bFlag);
        }
        [Route("list")]
        public IList<DevPos> GetDevPositions()
        {
            return service.GetDevPositions();
        }
        [Route("")]
        public IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> GetDoorAccessInfoByParent(int[] pids)
        {
            return service.GetDoorAccessInfoByParent(pids);
        }
        [Route("")]
        public List<EntranceGuardActionInfo> GetEntranceActionInfoByPerson24Hours(int id)
        {
            return service.GetEntranceActionInfoByPerson24Hours(id);
        }

        public AlarmStatistics GetLocationAlarmStatistics(SearchArg arg)
        {
            throw new NotImplementedException();
        }

        [Route("")]
        public List<NearbyDev> GetNearbyCamera_Alarm(int id, float fDis)
        {
            return service.GetNearbyCamera_Alarm(id,fDis);
        }
        [Route("")]
        public List<NearbyDev> GetNearbyDev_Currency(int id, float fDis, int nFlag)
        {
            return service.GetNearbyDev_Currency(id,fDis,nFlag);
        }
        [Route("")]
        public ObjectAddList GetObjectAddList()
        {
            return service.GetObjectAddList();
        }
        [Route("")]
        public TModel.Location.AreaAndDev.Dev_CameraInfo ModifyCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo camInfo)
        {
            return service.ModifyCameraInfo(camInfo);
        }
        [Route("")]
        public bool ModifyCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return service.ModifyCameraInfoByList(cameraInfoList);
        }
        [Route("")]
        public bool ModifyDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return service.ModifyDevInfo(devInfo);
        }
        [Route("")]
        public bool ModifyDoorAccess(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return service.ModifyDoorAccess(doorAccessList);
        }
        [Route("")]
        public bool ModifyPosByList(List<DevPos> posList)
        {
            return service.ModifyPosByList(posList);
        }
        [Route("")]
        public bool ModifyPosInfo(DevPos pos)
        {
            return service.ModifyPosInfo(pos);
        }
    }
}
