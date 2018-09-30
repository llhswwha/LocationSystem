using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Location.BLL;
using TransClass.Models;
using Location.Model;

namespace RsetfulSvr.Controllers
{
    public class MeterialController : ApiController
    {
        private LocationBll bll = new LocationBll();

        [HttpGet]
        public MeterialTrans GetMeterials(int? id)
        {
            MeterialTrans send = new MeterialTrans();
            send.total = 1;
            send.msg = "ok";

            if (id != null)
            {
                List<Meterial> meterialList = bll.Meterials.DbSet.Where(p => p.phtId == id).ToList();
                List<MeterialOther> meterialOtherList = new List<MeterialOther>();
                foreach (Meterial item in meterialList)
                {
                    MeterialOther item2 = new MeterialOther();
                    item2.Id = item.Id;
                    item2.Name = item.Name;
                    item2.qty = item.qty;
                    item2.unit = item.unit;
                    item2.loc = item.pht.Name;
                    meterialOtherList.Add(item2);
                }

                send.data = meterialOtherList;
            }

            return send;
        }
    }
}
