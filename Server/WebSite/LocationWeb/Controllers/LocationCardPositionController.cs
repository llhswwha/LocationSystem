using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Location.Model;
using Location.BLL;
using WebLocation.Models;
using System.IO;
using BLL;
using DbModel.Location.Data;
using ExcelLib;

using Location.IModel;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class LocationCardPositionController : Controller
    {
        private Bll db = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;
        private static int  nFlag = 0; 
        private static List<DbModel.Location.AreaAndDev.Area> lst = new List<DbModel.Location.AreaAndDev.Area>();
        private static List<DbModel.Location.Person.Personnel> lst2 = new List<DbModel.Location.Person.Personnel>();

        // GET: TagPositions
        public ActionResult Index(int pageIndex = 1)
        {
            if (nFlag == 0)
            {
                dhjs();
                nFlag = 1;
            }

            PagedList<LocationCardPosition> lst = db.LocationCardPositions.ToList().ToPagedList<LocationCardPosition>(pageIndex, pageSize);
            GetListToViewBag();
            return View("Index", lst);
        }

        public void dhjs()
        {
            lst = db.Areas.ToList();
            lst2 = db.Personnels.ToList();
        }

        private void GetListToViewBag()
        {
            List<LocationCardPosition> tagPositionList = db.LocationCardPositions.ToList();
            SelectList selList = new SelectList(tagPositionList,"Tag");
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
