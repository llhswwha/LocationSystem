﻿using System;
using System.Collections.Generic;
using System.Linq;
using DbModel.Tools;
using Location.Model.DataObjects.ObjectAddList;
using Location.TModel.FuncArgs;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.Person;
using LocationServices.Converters;
using Dev_DoorAccess = Location.TModel.Location.AreaAndDev.Dev_DoorAccess;
using TModel.Location.AreaAndDev;
using TModel.Tools;
using LocationServices.Locations.Services;
using DAL;
using BLL.Tools;
using IModel.Enums;
using TModel.LocationHistory.AreaAndDev;
using System.Data.Entity.Infrastructure;
using Location.BLL.Tool;
using TModel.BaseData;
using Location.TModel.Tools;
using System.IO;
//using DbModel.Location.Alarm;

namespace LocationServices.Locations
{
    //设备信息、位置信息相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        /// <summary>
        /// 获取所有设备位置信息
        /// </summary>
        /// <returns></returns>
        public IList<DevPos> GetDevPositions()
        {

            List<DevPos> posList = new List<DevPos>();
            var devs = db.DevInfos.ToList();
            foreach (DbModel.Location.AreaAndDev.DevInfo dev in devs)
            {
                posList.Add(dev.GetPos());
            }
            return posList.ToWCFList();
        }
        /// <summary>
        /// 添加一条设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        public bool AddDevPosInfo(DevPos pos)
        {
            return ModifyPosInfo(pos);
        }
        /// <summary>
        /// 修改设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool ModifyPosInfo(DevPos pos)
        {
            var devInfo = db.DevInfos.DbSet.FirstOrDefault(i => i.Local_DevID == pos.DevID);
            if (devInfo != null)
            {
                devInfo.SetPos(pos);
                return db.DevInfos.Edit(devInfo);
            }
            return false;
        }
        /// <summary>
        /// 修改设备位置信息
        /// </summary>
        /// <param name="posList"></param>
        /// <returns></returns>
        public bool ModifyPosByList(List<DevPos> posList)
        {
            //return db.DevPos.EditRange(db.Db, posList);
            foreach (var devPose in posList)
            {
                ModifyPosInfo(devPose);
            }
            return true;
        }
        /// <summary>
        /// 删除设备位置信息
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool DeletePosInfo(DevPos pos)
        {
            //return db.DevPos.DeleteById(pos.DevID);
            return false;
        }
        /// <summary>
        /// 添加设备位置信息（列表形式）
        /// </summary>
        /// <param name="posList"></param>
        public bool AddDevPosByList(List<DevPos> posList)
        {
            //return db.DevPos.AddRange(posList.ToList());
            return ModifyPosByList(posList);
        }


        private void BindingDev(List<DevInfo> devInfoList)
        {
            //DeviceInfoService service = new DeviceInfoService();
            ////service.BindingDevPos(devInfoList, db.DevPos.ToList());
            DeviceInfoService.BindingDevParent(devInfoList, db.Areas.ToList().ToTModel());
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> GetAllDevInfos()
        {
            var service = new DeviceService(db);
            var devList= service.GetList();
            return devList;
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> GetDevInfos(int[] typeList)
        {
            var service = new DeviceService(db);
            var devList = service.GetListByTypes(typeList);
            return devList;
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> FindDevInfos(string key)
        {
            return new DeviceService(db).GetListByName(key);
        }

        /// <summary>
        /// 添加一条设备基本信息
        /// </summary>
        /// <param name="devInfo"></param>
        public DevInfo AddDevInfo(DevInfo devInfo)
        {
            return new DeviceService(db).Post(devInfo);
        }
        /// <summary>
        /// 添加设备基本信息（列表形式）
        /// </summary>
        /// <param name="devInfoList"></param>
        public List<DevInfo> AddDevInfoByList(List<DevInfo> devInfoList)
        {
            return new DeviceService(db).PostRange(devInfoList);
        }
        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        public bool ModifyDevInfo(DevInfo devInfo)
        {
            return new DeviceService(db).Put(devInfo)!=null;
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="devInfo"></param>
        /// <returns></returns>
        public bool DeleteDevInfo(DevInfo devInfo)
        {
            return new DeviceService(db).Delete(devInfo.Id+"") != null;
        }
        /// <summary>
        /// 通过区域ID,获取区域下所有设备
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IList<DevInfo> GetDevInfoByParent(int[] pidList)
        {
            return new DeviceService(db).GetListByPids(pidList);
        }

        /// <summary>
        /// 通过设备Id,获取设备(字符串Id,GUID那部分)
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        public DevInfo GetDevByGUID(string devId)
        {
            return new DeviceService(db).GetEntityByDevId(devId);
        }

        /// <summary>
        /// 通过设备Id,获取设备(数字Id,主键)
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        public DevInfo GetDevById(int id)
        {
            return new DeviceService(db).GetEntityById(id);
        }

        public DevInfo GetDevByGameName(string nameName)
        {
            return new DeviceService(db).GetDevByGameName(nameName);
        }

        

        /// <summary>
        /// 添加门禁设备
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        public bool AddDoorAccessByList(IList<Dev_DoorAccess> doorAccessList)
        {
            return db.Dev_DoorAccess.AddRange(doorAccessList.ToList().ToDbModel());
        }
        /// <summary>
        /// 添加门禁信息
        /// </summary>
        /// <param name="doorAccess"></param>
        /// <returns></returns>
        public Dev_DoorAccess AddDoorAccess(Dev_DoorAccess doorAccess)
        {
            DbModel.Location.AreaAndDev.Dev_DoorAccess access = doorAccess.ToDbModel();
            bool value = db.Dev_DoorAccess.Add(access);
            return value == true ? access.ToTModel() : null;
            //return db.Dev_DoorAccess.Add(doorAccess.ToDbModel());
        }
        /// <summary>
        /// 删除门禁信息
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        public bool DeleteDoorAccess(IList<Dev_DoorAccess> doorAccessList)
        {
            bool value = true;
            foreach (Dev_DoorAccess item in doorAccessList)
            {
                var doorAccess = db.Dev_DoorAccess.DeleteById(item.Id);
                var dev = db.DevInfos.DeleteById(item.DevInfoId);
                //bool posResult = db.DevPos.DeleteById(item.DevID);
                bool valueTemp = doorAccess!=null && dev!=null;
                if (!valueTemp) value = valueTemp;
            }
            return value;
        }
        /// <summary>
        /// 修改门禁信息
        /// </summary>
        /// <param name="doorAccessList"></param>
        /// <returns></returns>
        public bool ModifyDoorAccess(IList<Dev_DoorAccess> doorAccessList)
        {
            return db.Dev_DoorAccess.EditRange(db.Db, doorAccessList.ToList().ToDbModel());
        }
        /// <summary>
        /// 通过区域ID,获取区域下所有门禁信息
        /// </summary>
        /// <param name="pid">区域ID</param>
        /// <returns></returns>
        public IList<Dev_DoorAccess> GetDoorAccessInfoByParent(int[] pidList)
        {
            try
            {
                List<Dev_DoorAccess> devInfoList = new List<Dev_DoorAccess>();
                foreach (var pId in pidList)
                {
                    devInfoList.AddRange(db.Dev_DoorAccess.DbSet.Where(item => item.ParentId != null && item.ParentId == pId).ToList().ToTModel());
                }
                return devInfoList.ToWCFList();
            }catch(Exception e)
            {
                Log.Error("LocationService_Dev.GetDoorAccessInfoByParent.Exception:"+e.ToString());
                return null;
            }           
        }
        /// <summary>
        /// 获取所有的门禁信息
        /// </summary>
        /// <returns></returns>
        public IList<Dev_DoorAccess> GetAllDoorAccessInfo()
        {
            List<Dev_DoorAccess> doorAccessList= db.Dev_DoorAccess.DbSet.ToList().ToTModel();
            return doorAccessList.ToWCFList();
        }
        #region 摄像头信息
        /// <summary>
        /// 添加摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        public bool AddCameraInfoByList(IList<Dev_CameraInfo> cameraInfoList)
        {
            return db.Dev_CameraInfos.AddRange(cameraInfoList.ToList().ToDbModel());
        }
        /// <summary>
        /// 添加摄像头信息
        /// </summary>
        /// <param name="cameraInfo"></param>
        /// <returns></returns>
        public Dev_CameraInfo AddCameraInfo(Dev_CameraInfo cameraInfo)
        {
            DbModel.Location.AreaAndDev.Dev_CameraInfo dbCamera = cameraInfo.ToDbModel();
            var result = db.Dev_CameraInfos.Add(dbCamera);
            return result ? dbCamera.ToTModel() : null;            
        }
        /// <summary>
        /// 删除摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        public bool DeleteCameraInfo(IList<Dev_CameraInfo> cameraInfoList)
        {
            bool value = true;
            foreach (Dev_CameraInfo item in cameraInfoList)
            {
                var cameraInfo = db.Dev_CameraInfos.DeleteById(item.Id);
                var dev = db.DevInfos.DeleteById(item.DevInfoId);
                //bool posResult = db.DevPos.DeleteById(item.DevID);
                bool valueTemp = cameraInfo != null && dev != null;
                if (!valueTemp) value = valueTemp;
            }
            return value;
        }
        /// <summary>
        /// 修改摄像头信息
        /// </summary>
        /// <param name="cameraInfoList"></param>
        /// <returns></returns>
        public bool ModifyCameraInfoByList(IList<Dev_CameraInfo> cameraInfoList)
        {
            //好像没用
            return db.Dev_CameraInfos.EditRange(db.Db, cameraInfoList.ToList().ToDbModel());
        }
        public Dev_CameraInfo ModifyCameraInfo(Dev_CameraInfo camInfo)
        {
            camInfo.AutoGenerateRtsp();
            var dbModel = camInfo.ToDbModel();
            if (db.Dev_CameraInfos.Edit(dbModel))
            {
                return camInfo;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 通过区域ID，获取所有摄像头信息
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        public IList<Dev_CameraInfo> GetCameraInfoByParent(int[] pidList)
        {
            List<Dev_CameraInfo> devInfoList = new List<Dev_CameraInfo>();
            foreach (var pId in pidList)
            {
                devInfoList.AddRange(db.Dev_CameraInfos.DbSet.Where(item => item.ParentId == pId).ToList().ToTModel());
            }
            return devInfoList.ToWCFList();
        }
        /// <summary>
        /// 获取所有的摄像头信息
        /// </summary>
        /// <returns></returns>
        public IList<Dev_CameraInfo> GetAllCameraInfo()
        {
            List<Dev_CameraInfo> cameraInfoList = db.Dev_CameraInfos.DbSet.ToList().ToTModel();
            return cameraInfoList.ToWCFList();
        }
        /// <summary>
        /// 通过设备信息，获取摄像头信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public Dev_CameraInfo GetCameraInfoByDevInfo(DevInfo dev)
        {
            Dev_CameraInfo cameraInfo = db.Dev_CameraInfos.DbSet.FirstOrDefault(item=>item.DevInfoId==dev.Id).ToTModel();
            return cameraInfo;
        }

        public Dev_CameraInfo GetCameraInfoByIp(string ip)
        {
            Dev_CameraInfo cameraInfo = db.Dev_CameraInfos.DbSet.FirstOrDefault(item => !(string.IsNullOrEmpty(item.Ip))&&item.Ip == ip).ToTModel();
            return cameraInfo;
        }
        #endregion
        public List<LocationAlarm> GetLocationAlarms(AlarmSearchArg arg)
        {
            //List<Personnel> ps = GetPersonList();
            //List<LocationAlarm> alarms = new List<LocationAlarm>();
            //alarms.Add(new LocationAlarm() { Id = 0, TagId = 1, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)4, Content = "进入无权限区域", CreateTime = new DateTime(2018, 9, 1, 10, 5, 34) }.SetPerson(ps[0]));
            //alarms.Add(new LocationAlarm() { Id = 1, TagId = 2, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)3, Content = "靠近高风险区域", CreateTime = new DateTime(2018, 9, 4, 15, 5, 11) }.SetPerson(ps[1]));
            //alarms.Add(new LocationAlarm() { Id = 2, TagId = 3, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)2, Content = "进入高风险区域", CreateTime = new DateTime(2018, 9, 7, 13, 35, 20) }.SetPerson(ps[2]));
            //alarms.Add(new LocationAlarm() { Id = 3, TagId = 4, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)1, Content = "进入危险区域", CreateTime = new DateTime(2018, 9, 10, 16, 15, 44) }.SetPerson(ps[3]));
            //return alarms;
            var list = db.LocationAlarms.Where(p=>p.AlarmLevel != 0).ToList();
            var persons = db.Personnels.ToList();
            foreach (var alarm in list)
            {
                alarm.Personnel = persons.Find(i => i.Id == alarm.PersonnelId);
            }
            return list.ToWcfModelList();
        }

        /// <summary>
        /// 生成定位 告警/消警 信息
        /// </summary>
        /// <param name="count"></param>
        /// <param name="isAlarm"></param>
        /// <returns></returns>
        public List<LocationAlarm> GetLocationAlarms(int count,bool isAlarm=true)
        {
            List<Personnel> ps = GetPersonList(false);
            List<LocationAlarm> alarms = new List<LocationAlarm>();
            for (int i = 0; i < count; i++)
            {
                Personnel person = null;
                if (i < ps.Count) person = ps[i];
                if (isAlarm)
                {
                    alarms.Add(new LocationAlarm()
                    {
                        Id = i,
                        TagId = person == null ? 1 : (int)person.TagId,   //TagId = 1,
                        AlarmType = 0,
                        AlarmLevel = (LocationAlarmLevel)4,
                        Content = "定位告警" + i,
                        CreateTime = new DateTime(2018, 9, 1, 10, 5, 34),
                    }.SetPerson(ps[0]));
                }
                else
                {
                    alarms.Add(new LocationAlarm()
                    {
                        Id = i,
                        TagId = person == null ? 1 : (int)person.TagId,   //TagId = 1,
                        AlarmType = 0,
                        AlarmLevel = (LocationAlarmLevel)0,
                        Content = "定位消警" + i,
                        CreateTime = new DateTime(2018, 9, 1, 11, 5, 34),
                    }.SetPerson(ps[0]));
                }         
            }
            return alarms;
        }

        public List<DeviceAlarm> GetSimulateDeviceAlarms(int count)
        {
            var devs = db.DevInfos.ToList();
            List<DeviceAlarm> alarms = new List<DeviceAlarm>();
            for (int i = 0; i < count; i++)
            {
                alarms.Add(new DeviceAlarm() { Id = i, Level = Abutment_DevAlarmLevel.低, Title = "告警"+ i, Message = "设备告警1", CreateTime = new DateTime(2018, 8, 28, 9, 5, 34) }.SetDev(devs[0].ToTModel()));
            }
            return alarms;
        }

        public List<DeviceAlarm> GetDeviceAlarms(AlarmSearchArg arg)
        {

            DateTime start = DateTime.Now;
            var devs = db.DevInfos.ToList().ToTModel();
            if (devs == null||devs.Count==0) return null;
            var dict = new Dictionary<int,DevInfo>();
            foreach (var item in devs)
            {
                if(item.ParentId!=null)
                dict.Add(item.Id, item);
            }
            List<DeviceAlarm> alarms = new List<DeviceAlarm>();
            List<DbModel.Location.Alarm.DevAlarm> alarms1 = null;

            DateTime now = DateTime.Now;
            DateTime todayStart = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
            DateTime todayEnd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);

            var startStamp=TimeConvert.ToStamp(todayStart);
            var endStamp = TimeConvert.ToStamp(todayEnd);

            if (arg.AreaId != 0)
            {
                var areas = db.Areas.GetAllSubAreas(arg.AreaId);//获取所有子区域
                if (arg.IsAll)
                {
                    alarms1 = db.DevAlarms.Where(i => i.DevInfo != null && areas.Contains(i.DevInfo.ParentId));
                }
                else
                {
                    alarms1 = db.DevAlarms.Where(i => i.DevInfo != null && areas.Contains(i.DevInfo.ParentId) && i.AlarmTimeStamp >= startStamp && i.AlarmTimeStamp <= endStamp);
                }
                
            }
            else
            {
                if (arg.IsAll)
                {
                    alarms1 = db.DevAlarms.ToList();
                }
                else
                {
                    alarms1 = db.DevAlarms.Where(i => i.AlarmTimeStamp >= startStamp && i.AlarmTimeStamp <= endStamp);
                }
                
            }
            if (alarms1 == null) return null;
            alarms = alarms1.ToTModel();
            alarms.Sort();
            if (alarms.Count == 0) return null;
            foreach (var item in alarms)
            {
                try
                {
                    if(dict.ContainsKey(item.DevId))
                    {
                        var dev = dict[item.DevId];
                        item.SetDev(dev);//这里设置DevId,AreaId等
                    }  
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
            }
            TimeSpan time = DateTime.Now - start;
            return alarms;
        }

        public Page<DeviceAlarm> GetDeviceAlarmsPage(AlarmSearchArg arg)
        {
            Page<DeviceAlarm> page = new Page<DeviceAlarm>();
            page.Content = GetDeviceAlarms(arg);
            return page;
        }


        public List<Archor> GetArchors()
        {
            return db.Archors.ToList().ToWcfModelList();
        }

        public Archor GetArchor(string id)
        {
            int id2 = id.ToInt();
            return db.Archors.Find(id2).ToTModel();
        }

        public Archor GetArchorByDevId(int devId)
        {
            return db.Archors.FirstOrDefault(i=>i.DevInfoId==devId).ToTModel();
        }

        public bool AddArchor(Archor archor)
        {
            if (archor.DevInfo != null)
            {
                archor.ParentId = archor.ParentId;
            }
            return db.Archors.Add(archor.ToDbModel());
        }
        public bool DeleteArchor(int devId)
        {
            //db.Archors.DeleteById(archorId);
            bool value = true;           
            var archor = db.Archors.DeleteByDev(devId);
            var dev = db.DevInfos.DeleteById(devId);
            bool valueTemp = archor != null && dev != null;
            if (!valueTemp) value = valueTemp;
            return value;
        }
        public bool EditArchor(Archor Archor, int ParentId)
        {
            bool bReturn = false;
            DbModel.Location.AreaAndDev.Archor Archor2;
            Archor2 = db.Archors.FirstOrDefault(p => p.Code == Archor.Code);
            if (Archor2 == null)
            {
                Archor2 = db.Archors.FirstOrDefault(p => p.DevInfoId == Archor.DevInfoId);
            }         
            if (Archor2 == null)
            {
                LocationService service = new LocationService();
                DbModel.Location.AreaAndDev.Area area = service.GetAreaById(ParentId);
                Archor2 = Archor.ToDbModel();

                DbModel.Location.AreaAndDev.DevInfo dev = new DbModel.Location.AreaAndDev.DevInfo();
                dev.Local_DevID = Guid.NewGuid().ToString();
                dev.IP = "";
                dev.KKS = "";
                dev.Name = Archor2.Name;
                if(area!=null)
                {
                    dev.ModelName = area.Name == DepNames.FactoryName ? TypeNames.ArchorOutdoor : TypeNames.Archor;//室外基站||室内基站
                }
                else
                {
                    dev.ModelName = TypeNames.Archor;
                }                
                dev.Status = 0;
                dev.ParentId = ParentId;
                dev.Local_TypeCode = TypeCodes.Archor;
                dev.UserName = "admin";
                Archor2.DevInfo = dev;
                Archor2.ParentId = ParentId;

                bReturn = db.Archors.Add(Archor2);
            }
            else
            {
                Archor2.Name = Archor.Name;
                Archor2.X = Archor.X;
                Archor2.Y = Archor.Y;
                Archor2.Z = Archor.Z;
                Archor2.Type = Archor.Type;
                Archor2.IsAutoIp = Archor.IsAutoIp;
                Archor2.Ip = Archor.Ip;
                Archor2.ServerIp = Archor.ServerIp;
                Archor2.ServerPort = Archor.ServerPort;
                Archor2.Power = Archor.Power;
                Archor2.AliveTime = Archor.AliveTime;
                Archor2.Enable = Archor.Enable;
                if (!string.IsNullOrEmpty(Archor.Code)) Archor2.Code = Archor.Code;
                bReturn = db.Archors.Edit(Archor2);
            }
            EditBusAnchor(Archor, ParentId);
            return bReturn;
        }

        public bool EditBusAnchor(Archor archor, int ParentId)
        {
            bool bDeal = false;

            try
            {
                int nFlag = 0;
                var bac = db.bus_anchors.FirstOrDefault(p => p.anchor_id == archor.Code);
                if (bac == null)
                {
                    bac = new DbModel.Engine.bus_anchor();
                    nFlag = 1;
                }

                bac.anchor_id = archor.Code;
                bac.anchor_x = (int)(archor.X * 100);
                bac.anchor_y = (int)(archor.Z * 100);
                bac.anchor_z = (int)(archor.Y * 100);
                bac.anchor_type = (int)archor.Type;
                bac.anchor_bno = 0;
                bac.syn_anchor_id = null;
                bac.offset = 0;
                bac.enabled = 1;

                if (nFlag == 0)
                {
                    bDeal = db.bus_anchors.Edit(bac);
                }
                else
                {
                    bDeal = db.bus_anchors.Add(bac);
                }

                //if (!bDeal)
                //{
                //    return bDeal;
                //}

                //bDeal = EditArchor(Archor, ParentId);
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }
            
            return bDeal;
        }

        public bool EditPictureInfo(Picture pc)
        {
            return db.Pictures.Update(pc.Name, pc.Info);
        }

        public Picture GetPictureInfo(string strPictureName)
        {
            DbModel.Location.AreaAndDev.Picture pc2 = db.Pictures.DbSet.FirstOrDefault(p => p.Name == strPictureName);
            if (pc2 == null)
            {
                pc2 = new DbModel.Location.AreaAndDev.Picture();
            }

            return pc2.ToTModel();
        }

        //通过人员找附近设备 nFlag 0 表示获取全部，1表示获取摄像头
        public List<NearbyDev> GetNearbyDev_Currency(int id, float fDis, int nFlag)
        {
            DeviceService ds = new DeviceService();
            List<NearbyDev> lst = ds.GetNearbyDev_Currency(id, fDis, nFlag);
            //if (lst == null)
            //{
            //    lst = new List<NearbyDev>();
            //}

            if (lst!=null&&lst.Count == 0)
            {
                lst = null;
            }

            return lst;
        }

        //通过设备找附近设备
        public List<NearbyDev> GetNearbyCamera_Alarm(int id, float fDis)
        {
            DeviceService ds = new DeviceService();
            List<NearbyDev> lst = ds.GetNearbyCamera_Alarm(id, fDis);
            if (lst == null)
            {
                lst = new List<NearbyDev>();
            }

            return lst;
        }

        //获取人员24小时内经过的门禁
        public List<EntranceGuardActionInfo> GetEntranceActionInfoByPerson24Hours(int id)
        {
            DeviceService ds = new DeviceService();
            List<EntranceGuardActionInfo> lst = ds.GetEntranceActionInfoByPerson24Hours(id);
            if (lst == null)
            {
                lst = new List<EntranceGuardActionInfo>();
            }

            if (lst.Count == 0)
            {
                lst = null;
            }
            
            return lst;
        }
        /// <summary>
        /// 根据模型名称，获取模型类型
        /// </summary>
        /// <param name="devModelName"></param>
        /// <returns></returns>
        public DbModel.Location.AreaAndDev.DevModel GetDevClassByDevModel(string devModelName)
        {            
            DbModel.Location.AreaAndDev.DevModel devModel = db.DevModels.FirstOrDefault(dev=>dev.Name==devModelName);
            return devModel;
        }

        static  private List<DbModel.Location.AreaAndDev.KKSCode> _kksCodes;

        public List<DbModel.Location.AreaAndDev.KKSCode> KKSCodes
        {
            get
            {
                if (_kksCodes == null)
                {
                    _kksCodes = db.KKSCodes.ToListEx();
                }
                return _kksCodes;
            }
        }


        static List<DbModel.Location.AreaAndDev.DevMonitorNode> _devMonitorNodes;

        List<DbModel.Location.AreaAndDev.DevMonitorNode> DevMonitorNodes
        {
            get
            {
                if (_devMonitorNodes == null)
                {
                    _devMonitorNodes = db.DevMonitorNodes.ToList();
                }
                return _devMonitorNodes;
            }
        }

        /// <summary>
        /// 根据KKS获取设备的信息、监控项，及设备树
        /// </summary>
        /// <param name="KKS"></param>
        /// <param name="isShowAll"></param>
        /// <returns></returns>
        public Dev_Monitor GetDevMonitorInfoByKKS(string KKS, bool isShowAll)
        {
            DateTime start = DateTime.Now;
            Dev_Monitor monitor = GetDevMonitor(KKS, isShowAll);
            if (monitor == null) return null;
            var tags=monitor.GetAllTagList();
            TimeSpan time = DateTime.Now - start;
            //Log.Info(LogTags.KKS, string.Format("[{2}]KKS:{0},测点:{1}", KKS, tags, time));
            List<DevMonitorNode> dataList = GetSomesisList(tags);//到基础平台获取数据
            //Log.Info(LogTags.KKS, string.Format("获取sis数据"));
            monitor = InsertDataToEveryDev(monitor,dataList);
            
            return monitor;
        }

        public Dev_Monitor GetDevMonitor(string KKS, bool isShowAll)
        {
            Dev_Monitor monitor = new Dev_Monitor();
            DbModel.Location.AreaAndDev.KKSCode kksCode = KKSCodes.FirstOrDefault(p => p.Code == KKS);
            if (kksCode == null)
            {
                string dirPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\DeviceData\\"+ KKS+".xml";
                if (File.Exists(dirPath))
                {
                    var list=XmlSerializeHelper.LoadFromFile<DevMonitorNodeList>(dirPath);
                    monitor.Name = KKS;
                    monitor.KKSCode = KKS;
                    monitor.MonitorNodeList = list;
                }
                else
                {
                    monitor = null;
                }
            }
            else
            {
                monitor.Name = kksCode.Name;
                monitor.KKSCode = kksCode.Code;
                var monitorNodeList = GetDevMonitorNodeListByKKS(kksCode.Code);
                monitor.MonitorNodeList = monitorNodeList;
                monitor.ChildrenList = GetChildMonitorDev(kksCode);
                if (isShowAll == false)
                {
                    monitor.RemoveEmpty();
                }
                monitor.AddChildrenMonitorNodes();
            }

            if (monitor != null)
            {
                monitor.SetEmpty();
            }
           
            return monitor;
        }

        private string GetMonitorTags(List<DevMonitorNode> nodes)
        {
            string tags = "";
            foreach (var item in nodes)
            {
                string strTagName = item.TagName;
                if (tags == "")
                {
                    tags = strTagName;
                }
                else
                {
                    tags += "," + strTagName;
                }
            }
            return tags;
        }

        private string GetMonitorTags(List<DbModel.Location.AreaAndDev.DevMonitorNode> nodes)
        {
            string tags = "";
            foreach (var item in nodes)
            {
                string strTagName = item.TagName;
                if (tags == "")
                {
                    tags = strTagName;
                }
                else
                {
                    tags += "," + strTagName;
                }
            }
            return tags;
        }


        private List<DevMonitorNode> GetDevMonitorNodeListByKKS(string KKS)
        {
            List<DbModel.Location.AreaAndDev.DevMonitorNode> lst = DevMonitorNodes.FindAll(p => p.ParentKKS == KKS);
            if (lst == null || lst.Count == 0)
            {
                return null;
            }
            else
            {
                return lst.ToTModel();
            }
        }

        private List<DevMonitorNode> GetDevMonitorNodeListByKKS(List<string> KKS)
        {
            List<DbModel.Location.AreaAndDev.DevMonitorNode> lst = DevMonitorNodes.FindAll(p => KKS.Contains(p.ParentKKS));
            if (lst == null || lst.Count == 0)
            {
                return null;
            }
            else
            {
                return lst.ToTModel();
            }
        }

        private List<Dev_Monitor> GetChildMonitorDev(DbModel.Location.AreaAndDev.KKSCode KKS)
        {
            Dictionary<string,Dev_Monitor> devMoniters = new Dictionary<string, Dev_Monitor>();
            List<DbModel.Location.AreaAndDev.KKSCode> childrenKKS = KKS.Children;
            if (childrenKKS != null)
            {
                List<string> kksList = new List<string>();
                foreach (DbModel.Location.AreaAndDev.KKSCode item in childrenKKS)
                {
                    kksList.Add(item.Code);//获取所有的子kks
                    //Dev_Monitor dev = new Dev_Monitor();
                    //dev.Name = item.Name;
                    //dev.KKSCode = item.Code;
                    //dev.MonitorNodeList = GetDevMonitorNodeListByKKS(dev.KKSCode);
                    //dev.ChildrenList = GetChildMonitorDev(item, bFlag);
                    //child.Add(dev);
                }

                var monitorList = GetDevMonitorNodeListByKKS(kksList);//从数据库获取

                if (monitorList != null)
                {
                    foreach (DbModel.Location.AreaAndDev.KKSCode item in childrenKKS)
                    {
                        kksList.Add(item.Code);
                        Dev_Monitor dev = new Dev_Monitor();
                        dev.Name = item.Name;
                        dev.KKSCode = item.Code;
                        dev.MonitorNodeList = monitorList.Where(i => i.ParentKKS == item.Code).ToList();//放到相应的对象中
                        //dev.ChildrenList = GetChildMonitorDev(item, bFlag);
                        devMoniters.Add(item.Code, dev);
                    }

                    foreach (DbModel.Location.AreaAndDev.KKSCode item in childrenKKS)
                    {
                        Dev_Monitor dev = devMoniters[item.Code];
                        dev.ChildrenList = GetChildMonitorDev(item);//递归找子物体
                    }
                }
            }

            if (devMoniters.Count == 0)
            {
                return null;
            }
            return devMoniters.Values.ToList();
        }

        private Dev_Monitor InsertDataToEveryDev(Dev_Monitor Dm, List<DevMonitorNode> dataList)
        {
            Dev_Monitor send = new Dev_Monitor();
            string strDevKKs = Dm.KKSCode;
            List<DevMonitorNode> MonitorNodeList = dataList.FindAll(p => p.ParentKKS == strDevKKs);
            if (Dm.MonitorNodeList != null)
            {
                foreach (DevMonitorNode item in Dm.MonitorNodeList)
                {
                    //string strNodeKKS = item.KKS;
                    DevMonitorNode data = MonitorNodeList.Find(p => p.KKS == item.KKS);
                    //DevMonitorNode data = dataList.Find(p => p.TagName == item.TagName);
                    if (data == null || MonitorNodeList.Count == 0)
                    {
                        data = dataList.Find(p => p.TagName == item.TagName);
                    }
                    else
                    {

                    }

                    if (data != null)
                    {
                        item.Value = data.Value;
                        item.Time = data.Time;
                    }
                }
            }

            if (Dm.ChildrenList != null && Dm.ChildrenList.Count > 0)
            {
                foreach (Dev_Monitor item2 in Dm.ChildrenList)
                {
                    Dev_Monitor ChildDm = InsertDataToEveryDev(item2, dataList);
                    if (ChildDm != null)
                    {
                        if (send.ChildrenList == null)
                        {
                            send.ChildrenList = new List<Dev_Monitor>();
                        }

                        send.ChildrenList.Add(ChildDm);
                    }
                }
            }

            send.KKSCode = Dm.KKSCode;
            send.Name = Dm.Name;
            send.MonitorNodeList = Dm.MonitorNodeList;

            return send;
        }


    }
}
