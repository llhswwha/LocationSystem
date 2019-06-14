using Location.TModel.FuncArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebApiCommunication.ExtremeVision;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface ICameraAlarmService
    {
        /// <summary>
        /// 获取定位告警列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        List<CameraAlarmInfo> GetCameraAlarms(AlarmSearchArg arg);
    }
}
