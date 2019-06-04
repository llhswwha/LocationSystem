using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLocation.Tools;

namespace WebLocation.Controllers
{
    public class ArgController : Controller
    {
        // GET: Arg            
        public string GetLoginInfo()       
        {
            string ip = ConfigurationHelper.GetValue("Ip"); //动态获取后端Ip
            //string ip = ConfigurationManager.AppSettings["Ip"];
            //ConfigurationManager.AppSettings[key]
            string port = ConfigurationHelper.GetValue("Port");
            string user = ConfigurationHelper.GetValue("User");
            string pass = ConfigurationHelper.GetValue("Password");

            var locations = ip + "|" + port + "|" + user + "|" + pass;  
            return locations;  //  127.0.0.1|8733|admin|admin
        }

        // GET: Arg            
        public string GetLoginInfo_Guest()
        {
            string ip = ConfigurationHelper.GetValue("Ip"); //动态获取后端Ip
            //string ip = ConfigurationManager.AppSettings["Ip"];
            //ConfigurationManager.AppSettings[key]
            string port = ConfigurationHelper.GetValue("Port");
            string user = "Guest";// ConfigurationHelper.GetValue("Guset");//写死的
            string pass = "Guest@123";// ConfigurationHelper.GetValue("Guset@123");

            var locations = ip + "|" + port + "|" + user + "|" + pass;
            return locations;  //  127.0.0.1|8733|admin|admin
        }
    }
}