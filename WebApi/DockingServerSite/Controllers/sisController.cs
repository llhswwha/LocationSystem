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
    [ServiceContract(Name = "sisController")]
    public interface IsisController
    {
        [OperationContract]
        [WebGet(UriTemplate = "?kks={kks}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        BaseTran<sis> GetSomesisList(string kks);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class sisController : IsisController
    {
        private static Dictionary<int, sis> dict = new Dictionary<int, sis>();

        public static void Init()
        {
            dict.Add(1, new sis { kks = "SCSB1", value = "10" });
            dict.Add(2, new sis { kks = "SXA1", value = "16" });
            dict.Add(3, new sis { kks = "SXB1", value = "78" });
            dict.Add(4, new sis { kks = "SXC1", value = "32" });
            dict.Add(5, new sis { kks = "MJA1", value = "9" });
            dict.Add(6, new sis { kks = "MJB1", value = "55" });
            dict.Add(7, new sis { kks = "XFSB1", value = "78" });
            dict.Add(8, new sis { kks = "WHP1", value = "3" });
            dict.Add(9, new sis { kks = "DWJZ1", value = "65" });
            dict.Add(10, new sis { kks = "XJD1", value = "43" });
            dict.Add(11, new sis { kks = "TCW1", value = "47" });
            dict.Add(12, new sis { kks = "YDSBA1", value = "6" });
            dict.Add(13, new sis { kks = "YDSBB1", value = "59" });
            dict.Add(14, new sis { kks = "YDSBC1", value = "78" });
            dict.Add(15, new sis { kks = "SCSB2", value = "43" });
            dict.Add(16, new sis { kks = "SXA2", value = "65" });
            dict.Add(17, new sis { kks = "SXB2", value = "7" });
            dict.Add(18, new sis { kks = "SXC2", value = "64" });

        }

        public BaseTran<sis> GetSomesisList(string kks)
        {
            BaseTran<sis> send = new BaseTran<sis>();
            List<sis> lst = new List<sis>();
            Dictionary<string, string> dictCom = new Dictionary<string, string>();

            string[] kkss = null;
            if (kks != null)
            {
                kkss = kks.Split(',');

                for (int i=0; i < kkss.Length; i++)
                {
                    string val = kkss[i];

                    if (!dictCom.ContainsKey(val))
                    {
                        dictCom.Add(val, val);
                    }
                }
            }

            foreach (var item in dict)
            {
                sis val = item.Value.Clone();
                if (dictCom.ContainsKey(val.kks))
                {
                    lst.Add(val);
                }
            }

            send.total = lst.Count;
            send.msg = "ok";
            send.data = lst;

            return send;

        }
    }
}
