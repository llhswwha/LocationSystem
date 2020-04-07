using DbModel.Tools;
using System;
using System.ComponentModel;

namespace LocationServer.Tools
{
    public static class Worker
    {
        public static BackgroundWorker Run(Action task,Action completed,Action<Exception> error=null,string logTag="",bool cancelable=false)
        {
            DateTime start=DateTime.Now;
            BackgroundWorker workder = new BackgroundWorker();
            if(cancelable)
                workder.WorkerSupportsCancellation = true;
            workder.DoWork += (sender,e)=>
            {
                try
                {
                    if (task != null)
                    {
                        task();
                    }
                }
                catch (Exception ex)
                {
                    if (error != null)
                    {
                        error(ex);
                    }

                }

            };
            workder.RunWorkerCompleted += (sender, e) =>
            {
                TimeSpan t = DateTime.Now - start;
                if (logTag != "")
                {
                    LogEvent.Info(logTag, string.Format("Worker完成，用时:{0}", t));
                }
               
                if (completed != null)
                {
                    completed();
                }
            };
            workder.RunWorkerAsync();
            return workder;
        }

        public static BackgroundWorker Run<T>(Func<T> task, Action<T> completed, Action<Exception> error = null, string logTag = "")
        {
            DateTime start = DateTime.Now;
            BackgroundWorker workder = new BackgroundWorker();
            T r = default(T);
            workder.DoWork += (sender, e) =>
            {
                try
                {
                    if (task != null)
                    {
                        r=task();
                    }
                }
                catch (Exception ex)
                {
                    if (error != null)
                    {
                        error(ex);
                    }
                }

            };
            workder.RunWorkerCompleted += (sender, e) =>
            {
                TimeSpan t = DateTime.Now - start;
                if (logTag != "")
                {
                    LogEvent.Info(logTag, string.Format("Worker完成，用时:{0}", t));
                }

                if (completed != null)
                {
                    completed(r);
                }
            };
            workder.RunWorkerAsync();
            return workder;
        }
    }
}
