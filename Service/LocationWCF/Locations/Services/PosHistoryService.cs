using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Blls.Location;
using BLL.Blls.LocationHistory;
using Location.TModel.LocationHistory.Data;
using Location.TModel.Tools;
using LocationServices.Converters;
using MathNet.Numerics.Interpolation;
using TModel.Tools;
//using PositionDb = DbModel.LocationHistory.Data.Position;
using PositionDb = DbModel.LocationHistory.Data.PosInfo;
//using PositionListDb = DbModel.LocationHistory.Data.PositionList;
using PositionListDb = DbModel.LocationHistory.Data.PosInfoList;
using PersonnelDb = DbModel.Location.Person.Personnel;
using System.Threading;
using LocationServer;
using Location.BLL.Tool;

namespace LocationServices.Locations.Services
{
    public interface IPosHistoryService
    {
        IList<Position> GetHistoryList(string start, string end, string tag, string person,string area);
    }

    public class PosHistoryService : IPosHistoryService
    {
        private Bll db;

        private PositionBll dbSet;

        public PosHistoryService()
        {
            db = new Bll();
            dbSet = db.Positions;
        }

        public PosHistoryService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Positions;
        }

        public IList<Position> GetHistory()
        {
            return dbSet.ToList().ToWcfModelList();
        }

        public IList<Position> GetHistoryList(string start, string end, string tagCode, string personId, string areaId)
        {
            if (string.IsNullOrEmpty(start))
            {
                start = "1970-1-1";
            }

            if (string.IsNullOrEmpty(end))
            {
                end = "2100-1-1";
            }

            IList<Position> result = new List<Position>();
            if (!string.IsNullOrEmpty(tagCode))
            {
                result = GetHistoryByTag(tagCode, start.ToDateTime(), end.ToDateTime());
            }
            else if (!string.IsNullOrEmpty(personId))
            {
                result = GetHistoryByPerson(personId.ToInt(), start.ToDateTime(), end.ToDateTime());
            }
            else if (!string.IsNullOrEmpty(areaId))
            {
                result = GetHistoryByArea(areaId.ToInt(), start.ToDateTime(), end.ToDateTime());
            }
            else
            {
                result = GetHistoryByTime(start.ToDateTime(), end.ToDateTime());
            }

            return result;
        }

        private List<Position> GetPoints(List<Position> points, int type)
        {
            List<Position> result = new List<Position>();
            IInterpolationMethod method = null;
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();
            if (type > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    xs.Add(points[i].X);
                    ys.Add(points[i].Z);
                }
            }

            if (type == 0)
            {
                result.AddRange(points);
            }
            else if (type == 1)
            {
                method = Interpolation.CreateNaturalCubicSpline(xs, ys);
            }
            else if (type == 2)
            {
                method = Interpolation.CreateAkimaCubicSpline(xs, ys);
            }

            if (method != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Position p1 = points[i];
                    Position p2 = points[i + 1];
                    double x = (p1.X + p2.X) / 2;
                }
            }

            return result;
        }

        public List<Position> GetHistoryByTag(string tag)
        {
            var info = from u in dbSet.DbSet
                where u.Code.Contains(tag)
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByTime(DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            bool showUnLocatedAreaPoint = AppContext.ShowUnLocatedAreaPoint;
            var info = from u in dbSet.DbSet
                where u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp &&
                      (showUnLocatedAreaPoint || !showUnLocatedAreaPoint && u.AreaState != 1)
                select u;
            var tempList = info.ToList();

            //var list = dbSet.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByTag(string tag, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            bool showUnLocatedAreaPoint = AppContext.ShowUnLocatedAreaPoint;
            var info = from u in dbSet.DbSet
                where u.Code.Contains(tag) && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp &&
                      (showUnLocatedAreaPoint || !showUnLocatedAreaPoint && u.AreaState != 1)
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        /// <summary>
        ///  获取标签3D历史位置
        /// </summary>
        /// <param name="tagcode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            var info = from u in db.U3DPositions.DbSet
                where tagcode == u.Code && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        /// <summary>
        /// 获取标签历史位置根据PersonnelID
        /// </summary>
        /// <param name="personnelID"></param>
        /// <returns></returns>
        public List<Position> GetHistoryByPerson(int personnelID)
        {
            var info = from u in dbSet.DbSet
                where personnelID == u.PersonnelID
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        /// <summary>
        /// 获取标签历史位置根据PersonnelID
        /// </summary>
        /// <param name="personnelID"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Position> GetHistoryByPerson(int personnelID, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            //var query = from t1 in db.LocationCardToPersonnels.DbSet
            //            where t1.PersonnelId == personnelID
            //            select (int?)t1.PersonnelId;
            //List<int?> lst1 = query.ToList();
            bool showUnLocatedAreaPoint = AppContext.ShowUnLocatedAreaPoint;
            var info = from u in dbSet.DbSet
                where u.PersonnelID == personnelID && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp &&
                      (showUnLocatedAreaPoint || !showUnLocatedAreaPoint && u.AreaState != 1)
                select u;

            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        /// <summary>
        /// 获取历史位置信息根据PersonnelID和TopoNodeId建筑id
        /// </summary>
        /// <param name="personnelID"></param>
        /// <param name="topoNodeId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Position> GetHistoryByPersonAndArea(int personnelID, List<int> topoNodeIds, DateTime start,
            DateTime end)
        {
            if (topoNodeIds == null || topoNodeIds.Count == 0)
            {
                return GetHistoryByPerson(personnelID, start, end);
            }

            return GetHistoryByArea(personnelID, topoNodeIds, start, end);
        }

        private List<Position> GetHistoryByArea(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            var info = from u in dbSet.DbSet
                where
                    personnelID == u.PersonnelID && u.IsInArea(topoNodeIds) && u.DateTimeStamp >= startStamp &&
                    u.DateTimeStamp <= endStamp
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByAreas(List<int> topoNodeIds, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            var info = from u in dbSet.DbSet
                where
                    u.IsInArea(topoNodeIds) && u.DateTimeStamp >= startStamp &&
                    u.DateTimeStamp <= endStamp
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByAreas(List<int> topoNodeIds)
        {
            var info = from u in dbSet.DbSet
                where
                    u.IsInArea(topoNodeIds)
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByArea(int topoNodeIds)
        {
            var info = from u in dbSet.DbSet
                where
                    u.IsInArea(topoNodeIds)
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByArea(int topoNode, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            bool showUnLocatedAreaPoint = AppContext.ShowUnLocatedAreaPoint;
            var info = from u in dbSet.DbSet
                where
                    topoNode == u.AreaId && u.DateTimeStamp >= startStamp &&
                    u.DateTimeStamp <= endStamp &&
                    (showUnLocatedAreaPoint || !showUnLocatedAreaPoint && u.AreaState != 1)
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public static long GetTimeStamp(DateTime time)
        {
            //DateTime zero = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            //long msStamp = (long)(time - zero).TotalMilliseconds;//毫秒
            //if (msStamp < 0)
            //{
            //    msStamp = 0;
            //}

            long msStamp2 = TimeConvert.ToStamp(time); //秒
            return msStamp2;
        }

        private static List<PositionDb> allPoslist;

        private static Dictionary<int, List<PositionListDb>> buffer = new Dictionary<int, List<PositionListDb>>();

        private void RefreshBuffer()
        {
            RefreshBuffer(1); //时间
            RefreshBuffer(2); //人员
            RefreshBuffer(3); //区域
        }

        private void RefreshBuffer(int flag)
        {
            var list1 = GetDayOperate(flag, allPoslist);
            if (buffer.ContainsKey(flag))
            {
                buffer[flag] = list1;
            }
            else
            {
                buffer.Add(flag, list1);
            }
        }

        private List<PositionListDb> GetFromBuffer(int flag)
        {
            if (buffer.ContainsKey(flag))
            {
                return buffer[flag];
            }
            else
            {
                var list1 = GetDayOperate(flag, allPoslist);
                buffer.Add(flag, list1);
                return list1;
            }
        }


        public void GetHistoryPositonThread()
        {
            bool bFirst = false;
            bool bGet = false;
            int nSleepTime = 1000 * 60; //60s，
            nSleepTime *= 10; //10分钟
            while (true)
            {
                try
                {
                    DateTime dt = DateTime.Now;
                    int nHour = dt.Hour;

                    if (!bFirst)
                    {
                        bFirst = true;
                        GetAllData("GetHistoryPositonThread");//获取数据
                        Thread.Sleep(nSleepTime);
                        continue;
                    }

                    if (nHour != 23)
                    {
                        bGet = false;
                        Thread.Sleep(nSleepTime);
                        continue;
                    }

                    if (bGet)
                    {
                        Thread.Sleep(nSleepTime);
                        continue;
                    }

                    GetAllData("GetHistoryPositonThread");//获取数据

                    bGet = true;

                    Thread.Sleep(nSleepTime);
                    continue;
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }
            }
        }

        public List<PositionDb> GetAllData(string tag,bool useBuffer=false)
        {
            try
            {
                if (useBuffer == true)
                {
                    if (allPoslist != null)
                    {
                        return allPoslist;
                    }
                }
                else
                {
                    if (allPoslist != null)
                    {
                        allPoslist.Clear();
                        GC.Collect();//垃圾收集，降低内存
                    }
                }
                //string tag = "GetAllPositionsByDay";// LogTags.Server LogTags.HisPos
                Log.Info(tag, "Start");
                DateTime start = DateTime.Now;

                Bll bll = Bll.Instance();
                //bll.history

                var count = bll.Positions.DbSet.Count();
                Log.Info(tag, string.Format("count:"+ count));
                //allPoslist = bll.Positions.ToList();

                var first = bll.Positions.GetFirst();
                Log.Info(tag, string.Format("first:" + first.Id));

                //bll.Positions.GetAllPositionsCountByDay((progress) =>
                //{
                //    if (progress.Count > 0)
                //    {
                //        Log.Info(tag, string.Format("date:{0},count:{1},({2}/{3},{4:p})",
                //        progress.Date.ToString("yyyy-MM-dd"), progress.Count, progress.Index, progress.Total, progress.Percent));
                //    }
                //});

                start = DateTime.Now;
                var list=bll.Positions.GetAllPosInfoListByDay((progress) =>
                {
                    if (progress.Count > 0)
                    {
                        Log.Info(tag, string.Format("date:{0},count:{1},({2}/{3},{4:p})",
                        progress.Date.ToString("yyyy-MM-dd"), progress.Count, progress.Index, progress.Total, progress.Percent));
                    }
                });
                allPoslist = list;

                var time1 = DateTime.Now - start;
                Log.Info(tag, string.Format("time1:" + time1));

                //int total = 0;

                //for (int i = 0; i < 10; i++)
                //{
                //    start = DateTime.Now;
                //    var list2 = bll.Positions.GetPageList(10, i, out total);
                //    Log.Info(tag, string.Format("list3:{0},time:{1}", list2.Count, DateTime.Now - start));
                //}

                //var last = bll.Positions.GetLast();
                //Log.Info(tag, string.Format("last:" + last.Id));


                ////太多了 一次取的话 卡住很久很久。
                //start = DateTime.Now;
                //var list4 = bll.Positions.GetAllPositionsByDay((progress) =>
                //{
                //    Log.Info(tag, string.Format("date:{0},count:{1},({2}/{3},{4:p})",
                //        progress.Date, progress.Count, progress.Index, progress.Total, progress.Percent));
                //});
                //allPoslist = list4;

                //return;

                //var time1 = DateTime.Now - start;
                //Log.Info(tag, string.Format("time1:"+ time1));

                var personnels = bll.Personnels.ToDictionary();
                foreach (var pos in allPoslist)
                {
                    var pid = pos.PersonnelID;
                    if (pid != null && personnels.ContainsKey((int) pid))
                    {
                        var p = personnels[(int) pid];
                        pos.PersonnelName = string.Format("{0}({1})", p.Name, pos.Code);
                    }
                    else
                    {
                        pos.PersonnelName = string.Format("{0}({1})", pos.Code, pos.Code); ; //有些卡对应的人员不存在
                    }
                }

                List<PositionDb> noAreaList = new List<PositionDb>();

                var areas = bll.Areas.ToDictionary();
                foreach (var pos in allPoslist)
                {
                    var areaId = pos.AreaId;
                    if (areaId != null && areas.ContainsKey((int)areaId))
                    {
                        var area = areas[(int)areaId];
                        //pos.Area = area;
                        pos.AreaPath = area.GetToBuilding(">");

                        allPoslist.Add(pos);
                    }
                    else
                    {
                        noAreaList.Add(pos);
                    }
                }

                RefreshBuffer();

                //return allPoslist;
                Log.Info(tag, "End");
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            return allPoslist;
        }

        /// <summary>
        /// 获取历史位置信息统计
        /// </summary>
        /// <param name="nFlag"></param>
        /// <param name="strName"></param>
        /// <param name="strName2"></param>
        /// <returns></returns>
        public List<PositionList> GetHistoryPositonStatistics(int nFlag, string strName, string strName2,
            string strName3)
        {
            if (nFlag != 1 && nFlag != 2 && nFlag != 3)
            {
                return null;
            }

            int nFlag2 = 0;
            int nFlag3 = 0;
            int nFlag4 = 0;

            if (nFlag == 1)
            {
                nFlag2 = 2;
                nFlag3 = 4;
            }
            else if (nFlag == 2)
            {
                nFlag2 = 1;
                nFlag3 = 4;
            }
            else
            {
                nFlag2 = 2;
                nFlag3 = 1;
                nFlag4 = 4;
            }

            //获取第一层数据
            //SendList = GetDayOperate(nFlag, allPoslist);
            List<PositionListDb> list = GetFromBuffer(nFlag); //从缓存取，避免重复计算。
            if (list == null)
            {
                return null;
            }

            if (strName == "")
            {
                return list.ToTModel();
            }

            //获取第二层数据
            PositionListDb Result = list.Find(p => p.Name == strName);
            if (Result == null)
            {
                return null;
            }

            list = GetDayOperate(nFlag2, Result.Items);
            if (list == null)
            {
                return null;
            }

            if (strName2 == "")
            {
                return list.ToTModel();
            }

            //获取第三层数据
            Result = list.Find(p => p.Name == strName2);
            if (Result == null)
            {
                return null;
            }

            list = GetDayOperate(nFlag3, Result.Items);
            if (list == null)
            {
                return null;
            }

            if (strName3 == "")
            {
                return list.ToTModel();
            }

            //获取第四层数据
            Result = list.Find(p => p.Name == strName3);
            if (Result == null)
            {
                return null;
            }

            list = GetDayOperate(nFlag4, Result.Items);
            if (list == null)
            {
                return null;
            }

            return list.ToTModel();
        }

        public List<PositionListDb> GetDayOperate(int nFlag, List<PositionDb> list)
        {
            List<PositionListDb> Send = null;
            if (list == null)
            {
                return Send;
            }

            switch (nFlag)
            {
                case 1:
                    Send = PositionListDb.GetListByDay(list);
                    break;
                case 2:
                    Send = PositionListDb.GetListByPerson(list);
                    break;
                case 3:
                    Send = PositionListDb.GetListByArea(list);
                    break;
                case 4:
                    Send = PositionListDb.GetListByHour(list);
                    break;
                default:
                    break;
            }

            return Send;
        }


        public List<PositionDb> GetHistoryPositonData(int nFlag, string strName, string strName2,
            string strName3)
        {
            if (nFlag != 1 && nFlag != 2 && nFlag != 3)
            {
                return null;
            }

            int nFlag2 = 0;
            int nFlag3 = 0;
            int nFlag4 = 0;

            if (nFlag == 1)
            {
                nFlag2 = 2;
                nFlag3 = 4;
            }
            else if (nFlag == 2)
            {
                nFlag2 = 1;
                nFlag3 = 4;
            }
            else
            {
                nFlag2 = 2;
                nFlag3 = 1;
                nFlag4 = 4;
            }

            //获取第一层数据
            //SendList = GetDayOperate(nFlag, allPoslist);
            List<PositionListDb> list = GetFromBuffer(nFlag); //从缓存取，避免重复计算。
            if (list == null)
            {
                return null;
            }

            if (strName == "")
            {
                //return list.ToTModel();
                return null;
            }

            //获取第二层数据
            PositionListDb Result = list.Find(p => p.Name == strName);
            if (Result == null)
            {
                return null;
            }


            if (strName2 == "")
            {
                return Result.Items;
            }

            list = GetDayOperate(nFlag2, Result.Items);
            if (list == null)
            {
                return null;
            }


            //获取第三层数据
            Result = list.Find(p => p.Name == strName2);
            if (Result == null)
            {
                return null;
            }

            if (strName3 == "")
            {
                return Result.Items;
            }

            list = GetDayOperate(nFlag3, Result.Items);
            if (list == null)
            {
                return null;
            }

            

            ////获取第四层数据
            //Result = list.Find(p => p.Name == strName3);
            //if (Result == null)
            //{
            //    return null;
            //}

            //list = GetDayOperate(nFlag4, Result.Items);
            //if (list == null)
            //{
            //    return null;
            //}

            //return list.ToTModel();

            return null;
        }
    }
}
