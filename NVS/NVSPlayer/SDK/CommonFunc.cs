using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVSPlayer.SDK
{
    public static class CommonFunc
    {
        public static double ConvertDateTimeToInt(DateTime time)
        {
            double intResult = 0;

            DateTime starttime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            //time.AddHours(+8);
            double dSpace = (DateTime.Now - DateTime.UtcNow).TotalSeconds;
            intResult = (time - starttime).TotalSeconds + dSpace;
            return intResult;
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
            iTime = Convert.ToInt64(dTime);
            return iTime;
        }

        public static DateTime ConvertIntToDateTime(double utc)
        {
            DateTime starttime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            double dSpace = (DateTime.Now - DateTime.UtcNow).TotalSeconds;
            starttime = starttime.AddSeconds(utc - dSpace);
            return starttime;
        }

        public static String AbsSecondsToStr(double utc)
        {
            DateTime dtTime = ConvertIntToDateTime(utc);
            String str = "";
            str = dtTime.Year.ToString() + "-" + dtTime.Month.ToString("D2") + "-" +
                    dtTime.Day.ToString("D2") + " " + dtTime.Hour.ToString("D2") + ":" +
                    dtTime.Minute.ToString("D2") + ":" + dtTime.Second.ToString("D2");
            return str;
        }

        //获取当前时间串
        public static String GetCurTimeStr()
        {
            System.DateTime dtCutTime = new System.DateTime();
            dtCutTime = System.DateTime.Now;

            string strTime = dtCutTime.Year.ToString() + dtCutTime.Month.ToString("D2") +
                             dtCutTime.Day.ToString("D2") + dtCutTime.Hour.ToString("D2") +
                             dtCutTime.Second.ToString("D2") + dtCutTime.Minute.ToString("D2");
            return strTime;
        }
    }
}
