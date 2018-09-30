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
    public class OperationTicketController : Controller
    {
        private Bll bll = new Bll();

        // GET: OperationTicket
        public ActionResult Index()
        {
            return View(bll.OperationTickets.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OperationTicket OperationTicket = bll.OperationTickets.Find(id);
            if (OperationTicket == null)
            {
                return HttpNotFound();
            }

            return View(OperationTicket);
        }

        public ActionResult Create()
        {
            OperationTicket OperationTicket = new OperationTicket();
            OperationTicket.OperationStartTime = DateTime.Now;
            OperationTicket.OperationEndTime = DateTime.Now;
            OperationTicket.OperationItems = new List<OperationItem>();
           
            return View(OperationTicket);
        }

        public ActionResult CreateTicket(OperationTicket OperationTicket)
        {
            if (OperationTicket != null)
            {
                bll.OperationTickets.Add(OperationTicket);
                return RedirectToAction("Index");
            }

            OperationTicket = new OperationTicket();
            OperationTicket.OperationStartTime = DateTime.Now;
            OperationTicket.OperationEndTime = DateTime.Now;
            OperationTicket.OperationItems = new List<OperationItem>();

            return RedirectToAction("Create", OperationTicket);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OperationTicket OperationTicket = bll.OperationTickets.Find(id);
            if (OperationTicket == null)
            {
                return HttpNotFound();
            }

            return View(OperationTicket);

        }

        public ActionResult EditTicket(OperationTicket OperationTicket)
        {
            if (OperationTicket != null)
            {
                bll.OperationTickets.Edit(OperationTicket);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bll.OperationTickets.DeleteById(id);
            return RedirectToAction("Index");
        }

        public ActionResult Finish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OperationTicket ot = bll.OperationTickets.Find(id);
            OperationTicketHistory oth = new OperationTicketHistory();
            oth.Id = ot.Id;
            oth.No = ot.No;
            oth.OperationTask = ot.OperationTask;
            oth.OperationStartTime = ot.OperationStartTime;
            oth.OperationEndTime = ot.OperationEndTime;
            
            oth.Guardian = ot.Guardian;
            oth.Operator = ot.Operator;
            oth.DutyOfficer = ot.DutyOfficer;
            oth.Dispatch = ot.Dispatch;
            oth.Remark = ot.Remark;

            List<OperationItemHistory> lst = new List<OperationItemHistory>();

            foreach (OperationItem item in ot.OperationItems)
            {
                OperationItemHistory oih = new OperationItemHistory();
                oih.Id = item.Id;
                oih.TicketId = item.TicketId;
                oih.OperationTime = item.OperationTime;
                oih.Mark = item.Mark;
                oih.OrderNum = item.OrderNum;
                oih.Item = item.Item;
                lst.Add(oih);
            }

            oth.OperationItems = lst;

            bll.OperationTickets.DeleteById(id);
            bll.OperationTicketHistorys.Add(oth);

            return RedirectToAction("Index");
        }


        public ActionResult CreateItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OperationItem OperationItem = new OperationItem();
            OperationItem.TicketId = id;
            return View(OperationItem);
        }

        public ActionResult CreateItem2(OperationItem OperationItem)
        {
            int? Id = OperationItem.TicketId;
            if (OperationItem != null)
            {
                bll.OperationItems.Add(OperationItem);
            }

            OperationTicket OperationTicket = bll.OperationTickets.Find(Id);
            if (OperationTicket == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Edit", OperationTicket);
        }
        
        public ActionResult EditItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OperationItem OperationItem = bll.OperationItems.Find(id);
            if (OperationItem == null)
            {
                return HttpNotFound();
            }

            return View(OperationItem);
        }

        public ActionResult EditItem2(OperationItem OperationItem)
        {
            int? Id = OperationItem.TicketId;
            if (OperationItem != null)
            {
                bll.OperationItems.Edit(OperationItem);
            }

            OperationTicket OperationTicket = bll.OperationTickets.Find(Id);
            if (OperationTicket == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Edit", OperationTicket);
        }

        public ActionResult DeleteItem(int? id, int? TicketId)
        {
            if (id == null || TicketId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bll.OperationItems.DeleteById(id);
            OperationTicket OperationTicket = bll.OperationTickets.Find(TicketId);
            if (OperationTicket == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Edit", OperationTicket);
        }
    }
}