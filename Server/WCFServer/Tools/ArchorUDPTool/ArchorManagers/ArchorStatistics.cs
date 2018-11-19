using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.ArchorManagers
{
    public class ArchorStatistics
    {
        public Dictionary<string, int> Statistics = new Dictionary<string, int>();

        public void Add(string id)
        {
            int i = id.LastIndexOf('.');
            string s = id.Substring(0, i);
            s += ".*";

            if (!Statistics.ContainsKey(s))
            {
                lock (Statistics)
                {
                    Statistics[s] = 0;
                }
            }
            Statistics[s]++;
        }

        public string GetText()
        {
            string txt = "";
            foreach (var key in Statistics.Keys)
            {
                txt += string.Format("{0}:[{1}] | ", key, Statistics[key]);
            }
            return txt;
        }
    }
}
