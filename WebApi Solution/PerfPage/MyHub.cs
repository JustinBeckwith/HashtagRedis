using Microsoft.AspNet.SignalR;
using ServiceStack.Redis;
using System.Diagnostics;

namespace PerfPage
{
    public class MyHub : Hub
    {
        private const int OPERATIONS = 2;

        public void RedisTests()
        {
            Clients.All.showmessage("Starting Redis tests...");
            var client = new RedisClient("angler.redistogo.com", 9313, "553eee0ecf0a87501f5c67cb4302fc55");
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < OPERATIONS; i++)
            {
                var data = i.ToString();
                client.Add(data, data);
            }
            sw.Stop();
            Clients.All.showmessage("Put " + OPERATIONS + " values in " + sw.ElapsedMilliseconds + "ms.");
            sw.Reset();
            sw.Start();
            for (var i = 0; i < OPERATIONS; i++)
            {
                var data = i.ToString();
                client.Get(data);
            }
            sw.Stop();
            Clients.All.showmessage("Read " + OPERATIONS + " values in " + sw.ElapsedMilliseconds + "ms.");
            sw.Reset();
            sw.Start();
            for (var i = 0; i < OPERATIONS; i++)
            {
                var data = i.ToString();
                client.Remove(data);
            }
            sw.Stop();
            Clients.All.showmessage("Deleted " + OPERATIONS + " values in " + sw.ElapsedMilliseconds + "ms.");
            Clients.All.showmessage("Completed Redis tests...");
        }

        public void OtherTests()
        {
        }
    }
}
