using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Blls.Location;
using BLL.Blls.LocationHistory;
using Location.TModel.Tools;
using LocationServices.Converters;
using MathNet.Numerics.Interpolation;
using TModel.Tools;
//using PosInfo = DbModel.LocationHistory.Data.Position;
using PosInfo = DbModel.LocationHistory.Data.PosInfo;
//using PositionListDb = DbModel.LocationHistory.Data.PositionList;
using PositionListDb = DbModel.LocationHistory.Data.PosInfoList;
using PersonnelDb = DbModel.Location.Person.Personnel;
using System.Threading;
using DbModel.Location.AreaAndDev;
using LocationServer;
using Location.BLL.Tool;
using Position = Location.TModel.LocationHistory.Data.Position;
using PositionList = Location.TModel.LocationHistory.Data.PositionList;
using U3DPosition = Location.TModel.LocationHistory.Data.U3DPosition;
using LocationServices.Tools;
using BLL.Tools;

namespace LocationServices.Locations.Services
{
    public interface IPosHistoryService
    {
        IList<Position> GetHistoryList(string start, string end, string tag, string person, string area);
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
            try
            {
                return dbSet.ToList().ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistory", "Exception:" + ex);
                return null;
            }
        }

        private string hisTag = "GetHisPos";

        public IList<Position> GetHistoryList(string start, string end, string tagCode, string personId, string areaId)
        {
            try
            {
                Log.Info(hisTag, string.Format("start:{0},end:{1},tagCode:{2},personId:{3},areaId:{4}", start, end, tagCode, personId, areaId));
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryList", "Exception:" + ex);
                return null;
            }
        }

        private List<Position> GetPoints(List<Position> points, int type)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetPoints", "Exception:" + ex);
                return null;
            }
        }

        public List<Position> GetHistoryByTag(string tag)
        {
            try
            {
                var info = from u in dbSet.DbSet
                           where u.Code.Contains(tag)
                           select u;
                var tempList = info.ToList();
                return tempList.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByTag", "Exception:" + ex);
                return null;
            }
        }

        public List<Position> GetHistoryByTime(DateTime start, DateTime end)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByTime", "Exception:" + ex);
                return null;
            }
        }

        public List<Position> GetHistoryByTag(string tag, DateTime start, DateTime end)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByTag", "Exception:" + ex);
                return null;
            }
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
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryU3DPositonsByTime", "Exception:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 获取标签历史位置根据PersonnelID
        /// </summary>
        /// <param name="personnelID"></param>
        /// <returns></returns>
        public List<Position> GetHistoryByPerson(int personnelID)
        {
            try
            {
                var info = from u in dbSet.DbSet
                           where personnelID == u.PersonnelID
                           select u;
                var tempList = info.ToList();
                return tempList.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByPerson", "Exception:" + ex);
                return null;
            }
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
            try
            {
                Log.Info(hisTag, string.Format("GetHistoryByPerson personnelId:{0},start:{1},end:{2}", personnelID, start, end));
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

                int tryCount = 3;
                List<Position> result = null;
                for (int i = 0; i < tryCount; i++)
                {

                    if (i > 0)
                    {
                        Log.Info(hisTag, string.Format("GetHistoryByPerson try:{0}", i));
                    }
                    try
                    {
                        //var info = from u in dbSet.DbSet
                        //           where u.PersonnelID == personnelID && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp &&
                        //                 (showUnLocatedAreaPoint || !showUnLocatedAreaPoint && u.AreaState != 1)
                        //           select u;

                        var info = from u in dbSet.DbSet
                                   where u.PersonnelID == personnelID && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp 
                                        //&& (showUnLocatedAreaPoint || !showUnLocatedAreaPoint && u.AreaState != 1)
                                   select u;
                        //2019-09-03 14:22:41,265 [19] INFO Logger - GetHisPos|0 sql:SELECT

                        

                        var tempList = info.ToList();
                        if (tempList == null) continue;

                        if (tempList != null)
                        {
                            Log.Info(hisTag, "count:" + tempList.Count);
                        }
                        if (tempList.Count == 0)
                        {
                            //string sql = info.ToString();
                            string sql = string.Format("select * from locationhistory.positions where DateTimeStamp >= {0} and DateTimeStamp <= {1} and PersonnelID = {2};", startStamp, endStamp, personnelID);
                            Log.Info(hisTag, "sql:" + sql);
                        }

                        PosDistanceHelper.FilterErrorPoints(tempList);

                        result = tempList.ToWcfModelList();

                        break;
                    }
                    catch (Exception ex1)
                    {
                        Log.Error(hisTag, "GetHistoryByPerson1 error:" + ex1);
                        Thread.Sleep(200);
                    }
                }
                if (result != null)
                {
                    result.Sort((a, b) =>
                    {
                        return a.Time.CompareTo(b.Time);
                    });
                }
                return result;
            }
            catch (Exception ex2)
            {
                Log.Error(hisTag, "GetHistoryByPerson2 error:" + ex2);
                return null;
            }

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
            try
            {
                if (topoNodeIds == null || topoNodeIds.Count == 0)
                {
                    return GetHistoryByPerson(personnelID, start, end);
                }

                return GetHistoryByArea(personnelID, topoNodeIds, start, end);
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByPersonAndArea", "Exception:" + ex);
                return null;
            }
        }

        private List<Position> GetHistoryByArea(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByArea", "Exception:" + ex);
                return null;
            }
        }

        public List<Position> GetHistoryByAreas(List<int> topoNodeIds, DateTime start, DateTime end)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByAreas", "Exception:" + ex);
                return null;
            }
        }

        public List<Position> GetHistoryByAreas(List<int> topoNodeIds)
        {
            try
            {
                var info = from u in dbSet.DbSet
                           where
                               u.IsInArea(topoNodeIds)
                           select u;
                var tempList = info.ToList();
                return tempList.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByAreas", "Exception:" + ex);
                return null;
            }
        }

        public List<Position> GetHistoryByArea(int topoNodeIds)
        {
            try
            {
                var info = from u in dbSet.DbSet
                           where
                               u.IsInArea(topoNodeIds)
                           select u;
                var tempList = info.ToList();
                return tempList.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByArea", "Exception:" + ex);
                return null;
            }
        }

        public List<Position> GetHistoryByArea(int topoNode, DateTime start, DateTime end)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryByArea", "Exception:" + ex);
                return null;
            }
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

        private static List<PosInfo> allPoslist;

        private static Dictionary<int, List<PositionListDb>> buffer = new Dictionary<int, List<PositionListDb>>();

        private void RefreshBuffer()
        {
            RefreshBuffer(1); //时间
            RefreshBuffer(2); //人员
            RefreshBuffer(3); //区域
        }

        private List<PositionListDb> RefreshBuffer(int flag)
        {
            try
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
                return list1;
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "RefreshBuffer", "Exception:" + ex);
                return null;
            }
        }

        private List<PositionListDb> GetFromBuffer(int flag)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "RefreshBuffer", "Exception:" + ex);
                return null;
            }
        }


        public void GetHistoryPositonThread()
        {
            bool bFirst = false;
            bool bGet = false;
            int nSleepTime = 1000 * 60; //60s 1m，

            GetAllData(LogTags.HisPosBuffer);//获取数据

            ////等待
            //Log.Info(LogTags.HisPosBuffer, "Sleep:" + nSleepTime + "ms");
            //Thread.Sleep(nSleepTime);
            //nSleepTime *= 10; //10分钟
            while (true)
            {
                try
                {
                    //if (!bFirst)//第一次初始化
                    //{
                    //    Log.Info("HisPosBuffer", "First Start");
                    //    bFirst = true;
                    //    GetAllData("HisPosBuffer");//获取数据
                    //    Thread.Sleep(nSleepTime); Log.Info("HisPosBuffer", "Sleep:" + nSleepTime + "ms");
                    //}
                    //else
                    //{
                    //    if (DateTime.Now.Hour != 23)
                    //    {
                    //        bGet = false;
                    //        Thread.Sleep(nSleepTime); Log.Info("HisPosBuffer", "Sleep:" + nSleepTime + "ms");
                    //    }
                    //    else
                    //    {
                    //        if (bGet)
                    //        {
                    //            Thread.Sleep(nSleepTime); Log.Info("HisPosBuffer", "Sleep:" + nSleepTime + "ms");
                    //        }
                    //        else
                    //        {
                    //            GetAllData("HisPosBuffer");//获取数据
                    //            bGet = true;
                    //            Thread.Sleep(nSleepTime); Log.Info("HisPosBuffer", "Sleep:" + nSleepTime + "ms");
                    //        }
                    //    }
                    //}

                    if (DateTime.Now.Hour == 04)//清晨四点时更新缓存
                    {
                        Log.Info(LogTags.HisPosBuffer, "更新数据");
                        GetAllData(LogTags.HisPosBuffer);//获取数据
                        //等待
                        //Log.Info(LogTags.HisPosBuffer, "Sleep:" + nSleepTime + "ms");
                        Thread.Sleep(1500 * 60 * 60);//等1.5小时后再继续循环
                    }
                    else
                    {
                        Log.Info(LogTags.HisPosBuffer, "Sleep:" + nSleepTime + "ms");
                        Thread.Sleep(nSleepTime);
                    }

                    //Log.Info(LogTags.HisPosBuffer, "Sleep:" + nSleepTime + "ms");
                    //Thread.Sleep(nSleepTime);
                    //GetAllData(LogTags.HisPosBuffer);//获取数据
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }
            }
        }

        public List<PosInfo> GetAllData(string tag, bool useBuffer = false)
        {
            try
            {
                if (useBuffer == true)
                {
                    if (allPoslist != null)//使用缓存
                    {
                        return allPoslist;
                    }
                }

                allPoslist = new List<PosInfo>();
                GC.Collect();//垃圾收集，降低内存

                //string tag = "GetAllPositionsByDay";// LogTags.Server LogTags.HisPos
                Log.Info(tag, "Start");
                DateTime start = DateTime.Now;

                Bll bll = Bll.Instance();

                //关联人员
                var personnels = bll.Personnels.ToDictionary();
                Log.Info(tag, string.Format(" personnels count:" + personnels.Count));
                //关联区域
                var areas = bll.Areas.ToDictionary();
                Log.Info(tag, string.Format(" areas count:" + areas.Count));

                var posCount = bll.Positions.DbSet.Count();
                Log.Info(tag, string.Format("posCount:{0:N}", posCount));
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

                //var list1 = bll.Positions.GetPositionsOfDay(first.DateTime);

                //var list=bll.Positions.GetPosInfoListOfDay(first.DateTime);

                //allPoslist = list;


                start = DateTime.Now;
                var list = bll.Positions.GetAllPosInfoListByDay((progress) =>
                  {
                      if (progress.Count > 0)
                      {
                          bool r = true;
                          Log.Info(tag, string.Format("date:{0},count:{1:N},({2}/{3},{4:p})",
                          progress.Date.ToString("yyyy-MM-dd"), progress.Count, progress.Index, progress.Total, progress.Percent));
                          var lst = progress.Items as List<PosInfo>;
                          if (lst != null)
                          {
                              //var groupList = lst.GroupBy(p => new { p.DateTimeStamp }).Select(p => new
                              //{
                              //    p.Key.DateTimeStamp,
                              //    Id = p.First(w => true).Id,
                              //    total = p.Count()
                              //}).Where(k => k.total > 1).ToList();
                              //if (groupList.Count > 0)
                              //{
                              //    r = false;//只取到有重复时间戳的数据为止
                              //}
                          }

                          //return false;//只取一个

                          return r;//false的话就中断了，即指获取最后一个
                      }
                      return true;
                  });
                //allPoslist = list;
                Log.Info(tag, string.Format(" getlist count:" + list.Count));
                Log.Info(tag, string.Format(" getlist time1:" + (DateTime.Now - start)));

                start = DateTime.Now;
                foreach (PosInfo item in list)
                {
                    item.ParseTimeStamp();
                }
                Log.Info(tag, string.Format(" ToDateTime time1:" + (DateTime.Now - start)));


                //var nopersonPosList=list.Where(i => i.PersonnelID == null).ToList();

                //var tagRelation = TagRelationBuffer.Instance();

                //SetPersonnalName(tag,personnels, list);
                Log.Info(tag, "SetPersonnalName Start");
                //var count = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    //if (i % 100000 == 0)
                    //{
                    //    Log.Info(tag, string.Format("SetPersonnalName {0}/{1}",(i+1), count));
                    //}
                    PosInfo pos = list[i];
                    var pid = pos.PersonnelID;

                    //if (pid == null)
                    //{
                    //    var pos2 = bll.Positions.FindById(pos.Id);
                    //    tagRelation.SetPositionInfo(pos2);
                    //}

                    if (pid != null && personnels.ContainsKey((int)pid))
                    {
                        var p = personnels[(int)pid];
                        pos.PersonnelName = string.Format("{0}({1})", p.Name, pos.Code);
                    }
                    else
                    {
                        pos.PersonnelName = string.Format("{0}({1})", pos.Code, pos.Code);//有些卡对应的人员不存在
                    }
                }
                Log.Info(tag, "SetPersonnalName End");

                //var noAreaList = SetAreaPath(tag, areas, ref list);//放到子方法中确实会导致内存占用增加 ？？？？
                Log.Info(tag, "SetAreaPath Start");
                var noAreaList = new List<PosInfo>();
                var noAreaIdList = new List<int>();
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    //if (i % 100000 == 0)
                    //{
                    //    Log.Info(tag, string.Format("SetAreaPath {0}/{1}", (i + 1), count));
                    //}
                    PosInfo pos = list[i];
                    var areaId = pos.AreaId;
                    if (areaId != null && areas.ContainsKey((int)areaId))
                    {
                        var area = areas[(int)areaId];
                        //pos.Area = area;
                        pos.AreaPath = area.GetToBuilding(">");
                        //allPoslist.Add(pos);//只有有区域数据的会被添加
                    }
                    else
                    {
                        noAreaList.Add(pos);//todo:没有区域数据的测试为什么找不到区域用
                        noAreaIdList.Add(pos.Id);
                    }
                }
                Log.Info(tag, "SetAreaPath End");
                Log.Info(tag, string.Format(" noAreaList count:" + noAreaList.Count));

                //if (noAreaIdList.Count > 0)
                //{
                //    var tagRelation = TagRelationBuffer.Instance();
                //    var posList2 = bll.Positions.Where(i => noAreaIdList.Contains(i.Id));
                //    List<int?> newAreas=new List<int?>();
                //    foreach (DbModel.LocationHistory.Data.Position position in posList2)
                //    {
                //        //tagRelation.SetArea(position);//设置区域
                //        //if (!newAreas.Contains(position.AreaId))
                //        //{
                //        //    newAreas.Add(position.AreaId);
                //        //}
                //        position.AreaId = tagRelation.GetParkArea().Id;//直接设置
                //    }
                //    //bll.Positions.EditRange(posList2);//保存到数据库
                //    Log.Info(tag, "处理找不到定位区域的数据 newAreas:"+ newAreas.Count);
                //}

                allPoslist = list;
                RefreshBuffer();

                //return allPoslist;
                Log.Info(tag, "End");
                //list.Clear();
                //list = null;
                GC.Collect();//垃圾收集，降低内存
            }
            catch (Exception ex)
            {
                string strError = ex.Message;

                Log.Info(tag, "Error:" + ex);
            }

            return allPoslist;
        }

        //private static List<PosInfo> noAreaList;

        private List<PosInfo> SetAreaPath(string tag, Dictionary<int, Area> areas, ref List<PosInfo> list)
        {
            try
            {
                Log.Info(tag, "SetAreaPath Start");
                var noAreaList = new List<PosInfo>();
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    //if (i % 10000 == 0)
                    //{
                    //    Log.Info(tag, string.Format("SetPersonnalName {0}/{1}", (i + 1), count));
                    //}
                    PosInfo pos = list[i];
                    var areaId = pos.AreaId;
                    if (areaId != null && areas.ContainsKey((int)areaId))
                    {
                        var area = areas[(int)areaId];
                        //pos.Area = area;
                        pos.AreaPath = area.GetToBuilding(">");

                        allPoslist.Add(pos);//只有有区域数据的会被添加
                    }
                    else
                    {
                        noAreaList.Add(pos);//todo:没有区域数据的测试为什么找不到区域用
                    }
                }
                Log.Info(tag, "SetAreaPath End");
                return noAreaList;
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "SetAreaPath", "Exception:" + ex);
                return null;
            }
        }

        private void SetPersonnalName(string tag, Dictionary<int, PersonnelDb> personnels, List<PosInfo> list)
        {
            try
            {
                Log.Info(tag, "SetPersonnalName Start");
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    //if (i % 10000 == 0)
                    //{
                    //    Log.Info(tag, string.Format("SetPersonnalName {0}/{1}",(i+1), count));
                    //}
                    PosInfo pos = list[i];
                    var pid = pos.PersonnelID;
                    if (pid != null && personnels.ContainsKey((int)pid))
                    {
                        var p = personnels[(int)pid];
                        pos.PersonnelName = string.Format("{0}({1})", p.Name, pos.Code);
                    }
                    else
                    {
                        pos.PersonnelName = string.Format("{0}({1})", pos.Code, pos.Code);
                        ; //有些卡对应的人员不存在
                    }
                }
                Log.Info(tag, "SetPersonnalName End");
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "SetPersonnalName", "Exception:" + ex);
            }
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
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryPositonStatistics", "Exception:" + ex);
                return null;
            }
        }

        public List<PositionListDb> GetDayOperate(int nFlag, List<PosInfo> list)
        {
            try
            {
                List<PositionListDb> Send = null;
                if (list == null)
                {
                    return Send;
                }

                Send = DbModel.LocationHistory.Data.PosInfoListHelper.GetSubList(list, nFlag);

                return Send;
            }
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetDayOperate", "Exception:" + ex);
                return null;
            }
        }


        public List<PosInfo> GetHistoryPositonData(int nFlag, string strName, string strName2,
            string strName3)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(hisTag, "GetHistoryPositonData", "Exception:" + ex);
                return null;
            }
        }
    }
}
