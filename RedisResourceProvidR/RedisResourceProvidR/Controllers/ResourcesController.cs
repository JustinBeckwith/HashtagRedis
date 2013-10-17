using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using WindowsAzure.ResourceProviderContracts;
using WindowsAzure.ResourceProviderDataLayer;
using WindowsAzure.ResourceProviderDataLayer.Entities;

namespace ResourceProvidR.Controllers
{
    public class ResourcesController : ApiController
    {
        //
        // GET /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceName}
        //
        [HttpGet]
        public ResourceOutput GetResource(string subscriptionId, string cloudServiceName, string resourceType, string resourceName)
        {
            if (String.IsNullOrEmpty(cloudServiceName) || String.IsNullOrEmpty(resourceType) || String.IsNullOrEmpty(resourceName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            ResourceEntity resourceEntity = WebApiApplication.Storage.ResourceStorage.GetResource(subscriptionId, cloudServiceName, resourceType, resourceName);
            if (resourceEntity != null)
            {
                return ResourceOutputFromResourceEntity(resourceEntity);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            //return DataModel.GetResource(subscriptionId, cloudServiceName, resourceName);
        }

        public static ResourceOutput ResourceOutputFromResourceEntity(ResourceEntity resourceEntity)
        {
            return new ResourceOutput()
            {
                BillingETag = resourceEntity.BillingETag,
                CloudServiceSettings = new CloudServiceSettings()
                {
                    GeoRegion = resourceEntity.CloudServiceGeoRegion
                },
                ETag = resourceEntity.ResourceETag,
                //IntrinsicSettings = resourceEntity.,
                Label = resourceEntity.Label,
                Name = resourceEntity.Name,
                OperationStatus = new OperationStatus()
                {
                    Error = new ErrorData()
                    {
                        ExtendedCode = String.Empty,
                        HttpCode = resourceEntity.LastOperationHttpCode,
                        Message = resourceEntity.LastOperationMessage,
                    },
                    Result = (OperationResult)Enum.Parse(typeof(OperationResult), resourceEntity.LastOperationResult)
                },
                OutputItems = DeserializeOutputItems(resourceEntity.OutputItems),
                Plan = resourceEntity.Plan,
                PromotionCode = resourceEntity.PromotionCode,
                SchemaVersion = resourceEntity.SchemaVersion,
                State = resourceEntity.State,
                SubState = resourceEntity.SubState,
                Type = resourceEntity.Type
            };
        }

        //
        // PUT /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceName}
        //
        [HttpPut]
        public ResourceOutput ProvisionOrUpdateResource(string subscriptionId, string cloudServiceName, string resourceType, string resourceName , ResourceInput resource)
        {
            //string theRawBody = Request.Content.ReadAsStringAsync().Result;
            //return null;

            if (String.IsNullOrEmpty(cloudServiceName) || String.IsNullOrEmpty(resourceType) || String.IsNullOrEmpty(resourceName) || (resource == null))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            

            ResourceEntity resourceEntity = WebApiApplication.Storage.ResourceStorage.GetResource(subscriptionId, cloudServiceName, resourceType, resourceName);
            if (resourceEntity == null)
            {
                                // Create a new resource
                resourceEntity = new ResourceEntity(subscriptionId, cloudServiceName, resourceType, resourceName)
                {
                    BillingETag = resource.BillingETag,
                    CloudServiceGeoRegion = resource.CloudServiceSettings.GeoRegion,
                    CloudServiceName = cloudServiceName,
                    ResourceETag = resource.ETag,
                    //IntrinsicSettings =
                    Label = resource.Label,
                    Name = resourceName,
                    OutputItems = null,
                    Plan = resource.Plan,
                    PromotionCode = resource.PromotionCode,
                    SchemaVersion = resource.SchemaVersion,

                    // Create always started
                    State = ResourceState.Started.ToString(),

                    SubscriptionId = subscriptionId,
                    SubState = String.Empty,

                    Type = resourceType,

                    LastOperation = "Create",
                    LastOperationHttpCode = 200,
                    LastOperationMessage = String.Empty,
                    LastOperationResult = OperationResult.Succeeded.ToString()
                };

                // Provision
                resourceEntity.OutputItems = SerializeOutputItems(ProvisionOnHashRedis(resourceEntity.PartitionKey + "-" + resourceEntity.RowKey));

                // Update resource on storage
                WebApiApplication.Storage.ResourceStorage.AddOrUpdateResource(resourceEntity);
            }
            else
            {
                // Only attempt if we have not yet already done so
                if (String.CompareOrdinal(resource.ETag, resourceEntity.ResourceETag) != 0)
                {
                    // Update an existing resource - Some things can never change for a resource
                    resourceEntity.BillingETag = resource.BillingETag;
                    resourceEntity.CloudServiceGeoRegion = resource.CloudServiceSettings.GeoRegion;
                    // Can never change -> resourceEntity.CloudServicename = cloudServiceName
                    resourceEntity.ResourceETag = resource.ETag;
                    //IntrinsicSettings = XXX
                    resourceEntity.Label = resource.Label;
                    // Can never change -> resourceEntity.Name = resourceName
                    //resourceEntity.OutputItems = SerializeOutputItems(resource.);
                    resourceEntity.Plan = resource.Plan;
                    resourceEntity.PromotionCode = resource.PromotionCode;
                    resourceEntity.SchemaVersion = resource.SchemaVersion;

                    // Upgrade always started
                    resourceEntity.State = ResourceState.Started.ToString();
                    // Can never change -> resourceEntity.SubscriptionId = subscriptionId

                    resourceEntity.SubState = String.Empty;
                    // Can never change -> resourceEntity.Type = resourceType

                    resourceEntity.LastOperation = "Update";
                    resourceEntity.LastOperationHttpCode = 200;
                    resourceEntity.LastOperationMessage = String.Empty;
                    resourceEntity.LastOperationResult = OperationResult.Succeeded.ToString();

                    // Update resource on storage
                    WebApiApplication.Storage.ResourceStorage.AddOrUpdateResource(resourceEntity);
                }
            }

            // Produce output for Azure Runtime
            ResourceOutput resourceToReturn = new ResourceOutput()
            {
                BillingETag = resourceEntity.BillingETag,
                CloudServiceSettings = new CloudServiceSettings()
                {
                    GeoRegion = resourceEntity.CloudServiceGeoRegion
                },
                ETag = resourceEntity.ResourceETag,
                //IntrinsicSettings = resourceEntity.XXX,
                Label = resourceEntity.Label,
                Name = resourceEntity.Name,
                OperationStatus = new OperationStatus()
                {
                    Error = new ErrorData()
                    {
                        ExtendedCode = String.Empty,
                        HttpCode = resourceEntity.LastOperationHttpCode,
                        Message = resourceEntity.LastOperationMessage
                    },
                    Result = (OperationResult)Enum.Parse(typeof(OperationResult), resourceEntity.LastOperationResult)
                },
                OutputItems = DeserializeOutputItems(resourceEntity.OutputItems),
                Plan = resourceEntity.Plan,
                PromotionCode = resourceEntity.PromotionCode,
                SchemaVersion = resourceEntity.SchemaVersion,
                State = resourceEntity.State,
                SubState = resourceEntity.SubState,
                Type = resourceEntity.Type
            };

            return resourceToReturn;

            //return DataModel.ProvisionOrUpdateResource(subscriptionId, cloudServiceName, resourceType, resourceName, resource);
        }

        //
        // DELETE /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceName}
        //
        [HttpDelete]
        public void DeleteResource(string subscriptionId, string cloudServiceName, string resourceType, string resourceName)
        {
            if (String.IsNullOrEmpty(cloudServiceName) || String.IsNullOrEmpty(resourceType) || String.IsNullOrEmpty(resourceName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            ResourceEntity resourceEntity = WebApiApplication.Storage.ResourceStorage.GetResource(subscriptionId, cloudServiceName, resourceType, resourceName);
            if (resourceEntity != null)
            {
                DeProvisionOnHashRedis(resourceEntity.PartitionKey + "-" + resourceEntity.RowKey);
                WebApiApplication.Storage.ResourceStorage.DeleteResource(subscriptionId, cloudServiceName, resourceType, resourceName);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            //DataModel.DeleteResource(subscriptionId, cloudServiceName, resourceName);
        }

        //
        // GET /subscriptions/{subscriptionId}/cloudservices/{cloudServiceName}/resources/{resourceType}/{resourceName}/GenerateSSOToken
        //
        [HttpPost]
        public SsoToken SsoToken(string subscriptionId, string cloudServiceName, string resourceType, string resourceName)
        {
            if (String.IsNullOrEmpty(cloudServiceName) || String.IsNullOrEmpty(resourceType) || String.IsNullOrEmpty(resourceName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            ResourceEntity resourceEntity = WebApiApplication.Storage.ResourceStorage.GetResource(subscriptionId, cloudServiceName, resourceType, resourceName);
            if (resourceEntity != null)
            {
                byte[] theVerySecretKety = UTF8Encoding.UTF32.GetBytes("I do not always use WCF but when I do, I prefer BasicHttpBinding");

                string token = String.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", subscriptionId, cloudServiceName, resourceType, resourceName);
                byte[] theHashedData;
                using (HMACSHA256 hmacSha1 = new HMACSHA256())
                {
                    theHashedData = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(token));
                }

                SsoToken theToken = new SsoToken()
                {
                    Token = Base32NoPaddingEncode(theHashedData),
                    TimeStamp = DateTime.UtcNow.Ticks.ToString()
                };

                return theToken;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private static string Base32NoPaddingEncode(byte[] data)
        {
            const string base32StandardAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

            StringBuilder result = new StringBuilder(Math.Max((int)Math.Ceiling(data.Length * 8 / 5.0), 1));

            byte[] emptyBuffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] workingBuffer = new byte[8];

            // Process input 5 bytes at a time
            for (int i = 0; i < data.Length; i += 5)
            {
                int bytes = Math.Min(data.Length - i, 5);
                Array.Copy(emptyBuffer, workingBuffer, emptyBuffer.Length);
                Array.Copy(data, i, workingBuffer, workingBuffer.Length - (bytes + 1), bytes);
                Array.Reverse(workingBuffer);
                ulong val = BitConverter.ToUInt64(workingBuffer, 0);

                for (int bitOffset = ((bytes + 1) * 8) - 5; bitOffset > 3; bitOffset -= 5)
                {
                    result.Append(base32StandardAlphabet[(int)((val >> bitOffset) & 0x1f)]);
                }
            }

            return result.ToString();
        }

        private OutputItemCollection ProvisionOnHashRedis(string instanceId)
        {
            try
            {
                HashTagRedisProvisioningClient provisioningClient = new HashTagRedisProvisioningClient();
                ProvisioningResult result = provisioningClient.Provision(instanceId);

                OutputItemCollection outputItems = new OutputItemCollection();
                outputItems.Add(new OutputItem() { Key = "CacheUrl", Value = result.CacheUrl });
                return outputItems;
            }
            catch (Exception e)
            {
                throw new Exception("HashTag Redis Provisioning failed", e);
            }
        }

        private void DeProvisionOnHashRedis(string instanceId)
        {
            try
            {
                HashTagRedisProvisioningClient provisioningClient = new HashTagRedisProvisioningClient();
                provisioningClient.DeProvision(instanceId);
            }
            catch (Exception e)
            {
            }
        }

        private static string SerializeOutputItems(OutputItemCollection outputItems)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(outputItems);
        }

        private static OutputItemCollection DeserializeOutputItems(string serializedOuputItems)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OutputItemCollection>(serializedOuputItems);
        }
    }
}
