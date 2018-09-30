using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Web.Sockets.Core.Others;

namespace Web.Sockets.Core
{
    public static class SocketHelper
    {
        public static Socket GetSocket(ConnectType type)
        {
            if (type == ConnectType.TCP)
            {
                return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            else if (type == ConnectType.UDP)
            {
                return new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            return null;
        }

        public static T GetSocketEntity<T>(string ip, int port, ConnectType type) where T : SocketEntity ,new()
        {
            IPEndPoint ipEndPoint1 = new IPEndPoint(IPAddress.Parse(ip), port);
            return GetSocketEntity<T>(ipEndPoint1, type);
        }
        public static T GetSocketEntity<T>(IPEndPoint ipEndPoint, ConnectType type) where T : SocketEntity, new()
        {
            Socket socket = SocketHelper.GetSocket(type);
            T entity = new T();
            entity.Socket = socket;
            entity.IpEndPoint = ipEndPoint;
            return entity;
        }

        //public static string GetString(byte[] bytes)
        //{
        //    string msg = Encoding.Default.GetString(bytes);
        //    int id = msg.IndexOf('\0');
        //    if (id != -1)
        //    {
        //        msg = msg.Substring(0, id);
        //    }
        //    return msg;
        //}

        public static string GetString(byte[] data)
        {
            string text = "";
            for (int i = 0; i < data.Length; i++)
            {
                Char ss = Convert.ToChar(data[i]);
                text = text + Convert.ToString(ss);
            }

            //int id = text.IndexOf('\0');
            //if (id != -1)
            //{
            //    text = text.Substring(0, id);
            //}
            return text;
        }

        public static string GetStringEx(byte[] data)
        {
            string text = GetString(data);
            int id = text.IndexOf('\0');
            if (id != -1)
            {
                text = text.Substring(0, id);
            }
            return text;
        }

        public static string GetStringEx(byte[] data,Encoding encoding)
        {
            string text = encoding.GetString(data);
            int id = text.IndexOf('\0');
            if (id != -1)
            {
                text = text.Substring(0, id);
            }
            return text;
        }

        public static byte[] GetBytesEx(string msg, Encoding encoding)
        {
            if (msg.Length == 0)
            {
                msg = Convert.ToString("\r\n");
            }

            if (msg.EndsWith("\n") && !msg.EndsWith("\r\n"))
            {
                msg = msg.Substring(0, msg.Length - 1) + "\r\n";
            }

            //return null;

            return GetBytes(msg, encoding);
        }

        public static byte[] GetBytes(string msg,Encoding encoding)
        {
            Byte[] bytes = null;
            if (encoding == null)
            {
                int len = msg.Length;
                bytes = new Byte[len];
                for (int i = 0; i < len; i++)
                {
                    try
                    {
                        bytes[i] = Convert.ToByte(msg[i]);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                bytes = encoding.GetBytes(msg);
            }
            return bytes;
        }

        /// <summary>
        /// 获取数组的一部分。
        /// </summary>
        /// <param name="bytMsg"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static T[] GetSubArray<T>(T[] bytMsg, int start, int len)
        {
            T[] data = new T[len];
            for (int i = 0; i < len; i++)
            {
                data[i] = bytMsg[start+i];
            }
            return data;
        }

        public static IPEndPoint GetIPEndPort(string ip,int port)
        {
            IPEndPoint ipEndPoint = null;
            try
            {
               ipEndPoint= new IPEndPoint(IPAddress.Parse(ip), port);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error at SocketHelper.GetIPEndPort:" + ex.Message);
                Log.error(ex);
            }
            return ipEndPoint;
        }
    }
}
