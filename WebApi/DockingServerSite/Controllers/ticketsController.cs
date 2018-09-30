using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SihuiThermalPowerPlant.Controllers
{
    [RoutePrefix("api/tickets")]
    public class ticketsController : ApiController
    {
        private static List<tickets> items = new List<tickets>();

        public static void Init()
        {
            items.Add(new tickets { id = 1, code = "1", type = 1, state = 0, worker_ids = "[1,2]", zone_ids = "[3]", detail = "操作票测试1" });
            items.Add(new tickets { id = 2, code = "2", type = 1, state = 0, worker_ids = "[2,3]", zone_ids = "[3]", detail = "操作票测试2" });
            items.Add(new tickets { id = 3, code = "3", type = 1, state = 0, worker_ids = "[3,4]", zone_ids = "[3]", detail = "操作票测试3" });
            items.Add(new tickets { id = 4, code = "4", type = 1, state = 0, worker_ids = "[4,5]", zone_ids = "[3]", detail = "操作票测试4" });
            items.Add(new tickets { id = 5, code = "5", type = 2, state = 0, worker_ids = "[1,2]", zone_ids = "[3]", detail = "工作票测试1" });
            items.Add(new tickets { id = 6, code = "6", type = 2, state = 0, worker_ids = "[2,3]", zone_ids = "[3]", detail = "工作票测试2" });
            items.Add(new tickets { id = 7, code = "7", type = 2, state = 0, worker_ids = "[3,4]", zone_ids = "[3]", detail = "工作票测试3" });
            items.Add(new tickets { id = 8, code = "8", type = 2, state = 0, worker_ids = "[4,5]", zone_ids = "[3]", detail = "工作票测试4" });

        }

        [HttpGet]
        [Route("")]
        public BaseTran<tickets> GetList()
        {
            var lst = items;
            var send = new BaseTran<tickets>(lst);
            return send;
        }

        [HttpGet]
        [Route("")]
        public BaseTran<tickets> GetList(int type, string begin_date, string end_date)
        {
            var lst = items.FindAll(i => i.type == type);
            var send = new BaseTran<tickets>(lst);
            return send;
        }

        [HttpGet]
        [Route("{id}")]
        public tickets GetDetail(int id)
        {
            return items.FirstOrDefault(i => i.id == id);
        }
    }
}
