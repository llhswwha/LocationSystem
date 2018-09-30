using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.Location.Alarm;

namespace   LocationServices.LocationCallbacks
{
    /// <summary>   
    /// 报警回调
    /// </summary>
    public interface ILocationAlarmServiceCallback
    {
        /// <summary>
        /// 告警
        /// </summary>
        /// <param name="localAlarms"></param>

        [OperationContract(IsOneWay = true)]
        void AlarmInfo(List<LocationAlarm> localAlarms);
        
    }
}
