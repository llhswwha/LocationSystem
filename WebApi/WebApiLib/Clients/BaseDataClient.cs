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

namespace WebApiLib.Clients
{
    public class BaseDataClient
    {
        private Bll bll = Bll.Instance();

        private string Message;

        public string BaseUri { get; set; }

        public static int nEightHourSecond = 0;

        public BaseDataClient()
        {
            BaseUri = "http://<host>:<port>/";
        }

        public BaseDataClient(string host, string port)
        {
            //BaseUri = string.Format("http://{0}:{1}/", host, port);
            BaseUri = string.Format("http://{0}/{1}/", host, port);
        }

        public BaseDataClient(string baseUri)
        {
            BaseUri = baseUri;
        }

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

        public List<T> GetEntityList2<T>(string url)
        {
            var recv = new List<T>();
            try
            {
                recv = WebApiHelper.GetEntity<List<T>>(url);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return null;
            }
            return recv;
        }

        public patrols GetEntityList3(string url)
        {
            patrols recv = new patrols();
            try
            {
                recv = WebApiHelper.GetEntity<patrols>(url);
                if (recv == null) return null;
                if (recv.route == null)
                {
                    recv.route = new List<checkpoints>();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return null;
            }
            return recv;

        }

        public checkpoints GetEntityList4(string url)
        {
            checkpoints recv = new checkpoints();
            try
            {
                recv = WebApiHelper.GetEntity<checkpoints>(url);
                if (recv == null) return null;
                if (recv.checks == null)
                {
                    recv.checks = new List<results>();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return null;
            }
            return recv;

        }

        public BaseTran_Compact GetEntityList5(string url)
        {
            var recv = new BaseTran_Compact();
            try
            {
                recv = WebApiHelper.GetEntity<BaseTran_Compact>(url);
                if (recv == null) return null;
                if (recv.schema == null)
                {
                    recv.schema = new sis_compact();
                }

                if (recv.data == null)
                {
                    recv.data = new List<List<string>>();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return null;
            }
            return recv;
        }

        public T GetEntityDetail<T>(string url)
        {
            T recv = default(T);
            try
            {
                recv = WebApiHelper.GetEntity<T>(url);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return default(T);
            }
            return recv;
        }

        /// <summary>
        /// 获取人员列表
        /// </summary>
        /// <returns></returns>
        public List<Personnel> GetUserList()
        {
            Bll bll = new Bll(false,false,false,false);
            List<Personnel> plst = bll.Personnels.ToList();
            List<Department> dlst = bll.Departments.ToList();

            BaseTran<user> recv = new BaseTran<user>();
            List<Personnel> send = new List<Personnel>();
            List<string> errorlst = new List<string>();

            try
            {
                string path = "users";
                string url = BaseUri + path;
                recv = GetEntityList<user>(url);

                foreach (user item in recv.data)
                {
                    //0表示添加，1表示修改
                    int nFlag = 1;

                    //先根据人员Id获取
                    Personnel Personnel = plst.Find(p => p.Abutment_Id == item.id);
                    if (Personnel == null)
                    {
                        Personnel = new Personnel();
                        Personnel.Pst = "检修";
                        nFlag = 0;
                    }

                    Sexs nSex = Sexs.未知;
                    if (item.gender != null)
                    {
                        nSex = (Sexs)item.gender;
                    }

                    Personnel.Abutment_Id = item.id;
                    Personnel.Name = item.name;
                    Personnel.Sex = nSex;
                    Personnel.Email = item.email;
                    Personnel.Phone = item.phone;
                    Personnel.Mobile = item.mobile;
                    Personnel.Enabled = item.enabled;

                    if (item.dept_name == null)
                    {
                        Department Department = dlst.Find(p => p.Name == "未绑定");
                        Personnel.ParentId = Department.Id;
                    }
                    else
                    {
                        Department Department = dlst.Find(p => p.Name == item.dept_name);
                        if (Department == null)
                        {
                            string strInfo = "获取人员列表错误信息：找不到匹配的部门  id=" + item.id + "  Name=" + item.name + " Depart=" + item.dept_name;
                            errorlst.Add(strInfo);
                            continue;
                        }

                        Personnel.ParentId = Department.Id;
                    }

                    if (nFlag == 0)
                    {
                        bll.Personnels.Add(Personnel);
                        plst.Add(Personnel);
                    }
                    else
                    {
                        bll.Personnels.Edit(Personnel);
                    }

                    send.Add(Personnel);
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

            return send;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<Department> GetorgList()
        {
            Bll bll = new Bll(false, false, false, false);
            List<Department> dlst = bll.Departments.ToList();

            BaseTran<org> recv = new BaseTran<org>();
            List<Department> send = new List<Department>();

            try
            {
                string path = "orgs";
                string url = BaseUri + path;
                recv = GetEntityList<org>(url);

                if (recv.data == null)
                {
                    recv.data = new List<org>();
                }

                foreach (org item in recv.data)
                {
                    //0表示添加，1表示修改
                    int nFlag = 1;

                    Department Dpt = dlst.Find(p => p.Abutment_Id == item.id);
                    if (Dpt == null)
                    {
                        Dpt = new Department();
                        Dpt.ShowOrder = 0;
                        nFlag = 0;
                    }

                    Dpt.Abutment_Id = item.id;
                    Dpt.Name = item.name;
                    Dpt.Abutment_ParentId = item.parentId;
                    Dpt.Type = (DepartType)item.type;
                    Dpt.Description = item.description;

                    if (nFlag == 0)
                    {
                        bll.Departments.Add(Dpt);
                        dlst.Add(Dpt);
                    }
                    else
                    {
                        bll.Departments.Edit(Dpt);
                    }

                    send.Add(Dpt);
                }

                foreach (Department item2 in send)
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
                    bll.Departments.Edit(item2);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        public List<Area> GetzonesList()
        {
            Bll bll = new Bll();
            List<Area> alst = bll.Areas.ToList();

            BaseTran<zone> recv = new BaseTran<zone>();
            List<Area> send = new List<Area>();
            List<string> errorlst = new List<string>();


            try
            {
                string path = "zones?struct=LIST";
                string url = BaseUri + path;
                recv = GetEntityList<zone>(url);

                if (recv.data == null)
                {
                    recv.data = new List<zone>();
                }

                foreach (zone item in recv.data)
                {
                    if (item.kks == null)
                    {
                        string strInfo = "获取获取区域列表错误信息：KKS为null  id=" + item.id + "  Name=" + item.name + " KKS=" + item.kks;
                        errorlst.Add(strInfo);
                        continue;
                    }

                    Area area = alst.Find(p=>p.KKS == item.kks);
                    if (area == null)
                    {
                        string strInfo = "获取获取区域列表错误信息：根据KKS找不到对应的区域 id=" + item.id + "  Name=" + item.name + " KKS=" + item.kks;
                        errorlst.Add(strInfo);
                        continue;
                    }

                    area.Abutment_Id = item.id;
                    area.Abutment_ParentId = item.parent_Id;
                    send.Add(area);
                }

                foreach (Area item2 in send)
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



            return send;
        }

        /// <summary>
        /// 获取单个区域信息
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="view">视图掩码，要附加内容时将掩码加上，1附加子区域，2附加关联设备，3附加(危险)物资，如返回字区域，则view=1+2,即view=3</param>
        /// <returns></returns>
        public Area GetSingleZonesInfo(int id, int view)
        {
            zone recv = new zone();
            Area send = new Area();

            return send;
            string strId = Convert.ToString(id);
            string strView = Convert.ToString(view);
            try
            {
                string path = "zones/" + strId + "?view=" + strView;
                string url = BaseUri + path;
                recv = GetEntityDetail<zone>(url);

                if (recv == null)
                {
                    recv = new zone();
                }

                if (recv.zones == null)
                {
                    recv.zones = new List<zone>();
                }

                if (recv.devices == null)
                {
                    recv.devices = new List<device>();
                }

                Area area = bll.Areas.DbSet.Where(p => p.Abutment_Id == recv.id).FirstOrDefault();

                int nFlag = 0;
                if (area == null)
                {
                    area = new Area();
                    area.IsRelative = false;
                    area.Type = 0;
                    nFlag = 1;
                }

                area.Abutment_Id = recv.id;
                area.Name = recv.name;
                area.KKS = recv.kks;
                area.Abutment_ParentId = recv.parent_Id;
                area.Describe = recv.description;

                if (recv.parent_Id != null)
                {
                    Area Parent = bll.Areas.DbSet.Where(p => p.Abutment_Id == recv.parent_Id).FirstOrDefault();
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

                foreach (zone item2 in recv.zones)
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

                foreach (device item3 in recv.devices)
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
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DevInfo> GetZoneDevList(int id)
        {
            BaseTran<device> recv = new BaseTran<device>();
            string strId = Convert.ToString(id);
            List<DevInfo> send = new List<DevInfo>();

            return send;
            try
            {
                string path = "api/zones/" + strId + "/devices";
                string url = BaseUri + path;
                recv = GetEntityList<device>(url);

                if (recv.data == null)
                {
                    recv.data = new List<device>();
                }

                Area area = bll.Areas.DbSet.Where(p => p.Abutment_Id == id).FirstOrDefault();
                if (area != null)
                {
                    foreach (device item in recv.data)
                    {
                        DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();
                        int nFlag = 0;
                        if (devinfo == null)
                        {
                            devinfo = new DevInfo();
                            nFlag = 1;
                        }

                        devinfo.ParentId = area.Id;
                        devinfo.Abutment_Id = item.id;
                        devinfo.Code = item.code;
                        devinfo.KKS = item.kks;
                        devinfo.Name = item.name;
                        devinfo.Abutment_Type = (Abutment_DevTypes)item.type;
                        devinfo.Status = (Abutment_Status)item.state;
                        devinfo.RunStatus = (Abutment_RunStatus)item.running_state;
                        devinfo.Placed = item.placed;
                        devinfo.Abutment_DevID = item.raw_id;
                        devinfo.IP = item.ip;
                        devinfo.Manufactor = "霍尼韦尔";

                        if (nFlag == 1)
                        {
                            devinfo.CreateTime = DateTime.Now;
                            devinfo.ModifyTime = DateTime.Now;

                            devinfo.CreateTimeStamp = TimeConvert.DateTimeToTimeStamp(devinfo.CreateTime);
                            devinfo.ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(devinfo.ModifyTime);

                            bll.DevInfos.Add(devinfo);
                        }
                        else
                        {
                            devinfo.ModifyTime = DateTime.Now;
                            devinfo.ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(devinfo.ModifyTime);

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
        /// <param name="types">设备类型，逗号分隔，只查询指定的设备类型</param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<DevInfo> GetDeviceList(string types, string code, string name)
        {
            Bll bll = new Bll();
            List<DevInfo> dlst = bll.DevInfos.ToList();
            List<Dev_CameraInfo> dclst = bll.Dev_CameraInfos.ToList();
            BaseTran<device> recv = new BaseTran<device>();
            List<DevInfo> send = new List<DevInfo>();
            List<string> errorLst = new List<string>();

            try
            {
                string path = "devices";
                string url = BaseUri + path;

                if (types != null)
                {
                    url += "?types=" + types;
                }
                else
                {
                    url += "?types";
                }

                if (code != null)
                {
                    url += "&code=" + code;
                }
                else
                {
                    url += "&code";
                }

                if (name != null)
                {
                    url += "&name=" + name;
                }
                else
                {
                    url += "&name";
                }

                recv = GetEntityList<device>(url);

                if (recv.data == null)
                {
                    recv.data = new List<device>();
                }

                foreach (device item in recv.data)
                {
                    if (item.type != 102 && item.type != 1021 && item.type != 1022 && item.type != 1023)
                    {
                        string strInfo = "获取设备列表错误信息：不是摄像头  id=" + item.id + "  Name=" + item.name + "  Type=" + Convert.ToString(item.type) + "  Code=" + item.code + " KKS=" + item.kks;
                        errorLst.Add(strInfo);
                        continue;
                    }

                    if (item.ip == null || item.ip == "")
                    {
                        string strInfo = "获取设备列表错误信息：IP为空  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        errorLst.Add(strInfo);
                        continue;
                    }

                    Dev_CameraInfo dc = dclst.Find(p => p.Ip == item.ip);
                    if (dc == null)
                    {
                        string strInfo = "获取设备列表错误信息：找不到匹配的IP  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        errorLst.Add(strInfo);
                        continue;
                    }

                    DevInfo devinfo = dlst.Find(p => p.Id == dc.DevInfoId);
                    if (devinfo == null)
                    {
                        string strInfo = "获取设备列表错误信息：摄像头找不到匹配的设备  id=" + item.id + "  Name=" + item.name + "  Code=" + item.code + " KKS=" + item.kks;
                        errorLst.Add(strInfo);
                        continue;
                    }

                    devinfo.Abutment_Id = item.id;
                    devinfo.Code = item.code;
                    devinfo.Abutment_Type = (Abutment_DevTypes)item.type;
                    devinfo.Status = (Abutment_Status)item.state;
                    devinfo.RunStatus = (Abutment_RunStatus)item.running_state;
                    devinfo.Placed = item.placed;
                    devinfo.Abutment_DevID = item.raw_id;
                    devinfo.IP = item.ip;
                    devinfo.Manufactor = "霍尼韦尔";

                    devinfo.ModifyTime = DateTime.Now;
                    devinfo.ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(devinfo.ModifyTime);

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
        /// 获取单个设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DevInfo GetSingleDeviceInfo(int id)
        {
            device recv = new device();
            string strId = Convert.ToString(id);
            DevInfo send = new DevInfo();

            try
            {
                string path = "api/devices/" + strId;
                string url = BaseUri + path;
                recv = GetEntityDetail<device>(url);

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

                    devinfo.CreateTimeStamp = TimeConvert.DateTimeToTimeStamp(devinfo.CreateTime);
                    devinfo.ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(devinfo.ModifyTime);

                    bll.DevInfos.Add(devinfo);
                }
                else
                {
                    devinfo.ModifyTime = DateTime.Now;
                    devinfo.ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(devinfo.ModifyTime);

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
        public List<DevEntranceGuardCardAction> GetSingleDeviceActionHistory(int id, string begin_date, string end_date)
        {
            BaseTran<devices_actions> recv = new BaseTran<devices_actions>();
            List<DevEntranceGuardCardAction> send = new List<DevEntranceGuardCardAction>();

            string strId = Convert.ToString(id);

            string path = "api/devices/" + strId + "/actions";
            string url = BaseUri + path;
            if (begin_date != null)
            {
                url += "?begin_date=" + begin_date;
            }
            else
            {
                url += "?begin_date";
            }

            if (end_date != null)
            {
                url += "&end_date=" + end_date;
            }
            else
            {
                url += "&end_date";
            }

            recv = GetEntityList<devices_actions>(url);

            if (recv.data == null)
            {
                recv.data = new List<devices_actions>();
            }

            DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.Abutment_Id == id).FirstOrDefault();
            if (devinfo != null)
            {
                foreach (devices_actions item in recv.data)
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
                        degca.OperateTime = TimeConvert.TimeStampToDateTime(t);

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
        public List<EntranceGuardCard> GetCardList()
        {
            Bll bll = new Bll();
            List<EntranceGuardCard> elst = bll.EntranceGuardCards.ToList();
            List<Personnel> plst = bll.Personnels.ToList();
            List<EntranceGuardCardToPersonnel> eglst = bll.EntranceGuardCardToPersonnels.ToList();
            BaseTran<cards> recv = new BaseTran<cards>();
            List<EntranceGuardCard> send = new List<EntranceGuardCard>();

            try
            {
                string path = "cards";
                string url = BaseUri + path;
                recv = GetEntityList<cards>(url);
                foreach (cards item in recv.data)
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
                        bll.EntranceGuardCards.Add(egc);
                        elst.Add(egc);
                    }
                    else
                    {
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
                        bll.EntranceGuardCardToPersonnels.DeleteById(egctp1.Id);
                    }

                    if (egctp2 != null)
                    {
                        eglst.Remove(egctp2);
                        bll.EntranceGuardCardToPersonnels.DeleteById(egctp2.Id);
                    }

                    egctp = new EntranceGuardCardToPersonnel();
                    egctp.PersonnelId = personnel.Id;
                    egctp.EntranceGuardCardId = egc.Id;
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
        public List<TModel.LocationHistory.AreaAndDev.EntranceGuardActionInfo> GetSingleCardActionHistory(int id, string begin_date, string end_date)
        {
            Bll bll = new Bll();
            List<EntranceGuardCard> elst = bll.EntranceGuardCards.ToList();
            List<DevEntranceGuardCardAction> delst = bll.DevEntranceGuardCardActions.ToList();
            List<DevInfo> dlst = bll.DevInfos.ToList();
            BaseTran<cards_actions> recv = new BaseTran<cards_actions>();
            //List<DevEntranceGuardCardAction> send = new List<DevEntranceGuardCardAction>();
            List<TModel.LocationHistory.AreaAndDev.EntranceGuardActionInfo> send = new List<TModel.LocationHistory.AreaAndDev.EntranceGuardActionInfo>();

            try
            {
                string path = "cards/" + Convert.ToString(id) + "/actions";
                string url = BaseUri + path;
                if (begin_date != null)
                {
                    url += "?begin_date=" + begin_date;
                }
                else
                {
                    url += "?begin_date";
                }

                if (end_date != null)
                {
                    url += "&end_date=" + end_date;
                }
                else
                {
                    url += "&end_date";
                }

                recv = GetEntityList<cards_actions>(url);

                if (recv.data == null)
                {
                    recv.data = new List<cards_actions>();
                }

                EntranceGuardCard egc = elst.Find(p => p.Abutment_Id == id);
                if (egc == null)
                {
                    return send;
                }

                foreach (cards_actions item in recv.data)
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
                            degca.OperateTime = TimeConvert.TimeStampToDateTime(t);
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
                string path = "api/tickets";
                string url = BaseUri + path;
                QueryArg query = new QueryArg();
                query.Add("type", type);
                query.Add("begin_date", begin_date);
                query.Add("end_date", end_date);
                url += query.GetQueryString();
                recv = GetEntityList<tickets>(url);

                if (recv.data == null)
                {
                    recv.data = new List<tickets>();
                }

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
            tickets recv = new tickets();

            try
            {
                string path = "api/tickets/" + id;
                string url = BaseUri + path;
                if (begin_date != null)
                {
                    url += "?begin_date=" + begin_date;
                }
                else
                {
                    url += "?begin_date";
                }

                if (end_date != null)
                {
                    url += "&end_date=" + end_date;
                }
                else
                {
                    url += "&end_date";
                }

                return GetEntityDetail<tickets>(url);
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;
        }

        /// <summary>
        /// 获取告警事件列表
        /// </summary>
        /// <param name="src"></param>
        /// <param name="level"></param>
        /// <param name="begin_t"></param>
        /// <param name="end_t"></param>
        /// <returns></returns>
        public List<DevAlarm> GeteventsList(int? src, int? level, long? begin_t, long? end_t)
        {
            Bll bll = new Bll();
            List<DevInfo> DevList = bll.DevInfos.ToList();
            List<DevAlarm> DaList = bll.DevAlarms.Where(p => p.Src == Abutment_DevAlarmSrc.视频监控 || p.Src == Abutment_DevAlarmSrc.门禁 || p.Src == Abutment_DevAlarmSrc.消防).ToList();
            BaseTran<events> recv = new BaseTran<events>();
            List<DevAlarm> send = new List<DevAlarm>();

            try
            {
                string path = "events";
                string url = BaseUri + path;
                if (src != null)
                {
                    url += "?src=" + Convert.ToString(src);
                }
                else
                {
                    url += "?src";
                }

                if (level != null)
                {
                    url += "&level=" + Convert.ToString(level);
                }
                else
                {
                    url += "&level";
                }

                if (begin_t != null && end_t != null)
                {
                    url += "&begin_t=" + Convert.ToString(begin_t) + "&end_t=" + Convert.ToString(end_t);
                }
                else
                {
                    url += "&begin_t" + "&end_t";
                }

                recv = GetEntityList<events>(url);

                if (recv.data == null)
                {
                    recv.data = new List<events>();
                }

                foreach (events item in recv.data)
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
                            da.AlarmTime = TimeConvert.TimeStampToDateTime(lTimeStamp);
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
        /// <param name="kks"></param>
        /// <returns></returns>
        public List<DevMonitorNode> GetSomesisList(string strTags)
        {
            BaseTran<sis> recv = new BaseTran<sis>();
            List<DevMonitorNode> send = new List<DevMonitorNode>();

            try
            {
                //string path = "api/rt/sis?kks=" + kks;
                //string url = BaseUri + path;
                //string[] sArray = BaseUri.Split(new string[] { "api" }, StringSplitOptions.RemoveEmptyEntries);
                //string BaseUri2 = sArray[0];
               // BaseUri2 += "api-viz/";
                string url = BaseUri + "rt/sis/" + strTags;
                recv = GetEntityList<sis>(url);

                if (recv.data == null)
                {
                    recv.data = new List<sis>();
                }

                foreach (sis item in recv.data)
                {
                    string strTag = item.kks;

                    //DevMonitorNode Dmn = bll.DevMonitorNodes.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();
                    DevMonitorNode Dmn = bll.DevMonitorNodes.DbSet.Where(p => p.TagName == strTag).FirstOrDefault();
                    if (Dmn == null)
                    {
                        continue;
                    }

                    Dmn.Value = item.value;
                    Dmn.Time = item.t + nEightHourSecond;
                    bll.DevMonitorNodes.Edit(Dmn);
                    DevMonitorNodeHistory Dmnh = Dmn.ToHistory();
                    bll.DevMonitorNodeHistorys.Add(Dmnh);

                    send.Add(Dmn);
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return send;
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
            BaseTran_Compact recv2 = new BaseTran_Compact();
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
                    recv2 = GetEntityList5(url);
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
            BaseTran_Compact recv2 = new BaseTran_Compact();
            List<DevMonitorNodeHistory> send = new List<DevMonitorNodeHistory>();

            try
            {
                string[] sArray = BaseUri.Split(new string[] { "api" }, StringSplitOptions.RemoveEmptyEntries);
                string BaseUri2 = sArray[0];
                BaseUri2 = BaseUri2 + "api-viz/";
                string path = "rt/sis/" + kks + "/sample";
                string url = BaseUri2 + path;

                recv2 = GetEntityList5(url);
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
            List<patrols> recv = new List<patrols>();

            try
            {
                string path = "patrols";
                string url = BaseUri + path;
                if (bFlag)
                {
                    url += "?startDate=" + Convert.ToString(lBegin);
                    url += "&endDate=" + Convert.ToString(lEnd);
                }

                recv = GetEntityList2<patrols>(url);

                //if (recv == null || recv.Count == 0)
                //{
                //    return recv;
                //}

                //foreach (patrols item in recv)
                //{
                //    InspectionTrack it = itlst.Find(p=>p.Abutment_Id == item.id);
                //    if (it != null)
                //    {
                //        send.Add(it);
                //        continue;
                //    }

                //    it = new InspectionTrack();
                //    it.Abutment_Id = item.id;
                //    it.Code = item.code;
                //    it.Name = item.name;
                //    it.CreateTime = item.createTime;
                //    it.StartTime = item.startTime;
                //    it.EndTime = item.endTime;
                //    it.dtCreateTime = TimeConvert.TimeStampToDateTime(1000* item.createTime);
                //    it.dtStartTime = TimeConvert.TimeStampToDateTime(1000* item.startTime);
                //    it.dtEndTime = TimeConvert.TimeStampToDateTime(1000* item.endTime);
                //    bll.InspectionTracks.Add(it);
                //    send.Add(it);
                //}
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;

        }

        /// <summary>
        /// 获取巡检节点列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public patrols Getcheckpoints(int patrolId)
        {
            patrols recv = new patrols();

            try
            {
                string path = "patrols/" + Convert.ToString(patrolId);
                string url = BaseUri + path;

                recv = GetEntityList3(url);
                
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;

        }

        /// <summary>
        /// 获取巡检结果列表
        /// </summary>
        /// <param name="patrolId"></param>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public checkpoints Getcheckresults(int patrolId, string deviceId)
        {
            checkpoints recv = new checkpoints();

            try
            {
                string path = "patrols/" + Convert.ToString(patrolId) + "/checkpoints/" + deviceId + "/results";
                string url = BaseUri + path;

                recv = GetEntityList4(url);
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;

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
