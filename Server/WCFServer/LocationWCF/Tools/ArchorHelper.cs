using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Tools
{
    public static class ArchorHelper
    {
        /// <summary>
        /// 厂区提供基站信息
        /// </summary>
        public static ArchorDevList ArchorList;
        public static ArchorDevList LoadArchoDevInfo()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\基站信息\\ArchorFiles.xml";
            var initInfo = XmlSerializeHelper.LoadFromFile<ArchorDevList>(filePath);
            if (initInfo != null)
                ArchorList = initInfo;
            return initInfo;
        }
    }
}
