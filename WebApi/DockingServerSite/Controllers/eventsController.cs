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
    public class eventsController : ApiController
    {
        private static Dictionary<int, events> dict = new Dictionary<int, events>();

        public static void Init()
        {
            dict.Add(1, new events { id = 1, title = "告警1", msg = "单向门禁001产生了告警1", level = 0, code = "event1", src = 2, device_id = 5, device_desc = "单向门禁001", t = 1536212067 });
            dict.Add(2, new events { id = 2, title = "告警2", msg = "单向门禁001产生了告警2", level = 1, code = "event2", src = 2, device_id = 5, device_desc = "单向门禁001", t = 1536215751 });
            dict.Add(3, new events { id = 3, title = "告警3", msg = "单向门禁001产生了告警3", level = 2, code = "event3", src = 2, device_id = 5, device_desc = "单向门禁001", t = 1536219362 });
            dict.Add(4, new events { id = 4, title = "告警4", msg = "单向门禁001产生了告警4", level = 3, code = "event4", src = 2, device_id = 5, device_desc = "单向门禁001", t = 1536222971 });
            dict.Add(5, new events { id = 5, title = "告警5", msg = "双向门禁001产生了告警5", level = 0, code = "event5", src = 2, device_id = 6, device_desc = "双向门禁001", t = 1536226580 });
            dict.Add(6, new events { id = 6, title = "告警6", msg = "双向门禁001产生了告警6", level = 1, code = "event6", src = 2, device_id = 6, device_desc = "双向门禁001", t = 1536230188 });
            dict.Add(7, new events { id = 7, title = "告警7", msg = "双向门禁001产生了告警7", level = 2, code = "event7", src = 2, device_id = 6, device_desc = "双向门禁001", t = 1536233798 });
            dict.Add(8, new events { id = 8, title = "告警8", msg = "双向门禁001产生了告警8", level = 3, code = "event8", src = 2, device_id = 6, device_desc = "双向门禁001", t = 1536237407 });
        }

        [HttpGet]
        public BaseTran<events> GeteventsList(int? src, int? level, long? begin_t, long? end_t)
        {
            BaseTran<events> send = new BaseTran<events>();
            List<events> lst = new List<events>();

            foreach (var item in dict)
            {
                events Val = item.Value.Clone();
                events data = null;

                if (src != null)
                {
                    if (Val.src == src)
                    {
                        data = Val.Clone();
                    }
                    else
                    {
                        data = null;
                    }
                }

                if (level != null)
                {
                    if (Val.level == level)
                    {
                        data = Val.Clone();
                    }
                    else
                    {
                        data = null;
                    }
                }

                if (begin_t != null && end_t != null)
                {
                    if (Val.t >= begin_t && Val.t <= end_t)
                    {
                        data = Val.Clone();
                    }
                    else
                    {
                        data = null;
                    }
                }

                if (data != null)
                {
                    lst.Add(data);
                }
            }

            
            send.total = lst.Count;
            send.msg = "ok";
            send.data = lst;

            return send;
        }

    }
}
