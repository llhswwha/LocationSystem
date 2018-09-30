using System;

namespace DbModel.Tools
{
    public static class TimeConvert
    {
        public static long DateTimeToTimeStamp(DateTime dt)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1));
            long TimeStamp = (long)(dt.Ticks - startTime.Ticks)/10000000;
            return TimeStamp;
        }

        public static DateTime TimeStampToDateTime(long TimeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime dt = startTime.AddSeconds(TimeStamp);
            return dt;
        }
    }
}
