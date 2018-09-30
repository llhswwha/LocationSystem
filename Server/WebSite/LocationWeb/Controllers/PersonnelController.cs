using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Location.BLL;
using Webdiyer.WebControls.Mvc;
using Location.Model;
using System.Net;
using WebLocation.Models;
using System.IO;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using ExcelLib;

using Location.IModel;
using WebLocation.Tools;
using DbModel.Location.Relation;
using Location.TModel.Tools;
using System.Globalization;

namespace WebLocation.Controllers
{
    public class PersonnelController : Controller
    {
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 1;
        // GET: Personnel
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<Personnel> lst = bll.Personnels.ToList().ToPagedList<Personnel>(pageIndex, pageSize);
            GetListToViewBag();
            return View(lst);
        }

        public ActionResult Search(string Name, int? ParentId, string Pst, int? TagId, int pageIndex = 1)
        {
            GetListToViewBag();

            PagedList<Personnel> lst = bll.Personnels.DbSet.Where(p => (string.IsNullOrEmpty(Name) ? true : p.Name.Contains(Name)) && (ParentId == null ? true : p.ParentId == ParentId) && (Pst == null ? true : p.Pst == Pst)).OrderBy(p => p.ParentId).ToPagedList<Personnel>(pageIndex, pageSize);

            return View("Index", lst);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnel Psl = bll.Personnels.Find(id);
            if (Psl == null)
            {
                return HttpNotFound();
            }

            return PartialView(Psl);
        }

        private void GetListToViewBag()
        {
            List<Department> DepList = bll.Departments.ToList();
            SelectList selList = new SelectList(DepList, "Id", "Name");
            ViewBag.DepList = selList.AsEnumerable();

            List<Post> PostList = bll.Posts.ToList();
            SelectList selList2 = new SelectList(PostList, "Name", "Name");
            ViewBag.PostList = selList2.AsEnumerable();
        }

        public ActionResult Create()
        {
            Personnel pel = new Personnel();
            GetListToViewBag();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Sex,Photo,BirthDay,Nation,Address,WorkNumber,Email,Phone,Mobile,Enabled,ParentId,Pst")] Personnel pel)
        {
            if (ModelState.IsValid)
            {
                pel.BirthTimeStamp = TimeConvert.DateTimeToTimeStamp(pel.BirthDay);

                var result = bll.Personnels.Add(pel);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Personnels.ErrorMessage });
                }
            }

            GetListToViewBag();
            return View(pel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Personnel pel = bll.Personnels.Find(id);

            if (pel == null)
            {
                return HttpNotFound();
            }

            GetListToViewBag();
            
            return PartialView(pel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Abutment_Id,Name,Sex,Photo,BirthDay,Nation,Address,WorkNumber,Email,Phone,Mobile,Enabled,ParentId,Pst")] Personnel pel)
        {
            if (ModelState.IsValid)
            {
                pel.BirthTimeStamp = TimeConvert.DateTimeToTimeStamp(pel.BirthDay);

                var result = bll.Personnels.Edit(pel);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Personnels.ErrorMessage });
                }
            }

            GetListToViewBag();
            return View(pel);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Personnel pel = bll.Personnels.Find(id);

            if (pel == null)
            {
                return HttpNotFound();
            }

            //return View(pel);
            return PartialView(pel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            bll.Personnels.DeleteById(id);

            

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