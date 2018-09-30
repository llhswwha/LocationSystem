using System;
using System.Net;

namespace Web.Sockets.Core
{
    public class UDPServerSocket : UDPSocketEntity
    {
        public UDPServerSocket()
        {
            this.MessageReceived += UDPServerSocket_MessageReceived;
        }

        public UDPServerSocket(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
            this.MessageReceived += UDPServerSocket_MessageReceived;
        }



        void UDPServerSocket_MessageReceived(byte[] arg1, object arg2)
        {
            EndPoint ip = arg2 as EndPoint;

            //if (ip != null && !clientIps.Contains(ip))
            //{
            //    clientIps.Add(ip);
            //}

            LastIp = ip;

            //根据客服端的消息（命令），执行并返回结果。
            //Command cmd = new Command(arg1);
            //CommandResult result = CommandManager.Execute(arg1);
            //SendMsg(result.Message);

            //OnMessageReceived(arg1, arg2);
        }

        //protected List<EndPoint> clientIps = new List<EndPoint>();
        public EndPoint LastIp { get; set; }
        public override int SendMsg(byte[] bytMsg, object arg = null)
        {
            //foreach (EndPoint ipEnd in clientIps)
            //{
            //    Socket.SendTo(bytMsg, ipEnd);
            //}

            if (LastIp != null)
            {
                int max = 1024*60;
                while (bytMsg.Length > max)
                {
                    byte[] tmp1=new byte[max];
                    byte[] tmp2 = new byte[bytMsg.Length-max];
                    Array.Copy(bytMsg, tmp1, max);
                    Array.Copy(bytMsg, max, tmp2, 0, bytMsg.Length - max);
                    bytMsg = tmp2;
                    Socket.SendTo(tmp1, LastIp);
                }
                Socket.SendTo(bytMsg, LastIp);
                //currentClientIp = null;
            }

            return -2;
        }

        public override bool Start()
        {
            InitSocket();
            Socket.Bind(IpEndPoint);
            return base.Start();
        }
    }
}
