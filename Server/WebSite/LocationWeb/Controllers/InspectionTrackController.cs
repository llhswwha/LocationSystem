using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Location.BLL;
using System.Net;
using BLL;
using DbModel.Location.Work;
using DbModel.LocationHistory.Work;

namespace WebLocation.Controllers
{
    public class InspectionTrackController : Controller
    {
        private Bll bll = new Bll();

        // GET: InspectionTrack
        public ActionResult Index()
        {
            return View(bll.InspectionTracks.ToList());
        }        

        //GET: InspectionTrack/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InspectionTrack InspectionTrack = bll.InspectionTracks.Find(id);
            if (InspectionTrack == null)
            {
                return HttpNotFound();
            }

            return View(InspectionTrack);
        }

        //GET: InspectionTrack/ItemDetails
        public ActionResult ItemDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatrolPoint PatrolPoint = bll.PatrolPoints.Find(id);
            if (PatrolPoint == null)
            {
                return HttpNotFound();
            }

            return View(PatrolPoint);
        }

        //GET: InspectionTrack/Create
        public ActionResult Create()
        {
            InspectionTrack InspectionTrack = new InspectionTrack();            
            return View(InspectionTrack);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InspectionTrack InspectionTrack)
        {
            if (ModelState.IsValid)
            {
                InspectionTrack.dtCreateTime = DateTime.Now;                
                bll.InspectionTracks.Add(InspectionTrack);                
            }
            InspectionTrack.Route = new List<PatrolPoint>();
            //return View(InspectionTrack);
            return RedirectToAction("Index");
        }

        //GET: InspectionTrack/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InspectionTrack InspectionTrack = bll.InspectionTracks.Find(id);
            if (InspectionTrack == null)
            {
                return HttpNotFound();
            }
            return View(InspectionTrack);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InspectionTrack InspectionTrack)
        {
            if (ModelState.IsValid)
            {                
                bll.InspectionTracks.Edit(InspectionTrack);                
            }
            //return View(InspectionTrack);
            return RedirectToAction("Index");
        }

        //GET: InspectionTrack/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bll.InspectionTracks.DeleteById(id);            
            return RedirectToAction("Index");
        }

        public ActionResult Finish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InspectionTrack ist = bll.InspectionTracks.Find(id);
            InspectionTrackHistory isth = new InspectionTrackHistory();
            isth.Id = ist.Id;
            isth.Code = ist.Code;
            isth.Name = ist.Name;
            isth.dtCreateTime = ist.dtCreateTime;
            isth.State = ist.State;
            isth.dtStartTime = ist.dtStartTime;
            isth.dtEndTime = ist.dtEndTime;

            List<PatrolPointHistory> lst = new List<PatrolPointHistory>();
            foreach (PatrolPoint item in ist.Route)
            {
                PatrolPointHistory pph = new PatrolPointHistory();
                pph.Id = item.Id;
                pph.ParentId = item.ParentId;
                pph.StaffCode = item.StaffCode;
                pph.StaffName = item.StaffName;
                pph.KksCode = item.KksCode;
                pph.DevName = item.DevName;
            }
            isth.Route = lst;

            bll.InspectionTracks.DeleteById(id);
            bll.InspectionTrackHistorys.Add(isth);

            return RedirectToAction("Index");
        }

        //GET: InspectionTrack/CreatePoint
        public ActionResult CreatePoint(int id)
        {          
            PatrolPoint PatrolPoint = new PatrolPoint();
            PatrolPoint.ParentId = id;
            return View(PatrolPoint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePoint(PatrolPoint PatrolPoint)
        {
            int Id = PatrolPoint.ParentId;
            if (ModelState.IsValid)
            {
                bll.PatrolPoints.Add(PatrolPoint);
            }

            InspectionTrack InspectionTrack = bll.InspectionTracks.Find(Id);
            if (InspectionTrack == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Edit", InspectionTrack);
        }

        //GET: InspectionTrack/EditPoint
        public ActionResult EditPoint(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatrolPoint PatrolPoint = bll.PatrolPoints.Find(id);
            if (PatrolPoint == null)
            {
                return HttpNotFound();
            }

            return View(PatrolPoint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPoint(PatrolPoint PatrolPoint)
        {
            int Id = PatrolPoint.ParentId;
            if (ModelState.IsValid)
            {
                bll.PatrolPoints.Edit(PatrolPoint);
            }

            InspectionTrack InspectionTrack = bll.InspectionTracks.Find(Id);
            if (InspectionTrack == null)
            {
                return HttpNotFound();
            }
            PatrolPoint.Checks = new List<PatrolPointItem>();
            return RedirectToAction("Edit", InspectionTrack);
        }

        //GET: InspectionTrack/DeletePoint
        public ActionResult DeletePoint(int? id, int ParentId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bll.PatrolPoints.DeleteById(id);
            InspectionTrack InspectionTrack = bll.InspectionTracks.Find(ParentId);
            if (InspectionTrack == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Edit", InspectionTrack);
        }

        //GET: InspectionTrack/CreateItem
        public ActionResult CreateItem(int id)
        {
            PatrolPointItem PatrolPointItem = new PatrolPointItem();
            PatrolPointItem.ParentId = id;
            return View(PatrolPointItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateItem(PatrolPointItem PatrolPointItem)
        {          
            int Id = PatrolPointItem.ParentId;
            if (ModelState.IsValid)
            {               
                bll.PatrolPointItems.Add(PatrolPointItem);
            }
            PatrolPoint PatrolPoint = bll.PatrolPoints.Find(Id);
            //InspectionTrack InspectionTrack = bll.InspectionTracks.Find(Id);
            if (PatrolPoint == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("EditPoint", PatrolPoint);
        }

        //GET: InspectionTrack/EditItem
        public ActionResult EditItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatrolPointItem PatrolPointItem = bll.PatrolPointItems.Find(id);
            if (PatrolPointItem == null)
            {
                return HttpNotFound();
            }

            return View(PatrolPointItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(PatrolPointItem PatrolPointItem)
        {
            int Id = PatrolPointItem.ParentId;
            if (ModelState.IsValid)
            {
                bll.PatrolPointItems.Edit(PatrolPointItem);
            }
            PatrolPoint PatrolPoint = bll.PatrolPoints.Find(Id);
            //InspectionTrack InspectionTrack = bll.InspectionTracks.Find(Id);
            if (PatrolPoint == null)
            {
                return HttpNotFound();
            }            
            return RedirectToAction("EditPoint", PatrolPoint);
        }

        //GET: InspectionTrack/Clock
        public ActionResult Clock(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatrolPointItem PatrolPointItem = bll.PatrolPointItems.Find(id);
            if (PatrolPointItem == null)
            {
                return HttpNotFound();
            }

            return View(PatrolPointItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Clock(PatrolPointItem PatrolPointItem)
        {
            int Id = PatrolPointItem.ParentId;
            if (ModelState.IsValid)
            {
                PatrolPointItem.dtCheckTime = DateTime.Now;
                bll.PatrolPointItems.Edit(PatrolPointItem);
            }
            PatrolPoint PatrolPoint = bll.PatrolPoints.Find(Id);
            //InspectionTrack InspectionTrack = bll.InspectionTracks.Find(Id);
            if (PatrolPoint == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("EditPoint", PatrolPoint);
        }

        //GET: InspectionTrack/DeleteItem
        public ActionResult DeleteItem(int? id, int ParentId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bll.PatrolPointItems.DeleteById(id);
            PatrolPoint PatrolPoint = bll.PatrolPoints.Find(ParentId);
            if (PatrolPoint == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("EditPoint", PatrolPoint);
        }
    }
}