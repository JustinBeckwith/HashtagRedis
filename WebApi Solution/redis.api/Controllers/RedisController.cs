using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            string[] args = {}; //todo: init from details
            string redis_server_path = "redis-server";
            var pi = new Process(); // todo: pass redis config file
            var p = Process.Start(redis_server_path, String.Join(" ", args));
            
            // assuming starting the porcess was succesful 
            // store the userid, portid, processid into table storage
            // build url
            // return url
        }

        public HttpResponseMessage Delete(string instance)
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK, "super");
            return response;
        }

        public HttpResponseMessage Get(ProvisioningDetails details)
        {
            
        }
    }

    public class ProvisioningDetails
    {
        
    }
}
