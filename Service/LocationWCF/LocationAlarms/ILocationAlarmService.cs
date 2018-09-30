using System.Collections.Generic;
using System.ServiceModel;

namespace LocationServices.LocationAlarms
{
    [ServiceContract(SessionMode = SessionMode.Required,
        CallbackContract = typeof (ILocationAlarmServiceCallback))]
    public interface ILocationAlarmService
    {
        /**
  * 订阅
  * UserId 注册用户ID
  * Alarms 要订阅的报警类型ID
  * 注意IsOneWay = true，避免回调时发生死锁
  */

        [OperationContract(IsOneWay = true)]
        void Subscribe(int userId, List<int> alarmTypes);

        //注销
        [OperationContract(IsOneWay = true)]
        void Unsubscribe(int userId);
    }
}
