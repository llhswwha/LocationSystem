using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TModel.Location.Manage;
using DbModel.Location.Manage;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController,IUserService
    {
        protected IUserService service;

        public UserController()
        {
            service = new UserService();
        }

        public LoginInfo Delete(string id)
        {
            return service.Delete(id);
        }

        public LoginInfo GetEntity(string id)
        {
            throw new NotImplementedException();
        }

        public List<LoginInfo> GetList()
        {
            throw new NotImplementedException();
        }

        [Route("Version")]
        public VersionInfo GetVersionInfo()
        {
            return service.GetVersionInfo();
        }

        [Route("HeartBeat/{info}")]
        [HttpGet]
        public string HeartBeat(string info)
        {
            return info;
        }
        [Route("KeepLive")]
        [HttpGet]
        public LoginInfo KeepLive(LoginInfo info)
        {
            return service.KeepLive(info);
        }
        [Route("KeepLivePost")]
        [HttpPost]
        public LoginInfo KeepLivePost(LoginInfo info)
        {
            return service.KeepLive(info);
        }


        [Route("Login")]
        [HttpGet]
        public LoginInfo Login(LoginInfo info)
        {
            return service.Login(info);
        }
        [Route("LoginPost")]
        [HttpPost]
        public LoginInfo LoginPost(LoginInfo info)
        {
            return service.Login(info);
        }


        [Route("Logout")]
        [HttpGet]
        public LoginInfo Logout(LoginInfo info)
        {
            return service.Logout(info);
        }

        [Route("LogoutPost")]
        [HttpPost]
        public LoginInfo LogoutPost(LoginInfo info)
        {
            return service.Logout(info);
        }

        public LoginInfo Post(LoginInfo item)
        {
            return service.Post(item);
        }

        public LoginInfo Put(LoginInfo item)
        {
            throw new NotImplementedException();
        }

        public LoginInfo Update(User user)
        {
            return service.Update(user);
        }
    }
}
