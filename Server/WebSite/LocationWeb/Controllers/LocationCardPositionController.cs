using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Location.Model;
using Location.BLL;
using WebLocation.Models;
using System.IO;
using BLL;
using DbModel.Location.Data;
using ExcelLib;

using Location.IModel;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class LocationCardPositionController : Controller
    {
        private Bll db = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;

        // GET: TagPositions
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<LocationCardPosition> lst = db.LocationCardPositions.ToList().ToPagedList<LocationCardPosition>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
        }

        private void GetListToViewBag()
        {
            List<LocationCardPosition> tagPositionList = db.LocationCardPositions.ToList();
            SelectList selList = new SelectList(tagPositionList,"Tag");
            ViewBag.selList = selList.AsEnumerable();
        }

        // GET: TagPositions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tagPosition = db.LocationCardPositions.Find(id);
            if (tagPosition == null)
            {
                return HttpNotFound();
            }
           
            return PartialView(tagPosition);
        }

        // GET: TagPositions/Create
        public ActionResult Create()
        {
            GetListToViewBag();
            return PartialView();
        }

        // POST: TagPositions/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tag,X,Y,Z,Time,Power,Number,Flag")] LocationCardPosition tagPosition)
        {
            if (ModelState.IsValid)
            {
                //db.LocationCardPositions.Add(tagPosition);
                //db.SaveChanges();
                //return RedirectToAction("Index");

                var result = db.LocationCardPositions.Add(tagPosition);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = db.LocationCardPositions.ErrorMessage });
                }
            }
            GetListToViewBag();
            return View(tagPosition);
        }

        // GET: TagPositions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationCardPosition tagPosition = db.LocationCardPositions.Find(id);
            if (tagPosition == null)
            {
                return HttpNotFound();
            }
            GetListToViewBag();
            //return View(tagPosition);
            return PartialView(tagPosition);
        }

        // POST: Tags/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tag,X,Y,Z,Time,Power,Number,Flag")] LocationCardPosition tagPosition)
        {
            if (ModelState.IsValid)
            {
                //db.LocationCardPositions.Edit(tagPosition);
                //db.Entry(tag).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");

                var result = db.LocationCardPositions.Edit(tagPosition);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = db.LocationCardPositions.ErrorMessage });
                }
            }
            GetListToViewBag();
            return View(tagPosition);
        }

        // GET: TagPositions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationCardPosition tagPosition = db.LocationCardPositions.Find(id);
            if (tagPosition == null)
            {
                return HttpNotFound();
            }
            //return View(tagPosition);
            return PartialView(tagPosition);
        }

        // POST: TagPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LocationCardPosition tagPosition = db.LocationCardPositions.Find(id);
            db.LocationCardPositions.Remove(tagPosition);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
