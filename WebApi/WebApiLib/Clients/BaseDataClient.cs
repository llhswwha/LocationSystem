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

namespace WebApiLib.Clients
{
    public class BaseDataClient
    {
        private Bll bll = Bll.Instance();

        private string Message;

        public string BaseUri { get; set; }

        public static int nEightHourSecond = 28800;

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
            BaseTran<user> recv = new BaseTran<user>();
            List<Personnel> send = new List<Personnel>();

            try
            {
                string path = "api/users";
                string url = BaseUri + path;
                recv = GetEntityList<user>(url);

                foreach (user item in recv.data)
                {
                    List<Personnel> PeList = bll.Personnels.DbSet.Where(p => p.Name == item.name).ToList();
                    Personnel Personnel = null;
                    int nFlag = 0;
                    int nCount = 0;
                    if (PeList.Count == 0)
                    {
                        Personnel = new Personnel();
                        Personnel.ParentId = null;
                        nFlag = 0;
                    }
                    else
                    {
                        foreach (Personnel item2 in PeList)
                        {
                            if (item2.Abutment_Id == item.id)
                            {
                                nFlag = 1;
                                Personnel = item2;
                                break;
                            }
                            else
                            {
                                nFlag = 2;
                                Personnel = item2;
                                nCount++;
                            }
                        }
                    }

                    if (nFlag == 2 && nCount > 1)
                    {
                        FileStream fs = System.IO.File.Create(@"a.txt");
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine("id=" + item.id + "  Name=" + item.name + " Depart=" + item.dept_name);
                        sw.Close();
                        fs.Close();
                        continue;
                    }

                    Personnel.Abutment_Id = item.id;
                    Personnel.Name = item.name;
                    Personnel.Sex = (Sexs)item.gender;
                    Personnel.Email = item.email;
                    Personnel.Phone = item.phone;
                    Personnel.Mobile = item.mobile;
                    Personnel.Enabled = item.enabled;
                    if (item.dept_name != null)
                    {
                        Department Department = bll.Departments.DbSet.Where(p => p.Name == item.dept_name).FirstOrDefault();
                        if (Department != null)
                        {
                            Personnel.ParentId = Department.Id;
                        }
                    }

                    if (nFlag == 0)
                    {
                        bll.Personnels.Add(Personnel);
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

            return send;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<Department> GetorgList()
        {
            BaseTran<org> recv = new BaseTran<org>();
            List<Department> send = new List<Department>();

            try
            {
                string path = "api/orgs";
                string url = BaseUri + path;
                recv = GetEntityList<org>(url);

                if (recv.data == null)
                {
                    recv.data = new List<org>();
                }

                foreach (org item in recv.data)
                {
                    var dp = bll.Departments.DbSet.Where(p => p.Abutment_Id == item.id && p.Name == item.name).FirstOrDefault();
                    int nFlag = 0;
                    if (dp == null)
                    {
                        dp = new Department();
                        dp.ParentId = null;
                        dp.ShowOrder = 0;
                        nFlag = 1;
                    }

                    dp.Abutment_Id = item.id;
                    dp.Name = item.name;
                    dp.Type = (DepartType)item.type;
                    dp.Description = item.description;

                    if (nFlag == 1)
                    {
                        bll.Departments.Add(dp);
                    }
                    else
                    {
                        bll.Departments.Edit(dp);
                    }
                }

                foreach (org item in recv.data)
                {
                    Department dp = bll.Departments.DbSet.Where(p => p.Abutment_Id == item.id && p.Name == item.name).FirstOrDefault();
                    Department Parent = bll.Departments.DbSet.Where(p => p.Abutment_Id == item.parentId).FirstOrDefault();
                    if (dp == null )
                    {
                        continue;
                    }

                    if (Parent == null)
                    {
                        send.Add(dp);
                        continue;
                    }

                    if (dp.Abutment_Id == 0 && Parent.Abutment_Id == 0)
                    {
                        continue;
                    }

                    dp.ParentId = Parent.Id;
                    bll.Departments.Edit(dp);
                    send.Add(dp);
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
            BaseTran<zone> recv = new BaseTran<zone>();
            List<Area> send = new List<Area>();

            try
            {
                string path = "api/zones?struct=LIST";
                string url = BaseUri + path;
                recv = GetEntityList<zone>(url);

                if (recv.data == null)
                {
                    recv.data = new List<zone>();
                }

                foreach (zone item in recv.data)
                {
                    Area area = bll.Areas.DbSet.Where(p => p.Abutment_Id == item.id).FirstOrDefault();
                    int nFlag = 0;
                    if (area == null)
                    {
                        area = new Area();
                        area.Abutment_Id = item.id;
                        area.IsRelative = false;
                        area.Type = 0;
                        nFlag = 1;
                    }

                    area.Name = item.name;
                    area.KKS = item.kks;
                    area.Abutment_ParentId = item.parent_Id;
                    area.Describe = item.description;

                    if (nFlag == 1)
                    {
                        bll.Areas.Add(area);
                    }
                    else
                    {
                        bll.Areas.Edit(area);
                    }
                }

                foreach (zone item in recv.data)
                {
                    if (item.parent_Id == null)
                    {
                        continue;
                    }

                    Area Child = bll.Areas.DbSet.Where(p => p.Abutment_Id == item.id).FirstOrDefault();
                    Area Parent = bll.Areas.DbSet.Where(p => p.Abutment_Id == item.parent_Id).FirstOrDefault();

                    if (Child != null)
                    {
                        if (Parent != null)
                        {
                            Child.ParentId = Parent.Id;
                            bll.Areas.Edit(Child);
                        }

                        send.Add(Child);
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
        /// 获取单个区域信息
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="view">视图掩码，要附加内容时将掩码加上，1附加子区域，2附加关联设备，3附加(危险)物资，如返回字区域，则view=1+2,即view=3</param>
        /// <returns></returns>
        public Area GetSingleZonesInfo(int id, int view)
        {
            zone recv = new zone();
            Area send = new Area();

            string strId = Convert.ToString(id);
            string strView = Convert.ToString(view);
            try
            {
                string path = "api/zones/" + strId + "?view=" + strView;
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
            BaseTran<device> recv = new BaseTran<device>();
            List<DevInfo> send = new List<DevInfo>();

            try
            {
                string path = "api/devices";
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
                    DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();
                    int nFlag = 0;
                    if (devinfo == null)
                    {
                        devinfo = new DevInfo();
                        nFlag = 1;
                    }

                    devinfo.Abutment_Id = item.id;
                    devinfo.Code = item.code;
                    devinfo.KKS = item.kks;
                    devinfo.Name = item.name;
                    devinfo.Abutment_Type = (Abutment_DevTypes)item.type;
                    devinfo.Status = (Abutment_Status)item.state;
                    devinfo.RunStatus = (Abutment_RunStatus)item.running_state;
                    devinfo.Placed = item.placed;
                    devinfo.Abutment_DevID = item.raw_id;
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
            catch (Exception ex)
            {
                string messgae = ex.Message;
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
            BaseTran<cards> recv = new BaseTran<cards>();
            List<EntranceGuardCard> send = new List<EntranceGuardCard>();

            try
            {
                string path = "api/cards";
                string url = BaseUri + path;
                recv = GetEntityList<cards>(url);
                foreach (cards item in recv.data)
                {
                    EntranceGuardCard egc = bll.EntranceGuardCards.DbSet.FirstOrDefault(p => p.Abutment_Id == item.cardId);
                    int nFlag = 0;
                    if (egc == null)
                    {
                        egc = new EntranceGuardCard();
                        nFlag = 1;
                    }

                    egc.Abutment_Id = item.cardId;
                    egc.Code = item.cardCode;
                    egc.State = item.state;

                    if (nFlag == 1)
                    {
                        bll.EntranceGuardCards.Add(egc);
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

                    Personnel personnel = bll.Personnels.DbSet.FirstOrDefault(p => p.Abutment_Id == item.emp_id);
                    if (personnel == null)
                    {
                        continue;
                    }

                    EntranceGuardCardToPersonnel egctp = bll.EntranceGuardCardToPersonnels.DbSet.FirstOrDefault(p => p.EntranceGuardCardId == egc.Id);
                    nFlag = 0;
                    if (egctp == null)
                    {
                        egctp = new EntranceGuardCardToPersonnel();
                        egctp.EntranceGuardCardId = egc.Id;
                        nFlag = 1;
                    }

                    egctp.PersonnelId = personnel.Id;

                    if (nFlag == 1)
                    {
                        bll.EntranceGuardCardToPersonnels.Add(egctp);
                    }
                    else
                    {
                        bll.EntranceGuardCardToPersonnels.Edit(egctp);
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
        /// 获取门禁卡操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public List<DevEntranceGuardCardAction> GetSingleCardActionHistory(int id, string begin_date, string end_date)
        {
            BaseTran<cards_actions> recv = new BaseTran<cards_actions>();
            List<DevEntranceGuardCardAction> send = new List<DevEntranceGuardCardAction>();

            try
            {
                string path = "api/cards/" + Convert.ToString(id) + "/actions";
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

                EntranceGuardCard egc = bll.EntranceGuardCards.DbSet.FirstOrDefault(p => p.Abutment_Id == id);

                if (egc != null)
                {
                    foreach (cards_actions item in recv.data)
                    {
                        DevEntranceGuardCardAction degca = bll.DevEntranceGuardCardActions.DbSet.FirstOrDefault(p => p.Abutment_Id == item.id);
                        DevInfo devinfo = bll.DevInfos.DbSet.FirstOrDefault(p => p.Abutment_Id == item.device_id);
                        if (devinfo == null)
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
            BaseTran<events> recv = new BaseTran<events>();
            List<DevAlarm> send = new List<DevAlarm>();

            try
            {
                string path = "api/events";
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
                    if (item.deviceId == null)
                    {
                        continue;
                    }

                    DevInfo di = bll.DevInfos.DbSet.Where(p => p.Abutment_Id == item.deviceId).FirstOrDefault();
                    if (di == null)
                    {
                        continue;
                    }

                    DevAlarm da = bll.DevAlarms.DbSet.Where(p => p.Abutment_Id == item.id).FirstOrDefault();
                    int nFlag = 0;
                    if (da == null)
                    {
                        da = new DevAlarm();
                        nFlag = 1;
                    }

                    da.Abutment_Id = item.id;
                    da.Title = item.title;
                    da.Msg = item.msg;
                    da.Level = (Abutment_DevAlarmLevel)item.level;
                    da.Code = item.code;
                    da.Src = (Abutment_DevAlarmSrc)item.src;
                    da.DevInfoId = di.Id;
                    da.Device_desc = item.deviceDesc;
                    da.AlarmTime = TimeConvert.TimeStampToDateTime(item.t);
                    da.AlarmTimeStamp = item.t;

                    if (nFlag == 1)
                    {
                        bll.DevAlarms.Add(da);
                    }
                    else
                    {
                        bll.DevAlarms.Edit(da);
                    }

                    send.Add(da);
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
                string[] sArray = BaseUri.Split(new string[] { "api" }, StringSplitOptions.RemoveEmptyEntries);
                string BaseUri2 = sArray[0];
               // BaseUri2 += "api-viz/";
                string url = BaseUri2 + "rt/sis/" + strTags;
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

        //public List<InspectionTrack> Getinspectionlist(DateTime dtTime, bool bFlag)
        //{
        //    List<patrols> recv = new List<patrols>();
        //    List<InspectionTrack> send = new List<InspectionTrack>();

        //    try
        //    {
        //        string path = "api/patrols";
        //        string url = BaseUri + path;
        //        if (bFlag)
        //        {
        //            DateTime dtEnd = dtTime.AddHours(-8);
        //            DateTime dtBegin = dtEnd.AddDays(-1);

        //            url += "?startDate=" + Convert.ToString(dtBegin);
        //            url += "&endDate=" + Convert.ToString(dtEnd);
        //        }

        //        recv = GetEntityList2<patrols>(url);

        //        foreach (patrols item in recv)
        //        {
        //            InspectionTrack now = bll.InspectionTracks.Find(p => p.Abutment_Id == item.id);
        //            InspectionTrackHistory history = bll.InspectionTrackHistorys.Find(p => p.Abutment_Id == item.id);

        //            InspectionStatus state =(InspectionStatus)Convert.ToInt32(item.state);
        //            int nFlag = 0;
        //            int nId = 0;

        //            if (state == InspectionStatus.NewBuild || state == InspectionStatus.AlreadyIssued || state == InspectionStatus.InExecution)
        //            {
        //                if (now == null)
        //                {
        //                    now = new InspectionTrack();

        //                    now.Abutment_Id = item.id;
        //                    now.Code = item.code;
        //                    now.Name = item.name;
        //                    now.CreateTime = (item.createTime + nEightHourSecond) * 1000;
        //                    now.dtCreateTime = TimeConvert.TimeStampToDateTime(now.CreateTime);
        //                    now.State = state;
        //                    now.StartTime = (item.startTime + nEightHourSecond) * 1000;
        //                    now.dtStartTime = TimeConvert.TimeStampToDateTime(now.StartTime);
        //                    now.EndTime = (item.endTime + nEightHourSecond) * 1000;
        //                    now.dtEndTime = TimeConvert.TimeStampToDateTime(now.EndTime);
        //                    bll.InspectionTracks.Add(now);
        //                }
        //                else
        //                {
        //                    now.State = state;
        //                    bll.InspectionTracks.Edit(now);
        //                }

        //                nId = now.Id;
        //            }
        //            else
        //            {
        //                if (now != null)
        //                {
        //                    bll.InspectionTracks.DeleteById(now.Id);
        //                }

        //                if (history != null)
        //                {
        //                    nFlag = 1;
        //                }
        //                else
        //                {
        //                    history = new InspectionTrackHistory();

        //                    history.Abutment_Id = item.id;
        //                    history.Code = item.code;
        //                    history.Name = item.name;
        //                    history.CreateTime = (item.createTime + nEightHourSecond) * 1000;
        //                    history.dtCreateTime = TimeConvert.TimeStampToDateTime(now.CreateTime);
        //                    history.State = state;
        //                    history.StartTime = (item.startTime + nEightHourSecond) * 1000;
        //                    history.dtStartTime = TimeConvert.TimeStampToDateTime(now.StartTime);
        //                    history.EndTime = (item.endTime + nEightHourSecond) * 1000;
        //                    history.dtEndTime = TimeConvert.TimeStampToDateTime(now.EndTime);

        //                    bll.InspectionTrackHistorys.Add(history);

        //                    nFlag = 2;
        //                }

        //                nId = history.Id;
        //            }

        //            if (nFlag == 1)
        //            {
        //                continue;
        //            }

        //            List<PatrolPoint> Route = Getcheckpoints(item.id, nId, nFlag);
        //            if (nFlag == 2 || Route.Count <= 0)
        //            {
        //                continue;
        //            }

        //            now.Route = Route;
        //            send.Add(now);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string messgae = ex.Message;
        //    }

        //    return send;

        //}

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
                string path = "api/patrols";
                string url = BaseUri + path;
                if (bFlag)
                {
                    url += "?startDate=" + Convert.ToString(lBegin);
                    url += "&endDate=" + Convert.ToString(lEnd);
                }

                recv = GetEntityList2<patrols>(url);
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
        //public List<PatrolPoint> Getcheckpoints(int patrolId, int ParentId, int nFlag)
        //{
        //    patrols recv = new patrols();
        //    List<PatrolPoint> send = new List<PatrolPoint>();

        //    try
        //    {
        //        string path = "api/patrols/" + Convert.ToString(patrolId);
        //        string url = BaseUri + path;
                
        //        recv = GetEntityList3(url);

        //        foreach (checkpoints item in recv.route)
        //        {
        //            PatrolPoint now = bll.PatrolPoints.Find(p=>p.KksCode == item.kksCode && p.ParentId == ParentId);
        //            PatrolPointHistory history = bll.PatrolPointHistorys.Find(p => p.KksCode == item.kksCode && p.ParentId == ParentId);
        //            int nId = 0;

        //            if (nFlag == 0)
        //            {
        //                if (now == null)
        //                {
        //                    now = new PatrolPoint();

        //                    now.ParentId = ParentId;
        //                    now.StaffCode = item.staffCode;
        //                    now.KksCode = item.kksCode;
        //                    now.DeviceCode = item.deviceCode;
        //                    now.DeviceId = item.deviceId;

        //                    bll.PatrolPoints.Add(now);
        //                }

        //                nId = now.Id;
        //            }
        //            else
        //            {
        //                if (history == null)
        //                {
        //                    history = new PatrolPointHistory();

        //                    history.ParentId = ParentId;
        //                    history.StaffCode = item.staffCode;
        //                    history.KksCode = item.kksCode;
        //                    history.DeviceCode = item.deviceCode;
        //                    history.DeviceId = item.deviceId;

        //                    bll.PatrolPointHistorys.Add(history);
        //                }

        //                nId = history.Id;
        //            }

        //            PatrolPoint pp = Getcheckresults(patrolId, item.deviceId, nId, nFlag);
        //            if (nFlag == 0)
        //            {
        //                now.Success = pp.Success;
        //                bll.PatrolPoints.Edit(now);
        //                now.Checks = pp.Checks;
        //                send.Add(now);
        //            }
        //            else
        //            {
        //                history.Success = pp.Success;
        //                bll.PatrolPointHistorys.Edit(history);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string messgae = ex.Message;
        //    }

        //    return send;

        //}

        public patrols Getcheckpoints(int patrolId)
        {
            patrols recv = new patrols();

            try
            {
                string path = "api/patrols/" + Convert.ToString(patrolId);
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
        //public PatrolPoint Getcheckresults(int patrolId, string deviceId, int parentId, int nFlag)
        //{
        //    checkpoints recv = new checkpoints();
        //    PatrolPoint send = new PatrolPoint();

        //    try
        //    {
        //        string path = "api/patrols/" + Convert.ToString(patrolId) + "/checkpoints/" + deviceId + "/results";
        //        //string path = "api/patrols/" + Convert.ToString(469) + "/checkpoints/100012/results";
        //        string url = BaseUri + path;

        //        recv = GetEntityList4(url);
        //        send.Success = recv.success;

        //        foreach (results item in recv.checks)
        //        {
        //            PatrolPointItem now = bll.PatrolPointItems.Find(p => p.CheckId == item.checkId && p.ParentId == parentId);
        //            PatrolPointItemHistory history = bll.PatrolPointItemHistorys.Find(p => p.CheckId == item.checkId && p.ParentId == parentId);


        //            if (nFlag == 0)
        //            {
        //                if (now == null)
        //                {
        //                    now = new PatrolPointItem();
        //                    now.ParentId = parentId;
        //                    now.KksCode = item.kksCode;
        //                    now.CheckItem = item.checkItem;
        //                    now.StaffCode = item.staffCode;
        //                    now.CheckTime = null;
        //                    now.dtCheckTime = null;
        //                    if (item.checkTime != null)
        //                    {
        //                        now.CheckTime = (item.checkTime + nEightHourSecond) * 1000;
        //                        now.dtCheckTime = TimeConvert.TimeStampToDateTime((long)now.CheckTime);
        //                    }
        //                    now.CheckId = item.checkId;
        //                    now.CheckResult = item.checkResult;
        //                    bll.PatrolPointItems.Add(now);
        //                }
        //                else
        //                {
        //                    now.CheckTime = null;
        //                    now.dtCheckTime = null;
        //                    if (item.checkTime != null)
        //                    {
        //                        now.CheckTime = (item.checkTime + nEightHourSecond) * 1000;
        //                        now.dtCheckTime = TimeConvert.TimeStampToDateTime((long)now.CheckTime);
        //                    }

        //                    now.CheckResult = item.checkResult;
        //                    bll.PatrolPointItems.Edit(now);
        //                }
        //            }
        //            else
        //            {
        //                if (history == null)
        //                {
        //                    history = new PatrolPointItemHistory();
        //                    history.ParentId = parentId;
        //                    history.KksCode = item.kksCode;
        //                    history.CheckItem = item.checkItem;
        //                    history.StaffCode = item.staffCode;
        //                    history.CheckTime = null;
        //                    history.dtCheckTime = null;
        //                    if (item.checkTime != null)
        //                    {
        //                        history.CheckTime = (item.checkTime + nEightHourSecond) * 1000;
        //                        history.dtCheckTime = TimeConvert.TimeStampToDateTime((long)history.CheckTime);
        //                    }
        //                    history.CheckId = item.checkId;
        //                    history.CheckResult = item.checkResult;
        //                    bll.PatrolPointItemHistorys.Add(history);
        //                }
        //            }

        //            if (nFlag == 0)
        //            {
        //                send.Checks.Add(now);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string messgae = ex.Message;
        //    }

        //    return send;

        //}

        public checkpoints Getcheckresults(int patrolId, string deviceId)
        {
            checkpoints recv = new checkpoints();

            try
            {
                string path = "api/patrols/" + Convert.ToString(patrolId) + "/checkpoints/" + deviceId + "/results";
                string url = BaseUri + path;

                recv = GetEntityList4(url);
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;

        }
    }
}
