
using BLL;
using BLL.Blls.Location;
using System;
using System.Collections.Generic;
using System.Configuration;
using TModel.Location.Manage;
using TEntity = TModel.Location.Manage.LoginInfo;

namespace LocationServices.Locations.Services
{

    public interface IUserService
    {
        TEntity Login(TEntity info);

        TEntity Logout(TEntity info);

        TEntity KeepLive(TEntity info);

        VersionInfo GetVersionInfo();
    }
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
            VersionInfo version = new VersionInfo();
            version.Version = ConfigurationManager.AppSettings["ClientVersionCode"];
            version.LocationURL = ConfigurationManager.AppSettings["LocationPackageURL"];
            return version;
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
