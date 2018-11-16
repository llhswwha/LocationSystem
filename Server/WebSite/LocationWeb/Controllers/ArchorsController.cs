using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BLL;
using DbModel.Location.AreaAndDev;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;
using BLL.Tools;
using DbModel.Tools;
using IModel.Enums;
namespace WebLocation.Controllers
{
    public class ArchorsController : Controller
    {
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 3;

        // GET: Archors
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<Archor> lst = bll.Archors.ToList().ToPagedList<Archor>(pageIndex, pageSize);

            return View("Index",lst);
        }

        // GET: Archors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archor archor = bll.Archors.Find(id);
            if (archor == null)
            {
                return HttpNotFound();
            }
           
            return PartialView(archor);
        }

        private void GetListToViewBag()
        {
            List<Area> areaList = bll.Areas.ToList();
            SelectList selList = new SelectList(areaList, "Id", "Name");
            IList<SelectListItem> TypeItem = EnumToList.EnumToListChoice<ArchorTypes>();
            IList<SelectListItem> StartItem = EnumToList.EnumToListChoice<IsStart>();

            ViewBag.selList = selList.AsEnumerable();
            ViewBag.TypeItem = TypeItem;
            ViewBag.StartItem = StartItem;
        }

        // GET: Archors/Create
        public ActionResult Create()
        {
            GetListToViewBag();
            return PartialView();
        }

        // POST: Archors/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,X,Y,Z,Type,IsAutoIp,Ip,ServerIp,ServerPort,Power,AliveTime,Enable,MapId")] Archor archor)
        {
            if (ModelState.IsValid)
            {
                DevInfo dev = new DevInfo();
                dev.Local_DevID = Guid.NewGuid().ToString();
                dev.IP = "";
                dev.KKS = "";
                dev.Name = archor.Name;
                dev.ModelName = TypeNames.Archor;
                dev.Status = 0;
                string DepID = Request["DepID"];
                if (DepID == "" || DepID == null)
                {
                    GetListToViewBag();
                    return View(archor);
                }
                dev.ParentId = Convert.ToInt32(Request["DepID"]);
                try
                {
                    dev.Local_TypeCode = TypeCodes.Archor;
                }
                catch (Exception e)
                {
                    dev.Local_TypeCode = 0;
                }
                dev.UserName = "admin";

                archor.DevInfo = dev;
                var result = bll.Archors.Add(archor);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Archors.ErrorMessage });
                }
            }
            GetListToViewBag();
            return View(archor);
        }

        // GET: Archors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archor archor = bll.Archors.Find(id);
            if (archor == null)
            {
                return HttpNotFound();
            }

            GetListToViewBag();
            return PartialView(archor);
        }

        // POST: Archors/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DevInfoId,Code,Name,X,Y,Z,Type,IsAutoIp,Ip,ServerIp,ServerPort,Power,AliveTime,Enable,MapId")] Archor archor)
        {
            if (ModelState.IsValid)
            {
                var result = bll.Archors.Edit(archor);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Archors.ErrorMessage });
                }
            }

            GetListToViewBag();
            return View(archor);
        }

        // GET: Archors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archor archor = bll.Archors.Find(id);
            if (archor == null)
            {
                return HttpNotFound();
            }
            
            return PartialView(archor);
        }

        // POST: Archors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bll.Archors.DeleteById(id);
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
