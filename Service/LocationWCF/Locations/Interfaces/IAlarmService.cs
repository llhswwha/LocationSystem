using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.FuncArgs;
using Location.TModel.Location.Alarm;
using TModel.BaseData;
using TModel.Location.AreaAndDev;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IAlarmService
    {
        /// <summary>
        /// 获取定位告警列表(一次性获取所有)
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        List<LocationAlarm> GetLocationAlarms(AlarmSearchArg arg);
        /// <summary>
        /// 获取定位告警列表(按页获取)
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        LocationAlarmInformation GetLocationAlarmByArgs(AlarmSearchArg arg);
        /// <summary>
        /// 获取设备告警列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        DeviceAlarmInformation GetDeviceAlarms(AlarmSearchArg arg);

        [OperationContract]
        Page<DeviceAlarm> GetDeviceAlarmsPage(AlarmSearchArg arg);

        //根据Id号删除指定定位告警
        [OperationContract]
        bool DeleteSpecifiedLocationAlarm(int id);

        [OperationContract]
        bool DeleteLocationAlarmByIdList(List<int> ids);

        ///// <summary>
        ///// 获取定位告警列表（新增事件）
        ///// </summary>
        ///// <param name="arg"></param>
        ///// <returns></returns>
        //[OperationContract]
        //List<LocationAlarm> GetNewLocationAlarms(string session);

        ///// <summary>
        ///// 获取设备告警列表（新增事件）
        ///// </summary>
        ///// <param name="arg"></param>
        ///// <returns></returns>
        //[OperationContract]
        //List<DeviceAlarm> GetNewDeviceAlarms(string session);
    }
}
