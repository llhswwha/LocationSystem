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
        public bool AddDevInfoByList(List<DevInfo> devInfoList)
        {
            return db.DevInfos.AddRange(devInfoList.ToDbModel());
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
                var dev = db.DevInfos.DeleteById(item.DevID);
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
        public List<LocationAlarm> GetLocationAlarms(AlarmSearchArg arg)
        {
            List<Personnel> ps = GetPersonList();
            List<LocationAlarm> alarms = new List<LocationAlarm>();
            alarms.Add(new LocationAlarm() { Id = 0, TagId = 1, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)4, Content = "进入无权限区域", CreateTime = new DateTime(2018, 9, 1, 10, 5, 34) }.SetPerson(ps[0]));
            alarms.Add(new LocationAlarm() { Id = 1, TagId = 2, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)3, Content = "靠近高风险区域", CreateTime = new DateTime(2018, 9, 4, 15, 5, 11) }.SetPerson(ps[1]));
            alarms.Add(new LocationAlarm() { Id = 2, TagId = 3, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)2, Content = "进入高风险区域", CreateTime = new DateTime(2018, 9, 7, 13, 35, 20) }.SetPerson(ps[2]));
            alarms.Add(new LocationAlarm() { Id = 3, TagId = 4, AlarmType = 0, AlarmLevel = (LocationAlarmLevel)1, Content = "进入危险区域", CreateTime = new DateTime(2018, 9, 10, 16, 15, 44) }.SetPerson(ps[3]));
            return alarms;
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
                if(isAlarm)
                {
                    alarms.Add(new LocationAlarm()
                    {
                        Id = i,
                        TagId = 1,
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
                        TagId = 1,
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
            List<DeviceAlarm> alarms = new List<DeviceAlarm>();
            alarms.Add(new DeviceAlarm() { Id = 0, Level = Abutment_DevAlarmLevel.低, Title = "告警1", Message = "设备告警1", CreateTime = new DateTime(2018, 8, 28, 9, 5, 34) }.SetDev(devs[0].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 1, Level = Abutment_DevAlarmLevel.中, Title = "告警2", Message = "设备告警2", CreateTime = new DateTime(2018, 8, 28, 9, 5, 34) }.SetDev(devs[0].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 2, Level = Abutment_DevAlarmLevel.高, Title = "告警3", Message = "设备告警3", CreateTime = new DateTime(2018, 9, 1, 13, 44, 11) }.SetDev(devs[1].ToTModel()));
            alarms.Add(new DeviceAlarm() { Id = 3, Level = Abutment_DevAlarmLevel.中, Title = "告警4", Message = "设备告警4", CreateTime = new DateTime(2018, 9, 2, 14, 55, 20) }.SetDev(devs[2].ToTModel()));
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
            return db.Archors.Find(i=>i.DevInfoId==devId).ToTModel();
        }

        public bool AddArchor(Archor archor)
        {
            if (archor.DevInfo != null)
            {
                archor.ParentId = archor.ParentId;
            }
            return db.Archors.Add(archor.ToDbModel());
        }
        public void DeleteArchor(int archorId)
        {
            db.Archors.DeleteById(archorId);
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
                Archor2 = Archor.ToDbModel();

                DbModel.Location.AreaAndDev.DevInfo dev = new DbModel.Location.AreaAndDev.DevInfo();
                dev.Local_DevID = Guid.NewGuid().ToString();
                dev.IP = "";
                dev.KKS = "";
                dev.Name = Archor2.Name;
                dev.ModelName = LocationDeviceHelper.LocationDevModelName;
                dev.Status = 0;
                dev.ParentId = ParentId;
                dev.Local_TypeCode = int.Parse(LocationDeviceHelper.LocationDevTypeCode);
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
                bac.min_x = 90000000;
                bac.max_x = 90000000;
                bac.min_y = 90000000;
                bac.max_y = 90000000;
                bac.min_z = 90000000;
                bac.max_z = 90000000;
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

            bDeal = EditTag(Tag, btag.id);

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

        public List<NearbyDev> GetNearbyDev_Currency(int id)
        {
            DeviceService ds = new DeviceService();
            List<NearbyDev> lst = ds.GetNearbyDev_Currency(id);
            if (lst == null)
            {
                lst = new List<NearbyDev>();
            }

            return lst;
        }

        public List<NearbyDev> GetNearbyCamera_Alarm(int id)
        {
            DeviceService ds = new DeviceService();
            List<NearbyDev> lst = ds.GetNearbyCamera_Alarm(id);
            if (lst == null)
            {
                lst = new List<NearbyDev>();
            }

            return lst;
        }
    }
}
