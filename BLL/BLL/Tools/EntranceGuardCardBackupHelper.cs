using DbModel.Location.AreaAndDev;
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
    public class EntranceGuardCardBackupHelper
    {
        /// <summary>
        /// 通过文件导入门禁卡信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="devBll"></param>
        /// <returns></returns>
        public static bool ImportEntranceGuardCardInfoFromFile(string filePath, Bll bll)
        {
            if (!File.Exists(filePath) || bll == null)
            {
                Log.Error("文件不存在:" + filePath);
                return false;
            }
            var initInfo = XmlSerializeHelper.LoadFromFile<EntranceGuardCardInfoBackupList>(filePath);
            if (initInfo == null || initInfo.EcList == null || initInfo.EcList.Count == 0) return false;
            var areas = bll.Areas.ToList();
            foreach (var ec in initInfo.EcList)
            {
                AddEntranceGuardCardInfo(ec, bll);
            }
            return true;
        }
        /// <summary>
        /// 添加门禁卡信息
        /// </summary>
        /// <param name="cameraDev"></param>
        /// <param name="bll"></param>
        private static void AddEntranceGuardCardInfo(EntranceGuardCardInfoBackup ec, Bll bll)
        {
            try
            {
                EntranceGuardCard EgCard = GetCardInfo(ec);
                bll.EntranceGuardCards.Add(EgCard);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in EntranceGuardCardBackupHelper.AddEntranceGuardCardInfo:" + e.ToString());
            }
        }


        /// <summary>
        /// 获取门禁卡信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        private static EntranceGuardCard GetCardInfo(EntranceGuardCardInfoBackup ec)
        {
            EntranceGuardCard EgCard = new EntranceGuardCard();
            if (ec.Abutment_Id != -1)
            {
                EgCard.Abutment_Id = ec.Abutment_Id;
            }
            else
            {
                EgCard.Abutment_Id = null;
            }

            EgCard.Code = ec.Code;
            EgCard.State = ec.State;

            return EgCard;
        }
    }
}
