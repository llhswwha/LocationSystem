using System;
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
            return new DeviceService(db).GetList();
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> GetDevInfos(int[] typeList)
        {
            return new DeviceService(db).GetListByTypes(typeList);
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
        /// 通过设备Id,获取设备
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        public DevInfo GetDevByID(string devId)
        {
            return new DeviceService(db).GetEntityByDevId(devId);
        }

        /// <summary>
        /// 通过设备Id,获取设备
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        public DevInfo GetDevByiId(int id)
        {
            return new DeviceService(db).GetEntityByid(id);
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
        public bool AddDoorAccess(Dev_DoorAccess doorAccess)
        {
            return db.Dev_DoorAccess.Add(doorAccess.ToDbModel());
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
            List<Dev_DoorAccess> devInfoList = new List<Dev_DoorAccess>();
            foreach (var pId in pidList)
            {
                devInfoList.AddRange(db.Dev_DoorAccess.DbSet.Where(item => item.ParentId == pId).ToList().ToTModel());
            }
            return devInfoList.ToWCFList();
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
        public bool ModifyCameraInfo(Dev_CameraInfo camInfo)
        {
            return db.Dev_CameraInfos.Edit(camInfo.ToDbModel());
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
            List<Personnel> ps = GetPersonList();
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

        public List<DeviceAlarm> GetDeviceAlarms(int count)
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
            var devs = db.DevInfos.ToList();
            if (devs == null) return null;
            List<DeviceAlarm> alarms = new List<DeviceAlarm>();
            alarms.Add(new DeviceAlarm() { Id = 927, Level = Abutment_DevAlarmLevel.低, Title = "告警1", Message = "设备告警1", CreateTime = new DateTime(2018, 8, 28, 9, 5, 34) }.SetDev(devs[926].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 8, Level = Abutment_DevAlarmLevel.中, Title = "告警2", Message = "设备告警2", CreateTime = new DateTime(2018, 8, 28, 9, 5, 34) }.SetDev(devs[7].ToTModel()));
//            alarms.Add(new DeviceAlarm() { Id = 1072, Level = Abutment_DevAlarmLevel.高, Title = "告警3", Message = "设备告警3", CreateTime = new DateTime(2018, 9, 1, 13, 44, 11) }.SetDev(devs[1071].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 930, Level = Abutment_DevAlarmLevel.中, Title = "告警4", Message = "设备告警4", CreateTime = new DateTime(2018, 9, 2, 14, 55, 20) }.SetDev(devs[929].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 4, Level = Abutment_DevAlarmLevel.低, Title = "告警5", Message = "设备告警5", CreateTime = new DateTime(2018, 9, 2, 13, 22, 44) }.SetDev(devs[3].ToTModel()));
            return alarms;
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

        public bool EditTag(Tag Tag, int? id)
        {
            bool bReturn = false;
            var lc = db.LocationCards.FirstOrDefault(p => p.Code == Tag.Code);
            if (lc == null)
            {
                lc = Tag.ToDbModel();
                lc.Abutment_Id = id;
                bReturn = db.LocationCards.Add(lc);
            }
            else
            {
                lc.Name = Tag.Name;
                lc.Describe = Tag.Describe;
                lc.Abutment_Id = id;
                lc.IsActive = Tag.IsActive;
                bReturn = db.LocationCards.Edit(lc);
            }
            
            return bReturn;
        }

        public bool EditBusTag(Tag Tag)
        {
            bool bDeal = false;
            int nFlag = 0;
            var btag = db.bus_tags.FirstOrDefault(p => p.tag_id == Tag.Code);
            if (btag == null)
            {
                btag = new DbModel.Engine.bus_tag();
                nFlag = 1;
            }

            btag.tag_id = Tag.Code;
            
            if (nFlag == 0)
            {
                bDeal = db.bus_tags.Edit(btag);
                
            }
            else
            {
                bDeal = db.bus_tags.Add(btag);
            }

            if (!bDeal)
            {
                return bDeal;
            }

            bDeal = EditTag(Tag, btag.Id);

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

        /// <summary>
        /// 根据KKS获取设备的信息、监控项，及设备树
        /// </summary>
        /// <param name="KKS"></param>
        /// <param name="bFlag"></param>
        /// <returns></returns>
        public Dev_Monitor GetDevMonitorInfoByKKS(string KKS, bool bFlag)
        {
            Dev_Monitor send = new Dev_Monitor();
            DbModel.Location.AreaAndDev.KKSCode dev = db.KKSCodes.FirstOrDefault(p=>p.Code == KKS);
            if (dev == null)
            {
                send = null;
                return send;
            }

            send.Name = dev.Name;
            send.KKSCode = dev.Code;
            send.MonitorNodeList = GetDevMonitorNodeListByKKS(send.KKSCode);
            send.ChildrenList = GetChildMonitorDev(send.KKSCode, bFlag);

            return send;
        }

        private List<Dev_Monitor> GetChildMonitorDev(string KKS, bool bFlag)
        {
            List<Dev_Monitor> child = new List<Dev_Monitor>();
            List<DbModel.Location.AreaAndDev.KKSCode> DevList = db.KKSCodes.Where(p => p.ParentCode == KKS).ToList();
            foreach (DbModel.Location.AreaAndDev.KKSCode item in DevList)
            {
                Dev_Monitor dev = new Dev_Monitor();
                dev.Name = item.Name;
                dev.KKSCode = item.Code;
                dev.MonitorNodeList = GetDevMonitorNodeListByKKS(dev.KKSCode);
                dev.ChildrenList = GetChildMonitorDev(dev.KKSCode, bFlag);
                if (!bFlag && dev.MonitorNodeList == null && dev.ChildrenList == null)
                {
                    continue;
                }

                child.Add(dev);
            }

            if (child.Count == 0)
            {
                child = null;
            }

            return child;
        }

        private List<DevMonitorNode> GetDevMonitorNodeListByKKS(string KKS)
        {
            List<DevMonitorNode> NodeList = null;
            List<DevMonitorNode> NodeList2 = new List<DevMonitorNode>();

            string strKKS = KKS.Replace(" ","");
            DbModel.Location.AreaAndDev.DevMonitorNode Node1 = db.DevMonitorNodes.FirstOrDefault(p => p.KKS == strKKS);
            if (Node1 != null)
            {
                NodeList = new List<DevMonitorNode>();
                DevMonitorNode Node = new DevMonitorNode();
                Node.Id = Node1.Id;
                Node.KKS = Node1.KKS;
                Node.TagName = Node1.TagName;
                Node.DataBaseName = Node1.DataBaseName;
                Node.DataBaseTagName = Node1.DataBaseTagName;
                Node.Describe = Node1.Describe;
                Node.Value = Node1.Value;
                Node.Unit = Node1.Unit;
                Node.DataType = Node1.DataType;
                Node.TagType = Node1.TagType;

                NodeList.Add(Node);
            }

            DbRawSqlQuery<DbModel.Location.AreaAndDev.DevMonitorNode> query2 = db.Db.Database.SqlQuery<DbModel.Location.AreaAndDev.DevMonitorNode>("select * from DevMonitorNodes where KKS like '%" + strKKS + "_%' and Id != " + Convert.ToString(Node1.Id));
            if (query2 == null)
            {
                return NodeList;
            }


            List<DbModel.Location.AreaAndDev.DevMonitorNode> MonitorNodeList = query2.ToList();
           
            foreach (DbModel.Location.AreaAndDev.DevMonitorNode item in MonitorNodeList)
            {
                if (NodeList == null)
                {
                    NodeList = new List<DevMonitorNode>();
                }

                DevMonitorNode NodeMonitor = new DevMonitorNode();
                NodeMonitor.Id = item.Id;
                NodeMonitor.KKS = item.KKS;
                NodeMonitor.TagName = item.TagName;
                NodeMonitor.DataBaseName = item.DataBaseName;
                NodeMonitor.DataBaseTagName = item.DataBaseTagName;
                NodeMonitor.Describe = item.Describe;
                NodeMonitor.Value = item.Value;
                NodeMonitor.Unit = item.Unit;
                NodeMonitor.DataType = item.DataType;
                NodeMonitor.TagType = item.TagType;

                NodeList.Add(NodeMonitor);
            }

            return NodeList;
        }
    }
}
