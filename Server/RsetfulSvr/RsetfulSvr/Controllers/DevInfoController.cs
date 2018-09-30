using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Location.BLL;
using TransClass.Models;
using Location.Model.LocationTables;

namespace RsetfulSvr.Controllers
{
    public class DevInfoController : ApiController
    {
        private LocationBll bll = new LocationBll();

        [HttpGet]
        public DevInfoTrans GetAllDevInfos()
        {
            DevInfoTrans send = new DevInfoTrans();

            List<DevInfo> devinfoList = bll.DevInfos.ToList();

            send.total = devinfoList.Count;
            send.msg = "ok";
            send.data = devinfoList;
            
            return send;
        }

        [HttpGet]
        public DevInfoTrans GetDevInfo(int? id)
        {
            DevInfoTrans send = new DevInfoTrans();
            send.total = 1;
            send.msg = "ok";

            if (id != null)
            {
                //DevInfo devinfo = bll.DevInfos.DbSet.Where(p=>p.Id == id).ToList()[0];//ToList结果为0个时会出异常，改为下面语句
                //DevInfo devinfo = bll.DevInfos.DbSet.Where(p => p.Id == id).FirstOrDefault();//等效于下面语句
                DevInfo devinfo = bll.DevInfos.DbSet.FirstOrDefault(p => p.Id == id);
                if (devinfo != null)
                {
                    List<DevInfo> devinfoList = new List<DevInfo>();
                    devinfoList.Add(devinfo);
                    send.data = devinfoList;
                }
            }
       
            return send;
        }



    }
}
