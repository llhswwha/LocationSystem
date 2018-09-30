using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.Tools
{
    public static class LogEvent
    {
        public static event Action<string> InfoEvent;

        public static void Info(string msg)
        {
            Console.WriteLine(msg);
            if (InfoEvent != null)
            {
                InfoEvent(msg);
            }
        }
    }
}
