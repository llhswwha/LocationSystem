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

namespace WebApiService.Controllers
{
    [RoutePrefix("api/dev")]
    public class DevController : ApiController, IDevService
    {
        protected IDevService service;
        public bool AddArchor(TModel.Location.AreaAndDev.Archor archor)
        {
            return service.AddArchor(archor);
        }

        public TModel.Location.AreaAndDev.Dev_CameraInfo AddCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo cameraInfo)
        {
            return service.AddCameraInfo(cameraInfo);
        }

        public bool AddCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return service.AddCameraInfoByList(cameraInfoList);
        }

        public Location.TModel.Location.AreaAndDev.DevInfo AddDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return service.AddDevInfo(devInfo);
        }

        public List<Location.TModel.Location.AreaAndDev.DevInfo> AddDevInfoByList(List<Location.TModel.Location.AreaAndDev.DevInfo> devInfoList)
        {
            return service.AddDevInfoByList(devInfoList);
        }

        public bool AddDevPosByList(List<DevPos> posList)
        {
            return service.AddDevPosByList(posList);
        }

        public bool AddDevPosInfo(DevPos pos)
        {
            return service.AddDevPosInfo(pos);
        }

        public Location.TModel.Location.AreaAndDev.Dev_DoorAccess AddDoorAccess(Location.TModel.Location.AreaAndDev.Dev_DoorAccess doorAccess)
        {
            return service.AddDoorAccess(doorAccess);
        }

        public bool AddDoorAccessByList(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return service.AddDoorAccessByList(doorAccessList);
        }

        public bool DeleteArchor(int archorId)
        {
            return service.DeleteArchor(archorId);
        }

        public bool DeleteCameraInfo(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return service.DeleteCameraInfo(cameraInfoList);
        }

        public bool DeleteDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return service.DeleteDevInfo(devInfo);
        }

        public bool DeleteDoorAccess(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return service.DeleteDoorAccess(doorAccessList);
        }

        public bool DeletePosInfo(DevPos pos)
        {
            return service.DeletePosInfo(pos);
        }

        public bool EditArchor(TModel.Location.AreaAndDev.Archor Archor, int ParentId)
        {
            return service.EditArchor(Archor,ParentId);
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> FindDevInfos(string key)
        {
            return service.FindDevInfos(key);
        }

        public IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetAllCameraInfo()
        {
            return service.GetAllCameraInfo();
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetAllDevInfos()
        {
            return service.GetAllDevInfos();
        }

        public IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> GetAllDoorAccessInfo()
        {
            return service.GetAllDoorAccessInfo();
        }

        public TModel.Location.AreaAndDev.Archor GetArchor(string id)
        {
            return service.GetArchor(id);
        }

        public TModel.Location.AreaAndDev.Archor GetArchorByDevId(int devId)
        {
            return service.GetArchorByDevId(devId);
        }

        public List<TModel.Location.AreaAndDev.Archor> GetArchors()
        {
            return service.GetArchors();
        }

        public TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByDevInfo(Location.TModel.Location.AreaAndDev.DevInfo dev)
        {
            return service.GetCameraInfoByDevInfo(dev);
        }

        public TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByIp(string ip)
        {
            return service.GetCameraInfoByIp(ip);
        }

        public IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetCameraInfoByParent(int[] pids)
        {
            return service.GetCameraInfoByParent(pids);
        }

        public Location.TModel.Location.AreaAndDev.DevInfo GetDevByGameName(string nameName)
        {
            return service.GetDevByGameName(nameName);
        }

        public Location.TModel.Location.AreaAndDev.DevInfo GetDevByGUID(string devId)
        {
            throw new NotImplementedException();
        }

        public Location.TModel.Location.AreaAndDev.DevInfo GetDevById(int id)
        {
            throw new NotImplementedException();
        }

        public DevModel GetDevClassByDevModel(string devModelName)
        {
            throw new NotImplementedException();
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetDevInfoByParent(int[] pids)
        {
            throw new NotImplementedException();
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetDevInfos(int[] typeList)
        {
            throw new NotImplementedException();
        }

        public Dev_Monitor GetDevMonitorInfoByKKS(string KKS, bool bFlag)
        {
            throw new NotImplementedException();
        }

        public IList<DevPos> GetDevPositions()
        {
            throw new NotImplementedException();
        }

        public IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> GetDoorAccessInfoByParent(int[] pids)
        {
            throw new NotImplementedException();
        }

        public List<EntranceGuardActionInfo> GetEntranceActionInfoByPerson24Hours(int id)
        {
            throw new NotImplementedException();
        }

        public List<NearbyDev> GetNearbyCamera_Alarm(int id, float fDis)
        {
            throw new NotImplementedException();
        }

        public List<NearbyDev> GetNearbyDev_Currency(int id, float fDis, int nFlag)
        {
            throw new NotImplementedException();
        }

        public ObjectAddList GetObjectAddList()
        {
            throw new NotImplementedException();
        }

        public TModel.Location.AreaAndDev.Dev_CameraInfo ModifyCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo camInfo)
        {
            throw new NotImplementedException();
        }

        public bool ModifyCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            throw new NotImplementedException();
        }

        public bool ModifyDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            throw new NotImplementedException();
        }

        public bool ModifyDoorAccess(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            throw new NotImplementedException();
        }

        public bool ModifyPosByList(List<DevPos> posList)
        {
            throw new NotImplementedException();
        }

        public bool ModifyPosInfo(DevPos pos)
        {
            throw new NotImplementedException();
        }
    }
}
