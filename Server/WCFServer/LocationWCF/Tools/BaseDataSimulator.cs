using BLL;
using BLL.Blls;
using DAL;
using DbModel.BaseData;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Tools
{
    public class BaseDataSimulator
    {
        public void SaveDepToOrg()
        {
            Bll bll = new Bll();
            List<Department> dlst = bll.Departments.ToList();
            List<org> orgs = new List<org>();
            for (int i = 0; i < dlst.Count; i++)
            {
                Department dept = dlst[i];
                org org = new org();

                org.id = dept.Abutment_Id ?? 0;
                org.name = dept.Name;
                org.parentId = dept.Abutment_ParentId;
                if (org.parentId == null)
                {
                    org.parentId = dept.ParentId;
                }
                org.type = (int)dept.Type;
                org.description = dept.Description;

                //db.orgs.Add(org);

                orgs.Add(org);

                //if (i % 10 == 0)
                {
                    Log.Info(LogTags.BaseData, string.Format("dept:{0}({1}/{2})", dept.Name, i + 1, dlst.Count));
                }

            }
            Log.Info(LogTags.BaseData, "保存区域信息...");
            BaseDataDb db = new BaseDataDb();
            var r = db.SetTable(db.orgs, orgs);
        }

        public void SavePersonnelToUser()
        {
            Log.Info(LogTags.BaseData, "获取人员信息...");
            Bll bll = new Bll();
            List<Personnel> list = bll.Personnels.ToList();
            List<user> users = new List<user>();
            for (int i = 0; i < list.Count; i++)
            {
                Personnel p = list[i];
                user user = new user();
                BaseDataHelper.SetUser(p, user);
                users.Add(user);

                if (i % 5 == 0)
                {
                    Log.Info(LogTags.BaseData, string.Format("device:{0}({1}/{2})", user.name, i, list.Count));
                }
            }

            Log.Info(LogTags.BaseData, "保存人员信息...");

            BaseDataDb db = new BaseDataDb();
            var r = db.SetTable(db.users, users);
        }

        public void SaveAreaToZone()
        {
            Log.Info(LogTags.BaseData, "获取区域信息...");

            Bll bll = new Bll();
            List<Area> list = bll.Areas.ToList();
            List<zone> zones = new List<zone>();
            for (int i = 0; i < list.Count; i++)
            {
                Area area = list[i];
                zone zone = new zone();
                zone.name = area.Name;
                zone.id = area.Abutment_Id ?? 0;
                zone.parent_Id = area.ParentId;
                zone.kks = area.KKS;
                zone.description = area.Describe;
                zone.x = area.X;
                zone.y = area.Y;
                zone.z = area.Z;
                zones.Add(zone);

                if (i % 20 == 0)
                {
                    Log.Info(LogTags.BaseData, string.Format("device:{0}({1}/{2})", zone.name, i, list.Count));
                }
            }

            Log.Info(LogTags.BaseData, "保存区域信息...");

            BaseDataDb db = new BaseDataDb();
            var r = db.SetTable(db.zones, zones);
        }

        public void SaveGuardCardToCard()
        {
            Bll bll = new Bll();
            Log.Info(LogTags.BaseData, "获取门禁信息...");

            List<EntranceGuardCard> elst = bll.EntranceGuardCards.ToList();
            List<Personnel> plst = bll.Personnels.ToList();
            List<EntranceGuardCardToPersonnel> eglst = bll.EntranceGuardCardToPersonnels.ToList();
            List<cards> cardsList = new List<cards>();
            for (int i = 0; i < elst.Count; i++)
            {
                var item = elst[i];
                cards card = new cards();
                card.cardId = item.Abutment_Id ?? 0;
                card.cardCode = item.Code;
                card.state = item.State;
                var r = eglst.Find(j => j.EntranceGuardCardId == item.Id);
                if (r != null)
                {
                    card.emp_id = r.Personnel.Abutment_Id ?? r.PersonnelId;
                }
                cardsList.Add(card);

                //if (i % 20 == 0)
                {
                    Log.Info(LogTags.BaseData, string.Format("device:{0}({1}/{2})", card.cardCode, i, elst.Count));
                }
            }

            Log.Info(LogTags.BaseData, "保存门禁信息...");

            BaseDataDb db = new BaseDataDb();
            var r2 = db.SetTable(db.cards, cardsList);
        }

        public void SaveDevInfoToDevice()
        {
            Bll bll = new Bll();
            Log.Info(LogTags.BaseData, "获取设备信息...");
            
            var archors = bll.Archors.ToList();
            var cameras = bll.Dev_CameraInfos.ToList();
            var list = bll.DevInfos.GetListWithDetail(archors, cameras);


            List<device> devices = new List<device>();
            for (int i = 0; i < list.Count; i++)
            {
                DevInfo item = list[i];
                device device = new device();
                BaseDataHelper.SetDevice(device, item);
                //db.devices.Add(device);
                devices.Add(device);
                if (i % 20 == 0)
                {
                    Log.Info(LogTags.BaseData, string.Format("device:{0}({1}/{2})", device.name, i, list.Count));
                }
            }
            Log.Info(LogTags.BaseData, "保存设备信息...");

            BaseDataDb db = new BaseDataDb();
            var r = db.SetTable(db.devices, devices);

            //db.AddRange(devices);
        }
    }
}
