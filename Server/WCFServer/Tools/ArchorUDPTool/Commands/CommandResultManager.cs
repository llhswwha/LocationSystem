using ArchorUDPTool.ArchorManagers;
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

        public CommandResultGroup Add(System.Net.IPEndPoint iep, byte[] data)
        {
            string id = iep.ToString();
            var group = GetById(id);
            group.AddData(data);
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
                g.IsNew = false;
            }
            return g;
        }

        public string GetStatistics()
        {
            return statistics.GetText();
        }
    }
}
