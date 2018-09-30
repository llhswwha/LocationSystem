using System;
using System.Net;
using System.Text;
using Web.Sockets.Core.Others;

namespace Web.Sockets.Core
{
    public class UDPSocketEntity : SocketEntity
    {
        public UDPSocketEntity()
        {
            //InitSocket();
        }

        public UDPSocketEntity(IPEndPoint ipEndPoint)
            : base(ipEndPoint)
        {
            //InitSocket();
        }

        protected void InitSocket()
        {
            this.Socket = SocketHelper.GetSocket(ConnectType.UDP);
        }

        public override void Listen(IPEndPoint ipEndPoint)
        {
            InitSocket();
            Socket.Bind(ipEndPoint);
            StartReceive(ReceiveMsg);
        }

        public override bool Start()
        {
            StartReceive(ReceiveMsg);
            Started = true;
            return true;
        }

        public override void Stop()
        {
            Socket.Close();
            StopThreads();
            Started = false;
        }

        public override int SendMsg(string strMsg, object arg = null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(strMsg);
            return SendMsg(bytes, arg);
        }

        public override int SendMsg(byte[] bytMsg, object arg = null)
        {
            IPEndPoint ipEnd = arg as IPEndPoint;
            if (ipEnd != null)
            {
                return Socket.SendTo(bytMsg, ipEnd);
            }
            else
            {
                return Socket.SendTo(bytMsg, IpEndPoint);
            }
        }

        public override void ReceiveMsg()
        {
            ReceiveMsg(null);
        }

        public override void ReceiveMsg(object obj)
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[BufferSize];
                    int len = ReceiveMsg(buffer, null);
                }
                catch (Exception ex)
                {
                    Log.error(ex);
                }
            }
        }

        public override int ReceiveMsg(byte[] bytMsg, object obj)
        {
            try
            {
                //byte[] buffer = new byte[BufferSize];
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint senderRemote = (EndPoint)sender;

                int len = Socket.ReceiveFrom(bytMsg, ref senderRemote);
                byte[] data = ParseData(bytMsg, len);
                OnMessageReceived(data, senderRemote);
                return len;
            }
            catch (Exception ex)
            {
                Log.error(ex);
                OnReceiveBreaked(Socket);
                return -1;
            }
        }
    }
}
