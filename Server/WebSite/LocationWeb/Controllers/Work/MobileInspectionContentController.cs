using Location.BLL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DbModel.Location.Work;

namespace WebLocation.Controllers.Work
{
    public class MobileInspectionContentController : Controller
    {
        private Bll bll = new Bll();
        // GET: MobileInspectionContent
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create(int id)
        {
            MobileInspectionContent MobileInspectionContent = new MobileInspectionContent();
            MobileInspectionContent.ParentId = id;

            int nCount = bll.MobileInspectionContents.DbSet.Where(p => p.ParentId == id).Count();
            int nOrder = 1;
            if (nCount != 0)
            {
                nOrder = bll.MobileInspectionContents.DbSet.Where(p => p.ParentId == id).Max(p => p.nOrder);
                nOrder += 1;
            }

            MobileInspectionContent.nOrder = nOrder;

            return View(MobileInspectionContent);
        }

        public ActionResult Create2(MobileInspectionContent MobileInspectionContent)
        {
            var DevID = MobileInspectionContent.ParentId;
            bll.MobileInspectionContents.Add(MobileInspectionContent);

            return RedirectToAction("Edit", "MobileInspectionDev", new { id = DevID });
        }

        public ActionResult Edit(int id)
        {
            MobileInspectionContent MobileInspectionContent = bll.MobileInspectionContents.Find(id);
            return View(MobileInspectionContent);
        }

        public ActionResult Edit2(MobileInspectionContent MobileInspectionContent)
        {
            var DevID = MobileInspectionContent.ParentId;
            bll.MobileInspectionContents.Edit(MobileInspectionContent);

            return RedirectToAction("Edit", "MobileInspectionDev", new { id = DevID });
        }

        public ActionResult Delete(int id, string pid)
        {
            bll.MobileInspectionContents.DeleteById(id);

            return RedirectToAction("Edit", "MobileInspectionDev", new { id = pid });
        }

    }
}