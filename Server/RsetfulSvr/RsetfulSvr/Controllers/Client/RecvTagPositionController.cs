using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TransClass.Models;
using Location.Model;

namespace RsetfulSvr.Controllers.Client
{
    
    public class RecvTagPositionController : ApiController
    {
        private static List<TagPosition> TagPositionList = new List<TagPosition>();

        [HttpPost]
        public TagPositionTrans PostArcho(TagPositionTrans recv)
        {
            int total = recv.total;
            List<TagPosition> data = recv.data;

            foreach (TagPosition item in data)
            {
                TagPosition item2 = TagPositionList.Find(p => p.Tag == item.Tag);
                if (item2 != null)
                {
                    TagPositionList.Remove(item2);
                    item2 = item;
                }
                
                TagPositionList.Add(item);
            }
            
            TagPositionTrans send = new TagPositionTrans();
            send.total = total;
            send.msg = "ok";

            return send;
        }

        public static List<TagPosition> GetList()
        {
            return TagPositionList;
        }


    }
}
