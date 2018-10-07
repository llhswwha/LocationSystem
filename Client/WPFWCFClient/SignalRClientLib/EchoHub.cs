using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRClientLib
{
    public class EchoHub:HubClient
    {
        public EchoHub(string uri) : base(uri, "EchoHub")
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
