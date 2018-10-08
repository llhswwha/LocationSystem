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
            HubProxy.On<string, UserData[]>("Welcome", (name, userList) =>
            {
                if (Welcome != null)
                {
                    Welcome(name, userList);
                }
            });

            HubProxy.On<string>("Message", msg =>
            {
                if (Message != null)
                {
                    Message(msg);
                }
            });

            HubProxy.On<UserData>("NewUserNotification", user =>
            {
                if (NewUserNotification != null)
                {
                    NewUserNotification(user);
                }
            });

            HubProxy.On<UserData>("UserDisconnectedNotification", user =>
            {
                if (UserDisconnectedNotification != null)
                {
                    UserDisconnectedNotification(user);
                }
            });

            HubProxy.On<UserData, string>("NicknameChangedNotification", (user,oldName) =>
            {
                if (NicknameChangedNotification != null)
                {
                    NicknameChangedNotification(user, oldName);
                }
            });
        }

        public event Action<string> Message;

        public event Action<string, UserData[]> Welcome;

        public event Action<UserData> NewUserNotification;

        public event Action<UserData> UserDisconnectedNotification;

        public event Action<UserData, string> NicknameChangedNotification;


        public void Send(string msg)
        {
            HubProxy.Invoke("Send", msg);
        }

        public void ChangeNickname(string newName)
        {
            HubProxy.Invoke("ChangeNickname", newName);
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
