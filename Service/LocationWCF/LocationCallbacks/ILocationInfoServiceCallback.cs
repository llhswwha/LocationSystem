using System;
using System.Collections.Generic;
using System.ServiceModel;
using Location.Model;


namespace LocationServices.LocationCallbacks
{
    public interface ILocationInfoServiceCallback
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="GetInfo"></param>
        /// i 表示获取不同类型的数据
        /// nFlag 用来判断是哪个客户端的连接

        [OperationContract]
        List<string> GetInfoFromRsetful(int i);

    }
}
