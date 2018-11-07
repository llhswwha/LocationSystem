using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BLL.Tools;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Data;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using ExcelLib;
using Location.BLL.Tool;
using Location.TModel.Tools;
using System.Threading;
using BLL.Blls.Location;
using BLL.Blls.LocationHistory;
using BLL.Initializers;
using DbModel.Location.Authorizations;
using DbModel.Location.Work;
using DbModel.Tools.InitInfos;

namespace BLL
{
    /// <summary>
    /// 数据库初始化类
    /// </summary>
    public class DbInitializer
    {

        private static bool isInited = false;

        private readonly Bll _bll;

        private CardRoleBll CardRoles => _bll.CardRoles;

        private LocationCardBll LocationCards => _bll.LocationCards;

        private LocationCardPositionBll LocationCardPositions => _bll.LocationCardPositions;

        private PositionBll Positions => _bll.Positions;

        private AreaBll Areas => _bll.Areas;

        private AreaAuthorizationBll AreaAuthorizations => _bll.AreaAuthorizations;

        private AreaAuthorizationRecordBll AreaAuthorizationRecords => _bll.AreaAuthorizationRecords;

        private DevModelBll DevModels => _bll.DevModels;

        private DevTypeBll DevTypes => _bll.DevTypes;

        private ConfigArgBll ConfigArgs => _bll.ConfigArgs;

        private PersonnelBll Personnels => _bll.Personnels;

        private DepartmentBll Departments => _bll.Departments;

        private PostBll Posts => _bll.Posts;

        private LocationCardToPersonnelBll LocationCardToPersonnels => _bll.LocationCardToPersonnels;

        public DbInitializer(Bll bll)
        {
            _bll = bll;
        }

        int maxPersonCount = 20;//初始人的数量
        int maxTagCount = 20;//初始卡的数量

        public void InitDbData(int mode, bool isForce = false)
        {
            if (isForce == false && isInited)
            {
                Log.Info("IsInited");
                return;
            }
            isInited = true;

            Log.InfoStart("InitDbData", true);
            if (!_bll.HasData())
            {
                Log.InfoStart("初始化数据库", true);
                if (mode == 0)
                {
                    InitByEntitys();
                }
                else if (mode == 1)
                {
                    InitBySqlFile();
                }
                else
                {
                    Log.Error("InitDbData mode:" + mode);
                }
                Log.InfoEnd("初始化数据库");
            }
            Log.InfoEnd("InitDbData");
        }

        private void InitBySqlFile()
        {
            string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\Location.sql";
            if (File.Exists(initFile))
            {
                Log.Info("从sql文件读取");
                try
                {
                    string txt = File.ReadAllText(initFile);
                    int count = _bll.Db.Database.ExecuteNonQuery(txt);
                }
                catch (Exception ex)
                {
                    Log.Error("执行sql语句失败", ex);
                }
            }
            else
            {
                Log.Info("sql文件不存在:" + initFile);

                InitByEntitys();
            }
        }

        public void InitByEntitys()
        {
            InitKKSCode();

            InitTagPositions();
            //卡角色、定位卡
            InitDepartments();
            //机构、人员

            InitUsers();
            //登录人员

            new AreaTreeInitializer(_bll).InitAreaAndDev();
            //区域、设备

            InitRealTimePositions();//初始化定位卡初始实时位置

            InitConfigArgs();
            //配置信息
            InitDevModelAndType();
            InitAuthorization();
        }

        public void InitCardAndPerson()
        {
            InitTagPositions();
            //卡角色、定位卡
            InitDepartments();
            //机构、人员

            InitUsers();

            InitRealTimePositions();//初始化定位卡初始实时位置
        }

        private void AddPerson(string name, Sexs sex, LocationCard tag, Department dep, Post pst, int worknumber, string phone)
        {
            Personnel person = new Personnel()
            {
                Name = name,
                Sex = sex,
                Enabled = true,
                ParentId = dep.Id,
                WorkNumber = worknumber,
                Phone = phone,
                Pst = pst.Name
            };
            Personnels.Add(person);


            if (tag != null && person != null)
            {
                LocationCardToPersonnel cardToPerson = new LocationCardToPersonnel();
                cardToPerson.PersonnelId = person.Id;
                cardToPerson.LocationCardId = tag.Id;
                LocationCardToPersonnels.Add(cardToPerson);
            }
        }

        Random r=new Random(DateTime.Now.Millisecond);

        public void InitDepartments()
        {
            Log.InfoStart("InitDepartments");

            LocationCardToPersonnels.Clear();
            Personnels.Clear();
            Departments.Clear();

            Department dep0 = new Department() { Name = "根节点", ShowOrder = 0, Parent = null, Type = DepartType.本厂 };
            Departments.Add(dep0);
            Department dep11 = new Department() { Name = "四会电厂", ShowOrder = 0, Parent = dep0, Type = DepartType.本厂 };
            Departments.Add(dep11);
            Department dep12 = new Department() { Name = "维修部门", ShowOrder = 0, Parent = dep11, Type = DepartType.本厂 };
            Departments.Add(dep12);//单个添加可以只是设置Parent
            Department dep13 = new Department() { Name = "发电部门", ShowOrder = 1, ParentId = dep11.Id, Type = DepartType.本厂 };//批量添加必须设置ParentId
            Department dep14 = new Department() { Name = "外委人员", ShowOrder = 2, ParentId = dep11.Id, Type = DepartType.本厂 };
            Department dep15 = new Department() { Name = "访客", ShowOrder = 0, ParentId = dep11.Id, Type = DepartType.本厂 };

            List<Department> subDeps = new List<Department>() { dep12,dep13, dep14,dep15};
            List<Department> subDeps2 = new List<Department>() { dep13, dep14, dep15 };
            Departments.AddRange(subDeps2);

            //Departments.AddRange(dep11, dep12, dep13, dep14, dep15);

            Posts.Clear();
            Post post1 = new Post() { Name = "前台" };
            Post post2 = new Post() { Name = "电工" };
            Post post3 = new Post() { Name = "维修工" };
            Post post4 = new Post() { Name = "保安" };
            Post post5 = new Post() { Name = "经理" };
            Post post6 = new Post() { Name = "电工" };
            Post post7 = new Post() { Name = "访客" };
            var posts = new List<Post>() {post1,post2,post3,post4,post5,post6,post7};
            Posts.AddRange(posts);
            List<LocationCard> tagsT = LocationCards.ToList();
            RandomTool rt=new RandomTool();

            for (int i = 0; i < maxPersonCount && i<tagsT.Count; i++)
            {
                var tag = tagsT[i];
                int n = r.Next(1);
                var post = posts[r.Next(posts.Count)];
                var dep = subDeps[r.Next(subDeps.Count)];
                if (n == 0)
                {
                    AddPerson(rt.GetWomanName(), Sexs.女, tag, dep, post, i, rt.GetRandomTel());
                }
                else
                {
                    AddPerson(rt.GetManName(), Sexs.男, tag, dep, post, i, rt.GetRandomTel());
                }
            }


        }

        public void InitUsers()
        {
            Log.InfoStart("InitUsers");
            //User user1 = new User() { Name = "管理员", LoginName = "admin", Password = "admin" };
            //User user2 = new User() { Name = "用户1", LoginName = "user1", Password = "user1" };
            //Users.AddRange(new List<Model.User>() { user1, user2 });
            Log.InfoEnd("InitUsers");
        }

        public void InitKKSCode()
        {

            //先导入KKS再初始化其他数据
            Log.Info("导入土建KKS");
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            Log.Info("BaseDirectory:" + basePath);
            string filePath = basePath + "Data\\中电四会部件级KKS编码2017.5.24\\土建\\中电四会热电有限责任公司KKS项目-土建系统-B.xls";
            KKSCodeHelper.ImportKKSFromFile<KKSCode>(new FileInfo(filePath));
        }

        private CardRoleInitializer iniRole;

        /// <summary>
        /// 初始化实时位置信息
        /// </summary>
        public void InitRealTimePositions()
        {
            DateTime dt = DateTime.Now;
            long TimeStamp = TimeConvert.DateTimeToTimeStamp(dt);

            LocationCardPositions.Clear();
            List<LocationCardPosition> tagpositions = new List<LocationCardPosition>();
            var cardPersons = _bll.LocationCardToPersonnels.ToList();//和卡绑定的人才有位置信息
            var cards = _bll.LocationCards.ToList();
            var area = _bll.Areas.Find(2);//电厂
            var bound = area.InitBound;

            for (int i = 0; i < cardPersons.Count; i++)
            {
                var cp = cardPersons[i];
                var card = cards.Find(j => j.Id == cp.LocationCardId);
                if (i < 10)//这部分固定初始位置
                {
                    var tagposition = new LocationCardPosition() { CardId = cp.LocationCardId, Code = card.Code, X = 2292.5f+i, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = i, Flag = "0:0:0:0:0", AreaId = area.Id, PersonId = cp.Id };
                    tagpositions.Add(tagposition);
                }
                else//这部分随机初始位置
                {
                    var x = r.Next((int)bound.GetWidth()) + bound.MinX;
                    var z = r.Next((int)bound.GetLength()) + bound.MinY;
                    var tagposition = new LocationCardPosition() { CardId = cp.LocationCardId, Code = card.Code, X = x, Y = 2, Z = z, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = i, Flag = "0:0:0:0:0", AreaId = area.Id, PersonId = cp.Id };
                    tagpositions.Add(tagposition);
                }
                
            }
            LocationCardPositions.AddRange(tagpositions);
        }

        public void InitTagPositions()
        {
            CardRoles.Clear();//清空角色
            iniRole = new CardRoleInitializer(_bll);
            iniRole.InitData();

            var roles = GetRoles();

            Random r=new Random(DateTime.Now.Millisecond);
            Log.InfoStart("InitTagPositions");
            List<LocationCard> tags = new List<LocationCard>();
            for (int i = 0; i < maxTagCount; i++)//400张卡
            {
                var role = roles[r.Next(roles.Count)];//随机分配角色
                var tag1 = new LocationCard() { Name = "标签"+i, Code = "000"+(i+1), CardRoleId = role.Id };
                tags.Add(tag1);
            }
            LocationCards.AddRange(tags);

            //DateTime dt = DateTime.Now;
            //long TimeStamp = TimeConvert.DateTimeToTimeStamp(dt);
            //Position position1 = new Position() { Code = "002", X = -50, Y = -50, Z = -50, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position2 = new Position() { Code = "002", X = 0, Y = 0, Z = 0, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position3 = new Position() { Code = "002", X = 50, Y = 50, Z = 50, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position4 = new Position() { Code = "002", X = 100, Y = 100, Z = 100, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            //Position position5 = new Position() { Code = "002", X = 150, Y = 150, Z = 150, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position6 = new Position() { Code = "002", X = 200, Y = 200, Z = 200, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position7 = new Position() { Code = "002", X = 250, Y = 250, Z = 250, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position8 = new Position() { Code = "002", X = 300, Y = 300, Z = 300, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            //Position position9 = new Position() { Code = "002", X = 350, Y = 350, Z = 350, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position10 = new Position() { Code = "002", X = 400, Y = 400, Z = 400, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position11 = new Position() { Code = "002", X = 500, Y = 500, Z = 450, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            //Position position12 = new Position() { Code = "002", X = 600, Y = 600, Z = 500, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position13 = new Position() { Code = "002", X = 700, Y = 700, Z = 550, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position14 = new Position() { Code = "002", X = 800, Y = 800, Z = 600, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position15 = new Position() { Code = "002", X = 900, Y = 900, Z = 650, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            //Position position16 = new Position() { Code = "002", X = 1100, Y = 1100, Z = 700, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position17 = new Position() { Code = "002", X = 1200, Y = 1200, Z = 750, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position18 = new Position() { Code = "002", X = 1300, Y = 1300, Z = 800, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position19 = new Position() { Code = "002", X = 1400, Y = 1400, Z = 850, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //Position position20 = new Position() { Code = "002", X = 1500, Y = 1500, Z = 900, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //List<Position> positions = new List<Position>() { position1, position2, position3, position4, position5, position6, position7, position8, position9, position10, position11, position12, position13, position14, position15, position16, position17, position18, position19, position20 };
            //Positions.Clear();
            //Positions.AddRange(positions);
            Log.InfoEnd("InitTagPositions");
        }

        public List<CardRole> GetRoles()
        {
            List<CardRole> roles = null;
            if (iniRole != null && iniRole.roles != null)
            {
                roles = iniRole.roles;
            }
            else
            {
                roles = _bll.CardRoles.ToList();
            }
            return roles;
        }

        public void InitAuthorization()
        {
            AreaAuthorizationRecords.Clear();
            AreaAuthorizations.Clear();

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\AuthorizationTree.xml";

            //if (File.Exists(path))
            //{
            //    InitAuthorizationFromFile(path);
            //}
            //else
            {
                InitAuthorizationFromAreas(path);
            }
        }

        private void InitAuthorizationFromAreas(string path)
        {
            var aaList = new List<AreaAuthorization>();
            var aarList = new List<AreaAuthorizationRecord>();
            //区域权限列表

            var areas = Areas.ToList();
            var areas2 = new List<AuthorizationArea>();
            foreach (var area in areas)
            {
                var aa2 = new AuthorizationArea();
                aa2.Id = area.Id;
                aa2.Name = area.Name;
                aa2.ParentId = area.ParentId ?? 0;
                areas2.Add(aa2);
                if (area.InitBound == null) continue;

                var accTypes = Enum.GetValues(typeof(AreaAccessType));
                foreach (AreaAccessType accType in accTypes)
                {
                    var aa = new AreaAuthorization();
                    aa.AreaId = area.Id;
                    //aa.Area = area;
                    aa.AccessType = accType; //可进入的权限
                    aa.RangeType = AreaRangeType.WithParent;
                    aa.Description = string.Format("权限[{0}][{1}]", accType,area.Name);
                    aa.Name = string.Format("权限：[{0}]区域‘{1}’", accType,area.Name);
                    aa.CreateTime = DateTime.Now;
                    aa.ModifyTime = DateTime.Now;
                    aa.RepeatDay = RepeatDay.All;
                    aa.TimeType = TimeSettingType.TimeRange;
                    aa.StartTime = new DateTime(2000, 1, 1, 8, 30, 0);
                    aa.EndTime = new DateTime(2000, 1, 1, 17, 30, 0);
                    aaList.Add(aa);
                    aa2.Items.Add(aa);
                }
            }
            bool r1 = AreaAuthorizations.AddRange(aaList);

            List<CardRole> roles = GetRoles();
            /*
            role1 = AddCardRole("超级管理员", "特殊角色，可以进入全部区域。");
            role2 = AddCardRole("管理人员");
            role3 = AddCardRole("巡检人员", "能够进入生产区域");
            role4 = AddCardRole("操作人员", "能够进入生产区域");
            role5 = AddCardRole("维修人员", "能够进入生产区域");
            role6 = AddCardRole("外维人员", "能够进入生活区域和指定生产区域");
            role7 = AddCardRole("参观人员(高级)", "能够进入生活区域和大部分生产区域");
            role8 = AddCardRole("参观人员(一般)", "能够进入生活区域和少部分生产区域");
             */

            //roles[0]
            
            //foreach (var item in roles)
            //{

            //}
            //foreach (var item in areas2)//区域列表
            //{

            //    item.Items.Find(i => i.AccessType == AreaAccessType.可以进入);
            //}

            //权限指派给标签角色
            foreach (var aa in aaList)
            {
                var aa2 = areas2.Find(i => i.Id == aa.AreaId);
                foreach (var role in roles)
                {
                    var aar = new AreaAuthorizationRecord(aa, role);
                    aarList.Add(aar);
                    aa2.Records.Add(aar);
                }
                //1.超级管理员能够进入全部区间
                //2.管理人员也能进入全部区域
                //3.巡检人员和维修人员能够进入生产区域
                //4.参观人员（高级）能够进入生活区域和大部分生产区域
                //5.参观人员（一般）能够进入生活区域和少部分生产区域
            }

            AreaAuthorizationRecords.AddRange(aarList);

            var tree = TreeHelper.CreateTree(areas2);
            XmlSerializeHelper.Save(tree[0], path);
            //tree[0]

            //角色,区域，卡
            //1.可以进入全部区域
            //2.可以进入生产区域
            //3.可以进入非生产区域
            //4.可以进入多个区域
            //5.可以进入某一个楼层
            //6.可以进入某个房间

            //AreaAuthorizations.Add(new AreaAuthorization() {})
        }

        private void InitAuthorizationFromFile(string path)
        {
            var aaList = new List<AreaAuthorization>();
            var aarList = new List<AreaAuthorizationRecord>();
            var tree = XmlSerializeHelper.LoadFromFile<AuthorizationArea>(path);
            var list = tree.GetAllChildren(null);
            foreach (var area in list)
            {
                if (area.Items != null)
                {
                    foreach (var aa in area.Items)
                    {
                        aa.CreateTime = DateTime.Now;
                        aa.ModifyTime = DateTime.Now;
                        aaList.Add(aa);
                    }
                }
            }
            bool r1 = AreaAuthorizations.AddRange(aaList);

            foreach (var area in list)
            {
                if (area.Records != null)
                {
                    foreach (var ar in area.Records)
                    {
                        ar.CreateTime = DateTime.Now;
                        ar.ModifyTime = DateTime.Now;
                        aarList.Add(ar);
                    }
                }
            }
            AreaAuthorizationRecords.AddRange(aarList);
        }

        /// <summary>
        /// 初始化设备模型和类型表
        /// </summary>
        public void InitDevModelAndType()
        {
            ////设备模型信息\\DevTypeModel.sql
            //string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\设备模型信息\\DevTypeModel.sql";
            //if (File.Exists(initFile))
            //{
            //    try
            //    {
            //        string txt = File.ReadAllText(initFile);
            //        int count = Db.Database.ExecuteNonQuery(txt);
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error("执行sql语句失败", ex);
            //    }
            //}
            //else
            //{
            //    Log.Info("sql文件不存在:" + initFile);
            //}
            //因为sqlite不支持改成从表格导入

            var list = LoadExcelToList<DevModel>(AppDomain.CurrentDomain.BaseDirectory + "Data\\DbInfos\\DevModel.xls");
            DevModels.AddRange(list);

            var list2 = LoadExcelToList<DevType>(AppDomain.CurrentDomain.BaseDirectory + "Data\\DbInfos\\DevType.xls");
            DevTypes.AddRange(list2);
        }

        public static List<T> LoadExcelToList<T>(string filePath) where T : class, new()
        {
            DataTable dt = ExcelHelper.LoadTable(new FileInfo(filePath), "", true);
            List<T> list = new List<T>();
            Type type = typeof(T);
            foreach (DataRow row in dt.Rows)
            {
                T item = new T();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var column = dt.Columns[i];
                    var value = row.ItemArray[i];
                    var pt = type.GetProperty(column.ColumnName);
                    if (pt == null) continue;
                    pt.SetValueEx(item, value);
                }
                list.Add(item);
            }
            return list;
        }

        public void InitConfigArgs()
        {
            ConfigArgs.Add(new ConfigArg()
            {
                Key = "TransferOfAxes.Zero",
                Value = "1372.066,0,1013.352",
                ValueType = "Vector3",
                Name = "坐标转换原点",
                Describe = "坐标转换原点（偏移)",
                Classify = "Axes"
            });
            ConfigArgs.Add(new ConfigArg()
            {
                Key = "TransferOfAxes.Scale",
                Value = "1.685735,0,1.686961",
                ValueType = "Vector3",
                Name = "坐标转换比例",
                Describe = "坐标转换比例",
                Classify = "Axes"
            });
            ConfigArgs.Add(new ConfigArg()
            {
                Key = "TransferOfAxes.Direction",
                Value = "-1,1,-1",
                ValueType = "Vector3",
                Name = "坐标转换方向",
                Describe = "坐标转换方向",
                Classify = "Axes"
            });
        }
    }
}
