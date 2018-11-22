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

namespace WebApiLib.Clients
{
    public class BaseDataClient
    {
        private Bll bll = Bll.Instance();

        private string Message;

        public string BaseUri { get; set; }

        public BaseDataClient()
        {
            BaseUri = "http://<host>:<port>/";
        }

        public BaseDataClient(string host, string port)
        {
            BaseUri = string.Format("http://{0}:{1}/", host, port);
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
        public BaseTran<user> GetUserList()
        {
            //users recv = new users();
            //BaseTran<user> recv2 = new BaseTran<user>();
            BaseTran<user> recv = new BaseTran<user>();


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
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public BaseTran<org> GetorgList()
        {
            BaseTran<org> recv = new BaseTran<org>();

            try
            {
                string path = "api/org";
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
                    Department Parent = bll.Departments.DbSet.Where(p => p.Abutment_Id == item.parent_id).FirstOrDefault();
                    if (dp == null || Parent == null)
                    {
                        continue;
                    }

                    if (dp.Abutment_Id == 0 && Parent.Abutment_Id == 0)
                    {
                        continue;
                    }

                    dp.ParentId = Parent.Id;
                    bll.Departments.Edit(dp);
                }

            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        public BaseTran<zone> GetzonesList()
        {
            BaseTran<zone> recv = new BaseTran<zone>();

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
                    area.KKS = item.kksCode;
                    area.Abutment_ParentId = item.parent_id;
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
                    if (item.parent_id == null)
                    {
                        continue;
                    }

                    Area Child = bll.Areas.DbSet.Where(p => p.Abutment_Id == item.id).FirstOrDefault();
                    Area Parent = bll.Areas.DbSet.Where(p => p.Abutment_Id == item.parent_id).FirstOrDefault();

                    if (Child != null && Parent != null)
                    {
                        Child.ParentId = Parent.Id;
                        bll.Areas.Edit(Child);
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
        /// 获取单个区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public zone GetSingleZonesInfo(int id, int view)
        {
            zone recv = new zone();
            
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
                area.KKS = recv.kksCode;
                area.Abutment_ParentId = recv.parent_id;
                area.Describe = recv.description;

                if (recv.parent_id != null)
                {
                    Area Parent = bll.Areas.DbSet.Where(p => p.Abutment_Id == recv.parent_id).FirstOrDefault();
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
                    area2.KKS = item2.kksCode;
                    area2.Abutment_ParentId = item2.parent_id;
                    area2.Describe = item2.description;

                    if (nFlag == 1)
                    {
                        bll.Areas.Add(area2);
                    }
                    else
                    {
                        bll.Areas.Edit(area2);
                    }
                }

                foreach (device item3 in recv.devices)
                {
                    DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == item3.kksCode).FirstOrDefault();
                    nFlag = 0;
                    if (devinfo == null)
                    {
                        devinfo = new DevInfo();
                        nFlag = 1;
                    }

                    devinfo.ParentId = area.Id;
                    devinfo.Abutment_Id = item3.id;
                    devinfo.Code = item3.code;
                    devinfo.KKS = item3.kksCode;
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
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;
        }

        /// <summary>
        /// 获取指定区域下设备列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseTran<device> GetZoneDevList(int id)
        {
            BaseTran<device> recv = new BaseTran<device>();
            string strId = Convert.ToString(id);

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
                        DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == item.kksCode).FirstOrDefault();
                        int nFlag = 0;
                        if (devinfo == null)
                        {
                            devinfo = new DevInfo();
                            nFlag = 1;
                        }

                        devinfo.ParentId = area.Id;
                        devinfo.Abutment_Id = item.id;
                        devinfo.Code = item.code;
                        devinfo.KKS = item.kksCode;
                        devinfo.Name = item.name;
                        devinfo.Abutment_Type = (Abutment_DevTypes)item.type;
                        devinfo.Status = (Abutment_Status)item.state;
                        devinfo.RunStatus = (Abutment_RunStatus)item.running_state;
                        devinfo.Placed = item.placed;
                        devinfo.Abutment_DevID = item.raw_id;

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
        /// 获取设备列表
        /// </summary>
        /// <param name="types"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public BaseTran<device> GetDeviceList(string types, string code, string name)
        {
            BaseTran<device> recv = new BaseTran<device>();

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
                    DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == item.kksCode).FirstOrDefault();
                    int nFlag = 0;
                    if (devinfo == null)
                    {
                        devinfo = new DevInfo();
                        nFlag = 1;
                    }

                    devinfo.Abutment_Id = item.id;
                    devinfo.Code = item.code;
                    devinfo.KKS = item.kksCode;
                    devinfo.Name = item.name;
                    devinfo.Abutment_Type = (Abutment_DevTypes)item.type;
                    devinfo.Status = (Abutment_Status)item.state;
                    devinfo.RunStatus = (Abutment_RunStatus)item.running_state;
                    devinfo.Placed = item.placed;
                    devinfo.Abutment_DevID = item.raw_id;

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
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;
        }

        /// <summary>
        /// 获取单个设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public device GetSingleDeviceInfo(int id)
        {
            device recv = new device();
            string strId = Convert.ToString(id);

            try
            {
                string path = "api/devices/" + strId;
                string url = BaseUri + path;
                recv = GetEntityDetail<device>(url);
                
                DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.KKS == recv.kksCode).FirstOrDefault();
                int nFlag = 0;
                if (devinfo == null)
                {
                    devinfo = new DevInfo();
                    nFlag = 1;
                }

                devinfo.Abutment_Id = recv.id;
                devinfo.Code = recv.code;
                devinfo.KKS = recv.kksCode;
                devinfo.Name = recv.name;
                devinfo.Abutment_Type = (Abutment_DevTypes)recv.type;
                devinfo.Status = (Abutment_Status)recv.state;
                devinfo.RunStatus = (Abutment_RunStatus)recv.running_state;
                devinfo.Placed = recv.placed;
                devinfo.Abutment_DevID = recv.raw_id;

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
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }
            
            return recv;
        }

        /// <summary>
        /// 获取单台设备操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public BaseTran<devices_actions> GetSingleDeviceActionHistory(int id, string begin_date, string end_date)
        {
            BaseTran<devices_actions> recv = new BaseTran<devices_actions>();

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
                }
            }

            return recv;
        }

        /// <summary>
        /// 获取门禁卡列表
        /// </summary>
        /// <returns></returns>
        public BaseTran<cards> GetCardList()
        {
            BaseTran<cards> recv = new BaseTran<cards>();

            try
            {
                string path = "api/cards";
                string url = BaseUri + path;
                recv = GetEntityList<cards>(url);
                foreach (cards item in recv.data)
                {
                    EntranceGuardCard egc = bll.EntranceGuardCards.DbSet.FirstOrDefault(p => p.Abutment_Id == item.id);
                    int nFlag = 0;
                    if (egc == null)
                    {
                        egc = new EntranceGuardCard();
                        nFlag = 1;
                    }

                    egc.Abutment_Id = item.id;
                    egc.Code = item.code;
                    egc.State = item.state;

                    if (nFlag == 1)
                    {
                        bll.EntranceGuardCards.Add(egc);
                    }
                    else
                    {
                        bll.EntranceGuardCards.Edit(egc);
                    }

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

            return recv;
        }

        /// <summary>
        /// 获取门禁卡操作历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public BaseTran<cards_actions> GetSingleCardActionHistory(int id, string begin_date, string end_date)
        {
            BaseTran<cards_actions> recv = new BaseTran<cards_actions>();

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
        public tickets GetTicketsDetail(int id)
        {
            tickets recv = new tickets();

            try
            {
                string path = "api/tickets/" + id;
                string url = BaseUri + path;
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
        public BaseTran<events> GeteventsList(int? src, int? level, long? begin_t, long? end_t)
        {
            BaseTran<events> recv = new BaseTran<events>();

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
                    if (item.device_id == null)
                    {
                        continue;
                    }

                    DevInfo di = bll.DevInfos.DbSet.Where(p => p.Abutment_Id == item.device_id).FirstOrDefault();
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
                    da.Device_desc = item.device_desc;
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
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;
        }

        public BaseTran<sis> GetSomesisList(string kks)
        {
            BaseTran<sis> recv = new BaseTran<sis>();

            try
            {
                string path = "api/rt/sis?kks=" + kks;
                string url = BaseUri + path;
                recv = GetEntityList<sis>(url);

                if (recv.data == null)
                {
                    recv.data = new List<sis>();
                }

                foreach (sis item in recv.data)
                {
                    DevInfo DevInfo = bll.DevInfos.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();
                    if (DevInfo == null)
                    {
                        continue;
                    }

                    DevInstantData did = bll.DevInstantDatas.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();

                    if (did == null)
                    {
                        did = new DevInstantData();
                        did.KKS = item.kks;
                        did.Value = item.value;
                        did.DateTime = DateTime.Now;
                        did.DateTimeStamp = TimeConvert.DateTimeToTimeStamp(did.DateTime);
                        bll.DevInstantDatas.Add(did);
                    }
                    else
                    {
                        DevInstantDataHistory didh = did.RemoveToHistory();
                        did.Value = item.value;
                        did.DateTime = DateTime.Now;
                        did.DateTimeStamp = TimeConvert.DateTimeToTimeStamp(did.DateTime);

                        bll.DevInstantDatas.Edit(did);
                        bll.DevInstantDataHistorys.Add(didh);
                    }

                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return recv;
        }
    }
}
