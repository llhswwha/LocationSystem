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
using DbModel.Location.AreaAndDev;
using ExcelLib;

using Location.IModel;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;
using DbModel.Location.Authorizations;

namespace WebLocation.Controllers
{
    public class CardRoleController : Controller
    {
        private Bll db = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 4;

        // GET: CardRole
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<CardRole> lst = db.CardRoles.ToList().ToPagedList<CardRole>(pageIndex, pageSize);
            return View("Index", lst);
        }

        // GET: CardRole/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cardRole = db.CardRoles.Find(id);
            if (cardRole == null)
            {
                return HttpNotFound();
            }

            return PartialView(cardRole);
        } 
        
        //GET: CardRoles/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: CardRoles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")]CardRole cardRole)
        {
            if (ModelState.IsValid)
            {
                var result = db.CardRoles.Add(cardRole);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = db.CardRoles.ErrorMessage });
                }
            }
            return View(cardRole);
        }

        // GET: CardRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cardRole = db.CardRoles.Find(id);
            if(cardRole == null)
            {
                return HttpNotFound();
            }
            return PartialView(cardRole);
        }

        // POST: CardRoles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")]CardRole cardRole)
        {
            if (ModelState.IsValid)
            {
                var result = db.CardRoles.Edit(cardRole);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, erroes = db.CardRoles.ErrorMessage });
                }
            }
            return View(cardRole);
        }

        // GET: CardRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cardRole = db.CardRoles.Find(id);
            if(cardRole == null)
            {
                return HttpNotFound();
            }
            return PartialView(cardRole);
        }

        // POST: CardRoles/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var cardRole = db.CardRoles.Find(id);
            db.CardRoles.Remove(cardRole);

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