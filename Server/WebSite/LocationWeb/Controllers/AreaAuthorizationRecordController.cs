using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Authorizations;
using DbModel.Location.Person;
using DbModel.Location.Work;
using DbModel.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;
using WArea = WModel.Location.AreaAndDev.Area;

namespace WebLocation.Controllers
{
    public class AreaAuthorizationRecordController : Controller
    {
        private Bll bll = new Bll();
        private Bll bll2 = Bll.NewBllNoRelation();
        private int pageSize = StaticArgs.DefaultPageSize;
        // GET: AreaAuthorizationRecord
        public ActionResult Index(int pageIndex = 1)
        {
            List<Area> Arealst = bll2.Areas.ToList();
            List<AreaAuthorizationRecord> rlst = bll.AreaAuthorizationRecords.ToList();
            foreach (AreaAuthorizationRecord item in rlst)
            {
                item.Area = Arealst.Find(p=>p.Id == item.AreaId);
            }

            PagedList<AreaAuthorizationRecord> lst = rlst.ToPagedList<AreaAuthorizationRecord>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AreaAuthorizationRecord jd = bll.AreaAuthorizationRecords.Find(id);
            if (jd == null)
            {
                return HttpNotFound();
            }

            jd.Area = bll2.Areas.Find(p=>p.Id == jd.AreaId);

            return PartialView(jd);
        }

        public string GetPartAreaList()
        {
            List<Area> lst = bll2.Areas.ToList();
            List<WArea> wlst = new List<WModel.Location.AreaAndDev.Area>();
            foreach (Area item in lst)
            {
                WArea area = new WArea();
                area.id = item.Id;
                if (item.ParentId != null)
                {
                    area.pId = (int)item.ParentId;
                }
                area.name = item.Name;
                wlst.Add(area);
            }

            string treeList = JsonConvert.SerializeObject(wlst);
            return treeList;

        }

        private void GetListToViewBag()
        {
            List<Area> Pts = bll.Areas.ToList();
            SelectList PtList = new SelectList(Pts, "Id", "Name");
            ViewBag.PtList = PtList.AsEnumerable();

            List<CardRole> CardRoles = bll.CardRoles.ToList();
            SelectList CardRoleList = new SelectList(CardRoles, "Id","Name");
            ViewBag.CardRoleList = CardRoleList.AsEnumerable();

            ViewBag.EnumList = EnumToList.EnumToListChoice<RepeatDay>();
            ViewBag.TimeSettingTypeList = EnumToList.EnumToListChoice<TimeSettingType>();
            ViewBag.AreaAccessTypeList = EnumToList.EnumToListChoice<AreaAccessType>();
        }        

        public ActionResult Search(DateTime? ModifyTime, string Name, string AreaName, string Description, TimeSettingType? TimeType, RepeatDay? RepeatDay, AreaAccessType? AccessType, int? CardRoleId, int pageIndex = 1)
        {           
            List<Area> Arealst = bll2.Areas.ToList();
            List<AreaAuthorizationRecord> rlst = bll.AreaAuthorizationRecords.ToList();
            foreach (AreaAuthorizationRecord item in rlst)
            {                              
                item.Area = Arealst.Find(p => p.Id == item.Area.Id);
            }           

            PagedList<AreaAuthorizationRecord> lst = bll.AreaAuthorizationRecords.DbSet.Where(p => ((string.IsNullOrEmpty(Name) || p.Name.Contains(Name)) && (string.IsNullOrEmpty(Description) || p.Description.Contains(Description)) && (string.IsNullOrEmpty(AreaName) || p.Area.Name.Contains(AreaName)) && (TimeType == null || p.TimeType == TimeType) && (RepeatDay == null || p.RepeatDay == RepeatDay) && (AccessType == null || p.AccessType == AccessType) && (CardRoleId == null || p.CardRoleId == CardRoleId))).OrderByDescending(p => p.ModifyTime).ToPagedList<AreaAuthorizationRecord>(pageIndex, pageSize);            
            GetListToViewBag();
            return View("Index", lst);
        }       

        public ActionResult Create()
        {
            AreaAuthorizationRecord jd = new AreaAuthorizationRecord();
            GetListToViewBag();
            return PartialView(jd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaAuthorizationRecord aa, FormCollection col)
        {
            if (ModelState.IsValid)
            {
                //aa.RepeatDay = col["RepeatType"];
                aa.CreateTime = DateTime.Now;
                aa.ModifyTime = null;
                aa.DeleteTime = null;
                //aa.SetTimeSpane();
                //bll.JurisDictions.Add(jd);
                //return RedirectToAction("Index");

                var result = bll.AreaAuthorizationRecords.Add(aa);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.AreaAuthorizationRecords.ErrorMessage });
                }
            }

            GetListToViewBag();
            return View(aa);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaAuthorizationRecord jd = bll.AreaAuthorizationRecords.Find(id);
            if (jd == null)
            {
                return HttpNotFound();
            }

            jd.Area = bll2.Areas.Find(p => p.Id == jd.AreaId);
            GetListToViewBag();
            return PartialView(jd);
        }

        // POST: Alarms/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AreaAuthorizationRecord jd, FormCollection col)
        {
            if (ModelState.IsValid)
            {
                jd.ModifyTime = DateTime.Now;
                //jd.RepeatDay = col["RepeatDay"];

                var result = bll.AreaAuthorizationRecords.Edit(jd);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.AreaAuthorizationRecords.ErrorMessage });
                }
            }

            GetListToViewBag();
            return View(jd);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaAuthorizationRecord jd = bll.AreaAuthorizationRecords.Find(id);
            if (jd == null)
            {
                return HttpNotFound();
            }

            jd.Area = bll2.Areas.Find(p => p.Id == jd.AreaId);
            //return View(jd);
            return PartialView(jd);
        }

        // POST: Alarms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bll.AreaAuthorizationRecords.DeleteById(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bll.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}