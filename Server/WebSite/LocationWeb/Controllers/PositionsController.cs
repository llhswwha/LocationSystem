using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Location.BLL;

using Location.Model;
using WebLocation.Models;
using System.IO;
using BLL;
using DbModel.LocationHistory.Data;
using ExcelLib;

using Location.IModel;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class PositionsController : Controller
    {
        private Bll db = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 15;

        // GET: Positions
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<Position> lst = db.Positions.ToList().ToPagedList<Position>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
            //return View(db.Position.ToList());
        }

        private void GetListToViewBag()
        {
            List<Position> positionList = db.Positions.ToList();
            SelectList selList = new SelectList(positionList,"Id", "Tag");
            ViewBag.selList = selList.AsEnumerable();
        }

        // GET: Positions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // GET: Positions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Tag,X,Y,Z,Time,Power,Number,Flag")] Position position)
        {
            if (ModelState.IsValid)
            {
                db.AddPosition(position);
                return RedirectToAction("Index");
            }

            return View(position);
        }

        // GET: Positions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tag,X,Y,Z,Time,Power,Number,Flag")] Position position)
        {
            if (ModelState.IsValid)
            {
                db.Positions.Edit(position);
                return RedirectToAction("Index");
            }
            return View(position);
        }

        // GET: Positions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Positions.DeleteById(id);
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
