using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Web.Sockets.Core.Others;

namespace Web.Sockets.Core
{
    public abstract class TCPSocketEntity:SocketEntity
    {
        public TCPSocketEntity()
        {
            //InitSocket();
        }

        public TCPSocketEntity(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
            //InitSocket();
        }

        public override void Stop()
        {
            Socket.Close();
            //Socket = null;
            StopThreads();
            OnReceiveStopted();
        }

        public override void Listen(IPEndPoint ipEndPoint)
        {
            InitSocket();
            Socket.Bind(ipEndPoint);
            Socket.Listen(10);
        }

        protected void InitSocket()
        {
            //if (Socket == null)
            //{
                Socket = SocketHelper.GetSocket(ConnectType.TCP);
            //}
        }

        public override int SendMsg(string strMsg, object arg)
        {
            byte[] bytes = SocketHelper.GetBytesEx(strMsg,Encoding);
            return SendMsg(bytes, arg);
        }

        public override int SendMsg(byte[] bytMsg, object arg)
        {
            Socket sender = arg as Socket;
            if (sender != null)
            {
                if (sender.RemoteEndPoint == null)
                {
                    return -1;
                }

                if (!sender.Connected)
                {
                    sender.Connect(sender.RemoteEndPoint);
                }

                if (sender.Connected)
                {
                    return sender.Send(bytMsg);
                }

            }
            return -1;
        }

        public int SendMsg(string strMsg, object arg, AsyncCallback callback)
        {
            byte[] bytes = SocketHelper.GetBytesEx(strMsg, Encoding);
            return SendMsg(bytes, arg, callback);
        }

        public int SendMsg(byte[] bytMsg, object arg, AsyncCallback callback)
        {
            Socket sender = arg as Socket;
            if (sender == null)
            {
                sender = Socket;
            }

            if (sender != null)
            {
                if (sender.Connected)
                {
                    IAsyncResult ar2 = sender.BeginSend(bytMsg, 0, bytMsg.Length, SocketFlags.None, callback, sender);
                    sender.EndSend(ar2);
                }
            }

            return -1;
        }

        //public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags,
        //                              AsyncCallback callback, object state)
        //{
        //    IAsyncResult ar2 = s.BeginSend(smk, 0, smk.Length, SocketFlags.None, callbackProc, s);
        //    s.EndSend(ar2);
        //}

        public override void ReceiveMsg()
        {
            ReceiveMsg(Socket);
        }

        public override void ReceiveMsg(object obj)
        {
            while (true)
            {
                byte[] buffer = new byte[BufferSize];
                int len = ReceiveMsg(buffer, obj);
                if (len == -1)
                {
                    OnReceiveBreaked(obj as Socket);
                    break;
                }
                else
                {
                    
                }
            }
        }

        public bool IsAsync = false;

        public override int ReceiveMsg(byte[] bytMsg, object obj)
        {
            Socket client = obj as Socket;
            if (client == null)
            {
                return -1;
            }
            try
            {
                if (IsAsync)
                {
                    SocketAsyncEventArgs asyncEvent = new SocketAsyncEventArgs();
                    asyncEvent.Completed += AsyncEvent_Completed;
                    client.ReceiveAsync(asyncEvent);
                    return 0;
                }
                else
                {
                    int len = client.Receive(bytMsg);//获取数据
                                                     //方式1：收到就发送
                    if (len > 0)
                    {
                        byte[] data = ParseData(bytMsg, len);
                        OnMessageReceived(data, obj);
                    }

                    //方式2：分次读取然后转存
                    //len = ReceiveToTempBuffer(bytMsg, len, obj);

                    //SocketDataBuffer.owner = this;
                    //len = SocketDataBuffer.ReceiveToTempBuffer(bytMsg, len, obj);
                    return len;
                }
                
            }
            catch (Exception ex)//远程主机强制关闭了一个现有连接
            {
                Log.error(ex);
                return -1;
            }
        }

        private void AsyncEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #region 分次读取然后转存

        //private static class SocketDataBuffer
        //{
        //    public static TCPSocketEntity owner;
        //    public static int BufferSize
        //    {
        //        get { return owner.BufferSize; }
        //    }
        //    private static void OnMessageReceived(byte[] bytMsg, object arg = null)
        //    {
        //        owner.OnMessageReceived(bytMsg, arg);
        //    }
        //    private static byte[] ParseData(byte[] data, int len)
        //    {
        //        return owner.ParseData(data, len);
        //    }

        //    static List<byte> tempBuffer;
        //    static int tempLen = 0;
        //    /// <summary>
        //    /// 分次读取然后转存（尝试）。
        //    /// 难题：如果实际获取的数据正好和缓存区大小相同的话会停在这里。怎么知道还有没有数据呢？
        //    /// </summary>
        //    /// <param name="bytMsg"></param>
        //    /// <param name="obj"></param>
        //    public static int ReceiveToTempBuffer(byte[] bytMsg, int len, object obj)
        //    {
        //        waitTime = DateTime.Now;
        //        client = obj;
        //        if (len >= BufferSize)
        //        {
        //            if (tempBuffer == null)
        //            {
        //                tempBuffer = new List<byte>();
        //            }
        //            tempBuffer.AddRange(bytMsg);
        //            StartWatting();//开始等待下一次信息
        //        }
        //        else
        //        {
        //            byte[] data = ParseData(bytMsg, len);
        //            if (tempBuffer != null)
        //            {
        //                tempBuffer.AddRange(data);
        //                len = tempBuffer.Count;
        //                StopWaitting();//
        //                OnMessageReceived(tempBuffer.ToArray(), obj);
        //                tempBuffer = null;
        //            }
        //            else
        //            {
        //                OnMessageReceived(data, obj);
        //            }
        //        }
        //        return len;
        //    }

        //    static private object client;
        //    static private Thread thread;
        //    static private DateTime waitTime;
        //    static private int waitCount;
        //    private static void StartWatting()
        //    {
        //        waitCount = 0;
        //        thread = new Thread(Waitting);
        //        thread.Start();
        //    }

        //    private static void StopWaitting()
        //    {
        //        thread.Abort();
        //    }

        //    private static void Waitting()
        //    {
        //        while (true)
        //        {
        //            if (tempBuffer == null)
        //            {
        //                break;
        //            }

        //            //超过一定时间（或次数）没有再获得信息（线程阻塞住），就把上次保存的发送出去
        //            //if ((DateTime.Now - waitTime).TotalMilliseconds > 1000)
        //            if (waitCount > 10)
        //            {
        //                OnMessageReceived(tempBuffer.ToArray(), client);
        //                tempBuffer = null;
        //                waitCount = 0;
        //                break;
        //            }
        //            else
        //            {
        //                waitCount++;
        //                Thread.Sleep(100);
        //            }
        //            //if (tempBuffer == null)
        //            //{

        //            //}
        //        }
        //        tempBuffer = null;

        //    }
        //}

        List<byte> tempBuffer;
        int tempLen = 0;
        /// <summary>
        /// 分次读取然后转存（尝试）。
        /// 难题：如果实际获取的数据正好和缓存区大小相同的话会停在这里。怎么知道还有没有数据呢？
        /// </summary>
        /// <param name="bytMsg"></param>
        /// <param name="obj"></param>
        private int ReceiveToTempBuffer(byte[] bytMsg, int len, object obj)
        {
            waitTime = DateTime.Now;
            client = obj;
            if (len >= BufferSize)
            {
                if (tempBuffer == null)
                {
                    tempBuffer = new List<byte>();
                }
                tempBuffer.AddRange(bytMsg);
                StartWatting();//开始等待下一次信息
            }
            else
            {
                byte[] data = ParseData(bytMsg, len);
                if (tempBuffer != null)
                {
                    tempBuffer.AddRange(data);
                    len = tempBuffer.Count;
                    StopWaitting();
                    OnMessageReceived(tempBuffer.ToArray(), obj);
                    tempBuffer = null;
                }
                else
                {
                    OnMessageReceived(data, obj);
                }
            }
            return len;
        }

        private object client;
        private Thread thread;
        private DateTime waitTime;
        private int waitCount;
        private void StartWatting()
        {
            waitCount = 0;
            thread = new Thread(Waitting);
            thread.Start();
        }

        private void StopWaitting()
        {
            thread.Abort();
        }

        private void Waitting()
        {
            while (true)
            {
                if (tempBuffer == null)
                {
                    break;
                }

                //超过一定时间（或次数）没有再获得信息（线程阻塞住），就把上次保存的发送出去
                if ((DateTime.Now - waitTime).TotalMilliseconds > 1000)
                //if (waitCount > 50)
                {
                    OnMessageReceived(tempBuffer.ToArray(), client);
                    tempBuffer = null;
                    waitCount = 0;
                    break;
                }
                else
                {
                    waitCount++;
                    Thread.Sleep(10);
                }
                //if (tempBuffer == null)
                //{

                //}
            }
            tempBuffer = null;

        }

        #endregion
    }
}
