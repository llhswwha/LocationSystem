using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.FuncArgs;
using Location.TModel.Location.Alarm;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IAlarmService
    {
        [OperationContract]
        List<LocationAlarm> GetLocationAlarms(AlarmSearchArg arg);

        [OperationContract]
        List<DeviceAlarm> GetDeviceAlarms(AlarmSearchArg arg);
    }
}
