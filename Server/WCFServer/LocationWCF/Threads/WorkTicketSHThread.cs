using Base.Common.Threads;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using Location.BLL.Tool;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.LocationHistory.Work;
using dbEntity = DbModel.LocationHistory.Work.WorkTicketHistorySH;
using WebApiLib;
using BLL;
using DbModel.Converters;

namespace LocationServer.Threads
{

    /// <summary>
    /// 四会工作票
    /// </summary>
    public class WorkTicketSHThread : IntervalTimerThread
    {
        private string Ip = "";
        private string Port = "";
        public WorkTicketSHThread(string ip, string port) :
            base(TimeSpan.FromHours(2)    //TimeSpan.FromDays(1)//一天检查一次
               , TimeSpan.FromSeconds(5))
        {
            Ip = ip;
            Port = port;
        }

        public override bool TickFunction()
        {
            try
            {
                string url = "http://" + Ip + ":" + Port + "/api/tickets?type=0";
               // string result = WebApiHelper.GetString("http://120.25.195.214:18000/api/tickets?type=0");
                string result = WebApiHelper.GetString(url);
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                Message<WorkTicketHistorySH> message = JsonConvert.DeserializeObject<Message<WorkTicketHistorySH>>(result, setting);
                List<WorkTicketHistorySH> list = message.data;
                List<dbEntity> saveList = list.ToDbModelList();
                List<dbEntity> addList = new List<dbEntity>();
                List<dbEntity> updaList = new List<dbEntity>();
                Bll db = Bll.NewBllNoRelation();
                if (saveList != null && saveList.Count > 0)
                {
                    DateTime now = DateTime.Now;
                    string strsql = string.Format(@"select distinct Abutment_Id from worktickethistoryshes  where CreateTime>'{0}'",now.AddDays(-3));
                    List<int> idList = db.WorkTicketHistorySHes.GetListIntBySql(strsql);
                    foreach (dbEntity entity in saveList)
                    {
                        if (!idList.Contains((int)entity.Abutment_Id))
                        {
                            addList.Add(entity);
                        }
                    }
                    bool result1 = db.WorkTicketHistorySHes.AddRange(addList);
                    Log.Info("WorkTicketSHThread.result1:"+result1);
                } 
            }
            catch (Exception ex)
            {
                Log.Error("WorkTicketSHThread:"+ex.ToString());
            }
            return true;
        }

        protected override void DoBeforeWhile()
        {
            throw new NotImplementedException();
        }
    }
}
