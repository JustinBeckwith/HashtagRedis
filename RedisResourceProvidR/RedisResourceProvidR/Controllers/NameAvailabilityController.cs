using System;
using System.Net;
using System.Web.Http;
using WindowsAzure.ResourceProviderContracts;

namespace ResourceProvidR.Controllers
{
    public class NameAvailabilityController : ApiController
    {
        //
        // GET /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}?op=checknameavailability&resourceName={resourceName}
        //
        [HttpGet]
        public ResourceNameAvailability CheckResourceNameAvailability(string subscriptionId, string cloudServiceName, string resourceType, [FromUri] string op, [FromUri] string resourceName)
        {
            // Determine which operation to perform
            if (String.CompareOrdinal(op, "checknameavailability") != 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // TODO: Check for availability

            return new ResourceNameAvailability()
            {
                IsAvailable = true
            };
        }
    }
}