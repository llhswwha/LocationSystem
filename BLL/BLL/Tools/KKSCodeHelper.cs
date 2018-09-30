using System.Collections.Generic;
using System.Data;
using System.IO;
using BLL;
using DbModel.Location.AreaAndDev;
using ExcelLib;
using Location.IModel;

namespace Location.BLL.Tool
{
    public class KKSImportInfo<T> where T : IKKSCode, new()
    {
        public List<T> listAddInfo = new List<T>();
        public List<T> listEditInfo = new List<T>();

    }
    public static class KKSCodeHelper
    {
        public static T CreateKKSCode<T>(string Serial, string Name, string Code, string ParentCode, string DesinCode,
    string MainType, string SubType, string System) where T : IKKSCode, new()
        {
            T kks1 = new T();
            kks1.Serial = Serial;
            kks1.Name = Name;
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

            Serial = dr[0].ToString();
            Name = dr[1].ToString();
            Code = dr[2].ToString();
            DesinCode = dr[3].ToString();

            if (Serial == "*" && Code == "")
            {
                System = Name;
                return default(T);
            }


            var kks1 = CreateKKSCode<T>(Serial, Name, Code, ParentCode, DesinCode, MainType, SubType, System);
            return kks1;
        }


        public static List<T> CreateKKSCodeListFromDataTable<T>(string mainType, DataTable dt) where T : IKKSCode, new()
        {
            List<T> list = new List<T>();

            string CodeCopy = "";
            string CodeCopy2 = "";
            char ch = ' ';

            foreach (DataRow dr in dt.Rows)
            {
                T kks1 = CreateKKSCodeFromDataRow<T>(mainType, dr);//根据表格内容创建KKS对象
                int nSplit = kks1.Code.Split(ch).Length - 1;
                if (nSplit == 0)
                {
                    CodeCopy = kks1.Code;
                }
                else if (nSplit == 1)
                {
                    CodeCopy2 = kks1.Code;
                    kks1.ParentCode = CodeCopy;
                }
                else
                {
                    kks1.ParentCode = CodeCopy;
                }
                if (string.IsNullOrEmpty(kks1.ParentCode))
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        T kks = list[i];
                        if (kks1.Code.StartsWith(kks.Code))
                        {
                            kks1.ParentCode = kks.Code;
                            break;
                        }
                    }
                list.Add(kks1);
            }
            return list;
        }

        public static void ImportKKSFromFile<T>(FileInfo file)
            where T : IKKSCode, new()
        {
            Log.InfoStart("KKSCodeHelper.ImportKKSFromFile");
            if (file.Exists == false)
            {
                Log.Info("不存在文件:" + file.FullName);
            }
            Bll bll = new Bll();
            List<KKSCode> kksList = bll.KKSCodes.ToList();
            if (kksList != null && kksList.Count == 0)
            {
                List<KKSCode> list = CreateKKSCodeListFromFile<KKSCode>(file);
                bll.KKSCodes.AddRange(bll.Db, list); //新增的部分
            }
            Log.InfoEnd("KKSCodeHelper.ImportKKSFromFile");
        }

        public static KKSImportInfo<T> ImportKKSFromDirectory<T>(string folderPath, List<T> kksList) where T : IKKSCode, new()
        {
            //List<KKSCode> kksList = bll.KKSCodes.ToList();
            KKSImportInfo<T> importInfo = new KKSImportInfo<T>();

            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                foreach (FileInfo fileInfo in subDir.GetFiles())
                {
                    string strFolderName = fileInfo.Directory.Name;
                    DataTable dtTable = ExcelHelper.Load(new FileInfo(fileInfo.FullName), false).Tables[0].Copy();
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

        public static List<T> CreateKKSCodeListFromFile<T>(FileInfo fileInfo) where T : IKKSCode, new()
        {
            string strFolderName = fileInfo.Directory.Name;
            DataTable dtTable = ExcelHelper.Load(new FileInfo(fileInfo.FullName), false).Tables[0].Copy();
            dtTable.Rows.RemoveAt(0);
            dtTable.Rows.RemoveAt(0);
            List<T> list1 = CreateKKSCodeListFromDataTable<T>(strFolderName,dtTable);
            return list1;
        }
    }
}
