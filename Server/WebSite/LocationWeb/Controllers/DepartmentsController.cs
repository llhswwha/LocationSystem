using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using BLL.Blls.Location;
using DbModel.Location.Person;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;
using BLL;
using DbModel.Tools;

namespace WebLocation.Controllers
{
    public class DepartmentsController : Controller
    {
        //private DepartmentBll db = new DepartmentBll();
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 2;

        // GET: Departments
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<Department> lst = bll.Departments.ToList().ToPagedList<Department>(pageIndex, pageSize);
            return View("Index", lst);
        }
    
        private void GetDepListToViewBag(Department dep)
        {
            List<Department> deps = bll.Departments.ToList();
            if (dep != null)
            {
                Department dep0 = deps.Find(i => i.Id == dep.Id);
                if (dep0 != null)
                {
                    deps.Remove(dep0);//删除本身，编辑时不能让自己成为自己的子节点
                }
            }
            SelectList list = new SelectList(deps, "Id", "Name");
            IList<SelectListItem> listItem = EnumToList.EnumToListChoice<DepartType>();

            ViewBag.List = list.AsEnumerable();
            ViewBag.selList = listItem;
        }

        public ActionResult Tree()
        {
            return View(bll.Departments.GetRoot());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = bll.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            
            return PartialView(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            GetDepListToViewBag(null);
            //return View();
            return PartialView();
        }


        // POST: Departments/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ShowOrder,ParentId,Type,Description")] Department department)
        {
            if (ModelState.IsValid)
            {
                var result = bll.Departments.Add(department);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Departments.ErrorMessage });
                }
            }
            GetDepListToViewBag(null);
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = bll.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            GetDepListToViewBag(department);
            return PartialView(department);
        }

        // POST: Departments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Abutment_Id,,ParentId,Name,ShowOrder,Type,Description")] Department department)
        {
            if (ModelState.IsValid)
            {
                var result = bll.Departments.Edit(department);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Departments.ErrorMessage });
                }
            }
            GetDepListToViewBag(department);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = bll.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            //return View(department);
            return PartialView(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bll.Departments.DeleteById(id);
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
