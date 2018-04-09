using System.Threading;

namespace SQL_CRM
{
    public class ThreadWraper
    {
        public delegate void ThreadHandler();
        private Thread _thread;
        private readonly ThreadHandler _handler;
        private bool _running;

        public ThreadWraper(ThreadHandler handler)
        {
            _handler = handler;
        }

        public void Start()
        {
            _thread = new Thread(() =>
            {
                while (_running) _handler();
            });
            _running = true;
            _thread.Start();
        }

        public void Abort()
        {
            _running = false;

            _thread = null;
        }

    }
}