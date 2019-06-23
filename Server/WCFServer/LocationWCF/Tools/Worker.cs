using Location.BLL.Tool;
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
        public static void Run(Action task,Action completed,string logTag="")
        {
            DateTime start=DateTime.Now;
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
                TimeSpan t = DateTime.Now - start;
                if (logTag != "")
                {
                    Log.Info(logTag, string.Format("完成，用时:{0}", t));
                }
               
                if (completed != null)
                {
                    completed();
                }
            };
            workder.RunWorkerAsync();
        }
    }
}
