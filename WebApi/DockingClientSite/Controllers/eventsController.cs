using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.Alarm;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Location.TModel.Tools;
using WebApiLib.Clients;

namespace Rsetful.Controllers
{
    public class eventsController : Controller
    {
        private Bll bll = Bll.Instance();

        BaseDataClient client = new BaseDataClient("https://localhost:9347/");
        // 获取告警事件列表
        public ActionResult GeteventsList(int? src, int? level, long? begin_t, long? end_t)
        {
            BaseTran<events> recv = client.GeteventsList(src, level, begin_t, end_t);
            
            return View(recv);
        }

        public string Delete()
        {
            List<int> LstId = bll.DevAlarms.DbSet.Select(p => p.Id).ToList();
            foreach (int id in LstId)
            {
                bll.DevAlarms.DeleteById(id);
            }

            return "ok";
        }
    }
}