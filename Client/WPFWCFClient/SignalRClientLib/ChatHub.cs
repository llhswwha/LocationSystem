using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRClientLib
{
    public class ChatHub:HubClient
    {
        public ChatHub(string uri) : base(uri, "ChatHub")
        {
            //HubProxy.On<string, UserData[]>("Welcome", (name,userList) =>
            //{
            //    if (Welcome != null)
            //    {
            //        Welcome(name, userList);
            //    }
            //});

            HubProxy.On<string>("Message", msg =>
            {
                if (Message != null)
                {
                    Message(msg);
                }
            });
        }

        public event Action<string> Message;

        public event Action<string, UserData[]> Welcome;

        public void Send(string msg)
        {
            HubProxy.Invoke("Send", msg);
        }
    }

    public class UserData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public DateTime ConnectedAt { get; set; }

        public string Image { get; set; }
    }
}
