using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFServiceForWPF.LocationServices;

namespace WCFClientLib
{
    public class LocationClient:WCFClient
    {
        public LocationServiceClient InnerClient { get; set; }

        public LocationClient(string host, string port):base(host,port)
        {
            SetConnectInfo();
        }

        protected void SetConnectInfo()
        {
            BasicHttpBinding wsBinding = new BasicHttpBinding();
            wsBinding.MaxReceivedMessageSize = int.MaxValue;
            Url =
                string.Format("http://{0}:{1}/LocationServices/Locations/LocationService/",
                    Host, Port);

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
