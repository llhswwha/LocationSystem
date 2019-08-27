using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Alarm
{
    public class AlarmGroupCount : IComparable<AlarmGroupCount>
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public AlarmGroupCount()
        {

        }

        public AlarmGroupCount(string n, int c)
        {
            this.Name = n;
            this.Count = c;
        }

        public int CompareTo(AlarmGroupCount other)
        {
            return other.Count.CompareTo(this.Count);
        }
    }
}
