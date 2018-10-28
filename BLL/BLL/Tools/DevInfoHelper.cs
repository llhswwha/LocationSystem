using System;
using System.IO;
using BLL.Blls.Location;
using DbModel.Tools;
using Location.BLL.Tool;
using Location.Model.InitInfos;
using Location.TModel.Location.AreaAndDev;
using DevInfo = DbModel.Location.AreaAndDev.DevInfo;
using TModel.Tools;

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
        public static bool ImportDevInfoFromFile(string filePath, Bll bll)
        {
            if (!File.Exists(filePath) || bll == null)
            {
                Log.Error("文件不存在:" + filePath);
                return false;
            }
            var initInfo = XmlSerializeHelper.LoadFromFile<DevInfoBackupList>(filePath);
            //for (int i = 0; i < initInfo.DevList.Count; i++)
            //{
            //    if (initInfo.DevList[i].TypeCode == LocationDeviceHelper.LocationDevTypeCode)
            //    {
            //        initInfo.DevList.RemoveAt(i);
            //        i--;
            //    }
            //}
            var areas = bll.Areas.ToList();
            foreach (var devInfo in initInfo.DevList)
            {
                if (devInfo.TypeCode == LocationDeviceHelper.LocationDevTypeCode)
                {
                    continue;
                }
                var area= areas.Find(i => i.Name == devInfo.ParentName);
                if (area != null)
                    devInfo.ParentId = area.Id;
                AddDevInfo(devInfo, bll.DevInfos);
            }
            return true;
        }

        public static void RemoveArchorDev()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\设备信息\\DevInfoBackup.xml";
            var initInfo = XmlSerializeHelper.LoadFromFile<DevInfoBackupList>(filePath);
            for (int i = 0; i < initInfo.DevList.Count; i++)
            {
                if (initInfo.DevList[i].TypeCode == LocationDeviceHelper.LocationDevTypeCode)
                {
                    initInfo.DevList.RemoveAt(i);
                    i--;
                }
            }
            initInfo.DevList.Sort();
            XmlSerializeHelper.Save(initInfo, filePath);
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
                ParentId = dev.ParentId,
                KKS = dev.KKSCode,
                Local_DevID = dev.DevId,
                Local_TypeCode = dev.TypeCode.ToInt(),
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
