using DbModel.LocationHistory.Data;
using Location.BLL.Tool;
using LocationServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Tools
{
    public static class PosDistanceHelper
    {


        public static void FilterErrorPoints<T>(List<T> posInfoList) where T : IPosInfo
        {
            if (posInfoList == null) return;
            var maxSpeed = AppContext.MoveMaxSpeed;
            if (maxSpeed <= 0) return;

            DateTime start = DateTime.Now;
            posInfoList.Sort((a, b) =>
            {
                return a.DateTimeStamp.CompareTo(b.DateTimeStamp);
            });

            
            var errorPoints = new List<T>();
            var disList = new List<PosDistance<T>>();
            var errorDisList = new List<PosDistance<T>>();
            string txt = "";
            
            for (int i = 0; i < posInfoList.Count - 1; i++)
            {
                var p1 = posInfoList[i];
                var p2 = posInfoList[i + 1];
                var dis = new PosDistance<T>(p1, p2);
                txt += dis + "\n";
                if (dis.speed > maxSpeed)
                {
                    errorDisList.Add(dis);

                    errorPoints.Add(p2);
                    posInfoList.RemoveAt(i);
                    i--;
                }
                else
                {
                    disList.Add(dis);
                }
            }
            disList.Sort();
            errorDisList.Sort();

            var time = DateTime.Now - start;
            PosDistance<T> first = null;
            if (disList.Count > 0)
            {
                first = disList.First();
            }
            Log.Info(LogTags.HisPos, string.Format("maxSpeed:{0},allCount:{1},errorCount:{2},time:{3}ms,first:{4}", maxSpeed,posInfoList.Count, errorPoints.Count, time.TotalMilliseconds, first));

            //Log.Info(LogTags.HisPos, txt);
        }
    }
}
