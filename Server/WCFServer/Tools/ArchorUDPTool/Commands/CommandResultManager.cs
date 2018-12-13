using ArchorUDPTool.ArchorManagers;
using ArchorUDPTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.Commands
{
    public class CommandResultManager
    {
        public List<CommandResultGroup> Groups { get; set; }
        ArchorStatistics statistics = new ArchorStatistics();

        public CommandResultManager()
        {
            Groups = new List<CommandResultGroup>();
        }

        public UDPArchor GetArchorByIp(string ip)
        {
            var group = Groups.Find(i => i.Id.StartsWith(ip));
            if (group == null) return null;
            return group.Archor;
        }

        public CommandResultGroup GetByIp(string ip)
        {
            var group = Groups.Find(i => i.Id.StartsWith(ip));
            return group;
        }

        public CommandResultGroup GetById(string id)
        {
            var g = Groups.Find(i => i!=null&&i.Id == id);
            if (g == null)
            {
                g = new CommandResultGroup(id);
                Groups.Add(g);
                statistics.Add(id);
            }
            else
            {
                if (string.IsNullOrEmpty(g.Archor.IsConnected))//从清单加载进来的
                {
                    statistics.Add(id);
                }
                g.IsNew = false;
            }
            return g;
        }

        public string GetStatistics()
        {
            return statistics.GetText();
        }
        public CommandResultGroup Add(string id)
        {
            var group = GetById(id);
            return group;
        }

        public CommandResultGroup Add(System.Net.IPEndPoint iep, byte[] data)
        {
            string id = iep.ToString();
            var group = GetById(id);
            group.AddData(data);
            return group;
        }

        internal CommandResultGroup Add(ArchorDev item)
        {
            CommandResultGroup group = new CommandResultGroup(item.ArchorIp+":4646");
            Groups.Add(group);

            return group;
        }

        internal CommandResultGroup Add(UDPArchor item)
        {
            var group = new CommandResultGroup(item);
            Groups.Add(group);
            if(!string.IsNullOrEmpty(item.IsConnected))
                statistics.Add(group.Id);
            return group;
        }

        internal void ClearPing(string[] ips)
        {
            var list = ips.ToList();
            foreach (var item in Groups)
            {
                if (list.Contains(item.Archor.GetClientIP()))
                {
                    item.Archor.Ping = "";
                }
            }
        }
    }
}
