using System.Collections.Concurrent;
using System.Threading;

namespace redis.logic
{
    static class Manager
    {
        public static InstanceInfo CreateInstance(string instanceId)
        {
            InstanceInfo info;
            if (_table.TryGetValue(instanceId, out info))
            {
                return info;
            }
            info = new InstanceInfo();
            info.instanceId = instanceId;
            info.port = (ushort)Interlocked.Increment(ref _nextPort);
            // TODO: Create process here...
            //info.processId = whatever;
            // TODO: Create connectionString
            //info.connectionString = "redis://username:password@server:port/";
            _table.TryAdd(instanceId, info); // false means it already existed; should be impossible
            return info;
        }

        private static ConcurrentDictionary<string, InstanceInfo> _table = new ConcurrentDictionary<string, InstanceInfo>();
        private static long _nextPort = 2000;

    }

    class InstanceInfo
    {
        public string instanceId { get; set; }
        public int processId { get; set; }
        public ushort port { get; set; }
        public string connectionString { get; set; }
    }
}
