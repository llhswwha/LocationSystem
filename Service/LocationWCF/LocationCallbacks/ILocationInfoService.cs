using System.Collections.Generic;
using System.ServiceModel;

namespace LocationServices.LocationCallbacks
{
    [ServiceContract(CallbackContract = typeof(ILocationInfoServiceCallback))]
    public interface ILocationInfoService
    {
        /// <summary>
        /// 添加回调连接
        /// </summary>
        [OperationContract]
        void InfoConnect();

        /// <summary>
        /// 断开回调连接
        /// </summary>
        [OperationContract]
        void DisInfoConnect();

        /// <summary>
        /// 获取数据
        /// </summary>
        [OperationContract]
        List<string> GetInfo(int i);
    }
}
