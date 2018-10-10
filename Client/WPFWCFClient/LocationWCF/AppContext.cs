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

        public WCFClientLib.LocationClient Client { get; set; }

        public LocationCallbackClient CallbackClient { get; set; }

        public LoginInfo LoginInfo { get; set; }

        public string Ip { get; set; }

        public string Port { get; set; }

        public string GetWebApiUrl()
        {
            return string.Format("http://{0}:{1}/api", Ip, Port);
        }

        public bool Login(string ip, string port, WCFClientHostType hostType,  string user, string pass)
        {
            this.Ip = ip;
            this.Port = port;
            SignalRClientLib.SignalRAppContext.SetUrl(ip, port);
            try
            {
                WCFClientLib.LocationClient client = new WCFClientLib.LocationClient(ip, port, hostType);

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
