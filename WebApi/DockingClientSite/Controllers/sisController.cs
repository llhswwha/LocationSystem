using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Data;
using DbModel.LocationHistory.AreaAndDev;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Location.TModel.Tools;
using WebApiLib.Clients;

namespace Rsetful.Controllers
{
    public class sisController : Controller
    {
        private Bll bll = Bll.Instance();

        BaseDataClient client = new BaseDataClient("http://localhost:9347/");

        // GET: sis
        public ActionResult GetSomesisList(string kks)
        {
            BaseTran<sis> recv = client.GetSomesisList(kks);
            
            return View(recv);
        }
    }
}