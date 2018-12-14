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
using DbModel.Tools;
using LocationServices.Locations.Services;

namespace WebLocation.Controllers
{
    public class LocationCardController : Controller
    {
        private Bll db = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;

        // GET: Tags
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<LocationCard> lst = db.LocationCards.ToList().ToPagedList<LocationCard>(pageIndex, pageSize);                                  
            return View("Index", lst);
        }       

        public ActionResult Position()
        {
            return View();
        }      

        public ActionResult RoleSet()
        {
            List<CardRole> cardRoleList = db.CardRoles.ToList();           
            return PartialView("RoleSet", cardRoleList);
        }

        public ActionResult SetTagRole(string tagId, string roleId)
        {
            var service = new TagService();
            var result = service.SetRole(tagId, roleId) != null;
            //result = true;
           
            return Json(new { success = result });
        }        

        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tag = db.LocationCards.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            
            return PartialView(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Tags/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationCard tag)
        {
            if (ModelState.IsValid)
            {
                var result = db.LocationCards.Add(tag);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = db.LocationCards.ErrorMessage });
                }
            }
            return base.View(tag);            
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tag = db.LocationCards.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            
            return PartialView(tag);
        }

        // POST: Tags/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LocationCard tag)
        {
            if (ModelState.IsValid)
            {
                var result = db.LocationCards.Edit(tag);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = db.LocationCards.ErrorMessage });
                }
            }
            return base.View(tag);
        }

        // GET: Tags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tag = db.LocationCards.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
           
            return PartialView(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var tag = db.LocationCards.Find(id);
            db.LocationCards.Remove(tag);
           
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
