using System;
using System.IO;
using BLL.Blls.Location;
using DbModel.Tools;
using Location.BLL.Tool;
using Location.Model.InitInfos;
using Location.TModel.Location.AreaAndDev;
using DevInfo = DbModel.Location.AreaAndDev.DevInfo;

namespace BLL.Tools
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
                Log.Error("文件不存在:" + filePath);
                return false;
            }
            DevInfoBackupList initInfo = XmlSerializeHelper.LoadFromFile<DevInfoBackupList>(filePath);
            foreach (var devInfo in initInfo.DevList)
            {
                if (devInfo.TypeCode == LocationDeviceHelper.LocationDevTypeCode)
                {
                    continue;
                }
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
                devInfo.SetPos(devPos);
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
                Name = dev.Name,
                ParentId = TryParseInt(dev.ParentId),
                KKS = dev.KKSCode,
                Local_DevID = dev.DevId,
                Local_TypeCode = TryParseInt(dev.TypeCode),
                Status = Abutment_Status.正常,
                ModelName = dev.ModelName,
                IP = "",
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
