using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices
{
    public static class AlarmEventManager
    {
        public static Dictionary<string, ClientAlarms> Dicts = new Dictionary<string, ClientAlarms>();

        public static void 
    }

    public class ClientAlarms
    {
        public string Session { get; set; }

        public List<int> SendedAlarms { get; set; }

        public List<int> NewAlarms { get; set; }
    }
}
