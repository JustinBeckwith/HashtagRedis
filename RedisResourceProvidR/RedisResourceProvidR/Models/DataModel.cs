using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WindowsAzure.ResourceProviderContracts;

namespace ResourceProvidR.Models
{
    internal class Subscription
    {
        public string Id { get; set; }
        public List<CloudService> CloudServices { get; private set; }

        public Subscription()
        {
            CloudServices = new List<CloudService>();
        }
    }

    internal class CloudService
    {
        public string Name { get; set; }
        public int IncarnationId { get; set; }
        public string GeoLocation { get; set; }

        public List<ResourceOutput> Resources { get; private set; }

        public CloudService()
        {
            IncarnationId = 0;
            GeoLocation = "";
            Resources = new List<ResourceOutput>();
        }
    }

    public static class DataModel
    {
        // Global lock protecting all in-memory data structures against concurrent access
        private static object theMassiveLock = new object();

        // Master data structure holding all known subscriptions, indexed by Id for quick access
        private static Dictionary<string, Subscription> allSubscriptions = new Dictionary<string, Subscription>();


        //----------------------------- Cloud Service Management -----------------------------

        public static CloudServiceOutput GetCloudServiceBySubscriptionIdAndName(string subscriptionId, string cloudServiceName)
        {
            lock (theMassiveLock)
            {
                Subscription subscription;
                if (!allSubscriptions.TryGetValue(subscriptionId, out subscription))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                CloudService theMatchingCloudService = subscription.CloudServices.SingleOrDefault<CloudService>(cs => String.CompareOrdinal(cs.Name, cloudServiceName) == 0);

                if (theMatchingCloudService == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                CloudServiceOutput cloudServiceOutput = new CloudServiceOutput()
                {
                    GeoRegion = theMatchingCloudService.Resources.Count > 0 ? theMatchingCloudService.Resources[0].CloudServiceSettings.GeoRegion : String.Empty,
                    Resources = new ResourceOutputCollection(theMatchingCloudService.Resources)
                };

                return cloudServiceOutput;
            }
        }

        public static CloudServiceOutput DeleteCloudService(string subscriptionId, string cloudServiceName)
        {
            lock (theMassiveLock)
            {
                Subscription subscription;
                if (!allSubscriptions.TryGetValue(subscriptionId, out subscription))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                CloudService theMatchingCloudService = subscription.CloudServices.SingleOrDefault<CloudService>(cs => String.CompareOrdinal(cs.Name, cloudServiceName) == 0);

                if (theMatchingCloudService == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                subscription.CloudServices.Remove(theMatchingCloudService);

                return new CloudServiceOutput()
                {
                    GeoRegion = theMatchingCloudService.GeoLocation,
                    Resources = new ResourceOutputCollection(theMatchingCloudService.Resources)
                };
            }
        }


        //----------------------------- Resource Management -----------------------------

        public static ResourceOutput GetResource(string subscriptionId, string cloudServiceName, string resourceName)
        {
            lock (theMassiveLock)
            {
                Subscription subscription;
                if (!allSubscriptions.TryGetValue(subscriptionId, out subscription))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                CloudService theMatchingCloudService = subscription.CloudServices.SingleOrDefault<CloudService>(cs => String.CompareOrdinal(cs.Name, cloudServiceName) == 0);

                if (theMatchingCloudService == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                ResourceOutput theMatchingResource = theMatchingCloudService.Resources.FirstOrDefault(r => String.Compare(r.Name, resourceName) == 0);

                if (theMatchingResource != null)
                {
                    return theMatchingResource;
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }

        public static ResourceOutput ProvisionOrUpdateResource(string subscriptionId, string cloudServiceName, string resourceType, string resourceName, ResourceInput resource)
        {
            ResourceOutput output;

            lock (theMassiveLock)
            {
                Subscription subscription;
                if (!allSubscriptions.TryGetValue(subscriptionId, out subscription))
                {
                    subscription = new Subscription() { Id = subscriptionId };
                    allSubscriptions[subscriptionId] = subscription;
                }

                CloudService theMatchingCloudService = subscription.CloudServices.SingleOrDefault<CloudService>(cs => String.CompareOrdinal(cs.Name, cloudServiceName) == 0);

                if (theMatchingCloudService == null)
                {
                    theMatchingCloudService = new CloudService() { Name = cloudServiceName };
                    subscription.CloudServices.Add(theMatchingCloudService);
                }

                ResourceOutput theMatchingResource = theMatchingCloudService.Resources.FirstOrDefault(r => String.Compare(r.Name, resourceName) == 0);

                if (theMatchingResource != null)
                {
                    // We can be called to provision / update a resource several time - Ignore the request if we have a record of the resource with the same incarnation id
                    if (theMatchingResource.ETag != resource.ETag)
                    {
                        theMatchingResource.CloudServiceSettings = resource.CloudServiceSettings;
                        theMatchingResource.ETag = resource.ETag;
                        theMatchingResource.IntrinsicSettings = resource.IntrinsicSettings;
                        theMatchingResource.Name = resourceName;
                        theMatchingResource.OperationStatus = new OperationStatus()
                        {
                            Error = new ErrorData() { HttpCode = 200, Message="OK" }, 
                            Result = OperationResult.Succeeded
                        };
                        theMatchingResource.Plan = resource.Plan;
                        theMatchingResource.SchemaVersion = resource.SchemaVersion;
                        theMatchingResource.State = ResourceState.Started.ToString();
                        theMatchingResource.SubState = "";
                        theMatchingResource.Type = resource.Type;
                        theMatchingResource.UsageMeters = GenerateUsageMeters();
                    }

                    output = theMatchingResource;

                    //UpgradeRequest upgradeParams = new UpgradeRequest() { heroku_id = theMatchingCloudService.Name + "-" + theMatchingResource.Type + "-" + theMatchingResource.Name, plan = output.Plan};
                  //  UpgradeResponse upgradeResponse = EnsureHerokuClient().Upgrade(/* the id form the mapped table*/, upgradeParams);
                }
                else
                {
                    output = new ResourceOutput()
                    {
                        CloudServiceSettings = resource.CloudServiceSettings,
                        ETag = resource.ETag,
                        IntrinsicSettings = resource.IntrinsicSettings,
                        Name = resourceName,
                        OperationStatus = new OperationStatus()
                        {
                            Error = new ErrorData() { HttpCode = 200, Message="OK" }, 
                            Result = OperationResult.Succeeded
                        },
                        OutputItems = GenerateOutputItems(),
                        Plan = resource.Plan,
                        SchemaVersion = resource.SchemaVersion,
                        State = ResourceState.Started.ToString(),
                        SubState = "",
                        Type = resource.Type,
                        UsageMeters = GenerateUsageMeters()
                    };

                    theMatchingCloudService.Resources.Add(output);

                    //ProvisionRequest provisionParams = new ProvisionRequest() { heroku_id = theMatchingCloudService.Name + "-" + theMatchingResource.Type + "-" + theMatchingResource.Name, plan = output.Plan };
                    //ProvisionResponse provisionResponse = EnsureHerokuClient().Provision(provisionParams);

                    // Get ID out of response, map back to resource table
                }
            }

            return output;
        }

        public static void DeleteResource(string subscriptionId, string cloudServiceName, string resourceName)
        {
            lock (theMassiveLock)
            {
                Subscription subscription;
                if (!allSubscriptions.TryGetValue(subscriptionId, out subscription))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                CloudService theMatchingCloudService = subscription.CloudServices.SingleOrDefault<CloudService>(cs => String.CompareOrdinal(cs.Name, cloudServiceName) == 0);

                if (theMatchingCloudService == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                ResourceOutput theMatchingResource = theMatchingCloudService.Resources.FirstOrDefault(r => String.Compare(r.Name, resourceName) == 0);

                if (theMatchingResource != null)
                {
                    //EnsureHerokuClient().Deprovision(name from resource);
                    theMatchingCloudService.Resources.Remove(theMatchingResource);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }

        private static OutputItemCollection GenerateOutputItems()
        {
            List<OutputItem> outputItems = new List<OutputItem>();

            // Add Well Known output values
            outputItems.Add(new OutputItem() { Key = "CacheUrl", Value = "" });
            return new OutputItemCollection(outputItems);
        }

        private static UsageMeterCollection GenerateUsageMeters()
        {
            UsageMeterCollection usageMeters = new UsageMeterCollection();

            // Meter 1
            {
                UsageMeter meter = new UsageMeter();

                meter.Name = "";
                meter.Included = "1024";
                meter.Unit = "some garbage";
                meter.Used = "1";

                usageMeters.Add(meter);
            }


            // Meter 2
            {
                UsageMeter meter = new UsageMeter();

                meter.Name = "Transactions";
                meter.Included = "1024";
                meter.Unit = "some garbage";
                meter.Used = "1";

                usageMeters.Add(meter);
            }


            // Meter 3
            {
                UsageMeter meter = new UsageMeter();

                meter.Name = "Transactions/months";
                meter.Included = "1024";
                meter.Unit = "transactions";
                meter.Used = "25";

                usageMeters.Add(meter);
            }

            // Meter 4
            {
                UsageMeter meter = new UsageMeter();

                meter.Name = "Compute Hours";
                meter.Included = "2";
                meter.Unit = "hours";
                meter.Used = "5";

                usageMeters.Add(meter);
            }

            // Meter 5
            {
                UsageMeter meter = new UsageMeter();

                meter.Name = "Storage";
                meter.Included = "1024";
                meter.Unit = "bytes";
                meter.Used = "256";

                usageMeters.Add(meter);
            }

            // Meter 6
            {
                UsageMeter meter = new UsageMeter();

                meter.Name = "NameUnlimited";
                meter.Included = "-1";
                meter.Unit = "generic";
                meter.Used = "256";

                usageMeters.Add(meter);
            }

            return usageMeters;
        }
    }
}