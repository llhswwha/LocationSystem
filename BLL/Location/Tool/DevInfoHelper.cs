using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model.InitInfos;
using System.IO;
using Location.Model;
using Location.BLL.Blls;
using Location.Model.LocationTables;
namespace Location.BLL.Tool
{
    public class DevInfoHelper
    {
        /// <summary>
        /// 通过文件导入设备信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="devBll"></param>
        /// <returns></returns>
        public static bool ImportDevInfoFromFile(string filePath, DevInfoBll devBll)
        {
            if (!File.Exists(filePath) || devBll == null)
            {
                return false;
            }
            DevInfoBackupList initInfo = XmlSerializeHelper.LoadFromFile<DevInfoBackupList>(filePath);
            foreach (var devInfo in initInfo.DevList)
            {
                if (devInfo.TypeCode == LocationDeviceHelper.LocationDevTypeCode) continue;
                AddDevInfo(devInfo,devBll);
            }
            return true;
        }
        /// <summary>
        /// 添加设备信息
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="devBll"></param>
        private static void AddDevInfo(DevInfoBackup dev,DevInfoBll devBll)
        {
            try
            {
                DevInfo devInfo = GetDevInfo(dev);
                DevPos devPos = GetDevPos(dev);
                devInfo.Pos = devPos;
                devBll.Add(devInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DevInfoHelper.AddDevInfo:"+e.ToString());   
            }
        }
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        private static DevInfo GetDevInfo(DevInfoBackup dev)
        {
            DevInfo devInfo = new DevInfo
            {
                DevID = dev.DevId,
                IP = "",
                KKSCode = dev.KKSCode,
                Name = dev.Name,
                ModelName = dev.ModelName,
                Status = 0,
                ParentId = TryParseInt(dev.ParentId),
                TypeCode = TryParseInt(dev.TypeCode),
                UserName = "admin"
            };
            return devInfo;
        }
        /// <summary>
        /// 获取设备位置信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        private static DevPos GetDevPos(DevInfoBackup dev)
        {
            DevPos pos = new DevPos
            {
                DevID = dev.DevId,
                PosX = TryParseFloat(dev.XPos),
                PosY = TryParseFloat(dev.YPos),
                PosZ = TryParseFloat(dev.ZPos),
                RotationX = TryParseFloat(dev.RotationX),
                RotationY = TryParseFloat(dev.RotationY),
                RotationZ = TryParseFloat(dev.RotationZ),
                ScaleX = TryParseFloat(dev.ScaleX),
                ScaleY = TryParseFloat(dev.ScaleY),
                ScaleZ = TryParseFloat(dev.ScaleZ)
            };
            return pos;
        }
        private static int TryParseInt(string num)
        {
            try
            {
                return int.Parse(num);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        /// <summary>
        /// 字符转Float
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static float TryParseFloat(string num)
        {
            try
            {
                if (string.IsNullOrEmpty(num))
                {
                    return 0;
                }
                float value = float.Parse(num);
                return value;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
