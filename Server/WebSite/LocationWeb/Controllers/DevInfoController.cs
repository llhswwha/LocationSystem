using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class DevInfoController : Controller
    {
        private Bll bll = new Bll();       
        private int pageSize = StaticArgs.DefaultPageSize;

        // GET: DevInfo
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<DevInfo> lst = bll.DevInfos.ToList().ToPagedList<DevInfo>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
        }

        private void GetListToViewBag()
        {
            List<Area> Pts = bll.Areas.ToList();
            SelectList PtList = new SelectList(Pts, "Id", "Name");
            ViewBag.PtList = PtList.AsEnumerable();           

            ViewBag.Abutment_DevTypesList = EnumToList.EnumToListChoice<Abutment_DevTypes>();
            ViewBag.Abutment_StatusList = EnumToList.EnumToListChoice<Abutment_Status>();
            ViewBag.Abutment_RunStatusList = EnumToList.EnumToListChoice<Abutment_RunStatus>();
        }

        // GET: DevInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevInfo devInfo = bll.DevInfos.Find(id);
            if (devInfo == null)
            {
                return HttpNotFound();
            }         
            return PartialView(devInfo);
        }

        //GET: DevInfo/Create
        public ActionResult Create()
        {
            GetListToViewBag();
            return PartialView();
        }

        // POST: DevInfo/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DevInfo devInfo)
        {
            if (ModelState.IsValid)
            {                      
                var result = bll.DevInfos.Add(devInfo);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.DevInfos.ErrorMessage });
                }
            }
            GetListToViewBag();
            return View(devInfo);
        }

        // GET: DevInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var devInfo = bll.DevInfos.Find(id);
            if (devInfo == null)
            {
                return HttpNotFound();
            }       
            GetListToViewBag();
            return PartialView(devInfo);
        }

        // POST: DevInfo/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DevInfo devInfo)
        {
            if (ModelState.IsValid)
            {
                var result = bll.DevInfos.Edit(devInfo);
                if (result)
                {                   
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, erroes = bll.DevInfos.ErrorMessage });
                }
            }
            GetListToViewBag();
            return View(devInfo);
        }

        // GET: DevInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevInfo devInfo = bll.DevInfos.Find(id);
            if (devInfo == null)
            {
                return HttpNotFound();
            }
            return PartialView(devInfo);
        }

        // POST: DevInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bll.DevInfos.DeleteById(id);   
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