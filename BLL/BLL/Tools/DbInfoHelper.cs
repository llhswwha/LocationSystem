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

        public static List<Archor> anchors = null;

        public static List<Archor> GetArchors()
        {
            //if(anchors== null)
            {
                anchors= DbInitializer.LoadExcelToList<Archor>(baseDir + "Archor.xls");
            }
            return anchors;
        }

        public static List<DevInfo> devInfos = null;

        public static List<DevInfo> GetDevInfos()
        {
            //if(devInfos== null)
            {
                devInfos = DbInitializer.LoadExcelToList<DevInfo>(baseDir + "DevInfo.xls");
            }
            
            return devInfos;
        }

        public static List<Area> areas;

        public static List<Area> GetAreas()
        {
            //if(areas== null)
            {
                areas= DbInitializer.LoadExcelToList<Area>(baseDir + "Area.xls");
            }
            return areas;
        }
    }
}
