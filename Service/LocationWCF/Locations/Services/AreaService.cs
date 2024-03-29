﻿using BLL;
using BLL.Blls.Location;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel.Location.Person;
using DbModel.Tools;
using Location.IModel;
using TModel.Location.AreaAndDev;
using TModel.Location.Nodes;
using TModel.Tools;
using DbEntity = DbModel.Location.AreaAndDev.Area;
using TEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;
using DbModel.Location.Data;
using TPerson = Location.TModel.Location.Person.Personnel;
using DbPerson = DbModel.Location.Person.Personnel;
using Point = Location.TModel.Location.AreaAndDev.Point;
using Bound = Location.TModel.Location.AreaAndDev.Bound;
using TModel.Location.Person;
using System.Diagnostics;
using Location.TModel.Tools;
using Base.Common.Tools;

namespace LocationServices.Locations.Services
{
    public interface IAreaService : ITreeEntityService<TEntity>
    {
        IList<TEntity> GetListWithPerson();

        TEntity GetTreeWithDev(bool containCAD = false);
        TEntity GetTreeWithPerson();

        /// <summary>
        /// 获取树节点
        /// </summary>
        /// <param name="view">0:基本数据;1:设备信息;2:人员信息;3:设备信息+人员信息</param>
        /// <returns></returns>
        TEntity GetTree(int view);

        /// <summary>
        /// 获取树节点基本数据
        /// </summary>
        /// <param name="view">0:基本数据;1:基本设备信息;2:基本人员信息;3:基本设备信息+基本人员信息;4:只显示设备的节点;5:只显示人员的节点;6:只显示人员或设备的节点</param>
        /// <returns></returns>
        AreaNode GetBasicTree(int view);

        AreaStatistics GetAreaStatistics(int id);

        Area GetAreaById(int id);

        AreaPoints GetPoints(string areaId);

        List<AreaPoints> GetPointsByPid(string pid);
    }
    public class AreaService : IAreaService
    {
        private Bll db;

        private AreaBll dbSet;

        public AreaService()
        {
            db = Bll.NewBllNoRelation();
            dbSet = db.Areas;
        }

        public AreaService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Areas;
        }

        public List<TEntity> GetList()
        {
            var list1 = dbSet.ToList();
            return list1.ToWcfModelList();
        }

        public AreaStatistics GetAreaStatisticsCount(List<int?> lst)
        {

            try
            {
                var query = from t1 in db.LocationCardPositions.DbSet
                            join t2 in db.Personnels.DbSet on t1.PersonId equals t2.Id
                            where lst.Contains(t1.AreaId)
                            select t2;

                var query2 = from t1 in db.DevInfos.DbSet
                             where lst.Contains(t1.ParentId) && t1.Local_TypeCode != 20180821 //把基站排除了
                             select t1;

                var query3 = from t1 in db.LocationAlarms.DbSet
                             where lst.Contains(t1.AreaId) && t1.AlarmLevel != 0
                             select t1;

                DateTime now = DateTime.Now;
                DateTime todayStart = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
                DateTime todayEnd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);
                var startStamp = TimeConvert.ToStamp(todayStart);
                var endStamp = TimeConvert.ToStamp(todayEnd);

                var query4 = from t1 in db.DevInfos.DbSet
                             join t2 in db.DevAlarms.DbSet on t1.Id equals t2.DevInfoId
                             where lst.Contains(t1.ParentId) && t2.AlarmTimeStamp >= startStamp && t2.AlarmTimeStamp <= endStamp
                             select t2;

                var query5 = (from t1 in db.LocationAlarms.DbSet
                              where lst.Contains(t1.AreaId) && t1.AlarmLevel != 0
                              select t1.PersonnelId).Distinct().ToList();

                var ass = new AreaStatistics();


                ass.PersonNum = query.Count();
                ass.DevNum = query2.Count();
                ass.LocationAlarmNum = query3.Count();
                ass.DevAlarmNum = query4.Count();
                ass.AlarmPersonNum = query5.Count();//只需要数量信息，不要用ToList()，避免大量数据时封装到实体类的消耗


                PersonService personService = new PersonService(db);
                var list = personService.GetList(false, false);
                if (list != null)
                {
                    ass.PersonNum = list.Count;
                }
                else
                {
                    ass.PersonNum = 0;
                }
                return ass;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetAreaStatisticsCount", ex.ToString());
                return null;
            }
        }

        public IList<TEntity> GetListWithPerson()
        {
            try
            {
                IList<TEntity> list = GetList();
                BindPerson(list);
                return list;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetListWithPerson", ex.ToString());
                return null;
            }
        }

        bool showHidePerson = false;//隐藏待机的人员

        private List<TPerson> GetPersonAreaList()
        {
            try
            {
                PersonService personService = new PersonService(db);
                return personService.GetList(false, showHidePerson);
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetPersonAreaList", ex.ToString());
                return null;
            }
        }

        public static string tag = "AreaService";

        public bool ModifySize(Bound bound, double cx1, double cy1, double sx2, double sy2)
        {
            try
            {
                if (bound.Points != null && bound.Points.Count != 4)
                {
                    return false;
                }
                if (bound.Points != null)
                {
                    var points = db.Points.FindAll(i => i.BoundId == bound.Id);
                    db.Points.RemoveList(points);
                    bound.Points.Clear();
                }

                float x1 = (float)(cx1 - sx2 / 2);
                float y1 = (float)(cy1 - sy2 / 2);
                float x2 = (float)(cx1 + sx2 / 2);
                float y2 = (float)(cy1 + sy2 / 2);

                var pointsNew = bound.SetInitBound(x1, y1, x2, y2);

                var dbPointsNew = pointsNew.ToDbModel();
                bool r1 = db.Points.AddRange(dbPointsNew);
                for (int i = 0; i < dbPointsNew.Count; i++)
                {
                    pointsNew[i].Id = dbPointsNew[i].Id;
                }

                var dbBound = bound.ToDbModel();
                bool r2 = db.Bounds.Edit(dbBound);

                return r1 && r2;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "ModifySize", ex.ToString());
                return false;
            }
        }

        class PersonArea
        {
            public Personnel Person { get; set; }

            public int? AreaId { get; set; }

            public LocationCard Tag { get; set; }

            public LocationCardPosition Pos { get; set; }
        }

        public TEntity GetTree()
        {
            try
            {
                var root0 = db.GetAreaTree(false);
                var root = root0.ToTModel();
                if (root != null)
                    root.SetParent();
                return root;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetTree", ex.ToString());
                return null;
            }
        }


        public IList<TEntity> GetList(int view)
        {
            try
            {
                var list1 = dbSet.ToList();
                return list1.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetList", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 获取树节点
        /// </summary>
        /// <param name="view">0:基本数据;1:设备信息;2:人员信息;3:设备信息+人员信息;4:1+CAD</param>
        /// <returns></returns>
        public TEntity GetTree(int view)
        {
            try
            {
                showHidePerson = false;//隐藏待机的人员

                TEntity tree = null;
                if (view == 0)
                {
                    tree = GetTree();
                }
                else if (view == 1)
                {
                    tree = GetTreeWithDev();
                }
                else if (view == 4)
                {
                    tree = GetTreeWithDev(true);
                }
                else if (view == 2)
                {
                    tree = GetTreeWithPerson();
                }
                else if (view == 3)
                {
                    var leafNodes = db.DevInfos.ToList();
                    tree = GetTreeWithPerson(leafNodes.ToTModel());
                }

                else
                {
                    Log.Error("GetTree View=" + view);
                }
                if (tree != null)
                    tree.SetParent();
                if (tree != null)
                {
                    //string xml = XmlSerializeHelper.GetXmlText(tree);
                    //int length = xml.Length;
                }
                else
                {
                    Log.Error("GetTree tree == null");
                }
                return tree;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetTree", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 获取树节点基本数据
        /// </summary>
        /// <param name="view">0:基本数据;1:基本设备信息;2:基本人员信息;3:基本设备信息+基本人员信息</param>
        /// <returns></returns>
        public AreaNode GetBasicTree(int view)
        {
            try
            {
                var areaList = dbSet.ToList();
                if (areaList == null) return null;
                List<Area> list2 = new List<Area>();
                for (int i = 0; i < areaList.Count; i++)
                {
                    if (areaList[i].Type != AreaTypes.CAD)
                    {
                        list2.Add(areaList[i]);
                    }
                }
                areaList = list2;
                var list = areaList.ToTModelS();

                List<DevNode> devs = null;
                if (view == 0)
                {

                }
                else if (view == 1 || view == 4)
                {
                    devs = db.DevInfos.ToList().ToTModelS();
                }
                else if (view == 2 || view == 5)
                {
                    BindPerson(list);
                }
                else if (view == 3 || view == 6)
                {
                    BindPerson(list);
                    devs = db.DevInfos.ToList().ToTModelS();
                }

                var roots = TreeHelper.CreateTree(list, devs);
                AreaNode root = null;
                if (roots.Count > 0)
                {
                    root = roots[0];//根节点

                    if(root.Children==null|| root.Children.Count == 0)//不应该，会有问题
                    {
                        Log.Error("AreaService","获取根区域信息有问题:"+root.Name);
                        return null;
                    }
                    var park = root.Children[0];//四会热电厂

                    //将电厂下的人员移动到其他区域中
                    var otherArea = new AreaNode();
                    otherArea.Id = 100000;
                    otherArea.Name = "厂区内";
                    if (park.Persons != null)
                    {
                        foreach (var person in park.Persons)
                        {
                            otherArea.AddPerson(person);
                        }
                        park.Persons.Clear();
                    }
                    if (park.Children == null)
                    {
                        park.Children = new List<AreaNode>();
                    }
                    park.Children.Add(otherArea);
                    //root = park;//将电厂做为根节点
                    //考虑多种场景切换，这里还是不设置哪个为根节点了
                }
                if (view == 4 || view == 5 || view == 6)
                {
                    RemoveEmptyNodes(root);
                }

                SumNodeCount(root);

                //if (root.Children.Count == 0)
                //{
                //    root.Children = null;
                //}

                SetChildrenNull(root);

                return root;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetBasicTree", ex.ToString());
                return null;
            }
        }

        private void SetChildrenNull(AreaNode node)
        {
            try
            {
                if (node == null) return;
                if (node.Children != null)
                {
                    foreach (var subNode in node.Children)
                    {
                        SetChildrenNull(subNode);
                    }
                    if (node.Children != null && node.Children.Count == 0)
                    {
                        node.Children = null;
                    }
                    if (node.Persons != null && node.Persons.Count == 0)
                    {
                        node.Persons = null;
                    }
                    if (node.LeafNodes != null && node.LeafNodes.Count == 0)
                    {
                        node.Children = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "SetChildrenNull", ex.ToString());
            }
        }

        private void RemoveEmptyNodes(AreaNode node)
        {
            try
            {
                if (node == null) return;
                if (node.Children != null)
                    for (int i = 0; i < node.Children.Count; i++)
                    {
                        AreaNode subNode = node.Children[i];
                        if (subNode.IsSelftEmpty())
                        {
                            node.Children.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            RemoveEmptyNodes(subNode);
                            if (subNode.IsSelftEmpty())
                            {
                                node.Children.RemoveAt(i);
                                i--;
                            }
                        }
                    }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "RemoveEmptyNodes", ex.ToString());
            }
        }

        /// <summary>
        /// 遍历并计算数量
        /// </summary>
        /// <param name="node"></param>
        private void SumNodeCount(AreaNode node)
        {
            try
            {
                if (node == null) return;
                if (node.Persons != null)
                    node.TotalPersonCount = node.Persons.Count;
                if (node.LeafNodes != null)
                    node.TotalDevCount = node.LeafNodes.Count;

                if (node.Children != null)
                {
                    for (int i = 0; i < node.Children.Count; i++)
                    {
                        AreaNode subNode = node.Children[i];
                        SumNodeCount(subNode);
                        node.TotalPersonCount += subNode.TotalPersonCount;
                        node.TotalDevCount += subNode.TotalDevCount;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "SumNodeCount", ex.ToString());
            }
        }

        private void BindPerson(IList<AreaNode> list)
        {
            try
            {
                var personList = GetPersonAreaList();
                if (personList != null)
                    foreach (var item in personList)
                    {
                        var entity = list.FirstOrDefault(i => i.Id == item.AreaId);
                        if (entity != null)
                        {
                            entity.AddPerson(item.ToTModelS());
                        }
                    }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "BindPerson", ex.ToString());
            }
        }

        private void BindPerson(IList<TEntity> list)
        {
            try
            {
                var personList = GetPersonAreaList();
                if (personList != null)
                    foreach (var item in personList)
                    {
                        var entity = list.FirstOrDefault(i => i.Id == item.AreaId);
                        if (entity != null)
                        {
                            entity.AddPerson(item);
                        }
                    }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "BindPerson", ex.ToString());
            }
        }

        public TEntity GetTreeWithDev(bool containCAD = false)
        {
            try
            {
                var root0 = db.GetAreaTree(true, null, containCAD);
                var root = root0.ToTModel();
                return root;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetTreeWithDev", ex.ToString());
                return null;
            }
        }

        public TEntity GetTreeWithPerson()
        {
            return GetTreeWithPerson(null);
        }

        private TEntity GetTreeWithPerson(List<Location.TModel.Location.AreaAndDev.DevInfo> devs)
        {
            try
            {
                var list = GetListWithPerson().ToList();
                List<TEntity> list2 = new List<TEntity>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Type != AreaTypes.CAD)
                    {
                        list2.Add(list[i]);
                    }
                }
                list = list2;
                var roots = TreeHelper.CreateTree(list, devs);
                if (roots.Count > 0)
                {
                    return roots[0];
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetTreeWithPerson", ex.ToString());
                return null;
            }
        }

        public TEntity GetTree(string id)
        {
            try
            {
                var item = dbSet.Find(id.ToInt());
                GetChildrenTree(item);
                return item.ToTModel();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetTree", ex.ToString());
                return null;
            }
        }

        private List<DbEntity> GetChildren(DbEntity item)
        {
            try
            {
                if (item != null)
                {
                    var list = dbSet.FindListByPid(item.Id);
                    item.Children = list;
                    return list;
                }
                else
                {
                    return new List<DbEntity>();
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetChildren", ex.ToString());
                return null;
            }
        }

        private List<DbModel.Location.AreaAndDev.DevInfo> GetLeafNodes(DbEntity area)
        {
            try
            {
                if (area != null)
                {
                    var list = db.DevInfos.GetListByPid(new List<int>() { area.Id });
                    area.LeafNodes = list;
                    return list;
                }
                else
                {
                    return new List<DbModel.Location.AreaAndDev.DevInfo>();
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetLeafNodes", ex.ToString());
                return null;
            }
        }

        private void GetChildrenTree(DbEntity entity)
        {
            try
            {
                if (entity == null) return;
                var list = GetChildren(entity);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        GetChildrenTree(item);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetChildrenTree", ex.ToString());

            }

        }

        public IList<TEntity> GetListByName(string name)
        {
            try
            {
                var list = dbSet.FindListByName(name);
                return list.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetListByName", ex.ToString());
                return null;
            }
        }

        public List<TEntity> GetListByPid(string pid)
        {
            try
            {
                var list = dbSet.FindListByPid(pid.ToInt());
                return list.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetListByPid", ex.ToString());
                return null;
            }
        }

        public AreaPoints GetPoints(string areaId)
        {
            try
            {
                TEntity area = GetEntity(areaId);
                AreaPoints points = new AreaPoints(area);
                return points;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetPoints", ex.ToString());
                return null;
            }
        }

        public List<AreaPoints> GetPointsByPid(string pid)
        {
            try
            {
                var areas = GetListByPid(pid);
                if (areas == null) return null;
                List<AreaPoints> list = new List<AreaPoints>();
                for (int i = 0; i < areas.Count; i++)
                {
                    TEntity area = areas[i];
                    AreaPoints points = new AreaPoints(area);
                    list.Add(points);
                }
                return list;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetPointsByPid", ex.ToString());
                return null;
            }
        }

        public TEntity GetEntity(string id)
        {
            return GetEntity(id, true);
        }

        public TEntity GetEntity(string id, bool getChildren)
        {
            try
            {
                var item = dbSet.Find(id.ToInt());
                if (item.InitBound == null)
                {
                    item.InitBound = db.Bounds.Find(item.InitBoundId);
                    if (item.InitBound != null)
                    {
                        item.InitBound.Points = db.Points.FindAll(i => i.BoundId == item.InitBoundId);
                    }
                }
                if (getChildren)
                {
                    GetChildren(item);
                    GetLeafNodes(item);
                }
                return item.ToTModel();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetEntity", ex.ToString());
                return null;
            }
        }

        public TEntity GetParent(string id)
        {
            try
            {
                var item = dbSet.Find(id.ToInt());
                if (item == null) return null;
                return GetEntity(item.ParentId + "");
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetParent", ex.ToString());
                return null;
            }
        }

        public TEntity Post(TEntity item)
        {
            try
            {
                var dbItem = item.ToDbModel();
                var result = dbSet.Add(dbItem);
                return result ? dbItem.ToTModel() : null;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "Post", ex.ToString());
                return null;
            }
        }

        public TEntity Post(string pid, TEntity item)
        {
            try
            {
                item.ParentId = pid.ToInt();
                var dbItem = item.ToDbModel();
                var result = dbSet.Add(dbItem);
                return result ? dbItem.ToTModel() : null;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "Post", ex.ToString());
                return null;
            }
        }

        public TEntity Put(TEntity item)
        {
            try
            {
                var dbItem = item.ToDbModel();
                var result = dbSet.Edit(dbItem);
                return result ? dbItem.ToTModel() : null;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "Put", ex.ToString());
                return null;
            }
        }

        public TEntity Delete(string id)
        {
            try
            {
                var item = dbSet.Find(id.ToInt());
                if (item == null) return null;
                GetChildren(item);
                if (item.Children != null && item.Children.Count > 0)//不能删除有子物体的节点
                {
                    //throw new Exception("Have Children !");
                }
                else
                {
                    dbSet.Remove(item);
                }
                return item.ToTModel();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "Delete", ex.ToString());
                return null;
            }
        }

        public IList<TEntity> DeleteListByPid(string pid)
        {
            try
            {
                var list2 = new List<DbEntity>();
                var list = dbSet.FindListByPid(pid.ToInt());
                foreach (var item in list)
                {
                    GetChildren(item);
                    if (item.Children == null || item.Children.Count == 0)
                    {
                        bool r = dbSet.Remove(item);//只删除无子物体的节点
                        if (r)
                        {
                            list2.Add(item);
                        }
                    }
                }
                return list2.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "DeleteListByPid", ex.ToString());
                return null;
            }
        }

        public string GetAreaSvgXml(int Id)
        {
            try
            {
                int Scale = 0;
                int Margin = 0;
                double DevSize = 0;
                double OffsetX = 0;
                double OffsetY = 0;
                double CanvaWidth = 0;
                double CanvaHeight = 0;
                string strReturn = "";
                string strXml = "";
                string strWidth = "";
                string strHeight = "";

                DbEntity area = dbSet.Find(Id);
                if (area == null)
                {
                    return strReturn;
                }

                int? ParentId = area.ParentId;
                DbEntity Parent = dbSet.Find(ParentId);
                if (Parent == null)
                {
                    return strReturn;
                }

                DbModel.Location.AreaAndDev.Bound bound = db.Bounds.Find(area.InitBoundId);
                if (bound == null)
                {
                    return strReturn;
                }



                List<DbModel.Location.AreaAndDev.Point> pointlist = db.Points.ToList();

                if (Parent.Name == "根节点") //电厂
                {
                    Scale = 3;
                    DevSize = 3;
                    Margin = 20;
                    OffsetX = bound.MinX - Margin;
                    OffsetY = bound.MinY - Margin;
                    CanvaWidth = (bound.MaxX - OffsetX + Margin) * Scale;
                    CanvaHeight = (bound.MaxY - OffsetY + Margin) * Scale;
                    strWidth = Convert.ToString(CanvaWidth);
                    strHeight = Convert.ToString(CanvaHeight);

                    strReturn = area.GetSvgXml(bound, pointlist, Scale, OffsetX, OffsetY, CanvaHeight);
                    var lst1 = dbSet.DbSet.Where(p => p.ParentId == Id).ToList();
                    if (lst1 != null)
                    {
                        foreach (var item1 in lst1)
                        {
                            DbModel.Location.AreaAndDev.Bound bound1 = db.Bounds.Find(item1.InitBoundId);
                            //if (bound1 == null)
                            //{
                            //    continue;
                            //}

                            strReturn += item1.GetSvgXml(bound1, pointlist, Scale, OffsetX, OffsetY, CanvaHeight);
                            var lst2 = dbSet.DbSet.Where(p => p.ParentId == item1.Id).ToList();
                            if (lst2 != null)
                            {
                                foreach (var item2 in lst2)
                                {
                                    DbModel.Location.AreaAndDev.Bound bound2 = db.Bounds.Find(item2.InitBoundId);
                                    //if (bound2 == null)
                                    //{
                                    //    continue;
                                    //}
                                    strReturn += item2.GetSvgXml(bound2, pointlist, Scale, OffsetX, OffsetY, CanvaHeight);
                                }
                            }
                        }
                    }
                }
                else if (area.Type == AreaTypes.楼层)
                {
                    Scale = 20;
                    DevSize = 0.3;
                    Margin = 10;
                    OffsetX = -Margin / 2;
                    OffsetY = -Margin / 2;
                    CanvaWidth = (bound.MaxX - OffsetX + Margin) * Scale;
                    CanvaHeight = (bound.MaxY - OffsetY + Margin) * Scale;
                    strWidth = Convert.ToString(CanvaWidth);
                    strHeight = Convert.ToString(CanvaHeight);

                    strReturn = area.GetSvgXml(bound, pointlist, Scale, OffsetX, OffsetY, CanvaHeight);
                    var lst1 = dbSet.DbSet.Where(p => p.ParentId == Id).ToList();
                    if (lst1 != null)
                    {
                        foreach (var item1 in lst1)
                        {
                            DbModel.Location.AreaAndDev.Bound bound1 = db.Bounds.Find(item1.InitBoundId);
                            //if (bound1 == null)
                            //{
                            //    continue;
                            //}

                            strReturn += item1.GetSvgXml(bound1, pointlist, Scale, OffsetX, OffsetY, CanvaHeight);
                        }
                    }
                }

                var devList = db.DevInfos.DbSet.Where(p => p.ParentId == Id).ToList();
                if (devList != null)
                {
                    foreach (var item3 in devList)
                    {
                        strReturn += item3.GetSvgXml(Scale, DevSize, OffsetX, OffsetY, CanvaHeight);
                    }
                }

                strXml = "<svg id=\"厂区\" width=\"" + strWidth + "\" height=\"" + strHeight + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" > ";
                strXml += "<defs><style>.cls-1{fill:none;stroke:#4d9fb5;}</style></defs>";
                strXml += strReturn;
                strXml += "</svg>";
                return strXml;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetAreaSvgXml", ex.ToString());
                return null;
            }
        }

        public Bound GetBound(TEntity area)
        {
            try
            {
                if (area.InitBound != null) return area.InitBound;
                var bound = new DbModel.Location.AreaAndDev.Bound();
                bool r1 = db.Bounds.Add(bound);
                area.InitBoundId = bound.Id;
                var newArea = new AreaService().Put(area);
                area.InitBound = bound.ToTModel();
                return area.InitBound;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetBound", ex.ToString());
                return null;
            }
        }

        public Point AddPoint(TEntity area, Point point)
        {
            try
            {
                var bound = GetBound(area);
                if (bound == null) return null;
                point.BoundId = bound.Id;
                //p1.Bound = Bound;
                var dbPoint = point.ToDbModel();
                bool r = db.Points.Add(dbPoint);
                if (r)
                {
                    point.Id = dbPoint.Id;
                    bound.AddPoint(point);
                    return point;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "AddPoint", ex.ToString());
                return null;
            }
        }

        public Point EditPoint(Point p1)
        {
            try
            {
                p1.Bound.SetMinMaxXY();
                var dbBound = db.Bounds.Find(p1.BoundId);
                dbBound.Update(p1.Bound);
                db.Bounds.Edit(dbBound);

                var dbPoint = db.Points.Find(p1.Id);
                dbPoint.Update(p1);
                bool r = db.Points.Edit(dbPoint);
                if (r)
                {
                    return p1;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "EditPoint", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 根据id，获取区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Area GetAreaById(int id)
        {
            try
            {
                return db.Areas.Find(i => i.Id == id);
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetAreaById", ex.ToString());
                return null;
            }
        }

        public AreaStatistics GetAreaStatistics(int id)
        {
            try
            {
                Log.Info(tag, ">>>>> GetAreaStatistics id=" + id);
                Stopwatch watch = new Stopwatch();
                watch.Start();

                if (id == 0)
                {
                    id = 2;
                    Log.Info(tag, "0=>2");//四会热电厂
                }
                var lst = db.Areas.GetAllSubAreas(id);//获取所有的子区域，和自身
                if (lst.Count == 0)
                {
                    Log.Error(tag, "Count == 0 !!");
                    return new AreaStatistics();
                }

                //AreaService asr = new AreaService();
                AreaStatistics ast = GetAreaStatisticsCount(lst);
                //AreaStatistics ast2 = GetAreaStatisticsCount();
                watch.Stop();
                TimeSpan time = watch.Elapsed;
                Log.Info(tag, "time:" + time);
                Log.Info(tag, "<<<<<< GetAreaStatistics id=" + id);
                return ast;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetAreaStatistics", ex.ToString());
                return null;
            }
        }
    }
}
