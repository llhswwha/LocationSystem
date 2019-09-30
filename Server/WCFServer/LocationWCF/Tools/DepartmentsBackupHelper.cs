using BLL;
using BLL.Tools;
using DbModel.Location.Person;
using DbModel.Tools;
using DbModel.Tools.InitInfos;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocationServer.Tools
{
    public class DepartmentsBackupHelper
    {
        public void BackupDepartmentsInfo(Action callBack = null)
        {
            Thread thread = new Thread(() =>
            {
                Bll bll = new Bll();
                List<Department> Deplst = bll.Departments.ToList();
                Log.Info("Init Departments...");
                DateTime recordT = DateTime.Now;
                //1.备份普通设备
                SaveBackupDepartmentsInfo(Deplst);
                Log.Info(string.Format("Init Departments complete,cost time: {0}s.", (DateTime.Now - recordT).TotalSeconds.ToString("f3")));
                
                if (callBack != null) callBack();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void SaveBackupDepartmentsInfo(List<Department> Deplst)
        {
            DepartmentInfoBackupList backUpList = new DepartmentInfoBackupList();
            backUpList.DepList = new List<DepartmentInfoBackup>();
            foreach (var item in Deplst)
            {
                DepartmentInfoBackup Dep = new DepartmentInfoBackup();
                if (item.Abutment_Id != null)
                {
                    Dep.Abutment_Id = (int)item.Abutment_Id;
                }
                else
                {
                    Dep.Abutment_Id = -1;
                }
               
                Dep.Name = item.Name;

                if (item.ParentId != null)
                {
                    Dep.ParentId = (int)item.ParentId;
                }
                else
                {
                    Dep.ParentId = -1;
                }


                if (item.Abutment_ParentId != null)
                {
                    Dep.Abutment_ParentId = (int)item.Abutment_ParentId;
                }
                else
                {
                    Dep.Abutment_ParentId = -1;
                }

                Dep.ShowOrder = item.ShowOrder;
                Dep.Type = item.Type;
                Dep.Description = item.Description;

                backUpList.DepList.Add(Dep);
            }

            string initFile = InitPaths.GetBackupDepartmentsInfo();
            XmlSerializeHelper.Save(backUpList, initFile, Encoding.UTF8);
        }
    }
}
