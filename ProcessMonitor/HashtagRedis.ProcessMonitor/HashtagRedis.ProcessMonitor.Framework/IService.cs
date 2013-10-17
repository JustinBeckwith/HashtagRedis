using System;

namespace HashtagRedis.ProcessMonitor
{
    public interface IService : IDisposable
    {
        void Initialize();
    }
}
