using System.Net;

namespace Web.Sockets.Core.Tansfers
{
    public class TCPSocketTransfer : SocketTransfer
    {
        /// <summary>
        /// 开始监听，等待消息
        /// </summary>
        public override void Start()
        {
            base.Start();
            (Server as TCPServerSocket).ClientConnected += TCPSocketTransfer_ClientConnected;
        }

        void TCPSocketTransfer_ClientConnected(System.Net.Sockets.Socket obj)
        {
            StartClient();
        }

        public TCPSocketTransfer(string ip1, int port1, string ip2, int port2)
        {
            Server = SocketHelper.GetSocketEntity<TCPServerSocket>(ip1, port1, ConnectType.TCP);
            Client = SocketHelper.GetSocketEntity<TCPClientSocket>(ip2, port2, ConnectType.TCP);
        }

        protected override void ReCreateClient()
        {
            Client = SocketHelper.GetSocketEntity<TCPClientSocket>(Client.IpEndPoint, ConnectType.TCP);
        }


        public string Text
        {
            get
            {
                if (Server == null || Client == null) return "";
                return Server.IpEndPoint + " => " + Client.IpEndPoint;
            }
        }
    }
}
