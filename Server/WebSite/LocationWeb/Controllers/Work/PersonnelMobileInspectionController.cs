using Location.BLL;
using Location.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DbModel.Location.Person;
using DbModel.Location.Work;
using DbModel.LocationHistory.Work;

namespace WebLocation.Controllers.Work
{
    public class PersonnelMobileInspectionController : Controller
    {
        private Bll bll = new Bll();

        // GET: PersonnelMobileInspection
        public ActionResult Index()
        {
            List<MobileInspection> mList = bll.MobileInspections.ToList();
            List<Personnel> pList = bll.Personnels.ToList();
            List<PersonnelMobileInspection> pmList = bll.PersonnelMobileInspections.ToList();
            List<PersonnelMobileInspection> pmList2 = bll.PersonnelMobileInspections.DbSet.Where(P => P.bOverTime == true).ToList();
            PersonnelMobileInspection pm = new PersonnelMobileInspection();
            pm.PlanStartTime = DateTime.Now;
            pm.PlanEndTime = DateTime.Now;
            pm.bOverTime = false;
            pm.Remark = "";

            //超时的人员或巡检轨迹在移到历史数据库之前，不能用于分配
            foreach (PersonnelMobileInspection item in pmList2)
            {
                MobileInspection m1 = mList.Find(p => p.Id == item.MobileInspectionId);
                Personnel p1 = pList.Find(p => p.Id == item.PersonnelId);
                if (m1 != null)
                {
                    mList.Remove(m1);
                }

                if (p1 != null)
                {
                    pList.Remove(p1);
                }
            }

            ViewBag.mList = mList;
            ViewBag.pList = pList;
            ViewBag.pmList = pmList;
            return View(pm);
        }

        public ActionResult Create(PersonnelMobileInspection pm)
        {
            List<PersonnelMobileInspection> pmList = bll.PersonnelMobileInspections.ToList();
            int nCount = pmList.Where(p => (p.MobileInspectionId == pm.MobileInspectionId || p.PersonnelId == pm.PersonnelId) && ((p.PlanStartTime <= pm.PlanStartTime && p.PlanEndTime >= pm.PlanStartTime) || (p.PlanStartTime <= pm.PlanEndTime && p.PlanEndTime >= pm.PlanEndTime) || (p.PlanStartTime >= pm.PlanStartTime && p.PlanEndTime <= pm.PlanEndTime))).Count();
            if (nCount >= 1)
            {
                Content("<script>alrer('该时间段内，已有该巡检人员或巡检轨迹')</script>");
                return RedirectToAction("Index");
                //Response.Write("<script>alrer('该时间段内，已有该巡检人员或巡检轨迹')</script>");
            }

            MobileInspection MobileInspection = bll.MobileInspections.Find(pm.MobileInspectionId);
            string MobileInspectionName = MobileInspection.Name;
            string PersonnelName = bll.Personnels.DbSet.Where(P => P.Id == pm.PersonnelId).Select(p => p.Name).FirstOrDefault();
            
            pm.PersonnelName = PersonnelName;
            pm.MobileInspectionName = MobileInspectionName;
            pm.StartTime = null;

            List<PersonnelMobileInspectionItem> pmiList = new List<PersonnelMobileInspectionItem>();

            foreach (MobileInspectionItem item in MobileInspection.Items)
            {
                PersonnelMobileInspectionItem pmi = new PersonnelMobileInspectionItem();
                pmi.ItemId = item.Id;
                pmi.PID = pm.Id;
                pmi.ItemName = item.ItemName;
                pmi.nOrder = item.nOrder;
                pmi.DevId = item.DevId;
                pmi.DevName = item.DevName;
                pmi.PunchTime = null;
                pmiList.Add(pmi);
            }

            pm.list = pmiList;
            bll.PersonnelMobileInspections.Add(pm);
            
            return RedirectToAction("Index");
        }

        public ActionResult Begin(int Id)
        {
            PersonnelMobileInspection pm = bll.PersonnelMobileInspections.Find(Id);
            if (pm == null)
            {
                return RedirectToAction("Index");
            }

            pm.list = bll.PersonnelMobileInspectionItems.DbSet.Where(p => p.PID == Id).ToList();

            if (pm.list == null)
            {
                pm.list = new List<PersonnelMobileInspectionItem>();

            }

            if (pm.StartTime == null)
            {
                pm.StartTime = DateTime.Now;
                if (pm.StartTime >= pm.PlanEndTime)
                {
                    pm.bOverTime = true;
                }
            }

            bll.PersonnelMobileInspections.Edit(pm);
            ViewBag.bOverTime = "False";
            if (pm.bOverTime)
            {
                ViewBag.bOverTime = "True";
            }

            return View(pm);
        }

        public ActionResult End(int Id)
        {
            PersonnelMobileInspection pm = bll.PersonnelMobileInspections.Find(Id);
            if (pm == null || pm.StartTime == null)
            {
                return RedirectToAction("Index");
            }

            pm.list = bll.PersonnelMobileInspectionItems.DbSet.Where(p => p.PID == Id).ToList();

            if (pm.list == null)
            {
                pm.list = new List<PersonnelMobileInspectionItem>();

            }

            PersonnelMobileInspectionHistory pmh = new PersonnelMobileInspectionHistory();
            pmh.Id = pm.Id;
            pmh.PersonnelId = pm.PersonnelId;
            pmh.PersonnelName = pm.PersonnelName;
            pmh.MobileInspectionId = pm.MobileInspectionId;
            pmh.MobileInspectionName = pm.MobileInspectionName;
            pmh.PlanStartTime = pm.PlanStartTime;
            pmh.PlanEndTime = pm.PlanEndTime;
            pmh.StartTime = pm.StartTime;
            pmh.EndTime = DateTime.Now;
            pmh.bOverTime = pm.bOverTime;
            pmh.Remark = pm.Remark;

            List<PersonnelMobileInspectionItemHistory> list = new List<PersonnelMobileInspectionItemHistory>();

            foreach (PersonnelMobileInspectionItem item in pm.list)
            {
                PersonnelMobileInspectionItemHistory pmih = new PersonnelMobileInspectionItemHistory();
                pmih.Id = item.Id;
                pmih.ItemId = item.ItemId;
                pmih.PID = item.PID;
                pmih.ItemName = item.ItemName;
                pmih.nOrder = item.nOrder;
                pmih.DevId = item.DevId;
                pmih.DevName = item.DevName;
                pmih.PunchTime = item.PunchTime;
                list.Add(pmih);
            }

            pmh.list = list;

            if (pmh.EndTime >= pm.PlanEndTime)
            {
                pmh.bOverTime = true;
            }

            bll.PersonnelMobileInspections.DeleteById(Id);
            bll.PersonnelMobileInspectionHistorys.Add(pmh);

            return RedirectToAction("Index");
        }

        public ActionResult Punch(int Id, int pid)
        {
            PersonnelMobileInspectionItem pmi = bll.PersonnelMobileInspectionItems.Find(Id);
            if (pmi != null && pmi.PunchTime == null)
            {
                pmi.PunchTime = DateTime.Now;
                bll.PersonnelMobileInspectionItems.Edit(pmi);
            }

            PersonnelMobileInspection pm = bll.PersonnelMobileInspections.Find(pid);
            if (pm != null && pmi.PunchTime >= pm.PlanEndTime)
            {
                pm.bOverTime = true;
                bll.PersonnelMobileInspections.Edit(pm);
            }

            return RedirectToAction("Begin", pm);
        }

        public ActionResult Detail(int Id)
        {
            PersonnelMobileInspectionItem pmi = bll.PersonnelMobileInspectionItems.Find(Id);
            MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(pmi.DevId);
            if (pmi == null || MobileInspectionDev == null)
            {
                return HttpNotFound();
            }

            ViewBag.Dev = MobileInspectionDev;
            
            return View(pmi);
        }
    }
}