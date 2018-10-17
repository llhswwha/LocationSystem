using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
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
    /// <summary>
    /// 区域
    /// </summary>
    public class zonesController : Controller
    {
        private Bll bll = Bll.Instance();
        BaseDataClient client = new BaseDataClient("https://ipms.datacase.io/api/");

        //获取区域列表
        public ActionResult GetzonesList()
        {
            BaseTran<zone> recv = client.GetzonesList();
            
            return View(recv);
        }

        //获取单个区域信息
        public ActionResult GetSingleZonesInfo(int id, int view)
        {
            zone data = client.GetSingleZonesInfo(id, view);

            return View(data);
        }

        //获取指定区域下设备列表
        public ActionResult GetZoneDevList(int id)
        {
            BaseTran<device> recv = client.GetZoneDevList(id);

            return View(recv);
        }

        public string Delete()
        {
            List<int> LstId = bll.Areas.DbSet.Select(p => p.Id).ToList();
            foreach (int id in LstId)
            {
                bll.Areas.DeleteById(id);
            }

            return "ok";
        }
    }
}