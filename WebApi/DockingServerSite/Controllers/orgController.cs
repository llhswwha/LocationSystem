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
    public class orgController : ApiController
    {
        private static Dictionary<int, org> dict = new Dictionary<int, org>();

        public static void Init()
        {
            dict.Add(1, new org { id = 0, name = "莘科", parent_id = 0, type = 0, description = "莘科" });
            dict.Add(2, new org { id = 1, name = "市场", parent_id = 0, type = 0, description = "莘科市场部" });
            dict.Add(3, new org { id = 2, name = "研发", parent_id = 0, type = 0, description = "莘科研发部" });
            dict.Add(4, new org { id = 3, name = "综合", parent_id = 0, type = 0, description = "莘科综合部" });
            dict.Add(5, new org { id = 4, name = "技术", parent_id = 0, type = 0, description = "莘科技术部" });
        }

        [HttpGet]
        public BaseTran<org> GetorgList()
        {
            BaseTran<org> send = new BaseTran<org>();
            List<org> lst = new List<org>();

            foreach (var item in dict)
            {
                org data = new org();
                data = item.Value.Clone();
                lst.Add(data);
            }

            send.total = lst.Count;
            send.msg = "ok";
            send.data = lst;

            return send;
        }
    }
}
