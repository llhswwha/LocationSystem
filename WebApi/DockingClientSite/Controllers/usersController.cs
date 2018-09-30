using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.Person;
using System.IO;
using DbModel.Tools;
using DbModel.Location.AreaAndDev;
using BLL;
using WebApiLib.Clients;

namespace Rsetful.Controllers
{
    /// <summary>
    /// 人员
    /// </summary>
    public class usersController : Controller
    {
        private Bll bll = Bll.Instance();
        BaseDataClient client = new BaseDataClient("http://localhost:9347/");

        // GET: users
        public ActionResult GetUserList()
        {
            BaseTran<users> recv = client.GetUserList();
            
            return View(recv);
        }

        public string Delete()
        {
            List<int> LstId = bll.Personnels.DbSet.Select(p => p.Id).ToList();
            foreach (int id in LstId)
            {

                bll.Personnels.DeleteById(id);
            }
            
            return "ok";
        }
    }
}