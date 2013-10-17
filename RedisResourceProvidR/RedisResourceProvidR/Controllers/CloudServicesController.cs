using ResourceProvidR.Models;
using System;
using System.Net;
using System.Web.Http;
using WindowsAzure.ResourceProviderContracts;
using System.Collections.Generic;
using WindowsAzure.ResourceProviderDataLayer.Entities;

namespace ResourceProvidR.Controllers
{
    public class CloudServicesController : ApiController
    {
        //
        // GET /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}
        //
        [HttpGet]
        public CloudServiceOutput GetAllResourcesInCloudService(string subscriptionId, string cloudServiceName)
        {
            if (String.IsNullOrEmpty(cloudServiceName) || String.IsNullOrEmpty(cloudServiceName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            IEnumerable<ResourceEntity> allResources = WebApiApplication.Storage.ResourceStorage.GetResources(subscriptionId, cloudServiceName);
            
            // Create the cloud service output
            CloudServiceOutput cloudServiceOutput = new CloudServiceOutput()
            {
                 GeoRegion = String.Empty,
                 Resources = new ResourceOutputCollection()
            };

            // Insert all resources
            foreach (ResourceEntity resourceEntity in allResources)
            {
                // Not great to reach out to another controller
                cloudServiceOutput.Resources.Add(ResourcesController.ResourceOutputFromResourceEntity(resourceEntity));
            }

            // Geo Region - we do nto save cloud services by themselves so deduce the region
            if (cloudServiceOutput.Resources.Count > 0)
            {
                cloudServiceOutput.GeoRegion = cloudServiceOutput.Resources[0].CloudServiceSettings.GeoRegion;
            }

            return cloudServiceOutput;

            //return DataModel.GetCloudServiceBySubscriptionIdAndName(subscriptionId, cloudServiceName);
        }

        //
        // DELETE /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}
        //
        [HttpDelete]
        public CloudServiceOutput DeleteCloudService(string subscriptionId, string cloudServiceName)
        {
            if (String.IsNullOrEmpty(cloudServiceName) || String.IsNullOrEmpty(cloudServiceName))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // TODO - Need to clean this
            return new CloudServiceOutput()
            {
                GeoRegion = String.Empty,
                Resources = new ResourceOutputCollection()
            };

            //return DataModel.DeleteCloudService(subscriptionId, cloudServiceName);
        }
    }
}