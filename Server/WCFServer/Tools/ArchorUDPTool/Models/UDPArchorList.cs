using ArchorUDPTool.ArchorManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArchorUDPTool.Models
{
    [XmlRoot("UDPArchorList")]
    public class UDPArchorList : List<UDPArchor>
    {
        //ArchorStatistics statistics = new ArchorStatistics();

        //Dictionary<string, UDPArchor> index = new Dictionary<string, UDPArchor>();
        //internal int AddOrUpdate(UDPArchor archor)
        //{
        //    if (archor == null) return -1;
        //    lock (index)
        //    {
        //        if (index.ContainsKey(archor.Client))
        //        {
        //            if (DataUpdated != null)
        //            {
        //                DataUpdated(archor);
        //            }
        //            return 0;
        //        }
        //        else
        //        {
        //            index[archor.Client] = archor;
        //            this.Add(archor);
        //            statistics.Add(archor.Client);
        //            if (DataAdded != null)
        //            {
        //                DataAdded(archor);
        //            }
        //            return 1;
        //        }
        //    }

        //}

        //public string GetStatistics()
        //{
        //    return statistics.GetText();
        //}

        //public event Action<UDPArchor> DataUpdated;

        //public event Action<UDPArchor> DataAdded;
        internal object GetConnectedCount()
        {
            int count = 0;
            foreach(var i in this)
            {
                if (i.IsConnected)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
