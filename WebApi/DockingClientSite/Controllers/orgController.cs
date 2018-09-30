using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbModel.Location.Person;
using DbModel.Tools;
using WebApiLib.Clients;

namespace Rsetful.Controllers
{
    public class orgController : Controller
    {
        private Bll bll = Bll.Instance();
        BaseDataClient client = new BaseDataClient("http://localhost:9347/");

        //获取部门列表
        public ActionResult GetorgList()
        {
            BaseTran<org> recv = client.GetorgList();
            
            return View(recv);
        }
    }
}