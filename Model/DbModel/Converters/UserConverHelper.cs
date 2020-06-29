using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntity = TModel.Location.Manage.LoginInfo;

namespace DbModel.Converters
{
  public static   class UserConverHelper
    {
        public  static  Location.Manage.User ToDbModel(this TEntity item)
        {
            if (item == null) return null;
            var user = new Location.Manage.User();
            user.Name = item.UserName;
            user.Password = item.Password;
            user.IsEncrypted = item.IsEncrypted;
            user.Session = item.Session;
            user.Authority = item.Authority;
            user.Result = item.Result;
            user.ClientIp = item.ClientIp;
            user.ClientPort = item.ClientPort;
            user.LoginTime = item.LoginTime;
            user.LiveTime = item.LiveTime;
            return user;
        }


        public static TEntity ToTModel(this Location.Manage.User user)
        {
            if (user == null) return null;
            var entity = new TEntity();
            entity.UserName = user.Name;
            entity.Password = user.Password;
            entity.IsEncrypted = user.IsEncrypted;
            entity.Session = user.Session;
            entity.Authority = user.Authority;
            entity.Result = user.Result;
            entity.ClientIp = user.ClientIp;
            entity.ClientPort = user.ClientPort;
            entity.LoginTime = user.LoginTime;
            entity.LiveTime = user.LiveTime;
            return entity;
        }
    }
}
