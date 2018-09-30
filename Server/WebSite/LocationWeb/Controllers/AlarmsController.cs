using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using DbModel.Location.Alarm;
using Webdiyer.WebControls.Mvc;
using WebLocation.Tools;
using BLL;

namespace WebLocation.Controllers
{
    public class AlarmsController : Controller
    {
        private Bll bll = new Bll();
        private int pageSize = StaticArgs.DefaultPageSize;

        // GET: Alarms
        public ActionResult Index(int pageIndex = 1)
        {
            PagedList<LocationAlarm> lst = bll.LocationAlarms.ToList().ToPagedList<LocationAlarm>(pageIndex, pageSize);
            return View("Index",lst);
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
