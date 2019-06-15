using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Tools
{
    public static class Worker
    {
        public static void Run(Action task,Action completed)
        {
            BackgroundWorker workder = new BackgroundWorker();
            workder.DoWork += (sender,e)=>
            {
                if (task != null)
                {
                    task();
                }
            };
            workder.RunWorkerCompleted += (sender, e) =>
            {
                if (completed != null)
                {
                    completed();
                }
            };
            workder.RunWorkerAsync();
        }
    }
}
