using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model.InitInfos;
using TModel.Tools;

namespace BLL
{
    public static class LocationContext
    {
        public static float OffsetX = 0;
        public static float OffsetY = 0;

        public static void LoadOffset(string off)
        {
            if (string.IsNullOrEmpty(off)) return;
            string[] parts = off.Split(',');
            if (parts.Length == 2)
            {
                OffsetX = parts[0].ToFloat();
                OffsetY = parts[1].ToFloat();
            }
        }

        public static float InitOffsetX = 0;
        public static float InitOffsetY = 0;
        public static float Power = 1;

        public static void LoadInitOffset(string off)
        {
            if (string.IsNullOrEmpty(off)) return;
            string[] parts = off.Split(',');
            if (parts.Length == 2)
            {
                InitOffsetX = parts[0].ToFloat();
                InitOffsetY = parts[1].ToFloat();
            }
        }

        public static void Transform(PointInfo point)
        {
            if(point==null)return;
            point.X = (point.X + InitOffsetX) * Power;
            point.Y = (point.Y + InitOffsetY) * Power;
        }
    }
}
