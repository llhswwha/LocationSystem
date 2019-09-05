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
                _thread.IsBackground = true;
                _thread.Start();
            }
        }

        public void Abort()
        {
            if (_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }
        }

        public abstract void DoFunction();
    }
}
