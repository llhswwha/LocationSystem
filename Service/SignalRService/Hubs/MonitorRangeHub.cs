using Location.TModel.Location.AreaAndDev;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRService.Hubs
{
    public class MonitorRangeHub : Hub
    {
        public static void SendMonitorRangeInfos(List<PhysicalTopology>rangeList )
        {
            SendMonitorRangeInfosAsync(rangeList);
        }

        private static async void SendMonitorRangeInfosAsync(List<PhysicalTopology> rangeList)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<MonitorRangeHub>();
            await chatHubContext.Clients.All.GetMonitorRangeInfos(rangeList);
        }
    }
}
