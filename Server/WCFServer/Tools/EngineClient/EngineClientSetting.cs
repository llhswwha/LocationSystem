using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineClient
{
    public static class EngineClientSetting
    {
        public static string LocalIp { get; set; }

        public static string EngineIp { get; set; }

        public static bool AutoStart { get; set; }
        public static int PosEngineKeepAliveInterval { get; set; }
    }
}
