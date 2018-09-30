using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Web.Sockets.Core.Others;

namespace Web.Sockets.Core
{
    public class TCPServerSocket : TCPSocketEntity
    {
        public TCPServerSocket()
        {
           
        }

        public TCPServerSocket( IPEndPoint ipEndPoint)
            : base(ipEndPoint)
        {
            
        }

        //public event Action<Socket> ClientConnected;
        //protected void OnClientConnected(Socket client)
        //{
        //    if (ClientConnected != null)
        //    {
        //        ClientConnected(client);
        //    }
        //}

        private readonly List<Socket> Clients=new List<Socket>();
        protected void Receive()
        {
            while (true)
            {
                try
                {
                    Socket client = Socket.Accept();
                    Clients.Add(client);
                    OnClientConnected(client);
                    StartReceive(ReceiveMsg, client);
                }
                catch (Exception ex)
                {
                    Log.error(ex);
                }
            }
        }

        public override void Listen(IPEndPoint ipEndPoint)
        {
            base.Listen(ipEndPoint);
            StartReceive(Receive);
        }

        public override bool Start()
        {
            try
            {
                //Socket.Bind(IpEndPoint);
                //Socket.Listen(10);
                //StartReceive(Receive);
                Listen(IpEndPoint);
                OnReceiveStarted();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override void Stop()
        {
            Socket.Close();
            foreach (Socket client in Clients)
            {
                client.Close();
            }
            StopThreads();
            OnReceiveStopted();
        }

        public override int SendMsg(byte[] bytMsg, object arg = null)
        {
            foreach (Socket client in Clients)
            {
                base.SendMsg(bytMsg, client);
            }
            return 0;
        }
    }
}
