using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using DbModel.Tools;
using DbModel.Tools.InitInfos;
using BLL.Tools;

namespace LocationServer.Tools
{
    public class EntranceGuardCardBackupHelper
    {
        public void BackupEntranceGuardCardInfo(Action callBack = null)
        {
            Thread thread = new Thread(() =>
            {
                Bll bll = new Bll();
                List<EntranceGuardCard> Eclst = bll.EntranceGuardCards.ToList();
                Log.Info("Init EntranceGuardCards...");
                DateTime recordT = DateTime.Now;
                //1.备份门禁卡信息
                SaveBackupEntranceGuardCardInfo(Eclst);
                Log.Info(string.Format("Init EntranceGuardCards complete,cost time: {0}s.", (DateTime.Now - recordT).TotalSeconds.ToString("f5")));

                if (callBack != null) callBack();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void SaveBackupEntranceGuardCardInfo(List<EntranceGuardCard> Eclst)
        {
            EntranceGuardCardInfoBackupList backUpList = new EntranceGuardCardInfoBackupList();
            backUpList.EcList = new List<EntranceGuardCardInfoBackup>();
            foreach (var item in Eclst)
            {
                EntranceGuardCardInfoBackup Ec = new EntranceGuardCardInfoBackup();
                if (item.Abutment_Id != null)
                {
                    Ec.Abutment_Id = (int)item.Abutment_Id;
                }
                else
                {
                    Ec.Abutment_Id = -1;
                }

                Ec.Code = item.Code;
                Ec.State = item.State;

                backUpList.EcList.Add(Ec);
            }

            string initFile = InitPaths.GetBackupEntranceGuardCardInfo();
            XmlSerializeHelper.Save(backUpList, initFile, Encoding.UTF8);
        }
    }
}
