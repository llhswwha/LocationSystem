using System.Collections.Generic;
using System.ServiceModel;
using Location.Model;

namespace   LocationServices.LocationAlarms
{
    /// <summary>   
    /// 报警回调
    /// </summary>
    public interface ILocationAlarmServiceCallback
    {
        /**
 * msgItems 接收到的报警事件集合
 */
        [OperationContract(IsOneWay = true)]
        void OnMessageReceived(List<LocationAlarm> msgItems);
    }
}
