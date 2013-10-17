using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ResourceProvidR
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static WindowsAzure.ResourceProviderDataLayer.Storage Storage = new WindowsAzure.ResourceProviderDataLayer.Storage("hashtagredis", "+1DBfTjOEisXv8z5agsZZlcV+opJsaILERc09+C8tEg6zhulM15NQmVNXKg1o24ku+HzAUrVpW31hVKEYo+4gA==");

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}