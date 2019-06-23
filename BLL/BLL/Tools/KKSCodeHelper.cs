using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BLL;
using DbModel.Location.AreaAndDev;
using ExcelLib;
using Location.IModel;
using TModel.Tools;

namespace Location.BLL.Tool
{
    public class KKSImportInfo<T> where T : IKKSCode, new()
    {
        public List<T> listAddInfo = new List<T>();
        public List<T> listEditInfo = new List<T>();

    }
    public static class KKSCodeHelper
    {
        public static T CreateKKSCode<T>(string Serial, string Name, string RawCode,string Code, string ParentCode, string DesinCode,
    string MainType, string SubType, string System) where T : IKKSCode, new()
        {
            T kks1 = new T();
            kks1.Serial = Serial;
            kks1.Name = Name;
            kks1.RawCode = RawCode;
            kks1.Code = Code;
            kks1.ParentCode = ParentCode;
            kks1.DesinCode = DesinCode;
            kks1.MainType = MainType;
            kks1.SubType = SubType;
            kks1.System = System;
            return kks1;
        }

        public static T CreateKKSCodeFromDataRow<T>(string mainType, DataRow dr) where T : IKKSCode, new()
        {
            DataTable dt = dr.Table;
            string strTableName = dt.TableName;
            string Serial = "";
            string Name = "";
            string RawCode = "";
            string Code = "";
            string ParentCode = "";
            string DesinCode = "";
            string MainType = mainType;
            string SubType = dt.TableName;
            string System = "";




            Serial = "";
            Name = "";
            Code = "";
            ParentCode = "";
            DesinCode = "";

            Serial = dr[0].ToString().Trim();
            Name = dr[1].ToString().Trim();
            RawCode = dr[2].ToString().Trim();
            DesinCode = dr[3].ToString().Trim();
            Code = RawCode.Replace(" ","");


            if (Serial == "*" && Code == "")
            {
                Code = Guid.NewGuid().ToString();//自己设置一个
                System = Name;
                //return default(T);
            }

            var kks1 = CreateKKSCode<T>(Serial, Name, RawCode,Code, ParentCode, DesinCode, MainType, SubType, System);
            return kks1;
        }


        public static List<T> CreateKKSCodeListFromDataTable<T>(string mainType, DataTable dt) where T : KKSCode, new()
        {
            List<T> list = new List<T>();
            List<List<T>> sysList = new List<List<T>>();
            List<T> subList = new List<T>();//子系统
            T sysRoot = default(T);//...系统 *
            T subSys = default(T);//...子系统 *N

            T parent1 = default(T);//一级父节点
            T parent2 = default(T);//二级父节点

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow dr = dt.Rows[j];
                T kksNew = CreateKKSCodeFromDataRow<T>(mainType, dr);//根据表格内容创建KKS对象
                if (kksNew.Name == "#1机凝汽器系统")
                {

                }
                if (kksNew == null) continue;
                if (kksNew.Serial.Contains("*"))//系统
                {
                    if (kksNew.Serial == "*")
                    {
                        if (j > 0)
                        {
                            string nextSerial = dt.Rows[j + 1][0].ToString();//下一行
                            int nextId = nextSerial.ToInt();
                            string pre = dt.Rows[j - 1][0].ToString();//上一行

                            if (!pre.Contains("*") && //自己不能是一个子系统
                                (nextSerial.Contains("*") //下一个还是一个子系统
                                || (nextId > 0 && nextId < 10)))
                            {
                                sysRoot = kksNew;
                                subSys = null;
                                subList = new List<T>();//开始一个子系统
                                sysList.Add(subList);
                            }
                            else//还是子系统
                            {
                                subSys = kksNew;
                            }
                        }
                        else//第一行
                        {
                            sysRoot = kksNew;
                            subSys = null;
                            subList = new List<T>();//开始一个子系统
                            sysList.Add(subList);
                        }
                    }
                    else
                    {
                        subSys = kksNew;
                    }
                }
                else
                {
                    if (kksNew.Name.EndsWith("系统"))
                    {
                        subSys = kksNew;
                    }
                    else //土建等没有*的表
                    {
                        if (sysRoot == null)
                        {
                            sysRoot = kksNew;
                            subSys = null;
                            subList = new List<T>();//开始一个子系统
                            sysList.Add(subList);
                        }
                    }
                }
                int nCount = list.FindAll(p => p.Code == kksNew.Code && p.Name == kksNew.Name).Count;
                if (nCount >= 1)
                {
                    continue;
                }
                string[] codeParts = kksNew.Code.Split(' ');
                int partNumber = codeParts.Length - 1;
                if (partNumber == 0)
                {
                    parent1 = kksNew;
                }
                else if (partNumber == 1)
                {
                    parent2 = kksNew;
                    kksNew.SetParent(parent1);
                }
                else
                {
                    kksNew.SetParent(parent1);
                }
                if (string.IsNullOrEmpty(kksNew.ParentCode))
                    for (int i = subList.Count - 1; i >= 0; i--)
                    {
                        T kks = subList[i];
                        if (kksNew.Code.StartsWith(kks.Code))
                        {
                            kksNew.SetParent(kks);
                            break;
                        }
                        else //再找下一个
                        {
                            //if (kks.ParentCode == "")//一张表的根节点
                            //{
                            //    kks1.ParentCode = kks.Code;
                            //    break;
                            //}
                            //else//再找下一个
                            //{

                            //}
                        }
                    }

                if (string.IsNullOrEmpty(kksNew.ParentCode) && kksNew!= sysRoot)
                {
                    if (kksNew.Serial.Contains("*"))//子系统
                    {
                        kksNew.SetParent(sysRoot);//设置为子系统的根节点
                    }
                    else
                    {
                        if (subSys != null && kksNew!= subSys)
                        {
                            kksNew.SetParent(subSys);//子系统下
                        }
                        else
                        {
                            kksNew.SetParent(sysRoot);//设置为子系统的根节点
                        }
                    }
                }

                kksNew.SetSystem(sysRoot, subSys);
                subList.Add(kksNew);
            }

            foreach (List<T> sys in sysList)
            {
                foreach (T kks in sys)
                {
                    list.Add(kks);
                }
            }
            return list;
        }

        public static void ImportKKSCodeFromFile<T>(FileInfo file)
            where T : KKSCode, new()
        {
            Log.InfoStart(LogTags.KKS, "KKSCodeHelper.ImportKKSCodeFromFile:"+ file);
            if (file.Exists == false)
            {
                Log.Info("不存在文件:" + file.FullName);
            }
            Bll bll = new Bll();
            List<KKSCode> kksList = bll.KKSCodes.ToList();
            if (kksList != null /*&& kksList.Count == 0*/)
            {
                List<KKSCode> list = CreateKKSCodeListFromFile<KKSCode>(file);
                if (list.Count == 0)
                {

                }
                bool r=bll.KKSCodes.AddRange(bll.Db, list); //新增的部分
                if (r == false)
                {
                    Log.Error(LogTags.KKS, "KKSCodeHelper.ImportKKSCodeFromFile 添加失败:"+file);
                }
                else
                {
                    Log.Info(LogTags.KKS, "KKS数量:" + list.Count);
                }
            }
            Log.InfoEnd("KKSCodeHelper.ImportKKSCodeFromFile");
        }

        public static void ImportKKSNodeFromFile<T>(FileInfo file)
            where T : KKSCode, new()
        {
            if (file.Exists == false)
            {
                Log.Info("不存在文件:" + file.FullName);
            }
            Log.Info(LogTags.KKS, string.Format("读取文件"));
            DataTable dt = ExcelHelper.Load(new FileInfo(file.FullName), true).Tables[0].Copy();
            dt.Rows.RemoveAt(0);//备注行
            Log.Info(LogTags.KKS, string.Format("解析数据"));
            Dictionary<string,KKSCode> kksList = new Dictionary<string, KKSCode>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow dr = dt.Rows[j];
                string code = dr[1].ToString().Trim();
                string name = dr[2].ToString().Trim();
                string nodeType = dr[3].ToString().Trim();//节点类型：机组、系统、子系统、设备、部件
                string parentCode = dr[4].ToString().Trim();
                string codeType = dr[5].ToString().Trim();//专业
                KKSCode kks=new KKSCode();
                if (code.StartsWith("SH"))
                {
                    code = code.Substring(2);
                }
                if (parentCode.StartsWith("SH"))
                {
                    parentCode = parentCode.Substring(2);
                }
                kks.Code = code;
                kks.Name = name;
                kks.MainType = nodeType;
                kks.SubType = codeType;
                kks.ParentCode = parentCode;
                if (!kksList.ContainsKey(code))
                {
                    kksList.Add(code, kks);
                }
                else
                {

                }
            }

            Bll bll = new Bll();
            Log.Info(LogTags.KKS, string.Format("清空数据"));
            bool r1 = bll.KKSCodes.Clear(1);

            List<KKSCode> list = kksList.Values.ToList();
            Log.Info(LogTags.KKS, string.Format("写入数据"));
            if (bll.KKSCodes.AddRange(bll.Db, list) == false)
            {
                Log.Error(LogTags.KKS, "写入失败:" + bll.KKSCodes.ErrorMessage);
            }
            else
            {
                Log.Info(LogTags.KKS, "KKS数量:" + list.Count);
            }

            //List<KKSCode> temp = new List<KKSCode>();
            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (i % 100 == 0)
            //    {
            //        if (bll.KKSCodes.AddRange(bll.Db, temp) == false)
            //        {
            //            Log.Error(LogTags.KKS, "写入失败:" + bll.KKSCodes.ErrorMessage);
            //        }
            //        else
            //        {
            //            Log.Info(LogTags.KKS, "KKS数量:" + temp.Count);
            //        }
            //        temp.Clear();
            //    }
            //    temp.Add(list[i]);
            //}
            //if (bll.KKSCodes.AddRange(bll.Db, temp) == false)
            //{
            //    Log.Error(LogTags.KKS, "写入失败:" + bll.KKSCodes.ErrorMessage);
            //}
            //else
            //{
            //    Log.Info(LogTags.KKS, "KKS数量:" + temp.Count);
            //}
        }

        public static KKSImportInfo<T> ImportKKSFromDirectory<T>(string folderPath, List<T> kksList) where T : KKSCode, new()
        {
            //List<KKSCode> kksList = bll.KKSCodes.ToList();
            KKSImportInfo<T> importInfo = new KKSImportInfo<T>();

            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                foreach (FileInfo fileInfo in subDir.GetFiles())
                {
                    string strFolderName = fileInfo.Directory.Name;
                    //DataTable dtTable = ExcelHelper.Load(new FileInfo(fileInfo.FullName), false).Tables[0].Copy();
                    DataSet ds = ExcelHelper.Load(new FileInfo(fileInfo.FullName), false);
                    if (ds == null || ds.Tables.Count < 1)
                    {
                        continue;
                    }
                    DataTable dtTable = ds.Tables[0].Copy();
                    dtTable.Rows.RemoveAt(0);
                    dtTable.Rows.RemoveAt(0);
                    List<T> list1 = CreateKKSCodeListFromDataTable<T>(strFolderName,
                        dtTable);
                    foreach (T item in list1)
                    {
                        ToKKSCodeList<T>(item, kksList, importInfo);
                    }
                }
            }

            //bll.KKSCodes.EditRange(bll.Db, listEditInfo); //修改的部分
            //bll.KKSCodes.AddRange(bll.Db, listAddInfo); //新增的部分

            return importInfo;
        }


        public static void TableToList(string strFolderName, DataTable dtTable, List<KKSCode> kksList,KKSImportInfo<KKSCode> importInfo1)
        {
            foreach (DataRow dr in dtTable.Rows)
            {
                var kks1 = CreateKKSCodeFromDataRow<KKSCode>(strFolderName, dr);//根据表格内容创建KKS对象
                ToKKSCodeList<KKSCode>(kks1, kksList, importInfo1);
            }
        }

        public static void ToKKSCodeList<T>(T kks, List<T> kksList, KKSImportInfo<T> importInfo) where T : IKKSCode, new()
        {
            bool bFlag = false;

            if (kksList != null)
            {
                T kks0 = kksList.Find(i => i.Code == kks.Code && i.System == kks.System);
                if (kks0 != null)
                {
                    if (kks0.Name != kks.Name)
                    {
                        bFlag = true;
                        kks0.Name = kks.Name;
                    }

                    if (kks0.DesinCode != kks.DesinCode)
                    {
                        bFlag = true;
                        kks0.DesinCode = kks.DesinCode;
                    }

                    if (bFlag)
                    {
                        bFlag = false;
                        importInfo.listEditInfo.Add(kks0);
                    }

                    return;
                }
                else
                {
                    kks0 = importInfo.listAddInfo.Find(i => i.Code == kks.Code && i.System == kks.System);
                    if (kks0 != null)
                    {
                        return;
                    }
                }
            }
            importInfo.listAddInfo.Add(kks);
        }

        public static List<T> CreateKKSCodeListFromFile<T>(FileInfo fileInfo) where T : KKSCode, new()
        {
            string strFolderName = fileInfo.Directory.Name;
            DataTable dtTable = ExcelHelper.Load(new FileInfo(fileInfo.FullName), false).Tables[0].Copy();
            dtTable.Rows.RemoveAt(0);
            dtTable.Rows.RemoveAt(0);
            List<T> list1 = CreateKKSCodeListFromDataTable<T>(strFolderName,dtTable);
            return list1;
        }

        #region
        /// <summary>
        /// 转义为KKS编码,通过 EDOSOriginalCode.xls 文件转义
        /// </summary>
        public static int OriginalKKSCode(FileInfo file, string createfilePath)
        {
            if (file.Exists == false)
            {
                return 1;
            }
            Bll bll = new Bll();
            List<KKSCode> kksList = bll.KKSCodes.ToList();
            if (kksList != null /*&& kksList.Count == 0*/)
            {
                string strFolderName = file.Directory.Name;
                DataTable table = new DataTable();
                table.Columns.Add("标签名", typeof(string));
                table.Columns.Add("数据库标签名", typeof(string));
                table.Columns.Add("描述", typeof(string));
                table.Columns.Add("单位", typeof(string));
                table.Columns.Add("类型", typeof(string));
                table.Columns.Add("转义后的KKS编码", typeof(string));
                table.Columns.Add("转义后的父类KKS编码", typeof(string));
                
                DataTable dtTable = ExcelHelper.Load(new FileInfo(file.FullName), false).Tables[0].Copy();
                dtTable.Rows.RemoveAt(0);

                foreach (DataRow dr in dtTable.Rows)
                {
                    string strTagName = dr[0].ToString();
                    int index = strTagName.IndexOf('.');
                    strTagName = strTagName.Substring(index + 1);
                    strTagName = strTagName.Replace("_", " ");
                    string strNodeKKS = strTagName;
                    TagToKKSInfo tagToKKS = GetKKSCode(strTagName, kksList);

                    DataRow dr2 = table.NewRow();
                    dr2["标签名"] = dr[0].ToString();
                    dr2["数据库标签名"] = dr[2].ToString();
                    dr2["描述"] = dr[3].ToString();
                    dr2["单位"] = dr[4].ToString();
                    dr2["类型"] = dr[5].ToString();
                    dr2["转义后的KKS编码"] = strNodeKKS;
                    dr2["转义后的父类KKS编码"] = tagToKKS.Key;
                    table.Rows.Add(dr2);
                }
                
                ExcelHelper.Save(table, new FileInfo(createfilePath), null);
            }

            return 0;
        }

        /// <summary>
        /// 转义为KKS编码,通过 UDPoints20190321.xlsx 文件转义
        /// </summary>
        /// <param name="file"></param>
        /// <param name="createfilePath"></param>
        /// <returns></returns>
        public static DataTable OriginalKKSCode_New(FileInfo file, bool loadAll)
        {
            if (file.Exists == false)
            {
                return null;
            }

            Bll bll = new Bll();
            List<KKSCode> kksList = bll.KKSCodes.ToListEx();
            string strFolderName = file.Directory.Name;
            DataTable table = new DataTable();
            table.Columns.Add("标签名", typeof(string));
            table.Columns.Add("数据库标签名", typeof(string));
            table.Columns.Add("描述", typeof(string));
            table.Columns.Add("单位", typeof(string));
            table.Columns.Add("类型", typeof(string));
            table.Columns.Add("KKS编码", typeof(string));
            table.Columns.Add("父类KKS编码", typeof(string));
            table.Columns.Add("父类KKS编码类型", typeof(string));

            DataTable dtTable = ExcelHelper.Load(new FileInfo(file.FullName), false).Tables[0].Copy();
            dtTable.Rows.RemoveAt(0);
            int count = 0;
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                DataRow dr = dtTable.Rows[i];

                string strTagName0 = dr[0].ToString();
                int index = strTagName0.IndexOf('.');
                string strTagName = strTagName0.Substring(index + 1);
                strTagName = strTagName.Replace("_", " ");
                string strNodeKKS = strTagName;
                string describe = dr[2].ToString().Trim();
                if (describe == "Ovation Control Builder")
                {
                    continue; //不用管的
                }


                TagToKKSInfo strDevKKS = GetKKSCode(strTagName, kksList);

                if (strDevKKS.Key == "")
                {
                    if (loadAll == false)
                    {
                        continue; //测点不存在对应的kks则过滤掉
                    }
                }

                DataRow dr2 = table.NewRow();
                dr2["标签名"] = dr[0].ToString();
                dr2["数据库标签名"] = "";
                dr2["描述"] = dr[2].ToString();
                dr2["单位"] = dr[3].ToString();
                dr2["类型"] = dr[4].ToString();
                dr2["KKS编码"] = strNodeKKS;
                dr2["父类KKS编码"] = strDevKKS.GetParentCode();
                dr2["父类KKS编码类型"] = strDevKKS.Type;
                if (strDevKKS.Type == 0)
                {
                    count++;
                }
                table.Rows.Add(dr2);

                if (i % 20 == 0)
                {
                    Log.Info(LogTags.KKS,
                        string.Format("获取数据:({2}/{3},{4:P2}){0}[{1}]", dr[2].ToString(), strTagName, i,
                            dtTable.Rows.Count, (float) i / (float) dtTable.Rows.Count));
                }
            }
            Log.Info(LogTags.KKS,string.Format("找到匹配KKS数量:{0}", count));
            //ExcelHelper.Save(table, new FileInfo(createfilePath), null);
            return table;
        }

        public class TagToKKSInfo
        {
            public string TagName { get; set; }

            public string Key { get; set; }

            public List<KKSCode> KKSList { get; set; }

            public KKSCode KKS { get; set; }

            public TagToKKSInfo(string tagName)
            {
                TagName = tagName;
                Key = tagName;
            }

            /// <summary>
            /// 0:没找到
            /// </summary>
            public int Type { get; set; }

            public string GetParentCode()
            {
                string parentCode = "";
                if (KKSList.Count == 0)
                {
                    Type = -1;
                }
                else if (KKSList.Count > 100)//太多了
                {
                    Type = -2;
                }
                else
                {
                    parentCode = Key;
                    if (KKSList.Count == 1)
                    {
                        KKS = KKSList[0];
                        if (KKSList[0].Code == Key)
                        {
                            Type = 0;//找到
                        }
                        else
                        {
                            Type = 1;//说明没找到完全匹配的
                            //parentCode = KKS.ParentCode;//应该是和KKS的结点同一级的
                            parentCode = KKS.Code;
                        }
                    }
                    else
                    {
                        Type = KKSList.Count;//说明没找到完全匹配的，这个就不清楚情况了
                    }
                }
                return parentCode;
            }
        }

        public static Dictionary<string, List<KKSCode>> kksDict = new Dictionary<string, List<KKSCode>>();

        public static List<KKSCode> FindCommonAncestor(List<KKSCode> input)
        {
            List<KKSCode> kksDevs = new List<KKSCode>();
            foreach (var item in input)
            {
                if(!kksDevs.Contains(item.Parent))
                    kksDevs.Add(item.Parent);
            }

            var result = FindCommonParent(kksDevs);
            return result;
        }

        public static TagToKKSInfo ReParese(List<KKSCode> kksCodes,string parentKKS,string kks)
        {
            List<KKSCode> kksDevs = kksCodes.FindAll(i => i.Code.StartsWith(parentKKS));
            List<KKSCode> kksDevs2 = KKSCodeHelper.FindCommonParent(kksDevs);
            if (kksDevs2.Count == 1)
            {
                //monitorNode.ParentKKS = kksDevs2[0].Code;
                //monitorNode.ParseResult = 1;
                //editMonitorNodes.Add(monitorNode);

                TagToKKSInfo info=new TagToKKSInfo(parentKKS);
                info.KKSList = kksDevs2;
                return info;
            }
            else
            {
                List<KKSCode> kksDevs3 = KKSCodeHelper.FindCommonAncestor(kksDevs2);
                if (kksDevs3.Count == 1)
                {
                    //monitorNode.ParentKKS = kksDevs3[0].Code;
                    //monitorNode.ParseResult = 1;
                    //editMonitorNodes.Add(monitorNode);

                    TagToKKSInfo info = new TagToKKSInfo(parentKKS);
                    info.KKSList = kksDevs3;
                    return info;
                }
                else
                {
                    TagToKKSInfo info = KKSCodeHelper.GetKKSCode(parentKKS, kksDevs3);
                    var code = info.GetParentCode();

                    //if (info.Type == 1)
                    //{
                    //    monitorNode.ParentKKS = code;
                    //    monitorNode.ParseResult = info.Type;

                    //    editMonitorNodes.Add(monitorNode);
                    //}
                    //else
                    //{

                    //}
                    return info;
                }
            }
        }

        public static List<KKSCode> FindCommonParent(List<KKSCode> input)
        {
            List<KKSCode> kksDevs = new List<KKSCode>();
            kksDevs.AddRange(input);

            int count1 = 0;
            int count2 = 1;
            do
            {
                count1 = kksDevs.Count;
                for (int i = 0; i < kksDevs.Count; i++)
                {
                    KKSCode code = kksDevs[i];
                    //KKSCode parent = code.Parent;
                    //if (kksDevs.Contains(parent))
                    //{
                    //    kksDevs.RemoveAt(i);
                    //    i--;
                    //}

                    var ancestors = code.GetAncestors();
                    foreach (KKSCode ancestor in ancestors)
                    {
                        if (kksDevs.Contains(ancestor))
                        {
                            kksDevs.RemoveAt(i);
                            i--;
                            break;
                        }
                    }

                }//删除子节点

                count2 = kksDevs.Count;

                bool isSameParent = true;
                KKSCode commonParent = null;
                foreach (var kksDev in kksDevs)
                {
                    if (commonParent == null)
                    {
                        commonParent = kksDev.Parent;
                    }
                    else
                    {
                        if (commonParent != kksDev.Parent)
                        {
                            isSameParent = false;
                            break;
                        }
                    }
                }

                if (isSameParent && commonParent != null)
                {
                    return new List<KKSCode>() {commonParent};
                }

            } while (count1 != count2);

            return kksDevs;
        }

        public static TagToKKSInfo GetKKSCode(string tagName, List<KKSCode> kksList)
        {
            TagToKKSInfo info = new TagToKKSInfo(tagName);
            if (tagName == "")
            {
                return info;
            }

            string key = tagName;
            List<KKSCode> kksCodes = null;
            while (key.Length > 0)
            {
                if (kksDict.ContainsKey(key))
                {
                    kksCodes = kksDict[key]; //从缓存取，考虑可能很多个测点是相同的kks
                }
                else
                {
                    kksCodes = kksList.Where(p => p.Code.StartsWith(key)).ToList();

                    if (kksCodes.Count > 0 && key.Length > 1)
                    {
                        KKSCode kks = kksCodes.Find(p => p.Code == key);//完全相同的
                        if (kks != null)
                        {
                            kksCodes = new List<KKSCode>() { kks };//使用一个
                        }
                        else
                        {
                            var kksCodes2 = FindCommonParent(kksCodes);
                            //if (kksCodes2.Count > 1)
                            //{
                            //    info = ReParese(kksList, key, tagName);
                            //    return info;
                            //}
                            //else
                            //{
                            //    kksCodes = kksCodes2;
                            //}

                            kksCodes = kksCodes2;
                        }
                    }
                    kksDict.Add(key, kksCodes);
                }
                if (kksCodes.Count == 0)
                {
                    int nLength = key.Length;
                    key = key.Substring(0, nLength - 1);
                }
                else
                {
                    info.Key = key;
                    break;
                }
            }

            info.KKSList = kksCodes;
            return info;
        }

        #endregion

    }
}
