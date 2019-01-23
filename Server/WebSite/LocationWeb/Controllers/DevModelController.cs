using Location.BLL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BLL;
using DbModel.Location.AreaAndDev;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class DevModelController : Controller
    {

        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 1;
        // GET: TypeModel
        public ActionResult Index(int pageIndex=1)
        {
            List<DevModel> modelList = bll.DevModels.ToList();
            var queryList = modelList.OrderBy(p=>p.Items).ThenBy(c=>c.Class);
            PagedList<DevModel> list = queryList.ToPagedList<DevModel>(pageIndex, pageSize);
            return View(list);
        }

        // GET: TypeModel/Details/5
        public ActionResult Details(int id)
        {
            DevModel model = bll.DevModels.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // GET: TypeModel/Create
        public ActionResult Create()
        {
            //return View();
            return PartialView();
        }

        // POST: TypeModel/Create
        [HttpPost]
        public ActionResult Create(DevModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = bll.DevModels.Add(model);
                    if (result)
                    {
                        return Json(new { success = result });
                    }
                    else
                    {
                        return Json(new { success = result, errors = bll.DevModels.ErrorMessage });
                    }
                    //return RedirectToAction("Index");
                }
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeModel/Edit/5
        public ActionResult Edit(int id)
        {
            DevModel model = bll.DevModels.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // POST: TypeModel/Edit/5
        [HttpPost]
        public ActionResult Edit(DevModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = bll.DevModels.Edit(model);
                    if (result)
                    {
                        return Json(new { success = result });
                    }
                    else
                    {
                        return Json(new { success = result, errors = bll.DevModels.ErrorMessage });
                    }
                    //return RedirectToAction("Index");
                }
               
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeModel/Delete/5
        public ActionResult Delete(int id)
        {
            DevModel model = bll.DevModels.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // POST: Alarms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bll.DevModels.DeleteById(id);
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

        public ActionResult SearchByClass(string Class, int pageIndex = 1)
        {            
            PagedList<DevModel> lst = bll.DevModels.DbSet.Where(p => (string.IsNullOrEmpty(Class) || p.Class.Contains(Class))).OrderBy(p => p.Class).ToPagedList<DevModel>(pageIndex, pageSize);
            return View("Index", lst);
        }

        public ActionResult SearchByItem(string Item, int pageIndex = 1)
        {
            //if (Item == "")
            //{
            //    Item = null;
            //}
            PagedList<DevModel> lst = bll.DevModels.DbSet.Where(p => (string.IsNullOrEmpty(Item) || p.Items.Contains(Item))).OrderBy(p => p.Class).ToPagedList<DevModel>(pageIndex, pageSize);
            return View("Index", lst);
        }

        public ActionResult SearchByName(string Name, int pageIndex = 1)
        {
            //if (Name == "")
            //{
            //    Name = null;
            //}
            PagedList<DevModel> lst = bll.DevModels.DbSet.Where(p => (string.IsNullOrEmpty(Name) || p.Name.Contains(Name))).OrderBy(p => p.Class).ToPagedList<DevModel>(pageIndex, pageSize);
            return View("Index", lst);
        }
    }
}
