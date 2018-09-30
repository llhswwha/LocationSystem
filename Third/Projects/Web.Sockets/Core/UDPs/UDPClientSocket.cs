using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Web.Sockets.Core
{
    public class UDPClientSocket : UDPSocketEntity
    {
        public UDPClientSocket()
        {
            
        }

        public UDPClientSocket(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {

        }

        public override int SendMsg(byte[] bytMsg, object arg = null)
        {
            //Client不需要主动Start()，第一次发送消息后自动会绑定一个端口
            if (Started == false)
            {
                Start();
            }
            return base.SendMsg(bytMsg, arg);
        }
    }
}
