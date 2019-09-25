using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Common.Tools
{
    public static class StaticEvents
    {
        public static event Action<List<string>> LocateArchorByIp;

        public static void OnLocateArchorByIp(List<string> ipList)
        {
            if (LocateArchorByIp != null)
            {
                LocateArchorByIp(ipList);
            }
        }
    }
}
