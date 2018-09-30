using Location.BLL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Work;

namespace WebLocation.Controllers.Work
{
    public class MobileInspectionDevController : Controller
    {
        private Bll bll = new Bll();

        // GET: MobileInspectionDev
       
        public ActionResult Index()
        {
            List<MobileInspectionDev> devList = bll.MobileInspectionDevs.ToList();
            List<DevInfo> devList2 = bll.DevInfos.DbSet.ToList();

            ViewBag.devList = devList2;
            return View(devList);
        }

        public ActionResult Create(MobileInspectionDev MobileInspectionDev)
        {
            int DevID = MobileInspectionDev.Id;

            MobileInspectionDev MobileInspectionDev2 = bll.MobileInspectionDevs.Find(DevID);
            if (MobileInspectionDev2 == null)
            {
                DevInfo DevInfo = bll.DevInfos.Find(DevID);
                MobileInspectionDev.Name = DevInfo.Name;

                bll.MobileInspectionDevs.Add(MobileInspectionDev);
            }

            return RedirectToAction("Index"); 
        }

        public ActionResult Edit(string id)
        {
            MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(id);

            return View(MobileInspectionDev);
        }

        public ActionResult Edit2(MobileInspectionDev MobileInspectionDev)
        {
            bll.MobileInspectionDevs.Edit(MobileInspectionDev);
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(id);

            return View(MobileInspectionDev);
        }


        public ActionResult Delete(int id)
        {
            MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(id);
            List<int> lst = new List<int>();
            foreach (MobileInspectionContent MobileInspectionContent in MobileInspectionDev.MobileInspectionContents)
            {
                lst.Add(MobileInspectionContent.Id);
            }

            foreach (int mId in lst)
            {
                bll.MobileInspectionContents.DeleteById(mId);
            }

            bll.MobileInspectionDevs.DeleteById(id);
            int ItemId = bll.MobileInspectionItems.DbSet.Where(p => p.Id == id).Select(p => p.Id).FirstOrDefault();
            bll.MobileInspectionItems.DeleteById(ItemId);

            return RedirectToAction("Index");
        }





    }
}