using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Threading;

namespace SimpleChat2
{
    public class ChatHub:Hub
    {
        private static ConcurrentDictionary<string, UserData> _users = new ConcurrentDictionary<string, UserData>();
        private static int _usersCount = 0;

        public override Task OnConnected()
        {
            Interlocked.Increment(ref _usersCount);
            var user = new UserData()
            {
                Id = Context.ConnectionId,
                Active = true,
                Name = "user" + _usersCount,
                Image = GravatarHelpers.GetImage(null),
                ConnectedAt = DateTime.Now
            };
            _users[Context.ConnectionId] = user;
            var notifyAll = (Task)Clients.All.NewUserNotification(user);
            var sendAllUsers = (Task)Clients.Caller.Welcome(user.Name, _users.Values.ToArray());
            return notifyAll.ContinueWith(_ => sendAllUsers);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserData user;
            if(_users.TryRemove(Context.ConnectionId,out user))
            {
                return Clients.All.UserDisconnectedNotification(user);
            }
            return base.OnDisconnected(stopCalled);
        }

        public Task ChangeNickname(string newName)
        {
            UserData user;
            if(_users.TryGetValue(Context.ConnectionId,out user))
            {
                var oldName = user.Name;
                user.Name = newName;
                return Clients.All.NicknameChangedNotification(user, oldName);
            }
            return null;
        }

        public Task ChangeImage(string email)
        {
            UserData user;
            if(_users.TryGetValue(Context.ConnectionId,out user))
            {
                user.Image = GravatarHelpers.GetImage(email);
                return Clients.All.ImageChangedNotification(user);
            }
            return null;
        }

        public Task Send(string message)
        {
            UserData user;
            if(_users.TryGetValue(Context.ConnectionId,out user))
            {
                var msgToSend = string.Format("[{0}]:{1}", user.Name, message);
                return Clients.All.Message(msgToSend);
            }
            return null;
        }
    }

    internal class UserData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public DateTime ConnectedAt { get; set; }

        public string Image { get; set; }
    }
}