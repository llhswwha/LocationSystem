using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using TransClass.Models;
using Location.Model;


namespace RsetfulSvr.Controllers.Client
{
    public class ShowPhysicalTopologyController : Controller
    {
        // GET: ShowArea
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllPhysicalTopologys()
        {
            PhysicalTopologyTrans recv = Activator.CreateInstance<PhysicalTopologyTrans>();
            try
            {
                string url = "http://localhost:41363/api/PhysicalTopology";

                recv = TransClass.GetMethod<PhysicalTopologyTrans>.Get(url);

                if (recv.data == null)
                {
                    recv.data = new List<PhysicalTopology>();
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return View(recv.data);
        }

        public ActionResult GetPhysicalTopology()
        {
            PhysicalTopologyTrans recv = Activator.CreateInstance<PhysicalTopologyTrans>();
            PhysicalTopology pht = new PhysicalTopology();

            try
            {
                string url = "http://localhost:41363/api/PhysicalTopology/1";

                recv = TransClass.GetMethod<PhysicalTopologyTrans>.Get(url);

                PhysicalTopology pht2 = new PhysicalTopology();
                pht2 = recv.data[0];
                if (pht2 != null)
                {
                    pht = pht2;
                }

            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return View(pht);
        }

        public string GetPhysicalTopologyTree()
        {
            string retstring = "";
            try
            {
                string url = "http://localhost:41363/api/PhysicalTopology/0";

                retstring = TransClass.GetMethod<PhysicalTopologyTrans>.GetNoSerialize(url);
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return retstring;
        }
    }
}