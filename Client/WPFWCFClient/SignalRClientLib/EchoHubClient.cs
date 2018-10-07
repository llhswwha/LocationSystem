using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRService.HubClients
{
    public class EchoHubClient:HubClient
    {
        public EchoHubClient(string uri) : base(uri, "EchoHub")
        {
            HubProxy.On<string>("Message", msg=>
            {
                if (Message != null)
                {
                    Message(msg);
                }
            });
        }

        public event Action<string> Message;

        public void Broadcast(string msg)
        {
            HubProxy.Invoke("Broadcast", msg);
        }
    }
}
