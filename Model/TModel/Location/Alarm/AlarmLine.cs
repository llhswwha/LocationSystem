using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Alarm
{
    public class AlarmLine
    {
        public AlarmLine()
        {

        }

        public AlarmLine(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public List<AlarmLinePoint> Points { get; set; }
    }
}
