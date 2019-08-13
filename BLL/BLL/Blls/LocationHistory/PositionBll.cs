using DAL;
using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Tools;

namespace BLL.Blls.LocationHistory
{
    public class PositionBll : BaseBll<Position, LocationHistoryDb>
    {
        public PositionBll() : base()
        {

        }
        public PositionBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Positions;
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfDay(DateTime date)
        {
            DateTime start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            return GetPositionsOfDate(start, end);
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfSevenDay(DateTime date)
        {
            DateTime start = new DateTime(date.Year, date.Month, date.Day-6, 0, 0, 0);
            DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            return GetPositionsOfDate(start, end);
        }

        //// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        private DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        //// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        private DateTime LastDayOfYear(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.DayOfYear).AddYears(1).AddDays(-1);
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfMonth(DateTime date)
        {
            DateTime start = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
            DateTime lastDay = LastDayOfMonth(date);
            DateTime end = new DateTime(date.Year, date.Month, lastDay.Day, 23, 59, 59);
            return GetPositionsOfDate(start, end);
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfYear(DateTime date)
        {
            DateTime start = new DateTime(date.Year, 1, 1, 0, 0, 0);
            DateTime lastDay = LastDayOfYear(date);
            DateTime end = new DateTime(date.Year, date.Month, lastDay.Day, 23, 59, 59);
            return GetPositionsOfDate(start, end);
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetAllPositionsByMonth(Action<ProgressInfo> progressCallback)
        {
            Position first = DbSet.First();
            DateTime firstDay = first.DateTime;

            int year = DateTime.Now.Year - firstDay.Year;
            int month = DateTime.Now.Month - firstDay.Month+1;
            int totalMonth = year * 12 + month;
            return GetPositionsOfMonths(DateTime.Now, totalMonth, progressCallback);
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetAllPositionsByDay(Action<ProgressInfo> progressCallback)
        {
            Position first = DbSet.First();
            DateTime firstDay = first.DateTime;
            if (firstDay.Year < 2019)
            {
                long timestamp = new DateTime(2019, 1, 1, 0, 0, 0).ToStamp();
                first = DbSet.First(i => i.DateTimeStamp > timestamp);
                firstDay = first.DateTime;
            }

            TimeSpan timeSpan = DateTime.Now - firstDay;
            var day = (int)timeSpan.TotalDays+2;
            return GetPositionsOfDays(DateTime.Now, day, progressCallback);
        }


        public Position GetFirst()
        {
            Position first = DbSet.First();
            DateTime firstDay = first.DateTime;
            if (firstDay.Year < 2019)
            {
                long timestamp = new DateTime(2019, 1, 1, 0, 0, 0).ToStamp();
                first = DbSet.First(i => i.DateTimeStamp > timestamp);
                firstDay = first.DateTime;
            }
            return first;
        }

        public Position GetLast()
        {
            Position first = DbSet.Last();
            DateTime firstDay = first.DateTime;
            if (firstDay.Year < 2019)
            {
                long timestamp = new DateTime(2019, 1, 1, 0, 0, 0).ToStamp();
                first = DbSet.Last(i => i.DateTimeStamp > timestamp);
                firstDay = first.DateTime;
            }
            return first;
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfMonths(DateTime date,int monthCount, Action<ProgressInfo> progressCallback)
        {
            List<Position> pos = new List<Position>();
            List<List<Position>> posList = new List<List<Position>>();
            for (int i = 0; i < monthCount; i++)
            {
                var dateNew = date.AddMonths(-i);
                List<Position> list = GetPositionsOfMonth(dateNew);
                posList.Add(list);
                pos.AddRange(list);
                if (progressCallback != null)
                {
                    ProgressInfo progress = new ProgressInfo();
                    progress.Index = i + 1;
                    progress.Total = monthCount;
                    progress.Count = list.Count;
                    progress.Date = dateNew;
                    progressCallback(progress);
                }
            }
            return pos;
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfDays(DateTime date, int dayCount,Action<ProgressInfo> progressCallback)
        {
            List<Position> pos = new List<Position>();
            List<List<Position>> posList = new List<List<Position>>();
            for (int i = 0; i < dayCount; i++)
            {
                var dateNew = date.AddDays(-i);
                List<Position> list = GetPositionsOfDay(dateNew);
                posList.Add(list);
                pos.AddRange(list);
                if (progressCallback != null)
                {
                    ProgressInfo progress = new ProgressInfo();
                    progress.Index = i + 1;
                    progress.Total = dayCount;
                    progress.Count = list.Count;
                    progress.Date = dateNew;
                    progressCallback(progress);
                }
            }

            return pos;
        }

        public class ProgressInfo
        {
            public int Index;
            public int Total;

            public double Percent
            {
                get { return (Index + 1.0) / Total; }
            }
        
            public int Count;
            public DateTime Date;
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfDate(DateTime start, DateTime end)
        {
            long startStamp = start.ToStamp();
            long endStamp = end.ToStamp();
            var list = DbSet.Where(i => i.DateTimeStamp >= startStamp && i.DateTimeStamp <= endStamp).ToList();
            return list;
        }

        
    }
}
