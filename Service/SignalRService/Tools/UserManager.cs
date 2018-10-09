using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using SignalRService.Models;

namespace SignalRService.Tools
{
    public static class UserManager
    {
        private static readonly ConcurrentDictionary<string, UserData> _users = new ConcurrentDictionary<string, UserData>();
        private static int _usersCount = 0;

        public static UserData[] GetUsers()
        {
            return _users.Values.ToArray();
        }

        public static UserData NewUser(string userId)
        {
            Interlocked.Increment(ref _usersCount);
            var user = new UserData()
            {
                Id = userId,
                Active = true,
                Name = "user" + _usersCount,
                ConnectedAt = DateTime.Now
            };
            _users[userId] = user;
            return user;
        }

        public static UserData RemoveUser(string userId)
        {
            Interlocked.Decrement(ref _usersCount);
            UserData user;
            if (_users.TryRemove(userId, out user))
            {
                return user;
            }
            return null;
        }

        public static UserData GetUser(string userId)
        {
            UserData user;
            if (_users.TryGetValue(userId, out user))
            {
                return user;
            }
            return null;
        }
    }
}
