using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redis.process
{
    public class RedisProcessManager
    {
        private static readonly string ExecutablePath = Path.Combine(Path.GetDirectoryName(typeof(RedisProcessManager).Assembly.Location), "redis-server.exe");
        
        public RedisProcessInfo Spawn(RedisStartDetails details)
        {
            StringBuilder arguments = new StringBuilder();
            arguments.Append(String.IsNullOrWhiteSpace(details.ConfigurationFilePath) ? String.Empty : details.ConfigurationFilePath);
            if (details.Port > 0)
            {
                arguments.AppendFormat(" --port {0}", details.Port);
            }

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(ExecutablePath, arguments.ToString());

            process.Start();
            return new RedisProcessInfo()
            {
                ProcessId = process.Id,
            };
        }
    }
}
