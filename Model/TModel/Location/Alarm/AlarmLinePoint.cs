using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Alarm
{
    public class AlarmLinePoint
    {
        public string Key { get; set; }

        public double Value { get; set; }

        public AlarmLinePoint(string key, double value)
        {
            this.Key = key;
            this.Value = value;
        }

        public AlarmLinePoint()
        {

        }
    }
}
