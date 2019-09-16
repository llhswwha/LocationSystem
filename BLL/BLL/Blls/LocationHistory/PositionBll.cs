using DAL;
using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.BLL.Tool;
using System.Threading;
using BLL.Tools;
using Base.Common.Tools;
using DbModel.Tools;
using Location.TModel.Tools;
//using PositionList = Location.TModel.LocationHistory.Data.PositionList;
using System.Threading;

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
        public PositionList GetPositionsCountOfDay(DateTime date)
        {
            DateTime start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            var count= GetPositionsCountOfDate(start, end);
            PositionList list = new PositionList(date.ToString("yyyy-MM-dd"));
            list.Name = date.ToString("yyyy-MM-dd");
            list.Count = count;
            return list;
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
        public List<PosInfo> GetPosInfoListOfDay(DateTime date)
        {
            DateTime start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            return GetInfoListOfDate(start, end);
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
        /// 获取某一月的数据
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
        /// 获取当前年份到目前为止的数据
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
        /// 获取某一月的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetAllPositionsByMonth(Func<ProgressInfo,bool> progressCallback)
        {
            Position first = DbSet.First();
            DateTime firstDay = first.DateTime;

            int year = DateTime.Now.Year - firstDay.Year;
            int month = DateTime.Now.Month - firstDay.Month+1;
            int totalMonth = year * 12 + month;
            return GetPositionsOfMonths(DateTime.Now, totalMonth, progressCallback);
        }

        /// <summary>
        /// 获取某一天的数据。
        /// 数据量小时可以用这个，数据量大(>1000w)的话要用GetAllPosInfoListByDay。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetAllPositionsByDay(Func<ProgressInfo,bool> progressCallback)
        {
            Position first = GetFirst();
            DateTime firstDay = first.DateTime;

            TimeSpan timeSpan = DateTime.Now - firstDay;
            var day = (int)timeSpan.TotalDays+2;
            return GetPositionsOfDays(DateTime.Now, day, progressCallback);
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<PosInfo> GetAllPosInfoListByDay(Func<ProgressInfo,bool> progressCallback)
        {
            Position first = GetFirst();
            DateTime firstDay = first.DateTime;

            TimeSpan timeSpan = DateTime.Now - firstDay;
            var day = (int)timeSpan.TotalDays + 2;
            return GetPosInfoListOfDays(DateTime.Now, day, progressCallback);
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<PositionList> GetAllPositionsCountByDay(Func<ProgressInfo,bool> progressCallback)
        {
            Position first = GetFirst();
            DateTime firstDay = first.DateTime;

            TimeSpan timeSpan = DateTime.Now - firstDay;
            var day = (int)timeSpan.TotalDays + 2;
            return GetPositionsCountOfDays(DateTime.Now, day, progressCallback);
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
        /// 获取某一月的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfMonths(DateTime date,int monthCount, Func<ProgressInfo,bool> progressCallback)
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
                    ProgressInfo progress = new ProgressInfo(list);
                    progress.Index = i + 1;
                    progress.Total = monthCount;
                    progress.Count = list.Count;
                    progress.Date = dateNew;
                    if (progressCallback(progress) == false)
                    {
                        break;
                    }
                }
            }
            return pos;
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfDays(DateTime date, int dayCount,Func<ProgressInfo,bool> progressCallback)
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
                    ProgressInfo progress = new ProgressInfo(list);
                    progress.Index = i + 1;
                    progress.Total = dayCount;
                    progress.Count = list.Count;
                    progress.Date = dateNew;
                    if (progressCallback(progress) == false)
                    {
                        break;
                    }
                }
            }

            return pos;
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<PosInfo> GetPosInfoListOfDays(DateTime date, int dayCount, Func<ProgressInfo,bool> progressCallback)
        {
            List<PosInfo> pos = new List<PosInfo>();
            List<List<PosInfo>> posList = new List<List<PosInfo>>();
            for (int i = 0; i < dayCount; i++)
            {
                var dateNew = date.AddDays(-i);
                List<PosInfo> list = GetPosInfoListOfDay(dateNew);
                posList.Add(list);
                pos.AddRange(list);
                if (progressCallback != null)
                {
                    ProgressInfo progress = new ProgressInfo(list);
                    progress.Index = i + 1;
                    progress.Total = dayCount;
                    progress.Count = list.Count;
                    progress.Date = dateNew;
                    if (progressCallback(progress) == false)
                    {
                        break;
                    }
                }
            }

            return pos;
        }

        public List<PositionList> GetPositionsCountOfDays(DateTime date, int dayCount, Func<ProgressInfo,bool> progressCallback)
        {
            List<PositionList> pos = new List<PositionList>();
            
            for (int i = 0; i < dayCount; i++)
            {
                var dateNew = date.AddDays(-i);
                PositionList list = GetPositionsCountOfDay(dateNew);
                pos.Add(list);
                if (progressCallback != null)
                {
                    ProgressInfo progress = new ProgressInfo(list);
                    progress.Index = i + 1;
                    progress.Total = dayCount;
                    progress.Count = list.Count;
                    progress.Date = dateNew;
                    if (progressCallback(progress) == false)
                    {
                        break;
                    }
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

            public object Items;

            public ProgressInfo(object items)
            {
                this.Items = items;
            }
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
        /// <summary>
        /// 获取某人某一时间段的数据(有区域是否为空判断)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Position> GetPositionsOfDateAndPerson(string code, DateTime start, DateTime end,string areaId)
        {
            long startStamp = start.ToStamp();
            long endStamp = end.ToStamp();
            
            if (areaId != null)
            {
                int id = Convert.ToInt32(areaId);
                var list1 = DbSet.Where(i => i.DateTimeStamp >= startStamp && i.DateTimeStamp <= endStamp && i.Code == code&&i.AreaId==id).ToList();
                return list1;
            }
            else
            {
                var list = DbSet.Where(i => i.DateTimeStamp >= startStamp && i.DateTimeStamp <= endStamp && i.Code == code).ToList();
                return list;
            } 
        }



        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int GetPositionsCountOfDate(DateTime start, DateTime end)
        {
            long startStamp = start.ToStamp();
            long endStamp = end.ToStamp();
            var query = DbSet.Where(i => i.DateTimeStamp >= startStamp && i.DateTimeStamp <= endStamp);
            var mysql = query.ToString();
            var r = query.Count();
            return r;
        }

        public string GetQuery_Count(DateTime start,DateTime end)
        {
            long startStamp = start.ToStamp();
            long endStamp = end.ToStamp();
            string mysql = string.Format("select count(*) from locationhistory.positions where DateTimeStamp >= {0} and DateTimeStamp <= {1};", startStamp,endStamp);
            return mysql;
        }

        public string GetQuery_Delete(DateTime start, DateTime end)
        {
            long startStamp = start.ToStamp();
            long endStamp = end.ToStamp();
            string mysql = string.Format("delete from locationhistory.positions where DateTimeStamp >= {0} and DateTimeStamp <= {1};", startStamp, endStamp);
            return mysql;
        }

        /// <summary>
        /// 获取某一天的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<PosInfo> GetInfoListOfDate(DateTime start, DateTime end, int tryCount = 3)
        {
            List<PosInfo> list=new List<PosInfo>();
            for (int i = 0; i < tryCount; i++)
            {
                try
                {
                    long startStamp = start.ToStamp();
                    long endStamp = end.ToStamp();
                    //var r = DbSet.Where(i => i.DateTimeStamp >= startStamp && i.DateTimeStamp <= endStamp).Count();
                    //return r;

                    var query = from p in DbSet
                        where p.DateTimeStamp >= startStamp && p.DateTimeStamp <= endStamp
                        select new PosInfo { Id = p.Id, DateTimeStamp = p.DateTimeStamp, PersonnelID = p.PersonnelID, Code = p.Code, AreaId = p.AreaId, X = p.X, Y = p.Y, Z = p.Z };
                    list = query.ToList();
                    break;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (msg.Contains(
                        "An error occurred while reading from the store provider's data reader. See the inner exception for details")
                    )
                    {
                        msg = ex.InnerException.Message;
                    }
                    Log.Error(LogTags.HisPosBuffer, msg);
                    Thread.Sleep(100);
                    //再重试
                    Log.Info(LogTags.HisPosBuffer, "GetInfoListOfDate 重试:" + (i+1));
                }
            }
            return list;
        }

        public int RemoveErrorPoints(List<PosInfo> list)
        {
            int sum = 0;
            var bags = PosInfoListHelper.GetListByCode(list);
            List<Position> allErrorPoints = new List<Position>();
            for (int i = 0; i < bags.Count; i++)
            {
                var person = bags[i];
                List<PosInfo> posInfoList = person.Items;//某个人
                Log.Info(LogTags.HisPos, string.Format("删除 {0},{1}({2}/{3})",person.Name, posInfoList.Count,(i+1), bags.Count));
                
                var errorPosList = PosDistanceHelper.FilterErrorPoints(posInfoList);
                if (errorPosList.Count > 0)
                {
                    List<int> ids = new List<int>();
                    foreach (var item in errorPosList)
                    {
                        ids.Add(item.Id);
                    }

                    var query = DbSet.AsNoTracking().Where(item => ids.Contains(item.Id)).OrderBy(item=>item.DateTimeStamp);
                    var count = query.Count();
                    var errorPoints = query.ToList();
                    allErrorPoints.AddRange(errorPoints);
                    Log.Info(LogTags.HisPos, string.Format("删除点 {0}/{1}", count,posInfoList.Count));
                    sum += count;
                    //query.DeleteFromQuery();
                }
            }
            if (sum > 0)
            {
                Log.Info(LogTags.HisPos, string.Format("删除点 {0}/{1}", sum, list.Count));
            }
            SavePositions(allErrorPoints, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
            return sum;
        }

        public void SavePositions(List<Position> posList,string name)
        {
            PositionList list = new PositionList(name);
            list.Add(posList);
            //XmlSerializeHelper.
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\ErrorPoints\\" + name + ".xml";
            XmlSerializeHelper.Save(list, filePath);
        }

        public void RemovePoints(IQueryable<Position> query,bool save)
        {
            if (save)
            {
                var points = query.ToList();
                SavePositions(points, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
            }
            
            query.DeleteFromQuery();
        }
    }
}
