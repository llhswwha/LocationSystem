using System.Collections.Generic;
using Location.Model;
using Location.TModel.Location.Obsolete;

namespace LocationServices.LocationCallbacks
{
    public class AlarmSubscriber
    {
        public User User { get; set; }

        public List<int> AlarmTypeList { get; set; }

        //public ILocationAlarmServiceCallback ClientCallback { get; set; }
    }
}
