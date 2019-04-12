using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DbModel.Location.Person;
using Location.BLL.Tool;
using DbModel.Tools.InitInfos;
using DbModel.Tools;

namespace LocationServer.Tools
{
    public class PersonBackupHelper
    {
        public void BackupPersonInfo(Action callBack = null)
        {
            Thread thread = new Thread(() =>
            {
                Bll bll = new Bll();
                List<Personnel> Perlst = bll.Personnels.ToList();
                Log.Info("Init PersonIs...");
                DateTime recordT = DateTime.Now;
                //1.备份人员信息
                SaveBackupPersonInfo(Perlst);
                Log.Info(string.Format("Init PersonIs complete,cost time: {0}s.", (DateTime.Now - recordT).TotalSeconds.ToString("f4")));

                if (callBack != null) callBack();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void SaveBackupPersonInfo(List<Personnel> Perlst)
        {
            PersonnelInfoBackupList backUpList = new PersonnelInfoBackupList();
            backUpList.PerList = new List<PersonnelInfoBackup>();
            foreach (var item in Perlst)
            {
                PersonnelInfoBackup Per = new PersonnelInfoBackup();
                if (item.Abutment_Id != null)
                {
                    Per.Abutment_Id = (int)item.Abutment_Id;
                }
                else
                {
                    Per.Abutment_Id = -1;
                }

                Per.Name = item.Name;
                Per.Sex = item.Sex;
                Per.Phone = item.Photo;
                Per.BirthDay = item.BirthDay;
                Per.BirthTimeStamp = item.BirthTimeStamp;
                Per.Nation = item.Nation;
                Per.Address = item.Address;


                if (item.WorkNumber != null)
                {
                    Per.WorkNumber = (int)item.WorkNumber;
                }
                else
                {
                    Per.WorkNumber = -1;
                }

                Per.Email = item.Email;
                Per.Phone = item.Phone;
                Per.Mobile = item.Mobile;
                Per.Enabled = item.Enabled;

                if (item.ParentId != null)
                {
                    Per.ParentId = (int)item.ParentId;
                }
                else
                {
                    Per.ParentId = -1;
                }

                Per.Pst = item.Pst;

                backUpList.PerList.Add(Per);
            }

            string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\部门人员门禁卡信息\\BackupPersonnelInfo.xml";
            XmlSerializeHelper.Save(backUpList, initFile, Encoding.UTF8);
        }
    }
}
