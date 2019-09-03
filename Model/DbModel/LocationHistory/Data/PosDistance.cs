using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Data
{
    public class PosDistance<T> : IComparable<PosDistance<T>> where T : IPosInfo
    {
        public T p1;
        public T p2;

        public double distance;

        public long time;

        public double speed;
        public PosDistance(T p1, T p2)
        {
            this.p1 = p1;
            this.p2 = p2;

            distance = Math.Pow((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) + (p1.Z - p2.Z) * (p1.Z - p2.Z), 0.5);
            time = p2.DateTimeStamp - p1.DateTimeStamp;
            speed = distance / time * 1000;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", time, distance, speed);
        }

        public int CompareTo(PosDistance<T> other)
        {
            return other.speed.CompareTo(this.speed);
        }

        
    }
}
