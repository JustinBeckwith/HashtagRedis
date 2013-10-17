using ServiceStack.Redis;
using System;
using System.Diagnostics;
using System.Linq;

namespace PerformanceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RedisClient("crestfish.redistogo.com", 10216, "password");
            var count = 10;
            var sw = new Stopwatch();
            sw.Start();
            foreach (var num in Enumerable.Range(0, count))
            {
                var key = num.ToString();
                client.Add(key, key);
            }
            sw.Stop();
            Console.WriteLine("Time to add {0} key/value pairs: {1}ms", count, sw.ElapsedMilliseconds);
        }
    }
}
