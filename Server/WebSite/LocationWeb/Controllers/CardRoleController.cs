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
using LocationServices.Locations.Services;
using WArea = WModel.Location.AreaAndDev.Area;
using Newtonsoft.Json;
using LocationServices.Locations;

namespace WebLocation.Controllers
{
    public class CardRoleController : Controller
    {
        private Bll bll2 = new Bll(false, false, false, false);
        private Bll db = new Bll();
        private Bll bll = new Bll();
        //private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 4;

        // GET: CardRole
        public ActionResult Index()
        {          
            List<CardRole> cardRoleList = db.CardRoles.ToList();
            return View(cardRoleList);
            //PagedList<CardRole> lst = db.CardRoles.ToList().ToPagedList<CardRole>(pageIndex, pageSize);            
            //return View("Index", lst);
        }

        private void GetListToViewBag()
        {
            List<Area> Pts = bll.Areas.ToList();
            SelectList PtList = new SelectList(Pts, "Id", "Name");
            ViewBag.PtList = PtList.AsEnumerable();
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
            var areas = new LocationService().GetCardRoleAccessAreas(cardRole.Id);
            if(areas!=null)
                cardRole.AreaIds = areas.ToArray();
            GetListToViewBag();
            return PartialView(cardRole);
        } 
        
        //GET: CardRoles/Create
        public ActionResult Create()
        {
            GetListToViewBag();
            return PartialView();
        }

        // POST: CardRoles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CardRole cardRole)
        {
            if (ModelState.IsValid)
            {
                var result = db.CardRoles.Add(cardRole);
                if (result)
                {
                    if (cardRole.AreaIds!=null && cardRole.AreaIds.Length > 0)
                    {
                        var result2 = new LocationService().SetCardRoleAccessAreas(cardRole.Id, cardRole.AreaIds.ToList());
                    }                
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = db.CardRoles.ErrorMessage });
                }
            }
            GetListToViewBag();
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
            var areas = new LocationService().GetCardRoleAccessAreas(cardRole.Id);
            if (areas != null)
                cardRole.AreaIds = areas.ToArray();
            GetListToViewBag();
            return PartialView(cardRole);
        }

        // POST: CardRoles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CardRole cardRole)
        {
            if (ModelState.IsValid)
            {
                var result = db.CardRoles.Edit(cardRole);
                if (result)
                {
                    if (cardRole.AreaIds != null && cardRole.AreaIds.Length > 0)
                    {
                        var result2 = new LocationService().SetCardRoleAccessAreas(cardRole.Id, cardRole.AreaIds.ToList());
                    }
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, erroes = db.CardRoles.ErrorMessage });
                }
            }
            GetListToViewBag();
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