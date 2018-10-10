using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WCFServiceForWPF.LocationServices;

namespace WCFClientLib
{
    public class LocationClient:WCFClient
    {


        public LocationServiceClient InnerClient { get; set; }

        public LocationClient(string host, string port,WCFClientHostType hostType):base(host,port, hostType)
        {
            SetConnectInfo();
        }

        public LocationClient(string url):base(url)
        {
            InitClient();
        }

        protected void SetConnectInfo()
        {
            string servicePath = "";
            if (HostType == WCFClientHostType.Self)
            {
                servicePath = "LocationService";
            }
            else
            {
                servicePath = "Services/LocationService.svc";
            }
            Url =
    string.Format("http://{0}:{1}/{2}",
        Host, Port, servicePath);

            InitClient();
        }

        private void InitClient()
        {
            BasicHttpBinding wsBinding = new BasicHttpBinding();
            wsBinding.MaxReceivedMessageSize = int.MaxValue;

            EndpointAddress endpointAddress = new EndpointAddress(Url);
            if (InnerClient != null)
            {
                if (InnerClient.State == CommunicationState.Opened)
                {
                    InnerClient.Close();
                }
            }
            InnerClient = new LocationServiceClient(wsBinding, endpointAddress);
        }
    }
}
