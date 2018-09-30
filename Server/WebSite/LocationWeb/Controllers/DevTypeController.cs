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
    /*insert into Location.dbo.DevType(TypeName,TypeCode,orientation,InstantTime,
    ReviseTime,height,energy,weight,model,style,manufacturer,sizen,colour,refrigeration,FrontElevation,RearView,BackInstruction,Obligate3,Obligate4) 
    select TypeName,TypeCode,orientation,InstantTime,ReviseTime,height,energy,weight,model,style,manufacturer,sizen,colour,refrigeration,FrontElevation,RearView,BackInstruction,Obligate3,Obligate4 from topviewxp.dbo.DevType
   */
    public class DevTypeController : Controller
    {
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 1;

        // GET: ModelProperty
        public ActionResult Index(int pageIndex = 1)
        {
            List<DevType> modelList = bll.DevTypes.ToList();
            var queryList = modelList.OrderBy(p => p.TypeCode);
            PagedList<DevType> list = queryList.ToPagedList<DevType>(pageIndex, pageSize);
            return View(list);
        }

        // GET: ModelProperty/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = bll.DevTypes.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: ModelProperty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModelProperty/Create
        [HttpPost]
        public ActionResult Create(DevType Property)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bll.DevTypes.Add(Property);
                    return RedirectToAction("Index");
                }
                return View(Property);
            }
            catch
            {
                return View();
            }
        }

        // GET: ModelProperty/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevType model = bll.DevTypes.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: ModelProperty/Edit/5
        [HttpPost]
        public ActionResult Edit(DevType Property)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bll.DevTypes.Edit(Property);
                    return RedirectToAction("Index");
                }
                return View(Property);
            }
            catch
            {
                return View();
            }
        }

        // GET: ModelProperty/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevType model = bll.DevTypes.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        // POST: Alarms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            //t_SetModel model = bll.t_SetModels.Find(id);
            bll.DevTypes.DeleteById(id);
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
        /// <summary>
        /// 按大类型搜索
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult SearchByClass(string Class, int pageIndex = 1)
        {
            if (Class == "")
            {
                Class = null;
            }
            PagedList<DevType> lst = bll.DevTypes.DbSet.Where(p => (string.IsNullOrEmpty(Class) ? true : p.Class == Class)).OrderBy(p => p.TypeCode).ToPagedList<DevType>(pageIndex, pageSize);
            return View("Index", lst);
        }
        /// <summary>
        /// 按类型名称搜索
        /// </summary>
        /// <param name="TypeName"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult SearchByTypeName(string TypeName, int pageIndex = 1)
        {
            if (TypeName == "")
            {
                TypeName = null;
            }
            //GetListToViewBag();
            PagedList<DevType> lst = bll.DevTypes.DbSet.Where(p => (string.IsNullOrEmpty(TypeName) ? true : p.TypeName == TypeName)).OrderBy(p => p.TypeCode).ToPagedList<DevType>(pageIndex, pageSize);
            return View("Index", lst);
        }
        /// <summary>
        /// 按Typecode搜索
        /// </summary>
        /// <param name="Typecode"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult SearchByTypeCode(string Typecode,int pageIndex=1)
        {
            long? typeCodeTemp;
            try
            {
                typeCodeTemp = long.Parse(Typecode);
            }catch(Exception e)
            {
                typeCodeTemp = null;
            }
            PagedList<DevType> lst = bll.DevTypes.DbSet.Where(p => (typeCodeTemp == null ? true : p.TypeCode == typeCodeTemp)).OrderBy(p => p.TypeCode).ToPagedList<DevType>(pageIndex, pageSize);
            return View("Index", lst);
        }
    }
}
