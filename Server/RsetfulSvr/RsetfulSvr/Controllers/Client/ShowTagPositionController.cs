using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RsetfulSvr.Controllers.Client
{
    public class ShowTagPositionController : Controller
    {
        // GET: ShowTagPosition
        public ActionResult Index()
        {
            List<TagPosition> tagPositionList = RecvTagPositionController.GetList();
            return View(tagPositionList);
        }
    }
}