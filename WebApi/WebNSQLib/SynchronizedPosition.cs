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
using System.Threading;
using Location.BLL.Tool;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;

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
        public SynchronizedPosition(string host)
        {
            if (producer == null)
            {
                //Println
                //producer = new Producer("127.0.0.1:4150");
                //producer = new Producer("http://127.0.0.1:4151/");
                TimeSpan httpRequestTimeout = new TimeSpan(200000000);
                producer = new NsqdHttpClient(host, httpRequestTimeout);
                //producer = new Producer("120.25.195.214:4150");
                //producer.Publish("position","测试语句");
            }
        }

        private bool isNsqEnabled = true;

        public void SendPositionMsgAsync(List<DbModel.LocationHistory.Data.Position> Info)
        {
            if (isNsqEnabled)
            {
                ThreadPool.QueueUserWorkItem((st) =>
                {
                    isNsqEnabled=SendPositionMsg(Info);
                }, null);
            }
            else
            {

            }
        }

        private List<Area> areaList;

        private List<Personnel> personList;

        private bool isBusy = false;

        public bool SendPositionMsg(List<DbModel.LocationHistory.Data.Position> Info)
        {
            if (isBusy) return false;
            if (personList == null)
            {
                personList = bll.Personnels.ToList();
            }
            if (areaList == null)
            {
                areaList = bll.Areas.ToList();
            }
            foreach (DbModel.LocationHistory.Data.Position item in Info)
            {
                try
                {
                    
                    position SendInfo = new position();
                    if (item.Code == "" || item.AreaId == null)
                    {
                        continue;
                    }
                    SetSendInfo(item, SendInfo);
                    if (item.PersonnelID != null)
                    {
                        var ps = personList.FirstOrDefault(p => p.Id == item.PersonnelID);
                        if (ps != null)
                            SendInfo.staffCode = Convert.ToString(ps.WorkNumber);
                    }
                    var ae = areaList.FirstOrDefault(p => p.Id == item.AreaId);
                    if (ae != null)
                        SendInfo.zoneKksCode = ae.KKS;

                    string strJson = JsonConvert.SerializeObject(SendInfo);
                    isBusy = true;
                    string result = producer.Publish("position", strJson);//这里可能会卡住
                    isBusy = false;
                    if (result == null) return false;
                    //producer.Publish("http://127.0.0.1:4151/pub?topic=position", strJson);
                }
                catch (Exception ex)
                {
                    Log.Error("SendPositionMsg", ex);
                }
            }
            return true;
        }

        private static void SetSendInfo(DbModel.LocationHistory.Data.Position item, position SendInfo)
        {
            SendInfo.deviceCode = item.Code;
            SendInfo.t = item.DateTimeStamp;
            SendInfo.x = item.X;
            SendInfo.y = item.Z;
            SendInfo.z = item.Y;
            SendInfo.staffCode = null;
        }
    }
}
