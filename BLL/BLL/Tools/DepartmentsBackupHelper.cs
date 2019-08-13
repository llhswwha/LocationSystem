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
    public class DepartmentsBackupHelper
    {
        /// <summary>
        /// 通过文件导入部门信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="devBll"></param>
        /// <returns></returns>
        public static bool ImportDepartmentInfoFromFile(string filePath, Bll bll)
        {
            if (!File.Exists(filePath) || bll == null)
            {
                Log.Error("文件不存在:" + filePath);
                return false;
            }
            var initInfo = XmlSerializeHelper.LoadFromFile<DepartmentInfoBackupList>(filePath);
            if (initInfo == null || initInfo.DepList == null || initInfo.DepList.Count == 0) return false;
            var areas = bll.Areas.ToList();
            foreach (var Dep in initInfo.DepList)
            {
                AddDepartmentInfo(Dep, bll);
            }
            return true;
        }
        /// <summary>
        /// 添加门禁卡信息
        /// </summary>
        /// <param name="cameraDev"></param>
        /// <param name="bll"></param>
        private static void AddDepartmentInfo(DepartmentInfoBackup DepB, Bll bll)
        {
            try
            {
                Department Dep = GetDepartmentInfo(DepB);
                bll.Departments.Add(Dep);
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
        private static Department GetDepartmentInfo(DepartmentInfoBackup DepB)
        {
            Department Dep = new Department();
            if (DepB.Abutment_Id != -1)
            {
                Dep.Abutment_Id = DepB.Abutment_Id;
            }
            else
            {
                Dep.Abutment_Id = null;
            }

            Dep.Name = DepB.Name;

            if (DepB.ParentId != -1)
            {
                Dep.ParentId = DepB.ParentId;
            }
            else
            {
                Dep.ParentId = null;
            }

            if (DepB.Abutment_ParentId != -1)
            {
                Dep.Abutment_ParentId = DepB.Abutment_ParentId;
            }
            else
            {
                Dep.Abutment_ParentId = null;
            }

            Dep.ShowOrder = DepB.ShowOrder;
            Dep.Type = DepB.Type;
            Dep.Description = DepB.Description;

            return Dep;
        }
    }
}
