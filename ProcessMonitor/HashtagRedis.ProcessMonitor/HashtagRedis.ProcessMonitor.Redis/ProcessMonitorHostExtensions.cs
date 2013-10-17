using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashtagRedis.ProcessMonitor.Redis
{
    public static class ProcessMonitorHostExtensions
    {
        public static RedisProcessInfo Spawn(this ProcessMontitorHost host, string executablePath, string configurationFile, string arguments)
        {
            var commandLineArguments = String.Join(" ", new string[]{ configurationFile, arguments }.Where(s => !String.IsNullOrWhiteSpace(s)));

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(executablePath, commandLineArguments);

            process.Start();
            return new RedisProcessInfo()
            {
                Id = process.Id,
            };
        }
    }
}
