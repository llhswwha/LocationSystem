using Base.Common.Threads;
using BLL;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Converters;
using DbModel.Location.Work;
using DbModel.LocationHistory.Work;
using Location.BLL.Tool;
using Location.TModel.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Work;
using WebApiLib;

namespace LocationServer.Threads
{
    public class OperationTicketSHThread : IntervalTimerThread
    {
        private string Ip = "";
        private string Port = "";

        public OperationTicketSHThread(string ip, string port) : 
            base(TimeSpan.FromHours(1)      //TimeSpan.FromDays(1)//一天检查一次
                 , TimeSpan.FromSeconds(5))
        {
            Ip = ip;
            Port = port;
        }
        /// <summary>
        /// 获取操作票并保存数据库
        /// </summary>
        /// <returns></returns>
        public override bool TickFunction()
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime lastdate = DateTime.Now.AddDays(-1);
                long nowTime = TimeConvert.ToStamp(now);
                long lastDateStamp = TimeConvert.ToStamp(lastdate);
                string url = "http://"+ Ip + ":"+Port+"/api/tickets?type=1";
                //string result = WebApiHelper.GetString("http://120.25.195.214:18000/api/tickets?type=1");
                string result = WebApiHelper.GetString(url);
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                List<OperationTicketSH> saveTickets = new List<OperationTicketSH>();
                List<OperationTicketSH> addTickets = new List<OperationTicketSH>();  //保存数据库
                List<OperationTicketSH> updateTickets = new List<OperationTicketSH>();
                List<OperationTicketHistorySH> saveTicketsHis = new List<OperationTicketHistorySH>();
                List<OperationTicketHistorySH> addTicketsHis = new List<OperationTicketHistorySH>();//保存数据库
                List<OperationTicketHistorySH> updateTicketsHis = new List<OperationTicketHistorySH>();
                Message<TwoTickets> message = JsonConvert.DeserializeObject<Message<TwoTickets>>(result, setting);
                List<TwoTickets> list = message.data;

                //1.判断保存历史还是实时

                //foreach (TwoTickets ticket in list)
                //{
                //    long thisTime = 0;
                //    if (ticket.endTime != "")
                //    {
                //    thisTime= TimeConvert.ToStamp(Convert.ToDateTime((ticket.endTime)));
                //    }
                //    if (nowTime >= thisTime)//当前时间大于结束时间,放入历史表
                //    {
                //        saveTicketsHis.Add(ticket.ToDbHistoryModel());
                //    }
                //    else
                //    {
                //        saveTickets.Add(ticket.ToDbModel());
                //    }
                //}

                //直接全部保存历史(0609)
                foreach (TwoTickets ticket in list)
                {
                    saveTicketsHis.Add(ticket.ToDbHistoryModel());
                }


                //判断保存还是修改
                Bll db = Bll.NewBllNoRelation();
                //if (saveTickets.Count > 0)
                //{
                //    string strsql = string.Format(@"select distinct Abutment_Id from operationticketshes");
                //    List<int> idList = db.OperationTicketSHs.GetListIntBySql(strsql);
                //    foreach (OperationTicketSH operation in saveTickets)
                //    {
                //        if (idList.Contains((int)operation.Abutment_Id))
                //        {
                //            updateTickets.Add(operation);
                //        }
                //        else
                //        {
                //            addTickets.Add(operation);
                //        }
                //    }
                //    bool result1= db.OperationTicketSHs.AddRange(addTickets);
                //    bool result2 = db.OperationTicketSHs.EditRange(updateTickets);
                //    Log.Info("OperationTicketSHThread.result1:" + result1);
                //    Log.Info("OperationTicketSHThread.result2:" + result2);
                //}
                if (saveTicketsHis.Count > 0)
                {
                    string strsql = string.Format(@"select distinct Abutment_Id from operationtickethistoryshes where CreateTime>'{0}' ",now.AddDays(-3));
                    List<int> idList = db.OperationTicketHistorySHs.GetListIntBySql(strsql);
                    foreach (OperationTicketHistorySH operationHis in saveTicketsHis)
                    {
                        if (!idList.Contains((int)operationHis.Abutment_Id))
                        {
                            addTicketsHis.Add(operationHis);
                        }
                    }
                    bool result3 = db.OperationTicketHistorySHs.AddRange(addTicketsHis);
                    Log.Info("OperationTicketSHThread.result3:" + result3);
                }

                //实时数据库更新到历史数据库
                saveTickets.Clear();
                saveTicketsHis.Clear();
                saveTickets = db.OperationTicketSHs.ToList();
                foreach (OperationTicketSH operation in saveTickets)
                {
                    long thisTime = 0;
                    if (operation.endTime !=null)
                    {
                        thisTime = TimeConvert.ToStamp(operation.endTime);
                    }

                    if (nowTime >= thisTime)//转入历史，并删除实时
                    {
                        saveTicketsHis.Add(operation.ToTModel().ToDbHistoryModel());
                        db.OperationTicketSHs.DeleteById(operation.Id);
                    }
                 
                }
                bool result4 = db.OperationTicketHistorySHs.AddRange(saveTicketsHis);
                Log.Info("OperationTicketSHThread.result4:"+result4+"time:"+now.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("OperationTicketSHThread:"+ex.ToString());
            }
            return true;
        }

        protected override void DoBeforeWhile()
        {
            throw new NotImplementedException();
        }
    }
}
