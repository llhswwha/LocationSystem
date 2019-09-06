using Location.BLL.Tool;
using System.Threading;

namespace Base.Common.Threads
{
    public abstract class FunctionThread
    {
        public string Name { get; set; }

        public FunctionThread(string name)
        {
            this.Name = name;
        }

        public FunctionThread()
        {
            this.Name = this.GetType().Name;
        }

        protected Thread _thread;

        public void Start()
        {
            if (_thread == null)
            {
                _thread = new Thread(DoFunction);
                if(!string.IsNullOrEmpty(Name))
                    _thread.Name = Name;
                _thread.IsBackground = true;
                _thread.Start();
            }
        }

        public virtual void Abort()
        {
            if (_thread != null)
            {
                try
                {
                    //Thread
                    _thread.Abort();
                    _thread = null;
                }
                catch (System.Exception ex)
                {
                    Log.Error(Name,"Abort:"+ex);
                }
               
            }
        }

        public abstract void DoFunction();
    }
}
