using BLL.Tools;
using DAL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Manage;

namespace BLL.Blls.Location
{
    public class UserBll : BaseBll<User, LocationDb>
    {
        public UserBll(LocationDb db) : base(db)
        {

        }

        public UserBll() : base()
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Users;
        }

        public override List<User> ToList(bool isTracking = false)
        {
            var list = base.ToList(isTracking);
            if (list.Count == 0)
            {
                list=Init();
            }

            var admin = list.Find(i => i.Name == "admin" && i.Password == "admin");//兼容原来的admin admin账号
            if (admin == null)
            {
                User user = new User("admin", "admin", "admin");
                DbSet.Add(user);

                list = base.ToList(isTracking);
            }

            return list;
        }

        public List<User> Init()
        {
            var list = new List<User>();
            list.Add(new User("Guest", "Guest@123", "guest"));
            list.Add(new User("Admin", "Admin@123456", "admin"));
            AddRange(list);
            return list;
        }

        public User GetByName(string name)
        {
            var list = ToList();
            User user = this.FirstOrDefault(i => i.Name == name);
            return user;
        }

        public void Login(LoginInfo info)
        {
            User user = GetByName(info.UserName);
            if (user != null)
            {
                if (info.IsEncrypted)
                {
                    if (MD5Encrypter.MD5Encrypt32(user.Password) == info.Password)
                    {
                        info.SetSuccess(user.Authority);
                    }
                    else
                    {
                        info.Result = false;
                    }
                }
                else
                {
                    if (info.Password == user.Password)
                    {
                        info.SetSuccess(user.Authority);
                    }
                    else
                    {
                        info.Result = false;
                    }
                }

                SetUser(user, info);
            }
            else
            {
                info.Result = false;
            }
        }

        private void SetUser(User user, LoginInfo info)
        {
            user.Session = info.Session;
            user.LoginTime = info.LoginTime;
            user.LiveTime = info.LiveTime;
            user.ClientIp = info.ClientIp;
            user.ClientPort = info.ClientPort;
            user.IsEncrypted = info.IsEncrypted;
            user.Result = info.Result;
        }
    }
}
