using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Location.BLL.Tool;

namespace WebLocation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Log.Info("HomeController.Index");
            return View();
        }

        public ActionResult About()
        {
            Log.Info("HomeController.About");
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Log.Info("HomeController.Contact");
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}