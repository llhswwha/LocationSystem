using LocationServices.Locations.Interfaces;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TModel.Location.Manage;

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



        public VersionInfo GetVersionInfo()
        {
            throw new NotImplementedException();
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

    }
}
