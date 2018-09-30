using System;
using System.Net;
using System.Net.Sockets;
using Web.Sockets.Core.Others;

namespace Web.Sockets.Core
{
    public class TCPClientSocket : TCPSocketEntity
    {
        public bool IsLesten { get; set; }

        public TCPClientSocket()
        {
            
        }

        public TCPClientSocket(IPEndPoint ipEndPoint):base(ipEndPoint)
        {
            
        }

        public override bool Start()
        {
            try
            {
                if (Socket == null)
                {
                    InitSocket();
                }
                Socket.Connect(IpEndPoint);
                StartReceive(ReceiveMsg);
                Started = true;
                OnReceiveStarted();
                return true;
            }
            catch (Exception ex)
            {
                Log.error(ex);
                return false;
            }
        }

        public override int SendMsg(byte[] bytMsg, object arg = null)
        {
            int len = base.SendMsg(bytMsg, Socket);
            //if (!IsLesten)
            //{
            //    StartReceive(ReceiveMsg);
            //    IsLesten = true;
            //}

            if (len == -1)//发送失败，和远端失去连接
            {
                //Stop();

                //InitSocket();
                //Start();
                //bool c = Socket.Connected;
                //len = base.SendMsg(bytMsg, Socket);
            }

            return len;
        }

    }
}
