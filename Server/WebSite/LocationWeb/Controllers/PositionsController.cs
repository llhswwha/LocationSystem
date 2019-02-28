using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Location.BLL;

using Location.Model;
using WebLocation.Models;
using System.IO;
using BLL;
using DbModel.LocationHistory.Data;
using ExcelLib;

using Location.IModel;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class PositionsController : Controller
    {
        private Bll db = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        //private int pageSize = 15;

        // GET: Positions
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<Position> lst = db.Positions.ToList().ToPagedList<Position>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
            //return View(db.Position.ToList());
        }

        private void GetListToViewBag()
        {
            List<Position> positionList = db.Positions.ToList();
            SelectList selList = new SelectList(positionList,"Id", "Tag");
            ViewBag.selList = selList.AsEnumerable();
        }       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
