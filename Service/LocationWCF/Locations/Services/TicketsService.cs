using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApiLib;
using TModel.Location.Work;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using BLL;
using DbModel.Location.Work;
using DbModel.Converters;
using DbModel.LocationHistory.Work;
using Location.BLL.Tool;
using TModel.Tools;
using tEntity = TModel.LocationHistory.Work.WorkTicketHistorySH;
using TModel.LocationHistory.Work;

namespace LocationServices.Locations.Services
{
    public interface ITicketsService : INameEntityService<TwoTickets>
    {
        List<TwoTickets> GetListByCondition(string value, DateTime startTime, DateTime endTime);
        List<TwoTickets> GetList();
        PageInfo<TwoTickets> GetHistoryPage(TModel.FuncArgs.TicketSearchArgs args);

        PageInfo<tEntity> GetWorkTicketHisSHPage(TModel.FuncArgs.TicketSearchArgs args);

        TwoTickets GetHisEntity(string id);

        List<TwoTickets> GetHistoryListByTime(int count, string field, string sort = "desc");

        tEntity GetWorkTicketHisSHById(string id);

        List<tEntity> GetWorkTickHisListByTime(int count, string field, string sort = "desc");
    }

    public class TicketsService : ITicketsService
    {
        private Bll db;
        public TicketsService()
        {
            db = Bll.NewBllNoRelation();
        }


      
        public TwoTickets Delete(string id)
        {
            throw new NotImplementedException();
        }

        public TwoTickets GetEntity(string id)
        {
            try
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                TwoTickets ticket = db.OperationTicketSHs.Find(i => i.Abutment_Id.ToString() == id).ToTModel();
                string results = ticket.detail;
                Details details = JsonConvert.DeserializeObject<Details>(results, setting);
                ticket.detail = "";
                DetailsSet detalsSet = new DetailsSet();
                detalsSet.optTicket = details.optTicket;
                List<LinesSet> lineSet = new List<LinesSet>();
                List<LinesGet> lineList = details.lines;

                //循环，给LineSet赋值
                if (lineList != null)
                {
                    foreach (LinesGet line in lineList)
                    {
                        LinesSet setline = new LinesSet();
                        setline.name = line.name;
                        List<Dictionary<string, string>> dicList = line.lineContentList;
                        List<LineContent> contentList = new List<LineContent>();
                        if (dicList != null)
                        {
                            foreach (Dictionary<string, string> dic in dicList)
                            {
                                LineContent linecontent = new LineContent();
                                List<KeyValue> keyList = new List<KeyValue>();
                                if (dic != null)
                                {
                                    foreach (KeyValuePair<string, string> kv in dic)
                                    {
                                        KeyValue keyValue = new KeyValue();
                                        keyValue.key = kv.Key;
                                        keyValue.value = kv.Value;
                                        keyList.Add(keyValue);
                                    }
                                    linecontent.Content = keyList;
                                    contentList.Add(linecontent);
                                }
                            }
                            setline.lineContentList = contentList;
                            lineSet.Add(setline);
                        }
                    }
                    detalsSet.lines = lineSet;
                    ticket.detailsSet = detalsSet;
                }
                return ticket;
            }
            catch (Exception ex)
            {
                Log.Error("TicketsService.GetEntity:"+ex.ToString());
                return null;
            }
        }

        public TwoTickets GetHisEntity(string id)
        {
            try
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                TwoTickets ticket = db.OperationTicketHistorySHs.Find(i => i.Abutment_Id.ToString() == id).ToTModel();
                string results = ticket.detail;
                Details details = JsonConvert.DeserializeObject<Details>(results, setting);
                ticket.detail = "";
                DetailsSet detalsSet = new DetailsSet();
                detalsSet.optTicket = details.optTicket;
                List<LinesSet> lineSet = new List<LinesSet>();
                List<LinesGet> lineList = details.lines;

                //循环，给LineSet赋值
                if (lineList != null)
                {
                    foreach (LinesGet line in lineList)
                    {
                        LinesSet setline = new LinesSet();
                        setline.name = line.name;
                        List<Dictionary<string, string>> dicList = line.lineContentList;
                        List<LineContent> contentList = new List<LineContent>();
                        if (dicList != null)
                        {
                            foreach (Dictionary<string, string> dic in dicList)
                            {
                                LineContent linecontent = new LineContent();
                                List<KeyValue> keyList = new List<KeyValue>();
                                if (dic != null)
                                {
                                    foreach (KeyValuePair<string, string> kv in dic)
                                    {
                                        KeyValue keyValue = new KeyValue();
                                        keyValue.key = kv.Key;
                                        keyValue.value = kv.Value;
                                        keyList.Add(keyValue);
                                    }
                                    linecontent.Content = keyList;
                                    contentList.Add(linecontent);
                                }
                            }
                            setline.lineContentList = contentList;
                            lineSet.Add(setline);
                        }
                    }
                    detalsSet.lines = lineSet;
                    ticket.detailsSet = detalsSet;
                }
                return ticket;
            }
            catch (Exception ex)
            {
                Log.Error("TicketsService.GetEntity:" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 获取工作票
        /// </summary>
        /// <param name="id">第三方对接ID：Abutment_Id</param>
        /// <returns></returns>
        public tEntity GetWorkTicketHisSHById(string id)
        {
            tEntity ticket = new tEntity(); 
            try
            {
                ticket = db.WorkTicketHistorySHes.Find(i => i.Abutment_Id.ToString() ==id ).ToTModel();
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
               WorkTicketDetails detail = JsonConvert.DeserializeObject<WorkTicketDetails>(ticket.detail,setting);
                ticket.workTicket = detail.workTicket ;
                ticket.detail = "";
            }
            catch (Exception ex)
            {
                Log.Error("TicketsService.GetWorkTicketHisSHById:" + ex.ToString());
            }
            return ticket;
        }

        public  List<TwoTickets> GetList()
        {
            List<OperationTicketSH> dbTable= db.OperationTicketSHs.ToList();
            List<TwoTickets> list = new List<TwoTickets>();
            foreach (OperationTicketSH ticketSH in dbTable)
            {
                TwoTickets ticket = ticketSH.ToTModel();
                ticket.detail = "";
                ticket.detailsSet = null;
                list.Add(ticket);
            }
            return list;
        }
        /// <summary>
        /// 实时操作票
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<TwoTickets> GetListByCondition(string value, DateTime startTime, DateTime endTime)
        {
            if (startTime == null || endTime == null) return null;
            List<OperationTicketSH> dList = db.OperationTicketSHs.GetListByCondition(value,startTime,endTime);
            return dList.ToTModelList();
        }
        /// <summary>
        /// 获取历史操作票
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<TwoTickets> GetHistoryListByCondition(string value, DateTime startTime, DateTime endTime)
        {
            if (startTime == null || endTime == null) return null;
            List<TwoTickets> tickets = new List<TwoTickets>();
            List<OperationTicketHistorySH> dHisList = db.OperationTicketHistorySHs.GetListByCondition(value,startTime,endTime);
            foreach (OperationTicketHistorySH hisDb in dHisList)
            {
                TwoTickets ticket = hisDb.ToTModel();
                ticket.detail = "";
                ticket.detailsSet = null;
                tickets.Add(ticket);
            }
            return tickets;
        }

        /// <summary>
        /// 操作票分页
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PageInfo<TwoTickets> GetHistoryPage(TModel.FuncArgs.TicketSearchArgs  args)
        {
             
            PageInfo<TwoTickets> page = new PageInfo<TwoTickets>();
            try {
                PageInfo<OperationTicketHistorySH> pageDb= db.OperationTicketHistorySHs.GetPageByCondition(args.value,args.startTime,args.endTime,args.pageIndex,args.pageSize);
                page.total = pageDb.total;
                page.totalPage = pageDb.totalPage;
                page.pageIndex = pageDb.pageIndex;
                page.pageSize = pageDb.pageSize;
                page.data = pageDb.data.ToTModelList();
            }
            catch (Exception ex)
            {
                Log.Error("TicketsService.GetHistoryPage:"+ex.ToString());
            }
            return page;
        }

        /// <summary>
        /// 工作票分页
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PageInfo<tEntity> GetWorkTicketHisSHPage(TModel.FuncArgs.TicketSearchArgs args)
        {
            PageInfo<tEntity> page = new PageInfo<tEntity>();
            try
            {
                PageInfo<DbModel.LocationHistory.Work.WorkTicketHistorySH> pageDb = db.WorkTicketHistorySHes.GetPageByCondition(args.value,args.startTime,args.endTime,args.pageIndex,args.pageSize);
                page.total = pageDb.total;
                page.totalPage = pageDb.totalPage;
                page.pageIndex = pageDb.pageIndex;
                page.pageSize = pageDb.pageSize;
                page.data = pageDb.data.ToTModelList();
            }
            catch (Exception ex)
            {
                Log.Error("TicketsService.GetWorkTicketHisSHPage:" + ex.ToString());
            }
            return page;
        }


     /// <summary>
     /// 获取操作票最近N条
     /// </summary>
     /// <param name="count"></param>
     /// <param name="field"></param>
     /// <param name="sort"></param>
     /// <returns></returns>
        public List<TwoTickets> GetHistoryListByTime(int count, string field,string sort="desc")
        {
            List<TwoTickets> list = new List<TwoTickets>();
            try
            {
                List<OperationTicketHistorySH> operationHis = db.OperationTicketHistorySHs.GetListByTime(count,field,sort);
                list = operationHis.ToTModelList();
            }
            catch (Exception ex)
            {
                Log.Error("TicketsService.GetHistoryListByTime:" + ex.ToString());
            }
            return list;
        }
        /// <summary>
        /// 获取工作票最近N条
        /// </summary>
        /// <param name="count"></param>
        /// <param name="field"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<tEntity> GetWorkTickHisListByTime(int count, string field, string sort = "desc")
        {
            List<tEntity> list = new List<tEntity>();
            try
            {
                List<DbModel.LocationHistory.Work.WorkTicketHistorySH> dbList = db.WorkTicketHistorySHes.GetListByTime(count, field, sort);
                list = dbList.ToTModelList();
            }
            catch (Exception ex)
            {
                Log.Error("TicketsService.GetWorkTickHisListByTime:" + ex.ToString());
            }
            return list;
        }
        


        /// <summary>
        ///  第三方接口获取
        /// </summary>
        /// <returns></returns>
        public List<TwoTickets> ListAll()
        {
            try
            {
                string result = WebApiHelper.GetString("http://120.25.195.214:18000/api/tickets?type=1");
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                Message<TwoTickets> message = JsonConvert.DeserializeObject<Message<TwoTickets>>(result, setting);
                int total = message.total;
                string msg = message.msg;
                List<TwoTickets> list = message.data;
                //循环获取DetailsSet
                if (list != null)
                {
                    foreach (TwoTickets ticket in list)
                    {
                        string results = ticket.detail;
                        Details details = JsonConvert.DeserializeObject<Details>(results, setting);
                       // ticket.detail = "";
                        DetailsSet detalsSet = new DetailsSet();
                        detalsSet.optTicket = details.optTicket;
                        List<LinesSet> lineSet = new List<LinesSet>();
                        List<LinesGet> lineList = details.lines;

                        //循环，给LineSet赋值
                        if (lineList != null)
                        {
                            foreach (LinesGet line in lineList)
                            {
                                LinesSet setline = new LinesSet();
                                setline.name = line.name;
                                List<Dictionary<string, string>> dicList = line.lineContentList;
                                List<LineContent> contentList = new List<LineContent>();
                                if (dicList != null)
                                {
                                    foreach (Dictionary<string, string> dic in dicList)
                                    {
                                        LineContent linecontent = new LineContent();
                                        List<KeyValue> keyList = new List<KeyValue>();
                                        if (dic != null)
                                        {
                                            foreach (KeyValuePair<string, string> kv in dic)
                                            {
                                                KeyValue keyValue = new KeyValue();
                                                keyValue.key = kv.Key;
                                                keyValue.value = kv.Value;
                                                keyList.Add(keyValue);
                                            }
                                            linecontent.Content = keyList;
                                            contentList.Add(linecontent);
                                        }
                                    }
                                    setline.lineContentList = contentList;
                                    lineSet.Add(setline);
                                }
                            }
                            detalsSet.lines = lineSet;
                            ticket.detailsSet = detalsSet;
                        }
                    }
                }
                Message<TwoTickets> datas = message;
                return message.data;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 实时
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<TwoTickets> GetListByName(string name)
        {
            return db.OperationTicketSHs.Where(i => i.ticketName.Contains(name.Trim())).ToTModelList();
        }

        public TwoTickets Post(TwoTickets item)
        {
            throw new NotImplementedException();
        }

        public TwoTickets Put(TwoTickets item)
        {
            throw new NotImplementedException();
        }

       
        


    }
}
