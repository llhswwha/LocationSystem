using System;
using System.Net;
using Web.Sockets.Core.Interfaces;

namespace Web.Sockets.Core.Tansfers
{
    /// <summary>
    /// 转发Socket数据包
    /// </summary>
    public class SocketTransfer
    {
        public event Action<byte[], object> ClientMessageReceived;
        protected void OnClientMessageReceived(byte[] data,object arg)
        {
            if (ClientMessageReceived != null)
            {
                ClientMessageReceived(data,arg);
            }
        }
        public event Action<byte[], object> ServerMessageReceived;
        protected void OnServerMessageReceived(byte[] data, object arg)
        {
            if (ServerMessageReceived != null)
            {
                ServerMessageReceived(data, arg);
            }
        }

        /// <summary>
        /// 接收远端输入，传给Server，转发给本端
        /// </summary>
        public virtual IReceiver Client { get; set; }

        /// <summary>
        /// 接收本端输入，传给Client，转发给远端
        /// </summary>
        public virtual IReceiver Server { get; set; }

        /// <summary>
        /// 开始监听，等待消息
        /// </summary>
        public virtual void Start()
        {
            Server.Start();
            Server.MessageReceived += Server_MessageReceived;
            Server.ReceiveBreaked += Server_ReceiveBreaked;
            //Client.Start();
        }

        void Server_ReceiveBreaked(System.Net.Sockets.Socket obj)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public virtual void Stop()
        {
            Client.Stop();
            Server.Stop();
        }

        void Client_MessageReceived(byte[] data, object arg)
        {
            OnClientMessageReceived(data, arg);
            Server.SendMsg(data);
        }

        void Server_MessageReceived(byte[] data, object arg)
        {
            OnServerMessageReceived(data, arg);
            StartClient();
            if (Client.SendMsg(data) == -1)//发送失败
            {
                ReCreateClient();
                int r=Client.SendMsg(data);
            }
        }

        protected virtual void ReCreateClient()
        {
            
        }

        protected void StartClient()
        {
            if (Client.Started == false)
            {
                Client.Start();
                Client.MessageReceived += Client_MessageReceived;
            }
        }


        public void SetRemoteIp(string ip)
        {
            Client.IpEndPoint.Address = IPAddress.Parse(ip);
        }
    }
}
