using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using ExcelLib;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tools
{
    public static class LocationCardToPersonnelsBackupHelper
    {
        public static void ImportRelationFromFile(FileInfo file)
        {
            Log.InfoStart(LogTags.DbInit, "LocationCardToPersonnelsBackupHelper.ImportRelationFromFile");
            if (file.Exists == false)
            {
                Log.Info("不存在文件:" + file.FullName);
            }
            Bll bll = new Bll();
            List<LocationCardToPersonnel> rList = bll.LocationCardToPersonnels.ToList();
            if (rList != null)
            {
                List<Personnel> pList = bll.Personnels.ToList();
                List<LocationCard> LcList = bll.LocationCards.ToList();
                List<LocationCardToPersonnel> list = CreateRelationListFromFile(file, pList, LcList);
                bll.LocationCardToPersonnels.AddRange(bll.Db, list); //新增的部分
            }
            Log.InfoEnd("LocationCardToPersonnelsBackupHelper.ImportRelationFromFile");
        }

        public static List<LocationCardToPersonnel> CreateRelationListFromFile(FileInfo fileInfo, List<Personnel> pList, List<LocationCard> LcList)
        {
            string strFolderName = fileInfo.Directory.Name;
            DataTable dtTable = ExcelHelper.Load(new FileInfo(fileInfo.FullName), false).Tables[0].Copy();
            dtTable.Rows.RemoveAt(0);
            List<LocationCardToPersonnel> list1 = CreateRelationListFromDataTable(strFolderName, dtTable, pList, LcList);
            return list1;
        }

        public static List<LocationCardToPersonnel> CreateRelationListFromDataTable(string mainType, DataTable dt, List<Personnel> pList, List<LocationCard> LcList)
        {
            List<LocationCardToPersonnel> list = new List<LocationCardToPersonnel>();

            foreach (DataRow dr in dt.Rows)
            {
                LocationCardToPersonnel lcp = CreateRelationFromDataRow(mainType, dr, ref pList, ref LcList);//根据表格内容创建KKS对象
                if (lcp == null) continue;
                int nCount = list.FindAll(p => p.PersonnelId == lcp.PersonnelId && p.LocationCardId == lcp.LocationCardId).Count;
                if (nCount >= 1)
                {
                    continue;
                }
                
                list.Add(lcp);
            }
            return list;
        }

        public static LocationCardToPersonnel CreateRelationFromDataRow(string mainType, DataRow dr, ref List<Personnel> pList, ref List<LocationCard> LcList)
        {
            DataTable dt = dr.Table;
            string strPersonName = dr[0].ToString();
            string strCode = dr[1].ToString();
            if (strPersonName == "" || strCode == "")
            {
                return null;
            }

            Bll bll = new Bll();
            int nCount = pList.Where(p => p.Name == strPersonName).Count();
            int nCount2 = LcList.Where(p => p.Code == strCode).Count();
            if (nCount == 0 || nCount2 == 0)
            {
                return null;
            }

            int nPId = pList.Where(p => p.Name == strPersonName).Select(p => p.Id).FirstOrDefault();
            int nCId = LcList.Where(p => p.Code == strCode).Select(p => p.Id).FirstOrDefault();

            LocationCardToPersonnel lcp = new LocationCardToPersonnel();
            lcp.PersonnelId = nPId;
            lcp.LocationCardId = nCId;
            
            return lcp;
        }
    }
}
