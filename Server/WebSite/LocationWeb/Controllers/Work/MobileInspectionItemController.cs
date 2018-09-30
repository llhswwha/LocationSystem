using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BLL;
using DbModel.Location.Work;

namespace WebLocation.Controllers.Work
{
    public class MobileInspectionItemController : Controller
    {
        private Bll bll = new Bll();

        // GET: MobileInspectionItem
        public ActionResult Index()
        {
            return RedirectToAction("Detail", "MobileInspection",new { id = 1});
        }

        private void GetListToViewBag()
        {
            List<MobileInspectionDev> devList = bll.MobileInspectionDevs.ToList();

            ViewBag.selList = devList;
        }

        public ActionResult Create(int id)
        {
            int nCount = bll.MobileInspectionItems.DbSet.Where(p => p.PID == id).Count();
            int nOrder = 1;
            if (nCount != 0)
            {
                nOrder = bll.MobileInspectionItems.DbSet.Where(p => p.PID == id).Max(p => p.nOrder);
                nOrder += 1;
            }

            MobileInspectionItem MobileInspectionItem = new MobileInspectionItem();
            MobileInspectionItem.PID = id;
            MobileInspectionItem.nOrder = nOrder;
            GetListToViewBag();
            return View(MobileInspectionItem);
        }

        public ActionResult Create2(MobileInspectionItem MobileInspectionItem)
        {
            int id = MobileInspectionItem.PID;
            var DevID = MobileInspectionItem.DevId;
            string strDevName = bll.DevInfos.DbSet.Where(p => p.Id == DevID).Select(p => p.Name).FirstOrDefault().ToString();
            MobileInspectionItem.DevName = strDevName;
            bll.MobileInspectionItems.Add(MobileInspectionItem);
           
            return RedirectToAction("Edit", "MobileInspection", new { id = id });
        }

        public ActionResult Edit(int id)
        {
            MobileInspectionItem MobileInspectionItem = bll.MobileInspectionItems.Find(id);
            if (MobileInspectionItem == null)
            {
                return HttpNotFound();
            }

            return View(MobileInspectionItem);
        }

        public ActionResult Edit2(MobileInspectionItem MobileInspectionItem)
        {
            int id = MobileInspectionItem.PID;
            bll.MobileInspectionItems.Edit(MobileInspectionItem);

            return RedirectToAction("Edit", "MobileInspection", new { id = id });
        }

        public ActionResult Detail(int id, string backTarget)
        {
            MobileInspectionItem MobileInspectionItem = bll.MobileInspectionItems.Find(id);
            MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(MobileInspectionItem.DevId);
            if (MobileInspectionItem == null || MobileInspectionDev == null)
            {
                return HttpNotFound();
            }

            ViewBag.Dev = MobileInspectionDev;
            ViewBag.backTarget = backTarget;
            return View(MobileInspectionItem);
        }


        public ActionResult Delete(int? id, int pid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bll.MobileInspectionItems.DeleteById(id);

            return RedirectToAction("Edit", "MobileInspection", new { id = pid });
        }
    }
}