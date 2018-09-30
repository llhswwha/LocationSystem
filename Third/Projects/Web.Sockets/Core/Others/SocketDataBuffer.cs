using System;
using System.Collections.Generic;
using System.Threading;

namespace Web.Sockets.Core.Others
{
    class SocketDataBuffer
    {
        private TCPSocketEntity socketEntity;
        private object client;
        private int bufferSize;
        private Thread thread;

        private byte[] buffer;
        List<byte> tempBuffer;

        public SocketDataBuffer(TCPSocketEntity socketEntity, object client)
        {
            socketEntity.MessageReceived += socketEntity_MessageReceived;

            this.socketEntity = socketEntity;
            this.client = client;
        }

        public bool Stoped
        {
            get { return _stoped; }
            set { _stoped = value; }
        }

        public void Start()
        {
            thread = new Thread(WaitingData);
            thread.Start();
            _stoped = false;
        }

        /// <summary>
        /// 上次收到数据的时间
        /// </summary>
        private DateTime lastTime;

        void socketEntity_MessageReceived(byte[] arg1, object arg2)
        {
            lastTime = DateTime.Now;
            AddData(arg1);
        }

        public void AddData(byte[] data)
        {
            while (this.buffer!=null)
            {
                Thread.Sleep(100);
            }
            this.buffer = data;
            Thread.Sleep(10);
        }

        private int waitCout = 0;
        private bool _stoped = true;

        private void WaitingData()
        {
            while (buffer == null)
            {
                Thread.Sleep(100);
            }

            lock (buffer)
            {
                while (true)
                {
                    if (buffer != null)
                    {
                        if (tempBuffer == null)
                        {
                            tempBuffer = new List<byte>();
                        }
                        tempBuffer.AddRange(buffer);
                        buffer = null;
                        Thread.Sleep(10);
                    }
                    else
                    {
                        waitCout++;
                        if (waitCout > 50)
                        {
                            socketEntity.OnMessageReceived(tempBuffer.ToArray(), client);
                            tempBuffer = null;
                            waitCout = 0;
                            break;
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            while (tempBuffer != null)
            {
                Thread.Sleep(100);
            }
            thread.Abort();
            _stoped = true;
        }

        //int tempLen = 0;
        ///// <summary>
        ///// 分次读取然后转存（尝试）。
        ///// 难题：如果实际获取的数据正好和缓存区大小相同的话会停在这里。怎么知道还有没有数据呢？
        ///// </summary>
        ///// <param name="bytMsg"></param>
        ///// <param name="obj"></param>
        //private int ReceiveToTempBuffer(byte[] bytMsg, int len, object obj)
        //{
        //    if (len >= BufferSize)
        //    {
        //        
        //        
        //    }
        //    else
        //    {
        //        byte[] data = ParseData(bytMsg, len);
        //        if (tempBuffer != null)
        //        {
        //            tempBuffer.AddRange(data);
        //            len = tempBuffer.Count;
        //            OnMessageReceived(tempBuffer.ToArray(), obj);
        //            tempBuffer = null;
        //        }
        //        else
        //        {
        //            OnMessageReceived(data, obj);
        //        }
        //    }
        //    return len;
        //}
    }
}
