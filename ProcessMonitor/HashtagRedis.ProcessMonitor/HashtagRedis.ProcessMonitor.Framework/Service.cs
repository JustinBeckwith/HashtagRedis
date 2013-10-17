using System;

namespace HashtagRedis.ProcessMonitor
{
    public abstract class Service : IService
    {
        public virtual void Initialize()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        ~Service()
        {
            this.Dispose(false);
        }
    }
}
