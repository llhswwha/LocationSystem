using Location.BLL.Blls;
using Location.BLL.topviewxpBlls;
using Location.DAL;
using Location.Model;
using Location.Model.topviewxp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
//using log4net.Core;
using Location.BLL.Tool;
using Location.Model.Locations;
using Location.Model.LocationTables;
using Location.Model.Manage;

namespace Location.BLL
{
    public partial class TestBll : IDisposable
    {

        private static bool isInited = false;


        public void Init(int mode)
        {
            InitDb();
            InitDbData(mode);
        }

        public void InitDb()
        {
            Log.InfoStart("InitDb");
            //List<Department> list = Departments.ToList();
            int count = Departments.DbSet.Count();//调试时，第一次使用EF获取数据要占用15-20s的时间，部署后不会。
            Log.Info("Count:" + count);

            int count2 = DbHistory.U3DPositions.Count();
            Log.Info("Count2:" + count2);
            Log.InfoEnd("InitDb");
        }

        public bool HasData()
        {
            return Departments.ToList().Count > 0;
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
            if (!HasData())
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


        private void InitByEntitys()
        {
            InitKKSCode();

            InitTagPositions(); //定位标签
            InitDepartments(); //机构、地图、区域、标签


            InitUsers(); //登录人员

            InitPhysicalTopologys(); //物理拓扑树

            InitConfigArgs();//配置信息

            InitLocationDevice();//基站设备

            InitDevInfo();//设备信息（不包含基站设备）        
        }
      
        private void InitConfigArgs()
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

        private void InitBySqlFile()
        {
            string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\Location.sql";
            if (File.Exists(initFile))
            {
                Log.Info("从sql文件读取");
                try
                {
                    string txt = File.ReadAllText(initFile);
                    int count = Db.Database.ExecuteNonQuery(txt);
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

        private void InitKKSCode()
        {

            //先导入KKS再初始化其他数据
            Log.Info("导入土建KKS");
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            Log.Info("BaseDirectory:" + basePath);
            string filePath = basePath + "Data\\中电四会部件级KKS编码2017.5.24\\土建\\中电四会热电有限责任公司KKS项目-土建系统-B.xls";
            KKSCodeHelper.ImportKKSFromFile<KKSCode>(new FileInfo(filePath));
        }
        /// <summary>
        /// 初始化基站信息
        /// </summary>
        private void InitLocationDevice()
        {
            Log.Info("导入基站信息");
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\基站信息\\基站信息.xml";
            bool value = LocationDeviceHelper.ImportLocationDeviceFromFile(filePath,Archors, PhysicalTopologys);
            Log.Info(string.Format("导入基站信息结果:{0}",value));
        }
        /// <summary>
        /// 初始化设备信息
        /// </summary>
        private void InitDevInfo()
        {
            Log.Info("导入设备信息");
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\设备信息\\DevInfoBackup.xml";
            bool value = DevInfoHelper.ImportDevInfoFromFile(filePath,DevInfos);
            Log.Info(string.Format("导入设备信息信息结果:{0}", value));
        }
        private void InitDepartments()
        {
            Log.InfoStart("InitDepartments");


            Department dep0 = new Department() { Name = "根节点", ShowOrder = 0, Parent = null };
            Departments.Add(dep0);

            Department dep11 = new Department() { Name = "四会电厂", ShowOrder = 0, Parent = dep0 };
            Departments.Add(dep11);
            Department dep12 = new Department() { Name = "维修部门", ShowOrder = 0, Parent = dep11 };
            Departments.Add(dep12);
            Department dep13 = new Department() { Name = "发电部门", ShowOrder = 1, Parent = dep11 };
            Departments.Add(dep13);
            Department dep14 = new Department() { Name = "外委人员", ShowOrder = 2, Parent = dep11 };
            Departments.Add(dep14);
            Department dep15 = new Department() { Name = "访客", ShowOrder = 0, Parent = dep11 };

            Departments.Add(dep15);


            //Departments.AddRange(dep11, dep12, dep13, dep14, dep15);

            List<Tag> tagsT = Tags.DbSet.ToList();
            if (tagsT.Count >= 1)
            {
                Post post = new Post() { Name = "前台" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "蔡小姐", Sex="女", Tag = tagsT[0], Parent = dep11, Pst = post, WorkNumber=1, PhoneNumber="13546849866" };
                Personnels.Add(Personnel1);
            }

            if (tagsT.Count >= 2)
            {
                Post post = new Post() { Name = "电工" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "刘先生", Sex = "男", Tag = tagsT[1], Parent = dep12, Pst = post, WorkNumber = 2, PhoneNumber = "13543544345" };
                Personnels.Add(Personnel1);
            }

            if (tagsT.Count >= 3)
            {
                Post post = new Post() { Name = "维修工" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "陈先生", Sex = "男", Tag = tagsT[2], Parent = dep12, Pst = post, WorkNumber = 11, PhoneNumber = "13546849116" };
                Personnels.Add(Personnel1);
            }

            if (tagsT.Count >= 4)
            {
                Post post = new Post() { Name = "保安" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "刘先生", Sex = "男", Tag = tagsT[3], Parent = dep11, Pst = post, WorkNumber = 12, PhoneNumber = "13546414256" };
                Personnels.Add(Personnel1);
            }

            if (tagsT.Count >= 5)
            {
                Post post = new Post() { Name = "经理" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "邱先生", Sex = "男", Tag = tagsT[4], Parent = dep11, Pst = post , WorkNumber = 13, PhoneNumber = "13546578656" };
                Personnels.Add(Personnel1);
            }

            if (tagsT.Count >= 6)
            {
                Post post = new Post() { Name = "电工" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "赵一男", Sex = "男", Tag = tagsT[5], Parent = dep11, Pst = post, WorkNumber = 14, PhoneNumber = "13546578656" };
                Personnels.Add(Personnel1);
            }

            if (tagsT.Count >= 7)
            {
                Post post = new Post() { Name = "经理" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "刘清风", Sex = "男", Tag = tagsT[6], Parent = dep11, Pst = post, WorkNumber = 15, PhoneNumber = "13546578656" };
                Personnels.Add(Personnel1);
            }

            if (tagsT.Count >= 8)
            {
                Post post = new Post() { Name = "经理" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "王依含", Sex = "女", Tag = tagsT[7], Parent = dep11, Pst = post, WorkNumber = 16, PhoneNumber = "13546578656" };
                Personnels.Add(Personnel1);
            }

            for (int i = 0; i < 100; i++)
            {
                Post post = new Post() { Name = "访客" };
                Posts.Add(post);
                Personnel Personnel1 = new Personnel() { Name = "张先生", Sex = "男", Parent = dep15, Pst = post, WorkNumber = 17+ i, PhoneNumber = "13546842227" };
                Personnels.Add(Personnel1);
            }

        }

        private void InitUsers()
        {
            Log.InfoStart("InitUsers");
            User user1 = new User() { Name = "管理员", LoginName = "admin", Password = "admin" };
            User user2 = new User() { Name = "用户1", LoginName = "user1", Password = "user1" };
            Users.AddRange(new List<Model.User>() { user1, user2 });
            Log.InfoEnd("InitUsers");
        }

        private void InitTagPositions()
        {
            Log.InfoStart("InitTagPositions");
            Tag tag1 = new Tag() { Name = "标签1", Code = "0002" };
            Tag tag2 = new Tag() { Name = "标签2", Code = "0003" };
            Tag tag3 = new Tag() { Name = "标签3", Code = "0004" };
            Tag tag4 = new Tag() { Name = "标签4", Code = "0005" };
            Tag tag5 = new Tag() { Name = "标签5", Code = "0006" };
            Tag tag6 = new Tag() { Name = "标签6", Code = "0007" };
            Tag tag7 = new Tag() { Name = "标签7", Code = "0008" };
            Tag tag8 = new Tag() { Name = "标签8", Code = "0009" };
            List<Tag> tags = new List<Tag>() { tag1, tag2, tag3, tag4, tag5, tag6, tag7, tag8 };
            Tags.AddRange(tags);


            //TagPosition tagposition1 = new TagPosition() { Tag = "0002", X = -50, Y = -50, Z = -50, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            //TagPosition tagposition2 = new TagPosition() { Tag = "0003", X = -50, Y = -50, Z = -50, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            TagPosition tagposition1 = new TagPosition() { Tag = "0002", X = 2293.5, Y = 2, Z = 1715.5F, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            TagPosition tagposition2 = new TagPosition() { Tag = "0003", X = 2294.5, Y = 2, Z = 1715.5f, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            TagPosition tagposition3 = new TagPosition() { Tag = "0004", X = 2295.5, Y = 2, Z = 1715.5F, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            TagPosition tagposition4 = new TagPosition() { Tag = "0005", X = 2296.5, Y = 2, Z = 1715.5f, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            TagPosition tagposition5 = new TagPosition() { Tag = "0006", X = 2297.5, Y = 2, Z = 1715.5F, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            TagPosition tagposition6 = new TagPosition() { Tag = "0007", X = 2298.5, Y = 2, Z = 1715.5F, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            TagPosition tagposition7 = new TagPosition() { Tag = "0008", X = 2299.5, Y = 2, Z = 1715.5f, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            TagPosition tagposition8 = new TagPosition() { Tag = "0009", X = 2300.5, Y = 2, Z = 1715.5F, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };

            List<TagPosition> tagpositions = new List<TagPosition>() { tagposition1, tagposition2, tagposition3, tagposition4, tagposition5, tagposition6, tagposition7, tagposition8 };
            TagPositions.AddRange(tagpositions);

            Position position1 = new Position() { Tag = "002", X = -50, Y = -50, Z = -50, Time = 1527754678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position2 = new Position() { Tag = "002", X = 0, Y = 0, Z = 0, Time = 1527756478, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position3 = new Position() { Tag = "002", X = 50, Y = 50, Z = 50, Time = 1527758278, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position4 = new Position() { Tag = "002", X = 100, Y = 100, Z = 100, Time = 1527760078, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position5 = new Position() { Tag = "002", X = 150, Y = 150, Z = 150, Time = 1527761878, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position6 = new Position() { Tag = "002", X = 200, Y = 200, Z = 200, Time = 1527763678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position7 = new Position() { Tag = "002", X = 250, Y = 250, Z = 250, Time = 1527765478, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position8 = new Position() { Tag = "002", X = 300, Y = 300, Z = 300, Time = 1527767278, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position9 = new Position() { Tag = "002", X = 350, Y = 350, Z = 350, Time = 1527769078, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position10 = new Position() { Tag = "002", X = 400, Y = 400, Z = 400, Time = 1527770878, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position11 = new Position() { Tag = "002", X = 500, Y = 500, Z = 450, Time = 1527772678, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position12 = new Position() { Tag = "002", X = 600, Y = 600, Z = 500, Time = 1527774478, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position13 = new Position() { Tag = "002", X = 700, Y = 700, Z = 550, Time = 1527776278, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position14 = new Position() { Tag = "002", X = 800, Y = 800, Z = 600, Time = 1527778078, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position15 = new Position() { Tag = "002", X = 900, Y = 900, Z = 650, Time = 1527779878, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position16 = new Position() { Tag = "002", X = 1100, Y = 1100, Z = 700, Time = 1527781678, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position17 = new Position() { Tag = "002", X = 1200, Y = 1200, Z = 750, Time = 1527783478, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position18 = new Position() { Tag = "002", X = 1300, Y = 1300, Z = 800, Time = 1527785278, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position19 = new Position() { Tag = "002", X = 1400, Y = 1400, Z = 850, Time = 1527787078, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position20 = new Position() { Tag = "002", X = 1500, Y = 1500, Z = 900, Time = 1527788878, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            List<Position> positions = new List<Position>() { position1, position2, position3, position4, position5, position6, position7, position8, position9, position10, position11, position12, position13, position14, position15, position16, position17, position18, position19, position20 };
            Position.AddRange(positions);
            Log.InfoEnd("InitTagPositions");
        }
    }
}
