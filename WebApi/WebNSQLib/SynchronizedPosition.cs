using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NsqSharp;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using BLL;
using Newtonsoft.Json;
using NsqSharp.Api;

namespace WebNSQLib
{
    public class SynchronizedPosition
    {
        //public static Producer producer;
        public NsqdHttpClient producer;

        private Bll bll = Bll.Instance();

        public SynchronizedPosition()
        {
            if (producer == null)
            {
                //Println
                //producer = new Producer("127.0.0.1:4150");
                //producer = new Producer("http://127.0.0.1:4151/");
                TimeSpan httpRequestTimeout = new TimeSpan(200000000);
                 producer = new NsqdHttpClient("120.25.195.214:4151", httpRequestTimeout);
                //producer = new Producer("120.25.195.214:4150");
                //producer.Publish("position","测试语句");
            }
        }

        public void SendPositionMsg(List<DbModel.LocationHistory.Data.Position> Info)
        {
            string strSendInfo = "";

            foreach (DbModel.LocationHistory.Data.Position item in Info)
            {
                position SendInfo = new position();

                if (item.Code == "" || item.AreaId == null)
                {
                    continue;
                }

                SendInfo.deviceCode = item.Code;
                SendInfo.t = item.DateTimeStamp;
                SendInfo.x = item.X;
                SendInfo.y = item.Z;
                SendInfo.z = item.Y;
                SendInfo.staffCode = null;

                if (item.PersonnelID != null)
                {
                    DbModel.Location.Person.Personnel ps = bll.Personnels.DbSet.Where(p => p.Id == item.PersonnelID).FirstOrDefault();
                    SendInfo.staffCode = Convert.ToString(ps.WorkNumber);
                }

                DbModel.Location.AreaAndDev.Area ae = bll.Areas.DbSet.Where(p => p.Id == item.AreaId).FirstOrDefault();
                SendInfo.zoneKksCode = ae.KKS;

                
                string strJson = JsonConvert.SerializeObject(SendInfo);
                producer.Publish("position", strJson);
                //producer.Publish("http://127.0.0.1:4151/pub?topic=position", strJson);
                strSendInfo = "";
            }
            return;
        }
    }
}
