using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCommunication.ExtremeVision;

namespace SignalRService.Hubs
{
    /// <summary>
    /// 极视角摄像头行为分析系统告警接口
    /// </summary>
    public class CameraAlarmHub:Hub
    {
        /// <summary>
        /// 发送告警信息
        /// </summary>
        /// <param name="alarms"></param>
        public static void SendInfo(params CameraAlarmInfo[] infos)
        {
            if(infos!=null)
                foreach (var item in infos)
                {
                    item.ParseData();
                }
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<CameraAlarmHub>();
            chatHubContext.Clients.All.GetCameraAlarms(infos);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}
