using DbModel.Location.Person;
using DbModel.Tools;
using DbModel.Tools.InitInfos;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tools
{
    public class PersonBackupHelper
    {
        /// <summary>
        /// 通过文件导入人员信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="devBll"></param>
        /// <returns></returns>
        public static bool ImportPersonInfoFromFile(string filePath, Bll bll)
        {
            if (!File.Exists(filePath) || bll == null)
            {
                Log.Error("文件不存在:" + filePath);
                return false;
            }
            var initInfo = XmlSerializeHelper.LoadFromFile<PersonnelInfoBackupList>(filePath);
            if (initInfo == null || initInfo.PerList == null || initInfo.PerList.Count == 0) return false;
            var areas = bll.Areas.ToList();
            var deps = bll.Departments.ToList();
            foreach (var Per in initInfo.PerList)
            {
                var dep = deps.Find(i => i.Id == Per.ParentId);
                if (dep == null)
                {
                    dep = deps[Per.ParentId];
                    Per.ParentId = dep.Id;
                }
                else
                {

                }
                AddPersonInfo(Per, bll);
            }
            return true;
        }
        /// <summary>
        /// 添加门禁卡信息
        /// </summary>
        /// <param name="cameraDev"></param>
        /// <param name="bll"></param>
        private static void AddPersonInfo(PersonnelInfoBackup PerB, Bll bll)
        {
            try
            {
                Personnel Per = GetPersonInfo(PerB);
                bll.Personnels.Add(Per);
            }
            catch (Exception e)
            {
                Log.Info("Error in DepartmentsBackupHelper.AddDepartmentInfo:" + e.ToString());
            }
        }


        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        private static Personnel GetPersonInfo(PersonnelInfoBackup PerB)
        {
            Personnel Per = new Personnel();

            if (PerB.Abutment_Id != -1)
            {
                Per.Abutment_Id = PerB.Abutment_Id;
            }
            else
            {
                Per.Abutment_Id = null;
            }

            Per.Name = PerB.Name;
            Per.Sex = PerB.Sex;
            Per.Phone = PerB.Photo;
            Per.BirthDay = PerB.BirthDay;
            Per.BirthTimeStamp = PerB.BirthTimeStamp;
            Per.Nation = PerB.Nation;
            Per.Address = PerB.Address;

            if (PerB.WorkNumber != null)
            {
                Per.WorkNumber = PerB.WorkNumber;
            }
            else
            {
                Per.WorkNumber = null;
                //Per.WorkNumberNew = null;
            }

            Per.Email = PerB.Email;
            Per.Phone = PerB.Phone;
            Per.Mobile = PerB.Mobile;
            Per.Enabled = PerB.Enabled;

            if (PerB.ParentId != -1)
            {
                Per.ParentId = PerB.ParentId;
            }
            else
            {
                Per.ParentId = null;
            }

            Per.Pst = PerB.Pst;

            return Per;
        }
    }
}
