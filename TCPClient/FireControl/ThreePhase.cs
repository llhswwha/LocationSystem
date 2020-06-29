using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPClient.FireControl
{
  public   class ThreePhase
    {
        private TcpListener tcp;
        private TcpClient client;
        private List<string> strList;

        public ThreePhase()
        {
            tcp = new TcpListener(IPAddress.Any, 8733);
            tcp.Start();
            Thread t = new Thread(server);
            t.IsBackground = true;
            t.Start(tcp);
            strList = new List<string>();
            SaveFireControl();
        }
        private void server(object o)
        {
            TcpListener list = o as TcpListener;
            client = list.AcceptTcpClient();
            while (true)
            {
                try
                {
                    NetworkStream strem = client.GetStream();
                    //if (strem.DataAvailable)
                    //{
                    const int buffer = 256;
                    byte[] b = new byte[buffer];
                    int r = strem.Read(b, 0, buffer);
                    string str = Encoding.UTF8.GetString(b, 0, r);
                    string bb = client.Client.RemoteEndPoint.ToString() + ":" + str + "\r\n";
                    strList.Add(str);
                    strem.Flush();
                }
                catch (Exception ex)
                {
                    Log.Error("ThreePhase.server:"+ex.ToString());
                }
            }
        }

        public bool SaveFireControl()
        {
           
            try
            {
                if (strList.Count > 0)
                {
                    foreach (string s in strList) //得到的字符要分拆
                    {
                       // string s = "70DF611FFF0851497535FF09";
                        List<byte> bytes = new List<byte>();
                        //s是多个数据组成，需要截取，分别解析

                        for (int i = 0; i < s.Length; i += 2)
                        {
                            bytes.Add(byte.Parse(s.Substring(i, 2), NumberStyles.HexNumber));
                        }
                        var str = Encoding.BigEndianUnicode.GetString(bytes.ToArray());
                        string ss = str.ToString();  //其中一个字符

                       
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("ThreePhase.SaveFireControl:" + ex.ToString());
                return true;
            }
        }


    }
}
