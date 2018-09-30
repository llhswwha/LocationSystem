using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransClass.Models;
using Location.Model;


namespace RsetfulSvr.Controllers.Client
{
    public class ShowArchorController : Controller
    {
        // GET: ShowArchor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllArchors()
        {
            ArchorTrans recv = Activator.CreateInstance<ArchorTrans>();
            try
            {
                string url = "http://localhost:41363/api/Archor";
                
                recv = TransClass.GetMethod<ArchorTrans>.Get(url);
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return View(recv.data);

        }

        public ActionResult GetArchor()
        {
            ArchorTrans recv = Activator.CreateInstance<ArchorTrans>();
            Archor ch = new Archor();
            try
            {
                string url = "http://localhost:41363/api/Archor/1";

                recv = TransClass.GetMethod<ArchorTrans>.GetHasDateTime(url);

                Archor ch2 = new Archor();
                ch2 = recv.data[0];
                if (ch2 != null)
                {
                    ch = ch2;
                }

            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return View(ch);

        }
        
    }
}