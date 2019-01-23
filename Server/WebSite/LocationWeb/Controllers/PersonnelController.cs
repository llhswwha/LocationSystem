using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Location.BLL;
using Webdiyer.WebControls.Mvc;
using Location.Model;
using System.Net;
using WebLocation.Models;
using System.IO;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using ExcelLib;

using Location.IModel;
using WebLocation.Tools;
using DbModel.Location.Relation;
using Location.TModel.Tools;
using System.Globalization;
using LocationServices.Locations.Services;

namespace WebLocation.Controllers
{
    public class PersonnelController : Controller
    {
        private Bll bll = new Bll(false,true,true,true);
        private Bll db = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 1;

        // GET: Personnel
        public ActionResult Index(int pageIndex = 1)
        {
            var query1 = from t1 in bll.Personnels.DbSet
                         join t2 in bll.LocationCardToPersonnels.DbSet on t1.Id equals t2.PersonnelId
                         join t3 in bll.LocationCards.DbSet on t2.LocationCardId equals t3.Id
                         //select new Personnel { Id = t1.Id, Abutment_Id = t1.Abutment_Id, Name = t1.Name, LocationCardName = t3.Name, Sex = t1.Sex, Photo = t1.Photo, BirthDay = t1.BirthDay, BirthTimeStamp = t1.BirthTimeStamp, Nation = t1.Nation, Address = t1.Address, WorkNumber = t1.WorkNumber, Email = t1.Email, Phone = t1.Phone, Mobile = t1.Mobile, Enabled = t1.Enabled, ParentId = t1.ParentId, Pst = t1.Pst };
                         select new { Personnel = t1, LocationCard = t3 };
            var l1 = query1.ToList();
            List<Personnel> personList = new List<Personnel>();
            foreach (var item in l1)
            {
                var p = item.Personnel;
                if (item.LocationCard != null)
                {
                    p.LocationCardName = item.LocationCard.Name;
                }
                personList.Add(p);
            }

            PagedList<Personnel> lst = personList.ToPagedList<Personnel>(pageIndex, pageSize);
          
            //PagedList < Personnel > lst = bll.Personnels.ToList().ToPagedList<Personnel>(pageIndex, pageSize);
            GetListToViewBag();            
            return View(lst);
        }

        public ActionResult CardSet(int? id)//添加id，接收点击某一行的人员传递的参数
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var psl = bll.Personnels.Find(id);//获取当前人员
            if (psl == null)
            {
                return HttpNotFound();
            }

            LocationCardToPersonnel lcp = bll.LocationCardToPersonnels.Find(p => p.PersonnelId == id);
            ViewBag.CardId = null;
            if (lcp != null)
            {
                ViewBag.CardId = lcp.LocationCardId;
            }

            //设置相关信息给前台
            ViewBag.PersonId = id;
            //ViewBag.CardId = ;
            ViewBag.PersonName = psl.Name;

            List<LocationCard> locationCardList = db.LocationCards.ToList();
            return PartialView("CardSet", locationCardList);
        }

        public ActionResult SetLocardtionCard(int personId, int cardId)
        {
            var service = new PersonService();
            var result = service.BindWithTag(personId, cardId);
            
            return Json(new { success = result });
        }

        public ActionResult Search(string Name, int? ParentId, string Pst, int pageIndex = 1)
        {
            var query1 = from t1 in bll.Personnels.DbSet
                         join t2 in bll.LocationCardToPersonnels.DbSet on t1.Id equals t2.PersonnelId
                         join t3 in bll.LocationCards.DbSet on t2.LocationCardId equals t3.Id
                         //select new Personnel { Id = t1.Id, Abutment_Id = t1.Abutment_Id, Name = t1.Name, LocationCardName = t3.Name, Sex = t1.Sex, Photo = t1.Photo, BirthDay = t1.BirthDay, BirthTimeStamp = t1.BirthTimeStamp, Nation = t1.Nation, Address = t1.Address, WorkNumber = t1.WorkNumber, Email = t1.Email, Phone = t1.Phone, Mobile = t1.Mobile, Enabled = t1.Enabled, ParentId = t1.ParentId, Pst = t1.Pst };
                         select new { Personnel = t1, LocationCard = t3 };
            var l1 = query1.ToList();
            List<Personnel> personList = new List<Personnel>();
            foreach (var item in l1)
            {
                var p = item.Personnel;
                if (item.LocationCard != null)
                {
                    p.LocationCardName = item.LocationCard.Name;
                }
                personList.Add(p);
            }
                                   
            PagedList<Personnel> lst = bll.Personnels.DbSet.Where(p => ((string.IsNullOrEmpty(Name) || p.Name.Contains(Name)) && (ParentId == null || p.ParentId == ParentId) && (string.IsNullOrEmpty(Pst) || p.Pst == Pst))).OrderBy(p => p.ParentId).ToPagedList<Personnel>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnel Psl = bll.Personnels.Find(id);
            if (Psl == null)
            {
                return HttpNotFound();
            }
           
            return PartialView(Psl);
        }

        private void GetListToViewBag()
        {
            List<Department> DepList = bll.Departments.ToList();
            SelectList selList = new SelectList(DepList, "Id", "Name");
            ViewBag.DepList = selList.AsEnumerable();

            List<Post> PostList = bll.Posts.ToList();
            SelectList selList2 = new SelectList(PostList, "Name", "Name");
            ViewBag.PostList = selList2.AsEnumerable();
        }

        public ActionResult Create()
        {
            Personnel pel = new Personnel();
            GetListToViewBag();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Sex,Photo,BirthDay,Nation,Address,WorkNumber,Email,Phone,Mobile,Enabled,ParentId,Pst")] Personnel pel)
        {
            if (ModelState.IsValid)
            {
                pel.BirthTimeStamp = TimeConvert.DateTimeToTimeStamp(pel.BirthDay);

                var result = bll.Personnels.Add(pel);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Personnels.ErrorMessage });
                }
            }

            GetListToViewBag();
            return View(pel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Personnel pel = bll.Personnels.Find(id);

            if (pel == null)
            {
                return HttpNotFound();
            }

            GetListToViewBag();
            
            return PartialView(pel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Abutment_Id,Name,Sex,Photo,BirthDay,Nation,Address,WorkNumber,Email,Phone,Mobile,Enabled,ParentId,Pst")] Personnel pel)
        {
            if (ModelState.IsValid)
            {
                pel.BirthTimeStamp = TimeConvert.DateTimeToTimeStamp(pel.BirthDay);

                var result = bll.Personnels.Edit(pel);
                if (result)
                {
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Personnels.ErrorMessage });
                }
            }

            GetListToViewBag();
            return View(pel);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Personnel pel = bll.Personnels.Find(id);

            if (pel == null)
            {
                return HttpNotFound();
            }

            //return View(pel);
            return PartialView(pel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            bll.Personnels.DeleteById(id);

            

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