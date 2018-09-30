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
    public class WorkTicketController : Controller
    {
        private Bll bll = new Bll();
        // GET: WorkTicket
        public ActionResult Index()
        {
            return View(bll.WorkTickets.ToList());
        }
        
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkTicket WorkTicket = bll.WorkTickets.Find(id);
            if (WorkTicket == null)
            {
                return HttpNotFound();
            }

            return View(WorkTicket);
        }

        public ActionResult Create()
        {
            WorkTicket WorkTicket = new WorkTicket();
            WorkTicket.StartTimeOfPlannedWork = DateTime.Now;
            WorkTicket.EndTimeOfPlannedWork = DateTime.Now;
            WorkTicket.SafetyMeasuress = new List<SafetyMeasures>();

            return View(WorkTicket);
        }

        public ActionResult CreateTicket(WorkTicket WorkTicket)
        {
            if (WorkTicket != null)
            {
                bll.WorkTickets.Add(WorkTicket);
                return RedirectToAction("Index");
            }

            WorkTicket = new WorkTicket();
            WorkTicket.StartTimeOfPlannedWork = DateTime.Now;
            WorkTicket.EndTimeOfPlannedWork = DateTime.Now;
            WorkTicket.SafetyMeasuress = new List<SafetyMeasures>();

            return RedirectToAction("Create", WorkTicket);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTicket WorkTicket = bll.WorkTickets.Find(id);
            if (WorkTicket == null)
            {
                return HttpNotFound();
            }

            return View(WorkTicket);

        }

        public ActionResult EditTicket(WorkTicket WorkTicket)
        {
            if (WorkTicket != null)
            {
                bll.WorkTickets.Edit(WorkTicket);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bll.WorkTickets.DeleteById(id);
            return RedirectToAction("Index");
        }

        public ActionResult Finish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkTicket wt = bll.WorkTickets.Find(id);
            WorkTicketHistory wth = new WorkTicketHistory();
            wth.Id = wt.Id;
            wth.No = wt.No;
            wth.PersonInCharge = wt.PersonInCharge;
            wth.HeadOfWorkClass = wt.HeadOfWorkClass;
            wth.WorkPlace = wt.WorkPlace;
            wth.JobContent = wt.JobContent;
            
            wth.StartTimeOfPlannedWork = wt.StartTimeOfPlannedWork;
            wth.EndTimeOfPlannedWork = wt.EndTimeOfPlannedWork;
            wth.WorkCondition = wt.WorkCondition;
            wth.Lssuer = wt.Lssuer;
            wth.Licensor = wt.Licensor;
            wth.Approver = wt.Approver;
            wth.Comment = wt.Comment;

            List<SafetyMeasuresHistory> lst = new List<SafetyMeasuresHistory>();

            foreach (SafetyMeasures sm in wt.SafetyMeasuress)
            {
                SafetyMeasuresHistory smh = new SafetyMeasuresHistory();
                smh.Id = sm.Id;
                smh.No = sm.No;
                smh.LssuerContent = sm.LssuerContent;
                smh.LicensorContent = sm.LicensorContent;
                smh.WorkTicketId = sm.WorkTicketId;
                lst.Add(smh);
            }

            wth.SafetyMeasuress = lst;
            

            bll.WorkTickets.DeleteById(id);
            bll.WorkTicketHistorys.Add(wth);

            return RedirectToAction("Index");
        }

        public ActionResult CreateSafetyMeasures(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SafetyMeasures SafetyMeasures = new SafetyMeasures();
            SafetyMeasures.WorkTicketId = id;
            return View(SafetyMeasures);
        }

        public ActionResult CreateSafetyMeasures2(SafetyMeasures SafetyMeasures)
        {
            int? Id = SafetyMeasures.WorkTicketId;
            if (SafetyMeasures != null)
            {
                bll.SafetyMeasuress.Add(SafetyMeasures);
            }

            WorkTicket WorkTicket = bll.WorkTickets.Find(Id);
            if (WorkTicket == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Edit", WorkTicket);
        }

        public ActionResult EditSafetyMeasures(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyMeasures SafetyMeasures = bll.SafetyMeasuress.Find(id);
            if (SafetyMeasures == null)
            {
                return HttpNotFound();
            }

            return View(SafetyMeasures);
        }

        public ActionResult EditSafetyMeasures2(SafetyMeasures SafetyMeasures)
        {
            int? Id = SafetyMeasures.WorkTicketId;
            if (SafetyMeasures != null)
            {
                bll.SafetyMeasuress.Edit(SafetyMeasures);
            }

            WorkTicket WorkTicket = bll.WorkTickets.Find(Id);
            if (WorkTicket == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Edit", WorkTicket);
        }

        public ActionResult DeleteSafetyMeasures(int? id, int? TicketId)
        {
            if (id == null || TicketId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bll.SafetyMeasuress.DeleteById(id);
            WorkTicket WorkTicket = bll.WorkTickets.Find(TicketId);
            if (WorkTicket == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Edit", WorkTicket);
        }












    }
}