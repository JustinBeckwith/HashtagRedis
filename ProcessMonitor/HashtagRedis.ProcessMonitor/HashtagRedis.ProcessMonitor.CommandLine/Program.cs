using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashtagRedis.ProcessMonitor.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ProcessMontitorHost())
            {
                host.Start();

                Console.ReadLine();
            }
        }
    }
}
