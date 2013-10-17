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
        private static readonly string ExecutablePath = Path.Combine(@"c:\external", "redis-server.exe");
        
        public RedisProcessInfo Spawn(RedisStartDetails details)
        {
            StringBuilder arguments = new StringBuilder();
            arguments.Append(String.IsNullOrWhiteSpace(details.ConfigurationFilePath) ? String.Empty : details.ConfigurationFilePath);
            if (details.Port > 0)
            {
                arguments.AppendFormat(" --port {0}", details.Port);
            }

            arguments.AppendFormat(" --requirepass {0}", "redpolo");

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(ExecutablePath, arguments.ToString());

            process.Start();
            return new RedisProcessInfo()
            {
                ProcessId = process.Id,
            };
        }

        public bool IsRunning(RedisProcessInfo info) 
        {
            Process process = Process.GetProcesses().Where(p => p.Id == info.ProcessId).FirstOrDefault();
            return process != null;
        }

        public void Stop(RedisProcessInfo info)
        {
            Process process = Process.GetProcesses().Where(p => p.Id == info.ProcessId).FirstOrDefault();
            if (process != null)
            {
                process.Kill();
            }
        }
    }
}
