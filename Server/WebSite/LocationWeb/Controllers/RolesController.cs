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
using DbModel.Location.Person;
using ExcelLib;

using Location.IModel;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;


namespace WebLocation.Controllers
{
    public class RolesController : Controller
    {
        private Bll db = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 1;

        // GET: Roles
        public ActionResult Index(int pageIndex =1)
        {
            PagedList<Role> lst = db.Roles.ToList().ToPagedList<Role>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
        }

        private void GetListToViewBag()
        {
            List<Role> roleList = db.Roles.ToList();
            SelectList selList = new SelectList(roleList, "Id", "Code");
            ViewBag.selList = selList.AsEnumerable();
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            
            return PartialView(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Roles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                var result = db.Roles.Add(role);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = db.Roles.ErrorMessage });
                }
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
           
            return PartialView(role);
        }

        // POST: Roles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                var result = db.Roles.Edit(role);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = db.Roles.ErrorMessage });
                }
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            
            return PartialView(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            
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
