using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BLL;
using BLL.ServiceHelpers;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.ServiceHelpers;
using Location.Model.DataObjects.ObjectAddList;
using Location.TModel.FuncArgs;
using Location.TModel.Location;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using Location.TModel.Location.Obsolete;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.Person;
using Location.TModel.LocationHistory.Data;
using LocationServices.Converters;
using LocationServices.Tools;
using LocationWCFService;
using LocationWCFService.ServiceHelper;
using ConfigArg = Location.TModel.Location.AreaAndDev.ConfigArg;
using DevInfo = Location.TModel.Location.AreaAndDev.DevInfo;
using KKSCode = Location.TModel.Location.AreaAndDev.KKSCode;
using Post = Location.TModel.Location.AreaAndDev.Post;
using Dev_DoorAccess = Location.TModel.Location.AreaAndDev.Dev_DoorAccess;
using TModel.Location.Work;
using TModel.LocationHistory.Work;

namespace LocationServices.Locations
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“LocationService”。
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class LocationService : ILocationService, IDisposable
    {
        private Bll db = new Bll(false, false, false);

        public static U3DPositionSP u3dositionSP;

        public void DoWork()
        {
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
            //return db.Users.ToList().ToWcfModelList();
            return null;
        }

        public IList<Tag> GetTags()
        {
            var tags = db.LocationCards.ToList();
            return tags.ToWcfModelList();
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public bool AddTags(List<Tag> tags)
        {
            var list = tags.ToDbModel();
            bool r = db.LocationCards.AddRange(list);
            db.AddTagPositionsByTags(list);
            return r;
        }

        /// <summary>
        /// 删除某一个标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTag(int id)
        {
            return db.LocationCards.DeleteById(id);
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
                var list = db.LocationCardPositions.ToList();
                foreach (var item in list)
                {
                    if (item.Archors != null && item.Archors.Count == 0)
                    {
                        item.Archors = null;
                    }
                }
                return list.ToWcfModelList();
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
                var list = db.LocationCardPositions.DbSet.Where(tag => tagCodes.Contains(tag.Code)).ToList();
                return list.ToWcfModelList();
            }
            catch (Exception ex)
            {
                return null;
            }
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
            return "Hello:" + msg;
        }

        public IList<Department> GetDepartmentList()
        {
            var list = db.Departments.ToList();
            var list2 = list.ToTModel();
            return list2.ToWCFList();
        }

        public Department GetDepartmentTree()
        {
            try
            {
                var list = db.Departments.ToList().ToTModel();
                var leafNodes = GetPersonList();
                var roots = TreeHelper.CreateTree(list, leafNodes);
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
            //return null;
        }

        //public IList<Map> GetMaps(int? depId)
        //{
        //    IList<Map> maps = null;
        //    if (depId != null)
        //    {
        //        maps = db.Maps.DbSet.Where(p => p.DepId == depId).ToList();
        //    }
        //    else
        //    {
        //        maps = db.Maps.ToList();
        //    }

        //    foreach (var item in maps)
        //    {
        //        if (item.Areas.Count == 0)
        //        {
        //            item.Areas = null;
        //        }
        //    }
        //    return maps.ToWcfModelList();
        //}

        //public IList<Area> GetAreas()
        //{
        //    return db.Areas.ToList().ToWcfModelList();
        //}

        //public IList<Area> FindAreas(string name)
        //{
        //    return db.Areas.DbSet.Where(p => p.Name == name).ToList().ToWcfModelList();
        //}

        //public IList<Area> GetAreas(int mapId)
        //{
        //    return db.Areas.DbSet.Where(p => p.MapId == mapId).ToList().ToWcfModelList();
        //}

        //public Area GetArea(int id)
        //{
        //    return db.Areas.Find(id);
        //}

        //public bool AddArea(Area area)
        //{
        //    return db.Areas.Add(area);
        //}

        //public bool EditArea(Area area)
        //{
        //    db.TransformMs.Edit(area.Transform);
        //    return db.Areas.Edit(area);
        //}

        //public bool DeleteArea(int id)
        //{
        //    return db.Areas.DeleteById(id);
        //}

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


        /// <summary>
        /// 3D保存历史数据
        /// </summary>
        public void AddU3DPosition(List<U3DPosition> list)
        {
            try
            {
                //if (list == null || list.Count == 0) return;
                if (u3dositionSP == null)
                {
                    u3dositionSP = new U3DPositionSP();
                }
                u3dositionSP.AddU3DPositions(list.ToDbModel());
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
                //if(list==null|| list.Count==0)return;
                //Log.Info("AddU3DPositions");
                if (u3dositionSP == null)
                {
                    u3dositionSP = new U3DPositionSP();
                }
                u3dositionSP.AddU3DPositions(list.ToDbModel());
            }
            catch (Exception ex)
            {
                Log.Error("AddU3DPositions", ex);
            }
        }

        //public void AddU3DPos(UPos pos)
        //{
        //    try
        //    {
        //        Log.Info("AddU3DPos:" + pos);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("AddU3DPos", ex);
        //    }
        //}




        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetPhysicalTopologyList()
        {
            var list = db.Areas.ToList();
            return list.ToWcfModelList();
        }

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public PhysicalTopology GetPhysicalTopologyTree()
        {
            try
            {
                Area root0 = LocationSP.GetPhysicalTopologyTree();
                PhysicalTopology root = root0.ToTModel();
                //string xml = XmlSerializeHelper.GetXmlText(root, Encoding.UTF8);
                //PhysicalTopology obj = XmlSerializeHelper.LoadFromText<PhysicalTopology>(xml);
                return root;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }



        /// <summary>
        /// 获取园区下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetParkMonitorRange()
        {
            PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
            var results = physicalTopologySp.GetParkMonitorRange();
            return results.ToWcfModelList();
        }

        /// <summary>
        /// 获取楼层下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetFloorMonitorRange()
        {
            PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
            var results = physicalTopologySp.GetFloorMonitorRange();
            return results.ToWcfModelList();
        }

        /// <summary>
        /// 根据PhysicalTopology的Id获取楼层以下级别的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetFloorMonitorRangeById(int id)
        {
            PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
            var results = physicalTopologySp.GetFloorMonitorRange(id);
            return results.ToWcfModelList();
        }

        /// <summary>
        /// 根据节点添加监控范围
        /// </summary>
        public bool EditMonitorRange(PhysicalTopology pt)
        {
            //db.TransformMs.Edit(pt.Transfrom);
            return db.Areas.Edit(pt.ToDbModel());
        }

        /// <summary>
        /// 根据节点添加子监控范围
        /// </summary>
        public bool AddMonitorRange(PhysicalTopology pt)
        {
            return db.Areas.Add(pt.ToDbModel());
        }

        /// <summary>
        /// 获取模型添加的信息列表
        /// </summary>
        /// <returns></returns>
        public ObjectAddList GetObjectAddList()
        {
            var modelList = db.DevModels.ToList();
            var typeList = db.DevTypes.ToList();
            ObjectAddListService service = new ObjectAddListService();
            return service.GetObjectAddListEx(modelList, typeList);
        }

        #region 设备信息、位置信息
        /// <summary>
        /// 获取所有设备位置信息
        /// </summary>
        /// <returns></returns>
        public IList<DevPos> GetDevPositions()
        {
            List<DevPos> posList = new List<DevPos>();
            var devs = db.DevInfos.ToList();
            foreach (DbModel.Location.AreaAndDev.DevInfo dev in devs)
            {
                posList.Add(dev.GetPos());
            }
            return posList.ToWCFList();
        }

        /// <summary>
        /// 添加一条设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        public bool AddDevPosInfo(DevPos pos)
        {
            return ModifyPosInfo(pos);
        }
        /// <summary>
        /// 修改设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool ModifyPosInfo(DevPos pos)
        {
            var devInfo = db.DevInfos.DbSet.FirstOrDefault(i => i.Local_DevID == pos.DevID);
            if (devInfo != null)
            {
                devInfo.SetPos(pos);
                return db.DevInfos.Edit(devInfo);
            }
            return false;
        }
        /// <summary>
        /// 修改设备位置信息
        /// </summary>
        /// <param name="posList"></param>
        /// <returns></returns>
        public bool ModifyPosByList(List<DevPos> posList)
        {
            //return db.DevPos.EditRange(db.Db, posList);
            foreach (var devPose in posList)
            {
                ModifyPosInfo(devPose);
            }
            return true;
        }
        /// <summary>
        /// 删除设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool DeletePosInfo(DevPos pos)
        {
            //return db.DevPos.DeleteById(pos.DevID);
            return false;
        }
        /// <summary>
        /// 添加设备位置信息（列表形式）
        /// </summary>
        /// <param name="posList"></param>
        public bool AddDevPosByList(List<DevPos> posList)
        {
            //return db.DevPos.AddRange(posList.ToList());
            return ModifyPosByList(posList);
        }


        private void BindingDev(List<DevInfo> devInfoList)
        {
            DeviceInfoService service = new DeviceInfoService();
            //service.BindingDevPos(devInfoList, db.DevPos.ToList());
            service.BindingDevParent(devInfoList, db.Areas.ToList().ToTModel());
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> GetAllDevInfos()
        {
            //List<DevInfo> devInfoList = db.DevInfos.ToList();
            //return devInfoList.ToWcfModelList(); 

            var devInfoList = db.DevInfos.DbSet.ToList().ToTModel();
            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> GetDevInfos(int[] typeList)
        {
            //List<DevInfo> devInfoList = db.DevInfos.ToList();
            //return devInfoList.ToWcfModelList(); 

            List<DevInfo> devInfoList = null;
            if (typeList == null || typeList.Length == 0)
            {
                devInfoList = db.DevInfos.ToList().ToTModel();
            }
            else
            {
                devInfoList = db.DevInfos.DbSet.Where(i => typeList.Contains(i.Local_TypeCode)).ToList().ToTModel();
            }

            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> FindDevInfos(string key)
        {
            //List<DevInfo> devInfoList = db.DevInfos.ToList();
            //return devInfoList.ToWcfModelList();           
            var devInfoList = db.DevInfos.DbSet.Where(i => i.Name.Contains(key)).ToList().ToTModel();
            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
        }

        /// <summary>
        /// 添加一条设备基本信息
        /// </summary>
        /// <param name="devInfo"></param>
        public DevInfo AddDevInfo(DevInfo devInfo)
        {
            DbModel.Location.AreaAndDev.DevInfo DbDevInfo = devInfo.ToDbModel();
            bool r = db.DevInfos.Add(DbDevInfo);
            return DbDevInfo.ToTModel();
        }
        /// <summary>
        /// 添加设备基本信息（列表形式）
        /// </summary>
        /// <param name="devInfoList"></param>
        public bool AddDevInfoByList(List<DevInfo> devInfoList)
        {
            return db.DevInfos.AddRange(devInfoList.ToDbModel());
        }
        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        public bool ModifyDevInfo(DevInfo devInfo)
        {
            return db.DevInfos.Edit(devInfo.ToDbModel());
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        public bool DeleteDevInfo(DevInfo devInfo)
        {
            bool devResult = db.DevInfos.DeleteById(devInfo.Id);
            //bool posResult = db.DevPos.DeleteById(devInfo.Local_DevID);
            bool value = devResult;
            return value;
        }
        /// <summary>
        /// 通过区域ID,获取区域下所有设备
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IList<DevInfo> GetDevInfoByParent(int[] pidList)
        {
            List<DevInfo> devInfoList = new List<DevInfo>();
            foreach (var pid in pidList)
            {
                devInfoList.AddRange(db.DevInfos.DbSet.Where(item => item.ParentId == pid).ToList().ToTModel());
                //BindingDev(devInfoList);
            }
            return devInfoList.ToWCFList();
        }
        /// <summary>
        /// 添加门禁设备
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        public bool AddDoorAccessByList(IList<Dev_DoorAccess> doorAccessList)
        {
            return db.Dev_DoorAccess.AddRange(doorAccessList.ToList().ToDbModel());
        }
        /// <summary>
        /// 添加门禁信息
        /// </summary>
        /// <param name="doorAccess"></param>
        /// <returns></returns>
        public bool AddDoorAccess(Dev_DoorAccess doorAccess)
        {
            return db.Dev_DoorAccess.Add(doorAccess.ToDbModel());
        }
        /// <summary>
        /// 删除门禁信息
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        public bool DeleteDoorAccess(IList<Dev_DoorAccess> doorAccessList)
        {
            bool value = true;
            foreach (Dev_DoorAccess item in doorAccessList)
            {
                bool accessResult = db.Dev_DoorAccess.DeleteById(item.Id);
                bool devResult = db.DevInfos.DeleteById(item.DevID);
                //bool posResult = db.DevPos.DeleteById(item.DevID);
                bool valueTemp = devResult && accessResult ? true : false;
                if (!valueTemp) value = valueTemp;
            }
            return value;
        }
        /// <summary>
        /// 修改门禁信息
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        public bool ModifyDoorAccess(IList<Dev_DoorAccess> doorAccessList)
        {
            return db.Dev_DoorAccess.EditRange(db.Db, doorAccessList.ToList().ToDbModel());
        }
        /// <summary>
        /// 通过区域ID,获取区域下所有门禁信息
        /// </summary>
        /// <param name="pid">区域ID</param>
        /// <returns></returns>
        public IList<Dev_DoorAccess> GetDoorAccessInfoByParent(int[] pidList)
        {
            List<Dev_DoorAccess> devInfoList = new List<Dev_DoorAccess>();
            foreach (var pId in pidList)
            {
                devInfoList.AddRange(db.Dev_DoorAccess.DbSet.Where(item => item.ParentId == pId).ToList().ToTModel());
            }
            return devInfoList.ToWCFList();
        }
        #endregion

        public KKSCode GetKKSInfoByNodeId(int id)
        {
            var query = from nk in db.NodeKKSs.DbSet join kks in db.KKSCodes.DbSet on nk.KKS equals kks.Code where nk.NodeId == id select kks;
            return query.FirstOrDefault().ToTModel();
        }

        public KKSCode GetKKSInfoByCode(string code)
        {
            return db.KKSCodes.DbSet.FirstOrDefault(i => i.Code == code).ToTModel();
        }

        public KKSCode FindKKSInfoByName(string name)
        {
            return db.KKSCodes.DbSet.FirstOrDefault(i => i.Name.Contains(name)).ToTModel();
        }
        public string FindKKSCodeByName(string name)
        {
            var kks = db.KKSCodes.DbSet.FirstOrDefault(i => i.Name.Contains(name));
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
            var kks = db.NodeKKSs.DbSet.FirstOrDefault(i => i.NodeId == id);
            if (kks == null)
            {
                return "";
            }
            else
            {
                return kks.KKS;
            }
        }

        public void Dispose()
        {

        }

        #region ConfigArg
        public bool AddConfigArg(ConfigArg config)
        {
            return db.ConfigArgs.Add(config.ToDbModel());
        }

        public bool EditConfigArg(ConfigArg config)
        {
            return db.ConfigArgs.Edit(config.ToDbModel());
        }

        public bool DeleteConfigArg(ConfigArg config)
        {
            return db.ConfigArgs.DeleteById(config.Id);
        }

        public ConfigArg GetConfigArg(int id)
        {
            return db.ConfigArgs.Find(id).ToTModel();
        }

        public ConfigArg GetConfigArgByKey(string key)
        {
            return db.ConfigArgs.GetConfigArgByKey(key).ToTModel();
        }

        public List<ConfigArg> FindConfigArgListByKey(string key)
        {
            return db.ConfigArgs.FindConfigArgListByKey(key).ToWcfModelList();
        }

        public List<ConfigArg> FindConfigArgListByClassify(string key)
        {
            return db.ConfigArgs.FindConfigArgListByClassify(key).ToWcfModelList();
        }

        public TransferOfAxesConfig GetTransferOfAxesConfig()
        {
            var args = db.ConfigArgs.GetTransferOfAxesConfig();
            TransferOfAxesConfig config = new TransferOfAxesConfig();
            config.Zero = args.Find(i => i.Key == "TransferOfAxes.Zero").ToTModel();
            config.Scale = args.Find(i => i.Key == "TransferOfAxes.Scale").ToTModel();
            config.Direction = args.Find(i => i.Key == "TransferOfAxes.Direction").ToTModel();
            return config;
        }

        public bool SetTransferOfAxesConfig(TransferOfAxesConfig config)
        {
            return db.ConfigArgs.SetTransferOfAxesConfig(config.Zero.ToDbModel(), config.Scale.ToDbModel(),
                config.Direction.ToDbModel());
        }

        public List<ConfigArg> GetConfigArgList()
        {
            return db.ConfigArgs.ToList().ToWcfModelList();
        }

        #endregion

        #region 人员定位历史数据

        /// <summary>
        /// 获取标签历史位置
        /// </summary>
        /// <returns></returns>
        public IList<Position> GetHistoryPositons()
        {
            var list = db.Positions.ToList();
            return list.ToWcfModelList();
        }

        /// <summary>
        /// 获取历史位置信息根据PersonnelID
        /// </summary>
        /// <param name="tagcode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<Position> GetHistoryPositonsByPersonnelID(int personnelID, DateTime start, DateTime end)
        {
            return LocationSP.GetHistoryPositonsByPersonnelID(personnelID, start, end).ToTModel();
        }

        /// <summary>
        /// 获取历史位置信息根据PersonnelID和TopoNodeId建筑id列表(人员所在的区域)
        /// </summary>
        /// <param name="tagcode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<Position> GetHistoryPositonsByPidAndTopoNodeIds(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            if (topoNodeIds == null || topoNodeIds.Count == 0)
            {
                return LocationSP.GetHistoryPositonsByPersonnelID(personnelID, start, end).ToTModel();
            }
            else
            {
                return LocationSP.GetHistoryPositonsByPidAndTopoNodeIds(personnelID, topoNodeIds, start, end).ToTModel();
            }
        }

        /// <summary>
        /// 获取历史位置信息
        /// </summary>
        /// <param name="tagcode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<Position> GetHistoryPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            return LocationSP.GetHistoryPositonsByTime(tagcode, start, end).ToTModel();
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
            return LocationSP.GetHistoryU3DPositonsByTime(tagcode, start, end).ToTModel();
        }

        #endregion

        #region 人员列表

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        public List<Personnel> GetPersonList()
        {
            var list = db.Personnels.ToList();
            var tagToPersons = db.LocationCardToPersonnels.ToList();
            var postList = db.Posts.ToList();//职位
            var tagList = db.LocationCards.ToList();//关联标签
            var departList = db.Departments.ToList();//部门
            var ps = list.ToTModel();
            foreach (var p in ps)
            {
                var ttp = tagToPersons.Find(i => i.PersonnelId == p.Id);
                if (ttp != null)
                {
                    p.Tag = tagList.Find(i => i.Id == ttp.LocationCardId).ToTModel();
                    p.TagId = ttp.LocationCardId;
                }
                //p.Tag = tagList.Find(i => i.Id == p.TagId).ToTModel();
                p.Parent = departList.Find(i => i.Id == p.ParentId).ToTModel();
            }
            return ps.ToWCFList();
        }

        public List<Personnel> FindPersonList(string name)
        {
            return db.Personnels.FindListByName(name).ToWcfModelList();
        }

        public Personnel GetPerson(int id)
        {
            return db.Personnels.Find(id).ToTModel();
        }

        public int AddPerson(Personnel p)
        {
            bool r = db.Personnels.Add(p.ToDbModel());
            if (r == false)
            {
                return -1;
            }
            return p.Id;
        }

        public bool EditPerson(Personnel p)
        {
            return db.Personnels.Edit(p.ToDbModel());
        }

        public bool DeletePerson(int id)
        {
            return db.Personnels.DeleteById(id);
        }

        public List<LocationAlarm> GetLocationAlarms(AlarmSearchArg arg)
        {
            List<Personnel> ps = GetPersonList();
            List<LocationAlarm> alarms = new List<LocationAlarm>();
            alarms.Add(new LocationAlarm() { Id = 0, TagId = 1, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)4, Content = "进入无权限区域", CreateTime = new DateTime(2018, 9, 1, 10, 5, 34) }.SetPerson(ps[0]));
            alarms.Add(new LocationAlarm() { Id = 1, TagId = 2, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)3, Content = "靠近高风险区域", CreateTime = new DateTime(2018, 9, 4, 15, 5, 11) }.SetPerson(ps[1]));
            alarms.Add(new LocationAlarm() { Id = 2, TagId = 3, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)2, Content = "进入高风险区域", CreateTime = new DateTime(2018, 9, 7, 13, 35, 20) }.SetPerson(ps[2]));
            alarms.Add(new LocationAlarm() { Id = 3, TagId = 4, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)1, Content = "进入危险区域", CreateTime = new DateTime(2018, 9, 10, 16, 15, 44) }.SetPerson(ps[3]));
            return alarms;
        }

        public List<Post> GetPostList()
        {
            var posts = db.Posts.ToList();
            return posts.ToWcfModelList();
        }

        public List<DeviceAlarm> GetDeviceAlarms(AlarmSearchArg arg)
        {
            var devs = db.DevInfos.ToList();
            List<DeviceAlarm> alarms = new List<DeviceAlarm>();
            alarms.Add(new DeviceAlarm() { Id = 0, Level = Abutment_DevAlarmLevel.低, Title = "告警1", Message = "设备告警1", CreateTime = new DateTime(2018, 8, 28, 9, 5, 34) }.SetDev(devs[0].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 1, Level = Abutment_DevAlarmLevel.中, Title = "告警2", Message = "设备告警2", CreateTime = new DateTime(2018, 8, 28, 9, 5, 34) }.SetDev(devs[0].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 2, Level = Abutment_DevAlarmLevel.高, Title = "告警3", Message = "设备告警3", CreateTime = new DateTime(2018, 9, 1, 13, 44, 11) }.SetDev(devs[1].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 3, Level = Abutment_DevAlarmLevel.中, Title = "告警4", Message = "设备告警4", CreateTime = new DateTime(2018, 9, 2, 14, 55, 20) }.SetDev(devs[2].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 4, Level = Abutment_DevAlarmLevel.低, Title = "告警5", Message = "设备告警5", CreateTime = new DateTime(2018, 9, 2, 13, 22, 44) }.SetDev(devs[3].ToTModel()));
            return alarms;
        }
        #endregion

        #region WORK
        //获取操作票列表
        public List<OperationTicket> GetOperationTicketList()
        {
            var operationTickets = db.OperationTickets.ToList();
            var lst= operationTickets.ToWcfModelList();
            return lst;
        }

        //获取工作票列表
        public List<WorkTicket> GetWorkTicketList()
        {
            var workTickets = db.WorkTickets.ToList();
            return workTickets.ToWcfModelList();
        }

        //获取巡检设备列表
        public List<MobileInspectionDev> GetMobileInspectionDevList()
        {
            var MobileInspectionDev = db.MobileInspectionDevs.ToList();
            return MobileInspectionDev.ToWcfModelList();
        }

        //获取巡检轨迹列表
        public List<MobileInspection> GetMobileInspectionList()
        {
            var MobileInspection = db.MobileInspections.ToList();
            return MobileInspection.ToWcfModelList();
        }

        //获取人员巡检轨迹列表
        public List<PersonnelMobileInspection> GetPersonnelMobileInspectionList()
        {
            var PersonnelMobileInspection = db.PersonnelMobileInspections.ToList();
            return PersonnelMobileInspection.ToWcfModelList();
        }

        //获取操作票历史记录
        public List<OperationItemHistory> GetOperationItemHistoryList()
        {
            var OperationItemHistory = db.OperationItemHistorys.ToList();
            return OperationItemHistory.ToWcfModelList();
        }

        //获取工作票历史记录
        public List<WorkTicketHistory> GetWorkTicketHistoryList()
        {
            var WorkTicketHistory = db.WorkTicketHistorys.ToList();
            return WorkTicketHistory.ToWcfModelList();
        }
        
        //获取人员巡检轨迹历史记录
        public List<PersonnelMobileInspectionHistory> GetPersonnelMobileInspectionHistoryList()
        {
            var PersonnelMobileInspectionHistory = db.PersonnelMobileInspectionHistorys.ToList();
            return PersonnelMobileInspectionHistory.ToWcfModelList();
        }
        

        #endregion
    }
}
