using CommunicationClass.SihuiThermalPowerPlant.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib;

namespace LocationServices.Locations.Services
{
    public interface ITicketsService : INameEntityService<TwoTickets>
    {

    }

    public class TicketsService : ITicketsService
    {
        public TwoTickets Delete(string id)
        {
            throw new NotImplementedException();
        }

        public TwoTickets GetEntity(string id)
        {
            List<TwoTickets> list = ListAll();
            TwoTickets ticket = list.Find(i=>i.id.ToString()==id);
            return ticket;
        }

        public List<TwoTickets> GetList()
        {
            List<TwoTickets> list = ListAll();
            foreach (TwoTickets tickets in list)
            {
                tickets.detailsSet = null;
            }
            return list;
        }

        public List<TwoTickets> ListAll()
        {
            try
            {
                string result = WebApiHelper.GetString("http://120.25.195.214:18000/api/tickets?type=1");
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                Message message = JsonConvert.DeserializeObject<Message>(result, setting);
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
                    }
                }
                Message datas = message;
                return message.data;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<TwoTickets> GetListByName(string name)
        {
            throw new NotImplementedException();
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
