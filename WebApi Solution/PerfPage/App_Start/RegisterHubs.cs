﻿using System.Web;
using System.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(SignalRMap.RegisterHubs), "Start")]
namespace SignalRMap
{
    public static class RegisterHubs
    {
        public static void Start()
        {
            // Register the default hubs route: ~/signalr/hubs
            RouteTable.Routes.MapHubs();            
        }
    }
}
