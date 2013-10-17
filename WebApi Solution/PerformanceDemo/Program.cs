using ServiceStack.Redis;

namespace PerformanceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RedisClient("angler.redistogo.com", 9313);
            // ...
        }
    }
}
