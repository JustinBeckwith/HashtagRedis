﻿using redis.process;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Generic;

namespace redis.logic
{
    public class Manager
    {
        private RedisProcessManager _processManager = new RedisProcessManager();
        private ConcurrentDictionary<string, InstanceInfo> _database = new ConcurrentDictionary<string, InstanceInfo>();
        private long _lastPort = 10000;

        public InstanceInfo CreateInstance(string instanceId)
        {
            // Look for existing instance
            InstanceInfo info;
            if (this._database.TryGetValue(instanceId, out info))
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
            info.processId = this._processManager.Spawn(details).ProcessId;

            // Create connectionString
            info.connectionString = string.Format("redis://{0}:{1}@{2}:{3}/", info.instanceId, info.password, "hashtagredis.cloudapp.net", info.port);

            // Return instance
            this._database.TryAdd(instanceId, info); // false means it already existed; should be impossible
            return info;
        }

        public bool IsInstanceRunning(string instanceId)
        {
            bool isRunning = false;

            InstanceInfo info;
            if (_database.TryGetValue(instanceId, out info))
            {
                isRunning = this._processManager.IsRunning(new RedisProcessInfo { ProcessId = info.processId });
            }

            return isRunning;
        }

        public void DeleteInstance(string instanceId)
        {
            InstanceInfo info;
            if (_database.TryGetValue(instanceId, out info))
            {
                this._processManager.Stop(new RedisProcessInfo { ProcessId = info.processId });
                this._database.TryRemove(instanceId, out info);
            }
        }

        public IEnumerable<PublicInstanceInfo> GetAllInstances()
        {
            var query = from instance in this._database.Values
                        select new PublicInstanceInfo { instanceId = instance.instanceId };

            return query;
        }
    }

    public class PublicInstanceInfo
    {
        public string instanceId { get; set; }
    }

    public class InstanceInfo : PublicInstanceInfo
    {
        public string password { get; set; }
        public int processId { get; set; }
        public ushort port { get; set; }
        public string connectionString { get; set; }
    }
}
