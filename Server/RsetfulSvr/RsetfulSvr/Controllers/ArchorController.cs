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
    public class ArchorController : ApiController
    {
        private LocationBll bllNoVirtual = new LocationBll(false, false, true);

        [HttpGet]
        public ArchorTrans GetAllArchors()
        {
            ArchorTrans send = new ArchorTrans();

            List<Archor> archorList = bllNoVirtual.Archors.ToList();
            
            send.total = archorList.Count;
            send.msg = "ok";
            send.data = archorList;

            return send;
        }

        [HttpGet]
        public ArchorTrans GetArchor(int? id)
        {
            ArchorTrans send = new ArchorTrans();
            send.total = 1;
            send.msg = "ok";

            if (id != null)
            {
                Archor archor = bllNoVirtual.Archors.Find(id);
                if (archor != null)
                {
                    List<Archor> archorList = new List<Archor>();
                    archorList.Add(archor);
                    send.data = archorList;
                }
            }

            return send;
        }

    }
}
