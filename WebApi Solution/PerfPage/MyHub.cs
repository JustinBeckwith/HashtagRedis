using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PerfPage
{
    public class MyHub : Hub
    {
        public void RedisTests()
        {
            Clients.All.showmessage("woot");
        }
    }
}