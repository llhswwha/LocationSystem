using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransClass.Models;
using Location.Model;
using Location.BLL;

namespace RsetfulSvr.Controllers.Client
{
    public class ShowMeterialController : Controller
    {
        private LocationBll bll = new LocationBll();

        // GET: ShowMeterial
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMeterials()
        {
            MeterialTrans recv = Activator.CreateInstance<MeterialTrans>();
            List<Meterial> meterialList = new List<Meterial>();
            try
            {
                string url = "http://localhost:41363/api/Meterial/1";

                recv = TransClass.GetMethod<MeterialTrans>.Get(url);

                foreach (MeterialOther item in recv.data)
                {
                    Meterial item2 = new Meterial();

                    item2.Id = item.Id;
                    item2.Name = item.Name;
                    item2.qty = item.qty;
                    item2.unit = item.unit;
                    item2.pht = bll.PhysicalTopologys.DbSet.Where(p => p.Name == item.loc).ToList()[0];
                    item2.phtId = item2.pht.Id;

                    meterialList.Add(item2);
                }

            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }

            return View(meterialList);
        }
    }
}