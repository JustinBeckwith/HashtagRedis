using redis.process;
using System.Collections.Concurrent;
using System.Threading;

namespace redis.logic
{
    public class Manager
    {
        private RedisProcessManager _processManager = new RedisProcessManager();
        private ConcurrentDictionary<string, InstanceInfo> _table = new ConcurrentDictionary<string, InstanceInfo>();
        private long _lastPort = 2000;

        public InstanceInfo CreateInstance(string instanceId)
        {
            // Look for existing instance
            InstanceInfo info;
            if (_table.TryGetValue(instanceId, out info))
            {
                return info;
            }

            // Create a new instance
            info = new InstanceInfo();
            info.instanceId = instanceId;

            // Get unique port
            info.port = (ushort)Interlocked.Increment(ref _lastPort);

            // Create a unique password
            info.password = "redpolo";

            // Create process
            var details = new RedisStartDetails
            {
                Port = info.port,
            };
            info.processId = _processManager.Spawn(details).ProcessId;

            // Create connectionString
            info.connectionString = string.Format("redis://{0}:{1}@{2}:{3}/", info.instanceId, info.password, "hashtagredis.cloudapp.net", info.port);

            // Return instance
            _table.TryAdd(instanceId, info); // false means it already existed; should be impossible
            return info;
        }
    }

    public class InstanceInfo
    {
        public string instanceId { get; set; }
        public string password { get; set; }
        public int processId { get; set; }
        public ushort port { get; set; }
        public string connectionString { get; set; }
    }
}
