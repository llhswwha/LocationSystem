using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRClientLib
{
    public class HubClient
    {
        public HubConnection Connection { get; set; }
        public IHubProxy HubProxy { get; set; }

        public string ServerURI { get; set; }

        public HubClient(string uri,string name)
        {
            ServerURI = uri;
            Connection = new HubConnection(ServerURI);
            //Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy(name);
        }

        public Exception Exception { get; set; }

        public async Task<bool> Start()
        {
            try
            {
                await Connection.Start();
                return true;
            }
            catch (Exception ex)
            {
                Exception = ex;
                return false;
            }
        }

        public void Stop()
        {
            if (Connection != null)
            {
                Connection.Stop();
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}
