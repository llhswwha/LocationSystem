using System.Collections.Generic;
using Location.Model;

namespace LocationServices.LocationAlarms
{
    public class AlarmSubscriber
    {
        public User User { get; set; }

        public List<int> AlarmTypeList { get; set; }

        public ILocationAlarmServiceCallback ClientCallback { get; set; }
    }
}
