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
using DbModel.Location.Authorizations;
using DbModel.Location.Work;

namespace BLL
{
    public partial class Bll
    {

        private static bool isInited = false;


        public void Init(int mode)
        {
            InitDb();
            InitDbData(mode);
        }

        public void InitAsync(int mode,Action callBack)
        {
            Thread thread = new Thread(() =>
              {
                  InitDb();
                  InitDbData(mode);
                  if (callBack != null)
                  {
                      callBack();
                  }
              });
            thread.Start();

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
            DbInitializer initializer=new DbInitializer(this);

            InitKKSCode();
            initializer.InitTagPositions(); 
            //定位标签
            InitDepartments(); 
            //机构、地图、区域、标签
            InitUsers(); 
            //登录人员
            InitAreaAndDev(); 
            //区域、设备
            InitConfigArgs();
            //配置信息
            InitDevModelAndType();
            initializer.InitAuthorization();
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

        public static List<T> LoadExcelToList<T>(string filePath) where T :class,new()
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
            bool value = LocationDeviceHelper.ImportLocationDeviceFromFile(filePath,Archors, Areas);
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
            bool value = DevInfoHelper.ImportDevInfoFromFile(filePath,this);
            Log.Info(string.Format("导入设备信息信息结果:{0}", value));
        }

        private void AddPerson(string name,Sexs sex,LocationCard tag,Department dep,Post pst,int worknumber,string phone)
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
        private void InitDepartments()
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
                AddPerson("张先生"+i, Sexs.男, null, dep15, post2, 17+i, "13549878000");
            }

        }

        private void InitUsers()
        {
            Log.InfoStart("InitUsers");
            //User user1 = new User() { Name = "管理员", LoginName = "admin", Password = "admin" };
            //User user2 = new User() { Name = "用户1", LoginName = "user1", Password = "user1" };
            //Users.AddRange(new List<Model.User>() { user1, user2 });
            Log.InfoEnd("InitUsers");
        }
    }
}
