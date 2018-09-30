using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Web.Sockets.Core.Interfaces;
using Web.Sockets.Core.Others;

namespace Web.Sockets.Core
{
    public abstract class SocketEntity:IReceiver
    {
        private Encoding _encoding = System.Text.Encoding.UTF8;
        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        private int _bufferSize = 81920;
        public int BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }

        private Socket _socket;
        public Socket Socket
        {
            get
            {
                return _socket;
            }
            set
            {
                _socket = value;
            }
        }

        private IPEndPoint _ipEndPoint;
        public IPEndPoint IpEndPoint
        {
            get { return _ipEndPoint; }
            set { _ipEndPoint = value; }
        }

        public SocketEntity()
        {
            
        }

        public SocketEntity(Socket socket, IPEndPoint ipEndPoint)
        {
            _socket = socket;
            _ipEndPoint = ipEndPoint;
        }

        /// <summary>
        /// 子类是UDP或TCP会在自己的构造函数中初始化相应的Socket
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public SocketEntity(IPEndPoint ipEndPoint)
        {
            _ipEndPoint = ipEndPoint;
        }

        #region 各阶段的事件
        public event Action<byte[],object> MessageReceived;
        protected internal void OnMessageReceived(byte[] bytMsg,object arg=null)
        {
            if (MessageReceived != null)
            {
                MessageReceived(bytMsg, arg);
            }
        }

        public event Action<Socket> ReceiveBreaked;
        protected void OnReceiveBreaked(Socket client)
        {
            if (ReceiveBreaked != null)
            {
                ReceiveBreaked(client);
            }
        }

        public event Action ReceiveStarted;
        protected void OnReceiveStarted()
        {
            if (ReceiveStarted != null)
            {
                ReceiveStarted();
            }
        }

        public event Action<Socket> ClientConnected;
        protected void OnClientConnected(Socket client)
        {
            if (ClientConnected != null)
            {
                ClientConnected(client);
            }
        }

        public event Action ReceiveStopted;
        protected void OnReceiveStopted()
        {
            if (ReceiveStopted != null)
            {
                ReceiveStopted();
            }
        }
        #endregion

        #region IReceiver
        public bool Started { get; set; }
        public abstract bool Start();
        public abstract void Stop();

        public int SendMsg(string strMsg)
        {
            return SendMsg(strMsg, null);
        }
        public abstract int SendMsg(string strMsg, object arg);
        public abstract int SendMsg(byte[] bytMsg, object arg);
        public abstract void Listen(IPEndPoint ipEndPoint);
        public abstract void ReceiveMsg();
        public abstract void ReceiveMsg(object obj);
        public abstract int ReceiveMsg(byte[] bytMsg, object obj);
        #endregion

        #region Thread

        protected List<Thread> Threads = new List<Thread>();


        public void StartReceive(ThreadStart receiveFunc)
        {
            Thread thread = new Thread(receiveFunc);
            thread.Start();
            Threads.Add(thread);
        }

        public void StartReceive(ParameterizedThreadStart receiveFunc, Socket client)
        {
            Thread thread = new Thread(receiveFunc);
            thread.Start(client);
            Threads.Add(thread);
        }

        public void StopThreads()
        {
            foreach (Thread thread in Threads)
            {
                thread.Abort();
            }
        }

        #endregion


        public IDataProtocol Protocol { get; set; }
        /// <summary>
        /// 获取数据后处理
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        protected byte[] ParseData(byte[] data, int len)
        {
            try
            {
                //根据获取的数据的长度，重新创建一个byte[]保存实际数据
                data = SocketHelper.GetSubArray(data, 0, len);
                if (Protocol != null)
                {
                    data = Protocol.Parse(data, this);
                }
            }
            catch (Exception ex)
            {
                Log.error(ex);
            }
            return data;
        }
    }
}
