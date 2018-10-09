using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using SignalRService.Models;
using SignalRService.Tools;

namespace SignalRService.Hubs
{
    public class ChatHub
        :Hub
    {
        public override Task OnConnected()
        {
            UserData user=UserManager.NewUser(Context.ConnectionId);

            var notifyAll = (Task)Clients.All.NewUserNotification(user);
            var sendAllUsers = (Task)Clients.Caller.Welcome(user.Name, UserManager.GetUsers());
            return notifyAll.ContinueWith(_ => sendAllUsers);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserData user=UserManager.RemoveUser(Context.ConnectionId);
            if (user!=null)
            {
                return Clients.All.UserDisconnectedNotification(user);
            }
            return base.OnDisconnected(stopCalled);
        }

        public Task ChangeNickname(string newName)
        {
            UserData user = UserManager.GetUser(Context.ConnectionId);
            if (user!=null)
            {
                var oldName = user.Name;
                user.Name = newName;
                return Clients.All.NicknameChangedNotification(user, oldName);
            }
            return null;
        }

        public Task Send(string message)
        {
            UserData user = UserManager.GetUser(Context.ConnectionId);
            if (user != null)
            {
                var msgToSend = string.Format("[{0}]:{1}", user.Name, message);
                return Clients.All.Message(msgToSend);
            }
            return null;
        }

        public static void SendToAll(string msg)
        {
            IHubContext chatHubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            chatHubContext.Clients.All.Message(msg);
        }
    }


}