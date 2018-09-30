using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Web.Sockets.Core.Interfaces
{
    /// <summary>
    /// 接收和发送消息
    /// </summary>
    public interface IReceiver
    {
        //bool IsConnected { get;}

        IPEndPoint IpEndPoint { get; set; } 

        bool Started { get; set; }

        /// <summary>
        /// 开始
        /// </summary>
        /// <returns></returns>
        bool Start();

        /// <summary>
        /// 结束
        /// </summary>
        void Stop();

        /// <summary>
        /// 编码方式
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        int SendMsg(string strMsg, object obj = null);
        int SendMsg(byte[] bytMsg, object obj = null);

        /// <summary>
        /// 循环获取消息
        /// </summary>
        void ReceiveMsg();
        /// <summary>
        /// 循环获取消息
        /// </summary>
        /// <param name="obj">获取消息的对象</param>
        void ReceiveMsg(object obj);

        /// <summary>
        /// 循环获取消息
        /// </summary>
        /// <param name="bytMsg">消息的内容</param>
        /// <param name="obj">获取消息的对象</param>
        /// <returns>消息的长度</returns>
        int ReceiveMsg(byte[] bytMsg, object obj);

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="ipEndPoint"></param>
        void Listen(IPEndPoint ipEndPoint);


        event Action<byte[], object> MessageReceived;
        event Action<Socket> ReceiveBreaked;
        event Action ReceiveStarted;
        event Action ReceiveStopted;
        event Action<Socket> ClientConnected;

        IDataProtocol Protocol { get; set; }
    }
}
