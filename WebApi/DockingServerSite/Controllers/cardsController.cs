using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Http;

namespace SihuiThermalPowerPlant.Controllers
{
    [ServiceContract(Name = "cardsController")]
    public interface IcardsController
    {
        [OperationContract]
        [WebGet(UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        BaseTran<cards> GetDeviceList();

        [OperationContract]
        [WebGet(UriTemplate = "/{id}/actions?begin_date={begin_date}&end_date={end_date}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        BaseTran<cards_actions> GetSingleCardActionHistory(string id, string begin_date, string end_date); 
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class cardsController : IcardsController
    {
        private static Dictionary<int, cards> dict = new Dictionary<int, cards>();
        private static Dictionary<int, cards_actions> dict2 = new Dictionary<int, cards_actions>();
        public static void Init()
        {
            dict.Add(1, new cards { id = 1, code = "card1", emp_id = 1, state = 0 });
            dict.Add(2, new cards { id = 2, code = "card2", emp_id = 2, state = 0 });
            dict.Add(3, new cards { id = 3, code = "card3", emp_id = 3, state = 0 });
            dict.Add(4, new cards { id = 4, code = "card4", emp_id = 4, state = 0 });
            dict.Add(5, new cards { id = 5, code = "card5", emp_id = 5, state = 0 });

            dict2.Add(1, new cards_actions { id = 1, device_id = 5, card_code = "card1", t = 1536212067, code = 0, description = "单向门禁001操作1" });
            dict2.Add(2, new cards_actions { id = 2, device_id = 5, card_code = "card2", t = 1536215751, code = 0, description = "单向门禁001操作2" });
            dict2.Add(3, new cards_actions { id = 3, device_id = 5, card_code = "card3", t = 1536219362, code = 0, description = "单向门禁001操作3" });
            dict2.Add(4, new cards_actions { id = 4, device_id = 5, card_code = "card4", t = 1536222971, code = 0, description = "单向门禁001操作4" });
            dict2.Add(5, new cards_actions { id = 5, device_id = 5, card_code = "card5", t = 1536226580, code = 0, description = "单向门禁001操作5" });
            dict2.Add(6, new cards_actions { id = 6, device_id = 6, card_code = "card1", t = 1536230188, code = 0, description = "双向门禁001操作1" });
            dict2.Add(7, new cards_actions { id = 7, device_id = 6, card_code = "card2", t = 1536233798, code = 0, description = "双向门禁001操作2" });
            dict2.Add(8, new cards_actions { id = 8, device_id = 6, card_code = "card3", t = 1536237407, code = 0, description = "双向门禁001操作3" });
            dict2.Add(9, new cards_actions { id = 9, device_id = 6, card_code = "card4", t = 1536241015, code = 0, description = "双向门禁001操作4" });
            dict2.Add(10, new cards_actions { id = 10, device_id = 6, card_code = "card5", t = 1536244625, code = 0, description = "双向门禁001操作5" });

        }

        public BaseTran<cards> GetDeviceList()
        {
            BaseTran<cards> send = new BaseTran<cards>();
            List<cards> lst = new List<cards>();

            foreach (var item in dict)
            {
                cards data = new cards();
                data = item.Value.Clone();
                lst.Add(data);
            }

            send.total = lst.Count;
            send.msg = "ok";
            send.data = lst;
            
            return send;
        }
        
        public BaseTran<cards_actions> GetSingleCardActionHistory(string id, string begin_date, string end_date)
        {
            BaseTran<cards_actions> send = new BaseTran<cards_actions>();
            List<cards_actions> lst = new List<cards_actions>();
            List<cards_actions> lst2 = new List<cards_actions>();
            int nId = Convert.ToInt32(id);


            foreach (var item in dict2)
            {
                cards_actions copy = item.Value.Clone();

                if (copy.device_id == nId)
                {
                    lst2.Add(copy);
                }
            }

            if (begin_date != null && end_date != null)
            {
                DateTime dtBegin = DateTime.ParseExact(begin_date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                DateTime dtEnd = DateTime.ParseExact(end_date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
                long begin = (dtBegin.Ticks - startTime.Ticks) / 10000000;
                long end = (dtEnd.Ticks - startTime.Ticks) / 10000000;

                lst = lst2.Where(p => (p.t >= begin && p.t <= end)).ToList();
            }
            else
            {
                lst = lst2;
            }

            send.total = lst.Count;
            send.msg = "ok";
            send.data = lst;

            return send;
        }
    }
}
