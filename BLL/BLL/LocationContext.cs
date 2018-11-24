using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
