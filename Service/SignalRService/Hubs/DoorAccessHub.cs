using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Data;

namespace SignalRService.Hubs
{
    public class DoorAccessHub:Hub
    {
        /// <summary>
        /// 发送门禁状态信息
        /// </summary>
        /// <param name="alarms"></param>
        public static void SendDoorAccessInfo(params DoorAccessState[] alarms)
        {           
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<DoorAccessHub>();
            chatHubContext.Clients.All.GetDoorAccessInfo(alarms);
        }
    }
}
