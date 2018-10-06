using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Tavis;
using Tavis.Home;

namespace WebApiService.Controllers
{
    public class HomeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var home = new HomeDocument();
            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new HomeContent(home)
            };

            //var areaLink = new Link()
            //{

            //}
        }
    }
}
