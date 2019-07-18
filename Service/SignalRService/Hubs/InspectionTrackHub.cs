using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Work;

namespace SignalRService.Hubs
{
    public class InspectionTrackHub : Hub
    {
        public static void SendInspectionTracks(TModel.Location.Work.InspectionTrackList it)
        {
            SendInspectionTrack(it);
        }

        private static async void SendInspectionTrack(TModel.Location.Work.InspectionTrackList it)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<InspectionTrackHub>();
            await chatHubContext.Clients.All.GetInspectionTrack(it);
        }
    }
}
