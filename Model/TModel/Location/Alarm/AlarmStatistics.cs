using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Alarm
{
    /// <summary>
    /// 设备或者人员告警统计信息
    /// </summary>
    public class AlarmStatistics
    {
        public List<AlarmGroupCount> DevTypeAlarms{ get; set; }

        public List<AlarmLine> Lines { get; set; }

        public AlarmStatistics()
        {
            DevTypeAlarms = new List<AlarmGroupCount>();
            Lines = new List<AlarmLine>();
        }

        public AlarmGroupCount AddTypeCount(string type,int count)
        {
            if (DevTypeAlarms == null)
            {
                DevTypeAlarms = new List<AlarmGroupCount>();
            }
            AlarmGroupCount typeGroupCount = new AlarmGroupCount(type,count);
            DevTypeAlarms.Add(typeGroupCount);
            return typeGroupCount;
        }

        public void AddLine(string name, int count, List<AlarmLinePoint> points)
        {
            AlarmLine line = new AlarmLine(name);
            line.Points = points;
            this.AddTypeCount(name, count);
        }

        public void AddLine(string name, List<AlarmLinePoint> points,bool addTypeCount)
        {
            double count = 0;
            foreach (var point in points)
            {
                count += point.Value;
            }
            AlarmLine line = new AlarmLine(name);
            line.Points = points;
            this.Lines.Add(line);
            if (addTypeCount)//设备告警，每个类型有一条线
            {
                this.AddTypeCount(name, (int)count);
            }
            else//历史告警，所有人只有一条线
            {

            }
        }

        public void Sort()
        {
            if (DevTypeAlarms != null)
            {
                DevTypeAlarms.Sort();
            }
        }
    }

    

    
}
