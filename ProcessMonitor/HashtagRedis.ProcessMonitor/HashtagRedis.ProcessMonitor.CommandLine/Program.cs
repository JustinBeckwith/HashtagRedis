using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashtagRedis.ProcessMonitor.Redis;
using System.IO;

namespace HashtagRedis.ProcessMonitor.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ProcessMontitorHost())
            {
                host.Start();

                var redis = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "redis-server.exe");
                var processInfo = host.Spawn(redis, null, "--port 3000");

                Console.ReadLine();
            }
        }
    }
}
