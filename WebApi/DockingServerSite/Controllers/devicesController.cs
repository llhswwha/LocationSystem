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
    [ServiceContract(Name = "devicesController")]
    public interface IdevicesController
    {
        [OperationContract]
        [WebGet(UriTemplate = "?types={types}&code={code}&name={name}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        BaseTran<devices> GetDeviceList(string types, string code, string name);

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        devices GetSingleDeviceInfo(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/{id}/actions?begin_date={begin_date}&end_date={end_date}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        BaseTran<devices_actions> GetSingleDeviceActionHistory(string id, string begin_date, string end_date);


    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class devicesController : IdevicesController
    {
        private static Dictionary<int, devices> dict;
        private static Dictionary<int, devices_actions> dict2;
        private static Dictionary<int, List<int>> dict3;


        public static void Init()
        {
            dict = new Dictionary<int, devices>();
            dict2 = new Dictionary<int, devices_actions>();
            dict3 = new Dictionary<int, List<int>>();

            dict.Add(1, new devices { id = 1, name = "生产设备001", code = "01", kks = "SCSB1", type = 101, state = 0, running_state = 0, placed = true, raw_id = "1" });
            dict.Add(2, new devices { id = 2, name = "枪机摄像头001", code = "02", kks = "SXA1", type = 1021, state = 0, running_state = 0, placed = true, raw_id = "2" });
            dict.Add(3, new devices { id = 3, name = "球机摄像头001", code = "02", kks = "SXB1", type = 1022, state = 0, running_state = 0, placed = true, raw_id = "3" });
            dict.Add(4, new devices { id = 4, name = "半球摄像头001", code = "02", kks = "SXC1", type = 1023, state = 0, running_state = 0, placed = true, raw_id = "4" });
            dict.Add(5, new devices { id = 5, name = "单向门禁001", code = "03", kks = "MJA1", type = 1031, state = 0, running_state = 0, placed = true, raw_id = "5" });
            dict.Add(6, new devices { id = 6, name = "双向门禁001", code = "03", kks = "MJB1", type = 1032, state = 0, running_state = 0, placed = true, raw_id = "6" });
            dict.Add(7, new devices { id = 7, name = "消防设备001", code = "04", kks = "XFSB1", type = 104, state = 0, running_state = 0, placed = true, raw_id = "7" });
            dict.Add(8, new devices { id = 8, name = "危化品001", code = "05", kks = "WHP1", type = 105, state = 0, running_state = 0, placed = true, raw_id = "8" });
            dict.Add(9, new devices { id = 9, name = "定位基站001", code = "06", kks = "DWJZ1", type = 106, state = 0, running_state = 0, placed = true, raw_id = "9" });
            dict.Add(10, new devices { id = 10, name = "巡检点001", code = "07", kks = "XJD1", type = 107, state = 0, running_state = 0, placed = true, raw_id = "10" });
            dict.Add(11, new devices { id = 11, name = "停车位001", code = "08", kks = "TCW1", type = 108, state = 0, running_state = 0, placed = true, raw_id = "11" });
            dict.Add(12, new devices { id = 12, name = "一卡通001", code = "09", kks = "YDSBA1", type = 201, state = 0, running_state = 0, placed = true, raw_id = "12" });
            dict.Add(13, new devices { id = 13, name = "人员定位终端001", code = "09", kks = "YDSBB1", type = 202, state = 0, running_state = 0, placed = true, raw_id = "13" });
            dict.Add(14, new devices { id = 14, name = "移动终端001", code = "09", kks = "YDSBC1", type = 203, state = 0, running_state = 0, placed = true, raw_id = "14" });
            dict.Add(15, new devices { id = 15, name = "生产设备002", code = "01", kks = "SCSB2", type = 101, state = 0, running_state = 0, placed = true, raw_id = "15" });
            dict.Add(16, new devices { id = 16, name = "枪机摄像头002", code = "02", kks = "SXA2", type = 1021, state = 0, running_state = 0, placed = true, raw_id = "16" });
            dict.Add(17, new devices { id = 17, name = "球机摄像头002", code = "02", kks = "SXB2", type = 1022, state = 0, running_state = 0, placed = true, raw_id = "17" });
            dict.Add(18, new devices { id = 18, name = "半球摄像头002", code = "02", kks = "SXC2", type = 1023, state = 0, running_state = 0, placed = true, raw_id = "18" });


            dict2.Add(1, new devices_actions { id = 1, device_id = 5, card_code = "card1", t = 1536212067, code = 0, description = "单向门禁001操作1" });
            dict2.Add(2, new devices_actions { id = 2, device_id = 5, card_code = "card2", t = 1536215751, code = 0, description = "单向门禁001操作2" });
            dict2.Add(3, new devices_actions { id = 3, device_id = 5, card_code = "card3", t = 1536219362, code = 0, description = "单向门禁001操作3" });
            dict2.Add(4, new devices_actions { id = 4, device_id = 5, card_code = "card4", t = 1536222971, code = 0, description = "单向门禁001操作4" });
            dict2.Add(5, new devices_actions { id = 5, device_id = 5, card_code = "card5", t = 1536226580, code = 0, description = "单向门禁001操作5" });
            dict2.Add(6, new devices_actions { id = 6, device_id = 6, card_code = "card1", t = 1536230188, code = 0, description = "双向门禁001操作1" });
            dict2.Add(7, new devices_actions { id = 7, device_id = 6, card_code = "card2", t = 1536233798, code = 0, description = "双向门禁001操作2" });
            dict2.Add(8, new devices_actions { id = 8, device_id = 6, card_code = "card3", t = 1536237407, code = 0, description = "双向门禁001操作3" });
            dict2.Add(9, new devices_actions { id = 9, device_id = 6, card_code = "card4", t = 1536241015, code = 0, description = "双向门禁001操作4" });
            dict2.Add(10, new devices_actions { id = 10, device_id = 6, card_code = "card5", t = 1536244625, code = 0, description = "双向门禁001操作5" });



            dict3.Add(100, new List<int>() { 101, 102, 103, 104, 105, 106, 107, 108});
            dict3.Add(101, new List<int>() { 101});
            dict3.Add(102, new List<int>() { 1021, 1022, 1023});
            dict3.Add(103, new List<int>() { 1031, 1032});
            dict3.Add(104, new List<int>() { 104});
            dict3.Add(105, new List<int>() { 105});
            dict3.Add(106, new List<int>() { 106});
            dict3.Add(107, new List<int>() { 107});
            dict3.Add(108, new List<int>() { 108});
            dict3.Add(200, new List<int>() { 201, 202, 203});
            dict3.Add(201, new List<int>() { 201});
            dict3.Add(202, new List<int>() { 202});
            dict3.Add(203, new List<int>() { 203});
            dict3.Add(1021, new List<int>(){ 1021});
            dict3.Add(1022, new List<int>(){ 1022});
            dict3.Add(1023, new List<int>(){ 1023});
            dict3.Add(1031, new List<int>(){ 1031});
            dict3.Add(1032, new List<int>(){ 1032});
            
        }


        [HttpGet]
        public BaseTran<devices> GetDeviceList(string types, string code, string name)
        {
            BaseTran<devices> send = new BaseTran<devices>();
            List<devices> lst = new List<devices>();
            string[] type = null;
            Dictionary<int, int> dictKey = new Dictionary<int, int>();
            Dictionary<int, int> dictCom = new Dictionary<int, int>();
            int[] ntype = null;

            if (types != null)
            {
                type = types.Split(',');
                ntype = new int[type.Length];
                foreach (string t in type)
                {
                    int nt = Convert.ToInt32(t);
                    if (dict3.ContainsKey(nt))
                    {
                        List<int> val = dict3[nt];
                        foreach (int i in val)
                        {
                            if (!dictKey.ContainsKey(i))
                            {
                                dictKey.Add(i,i);
                            }
                        }
                    }
                }

                foreach (var item in dictKey)
                {
                    if (dict3.ContainsKey(item.Key))
                    {
                        List<int> val = dict3[item.Key];
                        foreach (int i in val)
                        {
                            if (!dictCom.ContainsKey(i))
                            {
                                dictCom.Add(i, i);
                            }
                        }
                    }
                }
            }
            
            foreach (var item in dict)
            {
                devices dev1 = item.Value.Clone();
                devices dev2 = null;

                if (dictCom.ContainsKey(dev1.type))
                {
                    dev2 = dev1.Clone();
                }

                if (code != null)
                {
                    if (code == dev1.code)
                    {
                        dev2 = dev1.Clone();
                    }
                    else
                    {
                        dev2 = null;
                    }
                }


                if (name != null)
                {
                    if (name == dev1.name)
                    {
                        dev2 = dev1.Clone();
                    }
                    else
                    {
                        dev2 = null;
                    }
                }

                if (dev2 != null)
                {
                    lst.Add(dev2);
                }
            }
            

            send.total = lst.Count;
            send.msg = "ok";
            send.data = lst;

            return send;

        }

        [HttpGet]
        public devices GetSingleDeviceInfo(string id)
        {
            devices send = new devices();
            int nId = Convert.ToInt32(id);

            if (dict.ContainsKey(nId))
            {
                send = dict[nId];
            }

            return send;
        }

        [HttpGet]
        public BaseTran<devices_actions> GetSingleDeviceActionHistory(string id, string begin_date, string end_date)
        {
            BaseTran<devices_actions> send = new BaseTran<devices_actions>();
            List<devices_actions> lst = new List<devices_actions>();
            List<devices_actions> lst2 = new List<devices_actions>();
            int nId = Convert.ToInt32(id);

            foreach (var item in dict2)
            {
                devices_actions copy = item.Value.Clone();

                if (copy.device_id == nId)
                {
                    lst2.Add(copy);
                }
            }

            if (begin_date != null && end_date != null)
            {
                DateTime dtBegin = DateTime.ParseExact(begin_date,"yyyyMMdd",System.Globalization.CultureInfo.CurrentCulture);
                DateTime dtEnd = DateTime.ParseExact(end_date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1,0,0,0));
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
