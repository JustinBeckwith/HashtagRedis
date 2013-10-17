using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace redis.api.Controllers
{
    public class RedisController : ApiController
    {
        public HttpResponseMessage Post(ProvisioningDetails details)
        {
            
        }

        public HttpResponseMessage Delete(string instance)
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK, "super");
            return response;
        }

        public HttpResponseMessage Get(string instance)
        {
            
        }
    }

    public class ProvisioningDetails
    {
        
    }
}
