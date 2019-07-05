using BLL;
using BLL.Tools;
using DbModel.Location.AreaAndDev;
using ExcelLib;
using Location.BLL.Tool;
using LocationServices.Locations;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.AreaAndDev;
using TModel.Tools;
using WebApiLib;
using static Location.BLL.Tool.KKSCodeHelper;

namespace LocationServer.Tools
{
    public static class DevMonitorHelper
    {
        /// <summary>
        /// 从【中电四会部件级KKS编码2017.5.24】文件夹初始化KKS
        /// </summary>
        public static void InitKKSCode()
        {
            DateTime start = DateTime.Now;

            Log.Info(LogTags.KKS, string.Format("初始化KKS"));

            Bll bll = Bll.Instance();
            bool r1 = bll.KKSCodes.Clear(1);

            DbInitializer dil = new DbInitializer(bll);

            dil.InitKKSCode();

            //Log.Info(LogTags.KKS, string.Format("清空数据"));

            //dil.ImportKKSCodeFromFile("土建\\中电四会热电有限责任公司KKS项目-土建系统-B.xls");
            //dil.ImportKKSCodeFromFile("汽机\\中电四会热电有限责任公司KKS项目-#3汽机总表-B.xls");
            //dil.ImportKKSCodeFromFile("汽机\\中电四会热电有限责任公司KKS项目-#1汽机总表-B.xls");
            TimeSpan time = DateTime.Now - start;
            Log.Info(LogTags.KKS, string.Format("完成 用时:{0}", time));
        }

        /// <summary>
        /// 从【四会热电KKS和设备编码清册_KKS.xlsx】文件初始化KKS
        /// </summary>
        public static void InitKKSNode()
        {
            try
            {
                DateTime start = DateTime.Now;

                Log.Info(LogTags.KKS, string.Format("初始化KKS"));

                Bll bll = new Bll();
                bool r1 = bll.KKSCodes.Clear(1);

                DbInitializer dil = new DbInitializer(bll);

                dil.InitKKSNode(); //新的，为了和老的区分开了，老的称为KKSCode,新的称为KKSNode

                //Log.Info(LogTags.KKS, string.Format("清空数据"));

                //dil.ImportKKSCodeFromFile("土建\\中电四会热电有限责任公司KKS项目-土建系统-B.xls");
                //dil.ImportKKSCodeFromFile("汽机\\中电四会热电有限责任公司KKS项目-#3汽机总表-B.xls");
                //dil.ImportKKSCodeFromFile("汽机\\中电四会热电有限责任公司KKS项目-#1汽机总表-B.xls");
                TimeSpan time = DateTime.Now - start;
                Log.Info(LogTags.KKS, string.Format("【1】【InitKKSNode】完成 用时:{0}", time));
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.KKS, "初始化KKS失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 解析测定数据 关联KKS 【UDPoints20190321.xlsx】
        /// </summary>
        public static void ParseMonitorPoint()
        {
            try
            {
                DateTime start = DateTime.Now;

                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                //string filePath = basePath + "Data\\KKS\\EDOSOriginalCode.xls";
                //string createfilePath = basePath + "..\\..\\Data\\KKS\\EDOS.xls";
                //int nReturn = Location.BLL.Tool.KKSCodeHelper.OriginalKKSCode(new FileInfo(filePath), createfilePath);

                string filePath = basePath + "Data\\KKS\\UDPoints20190321.xlsx";
                Log.Info(LogTags.KKS, "读取测点文件");
                DataTable dt = KKSCodeHelper.OriginalKKSCode_New(new FileInfo(filePath), true); //解析
                if (dt != null)
                {
                    //保存到文件
                    FileInfo file1 = new FileInfo(basePath + "..\\..\\Data\\KKS\\EDOS_New.xls");
                    if (file1.Exists) //已经存在说明是在vs里面打开
                    {
                        ExcelHelper.Save(dt, file1, null); //更新的是项目文件中的文件
                    }

                    FileInfo file2 = new FileInfo(basePath + "Data\\KKS\\EDOS_New.xls");
                    ExcelHelper.Save(dt, file2, null);

                    Log.Info(LogTags.KKS, "KKS转义完成");

                    //保存到数据库

                    //Log.Info(LogTags.KKS, "保存到数据库");
                    //bool r1 = bll.DevMonitorNodes.Clear(1);
                    //ist<DbModel.Location.AreaAndDev.DevMonitorNode> list1 = DevInfoHelper.CreateDevMonitorNodeListFromDataTable<DbModel.Location.AreaAndDev.DevMonitorNode>(dt);
                    //bool r2 = bll.DevMonitorNodes.AddRange(bll.Db, list1); //新增的部分
                }
                else
                {
                    Log.Info(LogTags.KKS, "原始KKS码文件不存在");
                }

                TimeSpan time = DateTime.Now - start;
                Log.Info(LogTags.KKS, string.Format("【2】【ParseMonitorPoint】完成 用时:{0}", time));
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.KKS, "KKS转义失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 将分析出来保存的测点信息（EDOS_New.xls)保存到数据库中
        /// </summary>
        public static void SaveMonitorPoint()
        {
            try
            {
                DateTime start = DateTime.Now;

                Log.Info(LogTags.KKS, "导入设备监控节点");
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                Log.Info(LogTags.KKS, "BaseDirectory:" + basePath);
                //string filePath = basePath + "Data\\KKS\\EDOS.xls";
                string filePath = basePath + "Data\\KKS\\EDOS_New.xls";
                DevInfoHelper.ImportDevMonitorNodeFromFile<DbModel.Location.AreaAndDev.DevMonitorNode>(
                    new FileInfo(filePath), true);

                TimeSpan time = DateTime.Now - start;
                Log.Info(LogTags.KKS, string.Format("【3】【SaveMonitorPoint】完成 用时:{0}", time));
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.KKS, "导入测点失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 重新分析测点数据，原来未找到kks的进行二次深入分析
        /// </summary>
        public static void ReParseMonitPoint(bool fromFile)
        {
            DateTime start = DateTime.Now;
            Log.Info(LogTags.KKS, "重新分析设备监控节点");

            Bll bll = new Bll();
            List<DbModel.Location.AreaAndDev.DevMonitorNode> monitors = null;
            if (fromFile) //从文件读取
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                Log.Info(LogTags.KKS, "BaseDirectory:" + basePath);
                string filePath = basePath + "Data\\KKS\\EDOS_New.xls";
                monitors =
                    DevInfoHelper.CreateDevMonitorNodeListFromFile<DbModel.Location.AreaAndDev.DevMonitorNode>(
                        new FileInfo(filePath));
            }
            else //从数据库读取
            {
                monitors = bll.DevMonitorNodes.ToList();
            }

            var kksCodes = bll.KKSCodes.ToListEx();
            var kksDict = KKSCode.ToDict(kksCodes);
            for (int i = 0; i < monitors.Count; i++)
            {
                DbModel.Location.AreaAndDev.DevMonitorNode monitor = monitors[i];
                if (i > 0)
                    monitor.PreNode = monitors[i - 1];
                if (i < monitors.Count - 1)
                    monitor.NextMode = monitors[i + 1];
                if (kksDict.ContainsKey(monitor.ParentKKS))
                {
                    monitor.KKSCode = kksDict[monitor.ParentKKS];
                }
            }

            var monitors1 = monitors.Where(i => i.ParseResult == "0").ToList();
            var monitors2 = monitors.Where(i => i.ParseResult == "1").ToList();
            var monitors31 = monitors.Where(i => i.ParseResult == "-1").ToList();
            var monitors311 = monitors31.Where(i => i.Unit != "").ToList();
            var monitors32 = monitors.Where(i => i.ParseResult == "-2").ToList();
            var monitors321 = monitors32.Where(i => i.Unit != "").ToList();
            var monitors4 = monitors.Where(i => i.ParseResult.ToInt() > 1).ToList();

            List<DbModel.Location.AreaAndDev.DevMonitorNode> editMonitorNodes =
                new List<DbModel.Location.AreaAndDev.DevMonitorNode>();
            for (int i = 0; i < monitors4.Count; i++)
            {
                DbModel.Location.AreaAndDev.DevMonitorNode monitorNode = monitors4[i];
                try
                {
                   
                    var parentCode = monitorNode.ParentKKS;
                    TagToKKSInfo info = KKSCodeHelper.ReParese(kksCodes, monitorNode.ParentKKS, monitorNode.KKS);
                    var code = info.GetParentCode();

                    if (info.Type == 1 || info.Type == 0)
                    {
                        monitorNode.ParentKKS = code;
                        monitorNode.ParseResult = "1"; //这里统一为1
                        editMonitorNodes.Add(monitorNode);

                        Log.Info(LogTags.KKS,
                            string.Format("解析测点:{0}=>{1}[{2}],({3}/{4})", parentCode, code, info.KKS, i, monitors4.Count));
                    }
                    else
                    {
                        if (monitorNode.PreNode != null)
                        {
                            if (monitorNode.PreNode.KKSCode != null)
                            {
                                var kksCode = monitorNode.PreNode.KKSCode.GetAncestor("系统");
                                if (kksCode != null)
                                {
                                    monitorNode.ParentKKS = kksCode.Code;
                                    monitorNode.ParseResult = "1"; //感觉这里要区分开来了
                                    editMonitorNodes.Add(monitorNode);
                                }
                                else
                                {
                                    Log.Info(LogTags.KKS,
                                        string.Format("monitorNode.PreNode.KKSCode 未找到系统节点:{0}", monitorNode.PreNode.KKSCode));
                                }
                            }
                            else
                            {
                                Log.Info(LogTags.KKS,
                                    string.Format("monitorNode.PreNode.KKSCode == null:{0}", monitorNode.PreNode));
                            }
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(LogTags.KKS, monitorNode+"\n" +e+"");
                }
            }

            bll.DevMonitorNodes.EditRange(editMonitorNodes);

            TimeSpan time = DateTime.Now - start;
            Log.Info(LogTags.KKS, string.Format("【4】【ReParseMonitPoint】完成 用时:{0}", time));
        }

        /// <summary>
        /// 获取静态设备相关的测点数据
        /// </summary>
        public static void GetAllStaticDevsMonitorData()
        {
            WebApiHelper.IsSaveJsonToFile = true;
            Bll bll = Bll.Instance();
            LocationService service = new LocationService();

            var staticDevs = bll.DevInfos.Where(i => i.Local_TypeCode == 20181008); //镜头设备
            DateTime start = DateTime.Now;
            for (int i = 0; i < staticDevs.Count; i++)
            {
                DevInfo dev = staticDevs[i];
                Log.Info(LogTags.KKS, string.Format("获取数据:{0}[{1}],({2}/{3})", dev.Name, dev.KKS, i, staticDevs.Count));
                var monitor = service.GetDevMonitorInfoByKKS(dev.KKS, true); //获取数据，这里需要优化
            }

            TimeSpan time = DateTime.Now - start;
            Log.Info(LogTags.KKS, string.Format("完成 用时:{0}", time));
            WebApiHelper.IsSaveJsonToFile = false;
        }

        /// <summary>
        /// 获取老的“汽机、燃机”KKS相关的所有测点
        /// </summary>
        public static void GetJ1F2DevsMonitorNodes()
        {
            WebApiHelper.IsSaveJsonToFile = true;
            Bll bll = Bll.Instance();
            LocationService service = new LocationService();

            DateTime start = DateTime.Now;
            var kksList1 = bll.KKSCodes.Where(i => i.MainType == "汽机" && i.ParentCode == "");
            var kksList2 = bll.KKSCodes.Where(i => i.MainType == "燃机" && i.ParentCode == "");
            List<DbModel.Location.AreaAndDev.KKSCode> kksList = new List<DbModel.Location.AreaAndDev.KKSCode>();
            kksList.AddRange(kksList1);
            kksList.AddRange(kksList2);
            List<Dev_Monitor> monitorDevs = new List<Dev_Monitor>();
            for (int i = 0; i < kksList.Count; i++)
            {
                var kks = kksList[i];
                if (i % 20 == 0)
                {
                    Log.Info(LogTags.KKS,
                        string.Format("获取数据:{0}[{1}],({2}/{3})", kks.Name, kks.Code, i, kksList.Count));
                }

                var monitorDev = service.GetDevMonitor(kks.Code, true);
                monitorDevs.Add(monitorDev);
            }

            List<string> tags = new List<string>();
            foreach (Dev_Monitor item in monitorDevs)
            {
                var subTags = item.GetAllTagList();
                foreach (var tag in subTags)
                {
                    if (tags.Contains(tag))
                    {

                    }
                    else
                    {
                        tags.Add(tag);
                    }
                }

                //tags += item.GetAllTagList() + "\n";
            }

            string tagsString = "";
            foreach (var item in tags)
            {

            }

            TimeSpan time = DateTime.Now - start;
            Log.Info(LogTags.KKS, string.Format("完成 用时:{0}", time));
            WebApiHelper.IsSaveJsonToFile = false;
        }

        /// <summary>
        /// 获取所有KKS的相关监控数据
        /// </summary>
        public static void GetAllKKSMonitorData()
        {
            WebApiHelper.IsSaveJsonToFile = true;
            try
            {
                LocationService service = new LocationService();
                var bll = Bll.Instance();
                var lst = bll.KKSCodes.ToList();
                for (int i = 0; i < lst.Count; i++)
                {
                    DbModel.Location.AreaAndDev.KKSCode item = lst[i];
                    var monitor = service.GetDevMonitorInfoByKKS(item.Code, true);
                    if (i % 20 == 0)
                    {
                        Log.Info(LogTags.KKS,
                            string.Format("获取KKS数据[{3:p3}]:{0},({1}/{2})", item, i, lst.Count,
                                ((i + 0.0f) / lst.Count)));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.KKS, "获取监控点数据失败：" + ex.Message);
            }

            WebApiHelper.IsSaveJsonToFile = false;
        }

        /// <summary>
        /// 获取所有的测点相应的数据
        /// </summary>
        public static void GetAllMonitorNodeData()
        {
            DateTime start = DateTime.Now;
            Log.Info(LogTags.KKS, string.Format("获取KKS数据..."));
            LocationService service = new LocationService();
            var bll = Bll.Instance();
            var monitorList = bll.DevMonitorNodes.ToList();
            //List<string>
            string tags = "";
            WebApiHelper.IsSaveJsonToFile = true;

            //List<DbModel.Location.AreaAndDev.DevMonitorNode> monitorList2 =
            //    new List<DbModel.Location.AreaAndDev.DevMonitorNode>();
            //for (int i = 0; i < monitorList.Count; i++)
            //{
            //    DbModel.Location.AreaAndDev.DevMonitorNode node = monitorList[i];
            //    string tag = node.TagName;
            //    if (!tag.Contains("/"))// "/"会导致失败
            //    {
            //        monitorList2.Add(node);
            //    }
            //}

            for (int i = 0; i < monitorList.Count; i++)
            {
                DbModel.Location.AreaAndDev.DevMonitorNode node = monitorList[i];
                string tag = node.TagName;
                if (tags == "")
                {
                    tags = tag;
                }
                else
                {
                    tags += "," + tag;
                }

                if ((i + 1) % 40 == 0) //每100个获取一次数据，200个不行，url长度有限制的
                {
                    var dataList = service.GetSomesisList(tags); //获取监控信息并保存到数据库中
                    Log.Info(LogTags.KKS,
                        string.Format("获取KKS数据[{3:p3}]:{0}...,({1}/{2})", node.Describe, i, monitorList.Count,
                            ((i + 0.0f) / monitorList.Count)));
                    tags = "";
                }
            }

            if (tags != "")
            {
                var dataList = service.GetSomesisList(tags); //获取监控信息并保存到数据库中
            }

            WebApiHelper.IsSaveJsonToFile = false;
            TimeSpan time = DateTime.Now - start;
            Log.Info(LogTags.KKS, string.Format("【5】【GetAllMonitorNodeData】完成 用时:{0}", time));
        }
    }
}
