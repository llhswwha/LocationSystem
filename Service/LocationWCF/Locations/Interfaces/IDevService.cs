using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Location.TModel.FuncArgs;
using Location.TModel.Location.AreaAndDev;
using DevInfo = Location.TModel.Location.AreaAndDev.DevInfo;
using Dev_DoorAccess = Location.TModel.Location.AreaAndDev.Dev_DoorAccess;
using TModel.Location.AreaAndDev;
using TModel.LocationHistory.AreaAndDev;
using TModel.Location.Alarm;
using TModel.FuncArgs;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IDevService
    {
        #region 设备位置(DevPos)和信息(DevInfo),门禁、摄像头数据的增删改查
        /// <summary>
        /// 获取模型类型数量
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ObjectAddList GetObjectAddList();

        /// <summary>
        /// 获取所有设备的位置信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<DevPos> GetDevPositions();
        /// <summary>
        /// 添加一条设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        [OperationContract]
        bool AddDevPosInfo(DevPos pos);
        /// <summary>
        /// 添加设备位置信息（列表形式）
        /// </summary>
        /// <param name="posList"></param>
        [OperationContract]
        bool AddDevPosByList(List<DevPos> posList);
        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [OperationContract]
        bool ModifyPosInfo(DevPos pos);
        /// <summary>
        /// 修改设备位置信息，列表方式
        /// </summary>
        /// <param name="posList"></param>
        /// <returns></returns>
        [OperationContract]
        bool ModifyPosByList(List<DevPos> posList);
        /// <summary>
        /// 删除设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeletePosInfo(DevPos pos);
        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<DevInfo> GetAllDevInfos();
        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<DevInfo> GetDevInfos(int[] typeList);
        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<DevInfo> FindDevInfos(string key);
        /// <summary>
        /// 添加一条设备基本信息
        /// </summary>
        /// <param name="devInfo"></param>
        [OperationContract]
        DevInfo AddDevInfo(DevInfo devInfo);
        /// <summary>
        /// 添加设备基本信息（列表形式）
        /// </summary>
        /// <param name="devInfoList"></param>
        [OperationContract]
        List<DevInfo> AddDevInfoByList(List<DevInfo> devInfoList);
        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        [OperationContract]
        bool ModifyDevInfo(DevInfo devInfo);
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteDevInfo(DevInfo devInfo);
        /// <summary>
        /// 根据ID，获取区域ID下所有设备
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        [OperationContract]
        IList<DevInfo> GetDevInfoByParent(int[] pids);

        /// <summary>
        /// 通过设备ID,获取设备信息(字符串Id,GUID那部分)
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        [OperationContract]
        DevInfo GetDevByGUID(string devId);

        /// <summary>
        /// 通过设备ID,获取设备信息(数字Id,主键)
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        [OperationContract]
        DevInfo GetDevById(int id);

        /// <summary>
        /// 通过设备物体名称获取信息
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        [OperationContract]
        DevInfo GetDevByGameName(string nameName);

        //门禁设备的增删改查

        /// <summary>
        /// 添加门禁
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddDoorAccessByList(IList<Dev_DoorAccess> doorAccessList);
        /// <summary>
        /// 添加门禁信息
        /// </summary>
        /// <param name="doorAccess"></param>
        /// <returns></returns>
        [OperationContract]
        Dev_DoorAccess AddDoorAccess(Dev_DoorAccess doorAccess);
        /// <summary>
        /// 删除门禁
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteDoorAccess(IList<Dev_DoorAccess> doorAccessList);
        /// <summary>
        /// 修改门禁信息
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        [OperationContract]
        bool ModifyDoorAccess(IList<Dev_DoorAccess> doorAccessList);
        /// <summary>
        /// 通过区域ID，获取所有门禁信息
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Dev_DoorAccess> GetDoorAccessInfoByParent(int[] pids);
        /// <summary>
        /// 获取所有的门禁信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Dev_DoorAccess> GetAllDoorAccessInfo();

        //摄像头设备的增删改查

        /// <summary>
        /// 添加摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddCameraInfoByList(IList<Dev_CameraInfo> cameraInfoList);
        /// <summary>
        /// 添加摄像头信息
        /// </summary>
        /// <param name="cameraInfo"></param>
        /// <returns></returns>
        [OperationContract]
        Dev_CameraInfo AddCameraInfo(Dev_CameraInfo cameraInfo);
        /// <summary>
        /// 删除摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCameraInfo(IList<Dev_CameraInfo> cameraInfoList);
        /// <summary>
        /// 修改摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        [OperationContract]
        bool ModifyCameraInfoByList(IList<Dev_CameraInfo> cameraInfoList);
        /// <summary>
        /// 修改摄像头信息
        /// </summary>
        /// <param name="camInfo"></param>
        /// <returns></returns>
        [OperationContract]
        Dev_CameraInfo ModifyCameraInfo(Dev_CameraInfo camInfo);
        /// <summary>
        /// 通过区域ID，获取所有摄像头信息
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Dev_CameraInfo> GetCameraInfoByParent(int[] pids);
        /// <summary>
        /// 获取所有的摄像头信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Dev_CameraInfo> GetAllCameraInfo();

        /// <summary>
        /// 通过设备信息，获取摄像头信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        [OperationContract]
        Dev_CameraInfo GetCameraInfoByDevInfo(DevInfo dev);

        /// <summary>
        /// 通过设备信息，获取摄像头信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        [OperationContract]
        Dev_CameraInfo GetCameraInfoByIp(string ip);
        #endregion

        [OperationContract]
        [WebGet(UriTemplate = "/archor", ResponseFormat = WebMessageFormat.Json)]
        List<Archor> GetArchors();

        [OperationContract]
        [WebGet(UriTemplate = "/archor/{id}", ResponseFormat = WebMessageFormat.Json)]
        Archor GetArchor(string id);

        /// <summary>
        /// 通过设备Id,获取Archor信息
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        [OperationContract]
        Archor GetArchorByDevId(int devId);

        [OperationContract]
        bool EditArchor(Archor Archor, int ParentId);

        /// <summary>
        /// 添加基站信息，内含设备信息
        /// </summary>
        /// <param name="archor"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddArchor(Archor archor);
        [OperationContract]
        bool DeleteArchor(int archorId);

        /// <summary>
        /// 附近设备（通用）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        List<NearbyDev> GetNearbyDev_Currency(int id, float fDis, int nFlag);

        /// <summary>
        /// 附近摄像头（告警）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        List<NearbyDev> GetNearbyCamera_Alarm(int id, float fDis);

        /// <summary>
        /// 获取人员24小时内经过的门禁
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntranceGuardActionInfo> GetEntranceActionInfoByPerson24Hours(int id);

        /// <summary>
        /// 根据模型名称，获取模型类型
        /// </summary>
        /// <param name="devModelName"></param>
        /// <returns></returns>
        [OperationContract]
        DbModel.Location.AreaAndDev.DevModel GetDevClassByDevModel(string devModelName);

        [OperationContract]
        Dev_Monitor GetDevMonitorInfoByKKS(string KKS, bool bFlag);

        [OperationContract]
        AlarmStatistics GetDevAlarmStatistics(SearchArg arg);

        [OperationContract]
        AlarmStatistics GetLocationAlarmStatistics(SearchArg arg);
    }
}
