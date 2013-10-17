using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace redis.api.Controllers
{
    public class RedisController : ApiController
    {
        private const string InstanceId = "instanceId";

        [HttpPost]
        public HttpResponseMessage Provision(string json)
        {
            var details = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var instanceInfo = redis.logic.Manager.CreateInstance(details[RedisController.InstanceId]);
            return this.Request.CreateResponse(HttpStatusCode.Created, instanceInfo.connectionString);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(string json)
        {
            var details = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var response = this.Request.CreateResponse(HttpStatusCode.NoContent);

            // BusinessLogic.DeleteInstance(details[RedisController.InstanceId]);

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetStatus(string json)
        {
            var details = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            // var status = BusinessLogic.GetStatus(details[RedisController.InstanceId]);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            return response;
        }
    }
}
