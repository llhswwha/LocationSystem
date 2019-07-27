using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NetClient
{
    class CommonFunction
    {
        public static double ConvertDateTimeToInt(DateTime time)
        {
            double intResult = 0;
            
            DateTime starttime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1));
            //time.AddHours(+8);
            double dSpace = (DateTime.Now - DateTime.UtcNow).TotalSeconds;
            intResult = (time - starttime).TotalSeconds + dSpace;
            return intResult;
        }
        public static  DateTime ConvertIntToDateTime(double utc)
        {
            DateTime starttime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            double dSpace = (DateTime.Now - DateTime.UtcNow).TotalSeconds;
            starttime = starttime.AddSeconds(utc - dSpace);

            return starttime;
        
        }
        public static long NvsFileTimeToAbsSeconds(NVS_FILE_TIME nvstime)
        {
            long iTime = 0;
            DateTime dtTime = new DateTime(nvstime.m_iYear,
                                           nvstime.m_iMonth,
                                           nvstime.m_iDay,
                                           nvstime.m_iHour,
                                           nvstime.m_iSecond,
                                           nvstime.m_iMinute);
            double dTime = ConvertDateTimeToInt(dtTime);
            try
            {
                iTime = Convert.ToInt64(dTime);
            }
            catch (System.Exception ex)
            {
                return iTime;
            }
            return iTime;
        }
    }


}
