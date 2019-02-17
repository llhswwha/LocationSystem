using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Location.BLL;
using System.Net;
using Location.Model;
using System.Reflection;
using BLL;
using DbModel.Location.AreaAndDev;
using ExcelLib;

using Location.IModel;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;
using DbModel.Tools;

namespace WebLocation.Controllers
{

    public class AreaController : Controller
    {
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 3;

        // GET: PhysicalTopology
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<Area> lst = bll.Areas.ToList().ToPagedList<Area>(pageIndex, pageSize);
       
            return View("Index", lst);
        }

        private void GetListToViewBag(Area pht)
        {
            List<Area> phts = bll.Areas.ToList();
            if (pht != null)
            {
                Area pht0 = phts.Find(i => i.Id == pht.Id);
                if (pht0 != null)
                {
                    phts.Remove(pht0);//删除本身，编辑时不能让自己成为自己的子节点
                }
            }
            SelectList list = new SelectList(phts, "Id", "Name");
            ViewBag.List = list.AsEnumerable();

            ViewBag.EnumList = EnumToList.EnumToListChoice<AreaTypes>();            
        }

        public ActionResult Tree()
        {
            var root = bll.GetAreaTree(false);//主动用代码建立Children关系
            //var root = bll.Areas.GetRoot();//这里会为空
            
            return View(root);
        }      

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pht = bll.Areas.Find(id);
            if (pht == null)
            {
                return HttpNotFound();
            }
            //return View(pht);
            return PartialView(pht);
        }


        public ActionResult Create()
        {
            GetListToViewBag(null);
            
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,KKS,ParentId,Number,Type,Describe,InitBoundId")] Area pht)
        {
            if (ModelState.IsValid)
            {
                bool r3 = bll.Areas.Add(pht);
                
                bool result =  r3 ;
                
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = "添加节点出错！" });
                }
            }

            GetListToViewBag(pht);
            return View(pht);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pht = bll.Areas.Find(id);
            if (pht == null)
            {
                return HttpNotFound();
            }

            GetListToViewBag(pht);
          
            return PartialView(pht);
        }

        // POST: Departments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Abutment_Id,Abutment_ParentId,Name,KKS,ParentId,Number,Type,Describe,InitBoundId")] Area pht)
        {
            if (ModelState.IsValid)
            {
                bool r3 = bll.Areas.Edit(pht);

                bool result =   r3;
                
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = "添加节点出错！" });
                }
            }

            GetListToViewBag(pht);
            return View(pht);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pht = bll.Areas.Find(id);
            if (pht == null)
            {
                return HttpNotFound();
            }
            //return View(pht);
            return PartialView(pht);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pht = bll.Areas.Find(id);

            bll.Areas.DeleteById(id);
            //bll.TransformMs.DeleteById(pht.TransfromId);

            NodeKKS nodeKKS = bll.NodeKKSs.DbSet.FirstOrDefault(i => i.NodeId == id);
            if (nodeKKS != null)
            {
                bll.NodeKKSs.DbSet.Remove(nodeKKS);
            }

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