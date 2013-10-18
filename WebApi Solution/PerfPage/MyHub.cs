using Microsoft.AspNet.SignalR;
using ServiceStack.Redis;
using System.Diagnostics;
using System.Linq;

namespace PerfPage
{
    public class MyHub : Hub
    {
        private const int OPERATIONS = 2;

        abstract class Harness
        {
            public abstract string Name { get; }
            public abstract void Create(string data);
            public abstract void Read(string data);
            public abstract void Delete(string data);
        }

        class RedisHarness : Harness
        {
            private readonly RedisClient _client;

            public RedisHarness(string host, int port, string password)
            {
                _client = new RedisClient(host, port /*, password*/);
            }

            public override string Name
            {
                get { return "Redis"; }
            }

            public override void Create(string data)
            {
                _client.Add(data, data);
            }

            public override void Read(string data)
            {
                _client.Get(data);
            }

            public override void Delete(string data)
            {
                _client.Remove(data);
            }
        }

        public void RedisTests(string server)
        {
            var parts = server.Split(':');
            var host = parts.First().Trim();
            int port;
            int.TryParse(parts.Last().Trim(), out port);
            var password = "redpolo";
            RunTests(new RedisHarness(host, port, password));
        }

        public void OtherTests()
        {
            //RunTests(new OtherHarness());
        }

        private void RunTests(Harness harness)
        {
            Clients.All.showmessage("Starting " + harness.Name + " tests...");
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < OPERATIONS; i++)
            {
                var data = i.ToString();
                harness.Create(data);
            }
            sw.Stop();
            Clients.All.showmessage("Put " + OPERATIONS + " values in " + sw.ElapsedMilliseconds + "ms.");
            sw.Reset();
            sw.Start();
            for (var i = 0; i < OPERATIONS; i++)
            {
                var data = i.ToString();
                harness.Read(data);
            }
            sw.Stop();
            Clients.All.showmessage("Read " + OPERATIONS + " values in " + sw.ElapsedMilliseconds + "ms.");
            sw.Reset();
            sw.Start();
            for (var i = 0; i < OPERATIONS; i++)
            {
                var data = i.ToString();
                harness.Delete(data);
            }
            sw.Stop();
            Clients.All.showmessage("Deleted " + OPERATIONS + " values in " + sw.ElapsedMilliseconds + "ms.");
            Clients.All.showmessage("Completed " + harness.Name + " tests...");
        }
    }
}
