using System;
using System.Collections.Generic;
using System.Linq;
using Location.Model;
using Location.BLL;
using Location.IModel;
using Location.Model.topviewxp;
using Location.Model.DataObjects.ObjectAddList;
using Location.Model.LocationTables;
using Location.Model.Tool;
using Location.Model.Tools;
using LocationWCFService;
using LocationWCFService.ServiceHelper;
using Location.Model.Manage;

namespace LocationWCFServices
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“LocationService”。
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class LocationService : ILocationService
    {
        LocationBll db = new LocationBll(false,false);

        U3DPositionSP u3dositionSP;
        public U3DPositionSP U3dositionSP
        {
            get
            {
                if (u3dositionSP == null)
                {
                    u3dositionSP = new U3DPositionSP();
                }
                return u3dositionSP;
            }

            set
            {
                u3dositionSP = value;
            }
        }

        PhysicalTopologySP physicalTopologySP;
        public PhysicalTopologySP PhysicalTopologySP
        {
            get
            {
                if (physicalTopologySP == null)
                {
                    physicalTopologySP = new PhysicalTopologySP();
                }
                return physicalTopologySP;
            }

            set
            {
                physicalTopologySP = value;
            }
        }


        public void DoWork()
        {
        }

        public IList<Area> GetAreas()
        {
            return db.Areas.ToList();
        }

        //public IList<Map> GetMaps()
        //{
        //    IList<Map> maps = db.Maps.ToList();
        //    foreach (var item in maps)
        //    {
        //        if (item.Areas.Count == 0)
        //        {
        //            item.Areas = null;
        //        }
        //    }
        //    return maps;
        //}

        public IList<User> GetUsers()
        {
            return db.Users.ToList();
        }

        public IList<Tag> GetTags()
        {
            IList<Tag> tags = db.Tags.ToList();
            if (tags.Count == 0)
            {
                return null;
            }
            return tags;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public bool AddTags(List<Tag> tags)
        {
            bool r= db.Tags.AddRange(tags);
            db.AddTagPositionsByTags(tags);
            return r;
        }

        /// <summary>
        /// 删除某一个标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTag(int id)
        {
            return db.Tags.DeleteById(id);
        }

        /// <summary>
        /// 清空标签数据库表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAllTags()
        {
            bool r = true;
            try
            {
                string sql = "delete from Tags";
                //if (!string.IsNullOrEmpty(sql))
                db.Db.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception ex)
            {
                r = false;
            }
            return r;

            //return db.Areas.Delete(id);
        }

        /// <summary>
        /// 获取标签实时位置
        /// </summary>
        /// <returns></returns>
        public IList<TagPosition> GetRealPositons()
        {
            try
            {
                IList<TagPosition> list = db.TagPositions.ToList();
                if (list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取标签实时位置
        /// </summary>
        /// <returns></returns>
        public IList<TagPosition> GetRealPositonsByTags(List<string> tagCodes)
        {
            try
            {
                IList<TagPosition> list = db.TagPositions.DbSet.Where(tag => tagCodes.Contains(tag.Tag)).ToList();
                if (list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取标签历史位置
        /// </summary>
        /// <returns></returns>
        public IList<Position> GetHistoryPositons()
        {
            IList<Position> list = db.Position.ToList();
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

        public User GetUser()
        {
            return new User() { Id = 1, Name = "Name" };
        }

        public Map GetMap(int id)
        {
            return new Map() { Id = 1, Name = "Map1" };
        }

        public string Hello(string msg)
        {
            Log.Info("[Hello] msg:" + msg);
            return "Hello:"+msg;
        }

        public IList<Department> GetDepartmentList()
        {
            //List<Department> list = new List<Department>();
            //list.Add(new Department() {Id = 1, Name = "1"});
            //list.Add(new Department() {Id = 2, Name = "2"});
            //list.Add(new Department() {Id = 3, Name = "3"});
            //return list;

            IList<Department> list = db.Departments.ToList();
            IList<Department> list2 = list.CloneTreeListEx<Department, Personnel>();
            return list2;

            //bool r = list[0].Children[0] == list[1]; //r==true，代表，数据查询应该是一次获取表中的所有数据，然后组装成树的
            //return list;
        }

        public Department GetDepartmentTree()
        {
            try
            {
                //Department root = new Department() {Id = 1, Name = "root"};
                //root.Children = new List<Department>();
                //root.Children.Add(new Department() {Id = 2, Name = "c1"});
                //root.Children.Add(new Department() {Id = 3, Name = "c2"});
                //return root;

                //IList<Department> list = db.Departments.ToList();
                //Department root = list[0];
                //Department rootClone = root.CloneTreeRootEx<Department, Personnel>();
                //return rootClone;


                List<Department> list = db.Departments.ToList();
                List<Personnel> leafNodes = GetPersonList();
                List<Department> roots = TreeHelper.CreateTree(list, leafNodes);
                if (roots.Count > 0)
                {
                    return roots[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public IList<Map> GetMaps(int? depId)
        {
            IList<Map> maps = null;
            if (depId != null)
            {
                maps = db.Maps.DbSet.Where(p => p.DepId == depId).ToList();
            }
            else
            {
                maps = db.Maps.ToList();
            }

            foreach (var item in maps)
            {
                if (item.Areas.Count == 0)
                {
                    item.Areas = null;
                }
            }

            if (maps.Count == 0)
            {
                maps = null;
            }

            return maps;
        }

        public IList<Area> FindAreas(string name)
        {
            return db.Areas.DbSet.Where(p => p.Name == name).ToList();
        }

        public IList<Area> GetAreas(int mapId)
        {
            return db.Areas.DbSet.Where(p => p.MapId == mapId).ToList();
        }

        public Area GetArea(int id)
        {
            return db.Areas.Find(id);
        }

        public bool AddArea(Area area)
        {
            return db.Areas.Add(area);
        }

        public bool EditArea(Area area)
        {
            db.TransformMs.Edit(area.Transform);
            return db.Areas.Edit(area);
        }

        public bool DeleteArea(int id)
        {
            return db.Areas.DeleteById(id);
        }

        public IList<Position> GetHistoryPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long startTotalMilliseconds = (long)(start - startTime).TotalMilliseconds;
            long endTotalMilliseconds = (long)(end - startTime).TotalMilliseconds;
            if (startTotalMilliseconds >= endTotalMilliseconds)
            {
                return null;
            }
            if (startTotalMilliseconds < 0)
            {
                startTotalMilliseconds = 0;
            }
            if (endTotalMilliseconds < 0)
            {
                endTotalMilliseconds = 0;
            }

            IQueryable<Position> info = from u in db.Position.DbSet
                                        where tagcode == u.Tag && u.Time >= startTotalMilliseconds && u.Time <= endTotalMilliseconds
                                        select u;
            //var info = db.Position.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
            IList<Position> tempList = info.ToList();
            if (tempList.Count == 0)
            {
                tempList = null;
            }
            return tempList;
        }

        public static Action<string> ShowLog_Action;



        public string GetStrs(int n)
        {
            DateTime datetimeStart = DateTime.Now;
            string s = "";
            for (int i = 0; i < n; i++)
            {
                s += "A";
            }
            DateTime datetimeEnd = DateTime.Now;
            float t = (float)(datetimeEnd - datetimeStart).TotalSeconds;
            //ShowLog(t.ToString());
            return s;
        }

        private void ShowLog(string txt)
        {
            if (ShowLog_Action != null)
            {
                ShowLog_Action(txt);
            }
        }

        //Mutex m = new Mutex(false);
        /// <summary>
        /// 3D保存历史数据
        /// </summary>
        //public void AddU3DPosition(U3DPosition p)
        //{
        //    try
        //    {
        //        Log.Info("AddU3DPosition:" + p.Tag);
        //        if (u3dositionSP == null)
        //        {
        //            u3dositionSP = new U3DPositionSP();
        //        }
        //        List<U3DPosition> list = new List<U3DPosition>();
        //        list.Add(p);
        //        u3dositionSP.AddU3DPositions(list);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("AddU3DPosition", ex);
        //    }
        //}

        /// <summary>
        /// 3D保存历史数据
        /// </summary>
        public void AddU3DPosition(List<U3DPosition> pList)
        {
            try
            {
                //Log.Info("AddU3DPosition:" + p.Tag);
                if (U3dositionSP == null)
                {
                    U3dositionSP = new U3DPositionSP();
                }
                //List<U3DPosition> list = new List<U3DPosition>();
                //list.Add(p);
                U3dositionSP.AddU3DPositions(pList);
            }
            catch (Exception ex)
            {
                Log.Error("AddU3DPosition", ex);
            }
        }

        public void AddU3DPositions(List<U3DPosition> list)
        {
            try
            {
                Log.Info("AddU3DPositions");
                if (U3dositionSP == null)
                {
                    U3dositionSP = new U3DPositionSP();
                }
                U3dositionSP.AddU3DPositions(list);
            }
            catch (Exception ex)
            {
                Log.Error("AddU3DPositions", ex);
            }
        }

        public void AddU3DPos(UPos pos)
        {
            try
            {
                Log.Info("AddU3DPos:" + pos);
            }
            catch (Exception ex)
            {
                Log.Error("AddU3DPos", ex);
            }
        }

        /// <summary>
        ///  获取标签3D历史位置
        /// </summary>
        /// <param name="tagcode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long startTotalMilliseconds = (long)(start - startTime).TotalMilliseconds;
            long endTotalMilliseconds = (long)(end - startTime).TotalMilliseconds;
            if (startTotalMilliseconds >= endTotalMilliseconds)
            {
                return null;
            }
            if (startTotalMilliseconds < 0)
            {
                startTotalMilliseconds = 0;
            }
            if (endTotalMilliseconds < 0)
            {
                endTotalMilliseconds = 0;
            }

            IQueryable<U3DPosition> info = from u in db.U3DPositions.DbSet
                                           where tagcode == u.Tag && u.Time >= startTotalMilliseconds && u.Time <= endTotalMilliseconds
                                           select u;
            //var info = db.Position.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
            IList<U3DPosition> tempList = info.ToList();
            if (tempList.Count == 0)
            {
                tempList = null;
            }
            return tempList;
        }


        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetPhysicalTopologyList()
        {
            IList<PhysicalTopology> list = db.PhysicalTopologys.ToList();
            IList<PhysicalTopology> list2 = list.CloneTreeListEx<PhysicalTopology, DevInfo>();
            return list2;
        }

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public PhysicalTopology GetPhysicalTopologyTree()
        {

            try
            {
                List<PhysicalTopology> list = db.PhysicalTopologys.ToList();
                //PhysicalTopology root = list[0];
                //PhysicalTopology rootClone = root.CloneTreeRootEx<PhysicalTopology,DevInfo>();
                //return rootClone;

                List<DevInfo> leafNodes = db.DevInfos.ToList();
                List<PhysicalTopology> roots= TreeHelper.CreateTree(list, leafNodes);
                if (roots.Count > 0)
                {
                    return roots[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }



        /// <summary>
        /// 获取园区下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetParkMonitorRange()
        {
            IList<PhysicalTopology> results = PhysicalTopologySP.GetParkMonitorRange();
            if (results.Count == 0)
            {
                return null;
            }
            return results;
        }

        /// <summary>
        /// 获取楼层下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetFloorMonitorRange()
        {
            IList<PhysicalTopology> results = PhysicalTopologySP.GetFloorMonitorRange();
            if (results.Count == 0)
            {
                return null;
            }
            return results;
        }

        /// <summary>
        /// 根据PhysicalTopology的Id获取楼层以下级别的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetFloorMonitorRangeById(int id)
        {            
            IList<PhysicalTopology> results = PhysicalTopologySP.GetFloorMonitorRange(id);
            if (results.Count == 0)
            {
                return null;
            }
            return results;
        }

        /// <summary>
        /// 根据节点添加监控范围
        /// </summary>
        public bool EditMonitorRange(PhysicalTopology pt)
        {
            db.TransformMs.Edit(pt.Transfrom);
            return db.PhysicalTopologys.Edit(pt);
        }

        /// <summary>
        /// 根据节点添加子监控范围
        /// </summary>
        public bool AddMonitorRange(PhysicalTopology pt)
        {
            return db.PhysicalTopologys.Add(pt);
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        public List<Personnel> GetPersonList()
        {
            List<Personnel> list = db.Personnels.ToList();
            List<Post> postList=db.Posts.ToList();
            foreach (Personnel item in list)
            {
                item.Pst = postList.Find(i => i.Id == item.PstId);
            }
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

        /// <summary>
        /// 获取模型添加的信息列表
        /// </summary>
        /// <returns></returns>
        public ObjectAddList GetObjectAddList()
        {
            List<t_SetModel> modelList = db.t_SetModels.ToList();
            List<t_Template_TypeProperty> typeList = db.t_TypeProperties.ToList();
            ObjectAddListService service = new ObjectAddListService();
            return service.GetObjectAddListEx(modelList,typeList);          
        }

        /// <summary>
        /// 获取所有设备位置信息
        /// </summary>
        /// <returns></returns>
        public IList<DevPos> GetDevPositions()
        {
            List<DevPos> posList = db.DevPos.ToList();
            posList = posList.Count == 0 ? null : posList;
            return posList;
        }
        /// <summary>
        /// 添加一条设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        public bool AddDevPosInfo(DevPos pos)
        {
           return db.DevPos.Add(pos);
        }
        /// <summary>
        /// 修改设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool ModifyPosInfo(DevPos pos)
        {
            return db.DevPos.Edit(pos);
        }
        /// <summary>
        /// 删除设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool DeletePosInfo(DevPos pos)
        {
            return db.DevPos.DeleteById(pos);
        }
        /// <summary>
        /// 添加设备位置信息（列表形式）
        /// </summary>
        /// <param name="posList"></param>
        public bool AddDevPosByList(IList<DevPos> posList)
        {
            return db.DevPos.AddRange(posList.ToList());
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> GetDevInfos()
        {
            List<DevInfo> devInfoList = db.DevInfos.ToList();
            devInfoList = devInfoList.Count == 0 ? null : devInfoList;
            return devInfoList;
        }
        /// <summary>
        /// 添加一条设备基本信息
        /// </summary>
        /// <param name="devInfo"></param>
        public bool AddDevInfo(DevInfo devInfo)
        {
            return db.DevInfos.Add(devInfo);
        }
        /// <summary>
        /// 添加设备基本信息（列表形式）
        /// </summary>
        /// <param name="devInfoList"></param>
        public bool AddDevInfoByList(IList<DevInfo> devInfoList)
        {
            return db.DevInfos.AddRange(devInfoList.ToList());
        }
        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        public bool ModifyDevInfo(DevInfo devInfo)
        {
            return db.DevInfos.Edit(devInfo);
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        public bool DeleteDevInfo(DevInfo devInfo)
        {
            return db.DevInfos.DeleteById(devInfo);
        }
        /// <summary>
        /// 通过区域ID,获取区域下所有设备
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IList<DevInfo> GetDevInfoByID(int? pid)
        {
            List<DevInfo> devInfoList = db.DevInfos.ToList().FindAll(item=>item.ParentId== pid);
            devInfoList = devInfoList.Count == 0 ? null : devInfoList;
            devInfoList = devInfoList.Count == 0 ? null : devInfoList;
            return devInfoList;
        }

        public KKSCode GetKKSInfoByNodeId(int id)
        {
            var query = from nk in db.NodeKKSs.DbSet join kks in db.KKSCodes.DbSet on nk.KKS equals kks.Code where nk.NodeId ==id select kks;
            return query.FirstOrDefault();
        }

        public KKSCode GetKKSInfoByCode(string code)
        {
            return db.KKSCodes.DbSet.FirstOrDefault(i=>i.Code==code);
        }

        public KKSCode FindKKSInfoByName(string name)
        {
            return db.KKSCodes.DbSet.FirstOrDefault(i => i.Name.Contains(name));
        }
        public string FindKKSCodeByName(string name)
        {
            KKSCode kks = db.KKSCodes.DbSet.FirstOrDefault(i => i.Name.Contains(name));
            if (kks == null)
            {
                return "";
            }
            else
            {
                return kks.Code;
            }
        }

        public string GetKKSCodeByNodeId(int id)
        {
            NodeKKS kks = db.NodeKKSs.DbSet.FirstOrDefault(i => i.NodeId==id);
            if (kks == null)
            {
                return "";
            }
            else
            {
                return kks.KKS;
            }
        }


    }
}
