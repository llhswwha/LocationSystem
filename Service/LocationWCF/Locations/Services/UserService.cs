
using BLL;
using BLL.Blls.Location;
using BLL.Tools;
using DbModel.Location.Manage;
using LocationServices.Locations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Manage;
using TEntity = TModel.Location.Manage.LoginInfo;

namespace LocationServices.Locations.Services
{

  public  class UserService:IUserService
    {
        private Bll db;
        private UserBll dbSet;
        public static List<LoginInfo> loginInfos = new List<LoginInfo>();

        public UserService()
        {
            db = Bll.NewBllNoRelation();
            dbSet = db.Users;
        }

        public UserService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Users;
        }

        public VersionInfo GetVersionInfo()
        {
            throw new NotImplementedException();
        }

        public TEntity KeepLive(TEntity info)
        {
            var login = loginInfos.Find(i => i.Session == info.Session);
            if (login == null)
            {
                info.Result = false;
            }
            else
            {
                info.Result = true;
                login.LiveTime = DateTime.Now;
            }
            return info;
        }

        public TEntity Login(TEntity info)
        {
            //44874848
            //RemoteEndpointMessageProperty endpoint = GetClientEndPoint();
            //if (endpoint != null)
            //{
            //    info.ClientIp = endpoint.Address;
            //    info.ClientPort = endpoint.Port;
            //}
           db.Users.Login(info);
            //ShowLogEx(">>>>> Login !!!!!!!!!!!!!!!!!!!! :" + info.Session);
            return info;
        }

        public TEntity Logout(TEntity info)
        {
            var login = loginInfos.Find(i => i.Session == info.Session);
            if (login == null)
            {
                info.Result = false;
            }
            else
            {
                info.Result = true;
                loginInfos.Remove(login);
            }
            info.Session = "";
            return info;
        }
    }
}
