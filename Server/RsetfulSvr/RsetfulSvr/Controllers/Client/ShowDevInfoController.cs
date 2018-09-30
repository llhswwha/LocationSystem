using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransClass.Models;
using Location.Model.LocationTables;

namespace RsetfulSvr.Controllers.Client
{
    public class ShowDevInfoController : Controller
    {
        // GET: ShowDevInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllDevInfos()
        {
            DevInfoTrans recv = Activator.CreateInstance<DevInfoTrans>();
            try
            {
                string url = "http://localhost:41363/api/DevInfo";

                recv = TransClass.GetMethod<DevInfoTrans>.GetHasDateTime(url);
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return View(recv.data);
        }

        public ActionResult GetDevInfo(int? id)
        {
            DevInfoTrans recv = Activator.CreateInstance<DevInfoTrans>();
            DevInfo dit = new DevInfo();
            try
            {
                string url = "http://localhost:41363/api/DevInfo/1";

                recv = TransClass.GetMethod<DevInfoTrans>.GetHasDateTime(url);

                DevInfo dit2 = new DevInfo();
                dit2 = recv.data[0];
                if (dit2 != null)
                {
                    dit = dit2;
                }

            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return View(dit);
        }
    }
}