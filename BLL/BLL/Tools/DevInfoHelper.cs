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
using DbModel.Tools.InitInfos;
using IModel;
using ExcelLib;
using System.Data;

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
            if (initInfo==null||initInfo.DevList == null || initInfo.DevList.Count == 0) return false;
            var areas = bll.Areas.ToList();
            //var devs = bll.DevInfos.Where(i => i.Local_TypeCode != TypeCodes.Archor);
            //bll.DevInfos.RemoveList(devs);//先清空所有设备
            foreach (var devInfo in initInfo.DevList)
            {
                if (devInfo.TypeCode == TypeCodes.Archor+"")
                {
                    continue;
                }
                int? parentID = GetAreaIdByPath(devInfo.ParentName,areas);
                if(parentID!=null)
                {
                    devInfo.ParentId = (int)parentID;
                    AddDevInfo(devInfo, bll.DevInfos);
                }               
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
            if (initInfo == null || initInfo.DevList == null || initInfo.DevList.Count == 0) return false;
            var areas = bll.Areas.ToList();
            foreach (var devInfo in initInfo.DevList)
            {
                int? parentID = GetAreaIdByPath(devInfo.ParentName, areas);
                if (parentID != null)
                {
                    devInfo.ParentId = (int)parentID;
                    AddCameraInfo(devInfo, bll);
                }
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
            dev.DevId = camera.DevId;
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
        #region 门禁信息初始化

        /// <summary>
        /// 通过文件导入设备信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="devBll"></param>
        /// <returns></returns>
        public static bool ImportDoorAccessInfoFromFile(string filePath, Bll bll)
        {
            if (!File.Exists(filePath) || bll == null)
            {
                Log.Error("文件不存在:" + filePath);
                return false;
            }
            var initInfo = XmlSerializeHelper.LoadFromFile<DoorAccessList>(filePath);
            if (initInfo==null||initInfo.DevList == null || initInfo.DevList.Count == 0) return false;
            var areas = bll.Areas.ToList();
            foreach (var devInfo in initInfo.DevList)
            {
                int? parentId = GetAreaIdByPath(devInfo.ParentName,areas);
                if(parentId!=null)
                {
                    devInfo.ParentId =(int)parentId;
                    AddDoorAccessInfo(devInfo, bll);
                }                
            }
            return true;
        }
        /// <summary>
        /// 添加门禁信息
        /// </summary>
        /// <param name="cameraDev"></param>
        /// <param name="bll"></param>
        private static void AddDoorAccessInfo(DoorAccess doorAccess, Bll bll)
        {
            try
            {
                DevInfoBackup dev = DoorAccessToDevInfo(doorAccess);
                DevInfo devInfo = GetDevInfo(dev);
                DevPos devPos = GetDevPos(dev);
                devInfo.SetPos(devPos);
                bll.DevInfos.Add(devInfo);
                DbModel.Location.AreaAndDev.Dev_DoorAccess doorAccessDev = GetDoorAccessInfo(doorAccess, devInfo);
                bll.Dev_DoorAccess.Add(doorAccessDev);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DevInfoHelper.AddDevInfo:" + e.ToString());
            }
        }
        /// <summary>
        /// 获取门禁信息
        /// </summary>
        /// <param name="camDev"></param>
        /// <param name="dev"></param>
        /// <returns></returns>
        private static DbModel.Location.AreaAndDev.Dev_DoorAccess GetDoorAccessInfo(DoorAccess doorAccessDev, DevInfo dev)
        {
            DbModel.Location.AreaAndDev.Dev_DoorAccess info = new DbModel.Location.AreaAndDev.Dev_DoorAccess();
            info.ParentId = doorAccessDev.ParentId;
            info.DoorId = doorAccessDev.DoorId;
            info.DevInfoId = dev.Id;
            info.Local_DevID = dev.Local_DevID;
            return info;
        }
        /// <summary>
        /// 门禁转设备信息（相同部分）
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        private static DevInfoBackup DoorAccessToDevInfo(DoorAccess doorAccess)
        {
            DevInfoBackup dev = new DevInfoBackup();
            dev.DevId = doorAccess.DevId;
            dev.TypeCode = doorAccess.TypeCode;
            dev.Name = doorAccess.Name;
            dev.ParentName = doorAccess.ParentName;
            dev.ParentId = doorAccess.ParentId;
            dev.ModelName = doorAccess.ModelName;
            dev.KKSCode = doorAccess.KKSCode;

            dev.XPos = doorAccess.XPos;
            dev.YPos = doorAccess.YPos;
            dev.ZPos = doorAccess.ZPos;
            dev.RotationX = doorAccess.RotationX;
            dev.RotationY = doorAccess.RotationY;
            dev.RotationZ = doorAccess.RotationZ;
            dev.ScaleX = doorAccess.ScaleX;
            dev.ScaleY = doorAccess.ScaleY;
            dev.ScaleZ = doorAccess.ScaleZ;
            return dev;
        }

        #endregion
        /// <summary>
        /// 通过区域路径，获取区域ID
        /// </summary>
        /// <param name="areaPath"></param>
        /// <param name="areas"></param>
        /// <returns></returns>
        private static int? GetAreaIdByPath(string areaPath,List<Area> areas)
        {
            if (areas == null || areas.Count == 0) return null;
            string[] value = areaPath.Split('|');
            if (value.Length < 2) return null;
            string parentName = value[0];
            List<Area> arealist = areas.FindAll(i => (i.Name == parentName));
            if (arealist!=null&&arealist.Count > 1)
            {
                foreach (var area in arealist)
                {
                    Area areaParent = areas.Find(i => i.Id == area.ParentId);
                    if (areaParent.Name == value[1])
                    {
                        return areaParent.Id;
                    }
                }
            }
            else
            {
                if (arealist == null || arealist.Count == 0) return null;
                else
                {
                    return arealist[0].Id;
                }
            }
            return null;
        }
		
        #region 设备监控点
        public static void ImportDevMonitorNodeFromFile<T>(FileInfo file)
            where T : DevMonitorNode, new()
        {
            Log.InfoStart("DevInfoHelper.ImportDevMonitorNodeFromFile");
            if (file.Exists == false)
            {
                Log.Info("不存在文件:" + file.FullName);
                return;
            }
            Bll bll = new Bll();
            List<DevMonitorNode> DevMonitorNodeList = bll.DevMonitorNodes.ToList();
            if (DevMonitorNodeList != null && DevMonitorNodeList.Count == 0)
            {
                List<DevMonitorNode> list = CreateDevMonitorNodeListFromFile<DevMonitorNode>(file);
                bll.DevMonitorNodes.AddRange(bll.Db, list); //新增的部分
            }
            Log.InfoEnd("DevInfoHelper.ImportDevMonitorNodeFromFile");
        }

        public static List<T> CreateDevMonitorNodeListFromFile<T>(FileInfo fileInfo) where T : IDevMonitorNode, new()
        {
            string strFolderName = fileInfo.Directory.Name;
            DataTable dtTable = ExcelHelper.Load(new FileInfo(fileInfo.FullName), false).Tables[0].Copy();
            dtTable.Rows.RemoveAt(0);
            dtTable.Rows.RemoveAt(0);
            List<T> list1 = CreateDevMonitorNodeListFromDataTable<T>(dtTable);
            return list1;
        }

        public static List<T> CreateDevMonitorNodeListFromDataTable<T>(DataTable dt) where T : IDevMonitorNode, new()
        {
            List<T> list = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                T devmonitor1 = CreateDevMonitorNodeFromDataRow<T>( dr);//根据表格内容创建KKS对象
                
                list.Add(devmonitor1);
            }
            return list;
        }

        public static T CreateDevMonitorNodeFromDataRow<T>(DataRow dr) where T : IDevMonitorNode, new()
        {
            T devmonitor1 = new T();
            devmonitor1.TagName = dr[0].ToString();
            devmonitor1.DataBaseName = dr[1].ToString();
            devmonitor1.DataBaseTagName = dr[2].ToString();
            devmonitor1.Describe = dr[3].ToString();
            devmonitor1.Unit = dr[4].ToString();
            devmonitor1.DataType = dr[5].ToString();
            devmonitor1.TagType = dr[6].ToString();
            devmonitor1.KKS = dr[7].ToString();

            return devmonitor1;
        }

        #endregion
    }
}
