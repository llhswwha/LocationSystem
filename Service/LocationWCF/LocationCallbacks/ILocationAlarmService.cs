using System.Collections.Generic;
using System.ServiceModel;

namespace LocationServices.LocationCallbacks
{
    [ServiceContract(CallbackContract = typeof(ILocationAlarmServiceCallback))]
    //[ServiceContract]
    public interface ILocationAlarmService
    {
        /// <summary>
        /// 添加回调连接
        /// </summary>
        [OperationContract]
        void Connect();

        /// <summary>
        /// 断开回调连接
        /// </summary>
        [OperationContract]
        void DisConnect();

        /// <summary>
        /// Rsetful端上报告警
        /// </summary>
        [OperationContract]
        void AddAlarmInfo(string AlarmInfo);
        
        
    }
}
