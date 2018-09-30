using NsqSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SihuiThermalPowerPlant.Controllers
{
    public class ProductRealTimeAlarm
    {
        private static int nId = 10;

        public static void ProductRealTimeAlarmInfo()
        {
            var producer = new Producer("127.0.0.1:4150");

            while (true)
            {
                string strInfo = ProductAlarmInfo();
                producer.Publish("test-topic-name", strInfo);
                Thread.Sleep(6000);
            }

            producer.Stop();

        }

        public static string ProductAlarmInfo()
        {
            nId++;
            string strId = Convert.ToString(nId);
            int n = nId % 2;
            int nLevel = nId % 4;

            string strMsg = "单向门禁001产生了告警";
            string device_id = "5";
            string device_desc = "单向门禁001";
            DateTime dt = DateTime.Now;
            string strdt = "";
            if (n == 0)
            {
                strMsg = "双向门禁001产生了告警";
                device_id = "6";
                device_desc = "双向门禁001";
            }
            string strLevel = Convert.ToString(nLevel);


            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long TimeStamp = (long)(dt.Ticks - startTime.Ticks) / 10000000;
            strdt = Convert.ToString(TimeStamp);

            string strInfo = "{";
            strInfo += "\"id\":" + strId + ",";
            strInfo += "\"title\":\"告警" + strId + "\",";
            strInfo += "\"msg\":\"" + strMsg + strId + "\",";
            strInfo += "\"level\":" + strLevel + ",";
            strInfo += "\"code\":\"event" + strId + "\",";
            strInfo += "\"src\":2,";
            strInfo += "\"device_id\":" + device_id + ",";
            strInfo += "\"device_desc\":\"" + device_desc + "\",";
            strInfo += "\"t\":" + strdt;
            strInfo += "}";
            return strInfo;
        }

    }
}