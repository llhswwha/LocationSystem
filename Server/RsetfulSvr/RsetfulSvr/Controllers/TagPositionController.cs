using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Location.BLL;
using Location.Model;
using TransClass.Models;

namespace RsetfulSvr.Controllers
{
    public class TagPositionController : Controller
    {
        private System.Threading.Timer threadTimer;
        private LocationBll bll = new LocationBll();

        public TagPositionController()
        {
            threadTimer = new Timer(new TimerCallback(SendToUrl), null, 0, 300);
        }

        private void SendToUrl(object state)
        {
            try
            {
                TagPositionTrans recv = Activator.CreateInstance<TagPositionTrans>();
                TagPositionTrans send = new TagPositionTrans();
                List<TagPosition> data = bll.TagPositions.ToList();
                send.total = data.Count;
                send.msg = "";
                send.data = data;

                //string json = JsonConvert.SerializeObject(send);
                string url = "http://localhost:41363/api/RecvTagPosition";

                
                recv = TransClass.PostMethod<TagPositionTrans>.Post(url, send);
                int nn = 0;
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
            }
        }

        // GET: TagPosition
        public ActionResult Index()
        {
            return View();
        }
    }
}