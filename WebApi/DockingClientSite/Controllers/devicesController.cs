using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.AreaAndDev;
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
    public class devicesController : Controller
    {
        private Bll bll = Bll.Instance();

        BaseDataClient client = new BaseDataClient("http://localhost:9347/");

        //获取设备列表
        public ActionResult GetDeviceList(string types, string code, string name)
        {
            BaseTran<devices> recv = client.GetDeviceList(types, code, name);
            
            return View(recv);
        }

        // 获取单个设备信息
        public ActionResult GetSingleDeviceInfo(int id)
        {
            devices data = client.GetSingleDeviceInfo(id);
            
            return View(data);
        }

        //获取单台设备操作历史
        public ActionResult GetSingleDeviceActionHistory(int id, string begin_date, string end_date)
        {
            BaseTran<devices_actions> recv = client.GetSingleDeviceActionHistory(id, begin_date, end_date);
            
            return View(recv);
        }

        public string Delete()
        {
            List<int> LstId = bll.DevInfos.DbSet.Select(p => p.Id).ToList();
            foreach (int id in LstId)
            {
                bll.DevInfos.DeleteById(id);
            }

            return "ok";
        }

        public string Delete2()
        {
            List<int> LstId = bll.DevEntranceGuardCardActions.DbSet.Select(p => p.Id).ToList();
            foreach (int id in LstId)
            {
                bll.DevEntranceGuardCardActions.DeleteById(id);
            }

            return "ok";
        }
    }
}