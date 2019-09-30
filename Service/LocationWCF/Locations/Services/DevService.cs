using System;
using System.Collections.Generic;
using System.Linq;
using DbModel.Location.AreaAndDev;
using Location.TModel.Location.AreaAndDev;
using TModel.Location.AreaAndDev;
using TModel.LocationHistory.AreaAndDev;
using BLL;
using BLL.Blls.Location;
using LocationServices.Converters;
using IModel.Enums;
using DbModel.Tools;
using TModel.Tools;
using System.IO;
using Location.BLL.Tool;
using TModel.Location.Alarm;
using TModel.FuncArgs;
using TEntity = Location.TModel.Location.AreaAndDev.DevInfo;
using System.ServiceModel.Web;
using DevInfo = Location.TModel.Location.AreaAndDev.DevInfo;
using Dev_DoorAccess = Location.TModel.Location.AreaAndDev.Dev_DoorAccess;

namespace LocationServices.Locations.Services
{
    public interface IDevService : ILeafEntityService<TEntity, TEntity>
    {
        /// <summary>
        /// 获取模型类型数量
        /// </summary>
        /// <returns></returns>
        ObjectAddList GetObjectAddList();

        /// <summary>
        /// 获取所有设备的位置信息
        /// </summary>
        /// <returns></returns>
        IList<DevPos> GetDevPositions();
        /// <summary>
        /// 添加一条设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        bool AddDevPosInfo(DevPos pos);
        /// <summary>
        /// 添加设备位置信息（列表形式）
        /// </summary>
        /// <param name="posList"></param>
        bool AddDevPosByList(List<DevPos> posList);
        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        bool ModifyPosInfo(DevPos pos);
        /// <summary>
        /// 修改设备位置信息，列表方式
        /// </summary>
        /// <param name="posList"></param>
        /// <returns></returns>
        bool ModifyPosByList(List<DevPos> posList);
        /// <summary>
        /// 删除设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        bool DeletePosInfo(DevPos pos);
        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        IList<DevInfo> GetAllDevInfos();
        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        IList<DevInfo> GetDevInfos(int[] typeList);
        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        IList<DevInfo> FindDevInfos(string key);
        /// <summary>
        /// 添加一条设备基本信息
        /// </summary>
        /// <param name="devInfo"></param>
        DevInfo AddDevInfo(DevInfo devInfo);
        /// <summary>
        /// 添加设备基本信息（列表形式）
        /// </summary>
        /// <param name="devInfoList"></param>
        List<DevInfo> AddDevInfoByList(List<DevInfo> devInfoList);
        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        bool ModifyDevInfo(DevInfo devInfo);
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        bool DeleteDevInfo(DevInfo devInfo);
        /// <summary>
        /// 根据ID，获取区域ID下所有设备
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        IList<DevInfo> GetDevInfoByParent(int[] pids);

        /// <summary>
        /// 通过设备ID,获取设备信息(字符串Id,GUID那部分)
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        DevInfo GetDevByGUID(string devId);

        /// <summary>
        /// 通过设备ID,获取设备信息(数字Id,主键)
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        DevInfo GetDevById(int id);

        /// <summary>
        /// 通过设备物体名称获取信息
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        DevInfo GetDevByGameName(string nameName);

        //门禁设备的增删改查

        /// <summary>
        /// 添加门禁
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        bool AddDoorAccessByList(IList<Dev_DoorAccess> doorAccessList);
        /// <summary>
        /// 添加门禁信息
        /// </summary>
        /// <param name="doorAccess"></param>
        /// <returns></returns>
        Dev_DoorAccess AddDoorAccess(Dev_DoorAccess doorAccess);
        /// <summary>
        /// 删除门禁
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        bool DeleteDoorAccess(IList<Dev_DoorAccess> doorAccessList);
        /// <summary>
        /// 修改门禁信息
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        bool ModifyDoorAccess(IList<Dev_DoorAccess> doorAccessList);
        /// <summary>
        /// 通过区域ID，获取所有门禁信息
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        IList<Dev_DoorAccess> GetDoorAccessInfoByParent(int[] pids);
        /// <summary>
        /// 获取所有的门禁信息
        /// </summary>
        /// <returns></returns>
        IList<Dev_DoorAccess> GetAllDoorAccessInfo();

        //摄像头设备的增删改查

        /// <summary>
        /// 添加摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        bool AddCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList);
        /// <summary>
        /// 添加摄像头信息
        /// </summary>
        /// <param name="cameraInfo"></param>
        /// <returns></returns>
        TModel.Location.AreaAndDev.Dev_CameraInfo AddCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo cameraInfo);
        /// <summary>
        /// 删除摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        bool DeleteCameraInfo(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList);
        /// <summary>
        /// 修改摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        bool ModifyCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList);
        /// <summary>
        /// 修改摄像头信息
        /// </summary>
        /// <param name="camInfo"></param>
        /// <returns></returns>
        TModel.Location.AreaAndDev.Dev_CameraInfo ModifyCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo camInfo);
        /// <summary>
        /// 通过区域ID，获取所有摄像头信息
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetCameraInfoByParent(int[] pids);
        /// <summary>
        /// 获取所有的摄像头信息
        /// </summary>
        /// <returns></returns>
        IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetAllCameraInfo();

        /// <summary>
        /// 通过设备信息，获取摄像头信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByDevInfo(DevInfo dev);

        /// <summary>
        /// 通过设备信息，获取摄像头信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByIp(string ip);


        [WebGet(UriTemplate = "/archor", ResponseFormat = WebMessageFormat.Json)]
        List<TModel.Location.AreaAndDev.Archor> GetArchors();

        [WebGet(UriTemplate = "/archor/{id}", ResponseFormat = WebMessageFormat.Json)]
        TModel.Location.AreaAndDev.Archor GetArchor(string id);

        /// <summary>
        /// 通过设备Id,获取Archor信息
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        TModel.Location.AreaAndDev.Archor GetArchorByDevId(int devId);

        bool EditArchor(TModel.Location.AreaAndDev.Archor Archor, int ParentId);

        /// <summary>
        /// 添加基站信息，内含设备信息
        /// </summary>
        /// <param name="archor"></param>
        /// <returns></returns>
        bool AddArchor(TModel.Location.AreaAndDev.Archor archor);
        bool DeleteArchor(int archorId);

        /// <summary>
        /// 附近设备（通用）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<NearbyDev> GetNearbyDev_Currency(int id, float fDis, int nFlag);

        /// <summary>
        /// 附近摄像头（告警）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<NearbyDev> GetNearbyCamera_Alarm(int id, float fDis);

        /// <summary>
        /// 获取人员24小时内经过的门禁
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<EntranceGuardActionInfo> GetEntranceActionInfoByPerson24Hours(int id);

        /// <summary>
        /// 根据模型名称，获取模型类型
        /// </summary>
        /// <param name="devModelName"></param>
        /// <returns></returns>
        DbModel.Location.AreaAndDev.DevModel GetDevClassByDevModel(string devModelName);
        Dev_Monitor GetDevMonitorInfoByKKS(string KKS, bool bFlag);

        AlarmStatistics GetDevAlarmStatistics(SearchArg arg);

        AlarmStatistics GetLocationAlarmStatistics(SearchArg arg);
    }

    public class DevService : IDevService
    {

        private Bll db;
        private Dev_CameraInfoBll CameraInfoSet;
        private ArchorBll ArchorSet;
        public DevService()
        {
            db = Bll.NewBllNoRelation();
            CameraInfoSet = db.Dev_CameraInfos;
            ArchorSet = db.Archors;
        }

        public DevService(Bll bll)
        {
            this.db = bll;
            CameraInfoSet = db.Dev_CameraInfos;
            ArchorSet = db.Archors;
        }

        public bool AddArchor(TModel.Location.AreaAndDev.Archor archor)
        {
            if (archor.DevInfo != null)
            {
                archor.ParentId = archor.ParentId;
            }
            return ArchorSet.Add(archor.ToDbModel());
        }

        public TModel.Location.AreaAndDev.Dev_CameraInfo AddCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo cameraInfo)
        {
            DbModel.Location.AreaAndDev.Dev_CameraInfo dbCamera = cameraInfo.ToDbModel();
            var result = CameraInfoSet.Add(dbCamera);
            return result ? dbCamera.ToTModel() : null;
           
        }

        public bool AddCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return CameraInfoSet.AddRange(cameraInfoList.ToList().ToDbModel());
        }

        public Location.TModel.Location.AreaAndDev.DevInfo AddDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return new DeviceService(db).Post(devInfo);
        }

        public List<Location.TModel.Location.AreaAndDev.DevInfo> AddDevInfoByList(List<Location.TModel.Location.AreaAndDev.DevInfo> devInfoList)
        {
            return new DeviceService(db).PostRange(devInfoList);
        }

        public bool AddDevPosByList(List<DevPos> posList)
        {
            return ModifyPosByList(posList);
        }

        public bool AddDevPosInfo(DevPos pos)
        {
            return ModifyPosInfo(pos);
        }

        public Location.TModel.Location.AreaAndDev.Dev_DoorAccess AddDoorAccess(Location.TModel.Location.AreaAndDev.Dev_DoorAccess doorAccess)
        {
            DbModel.Location.AreaAndDev.Dev_DoorAccess access = doorAccess.ToDbModel();
            bool value = db.Dev_DoorAccess.Add(access);
            return value == true ? access.ToTModel() : null;
        }

        public bool AddDoorAccessByList(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return db.Dev_DoorAccess.AddRange(doorAccessList.ToList().ToDbModel());
        }

        public bool DeleteArchor(int archorId)
        {
            bool value = true;
            var archor = db.Archors.DeleteByDev(archorId);
            var dev = db.DevInfos.DeleteById(archorId);
            bool valueTemp = archor != null && dev != null;
            if (!valueTemp) value = valueTemp;
            return value;
        }

        public bool DeleteCameraInfo(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            bool value = true;
            foreach (TModel.Location.AreaAndDev.Dev_CameraInfo item in cameraInfoList)
            {
                var cameraInfo = db.Dev_CameraInfos.DeleteById(item.Id);
                var dev = db.DevInfos.DeleteById(item.DevInfoId);
                //bool posResult = db.DevPos.DeleteById(item.DevID);
                bool valueTemp = cameraInfo != null && dev != null;
                if (!valueTemp) value = valueTemp;
            }
            return value;
        }

        public bool DeleteDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return new DeviceService(db).Delete(devInfo.Id + "") != null;
        }

        public bool DeleteDoorAccess(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            bool value = true;
            foreach (Location.TModel.Location.AreaAndDev.Dev_DoorAccess item in doorAccessList)
            {
                var doorAccess = db.Dev_DoorAccess.DeleteById(item.Id);
                var dev = db.DevInfos.DeleteById(item.DevInfoId);
                //bool posResult = db.DevPos.DeleteById(item.DevID);
                bool valueTemp = doorAccess != null && dev != null;
                if (!valueTemp) value = valueTemp;
            }
            return value;
        }

        public bool DeletePosInfo(DevPos pos)
        {
            //return db.DevPos.DeleteById(pos.DevID);
            return false;
        }

        public bool EditArchor(TModel.Location.AreaAndDev.Archor Archor, int ParentId)
        {
            bool bReturn = false;
            DbModel.Location.AreaAndDev.Archor Archor2;
            Archor2 = db.Archors.FirstOrDefault(p => p.Code == Archor.Code);
            if (Archor2 == null)
            {
                Archor2 = db.Archors.FirstOrDefault(p => p.DevInfoId == Archor.DevInfoId);
            }
            if (Archor2 == null)
            {
                LocationService service = new LocationService();
                DbModel.Location.AreaAndDev.Area area = service.GetAreaById(ParentId);
                Archor2 = Archor.ToDbModel();

                DbModel.Location.AreaAndDev.DevInfo dev = new DbModel.Location.AreaAndDev.DevInfo();
                dev.Local_DevID = Guid.NewGuid().ToString();
                dev.IP = "";
                dev.KKS = "";
                dev.Name = Archor2.Name;
                if (area != null)
                {
                    dev.ModelName = area.Name == DepNames.FactoryName ? TypeNames.ArchorOutdoor : TypeNames.Archor;//室外基站||室内基站
                }
                else
                {
                    dev.ModelName = TypeNames.Archor;
                }
                dev.Status = 0;
                dev.ParentId = ParentId;
                dev.Local_TypeCode = TypeCodes.Archor;
                dev.UserName = "admin";
                Archor2.DevInfo = dev;
                Archor2.ParentId = ParentId;

                bReturn = db.Archors.Add(Archor2);
            }
            else
            {
                Archor2.Name = Archor.Name;
                Archor2.X = Archor.X;
                Archor2.Y = Archor.Y;
                Archor2.Z = Archor.Z;
                Archor2.Type = Archor.Type;
                Archor2.IsAutoIp = Archor.IsAutoIp;
                Archor2.Ip = Archor.Ip;
                Archor2.ServerIp = Archor.ServerIp;
                Archor2.ServerPort = Archor.ServerPort;
                Archor2.Power = Archor.Power;
                Archor2.AliveTime = Archor.AliveTime;
                Archor2.Enable = Archor.Enable;
                if (!string.IsNullOrEmpty(Archor.Code)) Archor2.Code = Archor.Code;
                bReturn = db.Archors.Edit(Archor2);
            }
            EditBusAnchor(Archor, ParentId);
            return bReturn;
        }

        public bool EditBusAnchor(TModel.Location.AreaAndDev.Archor archor, int ParentId)
        {
            bool bDeal = false;

            try
            {
                int nFlag = 0;
                var bac = db.bus_anchors.FirstOrDefault(p => p.anchor_id == archor.Code);
                if (bac == null)
                {
                    bac = new DbModel.Engine.bus_anchor();
                    nFlag = 1;
                }

                bac.anchor_id = archor.Code;
                bac.anchor_x = (int)(archor.X * 100);
                bac.anchor_y = (int)(archor.Z * 100);
                bac.anchor_z = (int)(archor.Y * 100);
                bac.anchor_type = (int)archor.Type;
                bac.anchor_bno = 0;
                bac.syn_anchor_id = null;
                bac.offset = 0;
                bac.enabled = 1;

                if (nFlag == 0)
                {
                    bDeal = db.bus_anchors.Edit(bac);
                }
                else
                {
                    bDeal = db.bus_anchors.Add(bac);
                }

                //if (!bDeal)
                //{
                //    return bDeal;
                //}

                //bDeal = EditArchor(Archor, ParentId);
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            return bDeal;
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> FindDevInfos(string key)
        {
            return new DeviceService(db).GetListByName(key);
        }

        public IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetAllCameraInfo()
        {
            List<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList = db.Dev_CameraInfos.DbSet.ToList().ToTModel();
            return cameraInfoList.ToWCFList();
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetAllDevInfos()
        {
            var service = new DeviceService(db);
            var devList = service.GetList();
            return devList;
        }

        public IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> GetAllDoorAccessInfo()
        {
            List<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList = db.Dev_DoorAccess.DbSet.ToList().ToTModel();
            return doorAccessList.ToWCFList();
        }

        public TModel.Location.AreaAndDev.Archor GetArchor(string id)
        {
            int id2 = id.ToInt();
            return db.Archors.Find(id2).ToTModel();
        }

        public TModel.Location.AreaAndDev.Archor GetArchorByDevId(int devId)
        {
            return db.Archors.FirstOrDefault(i => i.DevInfoId == devId).ToTModel();
        }

        public List<TModel.Location.AreaAndDev.Archor> GetArchors()
        {
            return db.Archors.ToList().ToWcfModelList();
        }

        public TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByDevInfo(Location.TModel.Location.AreaAndDev.DevInfo dev)
        {
            if (dev == null) return null;
            TModel.Location.AreaAndDev.Dev_CameraInfo cameraInfo = db.Dev_CameraInfos.DbSet.FirstOrDefault(item => item.DevInfoId == dev.Id).ToTModel();
            return cameraInfo;
        }

        public TModel.Location.AreaAndDev.Dev_CameraInfo GetCameraInfoByIp(string ip)
        {
            TModel.Location.AreaAndDev.Dev_CameraInfo cameraInfo = db.Dev_CameraInfos.DbSet.FirstOrDefault(item => !(string.IsNullOrEmpty(item.Ip)) && item.Ip == ip).ToTModel();
            return cameraInfo;
        }

        public IList<TModel.Location.AreaAndDev.Dev_CameraInfo> GetCameraInfoByParent(int[] pids)
        {
            List<TModel.Location.AreaAndDev.Dev_CameraInfo> devInfoList = new List<TModel.Location.AreaAndDev.Dev_CameraInfo>();
            foreach (var pId in pids)
            {
                devInfoList.AddRange(db.Dev_CameraInfos.DbSet.Where(item => item.ParentId == pId).ToList().ToTModel());
            }
            return devInfoList.ToWCFList();
        }

        public Location.TModel.Location.AreaAndDev.DevInfo GetDevByGameName(string nameName)
        {
            return new DeviceService(db).GetDevByGameName(nameName);
        }

        public Location.TModel.Location.AreaAndDev.DevInfo GetDevByGUID(string devId)
        {
            return new DeviceService(db).GetEntityByDevId(devId);
        }

        public Location.TModel.Location.AreaAndDev.DevInfo GetDevById(int id)
        {
            return new DeviceService(db).GetEntityById(id);
        }

        public DevModel GetDevClassByDevModel(string devModelName)
        {
            DevModel devModel = db.DevModels.FirstOrDefault(dev => dev.Name == devModelName);
            return devModel;
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetDevInfoByParent(int[] pids)
        {
            return new DeviceService(db).GetListByPids(pids);
        }

        public IList<Location.TModel.Location.AreaAndDev.DevInfo> GetDevInfos(int[] typeList)
        {
            var service = new DeviceService(db);
            var devList = service.GetListByTypes(typeList);
            return devList;
        }

        public Dev_Monitor GetDevMonitorInfoByKKS(string KKS, bool bFlag)
        {
              DateTime start = DateTime.Now;
            Dev_Monitor monitor = GetDevMonitor(KKS, bFlag);
            if (monitor == null) return null;
            var tags = monitor.GetAllTagList();
            TimeSpan time = DateTime.Now - start;
            //Log.Info(LogTags.KKS, string.Format("[{2}]KKS:{0},测点:{1}", KKS, tags, time));
            BaseDataService baseservice = new BaseDataService();
            List<TModel.Location.AreaAndDev.DevMonitorNode> dataList = baseservice.GetSomesisList(tags);//到基础平台获取数据
            //Log.Info(LogTags.KKS, string.Format("获取sis数据"));
            monitor = InsertDataToEveryDev(monitor, dataList);
            return monitor;
        }

        public IList<DevPos> GetDevPositions()
        {
            List<DevPos> posList = new List<DevPos>();
            var devs = db.DevInfos.ToList();
            foreach (DbModel.Location.AreaAndDev.DevInfo dev in devs)
            {
                posList.Add(dev.GetPos());
            }
            return posList.ToWCFList();
        }

        public IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> GetDoorAccessInfoByParent(int[] pids)
        {
            try
            {
                List<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> devInfoList = new List<Location.TModel.Location.AreaAndDev.Dev_DoorAccess>();
                foreach (var pId in pids)
                {
                    devInfoList.AddRange(db.Dev_DoorAccess.DbSet.Where(item => item.ParentId != null && item.ParentId == pId).ToList().ToTModel());
                }
                return devInfoList.ToWCFList();
            }
            catch (Exception e)
            {
                Log.Error("LocationService_Dev.GetDoorAccessInfoByParent.Exception:" + e.ToString());
                return null;
            }
        }

        public List<EntranceGuardActionInfo> GetEntranceActionInfoByPerson24Hours(int id)
        {
            DeviceService ds = new DeviceService();
            List<EntranceGuardActionInfo> lst = ds.GetEntranceActionInfoByPerson24Hours(id);
            if (lst == null)
            {
                lst = new List<EntranceGuardActionInfo>();
            }

            if (lst.Count == 0)
            {
                lst = null;
            }

            return lst;
        }

        public List<NearbyDev> GetNearbyCamera_Alarm(int id, float fDis)
        {
            DeviceService ds = new DeviceService();
            List<NearbyDev> lst = ds.GetNearbyCamera_Alarm(id, fDis);
            if (lst == null)
            {
                lst = new List<NearbyDev>();
            }

            return lst;
        }

        public List<NearbyDev> GetNearbyDev_Currency(int id, float fDis, int nFlag)
        {
            DeviceService ds = new DeviceService();
            List<NearbyDev> lst = ds.GetNearbyDev_Currency(id, fDis, nFlag);
            //if (lst == null)
            //{
            //    lst = new List<NearbyDev>();
            //}

            if (lst != null && lst.Count == 0)
            {
                lst = null;
            }

            return lst;
        }

        public ObjectAddList GetObjectAddList()
        {
            throw new NotImplementedException();
        }

        public TModel.Location.AreaAndDev.Dev_CameraInfo ModifyCameraInfo(TModel.Location.AreaAndDev.Dev_CameraInfo camInfo)
        {
            camInfo.AutoGenerateRtsp();
            var dbModel = camInfo.ToDbModel();
            if (db.Dev_CameraInfos.Edit(dbModel))
            {
                return camInfo;
            }
            else
            {
                return null;
            }
        }

        public bool ModifyCameraInfoByList(IList<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraInfoList)
        {
            return db.Dev_CameraInfos.EditRange(db.Db, cameraInfoList.ToList().ToDbModel());
        }

        public bool ModifyDevInfo(Location.TModel.Location.AreaAndDev.DevInfo devInfo)
        {
            return new DeviceService(db).Put(devInfo) != null;
        }

        public bool ModifyDoorAccess(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorAccessList)
        {
            return db.Dev_DoorAccess.EditRange(db.Db, doorAccessList.ToList().ToDbModel());
        }

        public bool ModifyPosByList(List<DevPos> posList)
        {
            //return db.DevPos.EditRange(db.Db, posList);
            foreach (var devPose in posList)
            {
                ModifyPosInfo(devPose);
            }
            return true;
        }

        public bool ModifyPosInfo(DevPos pos)
        {
            var devInfo = db.DevInfos.DbSet.FirstOrDefault(i => i.Local_DevID == pos.DevID);
            if (devInfo != null)
            {
                devInfo.SetPos(pos);
                return db.DevInfos.Edit(devInfo);
            }
            return false;
        }


        static private List<DbModel.Location.AreaAndDev.KKSCode> _kksCodes;
        public List<DbModel.Location.AreaAndDev.KKSCode> KKSCodes
        {
            get
            {
                if (_kksCodes == null)
                {
                    _kksCodes = db.KKSCodes.ToListEx();
                }
                return _kksCodes;
            }
        }

        public Dev_Monitor GetDevMonitor(string KKS, bool isShowAll)
        {
            Dev_Monitor monitor = new Dev_Monitor();
            DbModel.Location.AreaAndDev.KKSCode kksCode = KKSCodes.FirstOrDefault(p => p.Code == KKS);
            if (kksCode == null)
            {
                string dirPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\DeviceData\\" + KKS + ".xml";
                if (File.Exists(dirPath))
                {
                    var list = XmlSerializeHelper.LoadFromFile<TModel.Location.AreaAndDev.DevMonitorNodeList>(dirPath);
                    monitor.Name = KKS;
                    monitor.KKSCode = KKS;
                    monitor.MonitorNodeList = list;
                }
                else
                {
                    monitor = null;
                }
            }
            else
            {
                monitor.Name = kksCode.Name;
                monitor.KKSCode = kksCode.Code;
                var monitorNodeList = GetDevMonitorNodeListByKKS(kksCode.Code);
                monitor.MonitorNodeList = monitorNodeList;
                monitor.ChildrenList = GetChildMonitorDev(kksCode);
                if (isShowAll == false)
                {
                    monitor.RemoveEmpty();
                }
                monitor.AddChildrenMonitorNodes();
            }

            if (monitor != null)
            {
                monitor.SetEmpty();
            }

            return monitor;
        }
         private List<TModel.Location.AreaAndDev.DevMonitorNode> GetDevMonitorNodeListByKKS(List<string> KKS)
        {
            List<DbModel.Location.AreaAndDev.DevMonitorNode> lst = DevMonitorNodes.FindAll(p => KKS.Contains(p.ParentKKS));
            if (lst == null || lst.Count == 0)
            {
                return null;
            }
            else
            {
                return lst.ToTModel();
            }
        }

        private List<TModel.Location.AreaAndDev.DevMonitorNode> GetDevMonitorNodeListByKKS(string KKS)
        {
            List<DbModel.Location.AreaAndDev.DevMonitorNode> lst = DevMonitorNodes.FindAll(p => p.ParentKKS == KKS);
            if (lst == null || lst.Count == 0)
            {
                return null;
            }
            else
            {
                return lst.ToTModel();
            }
        }

        static List<DbModel.Location.AreaAndDev.DevMonitorNode> _devMonitorNodes;
        List<DbModel.Location.AreaAndDev.DevMonitorNode> DevMonitorNodes
        {
            get
            {
                if (_devMonitorNodes == null)
                {
                    _devMonitorNodes = db.DevMonitorNodes.ToList();
                }
                return _devMonitorNodes;
            }
        }

        private List<Dev_Monitor> GetChildMonitorDev(DbModel.Location.AreaAndDev.KKSCode KKS)
        {
            Dictionary<string, Dev_Monitor> devMoniters = new Dictionary<string, Dev_Monitor>();
            List<DbModel.Location.AreaAndDev.KKSCode> childrenKKS = KKS.Children;
            if (childrenKKS != null)
            {
                List<string> kksList = new List<string>();
                foreach (DbModel.Location.AreaAndDev.KKSCode item in childrenKKS)
                {
                    kksList.Add(item.Code);//获取所有的子kks
                    //Dev_Monitor dev = new Dev_Monitor();
                    //dev.Name = item.Name;
                    //dev.KKSCode = item.Code;
                    //dev.MonitorNodeList = GetDevMonitorNodeListByKKS(dev.KKSCode);
                    //dev.ChildrenList = GetChildMonitorDev(item, bFlag);
                    //child.Add(dev);
                }

                var monitorList = GetDevMonitorNodeListByKKS(kksList);//从数据库获取

                if (monitorList != null)
                {
                    foreach (DbModel.Location.AreaAndDev.KKSCode item in childrenKKS)
                    {
                        kksList.Add(item.Code);
                        Dev_Monitor dev = new Dev_Monitor();
                        dev.Name = item.Name;
                        dev.KKSCode = item.Code;
                        dev.MonitorNodeList = monitorList.Where(i => i.ParentKKS == item.Code).ToList();//放到相应的对象中
                        //dev.ChildrenList = GetChildMonitorDev(item, bFlag);
                        devMoniters.Add(item.Code, dev);
                    }

                    foreach (DbModel.Location.AreaAndDev.KKSCode item in childrenKKS)
                    {
                        Dev_Monitor dev = devMoniters[item.Code];
                        dev.ChildrenList = GetChildMonitorDev(item);//递归找子物体
                    }
                }
            }

            if (devMoniters.Count == 0)
            {
                return null;
            }
            return devMoniters.Values.ToList();
        }


        private Dev_Monitor InsertDataToEveryDev(Dev_Monitor Dm, List<TModel.Location.AreaAndDev.DevMonitorNode> dataList)
        {
            Dev_Monitor send = new Dev_Monitor();
            string strDevKKs = Dm.KKSCode;
            List<TModel.Location.AreaAndDev.DevMonitorNode> MonitorNodeList = dataList.FindAll(p => p.ParentKKS == strDevKKs);
            if (Dm.MonitorNodeList != null)
            {
                foreach (TModel.Location.AreaAndDev.DevMonitorNode item in Dm.MonitorNodeList)
                {
                    //string strNodeKKS = item.KKS;
                    TModel.Location.AreaAndDev.DevMonitorNode data = MonitorNodeList.Find(p => p.KKS == item.KKS);
                    //DevMonitorNode data = dataList.Find(p => p.TagName == item.TagName);
                    if (data == null || MonitorNodeList.Count == 0)
                    {
                        data = dataList.Find(p => p.TagName == item.TagName);
                    }
                    else
                    {

                    }

                    if (data != null)
                    {
                        item.Value = data.Value;
                        item.Time = data.Time;
                    }
                }
            }

            if (Dm.ChildrenList != null && Dm.ChildrenList.Count > 0)
            {
                foreach (Dev_Monitor item2 in Dm.ChildrenList)
                {
                    Dev_Monitor ChildDm = InsertDataToEveryDev(item2, dataList);
                    if (ChildDm != null)
                    {
                        if (send.ChildrenList == null)
                        {
                            send.ChildrenList = new List<Dev_Monitor>();
                        }

                        send.ChildrenList.Add(ChildDm);
                    }
                }
            }

            send.KKSCode = Dm.KKSCode;
            send.Name = Dm.Name;
            send.MonitorNodeList = Dm.MonitorNodeList;

            return send;
        }

        public AlarmStatistics GetDevAlarmStatistics(SearchArg arg)
        {
            AlarmStatistics alarmStatistics = new AlarmStatistics();
            return alarmStatistics;
        }

        public AlarmStatistics GetLocationAlarmStatistics(SearchArg arg)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> GetListByPid(string pid)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> DeleteListByPid(string pid)
        {
            throw new NotImplementedException();
        }

        public TEntity GetParent(string id)
        {
            throw new NotImplementedException();
        }

        public TEntity Post(string pid, TEntity item)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> GetListByName(string name)
        {
            throw new NotImplementedException();
        }

        public TEntity Delete(string id)
        {
            throw new NotImplementedException();
        }

        public TEntity GetEntity(string id)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public TEntity Post(TEntity item)
        {
            throw new NotImplementedException();
        }

        public TEntity Put(TEntity item)
        {
            throw new NotImplementedException();
        }
    }
}
