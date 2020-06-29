using Location.BLL.Tool;
using System;

namespace Location.TModel.Tools
{
    public static class TimeConvert
    {
        public static long ToStamp(this DateTime dt)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1));
            long timeStamp = (long)(dt.Ticks - startTime.Ticks)/10000;
            return timeStamp;
        }


        public static DateTime ToDateTime(this long timeStamp)
        {
            try
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                DateTime dt = startTime.AddMilliseconds(timeStamp);
                return dt;
            }
            catch (Exception ex)
            {
                Log.Error("TimeConvert", "ToDateTime:" + ex);
                return DateTime.MinValue;
            }
            
        }

        public static DateTime ToDateTimeSeconds(this long timeStamp, bool isSencond)
        {
            try
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                DateTime dt = startTime.AddSeconds(timeStamp);
                return dt;
            }
            catch (Exception ex)
            {
                Log.Error("TimeConvert", "ToDateTimeSeconds:" + ex);
                return DateTime.MinValue;
            }
            
        }

        public static int DateTimeToInt(DateTime time)
        {
            double intResult = 0;

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            intResult = (time - startTime).TotalSeconds;

            return (int)intResult;
        }
        public static DateTime IntToDatetime(double utc)

        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            startTime = startTime.AddSeconds(utc);

            startTime = startTime.AddHours(8);//转化为北京时间(北京时间=UTC时间+8小时 )            

            return startTime;

        }
    }

   
}
