using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.AreaAndDev;
using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Location.TModel.Tools;
using WebApiLib.Clients;

namespace Rsetful.Controllers
{
    public class cardsController : Controller
    {
        private Bll bll = Bll.Instance();
        BaseDataClient client = new BaseDataClient("https://localhost:9347/");
        //获取门禁卡列表
        public ActionResult GetCardList()
        {
            BaseTran<cards> recv = client.GetCardList();
            
            return View(recv);
        }

        //获取门禁卡操作历史
        public ActionResult GetSingleCardActionHistory(int id, string begin_date, string end_date)
        {
            BaseTran<cards_actions> recv = client.GetSingleCardActionHistory(id, begin_date, end_date);

            return View(recv);
        }
    }
}