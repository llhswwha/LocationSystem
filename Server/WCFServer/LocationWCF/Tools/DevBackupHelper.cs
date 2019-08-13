using Assets.z_Test.BackUpDevInfo;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using DbModel.Tools.InitInfos;
using IModel.Enums;
using Location.BLL.Tool;
using Location.Model.InitInfos;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocationServer.Tools
{
    public class DevBackupHelper
    {
        public void BackupDevInfo(Action callBack=null)
        {            
            Thread thread = new Thread(() =>
            {
                LocationService service = new LocationService();
                var devlist = service.GetAllDevInfos().ToList();
                Log.Info(LogTags.DbInit, "Init NoramlDev...");
                DateTime recordT = DateTime.Now;
                //1.备份普通设备
                SaveNormalDev(devlist, service);
                Log.Info(LogTags.DbInit, string.Format("Init NormalDev complete,cost time: {0}s.",(DateTime.Now-recordT).TotalSeconds.ToString("f2")));
              
                Log.Info(LogTags.DbInit, "Init CameraDev... \n");
                recordT = DateTime.Now;
                //2.摄像头备份
                BackupCameraDev(service);
                Log.Info(LogTags.DbInit, string.Format("Init CameraDev complete,cost time: {0}s.", (DateTime.Now - recordT).TotalSeconds.ToString("f2")));

                Log.Info(LogTags.DbInit, "Init ArchorDev... \n");
                recordT = DateTime.Now;
                //3.备份基站信息
                BackupArchorDev(service);
                Log.Info(LogTags.DbInit, string.Format("Init ArchorDev complete,cost time: {0}s.", (DateTime.Now - recordT).TotalSeconds.ToString("f2")));

                Log.Info(LogTags.DbInit, "Init DoorAccess... \n");
                recordT = DateTime.Now;
                //4.备份门禁设备
                BackupDoorAccess(service);
                Log.Info(LogTags.DbInit, string.Format("Init DoorAccessDev complete,cost time: {0}s.", (DateTime.Now - recordT).TotalSeconds.ToString("f2")));
                if (callBack != null) callBack();
            });
            thread.IsBackground = true;
            thread.Start();          
        }


        #region 普通设备备份
        /// <summary>
        /// 保存通用设备
        /// </summary>
        private void SaveNormalDev(List<Location.TModel.Location.AreaAndDev.DevInfo> devInfoList,LocationService service)
        {
            DevInfoBackupList backUpList = new DevInfoBackupList();
            backUpList.DevList = new List<DevInfoBackup>();
            foreach (var item in devInfoList)
            {
                string typeCodeT = item.TypeCode.ToString();
                if (TypeCodeHelper.IsLocationDev(typeCodeT) || TypeCodeHelper.IsCamera(typeCodeT)||TypeCodeHelper.IsDoorAccess(item.ModelName)||TypeCodeHelper.IsFireFightDevType(typeCodeT)) continue;
                DevInfoBackup dev = new DevInfoBackup();
                dev.DevId = item.DevID;
                dev.KKSCode = item.KKSCode;
                dev.Abutment_DevID = item.Abutment_DevID;
                dev.ModelName = item.ModelName;
                dev.Name = item.Name;
                dev.ParentName = GetAreaPath((int)item.ParentId,service);
                dev.TypeCode = item.TypeCode.ToString();

                DevPos pos = item.Pos;

                dev.RotationX = pos.RotationX.ToString();
                dev.RotationY = pos.RotationY.ToString();
                dev.RotationZ = pos.RotationZ.ToString();

                dev.XPos = pos.PosX.ToString();
                dev.YPos = pos.PosY.ToString();
                dev.ZPos = pos.PosZ.ToString();

                dev.ScaleX = pos.ScaleX.ToString();
                dev.ScaleY = pos.ScaleY.ToString();
                dev.ScaleZ = pos.ScaleZ.ToString();

                backUpList.DevList.Add(dev);
            }
            string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\设备信息\\DevInfoBackup.xml";
            XmlSerializeHelper.Save(backUpList, initFile, Encoding.UTF8);
        }
        #endregion

        #region 摄像头备份
        public void BackupCameraDev(LocationService service)
        {
            var camList = service.GetAllCameraInfo();
            if (camList == null || camList.Count == 0) return;
            var devInfoList = service.GetAllDevInfos();
            if (devInfoList == null || devInfoList.Count == 0) return;
            List<Location.TModel.Location.AreaAndDev.DevInfo> devList = devInfoList.ToList();
            foreach (var item in camList)
            {
                Location.TModel.Location.AreaAndDev.DevInfo dev = devList.Find(i => i.Id == item.DevInfoId);
                if (dev != null)
                {
                    item.DevInfo = dev;
                }
                else
                {
                    Log.Info("CamerInfo is null:" + item.DevInfo);
                }
            }
            SaveCameraInfoToXml(camList.ToList(),service);
        }

        /// <summary>
        /// 保存设备信息至Xml文件
        /// </summary>
        private void SaveCameraInfoToXml(List<TModel.Location.AreaAndDev.Dev_CameraInfo> cameraList, LocationService service)
        {
            CameraInfoBackUpList backUpList = new CameraInfoBackUpList();
            backUpList.DevList = new List<CameraInfoBackup>();

            foreach (var item in cameraList)
            {
                if (item.DevInfo == null) continue;
                CameraInfoBackup dev = new CameraInfoBackup();
                dev.DevId = item.DevInfo.DevID;
                dev.KKSCode = item.DevInfo.KKSCode;
                dev.Abutment_DevID = item.DevInfo.Abutment_DevID;
                dev.ModelName = item.DevInfo.ModelName;
                dev.Name = item.DevInfo.Name;
                dev.ParentName = GetAreaPath((int)item.ParentId, service);
                dev.TypeCode = item.DevInfo.TypeCode.ToString();

                DevPos pos = item.DevInfo.Pos;

                dev.RotationX = pos.RotationX.ToString();
                dev.RotationY = pos.RotationY.ToString();
                dev.RotationZ = pos.RotationZ.ToString();

                dev.XPos = pos.PosX.ToString();
                dev.YPos = pos.PosY.ToString();
                dev.ZPos = pos.PosZ.ToString();

                dev.ScaleX = pos.ScaleX.ToString();
                dev.ScaleY = pos.ScaleY.ToString();
                dev.ScaleZ = pos.ScaleZ.ToString();

                dev.IP = item.Ip;
                dev.UserName = item.UserName;
                dev.PassWord = item.PassWord;
                dev.CameraIndex = item.CameraIndex.ToString();
                dev.Port = item.Port.ToString();
                dev.RtspURL = item.RtspUrl;

                backUpList.DevList.Add(dev);
            }
            string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\设备信息\\CameraInfoBackup.xml";
            XmlSerializeHelper.Save(backUpList, initFile, Encoding.UTF8);
        }

        #endregion
        #region 基站设备
        /// <summary>
        /// 备份基站信息
        /// </summary>
        /// <param name="service"></param>
        private void BackupArchorDev(LocationService service)
        {
            List<TModel.Location.AreaAndDev.Archor> archorList = service.GetArchors();
            if (archorList == null || archorList.Count == 0) return;
            var devInfoList = service.GetAllDevInfos();
            if (devInfoList == null || devInfoList.Count == 0) return;
            List<Location.TModel.Location.AreaAndDev.DevInfo> devList = devInfoList.ToList();
            foreach (var item in archorList)
            {
                Location.TModel.Location.AreaAndDev.DevInfo dev = devList.Find(i => i.Id == item.DevInfoId);
                if (dev != null)
                {
                    item.DevInfo = dev;
                }
                else
                {
                    Log.Info("CamerInfo is null:" + item.DevInfo);
                }
            }
            SaveArchorInfoToXml(archorList,service);
        }
        private void SaveArchorInfoToXml(List<TModel.Location.AreaAndDev.Archor> archorList, LocationService service)
        {
            LocationDeviceList backUpList = new LocationDeviceList();
            backUpList.DepList = new List<LocationDevices>();

            foreach (var item in archorList)
            {
                if (item.DevInfo == null) continue;
                Area area = service.GetAreaById(item.ParentId);
                if(area==null)
                {
                    Log.Info(string.Format("Error: Dev {0} area not find...",item.DevInfo.Id));
                    continue;
                }
                LocationDevices areaList = backUpList.DepList.Find(i => i.Name == area.Name);
                if (areaList==null)
                {
                    areaList = new LocationDevices();
                    areaList.Name = area.Name;
                    areaList.DevList = new List<LocationDevice>();
                    backUpList.DepList.Add(areaList);
                }
                if (areaList.DevList == null) areaList.DevList = new List<LocationDevice>();
                LocationDevice dev = new LocationDevice();
                dev.Name = item.Name;
                dev.Abutment_DevID = item.DevInfo.Abutment_DevID;
                dev.AnchorId = item.Code;
                dev.IP = item.Ip;
                dev.AbsolutePosX = item.X.ToString("f2");
                dev.AbsolutePosY = item.Y.ToString("f2");
                dev.AbsolutePosZ = item.Z.ToString("f2");

                DevPos pos = item.DevInfo.Pos;
                if (pos != null)
                {
                    dev.XPos = pos.PosX.ToString("f2");
                    dev.YPos = pos.PosY.ToString("f2");
                    dev.ZPos = pos.PosZ.ToString("f2");
                }
                else
                {
                    Log.Info("Error: dev.pos is null->"+item.DevInfo.Id);
                }
                areaList.DevList.Add(dev);
            }
            string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\基站信息\\基站信息.xml";
            XmlSerializeHelper.Save(backUpList, initFile, Encoding.UTF8);
        }


        #endregion
        #region 门禁设备
        /// <summary>
        /// 备份门禁设备
        /// </summary>
        /// <param name="service"></param>
        public void BackupDoorAccess(LocationService service)
        {
            var doorList = service.GetAllDoorAccessInfo();
            if (doorList == null || doorList.Count == 0)
            {
                Log.Info("DoorAccess is null...");
                return;
            }
            var devInfoList = service.GetAllDevInfos();
            if (devInfoList == null || devInfoList.Count == 0) return;
            List<Location.TModel.Location.AreaAndDev.DevInfo> devList = devInfoList.ToList();
            foreach (var item in doorList)
            {
                Location.TModel.Location.AreaAndDev.DevInfo dev = devList.Find(i => i.Id == item.DevInfoId);
                if (dev != null)
                {
                    item.DevInfo = dev;
                }
                else
                {
                    Log.Info("CamerInfo is null:" + item.DevInfo);
                }
            }
            SaveDoorAccessToXml(doorList, service);
        }
        private void SaveDoorAccessToXml(IList<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> doorList, LocationService service)
        {
            DoorAccessList backUpList = new DoorAccessList();
            backUpList.DevList = new List<DoorAccess>();
            foreach (var item in doorList)
            {
                if (item.DevInfo == null) continue;
                DoorAccess dev = new DoorAccess();
                dev.DevId = item.DevInfo.DevID;
                dev.KKSCode = item.DevInfo.KKSCode;
                dev.Abutment_DevID = item.DevInfo.Abutment_DevID;
                dev.ModelName = item.DevInfo.ModelName;
                dev.Name = item.DevInfo.Name;
                dev.ParentName = GetAreaPath((int)item.ParentId, service);
                dev.TypeCode = item.DevInfo.TypeCode.ToString();

                if(dev.Name== "集控楼4.5米集控室大门(左)")
                {
                    int idc = 0;
                }

                DevPos pos = item.DevInfo.Pos;

                dev.RotationX = pos.RotationX.ToString();
                dev.RotationY = pos.RotationY.ToString();
                dev.RotationZ = pos.RotationZ.ToString();

                dev.XPos = pos.PosX.ToString();
                dev.YPos = pos.PosY.ToString();
                dev.ZPos = pos.PosZ.ToString();

                dev.ScaleX = pos.ScaleX.ToString();
                dev.ScaleY = pos.ScaleY.ToString();
                dev.ScaleZ = pos.ScaleZ.ToString();

                dev.DoorId = item.DoorId;
                dev.Local_DevId = item.DevID;
                backUpList.DevList.Add(dev);
            }
            string initFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\设备信息\\DoorAccessBackup.xml";
            XmlSerializeHelper.Save(backUpList, initFile, Encoding.UTF8);
        }
        #endregion
        /// <summary>
        /// 获取区域路径 （子区域|父区域）
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        private string GetAreaPath(int areaId, LocationService service)
        {
            Area area = service.GetAreaById(areaId);
            string parentName = "";
            if (area != null)
            {
                Area parentArea = service.GetAreaById((int)area.ParentId);
                string grandparentName = parentArea == null ? "" : parentArea.Name;
                parentName = string.Format("{0}|{1}", area.Name, grandparentName);
            }
            return parentName;
        }
    }
}
