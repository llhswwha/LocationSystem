using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tools
{
    public static class DbInfoHelper
    {
        public static string baseDir;
        static DbInfoHelper()
        {
            baseDir = AppDomain.CurrentDomain.BaseDirectory + "Data\\DbInfos\\";
        }
        public static List<ArchorSetting> GetArchorSettings()
        {
            return DbInitializer.LoadExcelToList<ArchorSetting>(baseDir + "ArchorSetting.xls");
        }

        public static List<DevModel> GetDevModels()
        {
            return DbInitializer.LoadExcelToList<DevModel>(baseDir + "DevModel.xls");
        }

        public static List<DevType> GetDevTypes()
        {
            return DbInitializer.LoadExcelToList<DevType>(baseDir + "DevType.xls");
        }

        public static List<Archor> GetArchors()
        {
            return DbInitializer.LoadExcelToList<Archor>(baseDir + "Archor.xls");
        }

        public static List<DevInfo> GetDevInfos()
        {
            return DbInitializer.LoadExcelToList<DevInfo>(baseDir + "DevInfo.xls");
        }
    }
}
