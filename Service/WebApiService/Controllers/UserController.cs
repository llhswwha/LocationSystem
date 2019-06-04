using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        [Route("HeartBeat/{info}")]
        [HttpGet]
        public string HeartBeat(string info)
        {
            return info;
        }
    }
}
