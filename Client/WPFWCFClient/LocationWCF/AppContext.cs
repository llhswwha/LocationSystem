using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Manage;
using WCFClientLib;
using WCFServiceForWPF.LocationServices;

namespace LocationWCFClient
{
    public class AppContext
    {
        private static AppContext _instance ;
        public static AppContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance=new AppContext();
                }
                return _instance;
            }
        }

        private AppContext()
        {
            
        }

        public LocationClient Client { get; set; }

        public LocationCallbackClient CallbackClient { get; set; }

        public LoginInfo LoginInfo { get; set; }

        public bool Login(string ip, string port,  string user, string pass)
        {
            try
            {
                LocationClient client = new LocationClient(ip, port);
                LoginInfo = client.InnerClient.Login(new LoginInfo() { UserName = user, Password = pass });
                Client = client;
                bool isSuccess= LoginInfo != null && !string.IsNullOrEmpty(LoginInfo.Session);
                if (isSuccess)
                {
                    CallbackClient = new LocationCallbackClient(ip, "8734");
                    CallbackClient.Connect();
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
    }
}
