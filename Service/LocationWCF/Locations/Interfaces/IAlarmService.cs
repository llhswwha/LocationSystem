using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.FuncArgs;
using Location.TModel.Location.Alarm;
using TModel.BaseData;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IAlarmService
    {
        /// <summary>
        /// 获取定位告警列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        List<LocationAlarm> GetLocationAlarms(AlarmSearchArg arg);

        /// <summary>
        /// 获取设备告警列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        List<DeviceAlarm> GetDeviceAlarms(AlarmSearchArg arg);

        [OperationContract]
        Page<DeviceAlarm> GetDeviceAlarmsPage(AlarmSearchArg arg);

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
