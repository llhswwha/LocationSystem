using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Location.BLL;
using Location.Model;
using TransClass.Models;

namespace RsetfulSvr.Controllers
{
    public class PhysicalTopologyController : ApiController
    {
        private LocationBll bllNoVirtual = new LocationBll(false, false,true);
        private LocationBll bll = new LocationBll();

        [HttpGet]
        public PhysicalTopologyTrans GetAllArchors()
        {
            PhysicalTopologyTrans send = new PhysicalTopologyTrans();

            List<PhysicalTopology> physicalTopologyList = bllNoVirtual.PhysicalTopologys.ToList();
            List<PhysicalTopology> physicalTopologyList2 = bll.PhysicalTopologys.ToList();
            
            
            foreach (PhysicalTopology item in physicalTopologyList)
            {
                PhysicalTopology item2 = physicalTopologyList2.Find(p => p.Id == item.Id);
                if (item2 != null)
                {
                    item.Transfrom = item2.Transfrom;
                    item.Nodekks = item2.Nodekks;
                }
            }

            send.total = physicalTopologyList.Count;
            send.msg = "ok";
            send.data = physicalTopologyList;

            return send;
        }

        [HttpGet]
        public PhysicalTopologyTrans GetArchor(int? id)
        {
            bool bTree = false;
            PhysicalTopologyTrans send = new PhysicalTopologyTrans();
            send.total = 1;
            send.msg = "ok";

            if (id == null)
            {
                send.data = new List<PhysicalTopology>();
                return send;
            }

            if (id == 0)
            {
                bTree = true;
                id = 1;
            }

            PhysicalTopology pht = bllNoVirtual.PhysicalTopologys.Find(id);
            PhysicalTopology pht2 = bll.PhysicalTopologys.Find(id);

            if (pht == null || pht2 == null)
            {
                send.data = new List<PhysicalTopology>();
                return send;
            }

            if (bTree)
            {
                List<PhysicalTopology> list1 = new List<PhysicalTopology>();
                list1.Add(pht2);
                send.data = list1;
            }
            else
            {
                pht.Transfrom = pht2.Transfrom;
                pht.Nodekks = pht2.Nodekks;
                List<PhysicalTopology> list1 = new List<PhysicalTopology>();
                list1.Add(pht);
                send.data = list1;
            }

            return send;
        }
    }
}
