using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CommunicationClass;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.AreaAndDev;
using BLL;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.AreaAndDev;
using Location.TModel.Tools;
using System.IO;
using DbModel.Tools;
using DbModel.Location.Work;
using DbModel.Location.Alarm;
using DbModel.Location.Data;
using DbModel.LocationHistory.Data;
using DbModel.LocationHistory.Work;
using DbModel.LocationHistory.Alarm;
using DbModel.BaseData;
using DAL;
using BLL.Blls;
using Location.BLL.Tool;
using IModel.Enums;
using System.Threading;

namespace WebApiLib.Clients
{
    public class BaseDataClient
    {
        private Bll _bll = null;

        protected Bll bll
        {
            get
            {
                if (_bll == null)
                {
                    _bll = Bll.Instance();
                }
                return _bll;
            }
        }

        private string Message;

        public string BaseUri { get; set; }

        public static long nEightHourSecond = 0;

        BaseDataInnerClient client;

        //public BaseDataClient()
        //{
        //    BaseUri = "http://<host>:<port>/";
        //    client = new BaseDataInnerClient(BaseUri);
        //}

        //public BaseDataClient(string host, string port)
        //{
        //    //BaseUri = string.Format("http://{0}:{1}/", host, port);
        //    BaseUri = string.Format("http://{0}:{1}/", host, port);
        //    client = new BaseDataInnerClient(BaseUri);
        //}

        public BaseDataClient(string host, string port,string suffix)
        {
            //BaseUri = string.Format("http://{0}:{1}/", host, port);
            if (string.IsNullOrEmpty(port))
            {
                BaseUri = string.Format("http://{0}/{1}/", host, suffix);
            }
            else
            {
                BaseUri = string.Format("http://{0}:{1}/{2}/", host, port, suffix);
            }
            client = new BaseDataInnerClient(BaseUri);
        }

        //public BaseDataClient(string baseUri)
        //{
        //    BaseUri = baseUri;
        //}

        public BaseTran<T> GetEntityList<T>(string url)
        {
            var recv = new BaseTran<T>();
            try
            {
                recv = WebApiHelper.GetEntity<BaseTran<T>>(url);
                if (recv == null) return null;
                if (recv.data == null)
                {
                    recv.data = new List<T>();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return null;
            }
            return recv;
        }


        /// <summary>
        /// 获取Sis数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public SisData_Compact GetSisData(string url)
        {
            return client.GetSisData(url);
        }

        public List<user> GetUserList()
        {
            return client.GetUserList();
        }

        /// <summary>
        /// 获取人员列表
        /// </summary>
        /// <returns></returns>
        public List<Personnel> GetPersonnelList(bool isSave)
        {
            Bll bll = Bll.NewBllNoRelation();
            List<Personnel> plst = bll.Personnels.ToList();
            List<Department> dlst = bll.Departments.ToList();
            
            List<Personnel> personnelList = new List<Personnel>();
            List<string> errorlst = new List<string>();

            try
            {
                var users = GetUserList();

                foreach (user item in users)
                {
                    //0表示添加，1表示修改
                    int nFlag = 1;

                    //先根据人员Id获取
                    Personnel person = plst.Find(p => p.Abutment_Id == item.id);
                    if (person == null)
                    {
                        person = new Personnel();
                        person.Pst = "检修";
                        nFlag = 0;
                    }

                    BaseDataHelper.SetPersonnel(person, item);

                    if (item.dept_name == null)
                    {
                        Department Department = dlst.Find(p => p.Name == "未绑定");
                        person.ParentId = Department.Id;
                    }
                    else
                    {
                        Department Department = dlst.Find(p => p.Name == item.dept_name);
                        if (Department == null)
                        {
                            string strInfo = "获取人员列表错误信息：找不到匹配的部门  id=" + item.id + "  Name=" + item.name + " Depart=" + item.dept_name;
                            Error(strInfo);
                            continue;
                        }

                        person.ParentId = Department.Id;
                    }

                    if (nFlag == 0)
                    {
                        if(isSave)
                            bll.Personnels.Add(person);
                        plst.Add(person);
                    }
                    else
                    {
                        if (isSave)
                            bll.Personnels.Edit(person);
                    }

                    personnelList.Add(person);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }


            if (errorlst.Count > 0)
            {
                WriteWrongInfo(errorlst, "Discard_PersonList.txt");
            }

            return personnelList;
        }


        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<Department> GetDepList(bool isSave)
        {
            Bll bll = Bll.NewBllNoRelation();
            List<Department> dlst = bll.Departments.ToList();
            List<Department> depList = new List<Department>();
            try
            {
                var orgList = GetOrgList();
                foreach (org org in orgList)
                {
                    //0表示添加，1表示修改
                    int nFlag = 1;

                    Department dept = dlst.Find(p => p.Abutment_Id == org.id);
                    if (dept == null)//不存在则添加
                    {
                        dept = new Department();
                        dept.ShowOrder = 0;
                        nFlag = 0;
                    }

                    dept.Abutment_Id = org.id;
                    dept.Name = org.name;
                    dept.Abutment_ParentId = org.parentId;
                    dept.Type = (DepartType)org.type;
                    dept.Description = org.description;

                    if (nFlag == 0)
                    {
                        if(isSave)
                            bll.Departments.Add(dept);
                        dlst.Add(dept);
                    }
                    else
                    {
                        if (isSave)
                            bll.Departments.Edit(dept);
                    }

                    depList.Add(dept);
                }

                foreach (Department item2 in depList)
                {
                    if (item2.Abutment_ParentId == null)
                    {
                        continue;
                    }

                    Department Parent = dlst.Find(p => p.Abutment_Id == item2.Abutment_ParentId);
                    if (Parent == null)
                    {
                        continue;
                    }

                    int nCount = dlst.FindAll(p => p.ParentId == Parent.Id).Count();
                    if (item2.ParentId == Parent.Id)
                    {
                        continue;
                    }
                    item2.ParentId = Parent.Id;
                    item2.ShowOrder = nCount;

                    if (isSave)
                        bll.Departments.Edit(item2);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return depList;
        }

        public List<zone> GetZoneList()
        {
            return client.GetZoneList();
        }

        public List<org> GetOrgList()
        {
            return client.GetOrgList();
        }

        /// <summary>
        /// 获取区域列表，并更新数据库
        /// </summary>
        /// <returns></returns>
        public List<Area> GetAreaList(bool isSave)
        {
            Bll bll = new Bll();
            List<Area> alst = bll.Areas.ToList();
            List<Area> areaList = new List<Area>();
            List<string> errorlst = new List<string>();
            try
            {
                var zoneList = GetZoneList();
                foreach (zone item in zoneList)
                {
                    if (item.kks == null)
                    {
                        string strInfo = "获取获取区域列表错误信息：KKS为null  id=" + item.id + "  Name=" + item.name + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }
                    Area area = alst.Find(p=>p.KKS == item.kks);
                    if (area == null)
                    {
                        string strInfo = "获取获取区域列表错误信息：根据KKS找不到对应的区域 id=" + item.id + "  Name=" + item.name + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }
                    area.Abutment_Id = item.id;
                    area.Abutment_ParentId = item.parent_Id;
                    areaList.Add(area);
                }

                foreach (Area item2 in areaList)
                {
                    if (item2.Abutment_ParentId == null)
                    {
                        continue;
                    }
                    Area parent = alst.Find(p => p.Abutment_Id == item2.Abutment_ParentId);
                    if (parent == null)
                    {
                        continue;
                    }
                    if (item2.ParentId == parent.Id)
                    {
                        continue;
                    }
                    item2.ParentId = parent.Id;
                    if (isSave)
                        bll.Areas.Edit(item2);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            if (errorlst.Count > 0)
            {
                WriteWrongInfo(errorlst, "Discard_AreaList.txt");
            }



            return areaList;
        }

        public zone GetZoneDetail(int id,int view)
        {
            return client.GetZoneDetail(id, view);
        }

        /// <summary>
        /// 获取单个区域信息
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="view">视图掩码，要附加内容时将掩码加上，1附加子区域，2附加关联设备，3附加(危险)物资，如返回字区域，则view=1+2,即view=3</param>
        /// <returns></returns>
        public Area GetAreaDetail(int id, int view)
        {
            zone zone = new zone();
            Area send = new Area();

            //return send;
            string strId = Convert.ToString(id);
            string strView = Convert.ToString(view);
            try
            {
                zone = GetZoneDetail(id, view);
                Area area = bll.Areas.DbSet.Where(p => p.Abutment_Id == zone.id).FirstOrDefault();

                int nFlag = 0;
                if (area == null)
                {
                    area = new Area();
                    area.IsRelative = false;
                    area.Type = 0;
                    nFlag = 1;
                }

                area.Abutment_Id = zone.id;
                area.Name = zone.name;
                area.KKS = zone.kks;
                area.Abutment_ParentId = zone.parent_Id;
                area.Describe = zone.description;

                if (zone.parent_Id != null)
                {
                    Area Parent = bll.Areas.DbSet.Where(p => p.Abutment_Id == zone.parent_Id).FirstOrDefault();
                    area.ParentId = Parent.Id;
                }

                if (nFlag == 1)
                {
                    bll.Areas.Add(area);
                }
                else
                {
                    bll.Areas.Edit(area);
                }
                
                send = area.Clone();

                foreach (zone item2 in zone.zones)
                {
                    Area area2 = bll.Areas.DbSet.Where(p => p.Abutment_Id == item2.id).FirstOrDefault();
                    nFlag = 0;
                    if (area2 == null)
                    {
                        area2 = new Area();
                        area2.Abutment_Id = item2.id;
                        area2.IsRelative = false;
                        area2.Type = 0;
                        nFlag = 1;
                    }

                    area2.ParentId = area.Id;
                    area2.Name = item2.name;
                    area2.KKS = item2.kks;
                    area2.Abutment_ParentId = item2.parent_Id;
                    area2.Describe = item2.description;

                    if (nFlag == 1)
                    {
                        bll.Areas.Add(area2);
                    }
                    else
                    {
                        bll.Areas.Edit(area2);
                    }

                    send.Children.Add(area2);
                }

                foreach (device item3 in zone.devices)
                {
                    DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == item3.kks).FirstOrDefault();
                    nFlag = 0;
                    if (devinfo == null)
                    {
                        devinfo = new DevInfo();
                        nFlag = 1;
                    }

                    devinfo.ParentId = area.Id;
                    devinfo.Abutment_Id = item3.id;
                    devinfo.Code = item3.code;
                    devinfo.KKS = item3.kks;
                    devinfo.Name = item3.name;
                    devinfo.Abutment_Type = (Abutment_DevTypes)item3.type;
                    devinfo.Status = (Abutment_Status)item3.state;
                    devinfo.RunStatus = (Abutment_RunStatus)item3.running_state;
                    devinfo.Placed = item3.placed;
                    devinfo.Abutment_DevID = item3.raw_id;
                    devinfo.IP = item3.ip;

                    if (nFlag == 1)
                    {
                        bll.DevInfos.Add(devinfo); 
                    }
                    else
                    {
                        bll.DevInfos.Edit(devinfo);
                    }

                    send.LeafNodes.Add(devinfo);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取指定区域下设备列表
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public List<device> GetZoneDeviceList(int zoneId)
        {
            return client.GetZoneDeviceList(zoneId);
        }

        /// <summary>
        /// 获取指定区域下设备列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DevInfo> GetZoneDevList(int id)
        {
            
            //string strId = Convert.ToString(id);
            List<DevInfo> send = new List<DevInfo>();

            //return send;
            try
            {
                var devices = GetZoneDeviceList(id);

                Area area = bll.Areas.DbSet.Where(p => p.Abutment_Id == id).FirstOrDefault();
                if (area != null)
                {
                    foreach (device item in devices)
                    {
                        DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();
                        int nFlag = 0;
                        if (devinfo == null)
                        {
                            devinfo = new DevInfo();
                            nFlag = 1;
                        }

                        devinfo.ParentId = area.Id;
                        BaseDataHelper.SetDeivInfo(devinfo, item);

                        if (nFlag == 1)
                        {
                            devinfo.CreateTime = DateTime.Now;
                            devinfo.ModifyTime = DateTime.Now;

                            devinfo.CreateTimeStamp = TimeConvert.ToStamp(devinfo.CreateTime);
                            devinfo.ModifyTimeStamp = TimeConvert.ToStamp(devinfo.ModifyTime);

                            bll.DevInfos.Add(devinfo);
                        }
                        else
                        {
                            devinfo.ModifyTime = DateTime.Now;
                            devinfo.ModifyTimeStamp = TimeConvert.ToStamp(devinfo.ModifyTime);

                            bll.DevInfos.Edit(devinfo);
                        }

                        send.Add(devinfo);
                    }

                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="types"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<device> GetDeviceList(string types, string code, string name)
        {
            return client.GetDeviceList(types, code, name);
        }

        public List<string> errorLst = new List<string>();

        public void Error(string error)
        {
            errorLst.Add(error);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="types">设备类型，逗号分隔，只查询指定的设备类型</param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<DevInfo> GetDevInfoList(string types, string code, string name,bool isSave)
        {
            Bll bll = new Bll();
            List<DevInfo> dlst = bll.DevInfos.ToList();
            List<Dev_CameraInfo> dclst = bll.Dev_CameraInfos.ToList();
            
            List<DevInfo> send = new List<DevInfo>();

            try
            {
                var deviceList = GetDeviceList(types, code, name);
                foreach (device item in deviceList)
                {
                    if (item.type != 102 && item.type != 1021 && item.type != 1022 && item.type != 1023)
                    {
                        string strInfo = "获取设备列表错误信息：不是摄像头  id=" + item.id + "  Name=" + item.name + "  Type=" + Convert.ToString(item.type) + "  Code=" + item.code + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }

                    if (item.ip == null || item.ip == "")
                    {
                        string strInfo = "获取设备列表错误信息：IP为空  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }

                    Dev_CameraInfo dc = dclst.Find(p => p.Ip == item.ip);
                    if (dc == null)
                    {
                        string strInfo = "获取设备列表错误信息：找不到匹配的IP  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }
                    dc.RtspUrl = item.uri;

                    if (isSave)
                    {
                        bll.Dev_CameraInfos.Edit(dc);
                    }

                    DevInfo devinfo = dlst.Find(p => p.Id == dc.DevInfoId);
                    if (devinfo == null)
                    {
                        string strInfo = "获取设备列表错误信息：摄像头找不到匹配的设备  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }

                    BaseDataHelper.SetDeivInfo(devinfo, item);

                    if (isSave)
                        bll.DevInfos.Edit(devinfo);

                    send.Add(devinfo);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            if (errorLst.Count > 0)
            {
                WriteWrongInfo(errorLst, "Discard_DevList.txt");
            }

            return send;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="types">设备类型，逗号分隔，只查询指定的设备类型</param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Dev_CameraInfo> GetCameraInfoList(string types, string code, string name, bool isSave)
        {
            Bll bll = new Bll();
            List<DevInfo> dlst = bll.DevInfos.ToList();
            List<Dev_CameraInfo> dclst = bll.Dev_CameraInfos.ToList();

            List<DevInfo> send = new List<DevInfo>();

            try
            {
                var deviceList = GetDeviceList(types, code, name);
                for (int i = 0; i < deviceList.Count; i++)
                {
                    device item = deviceList[i];
                    if (item.type != 102 && item.type != 1021 && item.type != 1022 && item.type != 1023)
                    {
                        string strInfo = "获取设备列表错误信息：不是摄像头  id=" + item.id + "  Name=" + item.name + "  Type=" + Convert.ToString(item.type) + "  Code=" + item.code + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }

                    if (item.ip == null || item.ip == "")
                    {
                        string strInfo = "获取设备列表错误信息：IP为空  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }

                    Dev_CameraInfo dc = dclst.Find(p => p.Ip == item.ip);
                    if (dc == null)
                    {
                        string strInfo = "获取设备列表错误信息：找不到匹配的IP  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }
                    dc.RtspUrl = item.uri;

                    if (isSave)
                    {
                        bll.Dev_CameraInfos.Edit(dc);
                    }

                    DevInfo devinfo = dlst.Find(p => p.Id == dc.DevInfoId);
                    if (devinfo == null)
                    {
                        string strInfo = "获取设备列表错误信息：摄像头找不到匹配的设备  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        Error(strInfo);
                        continue;
                    }

                    BaseDataHelper.SetDeivInfo(devinfo, item);

                    if (isSave)
                        bll.DevInfos.Edit(devinfo);

                    send.Add(devinfo);

                    if (i % 20 == 0)
                    {
                        Log.Info(LogTags.BaseData, string.Format("获取摄像头:{0}({1}/{2})", item.name, i, deviceList.Count));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            if (errorLst.Count > 0)
            {
                WriteWrongInfo(errorLst, "Discard_DevList.txt");
            }

            return dclst;
        }

        public device GetDeviceDetail(int id)
        {
            return client.GetDeviceDetail(id);
        }

        /// <summary>
        /// 获取单个设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DevInfo GetDevInfoDetail(int id)
        {
            DevInfo send = new DevInfo();

            try
            {
                device recv = GetDeviceDetail(id);

                DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == recv.kks).FirstOrDefault();
                int nFlag = 0;
                if (devinfo == null)
                {
                    devinfo = new DevInfo();
                    nFlag = 1;
                }

                devinfo.Abutment_Id = recv.id;
                devinfo.Code = recv.code;
                devinfo.KKS = recv.kks;
                devinfo.Name = recv.name;
                devinfo.Abutment_Type = (Abutment_DevTypes)recv.type;
                devinfo.Status = (Abutment_Status)recv.state;
                devinfo.RunStatus = (Abutment_RunStatus)recv.running_state;
                devinfo.Placed = recv.placed;
                devinfo.Abutment_DevID = recv.raw_id;
                devinfo.IP = recv.ip;
                devinfo.Manufactor = "霍尼韦尔";

                if (nFlag == 1)
                {
                    devinfo.CreateTime = DateTime.Now;
                    devinfo.ModifyTime = DateTime.Now;

                    devinfo.CreateTimeStamp = TimeConvert.ToStamp(devinfo.CreateTime);
                    devinfo.ModifyTimeStamp = TimeConvert.ToStamp(devinfo.ModifyTime);

                    bll.DevInfos.Add(devinfo);
                }
                else
                {
                    devinfo.ModifyTime = DateTime.Now;
                    devinfo.ModifyTimeStamp = TimeConvert.ToStamp(devinfo.ModifyTime);

                    bll.DevInfos.Edit(devinfo);
                }

                send = devinfo;
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取单台设备操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public List<devices_actions> GetDeviceActions(int id, string begin_date, string end_date)
        {
            return client.GetDeviceActions(id, begin_date, end_date);
        }

        /// <summary>
        /// 获取单台设备操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public List<DevEntranceGuardCardAction> GetSingleDeviceActionHistory(int id, string begin_date, string end_date)
        {
            
            List<DevEntranceGuardCardAction> send = new List<DevEntranceGuardCardAction>();

            List<devices_actions> actions = GetDeviceActions(id, begin_date, end_date);

             DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.Abutment_Id == id).FirstOrDefault();
            if (devinfo != null)
            {
                foreach (devices_actions item in actions)
                {
                    DevEntranceGuardCardAction degca = bll.DevEntranceGuardCardActions.DbSet.Where(p => p.Abutment_Id == item.id).FirstOrDefault();
                    EntranceGuardCard egc = bll.EntranceGuardCards.DbSet.Where(p => p.Code == item.card_code).FirstOrDefault();
                    if (egc == null)
                    {
                        continue;
                    }

                    int nFlag = 0;

                    if (degca == null)
                    {
                        degca = new DevEntranceGuardCardAction();
                        degca.OperateTime = null;
                        nFlag = 1;
                    }

                    degca.Abutment_Id = item.id;
                    degca.DevInfoId = devinfo.Id;
                    degca.EntranceGuardCardId = egc.Id;
                    degca.code = item.code;
                    degca.description = item.description;
                    degca.OperateTimeStamp = item.t;
                    degca.nInOutState = 0;

                    if (item.t != null)
                    {
                        long t = (long)item.t;
                        degca.OperateTime = TimeConvert.ToDateTime(t);

                    }

                    if (nFlag == 1)
                    {
                        bll.DevEntranceGuardCardActions.Add(degca);
                    }
                    else
                    {
                        bll.DevEntranceGuardCardActions.Edit(degca);
                    }

                    send.Add(degca);
                }
            }

            return send;
        }

        /// <summary>
        /// 获取门禁卡列表
        /// </summary>
        /// <returns></returns>
        public List<cards> GetCardList()
        {
            return client.GetCardList();
        }

        /// <summary>
        /// 获取门禁卡列表
        /// </summary>
        /// <returns></returns>
        public List<EntranceGuardCard> GetGuardCardList(bool isSave)
        {
            Bll bll = new Bll();
            List<EntranceGuardCard> elst = bll.EntranceGuardCards.ToList();
            List<Personnel> plst = bll.Personnels.ToList();
            List<EntranceGuardCardToPersonnel> eglst = bll.EntranceGuardCardToPersonnels.ToList();
            List<EntranceGuardCard> send = new List<EntranceGuardCard>();

            try
            {
                var cards = GetCardList();
                foreach (cards item in cards)
                {
                    EntranceGuardCard egc = elst.Find(p=>p.Code == item.cardCode);
                    int nFlag = 1;
                    if (egc == null)
                    {
                        egc = new EntranceGuardCard();
                        nFlag = 0;
                    }

                    egc.Abutment_Id = item.cardId;
                    egc.Code = item.cardCode;
                    egc.State = item.state;

                    if (nFlag == 0)
                    {
                        if(isSave)
                            bll.EntranceGuardCards.Add(egc);
                        elst.Add(egc);
                    }
                    else
                    {
                        if (isSave)
                            bll.EntranceGuardCards.Edit(egc);
                    }

                    send.Add(egc);

                    if (item.emp_id == null)
                    {
                        continue;
                    }

                    Personnel personnel = plst.Find(p=>p.Abutment_Id == item.emp_id);
                    if (personnel == null)
                    {
                        continue;
                    }

                    EntranceGuardCardToPersonnel egctp = eglst.Find(p=>p.PersonnelId == personnel.Id && p.EntranceGuardCardId == egc.Id);
                    if (egctp != null)
                    {
                        continue;
                    }

                    EntranceGuardCardToPersonnel egctp1 = eglst.Find(p => p.PersonnelId == personnel.Id);
                    EntranceGuardCardToPersonnel egctp2 = eglst.Find(p => p.EntranceGuardCardId == egc.Id);
                    if (egctp1 != null)
                    {
                        eglst.Remove(egctp1);
                        if (isSave)
                            bll.EntranceGuardCardToPersonnels.DeleteById(egctp1.Id);
                    }

                    if (egctp2 != null)
                    {
                        eglst.Remove(egctp2);
                        if (isSave)
                            bll.EntranceGuardCardToPersonnels.DeleteById(egctp2.Id);
                    }

                    egctp = new EntranceGuardCardToPersonnel();
                    egctp.PersonnelId = personnel.Id;
                    egctp.EntranceGuardCardId = egc.Id;
                    if (isSave)
                        bll.EntranceGuardCardToPersonnels.Add(egctp);
                    eglst.Add(egctp);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取门禁卡操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public List<cards_actions> GetCardActions(int id, string begin_date, string end_date)
        {
            return client.GetCardActions(id, begin_date, end_date);
        }

        /// <summary>
        /// 获取门禁卡操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public List<TModel.LocationHistory.AreaAndDev.EntranceGuardActionInfo> GetSingleCardActionHistory(int id, string begin_date, string end_date)
        {
            Bll bll = new Bll();
            List<EntranceGuardCard> elst = bll.EntranceGuardCards.ToList();
            List<DevEntranceGuardCardAction> delst = bll.DevEntranceGuardCardActions.ToList();
            List<DevInfo> dlst = bll.DevInfos.ToList();
           
            //List<DevEntranceGuardCardAction> send = new List<DevEntranceGuardCardAction>();
            List<TModel.LocationHistory.AreaAndDev.EntranceGuardActionInfo> send = new List<TModel.LocationHistory.AreaAndDev.EntranceGuardActionInfo>();

            try
            {
                var actions = GetCardActions(id, begin_date, end_date);

                EntranceGuardCard egc = elst.Find(p => p.Abutment_Id == id);
                if (egc == null)
                {
                    return send;
                }

                foreach (cards_actions item in actions)
                {
                    if (item.card_code != egc.Code)
                    {
                        continue;
                    }

                    DevEntranceGuardCardAction degca = delst.Find(p=>p.Abutment_Id == item.id);
                    DevInfo devinfo = dlst.Find(p=>p.Abutment_Id == item.device_id);
                    if (devinfo == null)
                    {
                        continue;
                    }

                    if (degca == null)
                    {
                        degca = new DevEntranceGuardCardAction();
                        degca.Abutment_Id = item.id;
                        degca.DevInfoId = devinfo.Id;
                        degca.EntranceGuardCardId = egc.Id;
                        degca.code = item.code;
                        degca.description = item.description;
                        degca.nInOutState = 0;

                        if (item.t != null)
                        {
                            long t = (long)item.t + nEightHourSecond;
                            t = 1000 * t;
                            degca.OperateTimeStamp = t;
                            degca.OperateTime = TimeConvert.ToDateTime(t);
                        }

                        bll.DevEntranceGuardCardActions.Add(degca);
                        delst.Add(degca);
                    }

                    TModel.LocationHistory.AreaAndDev.EntranceGuardActionInfo sendElement = new TModel.LocationHistory.AreaAndDev.EntranceGuardActionInfo();
                    sendElement.Id = degca.Id;
                    sendElement.Name = devinfo.Name;
                    sendElement.AreadId = devinfo.ParentId;
                    if (devinfo.Parent != null)
                    {
                        sendElement.AreadName = devinfo.Parent.Name;
                    }
                    sendElement.Code = egc.Code;
                    sendElement.OperateTime = degca.OperateTime;

                    send.Add(sendElement);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取两票列表
        /// </summary>
        /// <param name="type">1:操作票；2:工作票</param>
        /// <param name="begin_date">格式：yyyyMMdd 默认为当天</param>
        /// <param name="end_date">跨度最大一个月</param>
        /// <returns></returns>
        public BaseTran<tickets> GetTicketsList(string type, string begin_date, string end_date)
        {
            BaseTran<tickets> recv = new BaseTran<tickets>();

            try
            {
                recv = client.GetTicketsList(type, begin_date, end_date);
                foreach (tickets item in recv.data)
                {
                    int nFlag = 0;
                    if (item.type == 1)
                    {
                        OperationTicket ot = bll.OperationTickets.DbSet.Where(p => p.Abutment_Id == item.id).FirstOrDefault();

                        if (ot == null)
                        {
                            ot = new OperationTicket();
                            ot.OperationStartTime = DateTime.Now;
                            ot.OperationEndTime = DateTime.Now.AddDays(1);
                            nFlag = 1;
                        }

                        ot.Abutment_Id = item.id;
                        ot.No = item.code;
                        if (nFlag == 1)
                        {
                            bll.OperationTickets.Add(ot);
                        }
                        else
                        {
                            bll.OperationTickets.Edit(ot);
                        }
                    }
                    else
                    {
                        WorkTicket wt = bll.WorkTickets.DbSet.Where(p => p.Abutment_Id == item.id).FirstOrDefault();
                        if (wt == null)
                        {
                            wt = new WorkTicket();
                            wt.StartTimeOfPlannedWork = DateTime.Now;
                            wt.EndTimeOfPlannedWork = DateTime.Now.AddDays(1);
                            nFlag = 1;
                        }

                        wt.Abutment_Id = item.id;
                        wt.No = item.code;
                        if (nFlag == 1)
                        {
                            bll.WorkTickets.Add(wt);
                        }
                        else
                        {
                            bll.WorkTickets.Edit(wt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;
        }

        /// <summary>
        /// 获取指定的两票详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tickets GetTicketsDetail(int id, string begin_date, string end_date)
        {
            return client.GetTicketsDetail(id, begin_date, end_date);
        }

        /// <summary>
        /// 获取告警事件列表
        /// </summary>
        /// <param name="src"></param>
        /// <param name="level"></param>
        /// <param name="begin_t"></param>
        /// <param name="end_t"></param>
        /// <returns></returns>
        public List<events> GetEventList(int? src, int? level, long? begin_t, long? end_t)
        {
            return client.GetEventList(src, level, begin_t, end_t);
        }

            /// <summary>
            /// 获取告警事件列表
            /// </summary>
            /// <param name="src"></param>
            /// <param name="level"></param>
            /// <param name="begin_t"></param>
            /// <param name="end_t"></param>
            /// <returns></returns>
        public List<DevAlarm> GetDevAlarmList(int? src, int? level, long? begin_t, long? end_t)
        {
            Bll bll = new Bll();
            List<DevInfo> DevList = bll.DevInfos.ToList();
            List<DevAlarm> DaList = bll.DevAlarms.Where(p => p.Src == Abutment_DevAlarmSrc.视频监控 || p.Src == Abutment_DevAlarmSrc.门禁 || p.Src == Abutment_DevAlarmSrc.消防).ToList();
            List<DevAlarm> send = new List<DevAlarm>();
            try
            {
                var events = GetEventList(src, level, begin_t, end_t);
                foreach (events item in events)
                {
                    int nsrc = item.src;
                    DevInfo di = null;

                    if (nsrc == 1 || nsrc == 2)
                    {
                        if (item.raw_id == null || item.raw_id == "")
                        {
                            continue;
                        }

                        di = DevList.Find(p => p.Abutment_DevID == item.raw_id);
                    }
                    else if (nsrc == 3)
                    {
                        if (item.node == null || item.node == "")
                        {
                            continue;
                        }

                        di = DevList.Find(p => p.Code == item.node);
                    }

                    if (di == null)
                    {
                        continue;
                    }

                    bool bFlag = false;
                    int nLevel = (int)item.level;
                    Abutment_DevAlarmLevel adLevel = (Abutment_DevAlarmLevel)nLevel;

                    long lTimeStamp = item.t;

                    if (nLevel == 0)
                    {
                        adLevel = Abutment_DevAlarmLevel.未定;
                    }

                    DevAlarm da = DaList.Find(p => p.Abutment_Id == item.id);
                    if (da == null)
                    {
                        if (item.state == 0)
                        {
                            da = new DevAlarm();
                            da.Abutment_Id = item.id;
                            da.Title = item.title;
                            da.Msg = item.msg;
                            da.Level = adLevel;
                            da.Code = item.code;
                            da.Src = (Abutment_DevAlarmSrc)item.src;
                            da.DevInfoId = di.Id;
                            da.Device_desc = item.deviceDesc;
                            da.AlarmTime = TimeConvert.ToDateTime(lTimeStamp);
                            da.AlarmTimeStamp = lTimeStamp;
                            bll.DevAlarms.Add(da);
                            DaList.Add(da);
                            bFlag = true;
                        }
                    }
                    else
                    {
                        if (item.state == 1 || item.state == 2)
                        {
                            DevAlarmHistory da_history = da.RemoveToHistory();
                            bll.DevAlarms.DeleteById(da.Id);
                            bll.DevAlarmHistorys.Add(da_history);
                            DaList.Remove(da);
                            da.Level = Abutment_DevAlarmLevel.无;
                            bFlag = true;
                        }
                        else if (adLevel != da.Level)
                        {
                            da.Level = adLevel;
                            da.Title = item.title;
                            da.Msg = item.msg;
                            bll.DevAlarms.Edit(da);
                            bFlag = true;
                        }
                    }

                    if (bFlag)
                    {
                        send.Add(da);
                    }
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取SIS传感数据
        /// </summary>
        /// <param name="strTags"></param>
        /// <returns></returns>
        public List<sis> GetSisList(string strTags)
        {
            return client.GetSisList(strTags);
        }

        public string GetSisUrl(string tags)
        {
            return client.GetSisUrl(tags);
        }

        /// <summary>
        /// 获取SIS传感数据
        /// </summary>
        /// <param name="kks"></param>
        /// <returns></returns>
        public List<DevMonitorNode> GetSomesisList(string tags, bool isSaveToDb)
        {
            List<DevMonitorNode> monitorNodes = new List<DevMonitorNode>();
            var sisList = GetSisList(tags);//到基础平台获取数据
            if (isSaveToDb)
            {
                SisDataSaveClient.Instance.Save(sisList);
            }
            if (sisList != null)//这里判断是不是null，能够判断出来是否通信正常，包括url是否正确。
            {
                foreach (sis sis in sisList)
                {
                    DevMonitorNode monitor=new DevMonitorNode();
                    monitor.TagName = sis.kks;
                    monitor.Value = sis.value;
                    monitor.Unit = sis.unit;
                    monitor.Time = sis.t;
                    monitorNodes.Add(monitor);
                }
            }
            else
            {
                Log.Error(LogTags.KKS, string.Format("获取数据失败[{0}]:{1}",tags.Length,tags));
            }
            return monitorNodes;
        }



        /// <summary>
        /// 获取SIS历史数据，当compact为true时，获取紧凑型数据，当compact为false时，获取非紧凑型数据
        /// </summary>
        /// <param name="kks"></param>
        /// <param name="compact"></param>
        /// <returns></returns>
        public List<DevMonitorNodeHistory> GetSomeSisHistoryList(string kks, bool compact)
        {
            BaseTran<sis> recv = new BaseTran<sis>();
            SisData_Compact recv2 = new SisData_Compact();
            List<DevMonitorNodeHistory> send = new List<DevMonitorNodeHistory>();
            
            try
            {
                string[] sArray = BaseUri.Split(new string[] { "api" }, StringSplitOptions.RemoveEmptyEntries);
                string BaseUri2 = sArray[0];
                BaseUri2 = BaseUri2 + "api-viz/";
                string path = "/rt/sis/" + kks + "/history?compact=" + Convert.ToString(compact);
                string url = BaseUri2 + path;
                if (!compact)
                {
                    recv = GetEntityList<sis>(url);
                }
                else
                {
                    recv2 = GetSisData(url);
                    string strkks = recv2.schema.kks;
                    string strunit = recv2.schema.unit;
                    string field1 = recv2.schema.fields[0];
                    string field2 = recv2.schema.fields[1];
                    recv.total = recv2.total;
                    recv.msg = recv2.msg;

                    foreach (List<string> item in recv2.data)
                    {
                        string strVal1 = item[0];
                        string strVal2 = item[1];
                        sis si = new sis();
                        si.kks = strkks;
                        si.unit = strunit;
                        if (field1 == "t")
                        {
                            si.t = Convert.ToInt32(strVal1);
                            si.value = strVal2;
                        }
                        else
                        {
                            si.t = Convert.ToInt32(strVal2);
                            si.value = strVal1;
                        }

                        recv.data.Add(si);
                    }
                }

                if (recv.data == null)
                {
                    recv.data = new List<sis>();
                }

                foreach (sis item in recv.data)
                {
                    DevMonitorNode Dmn = bll.DevMonitorNodes.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();
                    if (Dmn == null)
                    {
                        continue;
                    }

                    DevMonitorNodeHistory Dmnh = Dmn.ToHistory();
                    Dmnh.Value = item.value;
                    Dmnh.Time = item.t + nEightHourSecond;
                    bll.DevMonitorNodeHistorys.Add(Dmnh);
                    
                    send.Add(Dmnh);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取SIS采样历史数据
        /// </summary>
        /// <param name="kks"></param>
        /// <returns></returns>
        public List<DevMonitorNodeHistory> GetSisSamplingHistoryList(string kks)
        {
            BaseTran<sis> recv = new BaseTran<sis>();
            SisData_Compact recv2 = new SisData_Compact();
            List<DevMonitorNodeHistory> send = new List<DevMonitorNodeHistory>();

            try
            {
                string[] sArray = BaseUri.Split(new string[] { "api" }, StringSplitOptions.RemoveEmptyEntries);
                string BaseUri2 = sArray[0];
                BaseUri2 = BaseUri2 + "api-viz/";
                string path = "rt/sis/" + kks + "/sample";
                string url = BaseUri2 + path;

                recv2 = GetSisData(url);
                string strkks = recv2.schema.kks;
                string strunit = recv2.schema.unit;
                string field1 = recv2.schema.fields[0];
                string field2 = recv2.schema.fields[1];
                recv.total = recv2.total;
                recv.msg = recv2.msg;

                foreach (List<string> item in recv2.data)
                {
                    string strVal1 = item[0];
                    string strVal2 = item[1];
                    sis si = new sis();
                    si.kks = strkks;
                    si.unit = strunit;
                    if (field1 == "t")
                    {
                        si.t = Convert.ToInt32(strVal1);
                        si.value = strVal2;
                    }
                    else
                    {
                        si.t = Convert.ToInt32(strVal2);
                        si.value = strVal1;
                    }

                    recv.data.Add(si);
                }

                if (recv.data == null)
                {
                    recv.data = new List<sis>();
                }

                foreach (sis item in recv.data)
                {
                    DevMonitorNode Dmn = bll.DevMonitorNodes.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();
                    if (Dmn == null)
                    {
                        continue;
                    }

                    DevMonitorNodeHistory Dmnh = Dmn.ToHistory();
                    Dmnh.Value = item.value;
                    Dmnh.Time = item.t + nEightHourSecond;
                    bll.DevMonitorNodeHistorys.Add(Dmnh);

                    send.Add(Dmnh);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取巡检轨迹列表
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="bFlag">值为True，按时间获取，值为false,获取全部</param>
        /// <returns></returns>
        public List<patrols> Getinspectionlist(long lBegin, long lEnd, bool bFlag)
        {
            return client.Getinspectionlist(lBegin, lEnd, bFlag);
        }

        /// <summary>
        /// 获取巡检节点列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public patrols Getcheckpoints(int patrolId)
        {
            return client.Getcheckpoints(patrolId);
        }

        /// <summary>
        /// 获取巡检结果列表
        /// </summary>
        /// <param name="patrolId"></param>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public checkpoints Getcheckresults(int patrolId, string deviceId)
        {
            return client.Getcheckresults(patrolId, deviceId);
        }

        private void WriteWrongInfo(List<string> lstInfo, string filename)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\" + filename;
            FileStream fs = System.IO.File.Create(filePath);
            //FileStream fs = new FileStream(filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            DateTime dt = DateTime.Now;
            int nCount = lstInfo.Count;
            string strInfo = "";
            for (int i = 0; i < nCount; ++i)
            {
                strInfo = lstInfo[i];
                strInfo += "      产生时间:" + dt;
                sw.WriteLine(strInfo);
            }

            sw.Close();
            fs.Close();
        }
    }
}
