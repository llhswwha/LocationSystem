using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApiLib.Clients;

namespace Rsetful.Controllers
{
    public class ticketsController : Controller
    {
        private Bll bll = Bll.Instance();
        BaseDataClient client=new BaseDataClient("http://localhost:9347/");
        //获取两票列表
        public ActionResult GetList(string type,string begin_date,string end_date)
        {
            BaseTran<tickets> recv = client.GetTicketsList(type, begin_date, end_date);
            
            return View(recv);
        }

        //获取指定的两票详情
        public ActionResult GetDetail(int id)
        {
            tickets recv=client.GetTicketsDetail(id);
            return View(recv);
        }
    }
}