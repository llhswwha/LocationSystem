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
using TModel.Tools;
using LocationServices.Locations.Services;

namespace LocationServices.Locations
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“LocationService”。
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class LocationService : ILocationService, IDisposable
    {
        private Bll db = new Bll(false, false, false,false);

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
            return new TagService(db).GetList();
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public bool AddTags(List<Tag> tags)
        {
            return new TagService(db).AddList(tags);
        }

        /// <summary>
        /// 删除某一个标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTag(int id)
        {
            return new TagService(db).Delete(id + "") != null;
        }

        /// <summary>
        /// 清空标签数据库表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAllTags()
        {
            return new TagService(db).DeleteAll();
        }

        /// <summary>
        /// 获取标签实时位置
        /// </summary>
        /// <returns></returns>
        public IList<TagPosition> GetRealPositons()
        {
            return new PosService(db).GetList();
        }

        /// <summary>
        /// 获取标签实时位置
        /// </summary>
        /// <returns></returns>
        public IList<TagPosition> GetRealPositonsByTags(List<string> tagCodes)
        {
            return new PosService(db).GetRealPositonsByTags(tagCodes);
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
            return new DepartmentService(db).GetList();
        }

        public Department GetDepartmentTree()
        {
            var leafNodes = GetPersonList();
            return new DepartmentService(db).GetTree(leafNodes);
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
            return db.ConfigArgs.DeleteById(config.Id) != null;
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
            return new PosHistoryService(db).GetHistory();
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
            return new PosHistoryService(db).GetHistoryByPerson(personnelID, start, end);
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
            return new PosHistoryService(db).GetHistoryByPersonAndArea(personnelID, topoNodeIds, start, end);
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
            return new PosHistoryService(db).GetHistoryByTag(tagcode, start, end);
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
            return new PosHistoryService(db).GetHistoryU3DPositonsByTime(tagcode, start, end);
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
            return db.Personnels.GetListByName(name).ToWcfModelList();
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
            return db.Personnels.DeleteById(id)!=null;
        }


        public List<Post> GetPostList()
        {
            var posts = db.Posts.ToList();
            return posts.ToWcfModelList();
        }
        #endregion

        #region WORK
        //获取操作票列表
        public List<OperationTicket> GetOperationTicketList()
        {
            var operationTickets = db.OperationTickets.ToList();
            //return operationTickets.ToWcfModelList();
            OperationTicket o1 = new OperationTicket() { Id = 1, No = "100001", Guardian = "李新风G" };
            o1.OperationTask = "操作任务";
            o1.OperationStartTime = DateTime.Now.AddHours(-2);
            o1.OperationEndTime = DateTime.Now;
            o1.Operator = "刘华名";
            o1.DutyOfficer = "车马风";
            o1.Dispatch = "调度";
            o1.Remark = "备注";
            o1.OperatorPersonelId = 7;
            OperationItem oi1 = new OperationItem() { Id = 1, OrderNum = 1, Item = "操作项1", DevId = "103cef6e-3155-4be8-a166-41ac567b7b01" };
            OperationItem oi2 = new OperationItem() { Id = 2, OrderNum = 2, Item = "操作项2", DevId = "e65ac4c1-0936-409f-9f09-894f5ee20fb8" };
            //OperationItem oi3 = new OperationItem() { Id = 3, OrderNum = 3, Item = "操作项3", DevId = "7ac0698a-8360-483e-b827-ba6e512ccdb2" };
            //OperationItem oi4 = new OperationItem() { Id = 4, OrderNum = 4, Item = "操作项4", DevId = "4c764e74-c03d-4967-9b6d-46c151a4fa23" };
            OperationItem oi3 = new OperationItem() { Id = 3, OrderNum = 3, Item = "操作项3", DevId = "56ae2f57-0068-4539-b496-f5198a216ef2" };
            OperationItem oi4 = new OperationItem() { Id = 4, OrderNum = 4, Item = "操作项4", DevId = "f2abcb56-8382-459e-b89f-2e1afd4043f4" };
            o1.OperationItems = new List<OperationItem>() { oi1 , oi2 , oi3 , oi4 };

            OperationTicket o2 = new OperationTicket() { Id = 2, No = "100002", Guardian = "赵一含G" };
            OperationTicket o3 = new OperationTicket() { Id = 3, No = "100003", Guardian = "刘国柱G" };
            OperationTicket o4 = new OperationTicket() { Id = 4, No = "100004", Guardian = "陈浩然G" };
            OperationTicket o5 = new OperationTicket() { Id = 5, No = "100005", Guardian = "李一样G" };
            List<OperationTicket> os = new List<OperationTicket>() { o1, o2, o3, o4, o5 };
            return os;
        }

        //获取工作票列表
        public List<WorkTicket> GetWorkTicketList()
        {
            var workTickets = db.WorkTickets.ToList();
            //return workTickets.ToWcfModelList();

            WorkTicket w1 = new WorkTicket() { Id = 1, No = "000001", PersonInCharge = "李新风", AreaId = 6 };
            w1.HeadOfWorkClass = "李新";
            w1.WorkPlace = "主厂房";
            w1.JobContent = "干活";
            w1.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
            w1.EndTimeOfPlannedWork = DateTime.Now;
            w1.WorkCondition = "工作条件，在什么...";
            w1.Lssuer = "叶路";
            w1.Licensor = "方城";
            w1.Approver = "吴发展";
            w1.Comment = "已经可以正常使用";
            w1.PersonInChargePersonelId = 7;

            SafetyMeasures s1 = new SafetyMeasures() { Id = 1, No = 1, LssuerContent = "小心触电", LicensorContent = "小心漏电" };
            SafetyMeasures s2 = new SafetyMeasures() { Id = 2, No = 2, LssuerContent = "禁止明火", LicensorContent = "易燃易爆" };
            SafetyMeasures s3 = new SafetyMeasures() { Id = 3, No = 3, LssuerContent = "轻拿轻放", LicensorContent = "设备已损坏" };
            w1.SafetyMeasuress = new List<SafetyMeasures>() { s1, s2, s3 };

            WorkTicket w2 = new WorkTicket() { Id = 2, No = "000002", PersonInCharge = "赵一含", AreaId = 38 };
            WorkTicket w3 = new WorkTicket() { Id = 3, No = "000003", PersonInCharge = "刘国柱", AreaId = 85 };
            WorkTicket w4 = new WorkTicket() { Id = 4, No = "000004", PersonInCharge = "陈浩然"};
            WorkTicket w5 = new WorkTicket() { Id = 5, No = "000005", PersonInCharge = "李一样" };
            List<WorkTicket> ws = new List<WorkTicket>() { w1, w2, w3, w4, w5 };
            return ws;

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
        public List<OperationTicketHistory> GetOperationTicketHistoryList()
        {
            var OperationTicketHistory = db.OperationTicketHistorys.ToList();
            //return OperationTicketHistory.ToWcfModelList();
            OperationTicketHistory o1 = new OperationTicketHistory() { Id = 1, No = "100001", Guardian = "李新风G" };
            o1.OperationTask = "操作任务";
            o1.OperationStartTime = DateTime.Now.AddHours(-2);
            o1.OperationEndTime = DateTime.Now;
            o1.Operator = "刘华名";
            o1.DutyOfficer = "车马风";
            o1.Dispatch = "调度";
            o1.Remark = "备注";
            //o1.OperatorPersonelId = 7;
            OperationItemHistory oi1 = new OperationItemHistory() { Id = 1, OrderNum = 1, Item = "操作项1" };
            OperationItemHistory oi2 = new OperationItemHistory() { Id = 2, OrderNum = 2, Item = "操作项2" };
            OperationItemHistory oi3 = new OperationItemHistory() { Id = 3, OrderNum = 3, Item = "操作项3" };
            OperationItemHistory oi4 = new OperationItemHistory() { Id = 4, OrderNum = 4, Item = "操作项4" };
            o1.OperationItems = new List<OperationItemHistory>() { oi1, oi2, oi3, oi4 };

            OperationTicketHistory o2 = new OperationTicketHistory() { Id = 2, No = "100002", Guardian = "赵一含G" };
            OperationTicketHistory o3 = new OperationTicketHistory() { Id = 3, No = "100003", Guardian = "刘国柱G" };
            OperationTicketHistory o4 = new OperationTicketHistory() { Id = 4, No = "100004", Guardian = "陈浩然G" };
            OperationTicketHistory o5 = new OperationTicketHistory() { Id = 5, No = "100005", Guardian = "李一样G" };
            List<OperationTicketHistory> os = new List<OperationTicketHistory>() { o1, o2, o3, o4, o5 };
            for (int i = 0; i < 20; i++)
            {
                OperationTicketHistory wT = new OperationTicketHistory() { Id = 6 + i, No = "000005" + i, Guardian = i.ToString() };
                os.Add(wT);
            }
            return os;
        }

        //获取工作票历史记录
        public List<WorkTicketHistory> GetWorkTicketHistoryList()
        {
            var WorkTicketHistory = db.WorkTicketHistorys.ToList();
            //return WorkTicketHistory.ToWcfModelList();
            WorkTicketHistory w1 = new WorkTicketHistory() { Id = 1, No = "000001", PersonInCharge = "李新风" };
            w1.HeadOfWorkClass = "李新";
            w1.WorkPlace = "主厂房";
            w1.JobContent = "干活";
            w1.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
            w1.EndTimeOfPlannedWork = DateTime.Now;
            w1.WorkCondition = "工作条件，在什么...";
            w1.Lssuer = "叶路";
            w1.Licensor = "方城";
            w1.Approver = "吴发展";
            w1.Comment = "已经可以正常使用";
            //w1.PersonInChargePersonelId = 7;

            SafetyMeasuresHistory s1 = new SafetyMeasuresHistory() { Id = 1, No = 1, LssuerContent = "小心触电", LicensorContent = "小心漏电" };
            SafetyMeasuresHistory s2 = new SafetyMeasuresHistory() { Id = 2, No = 2, LssuerContent = "禁止明火", LicensorContent = "易燃易爆" };
            SafetyMeasuresHistory s3 = new SafetyMeasuresHistory() { Id = 3, No = 3, LssuerContent = "轻拿轻放", LicensorContent = "设备已损坏" };
            w1.SafetyMeasuress = new List<SafetyMeasuresHistory>() { s1, s2, s3 };

            WorkTicketHistory w2 = new WorkTicketHistory() { Id = 2, No = "000002", PersonInCharge = "赵一含" };
            WorkTicketHistory w3 = new WorkTicketHistory() { Id = 3, No = "000003", PersonInCharge = "刘国柱" };
            WorkTicketHistory w4 = new WorkTicketHistory() { Id = 4, No = "000004", PersonInCharge = "陈浩然" };
            WorkTicketHistory w5 = new WorkTicketHistory() { Id = 5, No = "000005", PersonInCharge = "李一样" };
            List<WorkTicketHistory> ws = new List<WorkTicketHistory>() { w1, w2, w3, w4, w5 };
            for (int i = 0; i < 20; i++)
            {
                WorkTicketHistory wT = new WorkTicketHistory() { Id = 6 + i, No = "000005" + i, PersonInCharge = i.ToString() };
                ws.Add(wT);
            }
            return ws;
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
