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
            string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\部门人员门禁卡信息\\BackupDepartmentsInfo.xml";
            return initFile;
        }

        public static string GetBackupEntranceGuardCardInfo()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\部门人员门禁卡信息\\BackupEntranceGuardCardInfo.xml";
            return filePath;
        }

        public static string GetBackupPersonnelInfo()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = basePath + "Data\\部门人员门禁卡信息\\BackupPersonnelInfo.xml";
            return filePath;
        }

        public static string GetPersonAndCard()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = basePath + "Data\\部门人员门禁卡信息\\人员和定位卡对应关系.xlsx";
            return filePath;
        }
    }
}
