using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DbModel.Tools
{
    public static class ThreadTool
    {
        public static Thread Start(ThreadStart action)
        {
            Thread thread=new Thread(action);
            thread.Start();
            return thread;
        }
    }
}
