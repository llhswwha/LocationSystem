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
            //定位标签
            InitDepartments();
            //机构、地图、区域、标签
            InitUsers();
            //登录人员

            new AreaTreeInitializer(_bll).InitAreaAndDev();
            //区域、设备
            InitConfigArgs();
            //配置信息
            InitDevModelAndType();
            InitAuthorization();
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

        public void InitDepartments()
        {
            Log.InfoStart("InitDepartments");


            Department dep0 = new Department() { Name = "根节点", ShowOrder = 0, Parent = null, Type = DepartType.本厂 };
            Departments.Add(dep0);

            Department dep11 = new Department() { Name = "四会电厂", ShowOrder = 0, Parent = dep0, Type = DepartType.本厂 };
            Departments.Add(dep11);
            Department dep12 = new Department() { Name = "维修部门", ShowOrder = 0, Parent = dep11, Type = DepartType.本厂 };
            Departments.Add(dep12);
            Department dep13 = new Department() { Name = "发电部门", ShowOrder = 1, Parent = dep11, Type = DepartType.本厂 };
            Departments.Add(dep13);
            Department dep14 = new Department() { Name = "外委人员", ShowOrder = 2, Parent = dep11, Type = DepartType.本厂 };
            Departments.Add(dep14);
            Department dep15 = new Department() { Name = "访客", ShowOrder = 0, Parent = dep11, Type = DepartType.本厂 };

            Departments.Add(dep15);


            //Departments.AddRange(dep11, dep12, dep13, dep14, dep15);

            List<LocationCard> tagsT = LocationCards.ToList();
            if (tagsT.Count >= 1)
            {
                Post post = new Post() { Name = "前台" };
                Posts.Add(post);
                AddPerson("蔡小姐", Sexs.女, tagsT[0], dep11, post, 1, "13546849866");
            }

            if (tagsT.Count >= 2)
            {
                Post post = new Post() { Name = "电工" };
                Posts.Add(post);
                AddPerson("刘先生", Sexs.男, tagsT[1], dep12, post, 2, "13543544345");
            }

            if (tagsT.Count >= 3)
            {
                Post post = new Post() { Name = "维修工" };
                Posts.Add(post);
                AddPerson("陈先生", Sexs.男, tagsT[2], dep12, post, 11, "13546849116");
            }

            if (tagsT.Count >= 4)
            {
                Post post = new Post() { Name = "保安" };
                Posts.Add(post);
                AddPerson("刘先生", Sexs.男, tagsT[3], dep11, post, 12, "13546414256");
            }

            if (tagsT.Count >= 5)
            {
                Post post = new Post() { Name = "经理" };
                Posts.Add(post);
                AddPerson("邱先生", Sexs.男, tagsT[4], dep11, post, 13, "13546578656");
            }

            if (tagsT.Count >= 6)
            {
                Post post = new Post() { Name = "电工" };
                Posts.Add(post);
                AddPerson("赵一男", Sexs.男, tagsT[5], dep11, post, 14, "13546578633");
            }

            if (tagsT.Count >= 7)
            {
                Post post = new Post() { Name = "经理" };
                Posts.Add(post);
                AddPerson("刘清风", Sexs.男, tagsT[6], dep11, post, 15, "13546578632");
            }

            if (tagsT.Count >= 8)
            {
                Post post = new Post() { Name = "经理" };
                Posts.Add(post);
                AddPerson("王依含", Sexs.女, tagsT[7], dep11, post, 17, "13549878632");
            }

            Post post2 = new Post() { Name = "访客" };
            Posts.Add(post2);
            for (int i = 0; i < 100; i++)
            {
                AddPerson("张先生" + i, Sexs.男, null, dep15, post2, 17 + i, "13549878000");
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

        public void InitTagPositions()
        {
            DateTime dt = DateTime.Now;
            long TimeStamp = TimeConvert.DateTimeToTimeStamp(dt);

            iniRole = new CardRoleInitializer(_bll);
            iniRole.InitData();

            Log.InfoStart("InitTagPositions");
            var tag1 = new LocationCard() { Name = "标签1", Code = "0002", CardRoleId = iniRole.role1.Id };
            var tag2 = new LocationCard() { Name = "标签2", Code = "0003", CardRoleId = iniRole.role2.Id };
            var tag3 = new LocationCard() { Name = "标签3", Code = "0004", CardRoleId = iniRole.role3.Id };
            var tag4 = new LocationCard() { Name = "标签4", Code = "0005", CardRoleId = iniRole.role3.Id };
            var tag5 = new LocationCard() { Name = "标签5", Code = "0006", CardRoleId = iniRole.role4.Id };
            var tag6 = new LocationCard() { Name = "标签6", Code = "0007", CardRoleId = iniRole.role4.Id };
            var tag7 = new LocationCard() { Name = "标签7", Code = "0008", CardRoleId = iniRole.role5.Id };
            var tag8 = new LocationCard() { Name = "标签8", Code = "0009", CardRoleId = iniRole.role6.Id };
            List<LocationCard> tags = new List<LocationCard>() { tag1, tag2, tag3, tag4, tag5, tag6, tag7, tag8 };
            LocationCards.AddRange(tags);
            List<LocationCard> tagsT = new List<LocationCard>();
            //for (int i = 0; i < 100; i++)
            //{
            //    var tagT = new LocationCard() { Name = "标签T"+ i.ToString(), Code = "0000" + i.ToString() };
            //    tagsT.Add(tagT);
            //}

            LocationCards.AddRange(tagsT);


            var tagposition1 = new LocationCardPosition() { CardId = 1, Code = "0002", X = 2293.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 1 };
            var tagposition2 = new LocationCardPosition() { CardId = 2, Code = "0003", X = 2294.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 2 };
            var tagposition3 = new LocationCardPosition() { CardId = 3, Code = "0004", X = 2295.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 3 };
            var tagposition4 = new LocationCardPosition() { CardId = 4, Code = "0005", X = 2296.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 4 };
            var tagposition5 = new LocationCardPosition() { CardId = 5, Code = "0006", X = 2297.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 5 };
            var tagposition6 = new LocationCardPosition() { CardId = 6, Code = "0007", X = 2298.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 6 };
            var tagposition7 = new LocationCardPosition() { CardId = 7, Code = "0008", X = 2299.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 7 };
            var tagposition8 = new LocationCardPosition() { CardId = 8, Code = "0009", X = 2300.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 8 };

            List<LocationCardPosition> tagpositions = new List<LocationCardPosition>() { tagposition1, tagposition2, tagposition3, tagposition4, tagposition5, tagposition6, tagposition7, tagposition8 };
            LocationCardPositions.AddRange(tagpositions);

            Position position1 = new Position() { Code = "002", X = -50, Y = -50, Z = -50, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position2 = new Position() { Code = "002", X = 0, Y = 0, Z = 0, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position3 = new Position() { Code = "002", X = 50, Y = 50, Z = 50, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position4 = new Position() { Code = "002", X = 100, Y = 100, Z = 100, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position5 = new Position() { Code = "002", X = 150, Y = 150, Z = 150, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position6 = new Position() { Code = "002", X = 200, Y = 200, Z = 200, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position7 = new Position() { Code = "002", X = 250, Y = 250, Z = 250, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position8 = new Position() { Code = "002", X = 300, Y = 300, Z = 300, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position9 = new Position() { Code = "002", X = 350, Y = 350, Z = 350, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position10 = new Position() { Code = "002", X = 400, Y = 400, Z = 400, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position11 = new Position() { Code = "002", X = 500, Y = 500, Z = 450, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position12 = new Position() { Code = "002", X = 600, Y = 600, Z = 500, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position13 = new Position() { Code = "002", X = 700, Y = 700, Z = 550, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position14 = new Position() { Code = "002", X = 800, Y = 800, Z = 600, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position15 = new Position() { Code = "002", X = 900, Y = 900, Z = 650, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position16 = new Position() { Code = "002", X = 1100, Y = 1100, Z = 700, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position17 = new Position() { Code = "002", X = 1200, Y = 1200, Z = 750, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position18 = new Position() { Code = "002", X = 1300, Y = 1300, Z = 800, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position19 = new Position() { Code = "002", X = 1400, Y = 1400, Z = 850, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position20 = new Position() { Code = "002", X = 1500, Y = 1500, Z = 900, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            List<Position> positions = new List<Position>() { position1, position2, position3, position4, position5, position6, position7, position8, position9, position10, position11, position12, position13, position14, position15, position16, position17, position18, position19, position20 };
            Positions.AddRange(positions);
            Log.InfoEnd("InitTagPositions");
        }

        public void InitAuthorization()
        {
            
            //区域权限列表
            var areas = Areas.ToList();
            var aaList = new List<AreaAuthorization>();
            foreach (var area in areas)
            {
                var aa = new AreaAuthorization();
                aa.AreaId = aa.Id;
                aa.Area = area;
                aa.AccessType = AreaAccessType.Enter;//可进入的权限
                aa.RangeType = AreaRangeType.WithParent;
                aa.Description = string.Format("{0}权限", area.Name);
                aa.Name = string.Format("{0}权限", area.Name);
                aa.CreateTime = DateTime.Now;
                aa.ModifyTime = DateTime.Now;
                aa.RepeatDay = RepeatDay.All;
                aa.TimeType = TimeSettingType.TimeRange;
                aa.StartTime = new DateTime(2000, 0, 0, 8, 30, 0);
                aa.EndTime = new DateTime(2000, 0, 0, 17, 30, 0);
                aaList.Add(aa);
            }
            AreaAuthorizations.AddRange(aaList);

            var aarList = new List<AreaAuthorizationRecord>();
            //权限指派给标签角色
            foreach (var aa in aaList)
            {
                foreach (var role in iniRole.roles)
                {
                    var aar = new AreaAuthorizationRecord(aa, role);
                    aarList.Add(aar);
                }
                //1.超级管理员能够进入全部区间
                //2.管理人员也能进入全部区域
                //3.巡检人员和维修人员能够进入生产区域

                //4.参观人员（高级）能够进入生活区域和大部分生产区域
                //5.参观人员（一般）能够进入生活区域和少部分生产区域
            }
            AreaAuthorizationRecords.AddRange(aarList);

            //角色,区域，卡
            //1.可以进入全部区域
            //2.可以进入生产区域
            //3.可以进入非生产区域
            //4.可以进入多个区域
            //5.可以进入某一个楼层
            //6.可以进入某个房间

            //AreaAuthorizations.Add(new AreaAuthorization() {})
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
