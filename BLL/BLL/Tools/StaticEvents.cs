using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tools
{
    public class StaticEvents
    {
        public static event Action<DataChangArg> DbDataChanged;

        public static void OnDbDataChanged(DataChangArg arg)
        {
            if (DbDataChanged != null)
            {
                DbDataChanged(arg);
            }
        }
    }

    public class DataChangArg
    {
        public object Sender { get; set; }

        public object Data { get; set; }

        public DataChangArg()
        {

        }

        public DataChangArg(object sender,object data)
        {
            Sender = sender;
            Data = data;
        }
    }
}
