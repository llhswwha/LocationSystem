using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TModel.Location.Manage;

namespace WebLocation.Controllers
{
    public class LoginController : Controller
    {       
        private Bll bll = new Bll();
        // GET: Index
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
     
        public ActionResult LoginMessage(LoginInfo info)
        {
            bll.Users.Login(info);
            //return info;
            return Json(info, JsonRequestBehavior.AllowGet);
        }
    }
}