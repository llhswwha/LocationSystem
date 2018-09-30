using System;

namespace DbModel.Tools
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
