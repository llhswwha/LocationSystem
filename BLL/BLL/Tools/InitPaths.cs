using DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tools
{
    public static class InitPaths
    {
        public static string GetBackupDepartmentsInfo()
        {
            return GetInitInfoDir() + "部门人员门禁卡信息\\BackupDepartmentsInfo.xml";
        }

        public static string GetBackupEntranceGuardCardInfo()
        {
            return GetInitInfoDir() + "部门人员门禁卡信息\\BackupEntranceGuardCardInfo.xml";
        }

        public static string GetBackupPersonnelInfo()
        {
            return GetInitInfoDir() + "部门人员门禁卡信息\\BackupPersonnelInfo.xml";
        }

        public static string GetPersonAndCard()
        {
            return GetInitInfoDir()  +"部门人员门禁卡信息\\人员和定位卡对应关系.xlsx";
        }

        public static string GetInitInfo()
        {
            return GetInitInfoDir() + "InitInfo.xml";
        }

        public static string GetInitInfoDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Data\\InitInfos\\" + AppSetting.ParkName + "\\";
        }
    }
}
