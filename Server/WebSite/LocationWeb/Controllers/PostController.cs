using Location.BLL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebLocation.Models;
using System.IO;
using BLL;
using DbModel.Location.AreaAndDev;
using ExcelLib;

using Location.IModel;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class PostController : Controller
    {
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 1;
        // GET: Post
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<Post> lst = bll.Posts.ToList().ToPagedList<Post>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
            //return View(bll.Posts.ToList());
        }

        private void GetListToViewBag()
        {
            List<Post> postList = bll.Posts.ToList();
            SelectList selList = new SelectList(postList, "Id");
            ViewBag.selList = selList.AsEnumerable();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post pst = bll.Posts.Find(id);
            if (pst == null)
            {
                return HttpNotFound();
            }
            //return View(pst);
            return PartialView(pst);
        }


        public ActionResult Create()
        {
            //return View();
            return PartialView();
        }

        // POST: Departments/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Post pst)
        {
            if (ModelState.IsValid)
            {
                //bll.Posts.Add(pst);
                //return RedirectToAction("Index");

                var result = bll.Posts.Add(pst);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Posts.ErrorMessage });
                }
            }

            return View(pst);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post pst = bll.Posts.Find(id);
            if (pst == null)
            {
                return HttpNotFound();
            }

            //return View(pst);
            return PartialView(pst);
        }

        // POST: Departments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Post pst)
        {
            if (ModelState.IsValid)
            {
                //bll.Posts.Edit(pst);
                //return RedirectToAction("Index");

                var result = bll.Posts.Edit(pst);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Posts.ErrorMessage });
                }
            }
           
            return View(pst);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post pst = bll.Posts.Find(id);
            if (pst == null)
            {
                return HttpNotFound();
            }

            //return View(pst);
            return PartialView(pst);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bll.Posts.DeleteById(id);
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