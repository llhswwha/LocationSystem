using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Location.WCFServiceReferences
{
    public abstract class WCFClient
    {
        public Exception Exception { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }

        public string Url { get; set; }

        public EndpointAddress EndpointAddress { get; set; }
        public Binding Binding { get; set; }

        public WCFClient()
        {
            
        }

        public WCFClient(string host, string port)
        {
            Host = host;
            Port = port;
        }
    }
}
