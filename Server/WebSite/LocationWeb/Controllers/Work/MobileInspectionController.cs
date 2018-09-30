using Location.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using BLL;
using DbModel.Location.Work;
namespace WebLocation.Controllers.Work
{
    public class MobileInspectionController : Controller
    {
        private Bll bll = new Bll();
        // GET: MobileInspection
        public ActionResult Index()
        {
            return View(bll.MobileInspections.ToList());
        }

        public ActionResult Create()
        {
            int nCount = bll.MobileInspections.DbSet.Count();
            int nOrder = 1;
            if (nCount != 0)
            {
                nOrder = bll.MobileInspections.DbSet.Max(p => p.nOrder);
                nOrder += 1;
            }

           
            MobileInspection MobileInspection = new MobileInspection();
            MobileInspection.nOrder = nOrder;

            return View(MobileInspection);
        }

        public ActionResult Create2(MobileInspection MobileInspection)
        {
            bll.MobileInspections.Add(MobileInspection);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileInspection MobileInspection = bll.MobileInspections.Find(id);
            if (MobileInspection == null)
            {
                return HttpNotFound();
            }

            return View(MobileInspection);
        }

        public ActionResult Edit2(MobileInspection MobileInspection)
        {
            if (MobileInspection != null)
            {
                bll.MobileInspections.Edit(MobileInspection);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileInspection MobileInspection = bll.MobileInspections.Find(id);
            if (MobileInspection == null)
            {
                return HttpNotFound();
            }

            return View(MobileInspection);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bll.MobileInspections.DeleteById(id);
            return RedirectToAction("Index");
        }


        




        private void GetListToViewBag()
        {
            List<MobileInspectionDev> devList = bll.MobileInspectionDevs.ToList();
            
            ViewBag.selList = devList;
        }

        //public ActionResult CreateItem(int id)
        //{
        //    int nCount = bll.MobileInspectionItems.DbSet.Where(p=>p.PID == id).Count();
        //    int nOrder = 1;
        //    if (nCount != 0)
        //    {
        //        nOrder = bll.MobileInspectionItems.DbSet.Where(p => p.PID == id).Max(p => p.nOrder);
        //        nOrder += 1;
        //    }

        //    MobileInspectionItem MobileInspectionItem = new MobileInspectionItem();
        //    MobileInspectionItem.PID = id;
        //    MobileInspectionItem.nOrder = nOrder;
        //    GetListToViewBag();
        //    return View(MobileInspectionItem);
        //}

        //public ActionResult CreateItem2(MobileInspectionItem MobileInspectionItem)
        //{
        //    int id = MobileInspectionItem.PID;
        //    string DevID = MobileInspectionItem.DevID;
        //    string strDevName = bll.DevInfos.DbSet.Where(p => p.DevID == DevID).Select(p=>p.Name).FirstOrDefault().ToString();
        //    MobileInspectionItem.DevName = strDevName;
        //    bll.MobileInspectionItems.Add(MobileInspectionItem);
        //    MobileInspection MobileInspection = bll.MobileInspections.Find(id);
        //    if (MobileInspection == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View("Edit",MobileInspection);
        //}

        //public ActionResult EditItem(int id)
        //{ 
        //    MobileInspectionItem MobileInspectionItem = bll.MobileInspectionItems.Find(id);
        //    if (MobileInspectionItem == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(MobileInspectionItem);
        //}

        //public ActionResult EditItem2(MobileInspectionItem MobileInspectionItem)
        //{
        //    int id = MobileInspectionItem.PID;
        //    bll.MobileInspectionItems.Edit(MobileInspectionItem);
        //    MobileInspection MobileInspection = bll.MobileInspections.Find(id);
        //    if (MobileInspection == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View("Edit", MobileInspection);
        //}

        //public ActionResult DetailItem(int id, string backTarget)
        //{
        //    MobileInspectionItem MobileInspectionItem = bll.MobileInspectionItems.Find(id);
        //    MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(MobileInspectionItem.DevID);
        //    if (MobileInspectionItem == null || MobileInspectionDev == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    ViewBag.Dev = MobileInspectionDev;
        //    ViewBag.backTarget = backTarget;
        //    return View(MobileInspectionItem);
        //}


        //public ActionResult DeleteItem(int? id, int pid)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    bll.MobileInspectionItems.DeleteById(id);

        //    MobileInspection MobileInspection = bll.MobileInspections.Find(pid);
        //    if (MobileInspection == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View("Edit", MobileInspection);
        //}





        //public ActionResult IndexContentDev()
        //{
        //    List<MobileInspectionDev> devList = bll.MobileInspectionDevs.ToList();
        //    List<DevInfo> devList2 = bll.DevInfos.DbSet.ToList();

        //    ViewBag.devList = devList2;
        //    return View(devList);
        //}

        //public ActionResult CreateContentDev(MobileInspectionDev MobileInspectionDev)
        //{
        //    string DevID = MobileInspectionDev.DevID;

        //    MobileInspectionDev MobileInspectionDev2 = bll.MobileInspectionDevs.Find(DevID);
        //    if (MobileInspectionDev2 == null)
        //    {
        //        DevInfo DevInfo = bll.DevInfos.Find(DevID);
        //        MobileInspectionDev.Name = DevInfo.Name;

        //        bll.MobileInspectionDevs.Add(MobileInspectionDev);
        //    }

        //    List<MobileInspectionDev> devList = bll.MobileInspectionDevs.ToList();
        //    List<DevInfo> devList2 = bll.DevInfos.DbSet.ToList();

        //    ViewBag.devList = devList2;
        //    return View("IndexContentDev", devList);
        //}

        //public ActionResult EditContentDev(string id)
        //{
        //    MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(id);

        //    return View(MobileInspectionDev);
        //}

        //public ActionResult EditContentDev2(MobileInspectionDev MobileInspectionDev)
        //{
        //    bll.MobileInspectionDevs.Edit(MobileInspectionDev);
        //    List<MobileInspectionDev> devList = bll.MobileInspectionDevs.ToList();
        //    List<DevInfo> devList2 = bll.DevInfos.DbSet.ToList();

        //    ViewBag.devList = devList2;
        //    return View("IndexContentDev", devList);
        //}

        //public ActionResult DetailContentDev(string id)
        //{
        //    MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(id);

        //    return View(MobileInspectionDev);
        //}


        //public ActionResult DeleteContentDev(string id)
        //{
        //    MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(id);
        //    List<int> lst = new List<int>();
        //    foreach (MobileInspectionContent MobileInspectionContent in MobileInspectionDev.MobileInspectionContents)
        //    {
        //        lst.Add(MobileInspectionContent.Id);
        //    }
            
        //    foreach (int mId in lst)
        //    {
        //        bll.MobileInspectionContents.DeleteById(mId);
        //    }

        //    bll.MobileInspectionDevs.DeleteById(id);

        //    List<MobileInspectionDev> devList = bll.MobileInspectionDevs.ToList();
        //    List<DevInfo> devList2 = bll.DevInfos.DbSet.ToList();

        //    ViewBag.devList = devList2;
        //    return View("IndexContentDev", devList);
        //}

















        //public ActionResult CreateContent(string id)
        //{
        //    MobileInspectionContent MobileInspectionContent = new MobileInspectionContent();
        //    MobileInspectionContent.ParentDevID = id;

        //    int nCount = bll.MobileInspectionContents.DbSet.Where(p => p.ParentDevID == id).Count();
        //    int nOrder = 1;
        //    if (nCount != 0)
        //    {
        //        nOrder = bll.MobileInspectionContents.DbSet.Where(p => p.ParentDevID == id).Max(p => p.nOrder);
        //        nOrder += 1;
        //    }

        //    MobileInspectionContent.nOrder = nOrder;

        //    return View(MobileInspectionContent);
        //}

        //public ActionResult CreateContent2(MobileInspectionContent MobileInspectionContent)
        //{
        //    string DevID = MobileInspectionContent.ParentDevID;
        //    bll.MobileInspectionContents.Add(MobileInspectionContent);
        //    MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(DevID);
        //    return View("EditContentDev",MobileInspectionDev);
        //}

        //public ActionResult EditContent(int id)
        //{
        //    MobileInspectionContent MobileInspectionContent = bll.MobileInspectionContents.Find(id);
        //    return View(MobileInspectionContent);
        //}

        //public ActionResult EditContent2(MobileInspectionContent MobileInspectionContent)
        //{
        //    string DevID = MobileInspectionContent.ParentDevID;
        //    bll.MobileInspectionContents.Edit(MobileInspectionContent);
        //    MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(DevID);
        //    return View("EditContentDev", MobileInspectionDev);
        //}

        //public ActionResult DeleteContent(int id, string pid)
        //{
        //    bll.MobileInspectionContents.DeleteById(id);
        //    MobileInspectionDev MobileInspectionDev = bll.MobileInspectionDevs.Find(pid);
        //    return View("EditContentDev", MobileInspectionDev);
        //}
    }
}