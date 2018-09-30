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
    [ServiceContract(Name = "zonesController")]
    public interface IzonesController
    {
        [OperationContract]
        [WebGet(UriTemplate = "?struc={struc}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        BaseTran<zones> GetzonesList(string struc);

        [OperationContract]
        [WebGet(UriTemplate = "/{id}?view={view}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        zones GetSingleZonesInfo(string id, int view);

        [OperationContract]
        [WebGet(UriTemplate = "/{id}/devices", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        BaseTran<devices> GetZoneDevList(string id);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class zonesController : IzonesController
    {
        private static Dictionary<int, zones> dict;

        public static void Init()
        {
            dict = new Dictionary<int, zones>();

            dict.Add(1, new zones() { id = 1, name = "莘科楼房", kks = "SHENKE", description = "莘科楼房", x = 0, y = 0, z = 0, parent_id = null, path = "", lstzon = new List<zones>(), lstdev = new List<devices>() });
            dict.Add(2, new zones() { id = 2, name = "一楼", kks = "FIRSTFLOOR", description = "一楼", x = 0, y = 0, z = 0, parent_id = 1, path = "1", lstzon = new List<zones>(), lstdev = new List<devices>() });
            dict.Add(3, new zones() { id = 3, name = "一楼A房间", kks = "FIRSTFLOORA", description = "一楼A房间", x = 0.5f, y = 0.5f, z = 0f, parent_id = 2, path = "[1/]2", lstzon = new List<zones>(), lstdev = new List<devices>() });
            dict.Add(4, new zones() { id = 4, name = "一楼B房间", kks = "FIRSTFLOORB", description = "一楼B房间", x = 1.0f, y = 1.0f, z = 0f, parent_id = 2, path = "[1/]2", lstzon = new List<zones>(), lstdev = new List<devices>() });
            dict.Add(5, new zones() { id = 5, name = "一楼C房间", kks = "FIRSTFLOORC", description = "一楼C房间", x = 1.5f, y = 1.5f, z = 0f, parent_id = 2, path = "[1/]2", lstzon = new List<zones>(), lstdev = new List<devices>() });
            dict.Add(6, new zones() { id = 6, name = "二楼", kks = "SECONDFLOOR", description = "二楼", x = 0, y = 0, z = 0, parent_id = 1, path = "1", lstzon = new List<zones>(), lstdev = new List<devices>() });
            dict.Add(7, new zones() { id = 7, name = "二楼A房间", kks = "SECONDFLOORA", description = "二楼A房间", x = 0.5f, y = 0.5f, z = 1.0f, parent_id = 6, path = "[1/]6", lstzon = new List<zones>(), lstdev = new List<devices>() });
            dict.Add(8, new zones() { id = 8, name = "二楼B房间", kks = "SECONDFLOORB", description = "二楼B房间", x = 1.0f, y = 1.0f, z = 1.0f, parent_id = 6, path = "[1/]6", lstzon = new List<zones>(), lstdev = new List<devices>() });
            dict.Add(9, new zones() { id = 9, name = "二楼C房间", kks = "SECONDFLOORC", description = "二楼C房间", x = 1.5f, y = 1.5f, z = 1.0f, parent_id = 6, path = "[1/]6", lstzon = new List<zones>(), lstdev = new List<devices>() });

            dict[1].lstzon.Add(new zones() { id = 2, name = "一楼", kks = "FIRSTFLOOR", description = "一楼", x = 0, y = 0, z = 0, parent_id = 1, path = "1" });
            dict[1].lstzon.Add(new zones() { id = 6, name = "二楼", kks = "SECONDFLOOR", description = "二楼", x = 0, y = 0, z = 0, parent_id = 1, path = "1" });
            dict[2].lstzon.Add(new zones() { id = 3, name = "一楼A房间", kks = "FIRSTFLOORA", description = "一楼A房间", x = 0.5f, y = 0.5f, z = 0f, parent_id = 2, path = "[1/]2" });
            dict[2].lstzon.Add(new zones() { id = 4, name = "一楼B房间", kks = "FIRSTFLOORB", description = "一楼B房间", x = 1.0f, y = 1.0f, z = 0f, parent_id = 2, path = "[1/]2" });
            dict[2].lstzon.Add(new zones() { id = 5, name = "一楼C房间", kks = "FIRSTFLOORC", description = "一楼C房间", x = 1.5f, y = 1.5f, z = 0f, parent_id = 2, path = "[1/]2" });
            dict[6].lstzon.Add(new zones() { id = 7, name = "二楼A房间", kks = "SECONDFLOORA", description = "二楼A房间", x = 0.5f, y = 0.5f, z = 1.0f, parent_id = 6, path = "[1/]6" });
            dict[6].lstzon.Add(new zones() { id = 8, name = "二楼B房间", kks = "SECONDFLOORB", description = "二楼B房间", x = 1.0f, y = 1.0f, z = 1.0f, parent_id = 6, path = "[1/]6" });
            dict[6].lstzon.Add(new zones() { id = 9, name = "二楼C房间", kks = "SECONDFLOORC", description = "二楼C房间", x = 1.5f, y = 1.5f, z = 1.0f, parent_id = 6, path = "[1/]6" });



            dict[1].lstdev.Add(new devices { id = 1, name = "生产设备001", code = "01", kks = "SCSB1", type = 101, state = 0, running_state = 0, placed = true, raw_id = "1" });
            dict[1].lstdev.Add(new devices { id = 2, name = "枪机摄像头001", code = "02", kks = "SXA1", type = 1021, state = 0, running_state = 0, placed = true, raw_id = "2" });
            dict[2].lstdev.Add(new devices { id = 3, name = "球机摄像头001", code = "02", kks = "SXB1", type = 1022, state = 0, running_state = 0, placed = true, raw_id = "3" });
            dict[2].lstdev.Add(new devices { id = 4, name = "半球摄像头001", code = "02", kks = "SXC1", type = 1023, state = 0, running_state = 0, placed = true, raw_id = "4" });
            dict[3].lstdev.Add(new devices { id = 5, name = "单向门禁001", code = "03", kks = "MJA1", type = 1031, state = 0, running_state = 0, placed = true, raw_id = "5" });
            dict[3].lstdev.Add(new devices { id = 6, name = "双向门禁001", code = "03", kks = "MJB1", type = 1032, state = 0, running_state = 0, placed = true, raw_id = "6" });
            dict[4].lstdev.Add(new devices { id = 7, name = "消防设备001", code = "04", kks = "XFSB1", type = 104, state = 0, running_state = 0, placed = true, raw_id = "7" });
            dict[4].lstdev.Add(new devices { id = 8, name = "危化品001", code = "05", kks = "WHP1", type = 105, state = 0, running_state = 0, placed = true, raw_id = "8" });
            dict[5].lstdev.Add(new devices { id = 9, name = "定位基站001", code = "06", kks = "DWJZ1", type = 106, state = 0, running_state = 0, placed = true, raw_id = "9" });
            dict[5].lstdev.Add(new devices { id = 10, name = "巡检点001", code = "07", kks = "XJD1", type = 107, state = 0, running_state = 0, placed = true, raw_id = "10" });
            dict[6].lstdev.Add(new devices { id = 11, name = "停车位001", code = "08", kks = "TCW1", type = 108, state = 0, running_state = 0, placed = true, raw_id = "11" });
            dict[6].lstdev.Add(new devices { id = 12, name = "一卡通001", code = "09", kks = "YDSBA1", type = 201, state = 0, running_state = 0, placed = true, raw_id = "12" });
            dict[7].lstdev.Add(new devices { id = 13, name = "人员定位终端001", code = "09", kks = "YDSBB1", type = 202, state = 0, running_state = 0, placed = true, raw_id = "13" });
            dict[7].lstdev.Add(new devices { id = 14, name = "移动终端001", code = "09", kks = "YDSBC1", type = 203, state = 0, running_state = 0, placed = true, raw_id = "14" });
            dict[8].lstdev.Add(new devices { id = 15, name = "生产设备002", code = "01", kks = "SCSB2", type = 101, state = 0, running_state = 0, placed = true, raw_id = "15" });
            dict[8].lstdev.Add(new devices { id = 16, name = "枪机摄像头002", code = "02", kks = "SXA2", type = 1021, state = 0, running_state = 0, placed = true, raw_id = "16" });
            dict[9].lstdev.Add(new devices { id = 17, name = "球机摄像头002", code = "02", kks = "SXB2", type = 1022, state = 0, running_state = 0, placed = true, raw_id = "17" });
            dict[9].lstdev.Add(new devices { id = 18, name = "半球摄像头002", code = "02", kks = "SXC2", type = 1023, state = 0, running_state = 0, placed = true, raw_id = "18" });

            return;
        }

        public BaseTran<zones> GetzonesList(string struc)
        {
            BaseTran<zones> send = new BaseTran<zones>();
            List<zones> lst = new List<zones>();
            

            foreach (var item in dict)
            {
                zones data = new zones();
                data = item.Value.Clone();
                data.lstdev = null;
                data.lstzon = null;
                lst.Add(data);
            }

            send.total = lst.Count;
            send.msg = "ok";
            send.data = lst;

            return send;
            
        }

        
        public zones GetSingleZonesInfo(string id, int view)
        {
            zones send = new zones();
            int nId = Convert.ToInt32(id);

            if (dict.ContainsKey(nId))
            {
                send = dict[nId].Clone();
            }

            if (view == 0)
            {
                send.lstdev = null;
                send.lstzon = null;
            }
            else if (view == 1)
            {
                send.lstdev = null;
            }
            else if (view == 2)
            {
                send.lstzon = null;
            }
            
            return send;
        }

        
        public BaseTran<devices> GetZoneDevList(string id)
        {
            BaseTran<devices> send = new BaseTran<devices>();
            List<devices> lst = new List<devices>();

            zones data = new zones();
            int nId = Convert.ToInt32(id);

            if (dict.ContainsKey(nId))
            {
                data = dict[nId].Clone();
            }

            lst = data.lstdev;

            send.total = lst.Count;
            send.msg = "ok";
            send.data = lst;

            return send;
        }
    }
}
