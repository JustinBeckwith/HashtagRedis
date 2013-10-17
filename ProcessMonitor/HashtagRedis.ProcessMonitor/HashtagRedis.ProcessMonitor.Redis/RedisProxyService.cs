using System.ComponentModel.Composition;

namespace HashtagRedis.ProcessMonitor.Redis
{
    [Export(typeof(IService))]
    internal class RedisProxyService : Service
    {
    }
}
