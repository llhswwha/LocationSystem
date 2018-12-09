using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLocation.Controllers
{
    public class ArgController : Controller
    {
        // GET: Arg            
        public string GetLoginInfo()       
        {
            string ip = "127.0.0.1"; //动态获取后端Ip
            string port = "8733";
            string user = "admin";
            string pass = "admin";
            //return ""

            var locations = ip + "|" + port + "|" + user + "|" + pass;  
            return locations;  //  127.0.0.1|8733|admin|admin
        }
    }
}