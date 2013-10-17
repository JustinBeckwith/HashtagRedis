using System.Web.Http;

namespace ResourceProvidR
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // -------- Subscription Notifications --------
            // POST /subscriptions/{subscriptionId}/Events
            config.Routes.MapHttpRoute(
                name: "Events",
                routeTemplate: "subscriptions/{subscriptionId}/Events",
                defaults: new { controller = "Events", action = "HandleSubscriptionNotifications" }
            );

            // -------- Cloud Services Management --------

            // Cloud Service level resource management
            // GET /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}
            // DELETE /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}
            config.Routes.MapHttpRoute(
               name: "CloudService-Management",
               routeTemplate: "subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}",
               defaults: new
               {
                   controller = "CloudServices"
               }
           );


            // ----- Name availability checks ------

            // GET /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}?op=checknameavailability&resourceName={resourceName}
            //config.Routes.MapHttpRoute(
            //     name: "Name-Availability-Check",
            //     routeTemplate: "subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}",
            //     defaults: new
            //     {
            //         controller = "NameAvailability"
            //     }
            // );

            // -------- Resource Management --------

            // GET /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}/{resourceName}
            // PUT /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}/{resourceName}
            // DELETE subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}/{resourceName}
            config.Routes.MapHttpRoute(
                name: "Resource-Management",
                routeTemplate: "subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}/{resourceName}",
                defaults: new
                {
                    controller = "Resources"
                }
            );

            // -------- Single Sign On --------

            // GET subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}/{resourceName}/SSOToken
            config.Routes.MapHttpRoute(
                name: "Resource-Management-SingleSignOn",
                routeTemplate: "subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}/{resourceName}/SsoToken",
                defaults: new
                {
                    controller = "Resources",
                    action = "SsoToken"
                }
            );
        }
    }
}