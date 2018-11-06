using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Coldairarrow.Util.Sockets;

namespace PositionSimulation
{
    public class SimulationServer
    {
        private LightUDP udp;
        public void Start(IPAddress localIP, int localPort)
        {
            udp = new LightUDP(localIP, localPort);
            udp.DGramRecieved += Udp_DGramRecieved;
        }

        public List<IPEndPoint>  Clients=new List<IPEndPoint>();

        private void Udp_DGramRecieved(object sender, BUDPGram dgram)
        {
            if(!Clients.Contains(dgram.iep))
                Clients.Add(dgram.iep);
        }

        public void Stop()
        {
            udp.Close();
        }

        public void Send(string msg)
        {
            foreach (var client in Clients)
            {
                udp.Send(Encoding.UTF8.GetBytes(msg), client);
            }
        }
    }
}
