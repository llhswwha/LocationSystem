using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WCFClientLib
{


    public abstract class WCFClient
    {
        public Exception Exception { get; set; }

        public string Protocol { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }

        public string Url { get; set; }

        public WCFClientHostType HostType { get; set; }

        public EndpointAddress EndpointAddress { get; set; }
        public Binding Binding { get; set; }

        public WCFClient()
        {
            
        }

        public WCFClient(string host, string port, WCFClientHostType hostType)
        {
            Host = host;
            Port = port;
            HostType = hostType;
        }

        public WCFClient(string url)
        {
            Url = url;
        }
    }

    public enum WCFClientHostType
    {
        Self,IIS
    }
}
