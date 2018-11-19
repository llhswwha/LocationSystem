using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.Models
{
    public class ServerInfo
    {
        public string Id { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public ServerInfo(string ip,int port)
        {
            Ip = ip;
            Port = port;
            Id = ip + port;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Ip, Port);
        }
    }

    public class ServerInfoList:List<ServerInfo>
    {
        public void Add(string ip,int port)
        {
            if (string.IsNullOrEmpty(ip)) return;
            if (port==0) return;
            string id = ip + port;
            var item = this.Find(i => i.Id == id);
            if (item == null)
            {
                item = new ServerInfo(ip, port);
                this.Add(item);
            }
        }

        public string GetText()
        {
            string txt = "";
            foreach (var item in this)
            {
                txt += item.ToString() + ";";
            }
            return txt;
        }
    }
}
