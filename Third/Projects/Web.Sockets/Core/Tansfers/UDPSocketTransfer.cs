using System;

namespace Web.Sockets.Core.Tansfers
{
    public class UDPSocketTransfer : SocketTransfer
    {
        public UDPSocketTransfer(string ip1, int port1, string ip2, int port2)
        {
            Server = SocketHelper.GetSocketEntity<UDPServerSocket>(ip1, port1, ConnectType.UDP);
            Client = SocketHelper.GetSocketEntity<UDPClientSocket>(ip2, port2, ConnectType.UDP);
        }
    }
}
