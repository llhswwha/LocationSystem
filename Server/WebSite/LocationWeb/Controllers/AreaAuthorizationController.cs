using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Work;
using DbModel.Tools;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class AreaAuthorizationController : Controller
    {
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 1;

        // GET: JurisDiction
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<AreaAuthorization> lst = bll.JurisDictions.ToList().ToPagedList<AreaAuthorization>(pageIndex, pageSize);
            return View("Index", lst);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AreaAuthorization jd = bll.JurisDictions.Find(id);
            if (jd == null)
            {
                return HttpNotFound();
            }
       
            return PartialView(jd);
        }

        private void GetListToViewBag()
        {
            List<Area> Pts = bll.Areas.ToList();
            SelectList PtList = new SelectList(Pts, "Id", "Name");
            ViewBag.PtList = PtList.AsEnumerable();

            List<LocationCard> Tags = bll.LocationCards.ToList();
            SelectList TagList = new SelectList(Tags, "Id", "Name");
            ViewBag.TagList = TagList.AsEnumerable();

            ViewBag.EnumList = EnumToList.EnumToListChoice<JurisDictionType>();

            List<SelectListItem> RepeatTypeList = new List<SelectListItem>();
            RepeatTypeList.Add(new SelectListItem { Text = "星期一", Value = "星期一" });
            RepeatTypeList.Add(new SelectListItem { Text = "星期二", Value = "星期二" });
            RepeatTypeList.Add(new SelectListItem { Text = "星期三", Value = "星期三" });
            RepeatTypeList.Add(new SelectListItem { Text = "星期四", Value = "星期四" });
            RepeatTypeList.Add(new SelectListItem { Text = "星期五", Value = "星期五" });
            RepeatTypeList.Add(new SelectListItem { Text = "星期六", Value = "星期六" });
            RepeatTypeList.Add(new SelectListItem { Text = "星期天", Value = "星期天" });
            RepeatTypeList.Add(new SelectListItem { Text = "一次", Value = "一次" });
            ViewBag.RepeatTypeList = RepeatTypeList;

        }

        public ActionResult Create()
        {
            AreaAuthorization jd = new AreaAuthorization();
            GetListToViewBag();
            return PartialView(jd);
        }

        // POST: Alarms/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Describe,nFlag,StartTime,EndTime,nTimeLength,DelayTime,ErrorDistance,RepeatType,AreaId,LocationCardId")] AreaAuthorization aa, FormCollection col)
        {
            if (ModelState.IsValid)
            {
                aa.RepeatType = col["RepeatType"];
                aa.CreateTime = DateTime.Now;
                aa.ModifyTime = null;
                aa.DeleteTime = null;
                aa.SetTimeSpane();
                //bll.JurisDictions.Add(jd);
                //return RedirectToAction("Index");

                var result = bll.JurisDictions.Add(aa);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.JurisDictions.ErrorMessage });
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
            AreaAuthorization jd = bll.JurisDictions.Find(id);
            if (jd == null)
            {
                return HttpNotFound();
            }

            GetListToViewBag();
            return PartialView(jd);
        }

        // POST: Alarms/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Describe,nFlag,StartTime,EndTime,nTimeLength,DelayTime,ErrorDistance,RepeatType,AreaId,LocationCardId,CreateTime,ModifyTime")] AreaAuthorization jd, FormCollection col)
        {
            if (ModelState.IsValid)
            {
                jd.ModifyTime = DateTime.Now;
                jd.RepeatType = col["RepeatType"];

                var result = bll.JurisDictions.Edit(jd);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.JurisDictions.ErrorMessage });
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
            AreaAuthorization jd = bll.JurisDictions.Find(id);
            if (jd == null)
            {
                return HttpNotFound();
            }
            //return View(jd);
            return PartialView(jd);
        }

        // POST: Alarms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bll.JurisDictions.DeleteById(id);
          
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