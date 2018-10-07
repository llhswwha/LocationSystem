using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRService.Hubs
{
    public class EchoHub: Hub
    {
        private static int _visitors = 0;
        public override async Task OnConnected()
        {
            Interlocked.Increment(ref _visitors);
            await Clients.Others.Message("New connection " + Context.ConnectionId + "! Total visitors: " + _visitors);
            await Clients.Caller.Message("Hey, welcome!");
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Interlocked.Decrement(ref _visitors);
            return Clients.All.Message(Context.ConnectionId + " closed the connection. Current visitors: " + _visitors);
        }

        public Task Broadcast(string message)
        {
            return Clients.All.Message(Context.ConnectionId+"> "+ message);
        }
    }
}