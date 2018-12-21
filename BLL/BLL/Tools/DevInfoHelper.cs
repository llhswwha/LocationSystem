using System;
using System.IO;
using BLL.Blls.Location;
using DbModel.Tools;
using Location.BLL.Tool;
using Location.Model.InitInfos;
using Location.TModel.Location.AreaAndDev;
using DevInfo = DbModel.Location.AreaAndDev.DevInfo;
using TModel.Tools;
using IModel.Enums;
using Assets.z_Test.BackUpDevInfo;
using DbModel.Location.AreaAndDev;
using System.Collections.Generic;

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
            var areas = bll.Areas.ToList();
            foreach (var devInfo in initInfo.DevList)
            {
                if (devInfo.TypeCode == TypeCodes.Archor+"")
                {
                    continue;
                }
                string[] value = devInfo.ParentName.Split('|');
                //电子设备间|集控楼0m层
                if (value.Length < 2) continue;
                string parentName = value[0];
                List<Area> arealist = areas.FindAll(i => (i.Name == parentName));//找到所有 电子设备间
                if (arealist.Count > 1)
                {
                    foreach (var areaT in arealist)
                    {
                        Area areaParent = areas.Find(i => i.Id == areaT.ParentId);
                        if (areaParent.Name == value[1])  //找到父节点匹配的 ->集控楼0m层
                        {
                            devInfo.ParentId = areaT.Id;
                        }
                    }
                }
                else
                {
                    Area areaT = arealist[0];
                    if (areaT != null)
                        devInfo.ParentId = areaT.Id;
                }
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
                if (initInfo.DevList[i].TypeCode == TypeCodes.Archor+"")
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
        private static int TryParseInt(string num)
        {
            try
            {
                if (string.IsNullOrEmpty(num))
                {
                    return 0;
                }
                int value = int.Parse(num);
                return value;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        #region CameraInfoBackup

        /// <summary>
        /// 通过文件导入设备信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="devBll"></param>
        /// <returns></returns>
        public static bool ImportCameraInfoFromFile(string filePath, Bll bll)
        {
            if (!File.Exists(filePath) || bll == null)
            {
                Log.Error("文件不存在:" + filePath);
                return false;
            }
            var initInfo = XmlSerializeHelper.LoadFromFile<CameraInfoBackUpList>(filePath);
            var areas = bll.Areas.ToList();
            foreach (var devInfo in initInfo.DevList)
            {
                //  电子设备间|集控楼0m层(同名房间太多,比较一下上一层级)
                string[] value = devInfo.ParentName.Split('|');
                if (value.Length < 2) continue;
                string parentName = value[0];
                List<Area> arealist = areas.FindAll(i => (i.Name == parentName));
                if(arealist.Count>1)
                {
                    foreach (var area in arealist)
                    {
                        Area areaParent = areas.Find(i => i.Id == area.ParentId);
                        if (areaParent.Name == value[1])
                        {
                            devInfo.ParentId = area.Id;
                        }
                    }
                }
                else
                {
                    Area area = arealist[0];
                    if (area != null)
                        devInfo.ParentId = area.Id;
                }
                AddCameraInfo(devInfo, bll);
            }
            return true;
        }
        /// <summary>
        /// 添加摄像头信息
        /// </summary>
        /// <param name="cameraDev"></param>
        /// <param name="bll"></param>
        private static void AddCameraInfo(CameraInfoBackup cameraDev, Bll bll)
        {
            try
            {
                DevInfoBackup dev = CameraToDevInfo(cameraDev);
                DevInfo devInfo = GetDevInfo(dev);
                DevPos devPos = GetDevPos(dev);
                devInfo.SetPos(devPos);
                bll.DevInfos.Add(devInfo);
                Dev_CameraInfo camera = GetCameraInfo(cameraDev,devInfo);
                bll.Dev_CameraInfos.Add(camera);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DevInfoHelper.AddDevInfo:" + e.ToString());
            }
        }
        /// <summary>
        /// 获取摄像头数据
        /// </summary>
        /// <param name="camDev"></param>
        /// <param name="dev"></param>
        /// <returns></returns>
        private static Dev_CameraInfo GetCameraInfo(CameraInfoBackup camDev,DevInfo dev)
        {
            Dev_CameraInfo info = new Dev_CameraInfo();
            info.Ip = camDev.IP;
            info.UserName = camDev.UserName;
            info.PassWord = camDev.PassWord;
            info.CameraIndex = TryParseInt(camDev.CameraIndex);
            info.Port = TryParseInt(camDev.Port);
            info.DevInfoId = dev.Id;
            info.ParentId = dev.ParentId;
            info.Local_DevID = dev.Local_DevID;
            return info;
        }
        /// <summary>
        /// 摄像头转设备信息（相同部分）
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        private static DevInfoBackup CameraToDevInfo(CameraInfoBackup camera)
        {
            DevInfoBackup dev = new DevInfoBackup();
            dev.DevId = Guid.NewGuid().ToString();
            dev.TypeCode = camera.TypeCode;
            dev.Name = camera.Name;
            dev.ParentName = camera.ParentName;
            dev.ParentId = camera.ParentId;
            dev.ModelName = camera.ModelName;
            dev.KKSCode = camera.KKSCode;

            dev.XPos = camera.XPos;
            dev.YPos = camera.YPos;
            dev.ZPos = camera.ZPos;
            dev.RotationX = camera.RotationX;
            dev.RotationY = camera.RotationY;
            dev.RotationZ = camera.RotationZ;
            dev.ScaleX = camera.ScaleX;
            dev.ScaleY = camera.ScaleY;
            dev.ScaleZ = camera.ScaleZ;
            return dev;
        }
        #endregion
    }
}
