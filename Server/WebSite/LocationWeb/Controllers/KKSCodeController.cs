using Location.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ExcelLib;
using System.Data;

using System.Net;
using BLL;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Location.IModel;
using Webdiyer.WebControls.Mvc;

using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class KKSCodeController : Controller
    {
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 20;

        // GET: KKSCode
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<KKSCode> lst = bll.KKSCodes.ToList().ToPagedList<KKSCode>(pageIndex, pageSize);
            GetListToViewBag();
            return View(lst);
        }

        public ActionResult Import()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            string strFolder = strPath + "Data\\中电四会部件级KKS编码2017.5.24";

            ImportKKSFromDirectory(strFolder);

            return RedirectToAction("Index");
        }

        private void ImportKKSFromDirectory(string folderPath)
        {
            //导入到定位数据库中
            List<KKSCode> kksList = bll.KKSCodes.ToList();
            KKSImportInfo<KKSCode> importInfo1 = KKSCodeHelper.ImportKKSFromDirectory<KKSCode>(folderPath, kksList);
            bll.KKSCodes.EditRange(bll.Db, importInfo1.listEditInfo);//修改的部分
            bll.KKSCodes.AddRange(bll.Db, importInfo1.listAddInfo);//新增的部分
        }

        private void GetListToViewBag()
        {
            List<KKSCode> kksList = bll.KKSCodes.ToList();
            List<string> lstMain = kksList.Select(p => p.MainType).Distinct().ToList();
            List<string> lstSub = kksList.Select(p => p.SubType).Distinct().ToList();
            List<string> lstSystem = kksList.Select(p => p.System).Distinct().ToList();
            SelectList slistMain = new SelectList(lstMain);
            SelectList slistSub = new SelectList(lstSub);
            SelectList slistSystem = new SelectList(lstSystem);
            ViewBag.MainType = slistMain.Distinct().AsEnumerable();
            ViewBag.SubType = slistSub.Distinct().AsEnumerable();
            ViewBag.System = slistSystem.Distinct().AsEnumerable();
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Serial,Name,Code,ParentCode,DesinCode,MainType,SubType,System")] KKSCode kks)
        {
            if (ModelState.IsValid)
            {
                KKSCode Self = bll.KKSCodes.DbSet.Where(p => p.Code == kks.Code).FirstOrDefault();
                if (Self != null)
                {
                    return Json(new { success = false, errors = "这个KKS码已存在" });
                }

                if (kks.ParentCode != null)
                {
                    KKSCode Parent = bll.KKSCodes.DbSet.Where(p => p.Code == kks.ParentCode).FirstOrDefault();
                    if (Parent == null)
                    {
                        return Json(new { success = false, errors = "这个上级KKS码不存在" });
                    }
                }
                
                var result = bll.KKSCodes.Add(kks);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.KKSCodes.ErrorMessage });
                }
            }
            
            return View(kks);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KKSCode kks = bll.KKSCodes.Find(id);

            if (kks == null)
            {
                return HttpNotFound();
            }

            return PartialView(kks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Serial,Name,Code,ParentCode,DesinCode,MainType,SubType,System")] KKSCode kks)
        {
            if (ModelState.IsValid)
            {
                if (kks.ParentCode != null)
                {
                    KKSCode Parent = bll.KKSCodes.DbSet.Where(p => p.Code == kks.ParentCode).FirstOrDefault();
                    if (Parent == null)
                    {
                        return Json(new { success = false, errors = "这个上级KKS码不存在" });
                    }
                }

                var result = bll.KKSCodes.Edit(kks);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.KKSCodes.ErrorMessage });
                }
            }

            return View(kks);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KKSCode kks = bll.KKSCodes.Find(id);

            if (kks == null)
            {
                return HttpNotFound();
            }

            return PartialView(kks);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KKSCode kks = bll.KKSCodes.Find(id);

            if (kks == null)
            {
                return HttpNotFound();
            }

            return PartialView(kks);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KKSCode kks = bll.KKSCodes.Find(id);
            string Code = kks.Code.ToString();
            string System = kks.System.ToString();

            bll.KKSCodes.DeleteById(id);

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

        public ActionResult Search(string MainType, string SubType, string System, int pageIndex = 1)
        {
            if (MainType == "")
            {
                MainType = null;
            }

            if (SubType == "")
            {
                SubType = null;
            }

            GetListToViewBag();
            PagedList<KKSCode> lst = bll.KKSCodes.DbSet.Where(p => (string.IsNullOrEmpty(MainType) ? true : p.MainType == MainType) && (string.IsNullOrEmpty(SubType) ? true : p.SubType == SubType) && (string.IsNullOrEmpty(System) ? true : p.System == System)).OrderBy(p=>p.MainType).ToPagedList<KKSCode>(pageIndex, pageSize);
                                                                                                                                                                                
            return View("Index",lst);
        }
    }
}